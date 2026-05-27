#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_i16(n: i16) -> i16 {
    let mut i = 0i16;
    let mut acc = i16::MAX;
    while i < n {
        let value = i ^ 3;
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}
