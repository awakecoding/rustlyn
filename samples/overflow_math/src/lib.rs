#![no_std]

use core::panic::PanicInfo;

#[panic_handler]
fn panic(_info: &PanicInfo) -> ! {
    loop {}
}

/// Checked add that returns 0 on overflow, value otherwise
#[no_mangle]
pub extern "C" fn checked_add(a: i32, b: i32) -> i32 {
    match a.checked_add(b) {
        Some(v) => v,
        None => 0,
    }
}

/// Checked mul that returns -1 on overflow
#[no_mangle]
pub extern "C" fn checked_mul(a: i32, b: i32) -> i32 {
    match a.checked_mul(b) {
        Some(v) => v,
        None => -1,
    }
}

/// Saturating add
#[no_mangle]
pub extern "C" fn saturating_add(a: i32, b: i32) -> i32 {
    a.saturating_add(b)
}

/// Wrapping add (same as normal add but explicit)
#[no_mangle]
pub extern "C" fn wrapping_mul(a: i32, b: i32) -> i32 {
    a.wrapping_mul(b)
}

/// Count leading zeros
#[no_mangle]
pub extern "C" fn count_leading_zeros(x: u32) -> u32 {
    x.leading_zeros()
}

/// Count trailing zeros
#[no_mangle]
pub extern "C" fn count_trailing_zeros(x: u32) -> u32 {
    x.trailing_zeros()
}

/// Byte swap (endianness conversion)
#[no_mangle]
pub extern "C" fn byte_swap(x: u32) -> u32 {
    x.swap_bytes()
}

/// Combined probe: exercises all operations
/// checked_add(100, 200) = 300
/// checked_add(i32::MAX, 1) = 0 (overflow)
/// checked_mul(10, 20) = 200
/// saturating_add(i32::MAX, 100) = i32::MAX (but won't call in probe due to constant folding)
/// count_leading_zeros(1) = 31
/// count_trailing_zeros(8) = 3
/// Result: 300 + 0 + 200 + 31 + 3 = 534
#[no_mangle]
pub extern "C" fn overflow_probe() -> i32 {
    let a = checked_add(100, 200);        // 300
    let b = checked_add(i32::MAX, 1);     // 0 (overflow)
    let c = checked_mul(10, 20);          // 200
    let d = count_leading_zeros(1) as i32; // 31
    let e = count_trailing_zeros(8) as i32; // 3
    a + b + c + d + e                     // 534
}
