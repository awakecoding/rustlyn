#[inline(never)]
fn apply(f: impl Fn(i32) -> i32, x: i32) -> i32 {
    f(x)
}

#[no_mangle]
pub extern "C" fn closure_probe(x: i32) -> i32 {
    let offset = x * 2;
    let add_offset = |y: i32| y + offset;
    apply(add_offset, 10)
}
