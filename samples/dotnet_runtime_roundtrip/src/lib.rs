unsafe extern "C" {
    fn rustlyn_dotnet_path_get_file_name_utf8_len(path_ptr: *const u8, path_len: i64) -> i32;
    fn rustlyn_dotnet_path_copy_file_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
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
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_roundtrip_score() -> i32 {
    let path = concat!(env!("CARGO_MANIFEST_DIR"), "\\..\\std_fs\\fixtures\\input.txt");
    let needle = "txt";
    let required_len =
        unsafe { rustlyn_dotnet_path_get_file_name_utf8_len(path.as_ptr(), path.len() as i64) };
    let mut file_name = vec![0u8; required_len as usize];
    let written = unsafe {
        rustlyn_dotnet_path_copy_file_name_utf8(
            path.as_ptr(),
            path.len() as i64,
            file_name.as_mut_ptr(),
            file_name.len() as i64,
        )
    };
    let contains = unsafe {
        rustlyn_dotnet_string_contains(
            file_name.as_ptr(),
            written as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };
    let index = unsafe {
        rustlyn_dotnet_string_index_of(
            file_name.as_ptr(),
            written as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };

    contains * 100 + written * 10 + index
}