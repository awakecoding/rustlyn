mod system;

#[unsafe(no_mangle)]
pub extern "C" fn generated_bindings_score() -> i32 {
    let current_directory = match system::io::directory::get_current_directory() {
        Ok(value) => value,
        Err(_) => return -2,
    };

    let directory_len = match current_directory.utf8_len() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -3;
        }
    };

    let mut directory_buffer = [0u8; 512];
    if directory_len <= 0 || directory_len as usize > directory_buffer.len() {
        let _ = current_directory.release();
        return -5;
    }

    let written = match current_directory.copy_utf8_into(&mut directory_buffer) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -6;
        }
    };

    if written <= 0 {
        let _ = current_directory.release();
        return -7;
    }

    if system::console::write_line_utf8_parts(directory_buffer.as_ptr(), written as i64).is_err() {
        let _ = current_directory.release();
        return -8;
    }

    let roundtrip_directory = match system::ManagedString::from_utf8_parts(
        directory_buffer.as_ptr(),
        written as i64,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -9;
        }
    };

    if system::console::write_line(&roundtrip_directory).is_err() {
        let _ = roundtrip_directory.release();
        let _ = current_directory.release();
        return -10;
    }

    if roundtrip_directory.release().is_err() {
        let _ = current_directory.release();
        return -11;
    }

    let environment_directory = match system::environment::current_directory() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -12;
        }
    };

    if system::console::write_line(&environment_directory).is_err() {
        let _ = environment_directory.release();
        let _ = current_directory.release();
        return -13;
    }

    if environment_directory.release().is_err() {
        let _ = current_directory.release();
        return -14;
    }

    let char_len = match current_directory.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -15;
        }
    };

    if char_len <= 0 || char_len > directory_len {
        let _ = current_directory.release();
        return -16;
    }

    let file_name = match system::io::path::get_file_name(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -17;
        }
    };

    let file_name_len = match file_name.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name.release();
            let _ = current_directory.release();
            return -18;
        }
    };

    if file_name_len <= 0 || file_name_len > char_len {
        let _ = file_name.release();
        let _ = current_directory.release();
        return -19;
    }

    let combined_path = match system::io::path::combine(&current_directory, &file_name) {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name.release();
            let _ = current_directory.release();
            return -20;
        }
    };

    let combined_len = match combined_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = combined_path.release();
            let _ = file_name.release();
            let _ = current_directory.release();
            return -21;
        }
    };

    if combined_len <= char_len {
        let _ = combined_path.release();
        let _ = file_name.release();
        let _ = current_directory.release();
        return -22;
    }

    if combined_path.release().is_err() {
        let _ = file_name.release();
        let _ = current_directory.release();
        return -23;
    }

    if file_name.release().is_err() {
        let _ = current_directory.release();
        return -24;
    }

    let file_name_without_extension = match system::io::path::get_file_name_without_extension(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -25;
        }
    };

    let stem_len = match file_name_without_extension.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -26;
        }
    };

    if stem_len <= 0 || stem_len > file_name_len {
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -27;
    }

    let changed_path = match system::io::path::change_extension(
        &current_directory,
        &file_name_without_extension,
    ) {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -28;
        }
    };

    let changed_len = match changed_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -29;
        }
    };

    if changed_len <= 0 {
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -30;
    }

    let changed_extension = match system::io::path::get_extension(&changed_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -31;
        }
    };

    let changed_extension_len = match changed_extension.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = changed_extension.release();
            let _ = changed_path.release();
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -32;
        }
    };

    if changed_extension_len <= 0 {
        let _ = changed_extension.release();
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -33;
    }

    if changed_extension.release().is_err() {
        let _ = changed_path.release();
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -34;
    }

    if changed_path.release().is_err() {
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -35;
    }

    if file_name_without_extension.release().is_err() {
        let _ = current_directory.release();
        return -36;
    }

    let directory_name = match system::io::path::get_directory_name(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -37;
        }
    };

    let directory_name_len = match directory_name.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = directory_name.release();
            let _ = current_directory.release();
            return -38;
        }
    };

    if directory_name_len <= 0 || directory_name_len >= char_len {
        let _ = directory_name.release();
        let _ = current_directory.release();
        return -39;
    }

    if directory_name.release().is_err() {
        let _ = current_directory.release();
        return -40;
    }

    let temp_path = match system::io::path::get_temp_path() {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -41;
        }
    };

    let temp_len = match temp_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -42;
        }
    };

    if temp_len <= 0 {
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -43;
    }

    let full_temp_path = match system::io::path::get_full_path(&temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -44;
        }
    };

    let full_temp_len = match full_temp_path.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -45;
        }
    };

    if full_temp_len <= 0 {
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -46;
    }

    let temp_root = match system::io::path::get_path_root(&full_temp_path) {
        Ok(value) => value,
        Err(_) => {
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -47;
        }
    };

    let temp_root_len = match temp_root.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = temp_root.release();
            let _ = full_temp_path.release();
            let _ = temp_path.release();
            let _ = current_directory.release();
            return -48;
        }
    };

    if temp_root_len <= 0 || temp_root_len > full_temp_len {
        let _ = temp_root.release();
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -49;
    }

    if temp_root.release().is_err() {
        let _ = full_temp_path.release();
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -50;
    }

    if full_temp_path.release().is_err() {
        let _ = temp_path.release();
        let _ = current_directory.release();
        return -51;
    }

    if temp_path.release().is_err() {
        let _ = current_directory.release();
        return -52;
    }

    if current_directory.release().is_err() {
        return -4;
    }

    100
}