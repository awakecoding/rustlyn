/// Exercises three-element aggregate returns (tuples with 3+ fields).
/// At opt-level 1 these may survive as extractvalue { i32, i32, i32 }.

#[inline(never)]
fn compute_triple(x: i32) -> (i32, i32, i32) {
    (x + 1, x * 2, x * 3)
}

/// Returns sum of triple components: (x+1) + (x*2) + (x*3) = 6x + 1
/// For x=10: 11 + 20 + 30 = 61
#[unsafe(no_mangle)]
pub extern "C" fn triple_return_probe(x: i32) -> i32 {
    let (a, b, c) = compute_triple(x);
    a + b + c
}
