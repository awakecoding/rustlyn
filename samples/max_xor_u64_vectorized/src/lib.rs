#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn max_xor_u64(n: u64) -> u64 {
    let mut i = 0u64;
    let mut acc = 0u64;
    while i < n {
        let value = i ^ 3;
        if acc < value {
            acc = value;
        }
        i += 1;
    }
    acc
}
