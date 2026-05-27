unsafe extern "C" {
    fn rustlyn_dotnet_is_windows() -> i32;
    fn rustlyn_dotnet_directory_separator_char() -> i32;
    fn rustlyn_dotnet_path_separator_char() -> i32;
    fn rustlyn_dotnet_newline_len() -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_score() -> i32 {
    let os_score = if unsafe { rustlyn_dotnet_is_windows() } != 0 {
        100
    } else {
        -100
    };

    os_score
        + unsafe { rustlyn_dotnet_directory_separator_char() }
        + unsafe { rustlyn_dotnet_path_separator_char() }
        + unsafe { rustlyn_dotnet_newline_len() }
}