use std::env;
use std::ffi::{c_char, CStr, CString};
use std::fs;
use std::path::{Path, PathBuf};
use std::ptr;

type LlvmBool = i32;
type LlvmModuleRef = *mut std::ffi::c_void;
type LlvmMemoryBufferRef = *mut std::ffi::c_void;
type LlvmValueRef = *mut std::ffi::c_void;
type LlvmBasicBlockRef = *mut std::ffi::c_void;
type LlvmTypeRef = *mut std::ffi::c_void;

const LLVM_RETURN_STATUS_ACTION: i32 = 2;

extern "C" {
    fn LLVMGetVersion(major: *mut u32, minor: *mut u32, patch: *mut u32);
    fn LLVMCreateMemoryBufferWithContentsOfFile(
        path: *const c_char,
        out_mem_buf: *mut LlvmMemoryBufferRef,
        out_message: *mut *mut c_char,
    ) -> LlvmBool;
    fn LLVMDisposeMemoryBuffer(mem_buf: LlvmMemoryBufferRef);
    fn LLVMParseBitcode2(mem_buf: LlvmMemoryBufferRef, out_module: *mut LlvmModuleRef) -> LlvmBool;
    fn LLVMDisposeModule(module: LlvmModuleRef);
    fn LLVMPrintModuleToString(module: LlvmModuleRef) -> *mut c_char;
    fn LLVMPrintTypeToString(ty: LlvmTypeRef) -> *mut c_char;
    fn LLVMPrintValueToString(value: LlvmValueRef) -> *mut c_char;
    fn LLVMVerifyModule(module: LlvmModuleRef, action: i32, out_message: *mut *mut c_char) -> LlvmBool;
    fn LLVMDisposeMessage(message: *mut c_char);
    fn LLVMGetFirstFunction(module: LlvmModuleRef) -> LlvmValueRef;
    fn LLVMGetNextFunction(function: LlvmValueRef) -> LlvmValueRef;
    fn LLVMGetFirstBasicBlock(function: LlvmValueRef) -> LlvmBasicBlockRef;
    fn LLVMGetNextBasicBlock(block: LlvmBasicBlockRef) -> LlvmBasicBlockRef;
    fn LLVMGetFirstInstruction(block: LlvmBasicBlockRef) -> LlvmValueRef;
    fn LLVMGetNextInstruction(instruction: LlvmValueRef) -> LlvmValueRef;
    fn LLVMGetFirstGlobal(module: LlvmModuleRef) -> LlvmValueRef;
    fn LLVMGetNextGlobal(global: LlvmValueRef) -> LlvmValueRef;
    fn LLVMGetFirstGlobalAlias(module: LlvmModuleRef) -> LlvmValueRef;
    fn LLVMGetNextGlobalAlias(alias: LlvmValueRef) -> LlvmValueRef;
    fn LLVMAliasGetAliasee(alias: LlvmValueRef) -> LlvmValueRef;
    fn LLVMGlobalGetValueType(global: LlvmValueRef) -> LlvmTypeRef;
    fn LLVMGetValueName2(value: LlvmValueRef, length: *mut usize) -> *const c_char;
    fn LLVMGetBasicBlockName(block: LlvmBasicBlockRef) -> *const c_char;
    fn LLVMTypeOf(value: LlvmValueRef) -> LlvmTypeRef;
    fn LLVMGetReturnType(function_ty: LlvmTypeRef) -> LlvmTypeRef;
    fn LLVMCountParams(function: LlvmValueRef) -> u32;
    fn LLVMGetParam(function: LlvmValueRef, index: u32) -> LlvmValueRef;
    fn LLVMGetInstructionOpcode(instruction: LlvmValueRef) -> u32;
    fn LLVMGetNumOperands(value: LlvmValueRef) -> i32;
    fn LLVMGetOperand(value: LlvmValueRef, index: u32) -> LlvmValueRef;

    fn LLVMWriteBitcodeToFile(module: LlvmModuleRef, path: *const c_char) -> i32;
    fn LLVMCreatePassBuilderOptions() -> LlvmPassBuilderOptionsRef;
    fn LLVMDisposePassBuilderOptions(options: LlvmPassBuilderOptionsRef);
    fn LLVMPassBuilderOptionsSetVerifyEach(options: LlvmPassBuilderOptionsRef, verify_each: LlvmBool);
    fn LLVMRunPasses(
        module: LlvmModuleRef,
        passes: *const c_char,
        target_machine: *mut std::ffi::c_void,
        options: LlvmPassBuilderOptionsRef,
    ) -> LlvmErrorRef;
    fn LLVMGetErrorMessage(err: LlvmErrorRef) -> *mut c_char;
    fn LLVMDisposeErrorMessage(message: *mut c_char);
}

