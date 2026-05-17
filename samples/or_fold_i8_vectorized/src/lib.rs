#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn or_fold_i8(n: i8) -> i8 {
    let mut i = 0i8;
    let mut acc = 0i8;
    while i < n {
        acc |= i;
        i += 1;
    }
    acc
}