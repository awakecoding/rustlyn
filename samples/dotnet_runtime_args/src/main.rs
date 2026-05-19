#![no_main]

unsafe extern "C" {
    fn rust_mcil_dotnet_command_line_arg_count() -> i32;
    fn rust_mcil_dotnet_command_line_arg_utf8_len(index: i32) -> i32;
    fn rust_mcil_dotnet_copy_command_line_arg_utf8(
        index: i32,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_contains(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_console_write_line_utf8(value_ptr: *const u8, value_len: i64);
}

fn load_arg(index: i32) -> Vec<u8> {
    let arg_len = unsafe { rust_mcil_dotnet_command_line_arg_utf8_len(index) };
    let mut arg = vec![0u8; arg_len as usize];
    let written = unsafe {
        rust_mcil_dotnet_copy_command_line_arg_utf8(index, arg.as_mut_ptr(), arg.len() as i64)
    };
    arg.truncate(written as usize);
    arg
}

#[unsafe(no_mangle)]
pub extern "C" fn main() -> i32 {
    let count = unsafe { rust_mcil_dotnet_command_line_arg_count() };
    if count < 3 {
        return 1;
    }

    let candidate = load_arg(1);
    let needle = load_arg(2);
    let contains = unsafe {
        rust_mcil_dotnet_string_contains(
            candidate.as_ptr(),
            candidate.len() as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };

    if contains != 0 {
        unsafe {
            rust_mcil_dotnet_console_write_line_utf8(candidate.as_ptr(), candidate.len() as i64);
        }
    }

    0
}