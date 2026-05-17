#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn or_fold_i32(n: i32) -> i32 {
    let mut i = 0i32;
    let mut acc = 0i32;
    while i < n {
        acc |= i;
        i += 1;
    }
    acc
}
