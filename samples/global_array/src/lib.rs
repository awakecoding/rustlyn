#![no_std]

#[unsafe(no_mangle)]
pub static VALUES_I32: [i32; 2] = [10, 20];

#[unsafe(no_mangle)]
pub extern "C" fn read_second_i32() -> i32 {
    VALUES_I32[1]
}