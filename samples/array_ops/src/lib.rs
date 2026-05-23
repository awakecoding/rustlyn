#[no_mangle]
pub extern "C" fn array_ops_probe(n: i32) -> i32 {
    let arr = [10, 20, 30, 40, 50];
    let mut sum = 0i32;
    let limit = if n > 5 { 5 } else { n as usize };
    let mut i = 0;
    while i < limit {
        sum += arr[i];
        i += 1;
    }
    sum
}
