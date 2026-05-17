#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn and_i32(a: i32, b: i32) -> i32 {
    a & b
}