#![no_std]

use core::sync::atomic::{AtomicI32, Ordering};

/// Exercises atomic load, store, fetch_add, and compare_exchange.
/// Returns 75 if all checks pass.
#[no_mangle]
pub extern "C" fn atomics_probe() -> i32 {
    let counter = AtomicI32::new(0);
    let mut total: i32 = 0;

    // store + load
    counter.store(10, Ordering::SeqCst);
    total += counter.load(Ordering::SeqCst); // +10 => 10

    // fetch_add returns previous value
    let prev = counter.fetch_add(5, Ordering::SeqCst); // prev=10, counter=15
    total += prev; // +10 => 20

    // load the updated value
    total += counter.load(Ordering::SeqCst); // +15 => 35

    // compare_exchange success: 15 -> 25
    let result = counter.compare_exchange(15, 25, Ordering::SeqCst, Ordering::SeqCst);
    match result {
        Ok(old) => total += old,  // +15 => 50
        Err(_) => {}
    }

    // load after successful CAS
    total += counter.load(Ordering::SeqCst); // +25 => 75

    total
}
