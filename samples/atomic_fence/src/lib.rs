#![no_std]
#![no_main]

use core::sync::atomic::{AtomicI32, Ordering, fence};

#[panic_handler]
fn panic(_info: &core::panic::PanicInfo) -> ! {
    loop {}
}

static COUNTER: AtomicI32 = AtomicI32::new(0);

/// Atomically increment a global counter, with a fence for ordering.
/// atomic_fence_probe(5) → 5 (first call adds 5 to 0)
#[no_mangle]
pub extern "C" fn atomic_fence_probe(x: i32) -> i32 {
    COUNTER.store(0, Ordering::Relaxed);
    fence(Ordering::SeqCst);
    COUNTER.fetch_add(x, Ordering::SeqCst);
    fence(Ordering::SeqCst);
    COUNTER.load(Ordering::SeqCst)
}
