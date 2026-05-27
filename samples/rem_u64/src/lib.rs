#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn rem_u64(a: u64, b: u64) -> u64 {
    a % b
}