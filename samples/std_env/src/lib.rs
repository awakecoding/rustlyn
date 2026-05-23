use std::env;
use std::path::Path;

/// Returns a positive score if all std::env operations succeed, or a negative error code.
#[unsafe(no_mangle)]
pub extern "C" fn std_env_probe() -> i32 {
    // current_dir should return a non-empty path
    let current_dir = match env::current_dir() {
        Ok(path) => path,
        Err(_) => return -1,
    };

    if current_dir.as_os_str().is_empty() {
        return -2;
    }

    // current_dir should be absolute
    if !current_dir.is_absolute() {
        return -3;
    }

    // temp_dir should return a non-empty path
    let temp_dir = env::temp_dir();
    if temp_dir.as_os_str().is_empty() {
        return -4;
    }

    // temp_dir should be absolute
    if !temp_dir.is_absolute() {
        return -5;
    }

    // PATH env var should exist and be non-empty
    let path_var = match env::var("PATH") {
        Ok(val) => val,
        Err(_) => return -6,
    };

    if path_var.is_empty() {
        return -7;
    }

    // A non-existent env var should fail
    if env::var("RUSTMCIL_NONEXISTENT_VAR_12345").is_ok() {
        return -8;
    }

    // current_dir should have a file name component
    let dir_name = current_dir.file_name();
    if dir_name.is_none() {
        return -9;
    }

    // Construct a path from components
    let joined = Path::new(&temp_dir).join("rustmcil_test");
    if joined.as_os_str().is_empty() {
        return -10;
    }

    // The joined path should start with temp_dir
    if !joined.starts_with(&temp_dir) {
        return -11;
    }

    // Return a success score based on the path length (always positive)
    let score = current_dir.as_os_str().len() as i32;
    if score <= 0 { 1 } else { score }
}
