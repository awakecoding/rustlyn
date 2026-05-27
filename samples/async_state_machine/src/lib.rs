#![no_std]

//! Demonstrates Rust async/await state machine patterns compiled to LLVM IR.
//! Without a full async runtime, we manually implement Future-like state machines
//! that exercise the same IR patterns (tagged enums for states, phi nodes for
//! state transitions, complex control flow for poll loops).

/// A manual state machine that simulates an async computation:
/// accumulates values across multiple "poll" steps.
/// State 0: initial, State 1: first step done, State 2: complete.
#[no_mangle]
pub extern "C" fn poll_accumulator(state: i32, input: i32) -> i64 {
    // Encode both next_state and accumulated value in return:
    // high 32 bits = next_state, low 32 bits = result
    match state {
        0 => {
            // Initial state: start accumulating
            let partial = input * 2;
            ((1i64) << 32) | (partial as i64 & 0xFFFFFFFF)
        }
        1 => {
            // Second poll: add squared input
            let partial = input + (input * input);
            ((2i64) << 32) | (partial as i64 & 0xFFFFFFFF)
        }
        2 => {
            // Terminal state: return final value
            (3i64 << 32) | (input as i64 & 0xFFFFFFFF)
        }
        _ => {
            // Invalid state: signal error
            (-1i64 << 32) | 0
        }
    }
}

/// Simulates a multi-step coroutine that computes fibonacci-like sequence
/// state-by-state (exercises phi nodes and loop-like IR patterns).
/// Returns packed (next_state, value) similar to generator yield pattern.
#[no_mangle]
pub extern "C" fn fibonacci_generator(state: i32, prev: i32, curr: i32) -> i64 {
    match state {
        0 => {
            // Yield first value (0)
            ((1i64) << 32) | 0
        }
        1 => {
            // Yield second value (1)
            ((2i64) << 32) | 1
        }
        _ => {
            // General step: yield prev + curr
            let next = prev.wrapping_add(curr);
            let next_state = if state >= 10 { -1 } else { state + 1 };
            ((next_state as i64) << 32) | (next as i64 & 0xFFFFFFFF)
        }
    }
}

/// A cooperative task scheduler simulation.
/// Given a task_id and current progress, returns updated progress.
/// Demonstrates the kind of switch-on-state pattern that async desugaring produces.
#[no_mangle]
pub extern "C" fn cooperative_step(task_id: i32, progress: i32) -> i32 {
    let work_per_step = match task_id % 4 {
        0 => 10,
        1 => 7,
        2 => 3,
        _ => 5,
    };

    let new_progress = progress + work_per_step;
    if new_progress >= 100 {
        100
    } else {
        new_progress
    }
}

/// Simulates awaiting multiple "futures" by tracking completion flags.
/// join_state is a bitmask of completed sub-tasks (up to 4 tasks).
/// Returns updated bitmask after attempting to complete next pending task.
#[no_mangle]
pub extern "C" fn join_poll(join_state: i32, step: i32) -> i32 {
    let task_mask = 0b1111;
    let current = join_state & task_mask;

    if current == task_mask {
        return join_state; // all complete
    }

    // Complete the next unfinished task based on step
    let task_to_complete = step % 4;
    let bit = 1 << task_to_complete;

    if current & bit == 0 {
        join_state | bit
    } else {
        // Task already complete, try next one
        let next = (task_to_complete + 1) % 4;
        join_state | (1 << next)
    }
}

/// Simulates a select/race between two "futures":
/// Returns the result of whichever "completes first" based on the ready flags.
#[no_mangle]
pub extern "C" fn select_first_ready(a_ready: i32, a_value: i32, b_ready: i32, b_value: i32) -> i32 {
    if a_ready != 0 {
        a_value
    } else if b_ready != 0 {
        b_value
    } else {
        // Neither ready — return sentinel
        -1
    }
}

/// State machine that models retry-with-backoff pattern.
/// Returns negative value while retrying (encoding remaining retries),
/// returns positive result on success.
#[no_mangle]
pub extern "C" fn retry_backoff(attempt: i32, max_attempts: i32, success_on: i32) -> i32 {
    if attempt >= max_attempts {
        -1 // exhausted retries
    } else if attempt == success_on {
        attempt * 10 + 42 // success result
    } else {
        // Still retrying: return negative remaining count
        -(max_attempts - attempt)
    }
}
