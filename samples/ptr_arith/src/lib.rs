#![no_std]

/// Exercises pointer arithmetic, ptr-to-int, int-to-ptr, and offset calculations.
/// Returns 100 if all checks pass.
#[no_mangle]
pub extern "C" fn ptr_arith_probe() -> i32 {
    let mut buf: [i32; 4] = [10, 20, 30, 40];
    let mut total: i32 = 0;

    // Basic pointer offset reads
    let base = buf.as_ptr();
    unsafe {
        total += *base;           // 10
        total += *base.add(1);    // 20
        total += *base.add(2);    // 30
        total += *base.add(3);    // 40
    }
    // total = 100

    // Pointer write via offset
    let base_mut = buf.as_mut_ptr();
    unsafe {
        *base_mut.add(0) = 5;
        *base_mut.add(1) = 15;
    }

    // Verify writes
    total += buf[0]; // 5
    total += buf[1]; // 15
    // total = 120

    // ptr-to-int round trip: verify address difference
    let addr0 = base as usize;
    let addr1 = unsafe { base.add(1) } as usize;
    let stride = addr1 - addr0; // should be 4 (size_of::<i32>())
    total += stride as i32; // 4
    // total = 124

    // Subtract to get back a known value
    total -= 24;
    // total = 100

    total
}
