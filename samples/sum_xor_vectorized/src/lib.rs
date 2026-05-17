#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn sum_xor_i32(n: i32) -> i32 {
    let mut i = 0;
    let mut acc = 0;
    while i < n {
        acc += i ^ 3;
        i += 1;
    }
    acc
}