#[no_mangle]
pub extern "C" fn heap_alloc_probe(x: i32) -> i32 {
    let boxed = Box::new(x * 2);
    *boxed + 1
}
