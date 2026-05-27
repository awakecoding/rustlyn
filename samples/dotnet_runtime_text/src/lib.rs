unsafe extern "C" {
    fn rustlyn_dotnet_string_contains(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_string_index_of(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_get_file_name_len(path_ptr: *const u8, path_len: i64) -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_text_score() -> i32 {
    let phrase = "managed-runtime-interop";
    let needle = "runtime";
    let path = concat!(env!("CARGO_MANIFEST_DIR"), "\\..\\std_fs\\fixtures\\input.txt");

    let contains = unsafe {
        rustlyn_dotnet_string_contains(
            phrase.as_ptr(),
            phrase.len() as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };
    let index = unsafe {
        rustlyn_dotnet_string_index_of(
            phrase.as_ptr(),
            phrase.len() as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };
    let file_name_len =
        unsafe { rustlyn_dotnet_path_get_file_name_len(path.as_ptr(), path.len() as i64) };

    contains * 100 + index * 10 + file_name_len
}