#[unsafe(no_mangle)]
pub extern "C" fn float_add(a: f32, b: f32) -> f32 {
    a + b
}

#[unsafe(no_mangle)]
pub extern "C" fn float_mul(a: f32, b: f32) -> f32 {
    a * b
}

#[unsafe(no_mangle)]
pub extern "C" fn float_div(a: f32, b: f32) -> f32 {
    a / b
}

#[unsafe(no_mangle)]
pub extern "C" fn double_add(a: f64, b: f64) -> f64 {
    a + b
}

#[unsafe(no_mangle)]
pub extern "C" fn double_mul(a: f64, b: f64) -> f64 {
    a * b
}

#[unsafe(no_mangle)]
pub extern "C" fn float_to_int(x: f32) -> i32 {
    x as i32
}

#[unsafe(no_mangle)]
pub extern "C" fn int_to_float(x: i32) -> f32 {
    x as f32
}

#[unsafe(no_mangle)]
pub extern "C" fn double_to_int(x: f64) -> i64 {
    x as i64
}

#[unsafe(no_mangle)]
pub extern "C" fn float_compare_lt(a: f32, b: f32) -> i32 {
    if a < b { 1 } else { 0 }
}

/// Combined probe: (3.5 + 2.5) * 2.0 = 12.0 -> 12 as i32
#[unsafe(no_mangle)]
pub extern "C" fn float_probe() -> i32 {
    let sum = float_add(3.5, 2.5);
    let product = float_mul(sum, 2.0);
    float_to_int(product)
}
