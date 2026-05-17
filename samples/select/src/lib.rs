#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn select_i32(flag: i32, left: i32, right: i32) -> i32 {
    if flag == 0 { left } else { right }
}