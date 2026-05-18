unsafe extern "C" {
    fn rust_mcil_dotnet_current_directory_utf8_len() -> i32;
    fn rust_mcil_dotnet_copy_current_directory_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_temp_path_utf8_len() -> i32;
    fn rust_mcil_dotnet_copy_temp_path_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_documents_utf8_len() -> i32;
    fn rust_mcil_dotnet_copy_documents_utf8(
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_full_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        base_ptr: *const u8,
        base_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_full_utf8(
        path_ptr: *const u8,
        path_len: i64,
        base_ptr: *const u8,
        base_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_root_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_root_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_relative_utf8_len(
        relative_to_ptr: *const u8,
        relative_to_len: i64,
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_relative_utf8(
        relative_to_ptr: *const u8,
        relative_to_len: i64,
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_directory_name_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_directory_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_combine3_utf8_len(
        first_ptr: *const u8,
        first_len: i64,
        second_ptr: *const u8,
        second_len: i64,
        third_ptr: *const u8,
        third_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_combine3_utf8(
        first_ptr: *const u8,
        first_len: i64,
        second_ptr: *const u8,
        second_len: i64,
        third_ptr: *const u8,
        third_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_get_file_name_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
    ) -> i32;
    fn rust_mcil_dotnet_path_copy_file_name_utf8(
        path_ptr: *const u8,
        path_len: i64,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
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
pub extern "C" fn dotnet_runtime_path_double_select_score() -> i32 {
    let current_relative_input = "samples\\std_fs\\fixtures\\..\\fixtures\\input.txt";
    let current_extension = ".data";
    let current_old = "in";
    let current_new = "trace";

    let temp_second = "managed";
    let temp_third = "output.bin";
    let temp_extension = ".data";
    let temp_old = "out";
    let temp_new = "trace";

    let documents_second = "archive";
    let documents_third = "notes.log";
    let documents_extension = ".memo";
    let documents_old = "ot";
    let documents_new = "ace";
    let documents_needle = "ace";

    let current_segment = ".";
    let final_old = "out";
    let final_new = "trace";
    let final_needle = "ace";

    let current_base_len = unsafe { rust_mcil_dotnet_current_directory_utf8_len() };
    let mut current_base = vec![0u8; current_base_len as usize];
    let current_base_written = unsafe {
        rust_mcil_dotnet_copy_current_directory_utf8(
            current_base.as_mut_ptr(),
            current_base.len() as i64,
        )
    };

    let current_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            current_relative_input.as_ptr(),
            current_relative_input.len() as i64,
            current_base.as_ptr(),
            current_base_written as i64,
        )
    };
    let mut current_full = vec![0u8; current_full_len as usize];
    let current_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            current_relative_input.as_ptr(),
            current_relative_input.len() as i64,
            current_base.as_ptr(),
            current_base_written as i64,
            current_full.as_mut_ptr(),
            current_full.len() as i64,
        )
    };

    let current_root_len = unsafe {
        rust_mcil_dotnet_path_get_root_utf8_len(current_full.as_ptr(), current_full_written as i64)
    };
    let mut current_root = vec![0u8; current_root_len as usize];
    let current_root_written = unsafe {
        rust_mcil_dotnet_path_copy_root_utf8(
            current_full.as_ptr(),
            current_full_written as i64,
            current_root.as_mut_ptr(),
            current_root.len() as i64,
        )
    };

    let current_relative_len = unsafe {
        rust_mcil_dotnet_path_get_relative_utf8_len(
            current_root.as_ptr(),
            current_root_written as i64,
            current_full.as_ptr(),
            current_full_written as i64,
        )
    };
    let mut current_relative = vec![0u8; current_relative_len as usize];
    let current_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_relative_utf8(
            current_root.as_ptr(),
            current_root_written as i64,
            current_full.as_ptr(),
            current_full_written as i64,
            current_relative.as_mut_ptr(),
            current_relative.len() as i64,
        )
    };

    let current_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            current_relative.as_ptr(),
            current_relative_written as i64,
        )
    };
    let mut current_directory = vec![0u8; current_directory_len as usize];
    let current_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            current_relative.as_ptr(),
            current_relative_written as i64,
            current_directory.as_mut_ptr(),
            current_directory.len() as i64,
        )
    };

    let current_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            current_relative.as_ptr(),
            current_relative_written as i64,
        )
    };
    let mut current_file_name = vec![0u8; current_file_name_len as usize];
    let current_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            current_relative.as_ptr(),
            current_relative_written as i64,
            current_file_name.as_mut_ptr(),
            current_file_name.len() as i64,
        )
    };
    let current_changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            current_file_name.as_ptr(),
            current_file_name_written as i64,
            current_extension.as_ptr(),
            current_extension.len() as i64,
        )
    };
    let mut current_changed = vec![0u8; current_changed_len as usize];
    let current_changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            current_file_name.as_ptr(),
            current_file_name_written as i64,
            current_extension.as_ptr(),
            current_extension.len() as i64,
            current_changed.as_mut_ptr(),
            current_changed.len() as i64,
        )
    };
    let current_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            current_changed.as_ptr(),
            current_changed_written as i64,
        )
    };
    let mut current_leaf = vec![0u8; current_leaf_len as usize];
    let current_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            current_changed.as_ptr(),
            current_changed_written as i64,
            current_leaf.as_mut_ptr(),
            current_leaf.len() as i64,
        )
    };
    let current_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            current_leaf.as_ptr(),
            current_leaf_written as i64,
            current_old.as_ptr(),
            current_old.len() as i64,
            current_new.as_ptr(),
            current_new.len() as i64,
        )
    };
    let mut current_transformed = vec![0u8; current_transformed_len as usize];
    let current_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            current_leaf.as_ptr(),
            current_leaf_written as i64,
            current_old.as_ptr(),
            current_old.len() as i64,
            current_new.as_ptr(),
            current_new.len() as i64,
            current_transformed.as_mut_ptr(),
            current_transformed.len() as i64,
        )
    };
    let current_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            current_transformed.as_ptr(),
            current_transformed_written as i64,
            final_needle.as_ptr(),
            final_needle.len() as i64,
        )
    };

    let temp_root_len = unsafe { rust_mcil_dotnet_temp_path_utf8_len() };
    let mut temp_root = vec![0u8; temp_root_len as usize];
    let temp_root_written = unsafe {
        rust_mcil_dotnet_copy_temp_path_utf8(temp_root.as_mut_ptr(), temp_root.len() as i64)
    };
    let temp_combined_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
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
        rust_mcil_dotnet_path_copy_combine3_utf8(
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
    let temp_changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
            temp_extension.as_ptr(),
            temp_extension.len() as i64,
        )
    };
    let mut temp_changed = vec![0u8; temp_changed_len as usize];
    let temp_changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            temp_combined.as_ptr(),
            temp_combined_written as i64,
            temp_extension.as_ptr(),
            temp_extension.len() as i64,
            temp_changed.as_mut_ptr(),
            temp_changed.len() as i64,
        )
    };
    let temp_relative_len = unsafe {
        rust_mcil_dotnet_path_get_relative_utf8_len(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_changed.as_ptr(),
            temp_changed_written as i64,
        )
    };
    let mut temp_relative = vec![0u8; temp_relative_len as usize];
    let temp_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_relative_utf8(
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_changed.as_ptr(),
            temp_changed_written as i64,
            temp_relative.as_mut_ptr(),
            temp_relative.len() as i64,
        )
    };
    let temp_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            temp_relative.as_ptr(),
            temp_relative_written as i64,
        )
    };
    let mut temp_directory = vec![0u8; temp_directory_len as usize];
    let temp_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            temp_relative.as_ptr(),
            temp_relative_written as i64,
            temp_directory.as_mut_ptr(),
            temp_directory.len() as i64,
        )
    };
    let temp_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(temp_changed.as_ptr(), temp_changed_written as i64)
    };
    let mut temp_file_name = vec![0u8; temp_file_name_len as usize];
    let temp_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            temp_changed.as_ptr(),
            temp_changed_written as i64,
            temp_file_name.as_mut_ptr(),
            temp_file_name.len() as i64,
        )
    };
    let temp_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            temp_file_name.as_ptr(),
            temp_file_name_written as i64,
        )
    };
    let mut temp_leaf = vec![0u8; temp_leaf_len as usize];
    let temp_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            temp_file_name.as_ptr(),
            temp_file_name_written as i64,
            temp_leaf.as_mut_ptr(),
            temp_leaf.len() as i64,
        )
    };
    let temp_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            temp_leaf.as_ptr(),
            temp_leaf_written as i64,
            temp_old.as_ptr(),
            temp_old.len() as i64,
            temp_new.as_ptr(),
            temp_new.len() as i64,
        )
    };
    let mut temp_transformed = vec![0u8; temp_transformed_len as usize];
    let temp_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            temp_leaf.as_ptr(),
            temp_leaf_written as i64,
            temp_old.as_ptr(),
            temp_old.len() as i64,
            temp_new.as_ptr(),
            temp_new.len() as i64,
            temp_transformed.as_mut_ptr(),
            temp_transformed.len() as i64,
        )
    };
    let temp_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            temp_transformed.as_ptr(),
            temp_transformed_written as i64,
            final_needle.as_ptr(),
            final_needle.len() as i64,
        )
    };

    let documents_root_len = unsafe { rust_mcil_dotnet_documents_utf8_len() };
    let mut documents_root = vec![0u8; documents_root_len as usize];
    let documents_root_written = unsafe {
        rust_mcil_dotnet_copy_documents_utf8(
            documents_root.as_mut_ptr(),
            documents_root.len() as i64,
        )
    };
    let documents_combined_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_second.as_ptr(),
            documents_second.len() as i64,
            documents_third.as_ptr(),
            documents_third.len() as i64,
        )
    };
    let mut documents_combined = vec![0u8; documents_combined_len as usize];
    let documents_combined_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_second.as_ptr(),
            documents_second.len() as i64,
            documents_third.as_ptr(),
            documents_third.len() as i64,
            documents_combined.as_mut_ptr(),
            documents_combined.len() as i64,
        )
    };
    let documents_changed_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            documents_combined.as_ptr(),
            documents_combined_written as i64,
            documents_extension.as_ptr(),
            documents_extension.len() as i64,
        )
    };
    let mut documents_changed = vec![0u8; documents_changed_len as usize];
    let documents_changed_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            documents_combined.as_ptr(),
            documents_combined_written as i64,
            documents_extension.as_ptr(),
            documents_extension.len() as i64,
            documents_changed.as_mut_ptr(),
            documents_changed.len() as i64,
        )
    };
    let documents_relative_len = unsafe {
        rust_mcil_dotnet_path_get_relative_utf8_len(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_changed.as_ptr(),
            documents_changed_written as i64,
        )
    };
    let mut documents_relative = vec![0u8; documents_relative_len as usize];
    let documents_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_relative_utf8(
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_changed.as_ptr(),
            documents_changed_written as i64,
            documents_relative.as_mut_ptr(),
            documents_relative.len() as i64,
        )
    };
    let documents_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            documents_relative.as_ptr(),
            documents_relative_written as i64,
        )
    };
    let mut documents_directory = vec![0u8; documents_directory_len as usize];
    let documents_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            documents_relative.as_ptr(),
            documents_relative_written as i64,
            documents_directory.as_mut_ptr(),
            documents_directory.len() as i64,
        )
    };
    let documents_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            documents_changed.as_ptr(),
            documents_changed_written as i64,
        )
    };
    let mut documents_file_name = vec![0u8; documents_file_name_len as usize];
    let documents_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            documents_changed.as_ptr(),
            documents_changed_written as i64,
            documents_file_name.as_mut_ptr(),
            documents_file_name.len() as i64,
        )
    };
    let documents_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            documents_file_name.as_ptr(),
            documents_file_name_written as i64,
        )
    };
    let mut documents_leaf = vec![0u8; documents_leaf_len as usize];
    let documents_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            documents_file_name.as_ptr(),
            documents_file_name_written as i64,
            documents_leaf.as_mut_ptr(),
            documents_leaf.len() as i64,
        )
    };
    let documents_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            documents_leaf.as_ptr(),
            documents_leaf_written as i64,
            documents_old.as_ptr(),
            documents_old.len() as i64,
            documents_new.as_ptr(),
            documents_new.len() as i64,
        )
    };
    let mut documents_transformed = vec![0u8; documents_transformed_len as usize];
    let documents_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            documents_leaf.as_ptr(),
            documents_leaf_written as i64,
            documents_old.as_ptr(),
            documents_old.len() as i64,
            documents_new.as_ptr(),
            documents_new.len() as i64,
            documents_transformed.as_mut_ptr(),
            documents_transformed.len() as i64,
        )
    };
    let documents_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            documents_transformed.as_ptr(),
            documents_transformed_written as i64,
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
        )
    };

    let (selected_directory_ptr, selected_directory_written, directory_score) =
        if documents_contains != 0 && temp_index == current_index {
            (
                documents_directory.as_ptr(),
                documents_directory_written,
                3,
            )
        } else if temp_index > current_index {
            (temp_directory.as_ptr(), temp_directory_written, 2)
        } else {
            (current_directory.as_ptr(), current_directory_written, 1)
        };

    let (selected_file_name_ptr, selected_file_name_written, file_score) =
        if current_index == temp_index && documents_contains != 0 {
            (temp_file_name.as_ptr(), temp_file_name_written, 2)
        } else if current_index > temp_index {
            (current_changed.as_ptr(), current_changed_written, 1)
        } else {
            (documents_file_name.as_ptr(), documents_file_name_written, 3)
        };

    let merged_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            selected_directory_ptr,
            selected_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_file_name_ptr,
            selected_file_name_written as i64,
        )
    };
    let mut merged_relative = vec![0u8; merged_relative_len as usize];
    let merged_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            selected_directory_ptr,
            selected_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_file_name_ptr,
            selected_file_name_written as i64,
            merged_relative.as_mut_ptr(),
            merged_relative.len() as i64,
        )
    };

    let merged_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            merged_relative.as_ptr(),
            merged_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
        )
    };
    let mut merged_full = vec![0u8; merged_full_len as usize];
    let merged_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            merged_relative.as_ptr(),
            merged_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
            merged_full.as_mut_ptr(),
            merged_full.len() as i64,
        )
    };

    let merged_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            merged_full.as_ptr(),
            merged_full_written as i64,
        )
    };
    let mut merged_leaf = vec![0u8; merged_leaf_len as usize];
    let merged_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            merged_full.as_ptr(),
            merged_full_written as i64,
            merged_leaf.as_mut_ptr(),
            merged_leaf.len() as i64,
        )
    };

    let final_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            merged_leaf.as_ptr(),
            merged_leaf_written as i64,
            final_old.as_ptr(),
            final_old.len() as i64,
            final_new.as_ptr(),
            final_new.len() as i64,
        )
    };
    let mut final_transformed = vec![0u8; final_transformed_len as usize];
    let final_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            merged_leaf.as_ptr(),
            merged_leaf_written as i64,
            final_old.as_ptr(),
            final_old.len() as i64,
            final_new.as_ptr(),
            final_new.len() as i64,
            final_transformed.as_mut_ptr(),
            final_transformed.len() as i64,
        )
    };
    let final_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            final_needle.as_ptr(),
            final_needle.len() as i64,
        )
    };
    let final_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            final_needle.as_ptr(),
            final_needle.len() as i64,
        )
    };

    directory_score * 100000000
        + file_score * 10000000
        + merged_full_written * 1000000
        + merged_leaf_written * 1000
        + final_transformed_written * 100
        + final_contains * 10
        + final_index
}