fn is_even(n: u32) -> bool {
    if n == 0 { true } else { is_odd(n - 1) }
}

fn is_odd(n: u32) -> bool {
    if n == 0 { false } else { is_even(n - 1) }
}

#[no_mangle]
pub extern "C" fn mutual_recursion_probe(n: i32) -> i32 {
    if is_even(n as u32) { 1 } else { 0 }
}
