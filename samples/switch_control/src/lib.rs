#![no_std]

use core::panic::PanicInfo;

#[panic_handler]
fn panic(_info: &PanicInfo) -> ! {
    loop {}
}

/// Match on multiple values (generates switch/br table)
#[no_mangle]
pub extern "C" fn day_code(day: i32) -> i32 {
    match day {
        1 => 10,
        2 => 20,
        3 => 30,
        4 => 40,
        5 => 50,
        6 => 60,
        7 => 70,
        _ => 0,
    }
}

/// Nested control flow with early returns
#[no_mangle]
pub extern "C" fn classify(x: i32) -> i32 {
    if x < 0 {
        -1
    } else if x == 0 {
        0
    } else if x < 10 {
        1
    } else if x < 100 {
        2
    } else {
        3
    }
}

/// Loop with conditional break (multi-block phi)
#[inline(never)]
#[no_mangle]
pub extern "C" fn sum_until(limit: i32) -> i32 {
    let mut sum = 0i32;
    let mut i = 1i32;
    while i <= limit {
        sum = sum.wrapping_add(i);
        i = i.wrapping_add(1);
    }
    sum
}

/// Combined probe
/// day_code(3) = 30
/// classify(-5) = -1
/// classify(0) = 0
/// classify(7) = 1
/// classify(50) = 2
/// classify(200) = 3
/// sum_until(10) = 55
/// Result: 30 + (-1) + 0 + 1 + 2 + 3 + 55 = 90
#[no_mangle]
pub extern "C" fn switch_probe() -> i32 {
    let a = day_code(3);
    let b = classify(-5);
    let c = classify(0);
    let d = classify(7);
    let e = classify(50);
    let f = classify(200);
    let g = sum_until(10);
    a + b + c + d + e + f + g
}

