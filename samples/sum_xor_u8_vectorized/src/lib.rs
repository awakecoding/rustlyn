#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn sum_xor_u8(n: u8) -> u8 {
    let mut i = 0u8;
    let mut acc = 0u8;
    while i < n {
        acc = acc.wrapping_add(i ^ 3);
        i += 1;
    }
    acc
}