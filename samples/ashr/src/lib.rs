#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn ashr_i32(a: i32, b: u32) -> i32 {
    a >> b
}