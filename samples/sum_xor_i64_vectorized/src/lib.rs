#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn sum_xor_i64(n: i64) -> i64 {
    let mut i = 0i64;
    let mut acc = 0i64;
    while i < n {
        acc += i ^ 3;
        i += 1;
    }
    acc
}