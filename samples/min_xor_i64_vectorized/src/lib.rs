#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_i64(n: i64) -> i64 {
    let mut i = 0i64;
    let mut acc = i64::MAX;
    while i < n {
        let value = i ^ 3;
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}
