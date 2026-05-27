#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn max_add_sub_i8(n: i8) -> i8 {
    let mut i = 0i8;
    let mut acc = i8::MIN;
    while i < n {
        let value = i.wrapping_add(45).wrapping_sub(7);
        if acc < value {
            acc = value;
        }
        i += 1;
    }
    acc
}