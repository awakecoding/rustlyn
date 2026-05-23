/// Exercises cross-crate function calls that must survive in bitcode.
/// Returns fib(10) + collatz_steps(27) as a combined result.
#[unsafe(no_mangle)]
pub extern "C" fn dep_heavy_probe() -> i32 {
    let fib_result = dep_lib::fibonacci(10);
    let collatz_result = dep_lib::collatz_steps(27);

    // fib(10) = 55, collatz_steps(27) = 111
    (fib_result as i32) + (collatz_result as i32)
}
