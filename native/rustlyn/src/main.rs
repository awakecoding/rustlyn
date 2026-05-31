use std::env;
use std::error::Error as StdError;
use std::ffi::{c_char, c_void, CStr, CString};
use std::fmt;
use std::fs;
use std::os::raw::c_uint;
use std::path::{Path, PathBuf};
use std::process::Command;
use std::ptr;
use std::slice;

use serde_json::Value;

type LlvmBool = i32;
type LlvmModuleRef = *mut std::ffi::c_void;
type LlvmMemoryBufferRef = *mut std::ffi::c_void;

const RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION: i32 = 1;
const RUSTLYN_HOST_CALLBACK_SCHEMA_VERSION: i32 = 1;

#[repr(C)]
struct RustlynHostCallbacks {
    schema_version: i32,
    print_ir: unsafe extern "C" fn(*const u8, usize, *mut *mut u8, *mut usize) -> i32,
    free: unsafe extern "C" fn(*mut u8),
}

extern "C" {
    fn LLVMGetVersion(major: *mut c_uint, minor: *mut c_uint, patch: *mut c_uint);
    fn LLVMCreateMemoryBufferWithContentsOfFile(
        path: *const c_char,
        out_mem_buf: *mut LlvmMemoryBufferRef,
        out_message: *mut *mut c_char,
    ) -> LlvmBool;
    fn LLVMDisposeMemoryBuffer(mem_buf: LlvmMemoryBufferRef);
    fn LLVMParseBitcode2(mem_buf: LlvmMemoryBufferRef, out_module: *mut LlvmModuleRef) -> LlvmBool;
    fn LLVMDisposeModule(module: LlvmModuleRef);
    fn LLVMPrintModuleToString(module: LlvmModuleRef) -> *mut c_char;
    fn LLVMDisposeMessage(message: *mut c_char);
}

type RegisterHostCallbacksFn =
    unsafe extern "C" fn(schema_version: i32, callbacks: *const RustlynHostCallbacks) -> i32;
type JsonCommandFn = unsafe extern "C" fn(
    options_json: *const u8,
    options_len: usize,
    result_json: *mut *mut u8,
    result_len: *mut usize,
) -> i32;
type FreeFn = unsafe extern "C" fn(ptr: *mut u8);

#[cfg(rustlyn_nativeaot_static)]
extern "C" {
    fn rustlyn_register_host_callbacks(
        schema_version: i32,
        callbacks: *const RustlynHostCallbacks,
    ) -> i32;
    fn rustlyn_emit(
        options_json: *const u8,
        options_len: usize,
        result_json: *mut *mut u8,
        result_len: *mut usize,
    ) -> i32;
    fn rustlyn_lower(
        options_json: *const u8,
        options_len: usize,
        result_json: *mut *mut u8,
        result_len: *mut usize,
    ) -> i32;
    fn rustlyn_pack(
        options_json: *const u8,
        options_len: usize,
        result_json: *mut *mut u8,
        result_len: *mut usize,
    ) -> i32;
    fn rustlyn_free(ptr: *mut u8);
}

#[cfg(not(rustlyn_nativeaot_static))]
struct NativeAot {
    register_host_callbacks: RegisterHostCallbacksFn,
    emit: JsonCommandFn,
    lower: JsonCommandFn,
    pack: JsonCommandFn,
    free: FreeFn,
    _module: *mut c_void,
}

#[cfg(not(rustlyn_nativeaot_static))]
unsafe impl Send for NativeAot {}

#[cfg(not(rustlyn_nativeaot_static))]
unsafe impl Sync for NativeAot {}

#[cfg(not(rustlyn_nativeaot_static))]
fn nativeaot() -> Result<&'static NativeAot, Error> {
    use std::sync::OnceLock;

    static NATIVEAOT: OnceLock<Result<NativeAot, String>> = OnceLock::new();
    NATIVEAOT
        .get_or_init(load_nativeaot)
        .as_ref()
        .map_err(|message| Error::new(1, message.clone()))
}

#[cfg(not(rustlyn_nativeaot_static))]
fn load_nativeaot() -> Result<NativeAot, String> {
    #[cfg(windows)]
    unsafe {
        load_nativeaot_windows()
    }

    #[cfg(not(windows))]
    {
        Err("shared NativeAOT loading is currently implemented for Windows hosts".to_string())
    }
}

#[cfg(windows)]
#[cfg(not(rustlyn_nativeaot_static))]
unsafe fn load_nativeaot_windows() -> Result<NativeAot, String> {
    use std::os::windows::ffi::OsStrExt;

    #[link(name = "kernel32")]
    extern "system" {
        fn LoadLibraryW(file_name: *const u16) -> *mut c_void;
        fn GetProcAddress(module: *mut c_void, proc_name: *const c_char) -> *mut c_void;
    }

    unsafe fn symbol(module: *mut c_void, name: &'static [u8]) -> Result<*mut c_void, String> {
        let ptr = GetProcAddress(module, name.as_ptr().cast::<c_char>());
        if ptr.is_null() {
            Err(format!(
                "rustlyn_nativeaot.dll does not export {}",
                String::from_utf8_lossy(&name[..name.len() - 1])
            ))
        } else {
            Ok(ptr)
        }
    }

    let exe =
        env::current_exe().map_err(|err| format!("failed to locate rustlyn executable: {err}"))?;
    let dll = exe
        .parent()
        .ok_or_else(|| format!("failed to locate parent directory for '{}'", exe.display()))?
        .join("rustlyn_nativeaot.dll");
    let mut dll_wide: Vec<u16> = dll.as_os_str().encode_wide().collect();
    dll_wide.push(0);

    let module = LoadLibraryW(dll_wide.as_ptr());
    if module.is_null() {
        return Err(format!("failed to load '{}'", dll.display()));
    }

    Ok(NativeAot {
        register_host_callbacks: std::mem::transmute::<*mut c_void, RegisterHostCallbacksFn>(
            symbol(module, b"rustlyn_register_host_callbacks\0")?,
        ),
        emit: std::mem::transmute::<*mut c_void, JsonCommandFn>(symbol(module, b"rustlyn_emit\0")?),
        lower: std::mem::transmute::<*mut c_void, JsonCommandFn>(symbol(
            module,
            b"rustlyn_lower\0",
        )?),
        pack: std::mem::transmute::<*mut c_void, JsonCommandFn>(symbol(module, b"rustlyn_pack\0")?),
        free: std::mem::transmute::<*mut c_void, FreeFn>(symbol(module, b"rustlyn_free\0")?),
        _module: module,
    })
}

fn nativeaot_register_host_callbacks(callbacks: *const RustlynHostCallbacks) -> Result<i32, Error> {
    #[cfg(rustlyn_nativeaot_static)]
    unsafe {
        return Ok(rustlyn_register_host_callbacks(
            RUSTLYN_HOST_CALLBACK_SCHEMA_VERSION,
            callbacks,
        ));
    }

    #[cfg(not(rustlyn_nativeaot_static))]
    unsafe {
        Ok((nativeaot()?.register_host_callbacks)(
            RUSTLYN_HOST_CALLBACK_SCHEMA_VERSION,
            callbacks,
        ))
    }
}

fn nativeaot_emit(
    options_json: &[u8],
    result_json: *mut *mut u8,
    result_len: *mut usize,
) -> Result<i32, Error> {
    #[cfg(rustlyn_nativeaot_static)]
    unsafe {
        return Ok(rustlyn_emit(
            options_json.as_ptr(),
            options_json.len(),
            result_json,
            result_len,
        ));
    }

    #[cfg(not(rustlyn_nativeaot_static))]
    unsafe {
        Ok((nativeaot()?.emit)(
            options_json.as_ptr(),
            options_json.len(),
            result_json,
            result_len,
        ))
    }
}

fn nativeaot_lower(
    options_json: &[u8],
    result_json: *mut *mut u8,
    result_len: *mut usize,
) -> Result<i32, Error> {
    #[cfg(rustlyn_nativeaot_static)]
    unsafe {
        return Ok(rustlyn_lower(
            options_json.as_ptr(),
            options_json.len(),
            result_json,
            result_len,
        ));
    }

    #[cfg(not(rustlyn_nativeaot_static))]
    unsafe {
        Ok((nativeaot()?.lower)(
            options_json.as_ptr(),
            options_json.len(),
            result_json,
            result_len,
        ))
    }
}

fn nativeaot_pack(
    options_json: &[u8],
    result_json: *mut *mut u8,
    result_len: *mut usize,
) -> Result<i32, Error> {
    #[cfg(rustlyn_nativeaot_static)]
    unsafe {
        return Ok(rustlyn_pack(
            options_json.as_ptr(),
            options_json.len(),
            result_json,
            result_len,
        ));
    }

    #[cfg(not(rustlyn_nativeaot_static))]
    unsafe {
        Ok((nativeaot()?.pack)(
            options_json.as_ptr(),
            options_json.len(),
            result_json,
            result_len,
        ))
    }
}

unsafe fn nativeaot_free(ptr: *mut u8) {
    #[cfg(rustlyn_nativeaot_static)]
    {
        rustlyn_free(ptr);
    }

    #[cfg(not(rustlyn_nativeaot_static))]
    {
        if let Ok(nativeaot) = nativeaot() {
            (nativeaot.free)(ptr);
        }
    }
}

fn register_host_callbacks() -> Result<(), Error> {
    let callbacks = RustlynHostCallbacks {
        schema_version: RUSTLYN_HOST_CALLBACK_SCHEMA_VERSION,
        print_ir: rustlyn_llvm_print_ir,
        free: rustlyn_llvm_free,
    };
    let code = nativeaot_register_host_callbacks(&callbacks)?;
    if code == 0 {
        Ok(())
    } else {
        Err(Error::new(
            2,
            format!("rustlyn_register_host_callbacks returned code {code}"),
        ))
    }
}

