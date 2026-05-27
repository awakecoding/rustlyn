#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_sub_u8(n: u8) -> u8 {
    let mut i = 0u8;
    let mut acc = u8::MAX;
    while i < n {
        let value = (i ^ 85).wrapping_sub(7);
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}