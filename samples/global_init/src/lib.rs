#![no_std]

#[unsafe(no_mangle)]
pub static GLOBAL_INIT_I32: i32 = 7;

#[unsafe(no_mangle)]
pub extern "C" fn read_global_i32() -> i32 {
    GLOBAL_INIT_I32
}
