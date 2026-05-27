#![no_std]

//! Demonstrates generic collection patterns that exercise Rust monomorphization.
//! Uses no_std with fixed-size inline collections to avoid requiring an allocator
//! while still producing heavily monomorphized IR through generic type parameters.

/// A fixed-capacity stack (LIFO) generic over element type.
/// Monomorphizes to distinct IR for each concrete T used.
struct FixedStack<T: Copy + Default, const N: usize> {
    data: [T; N],
    len: usize,
}

impl<T: Copy + Default, const N: usize> FixedStack<T, N> {
    const fn new() -> Self {
        Self {
            data: [T::DEFAULT; N],
            len: 0,
        }
    }

    fn push(&mut self, val: T) -> bool {
        if self.len >= N {
            return false;
        }
        self.data[self.len] = val;
        self.len += 1;
        true
    }

    fn pop(&mut self) -> Option<T> {
        if self.len == 0 {
            None
        } else {
            self.len -= 1;
            Some(self.data[self.len])
        }
    }

    fn peek(&self) -> Option<T> {
        if self.len == 0 {
            None
        } else {
            Some(self.data[self.len - 1])
        }
    }

    fn len(&self) -> usize {
        self.len
    }
}

/// A generic pair that exercises monomorphization with different type combinations.
#[derive(Clone, Copy)]
struct Pair<A: Copy, B: Copy> {
    first: A,
    second: B,
}

impl<A: Copy, B: Copy> Pair<A, B> {
    const fn new(first: A, second: B) -> Self {
        Self { first, second }
    }
}

/// Uses FixedStack<i32, 8> — exercises monomorphized stack with 32-bit integers.
/// Pushes values, pops them (LIFO order), returns sum of popped values.
#[no_mangle]
pub extern "C" fn stack_i32_push_pop(a: i32, b: i32, c: i32) -> i32 {
    let mut stack = FixedStack::<i32, 8>::new();
    stack.push(a);
    stack.push(b);
    stack.push(c);

    let mut sum = 0i32;
    while let Some(val) = stack.pop() {
        sum = sum.wrapping_add(val);
    }
    sum
}

/// Uses FixedStack<i64, 4> — different monomorphization of the same generic.
/// Demonstrates that the compiler generates distinct code for i64 variant.
#[no_mangle]
pub extern "C" fn stack_i64_operations(x: i64, y: i64) -> i64 {
    let mut stack = FixedStack::<i64, 4>::new();
    stack.push(x);
    stack.push(y);
    stack.push(x.wrapping_add(y));

    // Peek without consuming
    let top = stack.peek().unwrap_or(0);

    // Pop all and multiply
    let mut product = 1i64;
    while let Some(val) = stack.pop() {
        product = product.wrapping_mul(val);
    }

    top.wrapping_add(product)
}

/// Exercises generic Pair with (i32, i32) — swap and combine.
#[no_mangle]
pub extern "C" fn pair_swap_sum(a: i32, b: i32) -> i32 {
    let p = Pair::new(a, b);
    let swapped = Pair::new(p.second, p.first);
    swapped.first.wrapping_add(swapped.second).wrapping_mul(2)
}

/// Exercises Pair<i32, i64> — mixed-width pair operations.
#[no_mangle]
pub extern "C" fn pair_mixed_width(x: i32, y: i64) -> i64 {
    let p = Pair::new(x, y);
    (p.first as i64).wrapping_add(p.second)
}

/// Generic linear search over a fixed array — monomorphized for i32.
/// Demonstrates generic algorithm patterns.
fn linear_search<T: Copy + PartialEq, const N: usize>(arr: &[T; N], target: T) -> i32 {
    let mut i = 0;
    while i < N {
        if arr[i] == target {
            return i as i32;
        }
        i += 1;
    }
    -1
}

/// Uses generic linear_search with a small i32 array.
#[no_mangle]
pub extern "C" fn search_in_array(target: i32) -> i32 {
    let arr = [10, 20, 30, 40, 50, 60, 70, 80];
    linear_search(&arr, target)
}

/// Generic min/max finder — demonstrates trait-bounded generics.
fn find_min_max<T: Copy + PartialOrd>(a: T, b: T, c: T) -> (T, T) {
    let min = if a < b {
        if a < c { a } else { c }
    } else {
        if b < c { b } else { c }
    };
    let max = if a > b {
        if a > c { a } else { c }
    } else {
        if b > c { b } else { c }
    };
    (min, max)
}

/// Monomorphized min_max for i32, returns min + max.
#[no_mangle]
pub extern "C" fn min_max_sum_i32(a: i32, b: i32, c: i32) -> i32 {
    let (min, max) = find_min_max(a, b, c);
    min.wrapping_add(max)
}

/// Monomorphized min_max for i64.
#[no_mangle]
pub extern "C" fn min_max_sum_i64(a: i64, b: i64, c: i64) -> i64 {
    let (min, max) = find_min_max(a, b, c);
    min.wrapping_add(max)
}

/// A fixed-capacity ring buffer — demonstrates generic with index wrapping.
struct RingBuffer<T: Copy + Default, const N: usize> {
    data: [T; N],
    head: usize,
    tail: usize,
    count: usize,
}

impl<T: Copy + Default, const N: usize> RingBuffer<T, N> {
    const fn new() -> Self {
        Self {
            data: [T::DEFAULT; N],
            head: 0,
            tail: 0,
            count: 0,
        }
    }

    fn enqueue(&mut self, val: T) -> bool {
        if self.count >= N {
            return false;
        }
        self.data[self.tail] = val;
        self.tail = (self.tail + 1) % N;
        self.count += 1;
        true
    }

    fn dequeue(&mut self) -> Option<T> {
        if self.count == 0 {
            return None;
        }
        let val = self.data[self.head];
        self.head = (self.head + 1) % N;
        self.count -= 1;
        Some(val)
    }
}

/// Ring buffer FIFO test: enqueue 3 values, dequeue them (should be in FIFO order).
/// Returns sum of dequeued values to verify ordering doesn't matter for sum
/// but the internal ring mechanics are exercised.
#[no_mangle]
pub extern "C" fn ring_buffer_fifo(a: i32, b: i32, c: i32) -> i32 {
    let mut ring = RingBuffer::<i32, 4>::new();
    ring.enqueue(a);
    ring.enqueue(b);
    ring.enqueue(c);

    let mut sum = 0i32;
    while let Some(val) = ring.dequeue() {
        sum = sum.wrapping_add(val);
    }
    sum
}

// Trait needed for Default on our types in const context
trait Default: Copy {
    const DEFAULT: Self;
}

impl Default for i32 {
    const DEFAULT: Self = 0;
}

impl Default for i64 {
    const DEFAULT: Self = 0;
}

impl Default for u8 {
    const DEFAULT: Self = 0;
}