fn main() {
    if let Err(err) = run(env::args().skip(1).collect()) {
        if !err.message.is_empty() {
            eprintln!("{err}");
        }
        std::process::exit(err.exit_code());
    }
}

fn run(args: Vec<String>) -> Result<(), Error> {
    if args.is_empty() || is_help(&args[0]) {
        print_usage();
        return Ok(());
    }

    match args[0].as_str() {
        "--version" | "-V" => {
            print_version();
            Ok(())
        }
        "help" => {
            if let Some(command) = args.get(1) {
                if let Some(help) = command_help(command) {
                    println!("{help}");
                } else {
                    return Err(Error::new(2, format!("unknown help topic '{command}'")));
                }
            } else {
                print_usage();
            }
            Ok(())
        }
        "new" => run_new(&args[1..]),
        "rustc" => run_rustc(&args[1..]),
        "diagnose" => run_diagnose(&args[1..]),
        "inspect" => run_inspect(&args[1..]),
        "cargo" => run_cargo(&args[1..]),
        "translate" => run_translate(&args[1..]),
        "pack" => run_pack(&args[1..]),
        "lower" => run_lower(&args[1..]),
        "emit" => run_emit(&args[1..]),
        "llvm" => run_llvm(&args[1..]),
        command => Err(Error::new(
            2,
            format!(
                "unsupported rustlyn command '{command}'\n\n{}",
                command_status()
            ),
        )),
    }
}

fn run_emit(args: &[String]) -> Result<(), Error> {
    let options = EmitOptions::parse(args)?;
    register_host_callbacks()?;
    let options_json = options.to_emit_options_json();
    let mut result_ptr: *mut u8 = ptr::null_mut();
    let mut result_len: usize = 0;
    let code = nativeaot_emit(options_json.as_bytes(), &mut result_ptr, &mut result_len)?;

    let result_json = unsafe {
        let result = if result_ptr.is_null() || result_len == 0 {
            String::new()
        } else {
            let bytes = slice::from_raw_parts(result_ptr, result_len);
            String::from_utf8_lossy(bytes).into_owned()
        };
        nativeaot_free(result_ptr);
        result
    };

    if let Some(schema_version) = extract_json_int(&result_json, "schemaVersion") {
        if schema_version != RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION {
            return Err(Error::new(
                2,
                format!(
                    "rustlyn_emit returned unsupported ABI schema version {schema_version}; expected {RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION}"
                ),
            ));
        }
    }

    if code == 0 && result_json.contains("\"success\":true") {
        println!("{}", path_to_string(&options.output_path));
        return Ok(());
    }

    let exit_code =
        extract_json_int(&result_json, "exitCode").unwrap_or(if code == 0 { 1 } else { code });
    Err(Error::new(
        exit_code,
        if result_json.is_empty() {
            format!("rustlyn_emit returned code {code}")
        } else {
            result_json
        },
    ))
}

