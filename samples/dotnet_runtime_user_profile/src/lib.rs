unsafe extern "C" {
    fn rustlyn_dotnet_user_profile_utf8_len() -> i32;
    fn rustlyn_dotnet_copy_user_profile_utf8(
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
pub extern "C" fn dotnet_runtime_user_profile_score() -> i32 {
    let second = "managed";
    let third = "output.bin";
    let extension = ".trace";
    let old = "out";
    let new = "managed";
    let needle = "put";

    let root_len = unsafe { rustlyn_dotnet_user_profile_utf8_len() };
    let mut root = vec![0u8; root_len as usize];
    let root_written = unsafe {
        rustlyn_dotnet_copy_user_profile_utf8(root.as_mut_ptr(), root.len() as i64)
    };

    let combined_len = unsafe {
        rustlyn_dotnet_path_combine3_utf8_len(
            root.as_ptr(),
            root_written as i64,
            second.as_ptr(),
            second.len() as i64,
            third.as_ptr(),
            third.len() as i64,
        )
    };
    let mut combined_path = vec![0u8; combined_len as usize];
    let combined_written = unsafe {
        rustlyn_dotnet_path_copy_combine3_utf8(
            root.as_ptr(),
            root_written as i64,
            second.as_ptr(),
            second.len() as i64,
            third.as_ptr(),
            third.len() as i64,
            combined_path.as_mut_ptr(),
            combined_path.len() as i64,
        )
    };

    let changed_len = unsafe {
        rustlyn_dotnet_path_change_extension_utf8_len(
            combined_path.as_ptr(),
            combined_written as i64,
            extension.as_ptr(),
            extension.len() as i64,
        )
    };
    let mut changed_path = vec![0u8; changed_len as usize];
    let changed_written = unsafe {
        rustlyn_dotnet_path_copy_change_extension_utf8(
            combined_path.as_ptr(),
            combined_written as i64,
            extension.as_ptr(),
            extension.len() as i64,
            changed_path.as_mut_ptr(),
            changed_path.len() as i64,
        )
    };
    let changed_file_name_len = unsafe {
        rustlyn_dotnet_path_get_file_name_len(changed_path.as_ptr(), changed_written as i64)
    };

    let file_name_len = unsafe {
        rustlyn_dotnet_path_get_file_name_without_extension_utf8_len(
            changed_path.as_ptr(),
            changed_written as i64,
        )
    };
    let mut file_name = vec![0u8; file_name_len as usize];
    let file_name_written = unsafe {
        rustlyn_dotnet_path_copy_file_name_without_extension_utf8(
            changed_path.as_ptr(),
            changed_written as i64,
            file_name.as_mut_ptr(),
            file_name.len() as i64,
        )
    };

    let transformed_len = unsafe {
        rustlyn_dotnet_string_replace_utf8_len(
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
        rustlyn_dotnet_string_copy_replace_utf8(
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

    changed_file_name_len * 1000 + file_name_written * 100 + transformed_written * 10 + contains * 10 + index
}