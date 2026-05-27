#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_i8(n: i8) -> i8 {
    let mut i = 0i8;
    let mut acc = i8::MAX;
    while i < n {
        let value = i ^ 3;
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}