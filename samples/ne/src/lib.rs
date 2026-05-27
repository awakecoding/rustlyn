#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn ne_i32(a: i32, b: i32) -> i32 {
    if a != b { 1 } else { 0 }
}