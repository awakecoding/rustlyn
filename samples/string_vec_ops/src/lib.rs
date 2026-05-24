#![no_std]
#![feature(alloc_error_handler)]

//! Exercises Vec<T> operations — heap-owning types with Drop.
//! Uses #![no_std] with alloc to test Vec through build-std.

extern crate alloc;
use alloc::vec::Vec;
use core::alloc::{GlobalAlloc, Layout};
use core::mem::align_of;
use core::ptr::{addr_of_mut, null_mut};

// Bump allocator with enough space for Vec operations
const HEAP_SIZE: usize = 4096;

struct BumpAllocator;

#[repr(align(16))]
struct Heap([u8; HEAP_SIZE]);

static mut HEAP: Heap = Heap([0; HEAP_SIZE]);
static mut HEAP_OFFSET: usize = 0;

unsafe impl GlobalAlloc for BumpAllocator {
    unsafe fn alloc(&self, layout: Layout) -> *mut u8 {
        unsafe {
            let base = addr_of_mut!(HEAP_OFFSET);
            let offset = *base;
            let aligned = (offset + layout.align() - 1) & !(layout.align() - 1);
            let end = aligned + layout.size();
            if end > HEAP_SIZE || layout.align() > align_of::<Heap>() {
                return null_mut();
            }
            *base = end;
            addr_of_mut!(HEAP).cast::<u8>().add(aligned)
        }
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

/// Create a Vec, push elements, return the sum
/// sum(1..=n) = n*(n+1)/2, for n=5 => 15
#[unsafe(no_mangle)]
pub extern "C" fn vec_push_sum(n: i32) -> i32 {
    let mut v: Vec<i32> = Vec::new();
    for i in 1..=n {
        v.push(i);
    }
    let mut sum = 0i32;
    for &x in v.iter() {
        sum += x;
    }
    sum
}

/// Vec with_capacity: allocate capacity then push
/// Returns capacity * 100 + len
#[unsafe(no_mangle)]
pub extern "C" fn vec_capacity_len(n: i32) -> i32 {
    let mut v: Vec<i32> = Vec::with_capacity(n as usize);
    let cap = v.capacity() as i32;
    for i in 0..n {
        v.push(i * i);
    }
    cap * 100 + v.len() as i32
}

/// Vec: push squares, return sum
/// 1^2 + 2^2 + 3^2 + 4^2 + 5^2 = 55
#[unsafe(no_mangle)]
pub extern "C" fn vec_squares_sum() -> i32 {
    let mut v: Vec<i32> = Vec::new();
    for i in 1..=5 {
        v.push(i * i);
    }
    let mut sum = 0i32;
    for &x in v.iter() {
        sum += x;
    }
    sum
}