type LlvmPassBuilderOptionsRef = *mut std::ffi::c_void;
type LlvmErrorRef = *mut std::ffi::c_void;

fn main() {
    if let Err(err) = run(env::args().skip(1).collect()) {
        eprintln!("{err}");
        std::process::exit(err.exit_code());
    }
}

fn run(args: Vec<String>) -> Result<(), Error> {
    if args.is_empty() {
        return Err(Error::usage());
    }

    match args[0].as_str() {
        "--version" | "-V" => {
            print_version();
            Ok(())
        }
        "diagnose" => {
            print_diagnostics();
            Ok(())
        }
        "print-ir" => run_print_ir(&args[1..]),
        "inspect-json" => run_inspect_json(&args[1..]),
        "lower-json" => run_lower_json(&args[1..]),
        "opt" => run_opt(&args[1..]),
        "-disable-verify" | "-S" | "-o" => run_compat_print_ir(&args),
        command => Err(Error::new(2, format!("unsupported rustlyn-llvm command '{command}'"))),
    }
}

fn run_lower_json(args: &[String]) -> Result<(), Error> {
    let mut input = None;
    let mut verify = true;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--disable-verify" | "-disable-verify" => {
                verify = false;
                index += 1;
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unsupported lower-json option '{value}'")));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "lower-json accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    let input = input.ok_or_else(|| Error::new(2, "lower-json requires an input bitcode path"))?;
    lower_json(&input, verify)
}

fn run_inspect_json(args: &[String]) -> Result<(), Error> {
    let mut input = None;
    let mut verify = true;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--disable-verify" | "-disable-verify" => {
                verify = false;
                index += 1;
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unsupported inspect-json option '{value}'")));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "inspect-json accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    let input = input.ok_or_else(|| Error::new(2, "inspect-json requires an input bitcode path"))?;
    inspect_json(&input, verify)
}

fn run_print_ir(args: &[String]) -> Result<(), Error> {
    let mut input = None;
    let mut output = PathOrStdout::Stdout;
    let mut verify = true;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--disable-verify" | "-disable-verify" => {
                verify = false;
                index += 1;
            }
            "--output" | "-o" => {
                let value = args
                    .get(index + 1)
                    .ok_or_else(|| Error::new(2, "missing value for --output"))?;
                output = PathOrStdout::from_arg(value);
                index += 2;
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unsupported print-ir option '{value}'")));
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
    print_ir(&input, output, verify)
}

