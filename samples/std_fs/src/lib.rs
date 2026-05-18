use std::path::PathBuf;

#[unsafe(no_mangle)]
pub extern "C" fn std_fs_line_count() -> i32 {
    let mut path = PathBuf::from(env!("CARGO_MANIFEST_DIR"));
    path.push("fixtures");
    path.push("input.txt");

    std::fs::read_to_string(path)
        .expect("std_fs fixture should exist")
        .lines()
        .count() as i32
}