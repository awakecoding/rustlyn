#[inline(never)]
fn compute(a: i32, b: i32) -> i32 {
    a * 2 + b
}

#[no_mangle]
pub extern "C" fn invoke_probe(x: i32) -> i32 {
    // Tests that invoke-style calls (panic=unwind) work
    let a = compute(x, 1);
    let b = compute(a, x);
    b
}
