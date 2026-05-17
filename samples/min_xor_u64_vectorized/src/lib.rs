#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn min_xor_u64(n: u64) -> u64 {
    let mut i = 0u64;
    let mut acc = u64::MAX;
    while i < n {
        let value = i ^ 3;
        if acc > value {
            acc = value;
        }
        i += 1;
    }
    acc
}
