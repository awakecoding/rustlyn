#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn add_one_i32(value: i32) -> i32 {
    value + 1
}

#[unsafe(no_mangle)]
pub extern "C" fn call_chain_i32(value: i32) -> i32 {
    add_one_i32(value)
}
