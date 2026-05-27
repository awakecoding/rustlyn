#![no_std]

pub const PROFILE_PROBE_DEPENDENCY_SCORE: i32 = 29;

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