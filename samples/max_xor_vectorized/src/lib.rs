#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn max_xor_i32(n: i32) -> i32 {
    let mut i = 0i32;
    let mut acc = i32::MIN;
    while i < n {
        let value = i ^ 3;
        if acc < value {
            acc = value;
        }
        i += 1;
    }
    acc
}
