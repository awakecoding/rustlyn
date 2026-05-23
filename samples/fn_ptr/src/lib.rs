fn double(x: i32) -> i32 {
    x * 2
}

fn triple(x: i32) -> i32 {
    x * 3
}

fn apply(f: fn(i32) -> i32, x: i32) -> i32 {
    f(x)
}

/// Probe: apply(double, 5) + apply(triple, 3) = 10 + 9 = 19
#[unsafe(no_mangle)]
pub extern "C" fn fn_ptr_probe() -> i32 {
    apply(double, 5) + apply(triple, 3)
}
