#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn and_fold_i64(n: i64) -> i64 {
    let mut i = 0i64;
    let mut acc = -1i64;
    while i < n {
        acc &= i;
        i += 1;
    }
    acc
}