fn run_lower(args: &[String]) -> Result<(), Error> {
    if !args.is_empty() && is_help(&args[0]) {
        println!("{}", command_help("lower").unwrap());
        return Ok(());
    }

    let mut input = None;
    let mut index = 0;
    while index < args.len() {
        match args[index].as_str() {
            "--llvm-root" => {
                return Err(Error::new(
                    2,
                    "--llvm-root is not used by the native host; LLVM is linked in-process",
                ));
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unsupported lower option '{value}'")));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "lower accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    let input = input.ok_or_else(|| Error::new(2, command_help("lower").unwrap()))?;
    register_host_callbacks()?;
    let request_json = lower_options_json(&input);
    let mut result_ptr: *mut u8 = ptr::null_mut();
    let mut result_len: usize = 0;
    let code = nativeaot_lower(request_json.as_bytes(), &mut result_ptr, &mut result_len)?;

    let result_json = unsafe {
        let result = if result_ptr.is_null() || result_len == 0 {
            String::new()
        } else {
            let bytes = slice::from_raw_parts(result_ptr, result_len);
            String::from_utf8_lossy(bytes).into_owned()
        };
        nativeaot_free(result_ptr);
        result
    };

    if !result_json.is_empty() {
        let value: Value = serde_json::from_str(&result_json).map_err(|err| {
            Error::new(1, format!("failed to parse rustlyn_lower response: {err}"))
        })?;
        if value.get("schemaVersion").and_then(Value::as_i64)
            != Some(RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION as i64)
        {
            return Err(Error::new(
                2,
                "rustlyn_lower returned an unsupported ABI schema version",
            ));
        }
        if code == 0 && value.get("success").and_then(Value::as_bool) == Some(true) {
            if let Some(lowered_ir) = value.get("loweredIr").and_then(Value::as_str) {
                print!("{lowered_ir}");
                return Ok(());
            }
        }
    }

    Err(Error::new(
        extract_json_int(&result_json, "exitCode").unwrap_or(if code == 0 { 1 } else { code }),
        if result_json.is_empty() {
            format!("rustlyn_lower returned code {code}")
        } else {
            result_json
        },
    ))
}

fn run_llvm(args: &[String]) -> Result<(), Error> {
    if args.is_empty() || is_help(&args[0]) {
        println!("Usage: rustlyn llvm print-ir <path-to-bc> [--output <path>|-o <path>]");
        return Ok(());
    }

    match args[0].as_str() {
        "print-ir" => run_llvm_print_ir(&args[1..]),
        "--version" | "-V" => {
            let (major, minor, patch) = probe_llvm_version();
            println!("LLVM {major}.{minor}.{patch}");
            Ok(())
        }
        command => Err(Error::new(
            2,
            format!("unsupported rustlyn llvm command '{command}'"),
        )),
    }
}

fn run_llvm_print_ir(args: &[String]) -> Result<(), Error> {
    let mut input = None;
    let mut output = PathOrStdout::Stdout;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--output" | "-o" => {
                let value = args
                    .get(index + 1)
                    .ok_or_else(|| Error::new(2, "missing value for --output"))?;
                output = PathOrStdout::Path(PathBuf::from(value));
                index += 2;
            }
            value if value.starts_with('-') => {
                return Err(Error::new(
                    2,
                    format!("unsupported print-ir option '{value}'"),
                ));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "print-ir accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    let input = input.ok_or_else(|| Error::new(2, "print-ir requires an input bitcode path"))?;
    let ir = print_ir(&input)?;
    match output {
        PathOrStdout::Stdout => {
            print!("{ir}");
        }
        PathOrStdout::Path(path) => {
            fs::write(&path, ir).map_err(|err| {
                Error::new(1, format!("failed to write '{}': {err}", path.display()))
            })?;
        }
    }

    Ok(())
}

fn print_ir(path: &Path) -> Result<String, Error> {
    let path_bytes = path_to_string(path);
    let mut result_ptr: *mut u8 = ptr::null_mut();
    let mut result_len: usize = 0;
    let code = unsafe {
        rustlyn_llvm_print_ir(
            path_bytes.as_ptr(),
            path_bytes.len(),
            &mut result_ptr,
            &mut result_len,
        )
    };

    let result = unsafe {
        let result = if result_ptr.is_null() || result_len == 0 {
            String::new()
        } else {
            let bytes = slice::from_raw_parts(result_ptr, result_len);
            String::from_utf8_lossy(bytes).into_owned()
        };
        rustlyn_llvm_free(result_ptr);
        result
    };

    if code == 0 {
        Ok(result)
    } else {
        Err(Error::new(code, result))
    }
}

#[no_mangle]
pub unsafe extern "C" fn rustlyn_llvm_print_ir(
    path_utf8: *const u8,
    path_len: usize,
    result_utf8: *mut *mut u8,
    result_len: *mut usize,
) -> i32 {
    if path_utf8.is_null() || path_len == 0 || result_utf8.is_null() || result_len.is_null() {
        return -1;
    }

    *result_utf8 = ptr::null_mut();
    *result_len = 0;

    let path_bytes = slice::from_raw_parts(path_utf8, path_len);
    let path = match CString::new(path_bytes) {
        Ok(path) => path,
        Err(_) => {
            return write_llvm_result(
                result_utf8,
                result_len,
                "input path contains a null byte",
                -2,
            )
        }
    };

    let mut load_error: *mut c_char = ptr::null_mut();
    let mut memory_buffer: LlvmMemoryBufferRef = ptr::null_mut();
    if LLVMCreateMemoryBufferWithContentsOfFile(path.as_ptr(), &mut memory_buffer, &mut load_error)
        != 0
    {
        let message = take_llvm_message(load_error)
            .unwrap_or_else(|| "failed to create LLVM memory buffer".to_string());
        return write_llvm_result(result_utf8, result_len, &message, 1);
    }

    let mut module: LlvmModuleRef = ptr::null_mut();
    let parse_result = LLVMParseBitcode2(memory_buffer, &mut module);
    LLVMDisposeMemoryBuffer(memory_buffer);
    if parse_result != 0 {
        return write_llvm_result(
            result_utf8,
            result_len,
            "failed to parse LLVM bitcode module",
            2,
        );
    }

    let ir = LLVMPrintModuleToString(module);
    LLVMDisposeModule(module);
    let Some(ir) = take_llvm_message(ir) else {
        return write_llvm_result(result_utf8, result_len, "failed to print LLVM module", 3);
    };

    write_llvm_result(result_utf8, result_len, &ir, 0)
}

#[no_mangle]
pub unsafe extern "C" fn rustlyn_llvm_free(ptr: *mut u8) {
    if !ptr.is_null() {
        let _ = CString::from_raw(ptr.cast::<c_char>());
    }
}

unsafe fn take_llvm_message(message: *mut c_char) -> Option<String> {
    if message.is_null() {
        return None;
    }

    let value = CStr::from_ptr(message).to_string_lossy().into_owned();
    LLVMDisposeMessage(message);
    Some(value)
}

unsafe fn write_llvm_result(
    result_utf8: *mut *mut u8,
    result_len: *mut usize,
    value: &str,
    code: i32,
) -> i32 {
    let c_string = match CString::new(value) {
        Ok(value) => value,
        Err(_) => {
            CString::new("LLVM result contains a null byte").expect("static message is valid")
        }
    };
    *result_len = c_string.as_bytes().len();
    *result_utf8 = c_string.into_raw().cast::<u8>();
    code
}

fn print_version() {
    let (major, minor, patch) = probe_llvm_version();
    println!("rustlyn native-host {}", env!("CARGO_PKG_VERSION"));
    println!("linked LLVM {major}.{minor}.{patch}");
}

fn probe_llvm_version() -> (c_uint, c_uint, c_uint) {
    let mut major = 0;
    let mut minor = 0;
    let mut patch = 0;
    unsafe {
        LLVMGetVersion(&mut major, &mut minor, &mut patch);
    }
    (major, minor, patch)
}

fn print_usage() {
    println!("rustlyn - Rust -> .NET translation toolchain");
    println!();
    println!("Usage: rustlyn <command> [options]");
    println!();
    println!("Implemented in the native host:");
    println!("  new        Scaffold a new sample crate");
    println!("  rustc      Build single-file Rust source into bitcode (+ LLVM IR)");
    println!("  cargo      Build a Cargo crate to bitcode + managed assembly");
    println!("  translate  Build a Cargo crate to a managed assembly");
    println!("  pack       Translate and emit a .nuspec + .nupkg");
    println!("  emit       Lower a .bc into a managed .dll");
    println!("  lower      Print the lowered IR for a .bc");
    println!("  inspect    Print metadata for a bitcode artifact");
    println!("  llvm       Run in-process LLVM commands");
    println!("  diagnose   Check toolchain and linked native components");
    println!("  help       Show this banner");
    println!("  --version  Show native host and linked LLVM versions");
    println!();
    println!("{}", command_status());
}

fn command_status() -> &'static str {
    "Pending parity commands: run, invoke."
}

fn command_help(command: &str) -> Option<&'static str> {
    match command {
        "new" => Some("Usage: rustlyn new <name> [--lib|--bin] [--in <dir>] [--edition <year>]\n\nScaffolds <dir>/<name>/Cargo.toml + src/lib.rs (or src/main.rs for --bin).\nDefaults: --lib, edition 2024. --in defaults to ./samples if present, else cwd."),
        "rustc" => Some("Usage: rustlyn rustc <source.rs> [--out-dir <dir>] [--crate-name <name>] [--crate-type <lib|bin>] [--edition <year>] [--emit <bc|ll|bc,ll>] [--panic <abort|unwind>] [--overflow-checks <on|off>]\n\nWraps rustc with rustlyn's standardized flags (panic=abort, overflow-checks=off, edition=2021) and emits .bc/.ll in a single invocation."),
        "cargo" => Some("Usage: rustlyn cargo [+<toolchain>] build [--manifest-path <Cargo.toml>] [--release] [--bin <name>] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--strict]\n\nBuilds a Cargo crate to bitcode + managed assembly, writing into Cargo's normal target/<profile>/ directory."),
        "translate" => Some("Usage: rustlyn translate <crate-path> --out <path-to-dll> [--bitcode-out <path-to-bc>] [--bin <name>] [--debug] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--strict]\n\nBuilds a Cargo crate and emits a managed assembly at the specified path."),
        "pack" => Some("Usage: rustlyn pack <crate-path> --out <dir> [--version <semver>] [--bin <name>] [--debug] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--strict]\n\nTranslates a crate and produces a .nuspec + .nupkg in the output directory."),
        "emit" => Some("Usage: rustlyn emit <path-to-bc> --out <path-to-dll> [--pdb] [--strict]\n\nLowers an existing .bc into a .NET assembly using the in-process LLVM/NativeAOT backend."),
        "lower" => Some("Usage: rustlyn lower <path-to-bc>\n\nPrints the lowered IR dump for a bitcode artifact using the in-process LLVM/NativeAOT backend."),
        "inspect" => Some("Usage: rustlyn inspect <path-to-bc>\n\nPrints bitcode metadata, functions, and globals using the LLVM library linked into this executable."),
        "llvm" => Some("Usage: rustlyn llvm print-ir <path-to-bc> [--output <path>|-o <path>]\n       rustlyn llvm --version\n\nRuns LLVM operations linked into this rustlyn executable."),
        "diagnose" => Some("Usage: rustlyn diagnose\n\nChecks cargo, rustc, optional nightly/rust-src, dotnet, and linked LLVM/NativeAOT state."),
        _ => None,
    }
}

fn is_help(value: &str) -> bool {
    value == "--help" || value == "-h" || value == "/?"
}

fn run_cargo(args: &[String]) -> Result<(), Error> {
    let (build_args, strict) = CargoBuildOptions::parse_cargo(args)?;
    let result = build_cargo_project(&build_args)?;
    emit_bitcode(&result.bitcode_path, &result.assembly_path, false, strict)?;
    println!("Bitcode: {}", result.bitcode_path.display());
    println!("Assembly: {}", result.assembly_path.display());
    println!(
        "PDB: {}",
        result.assembly_path.with_extension("pdb").display()
    );
    Ok(())
}

fn run_translate(args: &[String]) -> Result<(), Error> {
    let (build_args, output_path, strict) = CargoBuildOptions::parse_translate(args)?;
    let result = build_bitcode(&build_args)?;
    emit_bitcode(&result, &output_path, false, strict)?;
    println!("Bitcode: {}", result.display());
    println!("Assembly: {}", output_path.display());
    Ok(())
}

fn run_pack(args: &[String]) -> Result<(), Error> {
    let (build_args, output_dir, version, strict) = CargoBuildOptions::parse_pack(args)?;
    let manifest_path = resolve_manifest_path(&build_args.crate_path)?;
    let metadata = read_cargo_metadata(&manifest_path, &build_args)?;
    let output_dir = full_path(&output_dir)?;
    fs::create_dir_all(&output_dir).map_err(|err| {
        Error::new(
            1,
            format!("failed to create '{}': {err}", output_dir.display()),
        )
    })?;

    let bitcode_path = build_bitcode(&build_args)?;
    let assembly_path = output_dir.join(format!(
        "{}.dll",
        normalize_cargo_artifact_name(&metadata.target_name)
    ));
    emit_bitcode(&bitcode_path, &assembly_path, true, strict)?;
    let result = pack_assembly(&metadata.target_name, &version, &assembly_path, &output_dir)?;

    println!(
        "Package ID: {}",
        result
            .get("packageId")
            .and_then(Value::as_str)
            .unwrap_or("<unknown>")
    );
    println!(
        "Version: {}",
        result
            .get("version")
            .and_then(Value::as_str)
            .unwrap_or(version.as_str())
    );
    println!(
        "Assembly: {}",
        result
            .get("assemblyPath")
            .and_then(Value::as_str)
            .unwrap_or_else(|| assembly_path.to_str().unwrap_or("<unknown>"))
    );
    println!(
        "Nuspec: {}",
        result
            .get("nuspecPath")
            .and_then(Value::as_str)
            .unwrap_or("<unknown>")
    );
    println!(
        "Nupkg: {}",
        result
            .get("nupkgPath")
            .and_then(Value::as_str)
            .unwrap_or("<unknown>")
    );
    println!(
        "Files: {}",
        result.get("files").and_then(Value::as_i64).unwrap_or(0)
    );
    Ok(())
}

fn pack_assembly(
    crate_name: &str,
    version: &str,
    assembly_path: &Path,
    output_dir: &Path,
) -> Result<Value, Error> {
    register_host_callbacks()?;
    let options_json = pack_options_json(crate_name, version, assembly_path, output_dir);
    let mut result_ptr: *mut u8 = ptr::null_mut();
    let mut result_len: usize = 0;
    let code = nativeaot_pack(options_json.as_bytes(), &mut result_ptr, &mut result_len)?;

    let result_json = unsafe {
        let result = if result_ptr.is_null() || result_len == 0 {
            String::new()
        } else {
            let bytes = slice::from_raw_parts(result_ptr, result_len);
            String::from_utf8_lossy(bytes).into_owned()
        };
        nativeaot_free(result_ptr);
        result
    };

    if result_json.is_empty() {
        return Err(Error::new(
            code,
            format!("rustlyn_pack returned code {code}"),
        ));
    }

    let value: Value = serde_json::from_str(&result_json)
        .map_err(|err| Error::new(1, format!("failed to parse rustlyn_pack response: {err}")))?;
    if value.get("schemaVersion").and_then(Value::as_i64)
        != Some(RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION as i64)
    {
        return Err(Error::new(
            2,
            "rustlyn_pack returned an unsupported ABI schema version",
        ));
    }
    if code == 0 && value.get("success").and_then(Value::as_bool) == Some(true) {
        Ok(value)
    } else {
        Err(Error::new(
            value
                .get("exitCode")
                .and_then(Value::as_i64)
                .map(|value| value as i32)
                .unwrap_or(if code == 0 { 1 } else { code }),
            result_json,
        ))
    }
}

fn emit_bitcode(
    input_path: &Path,
    output_path: &Path,
    emit_pdb: bool,
    strict: bool,
) -> Result<(), Error> {
    register_host_callbacks()?;
    let options = EmitOptions {
        input_path: input_path.to_path_buf(),
        output_path: output_path.to_path_buf(),
        emit_pdb,
        strict,
    };
    let options_json = options.to_emit_options_json();
    let mut result_ptr: *mut u8 = ptr::null_mut();
    let mut result_len: usize = 0;
    let code = nativeaot_emit(options_json.as_bytes(), &mut result_ptr, &mut result_len)?;

    let result_json = unsafe {
        let result = if result_ptr.is_null() || result_len == 0 {
            String::new()
        } else {
            let bytes = slice::from_raw_parts(result_ptr, result_len);
            String::from_utf8_lossy(bytes).into_owned()
        };
        nativeaot_free(result_ptr);
        result
    };

    if code == 0 && result_json.contains("\"success\":true") {
        return Ok(());
    }

    Err(Error::new(
        extract_json_int(&result_json, "exitCode").unwrap_or(if code == 0 { 1 } else { code }),
        if result_json.is_empty() {
            format!("rustlyn_emit returned code {code}")
        } else {
            result_json
        },
    ))
}

fn run_new(args: &[String]) -> Result<(), Error> {
    if !args.is_empty() && is_help(&args[0]) {
        println!("{}", command_help("new").unwrap());
        return Ok(());
    }
    if args.is_empty() {
        return Err(Error::new(2, command_help("new").unwrap()));
    }

    let mut name = None;
    let mut target_dir = None;
    let mut edition = "2024".to_string();
    let mut is_binary = false;
    let mut explicit_lib = false;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--bin" => {
                is_binary = true;
                index += 1;
            }
            "--lib" => {
                explicit_lib = true;
                index += 1;
            }
            "--in" => {
                target_dir = Some(next_arg(args, &mut index, "--in")?.into());
            }
            "--edition" => {
                edition = next_arg(args, &mut index, "--edition")?.to_string();
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unknown new option '{value}'")));
            }
            value => {
                if name.replace(value.to_string()).is_some() {
                    return Err(Error::new(2, "rustlyn new accepts exactly one crate name"));
                }
                index += 1;
            }
        }
    }

    if is_binary && explicit_lib {
        return Err(Error::new(2, "--lib and --bin are mutually exclusive"));
    }

    let name = name.ok_or_else(|| Error::new(2, "rustlyn new requires a crate name"))?;
    if name
        .chars()
        .any(|ch| !(ch.is_ascii_alphanumeric() || ch == '_' || ch == '-'))
    {
        return Err(Error::new(
            2,
            format!("invalid crate name '{name}'; use letters, digits, '_' or '-'"),
        ));
    }

    let target_dir = target_dir.unwrap_or_else(|| {
        let samples = env::current_dir()
            .unwrap_or_else(|_| PathBuf::from("."))
            .join("samples");
        if samples.is_dir() {
            samples
        } else {
            env::current_dir().unwrap_or_else(|_| PathBuf::from("."))
        }
    });

    let crate_dir = target_dir.join(&name);
    if crate_dir.is_dir()
        && crate_dir
            .read_dir()
            .map_err(|err| {
                Error::new(
                    1,
                    format!("failed to read '{}': {err}", crate_dir.display()),
                )
            })?
            .next()
            .is_some()
    {
        return Err(Error::new(
            2,
            format!(
                "target directory already exists and is not empty: {}",
                crate_dir.display()
            ),
        ));
    }

    let src_dir = crate_dir.join("src");
    fs::create_dir_all(&src_dir).map_err(|err| {
        Error::new(
            1,
            format!("failed to create '{}': {err}", src_dir.display()),
        )
    })?;

    let mut cargo_toml = String::new();
    cargo_toml.push_str("[package]\n");
    cargo_toml.push_str(&format!("name = \"{name}\"\n"));
    cargo_toml.push_str("version = \"0.1.0\"\n");
    cargo_toml.push_str(&format!("edition = \"{edition}\"\n\n"));
    if is_binary {
        cargo_toml.push_str("[[bin]]\n");
        cargo_toml.push_str(&format!("name = \"{name}\"\n"));
        cargo_toml.push_str("path = \"src/main.rs\"\n");
    } else {
        cargo_toml.push_str("[lib]\ncrate-type = [\"lib\"]\n");
    }

    let cargo_toml_path = crate_dir.join("Cargo.toml");
    fs::write(&cargo_toml_path, cargo_toml).map_err(|err| {
        Error::new(
            1,
            format!("failed to write '{}': {err}", cargo_toml_path.display()),
        )
    })?;

    let source_path = if is_binary {
        let source_path = src_dir.join("main.rs");
        fs::write(
            &source_path,
            format!("fn main() {{\n    println!(\"hello from {name}\");\n}}\n"),
        )
        .map_err(|err| {
            Error::new(
                1,
                format!("failed to write '{}': {err}", source_path.display()),
            )
        })?;
        source_path
    } else {
        let source_path = src_dir.join("lib.rs");
        let safe_name = name.replace('-', "_");
        let stub = format!(
            "#[unsafe(no_mangle)]\npub extern \"C\" fn {safe_name}_answer() -> i32 {{\n    42\n}}\n"
        );
        fs::write(&source_path, stub).map_err(|err| {
            Error::new(
                1,
                format!("failed to write '{}': {err}", source_path.display()),
            )
        })?;
        source_path
    };

    println!(
        "Created {} crate '{name}' at:",
        if is_binary { "binary" } else { "library" }
    );
    println!("  {}", crate_dir.display());
    println!(
        "  {}",
        cargo_toml_path
            .strip_prefix(&crate_dir)
            .unwrap_or(&cargo_toml_path)
            .display()
    );
    println!(
        "  {}",
        source_path
            .strip_prefix(&crate_dir)
            .unwrap_or(&source_path)
            .display()
    );
    println!();
    println!(
        "Next: rustlyn cargo build --manifest-path {}",
        cargo_toml_path.display()
    );
    Ok(())
}

