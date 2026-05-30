use std::env;
use std::error::Error as StdError;
use std::ffi::{c_char, c_void};
use std::fmt;
use std::path::{Path, PathBuf};
use std::ptr;
use std::slice;

#[cfg(unix)]
use std::ffi::CString;

type RustlynEmit = unsafe extern "C" fn(*const u8, usize, *mut *mut u8, *mut usize) -> i32;
type RustlynFree = unsafe extern "C" fn(*mut u8);

fn main() {
    if let Err(err) = run(env::args().skip(1).collect()) {
        eprintln!("{err}");
        std::process::exit(err.exit_code());
    }
}

fn run(args: Vec<String>) -> Result<(), SmokeError> {
    let options = SmokeOptions::parse(&args)?;
    let library = DynamicLibrary::open(&options.library_path)?;
    let rustlyn_emit: RustlynEmit = unsafe { library.symbol(b"rustlyn_emit\0")? };
    let rustlyn_free: RustlynFree = unsafe { library.symbol(b"rustlyn_free\0")? };
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

    if code == 0 && result_json.contains("\"success\":true") {
        Ok(())
    } else if code < 0 {
        Err(SmokeError::new(
            1,
            format!("rustlyn_emit returned ABI error code {code}"),
        ))
    } else {
        Err(SmokeError::new(
            1,
            format!("rustlyn_emit returned operation code {code}"),
        ))
    }
}

struct SmokeOptions {
    library_path: PathBuf,
    input_path: PathBuf,
    output_path: PathBuf,
    llvm_root: Option<PathBuf>,
    emit_pdb: bool,
    strict_unsupported_ir: bool,
}

impl SmokeOptions {
    fn parse(args: &[String]) -> Result<Self, SmokeError> {
        let mut library_path = None;
        let mut input_path = None;
        let mut output_path = None;
        let mut llvm_root = None;
        let mut emit_pdb = false;
        let mut strict_unsupported_ir = true;
        let mut index = 0;

        while index < args.len() {
            match args[index].as_str() {
                "--library" => {
                    library_path = Some(next_path(args, &mut index, "--library")?);
                }
                "--input" => {
                    input_path = Some(next_path(args, &mut index, "--input")?);
                }
                "--output" => {
                    output_path = Some(next_path(args, &mut index, "--output")?);
                }
                "--llvm-root" => {
                    llvm_root = Some(next_path(args, &mut index, "--llvm-root")?);
                }
                "--pdb" => {
                    emit_pdb = true;
                    index += 1;
                }
                "--no-strict" => {
                    strict_unsupported_ir = false;
                    index += 1;
                }
                "--help" | "-h" => {
                    return Err(SmokeError::new(0, usage()));
                }
                value => {
                    return Err(SmokeError::new(
                        2,
                        format!("unsupported argument '{value}'\n\n{}", usage()),
                    ));
                }
            }
        }

        Ok(Self {
            library_path: require_path(library_path, "--library")?,
            input_path: require_path(input_path, "--input")?,
            output_path: require_path(output_path, "--output")?,
            llvm_root,
            emit_pdb,
            strict_unsupported_ir,
        })
    }

    fn to_emit_options_json(&self) -> String {
        let mut json = String::from("{");
        append_json_property(&mut json, "inputPath", &path_to_string(&self.input_path));
        json.push(',');
        append_json_property(&mut json, "outputPath", &path_to_string(&self.output_path));
        if let Some(llvm_root) = &self.llvm_root {
            json.push(',');
            append_json_property(&mut json, "llvmRoot", &path_to_string(llvm_root));
        }
        json.push_str(",\"emitPdb\":");
        json.push_str(if self.emit_pdb { "true" } else { "false" });
        json.push_str(",\"strictUnsupportedIr\":");
        json.push_str(if self.strict_unsupported_ir { "true" } else { "false" });
        json.push('}');
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
    "usage: rustlyn-nativeaot-smoke --library <rustlyn_nativeaot.dll> --input <input.bc> --output <output.dll> [--llvm-root <path>] [--pdb] [--no-strict]".to_string()
}

#[cfg(windows)]
struct DynamicLibrary {
    handle: *mut c_void,
}

#[cfg(windows)]
impl DynamicLibrary {
    fn open(path: &Path) -> Result<Self, SmokeError> {
        use std::os::windows::ffi::OsStrExt;
        let wide_path: Vec<u16> = path
            .as_os_str()
            .encode_wide()
            .chain(std::iter::once(0))
            .collect();
        let handle = unsafe { LoadLibraryW(wide_path.as_ptr()) };
        if handle.is_null() {
            return Err(SmokeError::new(
                1,
                format!("failed to load native library '{}'", path.display()),
            ));
        }

        Ok(Self { handle })
    }

    unsafe fn symbol<T>(&self, name: &[u8]) -> Result<T, SmokeError>
    where
        T: Copy,
    {
        let ptr = GetProcAddress(self.handle, name.as_ptr().cast());
        if ptr.is_null() {
            return Err(SmokeError::new(
                1,
                format!(
                    "failed to resolve export '{}'",
                    String::from_utf8_lossy(name).trim_end_matches('\0')
                ),
            ));
        }

        Ok(std::mem::transmute_copy(&ptr))
    }
}

#[cfg(windows)]
impl Drop for DynamicLibrary {
    fn drop(&mut self) {
        unsafe {
            FreeLibrary(self.handle);
        }
    }
}

#[cfg(windows)]
#[link(name = "kernel32")]
extern "system" {
    fn LoadLibraryW(path: *const u16) -> *mut c_void;
    fn GetProcAddress(module: *mut c_void, name: *const c_char) -> *mut c_void;
    fn FreeLibrary(module: *mut c_void) -> i32;
}

#[cfg(unix)]
struct DynamicLibrary {
    handle: *mut c_void,
}

#[cfg(unix)]
impl DynamicLibrary {
    fn open(path: &Path) -> Result<Self, SmokeError> {
        const RTLD_NOW: i32 = 2;
        let path = CString::new(path_to_string(path))
            .map_err(|_| SmokeError::new(1, "native library path contains an interior NUL"))?;
        let handle = unsafe { dlopen(path.as_ptr(), RTLD_NOW) };
        if handle.is_null() {
            return Err(SmokeError::new(1, "failed to load native library"));
        }

        Ok(Self { handle })
    }

    unsafe fn symbol<T>(&self, name: &[u8]) -> Result<T, SmokeError>
    where
        T: Copy,
    {
        let name = CString::new(name.trim_ascii_end_matches(&0))
            .map_err(|_| SmokeError::new(1, "symbol name contains an interior NUL"))?;
        let ptr = dlsym(self.handle, name.as_ptr());
        if ptr.is_null() {
            return Err(SmokeError::new(1, "failed to resolve native export"));
        }

        Ok(std::mem::transmute_copy(&ptr))
    }
}

#[cfg(unix)]
impl Drop for DynamicLibrary {
    fn drop(&mut self) {
        unsafe {
            dlclose(self.handle);
        }
    }
}

#[cfg(unix)]
extern "C" {
    fn dlopen(path: *const c_char, mode: i32) -> *mut c_void;
    fn dlsym(module: *mut c_void, name: *const c_char) -> *mut c_void;
    fn dlclose(module: *mut c_void) -> i32;
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
