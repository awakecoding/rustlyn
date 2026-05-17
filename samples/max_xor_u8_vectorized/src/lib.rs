#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn max_xor_u8(n: u8) -> u8 {
    let mut i = 0u8;
    let mut acc = 0u8;
    while i < n {
        let value = i ^ 3;
        if acc < value {
            acc = value;
        }
        i += 1;
    }
    acc
}