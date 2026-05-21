#![no_std]

cfg_if::cfg_if! {
    if #[cfg(target_pointer_width = "64")] {
        const CFG_IF_SCORE: i32 = 17;
    } else {
        const CFG_IF_SCORE: i32 = 3;
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn generated_cargo_score() -> i32 {
    profile_probe::PROFILE_PROBE_DEPENDENCY_SCORE + CFG_IF_SCORE + 31
}
