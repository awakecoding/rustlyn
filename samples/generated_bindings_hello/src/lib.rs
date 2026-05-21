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

    if file_name.release().is_err() {
        let _ = current_directory.release();
        return -20;
    }

    let file_name_without_extension = match system::io::path::get_file_name_without_extension(&current_directory) {
        Ok(value) => value,
        Err(_) => {
            let _ = current_directory.release();
            return -21;
        }
    };

    let stem_len = match file_name_without_extension.len() {
        Ok(value) => value,
        Err(_) => {
            let _ = file_name_without_extension.release();
            let _ = current_directory.release();
            return -22;
        }
    };

    if stem_len <= 0 || stem_len > file_name_len {
        let _ = file_name_without_extension.release();
        let _ = current_directory.release();
        return -23;
    }

    if file_name_without_extension.release().is_err() {
        let _ = current_directory.release();
        return -24;
    }

    if current_directory.release().is_err() {
        return -4;
    }

    100
}