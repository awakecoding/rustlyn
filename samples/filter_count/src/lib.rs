#[no_mangle]
pub extern "C" fn filter_count_probe(n: i32) -> i32 {
    // Count even numbers in 1..=n
    (1..=n).filter(|x| x % 2 == 0).count() as i32
}