fn run_rustc(args: &[String]) -> Result<(), Error> {
    if !args.is_empty() && is_help(&args[0]) {
        println!("{}", command_help("rustc").unwrap());
        return Ok(());
    }
    if args.is_empty() {
        return Err(Error::new(2, command_help("rustc").unwrap()));
    }

    let mut source_path = None;
    let mut out_dir = None;
    let mut crate_name = None;
    let mut crate_type = "lib".to_string();
    let mut edition = "2021".to_string();
    let mut emit = "bc,ll".to_string();
    let mut panic = "abort".to_string();
    let mut overflow_checks = "off".to_string();
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--out-dir" => out_dir = Some(PathBuf::from(next_arg(args, &mut index, "--out-dir")?)),
            "--crate-name" => {
                crate_name = Some(next_arg(args, &mut index, "--crate-name")?.to_string())
            }
            "--crate-type" => crate_type = next_arg(args, &mut index, "--crate-type")?.to_string(),
            "--edition" => edition = next_arg(args, &mut index, "--edition")?.to_string(),
            "--emit" => emit = next_arg(args, &mut index, "--emit")?.to_string(),
            "--panic" => panic = next_arg(args, &mut index, "--panic")?.to_string(),
            "--overflow-checks" => {
                overflow_checks = next_arg(args, &mut index, "--overflow-checks")?.to_string()
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unknown rustc option '{value}'")));
            }
            value => {
                if source_path.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(
                        2,
                        "rustlyn rustc accepts exactly one source path",
                    ));
                }
                index += 1;
            }
        }
    }

    let source_path =
        source_path.ok_or_else(|| Error::new(2, "rustlyn rustc requires a source file path"))?;
    let source_path = full_path(&source_path)?;
    if !source_path.is_file() {
        return Err(Error::new(
            2,
            format!("source file not found: {}", source_path.display()),
        ));
    }

    let crate_name = crate_name.unwrap_or_else(|| {
        source_path
            .file_stem()
            .and_then(|value| value.to_str())
            .unwrap_or("rustlyn_source")
            .to_string()
    });
    let out_dir = full_path(&out_dir.unwrap_or_else(|| {
        source_path
            .parent()
            .map(Path::to_path_buf)
            .unwrap_or_else(|| PathBuf::from("."))
    }))?;
    fs::create_dir_all(&out_dir).map_err(|err| {
        Error::new(
            1,
            format!("failed to create '{}': {err}", out_dir.display()),
        )
    })?;

    let emit_kinds = parse_emit_kinds(&emit)?;
    let status = Command::new("rustc")
        .arg(&source_path)
        .arg("--crate-name")
        .arg(&crate_name)
        .arg("--crate-type")
        .arg(&crate_type)
        .arg("--edition")
        .arg(&edition)
        .arg("-C")
        .arg(format!("overflow-checks={overflow_checks}"))
        .arg("-C")
        .arg(format!("panic={panic}"))
        .arg("--emit")
        .arg(emit_kinds.join(","))
        .arg("--out-dir")
        .arg(&out_dir)
        .status()
        .map_err(|err| Error::new(2, format!("rustc not found on PATH: {err}")))?;

    if !status.success() {
        return Err(Error::new(status.code().unwrap_or(1), ""));
    }

    for kind in emit_kinds {
        let extension = if kind == "llvm-bc" { "bc" } else { "ll" };
        let produced = out_dir.join(format!("{crate_name}.{extension}"));
        if !produced.is_file() {
            return Err(Error::new(
                3,
                format!(
                    "expected rustc output was not produced: {}",
                    produced.display()
                ),
            ));
        }
        println!("{}", produced.display());
    }

    Ok(())
}

fn run_diagnose(args: &[String]) -> Result<(), Error> {
    if !args.is_empty() && is_help(&args[0]) {
        println!("{}", command_help("diagnose").unwrap());
        return Ok(());
    }
    if !args.is_empty() {
        return Err(Error::new(
            2,
            "rustlyn diagnose does not take options in the native host",
        ));
    }

    let mut all_good = true;
    all_good &= print_tool_version("cargo", &["--version"], true);
    all_good &= print_tool_version("rustc", &["--version"], true);
    let nightly_available = print_tool_version("nightly", &["+nightly", "--version"], false);
    if nightly_available {
        print_rust_src_status();
    }

    let (major, minor, patch) = probe_llvm_version();
    println!("linked-llvm: {major}.{minor}.{patch}");
    println!("nativeaot: linked static rustlyn backend");
    all_good &= print_tool_version("dotnet", &["--version"], true);

    println!();
    if all_good {
        println!("Environment OK: all required tools are available.");
        Ok(())
    } else {
        Err(Error::new(
            2,
            "Environment INCOMPLETE: one or more required tools are missing.",
        ))
    }
}

