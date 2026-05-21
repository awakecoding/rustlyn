#![no_std]

#[cfg(debug_assertions)]
#[unsafe(no_mangle)]
pub extern "C" fn profile_probe_score() -> i32 {
    11
}

#[cfg(not(debug_assertions))]
#[unsafe(no_mangle)]
pub extern "C" fn profile_probe_score() -> i32 {
    29
}