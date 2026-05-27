#[no_mangle]
pub extern "C" fn string_len_probe() -> i32 {
    let s = "hello";
    s.len() as i32
}
