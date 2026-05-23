#[no_mangle]
pub extern "C" fn recursion_probe(n: i32) -> i32 {
    fib(n)
}

fn fib(n: i32) -> i32 {
    if n <= 1 {
        n
    } else {
        fib(n - 1) + fib(n - 2)
    }
}
