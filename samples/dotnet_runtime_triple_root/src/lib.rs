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
    fn rustlyn_dotnet_documents_utf8_len() -> i32;
    fn rustlyn_dotnet_copy_documents_utf8(
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
pub extern "C" fn dotnet_runtime_triple_root_score() -> i32 {
    let current_second = "samples";
    let current_third = "std_fs\\fixtures\\input.txt";
    let current_extension = ".data";

    let temp_second = "managed";
    let temp_third = "output.bin";
    let temp_old = "out";
    let temp_new = "trace";
    let temp_needle = "put";

    let documents_second = "archive";
    let documents_third = "notes.log";
    let documents_old = "no";
    let documents_new = "memo";
    let documents_needle = "ot";

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
    let current_changed_name_len = unsafe {
        rustlyn_dotnet_path_get_file_name_len(changed_path.as_ptr(), changed_written as i64)
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
    let temp_transformed_len = unsafe {
        rustlyn_dotnet_string_replace_utf8_len(
            temp_base.as_ptr(),
            temp_base_written as i64,
            temp_old.as_ptr(),
            temp_old.len() as i64,
            temp_new.as_ptr(),
            temp_new.len() as i64,
        )
    };
    let mut temp_transformed = vec![0u8; temp_transformed_len as usize];
    let temp_transformed_written = unsafe {
        rustlyn_dotnet_string_copy_replace_utf8(
            temp_base.as_ptr(),
            temp_base_written as i64,
            temp_old.as_ptr(),
            temp_old.len() as i64,
            temp_new.as_ptr(),
            temp_new.len() as i64,
            temp_transformed.as_mut_ptr(),
            temp_transformed.len() as i64,
        )
    };
    let temp_contains = unsafe {
        rustlyn_dotnet_string_contains(
            temp_transformed.as_ptr(),
            temp_transformed_written as i64,
            temp_needle.as_ptr(),
            temp_needle.len() as i64,
        )
    };
    let temp_index = unsafe {
        rustlyn_dotnet_string_index_of(
            temp_transformed.as_ptr(),
            temp_transformed_written as i64,
            temp_needle.as_ptr(),
            temp_needle.len() as i64,
        )
    };

    let documents_root_len = unsafe { rustlyn_dotnet_documents_utf8_len() };
    let mut documents_root = vec![0u8; documents_root_len as usize];
    let documents_root_written = unsafe {
        rustlyn_dotnet_copy_documents_utf8(documents_root.as_mut_ptr(), documents_root.len() as i64)
    };
    let documents_combined_len = unsafe {
        rustlyn_dotnet_path_combine3_utf8_len(
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
        rustlyn_dotnet_path_copy_combine3_utf8(
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
    let documents_base_len = unsafe {
        rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
            documents_combined.as_ptr(),
            documents_combined_written as i64,
        )
    };
    let mut documents_base = vec![0u8; documents_base_len as usize];
    let documents_base_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
            documents_combined.as_ptr(),
            documents_combined_written as i64,
            documents_base.as_mut_ptr(),
            documents_base.len() as i64,
        )
    };
    let documents_transformed_len = unsafe {
        rustlyn_dotnet_string_replace_utf8_len(
            documents_base.as_ptr(),
            documents_base_written as i64,
            documents_old.as_ptr(),
            documents_old.len() as i64,
            documents_new.as_ptr(),
            documents_new.len() as i64,
        )
    };
    let mut documents_transformed = vec![0u8; documents_transformed_len as usize];
    let documents_transformed_written = unsafe {
        rustlyn_dotnet_string_copy_replace_utf8(
            documents_base.as_ptr(),
            documents_base_written as i64,
            documents_old.as_ptr(),
            documents_old.len() as i64,
            documents_new.as_ptr(),
            documents_new.len() as i64,
            documents_transformed.as_mut_ptr(),
            documents_transformed.len() as i64,
        )
    };
    let documents_contains = unsafe {
        rustlyn_dotnet_string_contains(
            documents_transformed.as_ptr(),
            documents_transformed_written as i64,
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
        )
    };
    let documents_index = unsafe {
        rustlyn_dotnet_string_index_of(
            documents_transformed.as_ptr(),
            documents_transformed_written as i64,
            documents_needle.as_ptr(),
            documents_needle.len() as i64,
        )
    };

    current_changed_name_len * 100000
        + temp_contains * 10000
        + temp_transformed_written * 1000
        + temp_index * 100
        + documents_transformed_written * 10
        + documents_contains * 10
        + documents_index
}