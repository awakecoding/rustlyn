#![no_std]
#![no_main]

#[panic_handler]
fn panic(_info: &core::panic::PanicInfo) -> ! {
    loop {}
}

/// Computes (a * b + c) mod 2^128, returning the lower 64 bits as i64.
/// a=3, b=5, c=7 → 3*5+7 = 22
#[inline(never)]
fn i128_mul_add(a: i128, b: i128, c: i128) -> i128 {
    a.wrapping_mul(b).wrapping_add(c)
}

/// Probe: pass in small values, get back the lower 32 bits
/// i128_probe(3) → i128_mul_add(3, 5, 7) = 22
#[no_mangle]
pub extern "C" fn i128_probe(x: i32) -> i32 {
    let result = i128_mul_add(x as i128, 5i128, 7i128);
    result as i32
}