fn run_inspect(args: &[String]) -> Result<(), Error> {
    if !args.is_empty() && is_help(&args[0]) {
        println!("{}", command_help("inspect").unwrap());
        return Ok(());
    }
    if args.is_empty() {
        return Err(Error::new(2, command_help("inspect").unwrap()));
    }

    let mut input = None;
    let mut index = 0;
    while index < args.len() {
        match args[index].as_str() {
            "--llvm-root" => {
                return Err(Error::new(
                    2,
                    "--llvm-root is not used by the native host; LLVM is linked in-process",
                ));
            }
            value if value.starts_with('-') => {
                return Err(Error::new(
                    2,
                    format!("unsupported inspect option '{value}'"),
                ));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "inspect accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    let input = input.ok_or_else(|| Error::new(2, "inspect requires an input bitcode path"))?;
    let full_path = full_path(&input)?;
    let display_path = fs::canonicalize(&full_path).unwrap_or_else(|_| full_path.clone());
    let metadata = fs::metadata(&full_path).map_err(|err| {
        Error::new(
            2,
            format!("failed to read '{}': {err}", full_path.display()),
        )
    })?;
    let bytes = fs::read(&full_path).map_err(|err| {
        Error::new(
            2,
            format!("failed to read '{}': {err}", full_path.display()),
        )
    })?;
    if bytes.len() < 4 {
        return Err(Error::new(
            2,
            format!(
                "expected at least 4 bytes in bitcode artifact '{}', but only read {}",
                full_path.display(),
                bytes.len()
            ),
        ));
    }

    let magic = &bytes[..4];
    let looks_like_bitcode = magic == [0x42, 0x43, 0xC0, 0xDE] || magic == [0xDE, 0xC0, 0x17, 0x0B];
    println!("Path: {}", display_path_string(&display_path));
    println!("Length: {} bytes", metadata.len());
    println!(
        "MagicBytes: {:02X}{:02X}{:02X}{:02X}",
        magic[0], magic[1], magic[2], magic[3]
    );
    println!("LooksLikeLlvmBitcode: {looks_like_bitcode}");
    if let Ok(modified) = metadata.modified() {
        if let Ok(duration) = modified.duration_since(std::time::UNIX_EPOCH) {
            println!(
                "LastWriteTimeUtc: {}.{:09}Z",
                duration.as_secs(),
                duration.subsec_nanos()
            );
        }
    }

    if !looks_like_bitcode {
        return Err(Error::new(2, ""));
    }

    let summary = inspect_module(&full_path)?;
    println!("LlvmRoot: linked-in-process");
    println!("ReaderKind: native-host-llvm-c-api");
    println!("FunctionCount: {}", summary.functions.len());
    println!("AliasCount: {}", summary.aliases.len());
    println!("GlobalCount: {}", summary.globals.len());
    println!("BasicBlockCount: {}", summary.basic_block_count());
    println!("InstructionCount: {}", summary.instruction_count());
    for function in summary.functions {
        println!(
            "Function: {} blocks={} instructions={}",
            function.name, function.basic_block_count, function.instruction_count
        );
    }
    for alias in summary.aliases {
        println!("Alias: {}", alias.name);
    }
    for global in summary.globals {
        println!("Global: {}", global.name);
    }

    Ok(())
}

fn inspect_module(path: &Path) -> Result<ModuleSummary, Error> {
    let path_bytes = path_to_string(path);
    let path = CString::new(path_bytes.as_bytes())
        .map_err(|_| Error::new(2, "input path contains a null byte"))?;
    unsafe {
        let mut load_error: *mut c_char = ptr::null_mut();
        let mut memory_buffer: LlvmMemoryBufferRef = ptr::null_mut();
        if LLVMCreateMemoryBufferWithContentsOfFile(
            path.as_ptr(),
            &mut memory_buffer,
            &mut load_error,
        ) != 0
        {
            let message = take_llvm_message(load_error)
                .unwrap_or_else(|| "failed to create LLVM memory buffer".to_string());
            return Err(Error::new(1, message));
        }

        let mut module: LlvmModuleRef = ptr::null_mut();
        let parse_result = LLVMParseBitcode2(memory_buffer, &mut module);
        LLVMDisposeMemoryBuffer(memory_buffer);
        if parse_result != 0 {
            return Err(Error::new(2, "failed to parse LLVM bitcode module"));
        }

        let ir = LLVMPrintModuleToString(module);
        LLVMDisposeModule(module);
        let Some(ir) = take_llvm_message(ir) else {
            return Err(Error::new(1, "failed to print LLVM module"));
        };
        let summary = parse_module_summary(&ir);
        Ok(summary)
    }
}

fn parse_module_summary(ir: &str) -> ModuleSummary {
    let mut functions = Vec::new();
    let mut aliases = Vec::new();
    let mut globals = Vec::new();
    let mut current_function: Option<FunctionSummary> = None;

    for raw_line in ir.lines() {
        let line = raw_line.trim();
        if line.is_empty() || line.starts_with(';') {
            continue;
        }

        if let Some(function) = current_function.as_mut() {
            if line == "}" {
                functions.push(current_function.take().expect("function is active"));
            } else if is_basic_block_label(line) {
                function.basic_block_count += 1;
            } else if is_instruction_line(line) {
                function.instruction_count += 1;
            }

            continue;
        }

        if let Some(name) = parse_function_name(line) {
            current_function = Some(FunctionSummary {
                name,
                basic_block_count: 0,
                instruction_count: 0,
            });
            continue;
        }

        if let Some(name) = parse_alias_name(line) {
            aliases.push(AliasSummary { name });
            continue;
        }

        if let Some(name) = parse_global_name(line) {
            globals.push(GlobalSummary { name });
        }
    }

    if let Some(function) = current_function {
        functions.push(function);
    }

    ModuleSummary {
        functions,
        aliases,
        globals,
    }
}

fn parse_function_name(line: &str) -> Option<String> {
    if !line.starts_with("define ") {
        return None;
    }

    parse_symbol_after_at(line)
}

fn parse_alias_name(line: &str) -> Option<String> {
    if !line.starts_with('@') || !line.contains(" alias ") {
        return None;
    }

    parse_symbol_name(line)
}

fn parse_global_name(line: &str) -> Option<String> {
    if !line.starts_with('@') || !line.contains(" = ") {
        return None;
    }

    parse_symbol_name(line)
}

fn parse_symbol_after_at(line: &str) -> Option<String> {
    let start = line.find('@')?;
    parse_symbol_name(&line[start..])
}

fn parse_symbol_name(line: &str) -> Option<String> {
    if !line.starts_with('@') {
        return None;
    }

    let name = line[1..]
        .split(|ch: char| ch == '(' || ch == ' ' || ch == '\t' || ch == '=' || ch == ',')
        .next()?;
    if name.is_empty() {
        None
    } else {
        Some(name.to_string())
    }
}

fn is_basic_block_label(line: &str) -> bool {
    let Some((label, _)) = line.split_once(':') else {
        return false;
    };

    let mut chars = label.chars();
    let Some(first) = chars.next() else {
        return false;
    };

    matches!(first, 'A'..='Z' | 'a'..='z' | '$' | '.' | '_')
        && chars.all(|ch| matches!(ch, 'A'..='Z' | 'a'..='z' | '0'..='9' | '-' | '$' | '.' | '_'))
}

fn is_instruction_line(line: &str) -> bool {
    if line.starts_with("attributes ") || line.starts_with("declare ") {
        return false;
    }

    line.contains(" = ")
        || line.starts_with("ret ")
        || line.starts_with("br ")
        || line.starts_with("switch ")
        || line.starts_with("invoke ")
        || line.starts_with("call ")
        || line.starts_with("unreachable")
        || line.starts_with("resume ")
}

fn next_arg<'a>(args: &'a [String], index: &mut usize, name: &str) -> Result<&'a str, Error> {
    *index += 1;
    let value = args
        .get(*index)
        .ok_or_else(|| Error::new(2, format!("{name} requires a value")))?;
    *index += 1;
    Ok(value)
}

fn full_path(path: &Path) -> Result<PathBuf, Error> {
    if path.is_absolute() {
        Ok(path.to_path_buf())
    } else {
        Ok(env::current_dir()
            .map_err(|err| Error::new(1, format!("failed to read current directory: {err}")))?
            .join(path))
    }
}

fn parse_emit_kinds(emit: &str) -> Result<Vec<&'static str>, Error> {
    let mut emit_kinds = Vec::new();
    for part in emit
        .split(',')
        .map(str::trim)
        .filter(|part| !part.is_empty())
    {
        let normalized = match part {
            "bc" | "llvm-bc" => "llvm-bc",
            "ll" | "llvm-ir" => "llvm-ir",
            _ => {
                return Err(Error::new(
                    2,
                    format!("unsupported --emit kind: {part} (expected bc, ll, or bc,ll)"),
                ))
            }
        };
        if !emit_kinds.contains(&normalized) {
            emit_kinds.push(normalized);
        }
    }

    if emit_kinds.is_empty() {
        Err(Error::new(2, "--emit must include bc, ll, or bc,ll"))
    } else {
        Ok(emit_kinds)
    }
}

fn print_tool_version(label: &str, args: &[&str], required: bool) -> bool {
    print!("{label}: ");
    match run_capture(if label == "nightly" { "rustc" } else { label }, args) {
        Some(output) => {
            println!("{}", output.trim());
            true
        }
        None => {
            if required {
                println!("NOT FOUND");
            } else {
                println!("not available");
            }
            false
        }
    }
}

fn print_rust_src_status() {
    print!("rust-src: ");
    let Some(sysroot) = run_capture("rustc", &["+nightly", "--print", "sysroot"]) else {
        println!("could not determine sysroot");
        return;
    };

    let rust_source = PathBuf::from(sysroot.trim())
        .join("lib")
        .join("rustlib")
        .join("src")
        .join("rust")
        .join("library");
    if rust_source.is_dir() {
        println!("installed");
    } else {
        println!("NOT FOUND (install: rustup component add rust-src --toolchain nightly)");
    }
}

