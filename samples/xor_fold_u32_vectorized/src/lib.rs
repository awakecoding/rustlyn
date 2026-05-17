#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn xor_fold_u32(n: u32) -> u32 {
    let mut i = 0u32;
    let mut acc = 0u32;
    while i < n {
        acc ^= i;
        i += 1;
    }
    acc
}