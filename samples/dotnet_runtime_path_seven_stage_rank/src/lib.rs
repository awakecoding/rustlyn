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
pub extern "C" fn dotnet_runtime_path_seven_stage_rank_score() -> i32 {
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

    let trace_old = "out";
    let trace_new = "trace";
    let trace_extension = ".trace";
    let trace_needle = "trace";

    let grid_old = "ot";
    let grid_new = "ace";
    let grid_extension = ".grid";
    let grid_needle = "ace";

    let flow_old = "in";
    let flow_new = "path";
    let flow_extension = ".flow";
    let flow_needle = "path";

    let current_segment = ".";
    let current_stage_needle = "fixtures";
    let temp_stage_needle = "managed";
    let documents_stage_needle = "archive";
    let mesh_old = "ou";
    let mesh_new = "mesh";
    let mesh_needle = "mesh";
    let route_old = "in";
    let route_new = "route";
    let route_needle = "route";
    let array_old = "ot";
    let array_new = "array";
    let array_needle = "array";
    let spine_old = "fixtures";
    let spine_new = "spine";
    let spine_needle = "spine";
    let weave_old = "managed";
    let weave_new = "weave";
    let weave_needle = "weave";
    let orbit_old = "archive";
    let orbit_new = "orbit";
    let orbit_needle = "orbit";
    let self_stage_needle = "path";
    let temp_stage_six_needle = "managed";
    let documents_stage_six_needle = "archive";
    let final_old = "ou";
    let final_new = "path";
    let final_needle = "path";
    let crown_old = "path";
    let crown_new = "crown";
    let crown_needle = "crown";
    let pulse_extension = ".pulse";
    let pulse_needle = "pulse";
    let anchor_old = "path";
    let anchor_new = "anchor";
    let anchor_needle = "anchor";

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
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
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
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
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

    let mut selected_directory_index = 0i32;
    let mut selected_file_index = 0i32;
    let mut best_pair_score = i32::MIN;
    let mut directory_index = 0i32;
    while directory_index < 3 {
        let mut candidate_directory_rank = current_transformed_written * 10 + current_index;
        if directory_index == 1 {
            candidate_directory_rank = temp_transformed_written * 10 + temp_index;
        } else if directory_index == 2 {
            candidate_directory_rank = documents_contains * 100 + documents_transformed_written;
        }

        let mut file_index = 0i32;
        while file_index < 3 {
            let mut candidate_file_rank = current_changed_written * 10 + current_index;
            if file_index == 1 {
                candidate_file_rank = temp_file_name_written * 10 + temp_index;
            } else if file_index == 2 {
                candidate_file_rank = documents_file_name_written * 10 + documents_contains;
            }

            let mut pair_bonus = 0i32;
            if directory_index == file_index {
                pair_bonus += 200;
            }
            if directory_index == 2 && documents_contains != 0 {
                pair_bonus += 30;
            }
            if file_index == 1 && temp_index >= current_index {
                pair_bonus += 20;
            }
            if file_index == 0 && current_index >= 0 {
                pair_bonus += 10;
            }

            let pair_score = candidate_directory_rank * 100 + candidate_file_rank + pair_bonus;
            if pair_score > best_pair_score {
                best_pair_score = pair_score;
                selected_directory_index = directory_index;
                selected_file_index = file_index;
            }

            file_index += 1;
        }

        directory_index += 1;
    }

    let mut selected_directory_ptr = current_directory.as_ptr();
    let mut selected_directory_written = current_directory_written;
    if selected_directory_index == 1 {
        selected_directory_ptr = temp_directory.as_ptr();
        selected_directory_written = temp_directory_written;
    } else if selected_directory_index == 2 {
        selected_directory_ptr = documents_directory.as_ptr();
        selected_directory_written = documents_directory_written;
    }

    let mut selected_file_name_ptr = current_changed.as_ptr();
    let mut selected_file_name_written = current_changed_written;
    if selected_file_index == 1 {
        selected_file_name_ptr = temp_file_name.as_ptr();
        selected_file_name_written = temp_file_name_written;
    } else if selected_file_index == 2 {
        selected_file_name_ptr = documents_file_name.as_ptr();
        selected_file_name_written = documents_file_name_written;
    }

    let selected_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
        )
    };
    let mut selected_leaf = vec![0u8; selected_leaf_len as usize];
    let selected_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            selected_leaf.as_mut_ptr(),
            selected_leaf.len() as i64,
        )
    };

    let trace_variant_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            trace_old.as_ptr(),
            trace_old.len() as i64,
            trace_new.as_ptr(),
            trace_new.len() as i64,
        )
    };
    let mut trace_variant = vec![0u8; trace_variant_len as usize];
    let trace_variant_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            trace_old.as_ptr(),
            trace_old.len() as i64,
            trace_new.as_ptr(),
            trace_new.len() as i64,
            trace_variant.as_mut_ptr(),
            trace_variant.len() as i64,
        )
    };
    let trace_variant_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            trace_variant.as_ptr(),
            trace_variant_written as i64,
            trace_needle.as_ptr(),
            trace_needle.len() as i64,
        )
    };

    let grid_variant_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            grid_old.as_ptr(),
            grid_old.len() as i64,
            grid_new.as_ptr(),
            grid_new.len() as i64,
        )
    };
    let mut grid_variant = vec![0u8; grid_variant_len as usize];
    let grid_variant_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            grid_old.as_ptr(),
            grid_old.len() as i64,
            grid_new.as_ptr(),
            grid_new.len() as i64,
            grid_variant.as_mut_ptr(),
            grid_variant.len() as i64,
        )
    };
    let grid_variant_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            grid_variant.as_ptr(),
            grid_variant_written as i64,
            grid_needle.as_ptr(),
            grid_needle.len() as i64,
        )
    };

    let flow_variant_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            flow_old.as_ptr(),
            flow_old.len() as i64,
            flow_new.as_ptr(),
            flow_new.len() as i64,
        )
    };
    let mut flow_variant = vec![0u8; flow_variant_len as usize];
    let flow_variant_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_leaf.as_ptr(),
            selected_leaf_written as i64,
            flow_old.as_ptr(),
            flow_old.len() as i64,
            flow_new.as_ptr(),
            flow_new.len() as i64,
            flow_variant.as_mut_ptr(),
            flow_variant.len() as i64,
        )
    };
    let flow_variant_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            flow_variant.as_ptr(),
            flow_variant_written as i64,
            flow_needle.as_ptr(),
            flow_needle.len() as i64,
        )
    };

    let trace_file_name_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            trace_extension.as_ptr(),
            trace_extension.len() as i64,
        )
    };
    let mut trace_file_name = vec![0u8; trace_file_name_len as usize];
    let trace_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            trace_extension.as_ptr(),
            trace_extension.len() as i64,
            trace_file_name.as_mut_ptr(),
            trace_file_name.len() as i64,
        )
    };

    let grid_file_name_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            grid_extension.as_ptr(),
            grid_extension.len() as i64,
        )
    };
    let mut grid_file_name = vec![0u8; grid_file_name_len as usize];
    let grid_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            grid_extension.as_ptr(),
            grid_extension.len() as i64,
            grid_file_name.as_mut_ptr(),
            grid_file_name.len() as i64,
        )
    };

    let flow_file_name_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            flow_extension.as_ptr(),
            flow_extension.len() as i64,
        )
    };
    let mut flow_file_name = vec![0u8; flow_file_name_len as usize];
    let flow_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_file_name_ptr,
            selected_file_name_written as i64,
            flow_extension.as_ptr(),
            flow_extension.len() as i64,
            flow_file_name.as_mut_ptr(),
            flow_file_name.len() as i64,
        )
    };

    let mut selected_variant_index = 0i32;
    let mut best_variant_score = i32::MIN;
    let mut variant_index = 0i32;
    while variant_index < 3 {
        let mut candidate_variant_score = trace_variant_written * 10 + trace_variant_index;
        if variant_index == 1 {
            candidate_variant_score = grid_variant_written * 10 + grid_variant_contains * 50;
        } else if variant_index == 2 {
            candidate_variant_score = flow_variant_written * 10 + flow_variant_index;
        }

        let mut variant_bonus = 0i32;
        if variant_index == selected_file_index {
            variant_bonus += 25;
        }
        if variant_index == selected_directory_index {
            variant_bonus += 15;
        }
        if selected_file_index == 1 && variant_index == 0 {
            variant_bonus += 40;
        }
        if selected_directory_index == 2 && variant_index == 1 {
            variant_bonus += 10;
        }
        if selected_file_index == 0 && variant_index == 2 {
            variant_bonus += 5;
        }

        let variant_score = candidate_variant_score + variant_bonus;
        if variant_score > best_variant_score {
            best_variant_score = variant_score;
            selected_variant_index = variant_index;
        }

        variant_index += 1;
    }

    let mut selected_variant_file_name_ptr = trace_file_name.as_ptr();
    let mut selected_variant_file_name_written = trace_file_name_written;
    if selected_variant_index == 1 {
        selected_variant_file_name_ptr = grid_file_name.as_ptr();
        selected_variant_file_name_written = grid_file_name_written;
    } else if selected_variant_index == 2 {
        selected_variant_file_name_ptr = flow_file_name.as_ptr();
        selected_variant_file_name_written = flow_file_name_written;
    }

    let current_rebased_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            current_directory.as_ptr(),
            current_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
        )
    };
    let mut current_rebased_relative = vec![0u8; current_rebased_relative_len as usize];
    let current_rebased_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            current_directory.as_ptr(),
            current_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
            current_rebased_relative.as_mut_ptr(),
            current_rebased_relative.len() as i64,
        )
    };

    let current_rebased_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            current_rebased_relative.as_ptr(),
            current_rebased_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
        )
    };
    let mut current_rebased_full = vec![0u8; current_rebased_full_len as usize];
    let current_rebased_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            current_rebased_relative.as_ptr(),
            current_rebased_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
            current_rebased_full.as_mut_ptr(),
            current_rebased_full.len() as i64,
        )
    };

    let current_stage_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            current_rebased_full.as_ptr(),
            current_rebased_full_written as i64,
            current_stage_needle.as_ptr(),
            current_stage_needle.len() as i64,
        )
    };
    let current_stage_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            current_rebased_full.as_ptr(),
            current_rebased_full_written as i64,
            current_stage_needle.as_ptr(),
            current_stage_needle.len() as i64,
        )
    };

    let temp_rebased_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
        )
    };
    let mut temp_rebased_relative = vec![0u8; temp_rebased_relative_len as usize];
    let temp_rebased_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
            temp_rebased_relative.as_mut_ptr(),
            temp_rebased_relative.len() as i64,
        )
    };

    let temp_rebased_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            temp_rebased_relative.as_ptr(),
            temp_rebased_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
        )
    };
    let mut temp_rebased_full = vec![0u8; temp_rebased_full_len as usize];
    let temp_rebased_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            temp_rebased_relative.as_ptr(),
            temp_rebased_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_rebased_full.as_mut_ptr(),
            temp_rebased_full.len() as i64,
        )
    };

    let temp_stage_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            temp_rebased_full.as_ptr(),
            temp_rebased_full_written as i64,
            temp_stage_needle.as_ptr(),
            temp_stage_needle.len() as i64,
        )
    };
    let temp_stage_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            temp_rebased_full.as_ptr(),
            temp_rebased_full_written as i64,
            temp_stage_needle.as_ptr(),
            temp_stage_needle.len() as i64,
        )
    };

    let documents_rebased_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
        )
    };
    let mut documents_rebased_relative = vec![0u8; documents_rebased_relative_len as usize];
    let documents_rebased_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_variant_file_name_ptr,
            selected_variant_file_name_written as i64,
            documents_rebased_relative.as_mut_ptr(),
            documents_rebased_relative.len() as i64,
        )
    };

    let documents_rebased_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            documents_rebased_relative.as_ptr(),
            documents_rebased_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
        )
    };
    let mut documents_rebased_full = vec![0u8; documents_rebased_full_len as usize];
    let documents_rebased_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            documents_rebased_relative.as_ptr(),
            documents_rebased_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_rebased_full.as_mut_ptr(),
            documents_rebased_full.len() as i64,
        )
    };

    let documents_stage_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            documents_rebased_full.as_ptr(),
            documents_rebased_full_written as i64,
            documents_stage_needle.as_ptr(),
            documents_stage_needle.len() as i64,
        )
    };
    let documents_stage_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            documents_rebased_full.as_ptr(),
            documents_rebased_full_written as i64,
            documents_stage_needle.as_ptr(),
            documents_stage_needle.len() as i64,
        )
    };

    let mut selected_rebase_index = 0i32;
    let mut best_rebase_score = i32::MIN;
    let mut rebase_index = 0i32;
    while rebase_index < 3 {
        let mut candidate_rebase_score = current_rebased_full_written * 10
            + current_stage_contains * 100
            + current_stage_index;
        if rebase_index == 1 {
            candidate_rebase_score = temp_rebased_full_written * 10
                + temp_stage_contains * 100
                + temp_stage_index;
        } else if rebase_index == 2 {
            candidate_rebase_score = documents_rebased_full_written * 10
                + documents_stage_contains * 100
                + documents_stage_index;
        }

        let mut rebase_bonus = 0i32;
        if rebase_index == selected_directory_index {
            rebase_bonus += 25;
        }
        if rebase_index == selected_variant_index {
            rebase_bonus += 15;
        }
        if rebase_index == 0 && current_stage_contains != 0 {
            rebase_bonus += 10;
        }
        if rebase_index == 1 && temp_stage_contains != 0 {
            rebase_bonus += 20;
        }
        if rebase_index == 2 && documents_stage_contains != 0 {
            rebase_bonus += 30;
        }

        let rebase_score = candidate_rebase_score + rebase_bonus;
        if rebase_score > best_rebase_score {
            best_rebase_score = rebase_score;
            selected_rebase_index = rebase_index;
        }

        rebase_index += 1;
    }

    let mut selected_full_path_ptr = current_rebased_full.as_ptr();
    let mut selected_full_path_written = current_rebased_full_written;
    if selected_rebase_index == 1 {
        selected_full_path_ptr = temp_rebased_full.as_ptr();
        selected_full_path_written = temp_rebased_full_written;
    } else if selected_rebase_index == 2 {
        selected_full_path_ptr = documents_rebased_full.as_ptr();
        selected_full_path_written = documents_rebased_full_written;
    }

    let selected_leaf_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
        )
    };
    let mut selected_final_leaf = vec![0u8; selected_leaf_len as usize];
    let selected_final_leaf_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            selected_final_leaf.as_mut_ptr(),
            selected_final_leaf.len() as i64,
        )
    };

    let mesh_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            mesh_old.as_ptr(),
            mesh_old.len() as i64,
            mesh_new.as_ptr(),
            mesh_new.len() as i64,
        )
    };
    let mut mesh_leaf = vec![0u8; mesh_leaf_len as usize];
    let mesh_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            mesh_old.as_ptr(),
            mesh_old.len() as i64,
            mesh_new.as_ptr(),
            mesh_new.len() as i64,
            mesh_leaf.as_mut_ptr(),
            mesh_leaf.len() as i64,
        )
    };
    let mesh_leaf_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            mesh_leaf.as_ptr(),
            mesh_leaf_written as i64,
            mesh_needle.as_ptr(),
            mesh_needle.len() as i64,
        )
    };

    let route_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            route_old.as_ptr(),
            route_old.len() as i64,
            route_new.as_ptr(),
            route_new.len() as i64,
        )
    };
    let mut route_leaf = vec![0u8; route_leaf_len as usize];
    let route_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            route_old.as_ptr(),
            route_old.len() as i64,
            route_new.as_ptr(),
            route_new.len() as i64,
            route_leaf.as_mut_ptr(),
            route_leaf.len() as i64,
        )
    };
    let route_leaf_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            route_leaf.as_ptr(),
            route_leaf_written as i64,
            route_needle.as_ptr(),
            route_needle.len() as i64,
        )
    };

    let array_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            array_old.as_ptr(),
            array_old.len() as i64,
            array_new.as_ptr(),
            array_new.len() as i64,
        )
    };
    let mut array_leaf = vec![0u8; array_leaf_len as usize];
    let array_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_final_leaf.as_ptr(),
            selected_final_leaf_written as i64,
            array_old.as_ptr(),
            array_old.len() as i64,
            array_new.as_ptr(),
            array_new.len() as i64,
            array_leaf.as_mut_ptr(),
            array_leaf.len() as i64,
        )
    };
    let array_leaf_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            array_leaf.as_ptr(),
            array_leaf_written as i64,
            array_needle.as_ptr(),
            array_needle.len() as i64,
        )
    };

    let mut selected_leaf_transform_index = 0i32;
    let mut best_leaf_transform_score = i32::MIN;
    let mut leaf_transform_index = 0i32;
    while leaf_transform_index < 3 {
        let mut candidate_leaf_transform_score = mesh_leaf_written * 10 + mesh_leaf_contains * 50;
        if leaf_transform_index == 1 {
            candidate_leaf_transform_score = route_leaf_written * 10 + route_leaf_index;
        } else if leaf_transform_index == 2 {
            candidate_leaf_transform_score = array_leaf_written * 10 + array_leaf_contains * 50;
        }

        let mut leaf_transform_bonus = 0i32;
        if leaf_transform_index == selected_rebase_index {
            leaf_transform_bonus += 25;
        }
        if leaf_transform_index == selected_variant_index {
            leaf_transform_bonus += 15;
        }
        if leaf_transform_index == 0 && mesh_leaf_contains != 0 {
            leaf_transform_bonus += 20;
        }
        if leaf_transform_index == 1 && route_leaf_index >= 0 {
            leaf_transform_bonus += 10;
        }
        if leaf_transform_index == 2 && array_leaf_contains != 0 {
            leaf_transform_bonus += 5;
        }

        let leaf_transform_score = candidate_leaf_transform_score + leaf_transform_bonus;
        if leaf_transform_score > best_leaf_transform_score {
            best_leaf_transform_score = leaf_transform_score;
            selected_leaf_transform_index = leaf_transform_index;
        }

        leaf_transform_index += 1;
    }

    let mut selected_leaf_transform_ptr = mesh_leaf.as_ptr();
    let mut selected_leaf_transform_written = mesh_leaf_written;
    if selected_leaf_transform_index == 1 {
        selected_leaf_transform_ptr = route_leaf.as_ptr();
        selected_leaf_transform_written = route_leaf_written;
    } else if selected_leaf_transform_index == 2 {
        selected_leaf_transform_ptr = array_leaf.as_ptr();
        selected_leaf_transform_written = array_leaf_written;
    }

    let spine_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            spine_old.as_ptr(),
            spine_old.len() as i64,
            spine_new.as_ptr(),
            spine_new.len() as i64,
        )
    };
    let mut spine_path = vec![0u8; spine_path_len as usize];
    let spine_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            spine_old.as_ptr(),
            spine_old.len() as i64,
            spine_new.as_ptr(),
            spine_new.len() as i64,
            spine_path.as_mut_ptr(),
            spine_path.len() as i64,
        )
    };
    let spine_path_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            spine_path.as_ptr(),
            spine_path_written as i64,
            spine_needle.as_ptr(),
            spine_needle.len() as i64,
        )
    };

    let weave_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            weave_old.as_ptr(),
            weave_old.len() as i64,
            weave_new.as_ptr(),
            weave_new.len() as i64,
        )
    };
    let mut weave_path = vec![0u8; weave_path_len as usize];
    let weave_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            weave_old.as_ptr(),
            weave_old.len() as i64,
            weave_new.as_ptr(),
            weave_new.len() as i64,
            weave_path.as_mut_ptr(),
            weave_path.len() as i64,
        )
    };
    let weave_path_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            weave_path.as_ptr(),
            weave_path_written as i64,
            weave_needle.as_ptr(),
            weave_needle.len() as i64,
        )
    };

    let orbit_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            orbit_old.as_ptr(),
            orbit_old.len() as i64,
            orbit_new.as_ptr(),
            orbit_new.len() as i64,
        )
    };
    let mut orbit_path = vec![0u8; orbit_path_len as usize];
    let orbit_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_full_path_ptr,
            selected_full_path_written as i64,
            orbit_old.as_ptr(),
            orbit_old.len() as i64,
            orbit_new.as_ptr(),
            orbit_new.len() as i64,
            orbit_path.as_mut_ptr(),
            orbit_path.len() as i64,
        )
    };
    let orbit_path_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            orbit_path.as_ptr(),
            orbit_path_written as i64,
            orbit_needle.as_ptr(),
            orbit_needle.len() as i64,
        )
    };

    let mut selected_path_transform_index = 0i32;
    let mut best_path_transform_score = i32::MIN;
    let mut path_transform_index = 0i32;
    while path_transform_index < 3 {
        let mut candidate_path_transform_score = spine_path_written * 10 + spine_path_contains * 50;
        if path_transform_index == 1 {
            candidate_path_transform_score = weave_path_written * 10 + weave_path_index;
        } else if path_transform_index == 2 {
            candidate_path_transform_score = orbit_path_written * 10 + orbit_path_contains * 50;
        }

        let mut path_transform_bonus = 0i32;
        if path_transform_index == selected_rebase_index {
            path_transform_bonus += 25;
        }
        if path_transform_index == selected_leaf_transform_index {
            path_transform_bonus += 15;
        }
        if path_transform_index == 0 && spine_path_contains != 0 {
            path_transform_bonus += 20;
        }
        if path_transform_index == 1 && weave_path_index >= 0 {
            path_transform_bonus += 10;
        }
        if path_transform_index == 2 && orbit_path_contains != 0 {
            path_transform_bonus += 5;
        }

        let path_transform_score = candidate_path_transform_score + path_transform_bonus;
        if path_transform_score > best_path_transform_score {
            best_path_transform_score = path_transform_score;
            selected_path_transform_index = path_transform_index;
        }

        path_transform_index += 1;
    }

    let mut selected_path_transform_ptr = spine_path.as_ptr();
    let mut selected_path_transform_written = spine_path_written;
    if selected_path_transform_index == 1 {
        selected_path_transform_ptr = weave_path.as_ptr();
        selected_path_transform_written = weave_path_written;
    } else if selected_path_transform_index == 2 {
        selected_path_transform_ptr = orbit_path.as_ptr();
        selected_path_transform_written = orbit_path_written;
    }

    let selected_path_directory_len = unsafe {
        rust_mcil_dotnet_path_get_directory_name_utf8_len(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
        )
    };
    let mut selected_path_directory = vec![0u8; selected_path_directory_len as usize];
    let selected_path_directory_written = unsafe {
        rust_mcil_dotnet_path_copy_directory_name_utf8(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
            selected_path_directory.as_mut_ptr(),
            selected_path_directory.len() as i64,
        )
    };

    let selected_path_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
        )
    };
    let mut selected_path_file_name = vec![0u8; selected_path_file_name_len as usize];
    let selected_path_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            selected_path_transform_ptr,
            selected_path_transform_written as i64,
            selected_path_file_name.as_mut_ptr(),
            selected_path_file_name.len() as i64,
        )
    };

    let self_recomposed_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            selected_path_directory.as_ptr(),
            selected_path_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
        )
    };
    let mut self_recomposed_relative = vec![0u8; self_recomposed_relative_len as usize];
    let self_recomposed_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            selected_path_directory.as_ptr(),
            selected_path_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
            self_recomposed_relative.as_mut_ptr(),
            self_recomposed_relative.len() as i64,
        )
    };
    let self_recomposed_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            self_recomposed_relative.as_ptr(),
            self_recomposed_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
        )
    };
    let mut self_recomposed_full = vec![0u8; self_recomposed_full_len as usize];
    let self_recomposed_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            self_recomposed_relative.as_ptr(),
            self_recomposed_relative_written as i64,
            current_root.as_ptr(),
            current_root_written as i64,
            self_recomposed_full.as_mut_ptr(),
            self_recomposed_full.len() as i64,
        )
    };
    let self_recomposed_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            self_recomposed_full.as_ptr(),
            self_recomposed_full_written as i64,
            self_stage_needle.as_ptr(),
            self_stage_needle.len() as i64,
        )
    };

    let temp_recomposed_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
        )
    };
    let mut temp_recomposed_relative = vec![0u8; temp_recomposed_relative_len as usize];
    let temp_recomposed_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            temp_directory.as_ptr(),
            temp_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
            temp_recomposed_relative.as_mut_ptr(),
            temp_recomposed_relative.len() as i64,
        )
    };
    let temp_recomposed_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            temp_recomposed_relative.as_ptr(),
            temp_recomposed_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
        )
    };
    let mut temp_recomposed_full = vec![0u8; temp_recomposed_full_len as usize];
    let temp_recomposed_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            temp_recomposed_relative.as_ptr(),
            temp_recomposed_relative_written as i64,
            temp_root.as_ptr(),
            temp_root_written as i64,
            temp_recomposed_full.as_mut_ptr(),
            temp_recomposed_full.len() as i64,
        )
    };
    let temp_recomposed_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            temp_recomposed_full.as_ptr(),
            temp_recomposed_full_written as i64,
            temp_stage_six_needle.as_ptr(),
            temp_stage_six_needle.len() as i64,
        )
    };

    let documents_recomposed_relative_len = unsafe {
        rust_mcil_dotnet_path_combine3_utf8_len(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
        )
    };
    let mut documents_recomposed_relative = vec![0u8; documents_recomposed_relative_len as usize];
    let documents_recomposed_relative_written = unsafe {
        rust_mcil_dotnet_path_copy_combine3_utf8(
            documents_directory.as_ptr(),
            documents_directory_written as i64,
            current_segment.as_ptr(),
            current_segment.len() as i64,
            selected_path_file_name.as_ptr(),
            selected_path_file_name_written as i64,
            documents_recomposed_relative.as_mut_ptr(),
            documents_recomposed_relative.len() as i64,
        )
    };
    let documents_recomposed_full_len = unsafe {
        rust_mcil_dotnet_path_get_full_utf8_len(
            documents_recomposed_relative.as_ptr(),
            documents_recomposed_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
        )
    };
    let mut documents_recomposed_full = vec![0u8; documents_recomposed_full_len as usize];
    let documents_recomposed_full_written = unsafe {
        rust_mcil_dotnet_path_copy_full_utf8(
            documents_recomposed_relative.as_ptr(),
            documents_recomposed_relative_written as i64,
            documents_root.as_ptr(),
            documents_root_written as i64,
            documents_recomposed_full.as_mut_ptr(),
            documents_recomposed_full.len() as i64,
        )
    };
    let documents_recomposed_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            documents_recomposed_full.as_ptr(),
            documents_recomposed_full_written as i64,
            documents_stage_six_needle.as_ptr(),
            documents_stage_six_needle.len() as i64,
        )
    };

    let mut selected_recomposition_index = 0i32;
    let mut best_recomposition_score = i32::MIN;
    let mut recomposition_index = 0i32;
    while recomposition_index < 3 {
        let mut candidate_recomposition_score = self_recomposed_full_written * 10 + self_recomposed_contains * 50;
        if recomposition_index == 1 {
            candidate_recomposition_score = temp_recomposed_full_written * 10 + temp_recomposed_index;
        } else if recomposition_index == 2 {
            candidate_recomposition_score = documents_recomposed_full_written * 10 + documents_recomposed_contains * 50;
        }

        let mut recomposition_bonus = 0i32;
        if recomposition_index == selected_rebase_index {
            recomposition_bonus += 25;
        }
        if recomposition_index == selected_path_transform_index {
            recomposition_bonus += 15;
        }
        if recomposition_index == 0 && self_recomposed_contains != 0 {
            recomposition_bonus += 20;
        }
        if recomposition_index == 1 && temp_recomposed_index >= 0 {
            recomposition_bonus += 10;
        }
        if recomposition_index == 2 && documents_recomposed_contains != 0 {
            recomposition_bonus += 5;
        }

        let recomposition_score = candidate_recomposition_score + recomposition_bonus;
        if recomposition_score > best_recomposition_score {
            best_recomposition_score = recomposition_score;
            selected_recomposition_index = recomposition_index;
        }

        recomposition_index += 1;
    }

    let mut selected_recomposition_ptr = self_recomposed_full.as_ptr();
    let mut selected_recomposition_written = self_recomposed_full_written;
    if selected_recomposition_index == 1 {
        selected_recomposition_ptr = temp_recomposed_full.as_ptr();
        selected_recomposition_written = temp_recomposed_full_written;
    } else if selected_recomposition_index == 2 {
        selected_recomposition_ptr = documents_recomposed_full.as_ptr();
        selected_recomposition_written = documents_recomposed_full_written;
    }

    let final_transformed_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            final_old.as_ptr(),
            final_old.len() as i64,
            final_new.as_ptr(),
            final_new.len() as i64,
        )
    };
    let mut final_transformed = vec![0u8; final_transformed_len as usize];
    let final_transformed_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
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

    let crown_path_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            crown_old.as_ptr(),
            crown_old.len() as i64,
            crown_new.as_ptr(),
            crown_new.len() as i64,
        )
    };
    let mut crown_path = vec![0u8; crown_path_len as usize];
    let crown_path_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            crown_old.as_ptr(),
            crown_old.len() as i64,
            crown_new.as_ptr(),
            crown_new.len() as i64,
            crown_path.as_mut_ptr(),
            crown_path.len() as i64,
        )
    };
    let crown_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            crown_path.as_ptr(),
            crown_path_written as i64,
            crown_needle.as_ptr(),
            crown_needle.len() as i64,
        )
    };

    let pulse_path_len = unsafe {
        rust_mcil_dotnet_path_change_extension_utf8_len(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            pulse_extension.as_ptr(),
            pulse_extension.len() as i64,
        )
    };
    let mut pulse_path = vec![0u8; pulse_path_len as usize];
    let pulse_path_written = unsafe {
        rust_mcil_dotnet_path_copy_change_extension_utf8(
            selected_recomposition_ptr,
            selected_recomposition_written as i64,
            pulse_extension.as_ptr(),
            pulse_extension.len() as i64,
            pulse_path.as_mut_ptr(),
            pulse_path.len() as i64,
        )
    };
    let pulse_file_name_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_utf8_len(
            pulse_path.as_ptr(),
            pulse_path_written as i64,
        )
    };
    let mut pulse_file_name = vec![0u8; pulse_file_name_len as usize];
    let pulse_file_name_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_utf8(
            pulse_path.as_ptr(),
            pulse_path_written as i64,
            pulse_file_name.as_mut_ptr(),
            pulse_file_name.len() as i64,
        )
    };
    let pulse_index = unsafe {
        rust_mcil_dotnet_string_index_of(
            pulse_file_name.as_ptr(),
            pulse_file_name_written as i64,
            pulse_needle.as_ptr(),
            pulse_needle.len() as i64,
        )
    };

    let anchor_source_len = unsafe {
        rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
        )
    };
    let mut anchor_source = vec![0u8; anchor_source_len as usize];
    let anchor_source_written = unsafe {
        rust_mcil_dotnet_path_copy_file_name_without_extension_utf8(
            final_transformed.as_ptr(),
            final_transformed_written as i64,
            anchor_source.as_mut_ptr(),
            anchor_source.len() as i64,
        )
    };
    let anchor_leaf_len = unsafe {
        rust_mcil_dotnet_string_replace_utf8_len(
            anchor_source.as_ptr(),
            anchor_source_written as i64,
            anchor_old.as_ptr(),
            anchor_old.len() as i64,
            anchor_new.as_ptr(),
            anchor_new.len() as i64,
        )
    };
    let mut anchor_leaf = vec![0u8; anchor_leaf_len as usize];
    let anchor_leaf_written = unsafe {
        rust_mcil_dotnet_string_copy_replace_utf8(
            anchor_source.as_ptr(),
            anchor_source_written as i64,
            anchor_old.as_ptr(),
            anchor_old.len() as i64,
            anchor_new.as_ptr(),
            anchor_new.len() as i64,
            anchor_leaf.as_mut_ptr(),
            anchor_leaf.len() as i64,
        )
    };
    let anchor_contains = unsafe {
        rust_mcil_dotnet_string_contains(
            anchor_leaf.as_ptr(),
            anchor_leaf_written as i64,
            anchor_needle.as_ptr(),
            anchor_needle.len() as i64,
        )
    };

    let mut selected_closure_index = 0i32;
    let mut best_closure_score = i32::MIN;
    let mut closure_index = 0i32;
    while closure_index < 3 {
        let mut candidate_closure_score = crown_path_written * 10 + crown_contains * 50;
        if closure_index == 1 {
            candidate_closure_score = pulse_file_name_written * 10 + pulse_index;
        } else if closure_index == 2 {
            candidate_closure_score = anchor_leaf_written * 10 + anchor_contains * 50;
        }

        let mut closure_bonus = 0i32;
        if closure_index == selected_recomposition_index {
            closure_bonus += 25;
        }
        if closure_index == selected_path_transform_index {
            closure_bonus += 15;
        }
        if closure_index == selected_variant_index {
            closure_bonus += 5;
        }
        if closure_index == 0 && crown_contains != 0 {
            closure_bonus += 20;
        }
        if closure_index == 1 && pulse_index >= 0 {
            closure_bonus += 10;
        }
        if closure_index == 2 && anchor_contains != 0 {
            closure_bonus += 30;
        }

        let closure_score = candidate_closure_score + closure_bonus;
        if closure_score > best_closure_score {
            best_closure_score = closure_score;
            selected_closure_index = closure_index;
        }

        closure_index += 1;
    }

    let mut closure_digit = crown_contains.rem_euclid(10);
    if selected_closure_index == 1 {
        closure_digit = pulse_index.rem_euclid(10);
    } else if selected_closure_index == 2 {
        closure_digit = anchor_contains.rem_euclid(10);
    }

    (selected_directory_index + 1) * 100000000
        + (selected_file_index + 1) * 10000000
        + (selected_variant_index + 1) * 1000000
        + (selected_rebase_index + 1) * 100000
        + (selected_leaf_transform_index + 1) * 10000
        + (selected_path_transform_index + 1) * 1000
        + (selected_recomposition_index + 1) * 100
        + (selected_closure_index + 1) * 10
        + closure_digit
}