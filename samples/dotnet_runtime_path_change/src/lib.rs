unsafe extern "C" {
    fn rust_mcil_dotnet_path_change_extension_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        extension_ptr: *const u8,
        extension_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_change_extension_utf8(
        path_ptr: *const u8,
        path_len: i64,
        extension_ptr: *const u8,
        extension_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_file_name_len(path_ptr: *const u8, path_len: i64) -> i32;
    fn rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_replace_utf8_len(
        source_ptr: *const u8,
        source_len: i64,
        old_ptr: *const u8,
        old_len: i64,
        new_ptr: *const u8,
        new_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_copy_replace_utf8(
        source_ptr: *const u8,
        source_len: i64,
        old_ptr: *const u8,
        old_len: i64,
        new_ptr: *const u8,
        new_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_contains(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_string_index_of(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn dotnet_runtime_path_change_score() -> i32 {
    let path = concat!(env!("CARGO_MANIFEST_DIR"), "\\..\\std_fs\\fixtures\\input.txt");
    let extension = ".data";
    let old = "in";
    let new = "out";
    let needle = "put";

    let changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            path.as_ptr(),
            path.len() as i64,
            extension.as_ptr(),
            extension.len() as i64,
        )
    };
    let mut changed_path = vec![0u8; changed_len as usize];
    let changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            path.as_ptr(),
            path.len() as i64,
            extension.as_ptr(),
            extension.len() as i64,
            changed_path.as_mut_ptr(),
            changed_path.len() as i64,
        )
    };
    let changed_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_len(changed_path.as_ptr(), changed_written as i64)
    };

    let file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            changed_path.as_ptr(),
            changed_written as i64,
        )
    };
    let mut file_name = vec![0u8; file_name_len as usize];
    let file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            changed_path.as_ptr(),
            changed_written as i64,
            file_name.as_mut_ptr(),
            file_name.len() as i64,
        )
    };

    let transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            file_name.as_ptr(),
            file_name_written as i64,
            old.as_ptr(),
            old.len() as i64,
            new.as_ptr(),
            new.len() as i64,
        )
    };
    let mut transformed = vec![0u8; transformed_len as usize];
    let transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            file_name.as_ptr(),
            file_name_written as i64,
            old.as_ptr(),
            old.len() as i64,
            new.as_ptr(),
            new.len() as i64,
            transformed.as_mut_ptr(),
            transformed.len() as i64,
        )
    };
    let contains = unsafe {
        rust_mcil_dotnet_string_contains(
            transformed.as_ptr(),
            transformed_written as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };
    let index = unsafe {
        rust_mcil_dotnet_string_index_of(
            transformed.as_ptr(),
            transformed_written as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };

    changed_file_name_len * 1000 + contains * 100 + transformed_written * 10 + index
}