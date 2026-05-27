#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn and_fold_u8(n: u8) -> u8 {
    let mut i = 0u8;
    let mut acc = !0u8;
    while i < n {
        acc &= i;
        i += 1;
    }
    acc
}