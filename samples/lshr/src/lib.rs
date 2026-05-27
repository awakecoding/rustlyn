#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn lshr_i32(a: u32, b: u32) -> u32 {
    a >> b
}