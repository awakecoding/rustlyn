fn add(a: i32, b: i32) -> i32 {
    a + b
}

fn sub(a: i32, b: i32) -> i32 {
    a - b
}

#[no_mangle]
pub extern "C" fn fn_pointer_probe(x: i32) -> i32 {
    let op: fn(i32, i32) -> i32 = if x > 0 { add } else { sub };
    op(x, 10)
}
