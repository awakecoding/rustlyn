#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn mask_lt_i64(a: i64, b: i64) -> i64 {
    if a < b { -1 } else { 0 }
}