fn run_capture(file_name: &str, args: &[&str]) -> Option<String> {
    let output = Command::new(file_name).args(args).output().ok()?;
    if output.status.success() {
        Some(String::from_utf8_lossy(&output.stdout).into_owned())
    } else {
        None
    }
}

struct EmitOptions {
    input_path: PathBuf,
    output_path: PathBuf,
    emit_pdb: bool,
    strict: bool,
}

impl EmitOptions {
    fn parse(args: &[String]) -> Result<Self, Error> {
        if args.len() < 3 {
            return Err(Error::new(2, emit_usage()));
        }

        let mut input_path = None;
        let mut output_path = None;
        let mut emit_pdb = false;
        let mut strict = false;
        let mut index = 0;

        while index < args.len() {
            match args[index].as_str() {
                "--out" => {
                    let value = args
                        .get(index + 1)
                        .ok_or_else(|| Error::new(2, "missing value for --out"))?;
                    output_path = Some(PathBuf::from(value));
                    index += 2;
                }
                "--pdb" => {
                    emit_pdb = true;
                    index += 1;
                }
                "--strict" => {
                    strict = true;
                    index += 1;
                }
                "--llvm-root" => {
                    return Err(Error::new(
                        2,
                        "--llvm-root is not used by the native host; LLVM is linked in-process",
                    ));
                }
                value if value.starts_with('-') => {
                    return Err(Error::new(2, format!("unsupported emit option '{value}'")));
                }
                value => {
                    if input_path.replace(PathBuf::from(value)).is_some() {
                        return Err(Error::new(2, "emit accepts exactly one input path"));
                    }
                    index += 1;
                }
            }
        }

        Ok(Self {
            input_path: input_path.ok_or_else(|| Error::new(2, emit_usage()))?,
            output_path: output_path.ok_or_else(|| Error::new(2, emit_usage()))?,
            emit_pdb,
            strict,
        })
    }

    fn to_emit_options_json(&self) -> String {
        let mut json = format!("{{\"schemaVersion\":{RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION},");
        append_json_property(&mut json, "inputPath", &path_to_string(&self.input_path));
        json.push(',');
        append_json_property(&mut json, "outputPath", &path_to_string(&self.output_path));
        json.push_str(",\"emitPdb\":");
        json.push_str(if self.emit_pdb { "true" } else { "false" });
        json.push_str(",\"strictUnsupportedIr\":");
        json.push_str(if self.strict { "true" } else { "false" });
        json.push('}');
        json
    }
}

fn emit_usage() -> String {
    "Usage: rustlyn emit <path-to-bc> --out <path-to-dll> [--pdb] [--strict]".to_string()
}

fn lower_options_json(input_path: &Path) -> String {
    let mut json = format!("{{\"schemaVersion\":{RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION},");
    append_json_property(&mut json, "inputPath", &path_to_string(input_path));
    json.push('}');
    json
}

fn pack_options_json(
    crate_name: &str,
    version: &str,
    assembly_path: &Path,
    output_dir: &Path,
) -> String {
    let mut json = format!("{{\"schemaVersion\":{RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION},");
    append_json_property(&mut json, "crateName", crate_name);
    json.push(',');
    append_json_property(&mut json, "version", version);
    json.push(',');
    append_json_property(&mut json, "assemblyPath", &path_to_string(assembly_path));
    json.push(',');
    append_json_property(&mut json, "outputDir", &path_to_string(output_dir));
    json.push('}');
    json
}

fn append_json_property(json: &mut String, name: &str, value: &str) {
    json.push('"');
    json.push_str(name);
    json.push_str("\":\"");
    push_json_escaped(json, value);
    json.push('"');
}

fn push_json_escaped(json: &mut String, value: &str) {
    for ch in value.chars() {
        match ch {
            '"' => json.push_str("\\\""),
            '\\' => json.push_str("\\\\"),
            '\n' => json.push_str("\\n"),
            '\r' => json.push_str("\\r"),
            '\t' => json.push_str("\\t"),
            '\u{08}' => json.push_str("\\b"),
            '\u{0C}' => json.push_str("\\f"),
            ch if ch <= '\u{1F}' => {
                use std::fmt::Write as _;
                let _ = write!(json, "\\u{:04x}", ch as u32);
            }
            ch => json.push(ch),
        }
    }
}

fn extract_json_int(json: &str, property_name: &str) -> Option<i32> {
    let needle = format!("\"{property_name}\":");
    let start = json.find(&needle)? + needle.len();
    let bytes = json.as_bytes();
    let mut end = start;
    if bytes.get(end).copied() == Some(b'-') {
        end += 1;
    }
    while bytes.get(end).is_some_and(u8::is_ascii_digit) {
        end += 1;
    }

    json[start..end].parse().ok()
}

fn path_to_string(path: &Path) -> String {
    path.to_string_lossy().into_owned()
}

