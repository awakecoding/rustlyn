#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn rem_i64(a: i64, b: i64) -> i64 {
    a % b
}