fn run_compat_print_ir(args: &[String]) -> Result<(), Error> {
    let mut input = None;
    let mut output = PathOrStdout::Stdout;
    let mut verify = true;
    let mut saw_emit_llvm_ir = false;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "-disable-verify" | "--disable-verify" => {
                verify = false;
                index += 1;
            }
            "-S" => {
                saw_emit_llvm_ir = true;
                index += 1;
            }
            "-o" | "--output" => {
                let value = args
                    .get(index + 1)
                    .ok_or_else(|| Error::new(2, "missing value for -o"))?;
                output = PathOrStdout::from_arg(value);
                index += 2;
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unsupported compatibility option '{value}'")));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "compatibility mode accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    if !saw_emit_llvm_ir {
        return Err(Error::new(2, "compatibility mode requires -S"));
    }

    let input = input.ok_or_else(|| Error::new(2, "compatibility mode requires an input bitcode path"))?;
    print_ir(&input, output, verify)
}

fn print_ir(input: &Path, output: PathOrStdout, verify: bool) -> Result<(), Error> {
    if !input.is_file() {
        return Err(Error::new(3, format!("input bitcode file was not found: {}", input.display())));
    }

    let module = Module::parse(input)?;
    if verify {
        module.verify()?;
    }

    let ir = module.print_to_string()?;
    match output {
        PathOrStdout::Stdout => {
            print!("{ir}");
        }
        PathOrStdout::Path(path) => {
            fs::write(&path, ir)
                .map_err(|err| Error::new(1, format!("failed to write '{}': {err}", path.display())))?;
        }
    }

    Ok(())
}

fn inspect_json(input: &Path, verify: bool) -> Result<(), Error> {
    if !input.is_file() {
        return Err(Error::new(3, format!("input bitcode file was not found: {}", input.display())));
    }

    let module = Module::parse(input)?;
    if verify {
        module.verify()?;
    }

    print!("{}", module.inspect_json(input));
    Ok(())
}

fn lower_json(input: &Path, verify: bool) -> Result<(), Error> {
    if !input.is_file() {
        return Err(Error::new(3, format!("input bitcode file was not found: {}", input.display())));
    }

    let module = Module::parse(input)?;
    if verify {
        module.verify()?;
    }

    print!("{}", module.lower_json(input));
    Ok(())
}

fn run_opt(args: &[String]) -> Result<(), Error> {
    const DEFAULT_PIPELINE: &str = "mem2reg,sroa,simplifycfg";

    let mut input = None;
    let mut output = PathOrStdout::Stdout;
    let mut passes: Option<String> = None;
    let mut verify = true;
    let mut verify_each = false;
    let mut emit_ir = false;
    let mut index = 0;

    while index < args.len() {
        match args[index].as_str() {
            "--disable-verify" | "-disable-verify" => {
                verify = false;
                index += 1;
            }
            "--verify-each" => {
                verify_each = true;
                index += 1;
            }
            "--emit-llvm-ir" | "-S" => {
                emit_ir = true;
                index += 1;
            }
            "--passes" | "-passes" => {
                let value = args
                    .get(index + 1)
                    .ok_or_else(|| Error::new(2, "missing value for --passes"))?;
                passes = Some(value.clone());
                index += 2;
            }
            "--output" | "-o" => {
                let value = args
                    .get(index + 1)
                    .ok_or_else(|| Error::new(2, "missing value for --output"))?;
                output = PathOrStdout::from_arg(value);
                index += 2;
            }
            value if value.starts_with('-') => {
                return Err(Error::new(2, format!("unsupported opt option '{value}'")));
            }
            value => {
                if input.replace(PathBuf::from(value)).is_some() {
                    return Err(Error::new(2, "opt accepts exactly one input path"));
                }
                index += 1;
            }
        }
    }

    let input = input.ok_or_else(|| Error::new(2, "opt requires an input bitcode path"))?;
    let passes = passes.unwrap_or_else(|| DEFAULT_PIPELINE.to_string());
    if passes.trim().is_empty() {
        return Err(Error::new(2, "--passes must be a non-empty pipeline string"));
    }

    if let PathOrStdout::Path(path) = &output {
        if let Some(ext) = path.extension().and_then(|e| e.to_str()) {
            if ext.eq_ignore_ascii_case("ll") {
                emit_ir = true;
            }
        }
    }

    if !input.is_file() {
        return Err(Error::new(
            3,
            format!("input bitcode file was not found: {}", input.display()),
        ));
    }

    let module = Module::parse(&input)?;
    if verify {
        module.verify()?;
    }

    module.run_passes(&passes, verify_each)?;

    if verify {
        module.verify()?;
    }

    match output {
        PathOrStdout::Stdout => {
            if emit_ir {
                print!("{}", module.print_to_string()?);
            } else {
                return Err(Error::new(
                    2,
                    "opt to stdout requires --emit-llvm-ir (-S); bitcode cannot be safely written to stdout",
                ));
            }
        }
        PathOrStdout::Path(path) => {
            if emit_ir {
                let ir = module.print_to_string()?;
                fs::write(&path, ir).map_err(|err| {
                    Error::new(1, format!("failed to write '{}': {err}", path.display()))
                })?;
            } else {
                module.write_bitcode(&path)?;
            }
        }
    }

    Ok(())
}

fn print_version() {
    let (major, minor, patch) = llvm_version();
    println!(
        "rustlyn-llvm {} (LLVM {}.{}.{})",
        env!("CARGO_PKG_VERSION"),
        major,
        minor,
        patch
    );
}

fn print_diagnostics() {
    print_version();
    println!("build-llvm-version={}", option_env!("RUSTLYN_LLVM_VERSION").unwrap_or("unknown"));
    println!("linkage=static");
    println!("opt-pass-manager=new (LLVMRunPasses)");
    println!("opt-default-pipeline=mem2reg,sroa,simplifycfg");
}

fn llvm_version() -> (u32, u32, u32) {
    let mut major = 0;
    let mut minor = 0;
    let mut patch = 0;
    unsafe {
        LLVMGetVersion(&mut major, &mut minor, &mut patch);
    }
    (major, minor, patch)
}

struct Module {
    module: LlvmModuleRef,
}

impl Module {
    fn parse(path: &Path) -> Result<Self, Error> {
        let path = path
            .to_str()
            .ok_or_else(|| Error::new(3, format!("input path is not valid UTF-8: {}", path.display())))?;
        let path = CString::new(path).map_err(|_| Error::new(3, "input path contains an embedded NUL byte"))?;

        let mut message = ptr::null_mut();
        let mut buffer = ptr::null_mut();
        unsafe {
            if LLVMCreateMemoryBufferWithContentsOfFile(path.as_ptr(), &mut buffer, &mut message) != 0 {
                return Err(Error::new(4, consume_message(message).unwrap_or_else(|| "failed to create LLVM memory buffer".to_string())));
            }

            let buffer_guard = MemoryBuffer { buffer };
            let mut module = ptr::null_mut();
            if LLVMParseBitcode2(buffer_guard.buffer, &mut module) != 0 {
                return Err(Error::new(4, "failed to parse LLVM bitcode module"));
            }

            Ok(Self { module })
        }
    }

    fn verify(&self) -> Result<(), Error> {
        let mut message = ptr::null_mut();
        unsafe {
            if LLVMVerifyModule(self.module, LLVM_RETURN_STATUS_ACTION, &mut message) != 0 {
                return Err(Error::new(4, consume_message(message).unwrap_or_else(|| "LLVM module verification failed".to_string())));
            }
        }

        Ok(())
    }

    fn run_passes(&self, passes: &str, verify_each: bool) -> Result<(), Error> {
        let passes_c = CString::new(passes)
            .map_err(|_| Error::new(2, "--passes contains an embedded NUL byte"))?;
        unsafe {
            let options = LLVMCreatePassBuilderOptions();
            if options.is_null() {
                return Err(Error::new(1, "LLVMCreatePassBuilderOptions returned null"));
            }
            LLVMPassBuilderOptionsSetVerifyEach(options, if verify_each { 1 } else { 0 });
            let err = LLVMRunPasses(self.module, passes_c.as_ptr(), ptr::null_mut(), options);
            LLVMDisposePassBuilderOptions(options);
            if !err.is_null() {
                let raw = LLVMGetErrorMessage(err);
                let message = if raw.is_null() {
                    "LLVMRunPasses failed".to_string()
                } else {
                    let text = CStr::from_ptr(raw).to_string_lossy().into_owned();
                    LLVMDisposeErrorMessage(raw);
                    text
                };
                return Err(Error::new(4, format!("opt pipeline failed: {message}")));
            }
        }
        Ok(())
    }

    fn write_bitcode(&self, path: &Path) -> Result<(), Error> {
        let path_str = path.to_str().ok_or_else(|| {
            Error::new(3, format!("output path is not valid UTF-8: {}", path.display()))
        })?;
        let path_c = CString::new(path_str)
            .map_err(|_| Error::new(3, "output path contains an embedded NUL byte"))?;
        let rc = unsafe { LLVMWriteBitcodeToFile(self.module, path_c.as_ptr()) };
        if rc != 0 {
            return Err(Error::new(
                1,
                format!("LLVMWriteBitcodeToFile failed for '{}' (status {rc})", path.display()),
            ));
        }
        Ok(())
    }

    fn print_to_string(&self) -> Result<String, Error> {
        let text = unsafe { LLVMPrintModuleToString(self.module) };
        if text.is_null() {
            return Err(Error::new(1, "LLVMPrintModuleToString returned null"));
        }

        let output = unsafe { CStr::from_ptr(text) }
            .to_string_lossy()
            .into_owned();
        unsafe {
            LLVMDisposeMessage(text);
        }

        Ok(output)
    }

    fn inspect_json(&self, input: &Path) -> String {
        let (major, minor, patch) = llvm_version();
        let mut json = String::new();
        json.push_str("{\n");
        json.push_str("  \"schemaVersion\": 1,\n");
        push_json_property(&mut json, 1, "toolVersion", env!("CARGO_PKG_VERSION"), true);
        push_json_property(
            &mut json,
            1,
            "llvmVersion",
            &format!("{major}.{minor}.{patch}"),
            true,
        );
        push_json_property(&mut json, 1, "readerKind", "rustlyn-llvm-json", true);
        json.push_str("  \"module\": {\n");
        push_json_property(&mut json, 2, "sourcePath", &input.display().to_string(), true);
        push_functions_json(&mut json, self.module);
        json.push_str(",\n");
        push_aliases_json(&mut json, self.module);
        json.push_str(",\n");
        push_globals_json(&mut json, self.module);
        json.push_str("\n  }\n");
        json.push_str("}\n");
        json
    }

    fn lower_json(&self, input: &Path) -> String {
        let (major, minor, patch) = llvm_version();
        let mut json = String::new();
        json.push_str("{\n");
        json.push_str("  \"schemaVersion\": 1,\n");
        push_json_property(&mut json, 1, "toolVersion", env!("CARGO_PKG_VERSION"), true);
        push_json_property(
            &mut json,
            1,
            "llvmVersion",
            &format!("{major}.{minor}.{patch}"),
            true,
        );
        push_json_property(&mut json, 1, "readerKind", "rustlyn-llvm-lower-json", true);
        json.push_str("  \"module\": {\n");
        push_json_property(&mut json, 2, "sourcePath", &input.display().to_string(), true);
        push_globals_json(&mut json, self.module);
        json.push_str(",\n");
        push_aliases_json(&mut json, self.module);
        json.push_str(",\n");
        push_lower_functions_json(&mut json, self.module);
        json.push_str("\n  }\n");
        json.push_str("}\n");
        json
    }
}

impl Drop for Module {
    fn drop(&mut self) {
        unsafe {
            LLVMDisposeModule(self.module);
        }
    }
}

struct MemoryBuffer {
    buffer: LlvmMemoryBufferRef,
}

impl Drop for MemoryBuffer {
    fn drop(&mut self) {
        unsafe {
            LLVMDisposeMemoryBuffer(self.buffer);
        }
    }
}

unsafe fn consume_message(message: *mut c_char) -> Option<String> {
    if message.is_null() {
        return None;
    }

    let text = CStr::from_ptr(message).to_string_lossy().into_owned();
    LLVMDisposeMessage(message);
    Some(text)
}

fn push_functions_json(json: &mut String, module: LlvmModuleRef) {
    json.push_str("    \"functions\": [");
    let mut first = true;
    let mut function = unsafe { LLVMGetFirstFunction(module) };
    while !function.is_null() {
        let (basic_block_count, instruction_count) = count_function_body(function);
        if basic_block_count > 0 {
            if first {
                json.push('\n');
                first = false;
            } else {
                json.push_str(",\n");
            }

            json.push_str("      { ");
            push_json_string_field(json, "name", &value_name(function));
            json.push_str(&format!(
                ", \"basicBlockCount\": {basic_block_count}, \"instructionCount\": {instruction_count} }}"
            ));
        }

        function = unsafe { LLVMGetNextFunction(function) };
    }

    if !first {
        json.push('\n');
        json.push_str("    ");
    }
    json.push(']');
}

fn push_lower_functions_json(json: &mut String, module: LlvmModuleRef) {
    json.push_str("    \"functions\": [");
    let mut first_function = true;
    let mut function = unsafe { LLVMGetFirstFunction(module) };
    while !function.is_null() {
        let (basic_block_count, _) = count_function_body(function);
        if basic_block_count > 0 {
            if first_function {
                json.push('\n');
                first_function = false;
            } else {
                json.push_str(",\n");
            }

            push_lower_function_json(json, function);
        }

        function = unsafe { LLVMGetNextFunction(function) };
    }

    if !first_function {
        json.push('\n');
        json.push_str("    ");
    }
    json.push(']');
}

fn push_lower_function_json(json: &mut String, function: LlvmValueRef) {
    json.push_str("      {\n");
    push_json_property(json, 4, "name", &value_name(function), true);
    push_json_property(json, 4, "returnType", &function_return_type(function), true);
    push_parameters_json(json, function);
    json.push_str(",\n");
    push_blocks_json(json, function);
    json.push('\n');
    json.push_str("      }");
}

fn push_parameters_json(json: &mut String, function: LlvmValueRef) {
    json.push_str("        \"parameters\": [");
    let parameter_count = unsafe { LLVMCountParams(function) };
    for index in 0..parameter_count {
        let parameter = unsafe { LLVMGetParam(function, index) };
        if index == 0 {
            json.push('\n');
        } else {
            json.push_str(",\n");
        }

        json.push_str("          { ");
        let parameter_name = value_name(parameter);
        let parameter_display_name = if parameter_name.is_empty() {
            format!("arg{index}")
        } else {
            parameter_name
        };
        push_json_string_field(json, "name", &parameter_display_name);
        json.push_str(", ");
        push_json_string_field(json, "type", &type_string(parameter));
        json.push_str(" }");
    }

    if parameter_count > 0 {
        json.push('\n');
        json.push_str("        ");
    }
    json.push(']');
}

fn push_blocks_json(json: &mut String, function: LlvmValueRef) {
    json.push_str("        \"blocks\": [");
    let mut first_block = true;
    let mut block_index = 0;
    let mut block = unsafe { LLVMGetFirstBasicBlock(function) };
    while !block.is_null() {
        if first_block {
            json.push('\n');
            first_block = false;
        } else {
            json.push_str(",\n");
        }

        push_block_json(json, block, block_index);
        block_index += 1;
        block = unsafe { LLVMGetNextBasicBlock(block) };
    }

    if !first_block {
        json.push('\n');
        json.push_str("        ");
    }
    json.push(']');
}

fn push_block_json(json: &mut String, block: LlvmBasicBlockRef, block_index: usize) {
    json.push_str("          {\n");
    push_json_property(json, 6, "name", &block_name(block, block_index), true);
    json.push_str("            \"instructions\": [");

    let mut first_instruction = true;
    let mut instruction = unsafe { LLVMGetFirstInstruction(block) };
    while !instruction.is_null() {
        if first_instruction {
            json.push('\n');
            first_instruction = false;
        } else {
            json.push_str(",\n");
        }

        push_instruction_json(json, instruction);
        instruction = unsafe { LLVMGetNextInstruction(instruction) };
    }

    if !first_instruction {
        json.push('\n');
        json.push_str("            ");
    }
    json.push_str("]\n");
    json.push_str("          }");
}

fn push_instruction_json(json: &mut String, instruction: LlvmValueRef) {
    let opcode = unsafe { LLVMGetInstructionOpcode(instruction) };
    json.push_str("              {\n");
    push_json_property(json, 8, "opcode", opcode_name(opcode), true);
    push_json_property(json, 8, "text", &printed_value(instruction), true);

    let result = value_name(instruction);
    push_json_property(json, 8, "result", &result, true);
    json.push_str("                \"operands\": [");

    let operand_count = unsafe { LLVMGetNumOperands(instruction) };
    for index in 0..operand_count {
        let operand = unsafe { LLVMGetOperand(instruction, index as u32) };
        if index == 0 {
            json.push('\n');
        } else {
            json.push_str(",\n");
        }

        json.push_str("                  { ");
        push_json_string_field(json, "text", &value_name_or_printed(operand));
        json.push_str(", ");
        push_json_string_field(json, "type", &type_string(operand));
        json.push_str(" }");
    }

    if operand_count > 0 {
        json.push('\n');
        json.push_str("                ");
    }
    json.push_str("]\n");
    json.push_str("              }");
}

fn push_aliases_json(json: &mut String, module: LlvmModuleRef) {
    json.push_str("    \"aliases\": [");
    let mut first = true;
    let mut alias = unsafe { LLVMGetFirstGlobalAlias(module) };
    while !alias.is_null() {
        if first {
            json.push('\n');
            first = false;
        } else {
            json.push_str(",\n");
        }

        let aliasee = unsafe { LLVMAliasGetAliasee(alias) };
        json.push_str("      { ");
        push_json_string_field(json, "name", &value_name(alias));
        json.push_str(", ");
        push_json_string_field(json, "target", &value_name_or_printed(aliasee));
        json.push_str(", ");
        push_json_string_field(json, "signature", &type_string(alias));
        json.push_str(" }");

        alias = unsafe { LLVMGetNextGlobalAlias(alias) };
    }

    if !first {
        json.push('\n');
        json.push_str("    ");
    }
    json.push(']');
}

fn push_globals_json(json: &mut String, module: LlvmModuleRef) {
    json.push_str("    \"globals\": [");
    let mut first = true;
    let mut global = unsafe { LLVMGetFirstGlobal(module) };
    while !global.is_null() {
        if first {
            json.push('\n');
            first = false;
        } else {
            json.push_str(",\n");
        }

        json.push_str("      { ");
        push_json_string_field(json, "name", &value_name(global));
        json.push_str(" }");

        global = unsafe { LLVMGetNextGlobal(global) };
    }

    if !first {
        json.push('\n');
        json.push_str("    ");
    }
    json.push(']');
}

fn count_function_body(function: LlvmValueRef) -> (usize, usize) {
    let mut basic_block_count = 0;
    let mut instruction_count = 0;
    let mut block = unsafe { LLVMGetFirstBasicBlock(function) };
    while !block.is_null() {
        basic_block_count += 1;
        let mut instruction = unsafe { LLVMGetFirstInstruction(block) };
        while !instruction.is_null() {
            instruction_count += 1;
            instruction = unsafe { LLVMGetNextInstruction(instruction) };
        }
        block = unsafe { LLVMGetNextBasicBlock(block) };
    }

    (basic_block_count, instruction_count)
}

fn value_name(value: LlvmValueRef) -> String {
    if value.is_null() {
        return String::new();
    }

    let mut length = 0;
    let pointer = unsafe { LLVMGetValueName2(value, &mut length) };
    if pointer.is_null() || length == 0 {
        return String::new();
    }

    let bytes = unsafe { std::slice::from_raw_parts(pointer.cast::<u8>(), length) };
    String::from_utf8_lossy(bytes).into_owned()
}

fn value_name_or_printed(value: LlvmValueRef) -> String {
    let name = value_name(value);
    if !name.is_empty() {
        return name;
    }

    if value.is_null() {
        return String::new();
    }

    let text = unsafe { LLVMPrintValueToString(value) };
    if text.is_null() {
        return String::new();
    }

    let output = unsafe { CStr::from_ptr(text) }.to_string_lossy().into_owned();
    unsafe {
        LLVMDisposeMessage(text);
    }
    output
}

fn printed_value(value: LlvmValueRef) -> String {
    if value.is_null() {
        return String::new();
    }

    let text = unsafe { LLVMPrintValueToString(value) };
    if text.is_null() {
        return String::new();
    }

    let output = unsafe { CStr::from_ptr(text) }.to_string_lossy().into_owned();
    unsafe {
        LLVMDisposeMessage(text);
    }
    output
}

fn type_string(value: LlvmValueRef) -> String {
    if value.is_null() {
        return String::new();
    }

    let ty = unsafe { LLVMTypeOf(value) };
    if ty.is_null() {
        return String::new();
    }

    let text = unsafe { LLVMPrintTypeToString(ty) };
    if text.is_null() {
        return String::new();
    }

    let output = unsafe { CStr::from_ptr(text) }.to_string_lossy().into_owned();
    unsafe {
        LLVMDisposeMessage(text);
    }
    output
}

fn type_to_string(ty: LlvmTypeRef) -> String {
    if ty.is_null() {
        return String::new();
    }

    let text = unsafe { LLVMPrintTypeToString(ty) };
    if text.is_null() {
        return String::new();
    }

    let output = unsafe { CStr::from_ptr(text) }.to_string_lossy().into_owned();
    unsafe {
        LLVMDisposeMessage(text);
    }
    output
}

fn function_return_type(function: LlvmValueRef) -> String {
    if function.is_null() {
        return String::new();
    }

    let function_ty = unsafe { LLVMGlobalGetValueType(function) };
    let return_ty = unsafe { LLVMGetReturnType(function_ty) };
    type_to_string(return_ty)
}

fn block_name(block: LlvmBasicBlockRef, block_index: usize) -> String {
    if block.is_null() {
        return String::new();
    }

    let name = unsafe { LLVMGetBasicBlockName(block) };
    if name.is_null() {
        return default_block_name(block_index);
    }

    let name = unsafe { CStr::from_ptr(name) }.to_string_lossy().into_owned();
    if name.is_empty() {
        default_block_name(block_index)
    } else {
        name
    }
}

fn default_block_name(block_index: usize) -> String {
    if block_index == 0 {
        "start".to_string()
    } else {
        block_index.to_string()
    }
}

fn opcode_name(opcode: u32) -> &'static str {
    match opcode {
        1 => "ret",
        2 => "br",
        3 => "switch",
        8 => "add",
        9 => "fadd",
        10 => "sub",
        11 => "fsub",
        12 => "mul",
        13 => "fmul",
        14 => "udiv",
        15 => "sdiv",
        16 => "fdiv",
        17 => "urem",
        18 => "srem",
        19 => "frem",
        20 => "shl",
        21 => "lshr",
        22 => "ashr",
        23 => "and",
        24 => "or",
        25 => "xor",
        27 => "alloca",
        28 => "load",
        29 => "store",
        30 => "getelementptr",
        38 => "trunc",
        39 => "zext",
        40 => "sext",
        41 => "fptoui",
        42 => "icmp",
        43 => "fcmp",
        44 => "phi",
        45 => "call",
        46 => "select",
        51 => "extractvalue",
        52 => "insertvalue",
        56 => "freeze",
        57 => "atomicrmw",
        58 => "cmpxchg",
        59 => "fence",
        60 => "addrspacecast",
        64 => "ptrtoint",
        65 => "inttoptr",
        67 => "callbr",
        _ => "unknown",
    }
}

