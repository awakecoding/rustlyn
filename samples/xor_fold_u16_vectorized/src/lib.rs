#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn xor_fold_u16(n: u16) -> u16 {
    let mut i = 0u16;
    let mut acc = 0u16;
    while i < n {
        acc ^= i;
        i += 1;
    }
    acc
}