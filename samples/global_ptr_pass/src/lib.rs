#![no_std]

static mut COUNTER: i32 = 0;

#[inline(never)]
unsafe fn store_to_ptr(dst: *mut i32, val: i32) {
    *dst = val;
}

#[inline(never)]
unsafe fn load_from_ptr(src: *const i32) -> i32 {
    *src
}

#[no_mangle]
pub extern "C" fn global_ptr_pass_probe(x: i32) -> i32 {
    unsafe {
        store_to_ptr(&raw mut COUNTER, x);
        load_from_ptr(&raw const COUNTER)
    }
}

#[panic_handler]
fn panic(_info: &core::panic::PanicInfo) -> ! { loop {} }
