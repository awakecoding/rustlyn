// SPDX-License-Identifier: MIT
//
// Starter Rust binary translated to a managed .NET executable by Rustlyn.

fn main() {
    let n = 10u32;
    let mut a: u64 = 0;
    let mut b: u64 = 1;
    for _ in 0..n {
        let next = a + b;
        a = b;
        b = next;
    }
    println!("fib({}) = {}", n, a);
}