fn push_json_property(json: &mut String, indent: usize, name: &str, value: &str, trailing_comma: bool) {
    json.push_str(&"  ".repeat(indent));
    push_json_string_field(json, name, value);
    if trailing_comma {
        json.push(',');
    }
    json.push('\n');
}

fn push_json_string_field(json: &mut String, name: &str, value: &str) {
    json.push('"');
    push_json_escaped(json, name);
    json.push_str("\": \"");
    push_json_escaped(json, value);
    json.push('"');
}

fn push_json_escaped(json: &mut String, value: &str) {
    for character in value.chars() {
        match character {
            '"' => json.push_str("\\\""),
            '\\' => json.push_str("\\\\"),
            '\n' => json.push_str("\\n"),
            '\r' => json.push_str("\\r"),
            '\t' => json.push_str("\\t"),
            character if character.is_control() => {
                json.push_str(&format!("\\u{:04x}", character as u32));
            }
            character => json.push(character),
        }
    }
}

enum PathOrStdout {
    Stdout,
    Path(PathBuf),
}

impl PathOrStdout {
    fn from_arg(value: &str) -> Self {
        if value == "-" {
            Self::Stdout
        } else {
            Self::Path(PathBuf::from(value))
        }
    }
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

    fn usage() -> Self {
        Self::new(
            2,
            "usage: rustlyn-llvm <--version|diagnose|print-ir <input.bc>|inspect-json <input.bc>|lower-json <input.bc> [--disable-verify] [--output <path|->]>",
        )
    }

    fn exit_code(&self) -> i32 {
        self.exit_code
    }
}

impl std::fmt::Display for Error {
    fn fmt(&self, formatter: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        formatter.write_str(&self.message)
    }
}
