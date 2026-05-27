#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn eq_i32(a: i32, b: i32) -> i32 {
    if a == b { 1 } else { 0 }
}