use std::env;
use std::error::Error as StdError;
use std::fmt;
use std::path::{Path, PathBuf};
use std::ptr;
use std::slice;

extern "C" {
    fn rustlyn_emit(
        options_json: *const u8,
        options_len: usize,
        result_json: *mut *mut u8,
        result_len: *mut usize,
    ) -> i32;
    fn rustlyn_free(ptr: *mut u8);
}

fn main() {
    if let Err(err) = run(env::args().skip(1).collect()) {
        eprintln!("{err}");
        std::process::exit(err.exit_code());
    }
}

fn run(args: Vec<String>) -> Result<(), SmokeError> {
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
        let mut json = String::from("{");
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
