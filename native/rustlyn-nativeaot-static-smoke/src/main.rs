use std::env;
use std::error::Error as StdError;
use std::ffi::{c_char, CStr, CString};
use std::fmt;
use std::os::raw::c_uint;
use std::path::{Path, PathBuf};
use std::ptr;
use std::slice;

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
    fn rustlyn_free(ptr: *mut u8);
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

fn main() {
    if let Err(err) = run(env::args().skip(1).collect()) {
        eprintln!("{err}");
        std::process::exit(err.exit_code());
    }
}

fn run(args: Vec<String>) -> Result<(), SmokeError> {
    let llvm_version = probe_llvm_version();
    eprintln!(
        "linked LLVM {}.{}.{}",
        llvm_version.0, llvm_version.1, llvm_version.2
    );

    register_host_callbacks()?;

    let options = SmokeOptions::parse(&args)?;
    let options_json = options.to_emit_options_json();
    let mut result_ptr: *mut u8 = ptr::null_mut();
    let mut result_len: usize = 0;
    let code = unsafe {
        rustlyn_emit(
            options_json.as_ptr(),
            options_json.len(),
            &mut result_ptr,
            &mut result_len,
        )
    };

    let result_json = unsafe {
        let result = if result_ptr.is_null() || result_len == 0 {
            String::new()
        } else {
            let bytes = slice::from_raw_parts(result_ptr, result_len);
            String::from_utf8_lossy(bytes).into_owned()
        };
        rustlyn_free(result_ptr);
        result
    };

    if !result_json.is_empty() {
        println!("{result_json}");
    }

    fn register_host_callbacks() -> Result<(), SmokeError> {
        let callbacks = RustlynHostCallbacks {
            schema_version: RUSTLYN_HOST_CALLBACK_SCHEMA_VERSION,
            print_ir: rustlyn_llvm_print_ir,
            free: rustlyn_llvm_free,
        };
        let code = unsafe {
            rustlyn_register_host_callbacks(RUSTLYN_HOST_CALLBACK_SCHEMA_VERSION, &callbacks)
        };
        if code == 0 {
            Ok(())
        } else {
            Err(SmokeError::new(
                2,
                format!("rustlyn_register_host_callbacks returned code {code}"),
            ))
        }
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

    if code == 0 && result_json.contains("\"success\":true") {
        Ok(())
    } else {
        Err(SmokeError::new(
            1,
            format!("rustlyn_emit returned code {code}"),
        ))
    }
}

struct SmokeOptions {
    input_path: PathBuf,
    output_path: PathBuf,
    llvm_root: Option<PathBuf>,
}

impl SmokeOptions {
    fn parse(args: &[String]) -> Result<Self, SmokeError> {
        let mut input_path = None;
        let mut output_path = None;
        let mut llvm_root = None;
        let mut index = 0;

        while index < args.len() {
            match args[index].as_str() {
                "--input" => input_path = Some(next_path(args, &mut index, "--input")?),
                "--output" => output_path = Some(next_path(args, &mut index, "--output")?),
                "--llvm-root" => llvm_root = Some(next_path(args, &mut index, "--llvm-root")?),
                "--help" | "-h" => return Err(SmokeError::new(0, usage())),
                value => {
                    return Err(SmokeError::new(
                        2,
                        format!("unsupported argument '{value}'\n\n{}", usage()),
                    ));
                }
            }
        }

        Ok(Self {
            input_path: require_path(input_path, "--input")?,
            output_path: require_path(output_path, "--output")?,
            llvm_root,
        })
    }

    fn to_emit_options_json(&self) -> String {
        let mut json = format!("{{\"schemaVersion\":{RUSTLYN_NATIVEAOT_ABI_SCHEMA_VERSION},");
        append_json_property(&mut json, "inputPath", &path_to_string(&self.input_path));
        json.push(',');
        append_json_property(&mut json, "outputPath", &path_to_string(&self.output_path));
        if let Some(llvm_root) = &self.llvm_root {
            json.push(',');
            append_json_property(&mut json, "llvmRoot", &path_to_string(llvm_root));
        }
        json.push_str(",\"emitPdb\":false,\"strictUnsupportedIr\":true}");
        json
    }
}

fn next_path(args: &[String], index: &mut usize, name: &str) -> Result<PathBuf, SmokeError> {
    *index += 1;
    let value = args
        .get(*index)
        .ok_or_else(|| SmokeError::new(2, format!("{name} requires a value")))?;
    *index += 1;
    Ok(PathBuf::from(value))
}

fn require_path(value: Option<PathBuf>, name: &str) -> Result<PathBuf, SmokeError> {
    value.ok_or_else(|| SmokeError::new(2, format!("{name} is required\n\n{}", usage())))
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

fn path_to_string(path: &Path) -> String {
    path.to_string_lossy().into_owned()
}

fn usage() -> String {
    "usage: rustlyn-nativeaot-static-smoke --input <input.bc> --output <output.dll> [--llvm-root <path>]".to_string()
}

#[derive(Debug)]
struct SmokeError {
    exit_code: i32,
    message: String,
}

impl SmokeError {
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

impl fmt::Display for SmokeError {
    fn fmt(&self, formatter: &mut fmt::Formatter<'_>) -> fmt::Result {
        formatter.write_str(&self.message)
    }
}

impl StdError for SmokeError {}
