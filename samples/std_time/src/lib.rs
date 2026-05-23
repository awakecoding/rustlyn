use std::time::{Duration, Instant};

/// Returns a positive score if monotonic clock and duration operations work, or a negative error code.
#[unsafe(no_mangle)]
pub extern "C" fn std_time_probe() -> i32 {
    // Instant::now() should not panic
    let start = Instant::now();

    // Do a small amount of work to ensure elapsed time > 0
    let mut accumulator: u64 = 0;
    for i in 0u64..1000 {
        accumulator = accumulator.wrapping_add(i);
    }

    let elapsed = start.elapsed();

    // Elapsed should be non-negative (Duration is always >= 0)
    if elapsed.as_nanos() == 0 && accumulator == 0 {
        // Edge case: if the loop was optimized away and time is 0, still pass
        // but this shouldn't happen with wrapping_add
        return -1;
    }

    // Duration arithmetic
    let one_sec = Duration::from_secs(1);
    let half_sec = Duration::from_millis(500);
    let sum = one_sec + half_sec;

    if sum.as_millis() != 1500 {
        return -2;
    }

    if sum.as_secs() != 1 {
        return -3;
    }

    if sum.subsec_millis() != 500 {
        return -4;
    }

    // Duration comparison
    if one_sec >= sum {
        return -5;
    }

    if half_sec >= one_sec {
        return -6;
    }

    // Duration from parts
    let from_nanos = Duration::from_nanos(1_000_000_000);
    if from_nanos != one_sec {
        return -7;
    }

    // Return accumulator mod as success indicator (always > 0 for this range)
    let result = (accumulator % 100) as i32 + 1;
    result
}
