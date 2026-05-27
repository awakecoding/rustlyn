#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_eq_u32(a: u32, b: u32) -> u32 {
    if a <= b { a } else { b }
}