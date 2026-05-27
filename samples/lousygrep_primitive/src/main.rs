#![no_main]

unsafe extern "C" {
    fn rustlyn_dotnet_command_line_arg_count() -> i32;
    fn rustlyn_dotnet_command_line_arg_utf8_len(index: i32) -> i32;
    fn rustlyn_dotnet_copy_command_line_arg_utf8(
        index: i32,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_file_read_all_lines_count(path_ptr: *const u8, path_len: i64) -> i32;
    fn rustlyn_dotnet_file_read_all_lines_line_utf8_len(
        path_ptr: *const u8,
        path_len: i64,
        index: i32,
    ) -> i32;
    fn rustlyn_dotnet_file_copy_read_all_lines_line_utf8(
        path_ptr: *const u8,
        path_len: i64,
        index: i32,
        destination_ptr: *mut u8,
        destination_capacity: i64,
    ) -> i32;
    fn rustlyn_dotnet_string_contains(
        haystack_ptr: *const u8,
        haystack_len: i64,
        needle_ptr: *const u8,
        needle_len: i64,
    ) -> i32;
    fn rustlyn_dotnet_console_write_line_utf8(value_ptr: *const u8, value_len: i64);
    fn rustlyn_dotnet_console_write_prefixed_line_utf8(
        path_ptr: *const u8,
        path_len: i64,
        line_number: i32,
        value_ptr: *const u8,
        value_len: i64,
    );
    fn rustlyn_dotnet_console_write_path_line_utf8(
        path_ptr: *const u8,
        path_len: i64,
        value_ptr: *const u8,
        value_len: i64,
    );
    fn rustlyn_dotnet_console_write_numbered_line_utf8(
        line_number: i32,
        value_ptr: *const u8,
        value_len: i64,
    );
    fn rustlyn_dotnet_console_write_i32(value: i32);
    fn rustlyn_dotnet_console_write_path_count_utf8(
        path_ptr: *const u8,
        path_len: i64,
        value: i32,
    );
}

fn load_arg(index: i32) -> Vec<u8> {
    let arg_len = unsafe { rustlyn_dotnet_command_line_arg_utf8_len(index) };
    let mut arg = vec![0u8; arg_len as usize];
    let written = unsafe {
        rustlyn_dotnet_copy_command_line_arg_utf8(index, arg.as_mut_ptr(), arg.len() as i64)
    };
    arg.truncate(written as usize);
    arg
}

fn load_line(path: &[u8], index: i32) -> Vec<u8> {
    let line_len = unsafe {
        rustlyn_dotnet_file_read_all_lines_line_utf8_len(path.as_ptr(), path.len() as i64, index)
    };
    let mut line = vec![0u8; line_len as usize];
    let written = unsafe {
        rustlyn_dotnet_file_copy_read_all_lines_line_utf8(
            path.as_ptr(),
            path.len() as i64,
            index,
            line.as_mut_ptr(),
            line.len() as i64,
        )
    };
    line.truncate(written as usize);
    line
}

fn is_numbered_flag(value: &[u8]) -> bool {
    value.len() == 2 && value[0] == b'-' && value[1] == b'n'
}

fn is_hidden_filename_flag(value: &[u8]) -> bool {
    value.len() == 2 && value[0] == b'-' && value[1] == b'h'
}

fn is_list_matches_flag(value: &[u8]) -> bool {
    value.len() == 2 && value[0] == b'-' && value[1] == b'l'
}

fn is_count_matches_flag(value: &[u8]) -> bool {
    value.len() == 2 && value[0] == b'-' && value[1] == b'c'
}

#[unsafe(no_mangle)]
pub extern "C" fn main() -> i32 {
    let count = unsafe { rustlyn_dotnet_command_line_arg_count() };
    if count < 3 {
        return 1;
    }

    let mut show_line_numbers = false;
    let mut show_paths = true;
    let mut list_matching_files = false;
    let mut count_matching_lines = false;
    let mut search_index = 1;

    while search_index < count {
        let option = load_arg(search_index);
        if is_numbered_flag(&option) {
            show_line_numbers = true;
            search_index += 1;
            continue;
        }

        if is_hidden_filename_flag(&option) {
            show_paths = false;
            search_index += 1;
            continue;
        }

        if is_list_matches_flag(&option) {
            list_matching_files = true;
            search_index += 1;
            continue;
        }

        if is_count_matches_flag(&option) {
            count_matching_lines = true;
            search_index += 1;
            continue;
        }

        break;
    }

    let file_start_index = search_index + 1;

    if count <= file_start_index {
        return 1;
    }

    let search = load_arg(search_index);
    if show_paths {
        show_paths = count - file_start_index > 1;
    }

    for arg_index in file_start_index..count {
        let path = load_arg(arg_index);
        let line_count = unsafe {
            rustlyn_dotnet_file_read_all_lines_count(path.as_ptr(), path.len() as i64)
        };
        let mut match_count = 0;

        for index in 0..line_count {
            let line = load_line(&path, index);
            let contains = unsafe {
                rustlyn_dotnet_string_contains(
                    line.as_ptr(),
                    line.len() as i64,
                    search.as_ptr(),
                    search.len() as i64,
                )
            };

            if contains != 0 {
                match_count += 1;

                if list_matching_files {
                    unsafe {
                        rustlyn_dotnet_console_write_line_utf8(
                            path.as_ptr(),
                            path.len() as i64,
                        );
                    }

                    break;
                }

                if count_matching_lines {
                    continue;
                }

                if show_paths && show_line_numbers {
                    unsafe {
                        rustlyn_dotnet_console_write_prefixed_line_utf8(
                            path.as_ptr(),
                            path.len() as i64,
                            index + 1,
                            line.as_ptr(),
                            line.len() as i64,
                        );
                    }
                }
                else if show_paths {
                    unsafe {
                        rustlyn_dotnet_console_write_path_line_utf8(
                            path.as_ptr(),
                            path.len() as i64,
                            line.as_ptr(),
                            line.len() as i64,
                        );
                    }
                }
                else if show_line_numbers {
                    unsafe {
                        rustlyn_dotnet_console_write_numbered_line_utf8(
                            index + 1,
                            line.as_ptr(),
                            line.len() as i64,
                        );
                    }
                }
                else {
                    unsafe {
                        rustlyn_dotnet_console_write_line_utf8(
                            line.as_ptr(),
                            line.len() as i64,
                        );
                    }
                }
            }
        }

        if count_matching_lines && !list_matching_files {
            if show_paths {
                unsafe {
                    rustlyn_dotnet_console_write_path_count_utf8(
                        path.as_ptr(),
                        path.len() as i64,
                        match_count,
                    );
                }
            }
            else {
                unsafe {
                    rustlyn_dotnet_console_write_i32(match_count);
                }
            }
        }
    }

    0
}