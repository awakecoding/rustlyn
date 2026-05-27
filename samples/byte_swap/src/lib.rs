#[no_mangle]
pub extern "C" fn byte_swap_probe(x: i32) -> i32 {
    x.swap_bytes()
}
