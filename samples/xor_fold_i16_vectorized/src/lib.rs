#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn xor_fold_i16(n: i16) -> i16 {
    let mut i = 0i16;
    let mut acc = 0i16;
    while i < n {
        acc ^= i;
        i += 1;
    }
    acc
}