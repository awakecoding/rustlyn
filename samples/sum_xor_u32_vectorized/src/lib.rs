#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn sum_xor_u32(n: u32) -> u32 {
    let mut i = 0u32;
    let mut acc = 0u32;
    while i < n {
        acc += i ^ 3;
        i += 1;
    }
    acc
}