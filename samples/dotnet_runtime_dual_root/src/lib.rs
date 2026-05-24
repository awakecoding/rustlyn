unsafe extern "C" {
    fn rustlyn_dotnet_current_directory_utf8_len() -> i32;
    fn rustlyn_dotnet_copy_current_directory_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_temp_path_utf8_len() -> i32;
    fn rustlyn_dotnet_copy_temp_path_utf8(
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
    fn rustlyn_dotnet_path_change_extension_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        extension_ptr: *const u8,
        extension_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_change_extension_utf8(
        path_ptr: *const u8,
        path_len: i64,
        extension_ptr: *const u8,
        extension_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_get_file_name_len(path_ptr: *const u8, path_len: i64) -> i32;
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
pub extern "C" fn dotnet_runtime_dual_root_score() -> i32 {
    let current_second = "samples";
    let current_third = "std_fs\\fixtures\\input.txt";
    let current_extension = ".data";
    let temp_second = "managed";
    let temp_third = "output.bin";
    let old = "out";
    let new = "trace";
    let needle = "put";

    let current_root_len = unsafe { rustlyn_dotnet_current_directory_utf8_len() };
    let mut current_root = vec![0u8; current_root_len as usize];
    let current_root_written = unsafe {
        rustlyn_dotnet_copy_current_directory_utf8(
            current_root.as_mut_ptr(),
            current_root.len() as i64,
        )
    };

    let current_combined_len = unsafe {
        rustlyn_dotnet_path_combine3_utf8_len(
            current_root.as_ptr(),
            current_root_written as i64,
            current_second.as_ptr(),
            current_second.len() as i64,
            current_third.as_ptr(),
            current_third.len() as i64,
        )
    };
    let mut current_combined = vec![0u8; current_combined_len as usize];
    let current_combined_written = unsafe {
        rustlyn_dotnet_path_copy_combine3_utf8(
            current_root.as_ptr(),
            current_root_written as i64,
            current_second.as_ptr(),
            current_second.len() as i64,
            current_third.as_ptr(),
            current_third.len() as i64,
            current_combined.as_mut_ptr(),
            current_combined.len() as i64,
        )
    };
    let changed_len = unsafe {
        rustlyn_dotnet_path_change_extension_utf8_len(
            current_combined.as_ptr(),
            current_combined_written as i64,
            current_extension.as_ptr(),
            current_extension.len() as i64,
        )
    };
    let mut changed_path = vec![0u8; changed_len as usize];
    let changed_written = unsafe {
        rustlyn_dotnet_path_copy_change_extension_utf8(
            current_combined.as_ptr(),
            current_combined_written as i64,
            current_extension.as_ptr(),
            current_extension.len() as i64,
            changed_path.as_mut_ptr(),
            changed_path.len() as i64,
        )
    };
    let changed_file_name_len = unsafe {
        rustlyn_dotnet_path_get_file_name_len(changed_path.as_ptr(), changed_written as i64)
    };
    let current_file_name_len = unsafe {
        rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
            changed_path.as_ptr(),
            changed_written as i64,
        )
    };
    let mut current_file_name = vec![0u8; current_file_name_len as usize];
    let current_file_name_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
            changed_path.as_ptr(),
            changed_written as i64,
            current_file_name.as_mut_ptr(),
            current_file_name.len() as i64,
        )
    };

    let temp_root_len = unsafe { rustlyn_dotnet_temp_path_utf8_len() };
    let mut temp_root = vec![0u8; temp_root_len as usize];
    let temp_root_written =
        unsafe { rustlyn_dotnet_copy_temp_path_utf8(temp_root.as_mut_ptr(), temp_root.len() as i64) };
    let temp_combined_len = unsafe {
        rustlyn_dotnet_path_combine3_utf8_len(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_second.as_ptr(),
            temp_second.len() as i64,
            temp_third.as_ptr(),
            temp_third.len() as i64,
        )
    };
    let mut temp_combined = vec![0u8; temp_combined_len as usize];
    let temp_combined_written = unsafe {
        rustlyn_dotnet_path_copy_combine3_utf8(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_second.as_ptr(),
            temp_second.len() as i64,
            temp_third.as_ptr(),
            temp_third.len() as i64,
            temp_combined.as_mut_ptr(),
            temp_combined.len() as i64,
        )
    };
    let temp_file_name_len =
        unsafe { rustlyn_dotnet_path_get_file_name_len(temp_combined.as_ptr(), temp_combined_written as i64) };
    let temp_base_len = unsafe {
        rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
        )
    };
    let mut temp_base = vec![0u8; temp_base_len as usize];
    let temp_base_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
            temp_base.as_mut_ptr(),
            temp_base.len() as i64,
        )
    };

    let transformed_len = unsafe {
        rustlyn_dotnet_string_replace_utf8_len(
            temp_base.as_ptr(),
            temp_base_written as i64,
            old.as_ptr(),
            old.len() as i64,
            new.as_ptr(),
            new.len() as i64,
        )
    };
    let mut transformed = vec![0u8; transformed_len as usize];
    let transformed_written = unsafe {
        rustlyn_dotnet_string_copy_replace_utf8(
            temp_base.as_ptr(),
            temp_base_written as i64,
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

    changed_file_name_len * 1000
        + temp_file_name_len * 100
        + current_file_name_written * 10
        + transformed_written * 10
        + contains * 10
        + index
}