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
    fn rustlyn_dotnet_path_get_full_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        base_ptr: *const u8,
        base_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_full_utf8(
        path_ptr: *const u8,
        path_len: i64,
        base_ptr: *const u8,
        base_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_get_root_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_root_utf8(
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
    fn rustlyn_dotnet_path_get_file_name_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_path_copy_file_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
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
pub extern "C" fn dotnet_runtime_path_cross_root_score() -> i32 {
    let current_relative_input = "samples\\std_fs\\fixtures\\..\\fixtures\\input.txt";
    let temp_second = "managed";
    let temp_third = "output.bin";
    let current_segment = ".";
    let extension = ".data";
    let old = "out";
    let new = "trace";
    let needle = "ace";

    let current_base_len = unsafe { rustlyn_dotnet_current_directory_utf8_len() };
    let mut current_base = vec![0u8; current_base_len as usize];
    let current_base_written = unsafe {
        rustlyn_dotnet_copy_current_directory_utf8(
            current_base.as_mut_ptr(),
            current_base.len() as i64,
        )
    };

    let current_full_len = unsafe {
        rustlyn_dotnet_path_get_full_utf8_len(
            current_relative_input.as_ptr(),
            current_relative_input.len() as i64,
            current_base.as_ptr(),
            current_base_written as i64,
        )
    };
    let mut current_full = vec![0u8; current_full_len as usize];
    let current_full_written = unsafe {
        rustlyn_dotnet_path_copy_full_utf8(
            current_relative_input.as_ptr(),
            current_relative_input.len() as i64,
            current_base.as_ptr(),
            current_base_written as i64,
            current_full.as_mut_ptr(),
            current_full.len() as i64,
        )
    };

    let current_root_len = unsafe {
        rustlyn_dotnet_path_get_root_utf8_len(current_full.as_ptr(), current_full_written as i64)
    };
    let mut current_root = vec![0u8; current_root_len as usize];
    let current_root_written = unsafe {
        rustlyn_dotnet_path_copy_root_utf8(
            current_full.as_ptr(),
            current_full_written as i64,
            current_root.as_mut_ptr(),
            current_root.len() as i64,
        )
    };

    let current_relative_len = unsafe {
        rustlyn_dotnet_path_get_relative_utf8_len(
            current_root.as_ptr(),
            current_root_written as i64,
            current_full.as_ptr(),
            current_full_written as i64,
        )
    };
    let mut current_relative = vec![0u8; current_relative_len as usize];
    let current_relative_written = unsafe {
        rustlyn_dotnet_path_copy_relative_utf8(
            current_root.as_ptr(),
            current_root_written as i64,
            current_full.as_ptr(),
            current_full_written as i64,
            current_relative.as_mut_ptr(),
            current_relative.len() as i64,
        )
    };

    let current_directory_len = unsafe {
        rustlyn_dotnet_path_get_directory_name_utf8_len(
            current_relative.as_ptr(),
            current_relative_written as i64,
        )
    };
    let mut current_directory = vec![0u8; current_directory_len as usize];
    let current_directory_written = unsafe {
        rustlyn_dotnet_path_copy_directory_name_utf8(
            current_relative.as_ptr(),
            current_relative_written as i64,
            current_directory.as_mut_ptr(),
            current_directory.len() as i64,
        )
    };

    let temp_root_len = unsafe { rustlyn_dotnet_temp_path_utf8_len() };
    let mut temp_root = vec![0u8; temp_root_len as usize];
    let temp_root_written = unsafe {
        rustlyn_dotnet_copy_temp_path_utf8(temp_root.as_mut_ptr(), temp_root.len() as i64)
    };
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

    let temp_file_name_len = unsafe {
        rustlyn_dotnet_path_get_file_name_utf8_len(temp_combined.as_ptr(), temp_combined_written as i64)
    };
    let mut temp_file_name = vec![0u8; temp_file_name_len as usize];
    let temp_file_name_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_utf8(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
            temp_file_name.as_mut_ptr(),
            temp_file_name.len() as i64,
        )
    };

    let changed_file_name_len = unsafe {
        rustlyn_dotnet_path_change_extension_utf8_len(
            temp_file_name.as_ptr(),
            temp_file_name_written as i64,
            extension.as_ptr(),
            extension.len() as i64,
        )
    };
    let mut changed_file_name = vec![0u8; changed_file_name_len as usize];
    let changed_file_name_written = unsafe {
        rustlyn_dotnet_path_copy_change_extension_utf8(
            temp_file_name.as_ptr(),
            temp_file_name_written as i64,
            extension.as_ptr(),
            extension.len() as i64,
            changed_file_name.as_mut_ptr(),
            changed_file_name.len() as i64,
        )
    };

    let merged_relative_len = unsafe {
        rustlyn_dotnet_path_combine3_utf8_len(
            current_directory.as_ptr(),
            current_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            changed_file_name.as_ptr(),
            changed_file_name_written as i64,
        )
    };
    let mut merged_relative = vec![0u8; merged_relative_len as usize];
    let merged_relative_written = unsafe {
        rustlyn_dotnet_path_copy_combine3_utf8(
            current_directory.as_ptr(),
            current_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            changed_file_name.as_ptr(),
            changed_file_name_written as i64,
            merged_relative.as_mut_ptr(),
            merged_relative.len() as i64,
        )
    };

    let merged_full_len = unsafe {
        rustlyn_dotnet_path_get_full_utf8_len(
            merged_relative.as_ptr(),
            merged_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
        )
    };
    let mut merged_full = vec![0u8; merged_full_len as usize];
    let merged_full_written = unsafe {
        rustlyn_dotnet_path_copy_full_utf8(
            merged_relative.as_ptr(),
            merged_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
            merged_full.as_mut_ptr(),
            merged_full.len() as i64,
        )
    };

    let merged_leaf_len = unsafe {
        rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
            merged_full.as_ptr(),
            merged_full_written as i64,
        )
    };
    let mut merged_leaf = vec![0u8; merged_leaf_len as usize];
    let merged_leaf_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
            merged_full.as_ptr(),
            merged_full_written as i64,
            merged_leaf.as_mut_ptr(),
            merged_leaf.len() as i64,
        )
    };

    let transformed_len = unsafe {
        rustlyn_dotnet_string_replace_utf8_len(
            merged_leaf.as_ptr(),
            merged_leaf_written as i64,
            old.as_ptr(),
            old.len() as i64,
            new.as_ptr(),
            new.len() as i64,
        )
    };
    let mut transformed = vec![0u8; transformed_len as usize];
    let transformed_written = unsafe {
        rustlyn_dotnet_string_copy_replace_utf8(
            merged_leaf.as_ptr(),
            merged_leaf_written as i64,
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

    merged_full_written * 1000000
        + changed_file_name_written * 10000
        + merged_leaf_written * 1000
        + transformed_written * 100
        + contains * 10
        + index
}