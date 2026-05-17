#![no_std]

#[inline(never)]
fn add_one(x: i32) -> i32 {
    x + 1
}

#[inline(never)]
fn add_two(x: i32) -> i32 {
    x + 2
}

#[unsafe(no_mangle)]
pub extern "C" fn merge_call_i32(flag: i32, x: i32) -> i32 {
    let value = if flag == 0 { add_one(x) } else { add_two(x) };
    value * 2
}