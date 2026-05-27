#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn max_eq_i32(a: i32, b: i32) -> i32 {
    if a >= b { a } else { b }
}