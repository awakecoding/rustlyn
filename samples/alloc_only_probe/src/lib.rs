#![no_std]
#![feature(alloc_error_handler)]

extern crate alloc;

use alloc::vec::Vec;
use core::alloc::{GlobalAlloc, Layout};
use core::mem::align_of;
use core::ptr::{addr_of_mut, null_mut};

const HEAP_SIZE: usize = 256;

struct BumpAllocator;

#[repr(align(16))]
struct Heap([u8; HEAP_SIZE]);

static mut HEAP: Heap = Heap([0; HEAP_SIZE]);

unsafe impl GlobalAlloc for BumpAllocator {
    unsafe fn alloc(&self, layout: Layout) -> *mut u8 {
        if layout.size() > HEAP_SIZE || layout.align() > align_of::<Heap>() {
            return null_mut();
        }

        unsafe { addr_of_mut!(HEAP.0).cast::<u8>() }
    }

    unsafe fn dealloc(&self, _ptr: *mut u8, _layout: Layout) {}
}

#[global_allocator]
static ALLOCATOR: BumpAllocator = BumpAllocator;

#[alloc_error_handler]
fn alloc_error(_layout: Layout) -> ! {
    loop {}
}

#[panic_handler]
fn panic(_info: &core::panic::PanicInfo<'_>) -> ! {
    loop {}
}

#[unsafe(no_mangle)]
pub extern "C" fn alloc_vec_capacity_score() -> i32 {
    let values: Vec<i32> = Vec::with_capacity(4);

    values.capacity() as i32 + values.len() as i32
}
