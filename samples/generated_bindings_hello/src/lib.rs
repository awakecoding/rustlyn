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

    if current_directory.release().is_err() {
        return -4;
    }

    100
}