unsafe extern "C" {
    fn rust_mcil_dotnet_is_windows() -> i32;
    fn rust_mcil_dotnet_directory_separator_char() -> i32;
    fn rust_mcil_dotnet_path_separator_char() -> i32;
    fn rust_mcil_dotnet_newline_len() -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_score() -> i32 {
    let os_score = if unsafe { rust_mcil_dotnet_is_windows() } != 0 {
        100
    } else {
        -100
    };

    os_score
        + unsafe { rust_mcil_dotnet_directory_separator_char() }
        + unsafe { rust_mcil_dotnet_path_separator_char() }
        + unsafe { rust_mcil_dotnet_newline_len() }
}