fn display_path_string(path: &Path) -> String {
    let path = path_to_string(path);
    path.strip_prefix(r"\\?\")
        .map(str::to_string)
        .unwrap_or(path)
}

enum PathOrStdout {
    Path(PathBuf),
    Stdout,
}

#[derive(Clone)]
struct CargoBuildOptions {
    crate_path: PathBuf,
    release: bool,
    binary_target_name: Option<String>,
    infer_binary_target: bool,
    toolchain: Option<String>,
    target: Option<String>,
    build_std: Option<String>,
    build_std_features: Option<String>,
    output_bitcode_path: Option<PathBuf>,
}

impl CargoBuildOptions {
    fn new(crate_path: PathBuf) -> Self {
        Self {
            crate_path,
            release: true,
            binary_target_name: None,
            infer_binary_target: false,
            toolchain: None,
            target: None,
            build_std: None,
            build_std_features: None,
            output_bitcode_path: None,
        }
    }

    fn parse_cargo(args: &[String]) -> Result<(Self, bool), Error> {
        if args.is_empty() || is_help(&args[0]) {
            return Err(Error::new(2, command_help("cargo").unwrap()));
        }

        let mut index = 0;
        let mut options = Self::new(env::current_dir().unwrap_or_else(|_| PathBuf::from(".")));
        options.release = false;
        options.infer_binary_target = true;
        let mut strict = false;

        if args[index].starts_with('+') && args[index].len() > 1 {
            options.toolchain = Some(args[index][1..].to_string());
            index += 1;
        }
        if args.get(index).map(String::as_str) != Some("build") {
            return Err(Error::new(2, command_help("cargo").unwrap()));
        }
        index += 1;

        while index < args.len() {
            match args[index].as_str() {
                "--release" => {
                    options.release = true;
                    index += 1;
                }
                "--manifest-path" => {
                    options.crate_path =
                        PathBuf::from(next_arg(args, &mut index, "--manifest-path")?)
                }
                "--bin" => {
                    options.binary_target_name =
                        Some(next_arg(args, &mut index, "--bin")?.to_string())
                }
                "--toolchain" => {
                    options.toolchain = Some(next_arg(args, &mut index, "--toolchain")?.to_string())
                }
                "--target" => {
                    options.target = Some(next_arg(args, &mut index, "--target")?.to_string())
                }
                "--build-std" => {
                    options.build_std = Some(next_arg(args, &mut index, "--build-std")?.to_string())
                }
                "--build-std-features" => {
                    options.build_std_features =
                        Some(next_arg(args, &mut index, "--build-std-features")?.to_string())
                }
                "-Z" => apply_cargo_unstable(next_arg(args, &mut index, "-Z")?, &mut options)?,
                option if option.starts_with("-Z") && option.len() > 2 => {
                    apply_cargo_unstable(&option[2..], &mut options)?;
                    index += 1;
                }
                "--strict" => {
                    strict = true;
                    index += 1;
                }
                "--llvm-root" => {
                    return Err(Error::new(
                        2,
                        "--llvm-root is not used by the native host; LLVM is linked in-process",
                    ));
                }
                "--powershell-cmdlet-bindings" => {
                    return Err(Error::new(
                        2,
                        "--powershell-cmdlet-bindings is not available in the native host yet",
                    ));
                }
                value => return Err(Error::new(2, format!("unsupported cargo option '{value}'"))),
            }
        }

        options.validate()?;
        Ok((options, strict))
    }

    fn parse_translate(args: &[String]) -> Result<(Self, PathBuf, bool), Error> {
        if args.is_empty() || is_help(&args[0]) {
            return Err(Error::new(2, command_help("translate").unwrap()));
        }

        let mut options = Self::new(PathBuf::from(&args[0]));
        let mut output_path = None;
        let mut strict = false;
        let mut index = 1;
        while index < args.len() {
            match args[index].as_str() {
                "--out" => output_path = Some(PathBuf::from(next_arg(args, &mut index, "--out")?)),
                "--bitcode-out" => {
                    options.output_bitcode_path =
                        Some(PathBuf::from(next_arg(args, &mut index, "--bitcode-out")?))
                }
                "--bin" => {
                    options.binary_target_name =
                        Some(next_arg(args, &mut index, "--bin")?.to_string())
                }
                "--debug" => {
                    options.release = false;
                    index += 1;
                }
                "--toolchain" => {
                    options.toolchain = Some(next_arg(args, &mut index, "--toolchain")?.to_string())
                }
                "--target" => {
                    options.target = Some(next_arg(args, &mut index, "--target")?.to_string())
                }
                "--build-std" => {
                    options.build_std = Some(next_arg(args, &mut index, "--build-std")?.to_string())
                }
                "--build-std-features" => {
                    options.build_std_features =
                        Some(next_arg(args, &mut index, "--build-std-features")?.to_string())
                }
                "--strict" => {
                    strict = true;
                    index += 1;
                }
                "--cache" => {
                    let _ = next_arg(args, &mut index, "--cache")?;
                }
                "--llvm-root" => {
                    return Err(Error::new(
                        2,
                        "--llvm-root is not used by the native host; LLVM is linked in-process",
                    ));
                }
                "--powershell-cmdlet-bindings" => {
                    return Err(Error::new(
                        2,
                        "--powershell-cmdlet-bindings is not available in the native host yet",
                    ));
                }
                value => {
                    return Err(Error::new(
                        2,
                        format!("unsupported translate option '{value}'"),
                    ))
                }
            }
        }

        options.validate()?;
        let output_path =
            output_path.ok_or_else(|| Error::new(2, command_help("translate").unwrap()))?;
        Ok((options, output_path, strict))
    }

    fn parse_pack(args: &[String]) -> Result<(Self, PathBuf, String, bool), Error> {
        if args.is_empty() || is_help(&args[0]) {
            return Err(Error::new(2, command_help("pack").unwrap()));
        }

        let mut options = Self::new(PathBuf::from(&args[0]));
        let mut output_dir = None;
        let mut version = "0.1.0".to_string();
        let mut strict = false;
        let mut index = 1;
        while index < args.len() {
            match args[index].as_str() {
                "--out" => output_dir = Some(PathBuf::from(next_arg(args, &mut index, "--out")?)),
                "--version" => version = next_arg(args, &mut index, "--version")?.to_string(),
                "--bin" => {
                    options.binary_target_name =
                        Some(next_arg(args, &mut index, "--bin")?.to_string())
                }
                "--debug" => {
                    options.release = false;
                    index += 1;
                }
                "--toolchain" => {
                    options.toolchain = Some(next_arg(args, &mut index, "--toolchain")?.to_string())
                }
                "--target" => {
                    options.target = Some(next_arg(args, &mut index, "--target")?.to_string())
                }
                "--build-std" => {
                    options.build_std = Some(next_arg(args, &mut index, "--build-std")?.to_string())
                }
                "--build-std-features" => {
                    options.build_std_features =
                        Some(next_arg(args, &mut index, "--build-std-features")?.to_string())
                }
                "--strict" => {
                    strict = true;
                    index += 1;
                }
                "--cache" => {
                    let _ = next_arg(args, &mut index, "--cache")?;
                }
                "--llvm-root" => {
                    return Err(Error::new(
                        2,
                        "--llvm-root is not used by the native host; LLVM is linked in-process",
                    ));
                }
                "--powershell-cmdlet-bindings" => {
                    return Err(Error::new(
                        2,
                        "--powershell-cmdlet-bindings is not available in the native host yet",
                    ));
                }
                value => return Err(Error::new(2, format!("unsupported pack option '{value}'"))),
            }
        }

        options.validate()?;
        if version.trim().is_empty() {
            return Err(Error::new(2, "--version requires a non-empty value"));
        }
        let output_dir = output_dir.ok_or_else(|| Error::new(2, command_help("pack").unwrap()))?;
        Ok((options, output_dir, version, strict))
    }

    fn validate(&self) -> Result<(), Error> {
        if self.build_std.is_some() && self.toolchain.is_none() {
            return Err(Error::new(
                2,
                "build-std requires an explicit cargo toolchain such as 'nightly'. Configure --toolchain when using --build-std.",
            ));
        }
        if self.build_std_features.is_some() && self.build_std.is_none() {
            return Err(Error::new(
                2,
                "build-std-features requires --build-std. Configure --build-std when using --build-std-features.",
            ));
        }
        Ok(())
    }

    fn clone_for_build(&self) -> Self {
        self.clone()
    }
}

struct CargoBuildResult {
    bitcode_path: PathBuf,
    assembly_path: PathBuf,
}

struct CargoMetadata {
    target_directory: PathBuf,
    target_name: String,
    is_binary: bool,
}

fn apply_cargo_unstable(option: &str, options: &mut CargoBuildOptions) -> Result<(), Error> {
    if let Some(value) = option.strip_prefix("build-std=") {
        options.build_std = Some(value.to_string());
        return Ok(());
    }
    if let Some(value) = option.strip_prefix("build-std-features=") {
        options.build_std_features = Some(value.to_string());
        return Ok(());
    }
    Err(Error::new(
        2,
        format!("unsupported cargo -Z option '{option}'"),
    ))
}

fn build_cargo_project(options: &CargoBuildOptions) -> Result<CargoBuildResult, Error> {
    let manifest_path = resolve_manifest_path(&options.crate_path)?;
    let metadata = read_cargo_metadata(&manifest_path, options)?;
    let profile_directory = profile_directory(
        &metadata.target_directory,
        options.release,
        options.target.as_deref(),
    );
    let bitcode_path = profile_directory.join(format!(
        "{}.bc",
        normalize_cargo_artifact_name(&metadata.target_name)
    ));
    let build_options = CargoBuildOptions {
        output_bitcode_path: Some(bitcode_path.clone()),
        binary_target_name: if metadata.is_binary && options.binary_target_name.is_none() {
            Some(metadata.target_name.clone())
        } else {
            options.binary_target_name.clone()
        },
        ..options.clone_for_build()
    };
    let bitcode_path = build_bitcode(&build_options)?;
    Ok(CargoBuildResult {
        assembly_path: bitcode_path.with_extension("dll"),
        bitcode_path,
    })
}

fn build_bitcode(options: &CargoBuildOptions) -> Result<PathBuf, Error> {
    let manifest_path = resolve_manifest_path(&options.crate_path)?;
    let crate_directory = manifest_path.parent().unwrap_or(Path::new("."));
    validate_target_path(options, crate_directory)?;
    preflight_build_std(options, crate_directory)?;
    let metadata = read_cargo_metadata(&manifest_path, options)?;
    let mut build_options = options.clone_for_build();
    if metadata.is_binary && build_options.binary_target_name.is_none() {
        build_options.binary_target_name = Some(metadata.target_name.clone());
    }

    let status = Command::new("cargo")
        .args(create_cargo_rustc_arguments(&manifest_path, &build_options))
        .current_dir(crate_directory)
        .status()
        .map_err(|err| Error::new(2, format!("cargo not found on PATH: {err}")))?;
    if !status.success() {
        return Err(Error::new(
            status.code().unwrap_or(3),
            format!(
                "cargo rustc failed with exit code {}",
                status.code().unwrap_or(3)
            ),
        ));
    }

    let artifact_path = find_bitcode_artifact(
        &metadata.target_directory,
        build_options.release,
        &metadata.target_name,
        build_options.target.as_deref(),
    )?;

    if let Some(output_path) = &build_options.output_bitcode_path {
        let output_path = full_path(output_path)?;
        if let Some(parent) = output_path.parent() {
            fs::create_dir_all(parent).map_err(|err| {
                Error::new(1, format!("failed to create '{}': {err}", parent.display()))
            })?;
        }
        fs::copy(&artifact_path, &output_path).map_err(|err| {
            Error::new(
                1,
                format!(
                    "failed to copy '{}' to '{}': {err}",
                    artifact_path.display(),
                    output_path.display()
                ),
            )
        })?;
        copy_sibling_llvm_ir_artifact(&artifact_path, &output_path)?;
        Ok(output_path)
    } else {
        Ok(artifact_path)
    }
}

fn resolve_manifest_path(crate_path: &Path) -> Result<PathBuf, Error> {
    let candidate = full_path(crate_path)?;
    if candidate.is_file() {
        if candidate.file_name().and_then(|value| value.to_str()) != Some("Cargo.toml") {
            return Err(Error::new(
                2,
                format!(
                    "expected a Cargo.toml manifest or a crate directory, but got '{}'",
                    crate_path.display()
                ),
            ));
        }
        return Ok(candidate);
    }

    let manifest_path = candidate.join("Cargo.toml");
    if manifest_path.is_file() {
        return Ok(manifest_path);
    }

    Err(Error::new(
        2,
        format!(
            "Cargo manifest not found for crate path '{}'",
            crate_path.display()
        ),
    ))
}

fn read_cargo_metadata(
    manifest_path: &Path,
    options: &CargoBuildOptions,
) -> Result<CargoMetadata, Error> {
    let mut arguments = Vec::new();
    if let Some(toolchain) = &options.toolchain {
        arguments.push(format!("+{toolchain}"));
    }
    arguments.extend([
        "metadata".to_string(),
        "--format-version".to_string(),
        "1".to_string(),
        "--no-deps".to_string(),
        "--manifest-path".to_string(),
        path_to_string(manifest_path),
    ]);

    let output = Command::new("cargo")
        .args(&arguments)
        .current_dir(manifest_path.parent().unwrap_or(Path::new(".")))
        .output()
        .map_err(|err| Error::new(2, format!("cargo not found on PATH: {err}")))?;
    if !output.status.success() {
        return Err(Error::new(
            output.status.code().unwrap_or(3),
            String::from_utf8_lossy(&output.stderr).trim().to_string(),
        ));
    }

    let root: Value = serde_json::from_slice(&output.stdout)
        .map_err(|err| Error::new(1, format!("failed to parse cargo metadata JSON: {err}")))?;
    let manifest_compare = comparable_path(manifest_path);
    let package = root
        .get("packages")
        .and_then(Value::as_array)
        .and_then(|packages| {
            packages.iter().find(|package| {
                package
                    .get("manifest_path")
                    .and_then(Value::as_str)
                    .map(|path| comparable_path(Path::new(path)) == manifest_compare)
                    .unwrap_or(false)
            })
        })
        .ok_or_else(|| {
            Error::new(
                1,
                format!(
                    "Cargo metadata did not contain a package entry for manifest '{}'",
                    manifest_path.display()
                ),
            )
        })?;

    let target = select_cargo_target(package, manifest_path, options)?;
    let target_directory = root
        .get("target_directory")
        .and_then(Value::as_str)
        .ok_or_else(|| Error::new(1, "Cargo metadata was missing target_directory"))?;
    let target_name = target
        .get("name")
        .and_then(Value::as_str)
        .ok_or_else(|| Error::new(1, "Cargo metadata target was missing name"))?;
    let is_binary = target_kind_contains(target, "bin");
    Ok(CargoMetadata {
        target_directory: PathBuf::from(target_directory),
        target_name: target_name.to_string(),
        is_binary,
    })
}

fn select_cargo_target<'a>(
    package: &'a Value,
    manifest_path: &Path,
    options: &CargoBuildOptions,
) -> Result<&'a Value, Error> {
    let targets = package
        .get("targets")
        .and_then(Value::as_array)
        .ok_or_else(|| Error::new(1, "Cargo metadata package was missing targets"))?;
    if let Some(binary_target_name) = &options.binary_target_name {
        return targets
            .iter()
            .find(|target| {
                target.get("name").and_then(Value::as_str) == Some(binary_target_name)
                    && target_kind_contains(target, "bin")
            })
            .ok_or_else(|| {
                Error::new(
                    2,
                    format!(
                        "Cargo package '{}' does not expose a binary target named '{}'",
                        manifest_path.display(),
                        binary_target_name
                    ),
                )
            });
    }

    if let Some(library_target) = targets.iter().find(|target| {
        target_kind_contains(target, "lib")
            || target_kind_contains(target, "staticlib")
            || target_kind_contains(target, "cdylib")
    }) {
        return Ok(library_target);
    }

    if options.infer_binary_target {
        let binary_targets = targets
            .iter()
            .filter(|target| target_kind_contains(target, "bin"))
            .collect::<Vec<_>>();
        if binary_targets.len() == 1 {
            return Ok(binary_targets[0]);
        }
        if binary_targets.len() > 1 {
            return Err(Error::new(
                2,
                format!(
                    "Cargo package '{}' exposes multiple binary targets. Specify --bin <name> for rustlyn cargo build.",
                    manifest_path.display()
                ),
            ));
        }
    }

    Err(Error::new(
        2,
        format!(
            "Cargo package '{}' does not expose a library target yet. Specify --bin <name> for binary targets.",
            manifest_path.display()
        ),
    ))
}

