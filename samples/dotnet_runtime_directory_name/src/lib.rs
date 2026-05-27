unsafe extern "C" {
    fn rustlyn_dotnet_current_directory_utf8_len() -> i32;
    fn rustlyn_dotnet_copy_current_directory_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_combine3_utf8_len(
        first_ptr: *const u8,
        first_len: i64,
        second_ptr: *const u8,
        second_len: i64,
        third_ptr: *const u8,
        third_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_combine3_utf8(
        first_ptr: *const u8,
        first_len: i64,
        second_ptr: *const u8,
        second_len: i64,
        third_ptr: *const u8,
        third_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_get_directory_name_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_directory_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_get_relative_utf8_len(
        relative_to_ptr: *const u8,
        relative_to_len: i64,
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_relative_utf8(
        relative_to_ptr: *const u8,
        relative_to_len: i64,
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_string_replace_utf8_len(
        source_ptr: *const u8,
        source_len: i64,
        old_ptr: *const u8,
        old_len: i64,
        new_ptr: *const u8,
        new_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_string_copy_replace_utf8(
        source_ptr: *const u8,
        source_len: i64,
        old_ptr: *const u8,
        old_len: i64,
        new_ptr: *const u8,
        new_len: i64,
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
pub extern "C" fn dotnet_runtime_directory_name_score() -> i32 {
    let second = "samples";
    let third = "std_fs\\fixtures\\input.txt";
    let old = "fix";
    let new = "trace";
    let needle = "ace";

    let root_len = unsafe { rustlyn_dotnet_current_directory_utf8_len() };
    let mut root = vec![0u8; root_len as usize];
    let root_written = unsafe {
        rustlyn_dotnet_copy_current_directory_utf8(root.as_mut_ptr(), root.len() as i64)
    };

    let full_path_len = unsafe {
        rustlyn_dotnet_path_combine3_utf8_len(
            root.as_ptr(),
            root_written as i64,
            second.as_ptr(),
            second.len() as i64,
            third.as_ptr(),
            third.len() as i64,
        )
    };
    let mut full_path = vec![0u8; full_path_len as usize];
    let full_path_written = unsafe {
        rustlyn_dotnet_path_copy_combine3_utf8(
            root.as_ptr(),
            root_written as i64,
            second.as_ptr(),
            second.len() as i64,
            third.as_ptr(),
            third.len() as i64,
            full_path.as_mut_ptr(),
            full_path.len() as i64,
        )
    };

    let directory_len = unsafe {
        rustlyn_dotnet_path_get_directory_name_utf8_len(
            full_path.as_ptr(),
            full_path_written as i64,
        )
    };
    let mut directory = vec![0u8; directory_len as usize];
    let directory_written = unsafe {
        rustlyn_dotnet_path_copy_directory_name_utf8(
            full_path.as_ptr(),
            full_path_written as i64,
            directory.as_mut_ptr(),
            directory.len() as i64,
        )
    };

    let relative_len = unsafe {
        rustlyn_dotnet_path_get_relative_utf8_len(
            root.as_ptr(),
            root_written as i64,
            directory.as_ptr(),
            directory_written as i64,
        )
    };
    let mut relative = vec![0u8; relative_len as usize];
    let relative_written = unsafe {
        rustlyn_dotnet_path_copy_relative_utf8(
            root.as_ptr(),
            root_written as i64,
            directory.as_ptr(),
            directory_written as i64,
            relative.as_mut_ptr(),
            relative.len() as i64,
        )
    };

    let leaf_len = unsafe {
        rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
            relative.as_ptr(),
            relative_written as i64,
        )
    };
    let mut leaf = vec![0u8; leaf_len as usize];
    let leaf_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
            relative.as_ptr(),
            relative_written as i64,
            leaf.as_mut_ptr(),
            leaf.len() as i64,
        )
    };

    let transformed_len = unsafe {
        rustlyn_dotnet_string_replace_utf8_len(
            leaf.as_ptr(),
            leaf_written as i64,
            old.as_ptr(),
            old.len() as i64,
            new.as_ptr(),
            new.len() as i64,
        )
    };
    let mut transformed = vec![0u8; transformed_len as usize];
    let transformed_written = unsafe {
        rustlyn_dotnet_string_copy_replace_utf8(
            leaf.as_ptr(),
            leaf_written as i64,
            old.as_ptr(),
            old.len() as i64,
            new.as_ptr(),
            new.len() as i64,
            transformed.as_mut_ptr(),
            transformed.len() as i64,
        )
    };
    let contains = unsafe {
        rustlyn_dotnet_string_contains(
            transformed.as_ptr(),
            transformed_written as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };
    let index = unsafe {
        rustlyn_dotnet_string_index_of(
            transformed.as_ptr(),
            transformed_written as i64,
            needle.as_ptr(),
            needle.len() as i64,
        )
    };

    relative_written * 10000 + leaf_written * 1000 + transformed_written * 100 + contains * 10 + index
}