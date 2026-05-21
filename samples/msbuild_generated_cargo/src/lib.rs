#![no_std]

#[unsafe(no_mangle)]
pub extern "C" fn generated_cargo_score() -> i32 {
    profile_probe::PROFILE_PROBE_DEPENDENCY_SCORE + 31
}