#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn and_fold_u64(n: u64) -> u64 {
    let mut i = 0u64;
    let mut acc = !0u64;
    while i < n {
        acc &= i;
        i += 1;
    }
    acc
}