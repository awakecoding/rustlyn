/// A nontrivial computation that won't inline away easily.
/// Uses a loop with state to prevent the optimizer from constant-folding.
#[inline(never)]
pub fn fibonacci(n: u32) -> u64 {
    if n == 0 {
        return 0;
    }
    let mut a: u64 = 0;
    let mut b: u64 = 1;
    for _ in 1..n {
        let next = a.wrapping_add(b);
        a = b;
        b = next;
    }
    b
}

/// Another nontrivial function to ensure dependency code is present.
#[inline(never)]
pub fn collatz_steps(mut n: u64) -> u32 {
    let mut steps = 0u32;
    while n > 1 {
        if n % 2 == 0 {
            n /= 2;
        } else {
            n = n.wrapping_mul(3).wrapping_add(1);
        }
        steps += 1;
    }
    steps
}
