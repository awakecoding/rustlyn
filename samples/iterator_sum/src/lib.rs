#[no_mangle]
pub extern "C" fn iterator_sum_probe(n: i32) -> i32 {
    // Sum of squares: 1^2 + 2^2 + ... + n^2
    (1..=n).map(|x| x * x).sum()
}
