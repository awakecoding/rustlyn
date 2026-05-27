#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn rem_i32(a: i32, b: i32) -> i32 {
    a % b
}