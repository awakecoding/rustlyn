use std::path::{Path, PathBuf};

/// Returns a positive score if std::path operations work correctly, or a negative error code.
#[unsafe(no_mangle)]
pub extern "C" fn std_path_probe() -> i32 {
    // Basic Path construction
    let p = Path::new("/foo/bar/baz.txt");

    // file_name
    let file_name = match p.file_name() {
        Some(name) => name,
        None => return -1,
    };
    if file_name != "baz.txt" {
        return -2;
    }

    // file_stem
    let stem = match p.file_stem() {
        Some(s) => s,
        None => return -3,
    };
    if stem != "baz" {
        return -4;
    }

    // extension
    let ext = match p.extension() {
        Some(e) => e,
        None => return -5,
    };
    if ext != "txt" {
        return -6;
    }

    // parent
    let parent = match p.parent() {
        Some(par) => par,
        None => return -7,
    };
    if parent != Path::new("/foo/bar") {
        return -8;
    }

    // is_absolute / is_relative
    if !p.is_absolute() {
        return -9;
    }
    let relative = Path::new("relative/path");
    if relative.is_absolute() {
        return -10;
    }
    if !relative.is_relative() {
        return -11;
    }

    // PathBuf push/join
    let mut buf = PathBuf::from("/usr");
    buf.push("local");
    buf.push("bin");
    if buf != Path::new("/usr/local/bin") {
        return -12;
    }

    let joined = Path::new("/home").join("user").join("docs");
    if joined != Path::new("/home/user/docs") {
        return -13;
    }

    // with_extension
    let changed = Path::new("/foo/bar.rs").with_extension("txt");
    if changed != Path::new("/foo/bar.txt") {
        return -14;
    }

    // with_file_name
    let renamed = Path::new("/foo/old.rs").with_file_name("new.rs");
    if renamed != Path::new("/foo/new.rs") {
        return -15;
    }

    // Components count (platform-dependent but always > 0 for non-empty paths)
    let component_count = p.components().count();
    if component_count == 0 {
        return -16;
    }

    // starts_with / ends_with
    if !joined.starts_with("/home/user") {
        return -17;
    }
    if !joined.ends_with("docs") {
        return -18;
    }

    // Return component count as positive success indicator
    component_count as i32
}
