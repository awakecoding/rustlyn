#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn sext_i32_to_i64(a: i32) -> i64 {
    a as i64
}