fn target_kind_contains(target: &Value, expected_kind: &str) -> bool {
    target
        .get("kind")
        .and_then(Value::as_array)
        .map(|kinds| {
            kinds
                .iter()
                .any(|kind| kind.as_str() == Some(expected_kind))
        })
        .unwrap_or(false)
}

fn create_cargo_rustc_arguments(manifest_path: &Path, options: &CargoBuildOptions) -> Vec<String> {
    let mut arguments = Vec::new();
    if let Some(toolchain) = &options.toolchain {
        arguments.push(format!("+{toolchain}"));
    }
    arguments.extend([
        "rustc".to_string(),
        "--manifest-path".to_string(),
        path_to_string(manifest_path),
    ]);
    if let Some(binary_target_name) = &options.binary_target_name {
        arguments.push("--bin".to_string());
        arguments.push(binary_target_name.clone());
    } else {
        arguments.push("--lib".to_string());
    }
    if options.release {
        arguments.push("--release".to_string());
    }
    if let Some(build_std) = &options.build_std {
        arguments.push("-Z".to_string());
        arguments.push(format!("build-std={build_std}"));
    }
    if let Some(build_std_features) = &options.build_std_features {
        arguments.push("-Z".to_string());
        arguments.push(format!("build-std-features={build_std_features}"));
    }
    if let Some(target) = &options.target {
        arguments.push("--target".to_string());
        arguments.push(target.clone());
    }
    arguments.extend([
        "--".to_string(),
        "--emit".to_string(),
        "llvm-bc,llvm-ir".to_string(),
        "-C".to_string(),
        "overflow-checks=off".to_string(),
        "-C".to_string(),
        "panic=abort".to_string(),
    ]);
    arguments
}

fn find_bitcode_artifact(
    target_directory: &Path,
    release: bool,
    target_name: &str,
    target: Option<&str>,
) -> Result<PathBuf, Error> {
    let deps_directory = profile_directory(target_directory, release, target).join("deps");
    let normalized_target_name = normalize_cargo_artifact_name(target_name);
    let exact_candidate = deps_directory.join(format!("{normalized_target_name}.bc"));
    let mut candidates = Vec::new();
    if exact_candidate.is_file() {
        candidates.push(exact_candidate);
    }
    if deps_directory.is_dir() {
        for entry in fs::read_dir(&deps_directory).map_err(|err| {
            Error::new(
                2,
                format!("failed to read '{}': {err}", deps_directory.display()),
            )
        })? {
            let entry =
                entry.map_err(|err| Error::new(2, format!("failed to read dir entry: {err}")))?;
            let path = entry.path();
            let Some(file_name) = path.file_name().and_then(|value| value.to_str()) else {
                continue;
            };
            if file_name.starts_with(&format!("{normalized_target_name}-"))
                && file_name.ends_with(".bc")
            {
                candidates.push(path);
            }
        }
    }

    candidates
        .into_iter()
        .max_by_key(|path| {
            path.metadata()
                .and_then(|metadata| metadata.modified())
                .ok()
        })
        .ok_or_else(|| {
            Error::new(
                2,
                format!(
                    "No LLVM bitcode artifact matching '{}-*.bc' was found in '{}'.",
                    normalized_target_name,
                    deps_directory.display()
                ),
            )
        })
}

fn copy_sibling_llvm_ir_artifact(
    artifact_path: &Path,
    output_bitcode_path: &Path,
) -> Result<(), Error> {
    let source_llvm_ir_path = artifact_path.with_extension("ll");
    if !source_llvm_ir_path.is_file() {
        return Ok(());
    }
    fs::copy(
        &source_llvm_ir_path,
        output_bitcode_path.with_extension("ll"),
    )
    .map_err(|err| {
        Error::new(
            1,
            format!(
                "failed to copy '{}' beside '{}': {err}",
                source_llvm_ir_path.display(),
                output_bitcode_path.display()
            ),
        )
    })?;
    Ok(())
}

fn validate_target_path(options: &CargoBuildOptions, crate_directory: &Path) -> Result<(), Error> {
    let Some(target) = &options.target else {
        return Ok(());
    };
    if !target.ends_with(".json") {
        return Ok(());
    }
    let target_path = if Path::new(target).is_absolute() {
        PathBuf::from(target)
    } else {
        crate_directory.join(target)
    };
    if !target_path.is_file() {
        return Err(Error::new(
            2,
            format!("Rust target JSON '{}' was not found.", target),
        ));
    }
    Ok(())
}

fn preflight_build_std(options: &CargoBuildOptions, working_directory: &Path) -> Result<(), Error> {
    let Some(build_std) = &options.build_std else {
        return Ok(());
    };
    let toolchain = options
        .toolchain
        .as_deref()
        .ok_or_else(|| Error::new(2, "build-std requires --toolchain"))?;
    if build_std.trim().is_empty() {
        return Err(Error::new(
            2,
            "--build-std requires a non-empty component list",
        ));
    }
    let output = Command::new("rustc")
        .arg(format!("+{toolchain}"))
        .arg("--print")
        .arg("sysroot")
        .current_dir(working_directory)
        .output()
        .map_err(|err| Error::new(2, format!("rustc not found on PATH: {err}")))?;
    if !output.status.success() {
        return Err(Error::new(
            2,
            String::from_utf8_lossy(&output.stderr).trim().to_string(),
        ));
    }
    let rust_source_path =
        PathBuf::from(String::from_utf8_lossy(&output.stdout).trim().to_string())
            .join("lib")
            .join("rustlib")
            .join("src")
            .join("rust")
            .join("library");
    if !rust_source_path.is_dir() {
        return Err(Error::new(
            2,
            format!("build-std requires the rust-src component for toolchain '{toolchain}'. Install it with: rustup component add rust-src --toolchain {toolchain}."),
        ));
    }
    Ok(())
}

fn profile_directory(target_directory: &Path, release: bool, target: Option<&str>) -> PathBuf {
    let profile = if release { "release" } else { "debug" };
    match target {
        Some(target) if !target.trim().is_empty() => target_directory
            .join(resolve_target_directory_name(target))
            .join(profile),
        _ => target_directory.join(profile),
    }
}

fn resolve_target_directory_name(target: &str) -> String {
    if target.ends_with(".json") {
        Path::new(target)
            .file_stem()
            .and_then(|value| value.to_str())
            .unwrap_or(target)
            .to_string()
    } else {
        target.to_string()
    }
}

fn normalize_cargo_artifact_name(target_name: &str) -> String {
    target_name.replace('-', "_")
}

fn comparable_path(path: &Path) -> String {
    fs::canonicalize(path)
        .unwrap_or_else(|_| path.to_path_buf())
        .to_string_lossy()
        .trim_start_matches(r"\\?\")
        .to_ascii_lowercase()
}

struct ModuleSummary {
    functions: Vec<FunctionSummary>,
    aliases: Vec<AliasSummary>,
    globals: Vec<GlobalSummary>,
}

impl ModuleSummary {
    fn basic_block_count(&self) -> usize {
        self.functions
            .iter()
            .map(|function| function.basic_block_count)
            .sum()
    }

    fn instruction_count(&self) -> usize {
        self.functions
            .iter()
            .map(|function| function.instruction_count)
            .sum()
    }
}

struct FunctionSummary {
    name: String,
    basic_block_count: usize,
    instruction_count: usize,
}

struct AliasSummary {
    name: String,
}

struct GlobalSummary {
    name: String,
}

#[derive(Debug)]
struct Error {
    exit_code: i32,
    message: String,
}

impl Error {
    fn new(exit_code: i32, message: impl Into<String>) -> Self {
        Self {
            exit_code,
            message: message.into(),
        }
    }

    fn exit_code(&self) -> i32 {
        self.exit_code
    }
}

impl fmt::Display for Error {
    fn fmt(&self, formatter: &mut fmt::Formatter<'_>) -> fmt::Result {
        formatter.write_str(&self.message)
    }
}

impl StdError for Error {}
