#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn max_xor_sub_i8(n: i8) -> i8 {
    let mut i = 0i8;
    let mut acc = i8::MIN;
    while i < n {
        let value = (i ^ 85).wrapping_sub(7);
        if acc < value {
            acc = value;
        }
        i += 1;
    }
    acc
}