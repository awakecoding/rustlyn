#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn second_of_pair_i32(a: i32, b: i32) -> i32 {
    let values = [a, b];
    values[1]
}