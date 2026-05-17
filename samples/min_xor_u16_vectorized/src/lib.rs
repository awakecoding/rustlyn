#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_u16(n: u16) -> u16 {
    let mut i = 0u16;
    let mut acc = u16::MAX;
    while i < n {
        let value = i ^ 3;
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}