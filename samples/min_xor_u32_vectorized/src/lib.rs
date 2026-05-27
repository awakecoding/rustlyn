#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_u32(n: u32) -> u32 {
    let mut i = 0u32;
    let mut acc = u32::MAX;
    while i < n {
        let value = i ^ 3;
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}
