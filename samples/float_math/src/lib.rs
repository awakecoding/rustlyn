#![no_std]
#![no_main]

#[panic_handler]
fn panic(_info: &core::panic::PanicInfo) -> ! {
    loop {}
}

// Use extern "C" libm functions that LLVM recognizes and lowers to intrinsics
extern "C" {
    fn sqrt(x: f64) -> f64;
    fn floor(x: f64) -> f64;
}

/// Compute sqrt(x*x + y*y) floored, using float intrinsics.
/// float_probe(3, 4) → floor(sqrt(9+16)) = floor(5.0) = 5
#[no_mangle]
pub extern "C" fn float_probe(x: i32, y: i32) -> i32 {
    let fx = x as f64;
    let fy = y as f64;
    let sum = fx * fx + fy * fy;
    let root = unsafe { sqrt(sum) };
    let floored = unsafe { floor(root) };
    floored as i32
}
