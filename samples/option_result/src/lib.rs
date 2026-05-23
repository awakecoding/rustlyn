/// Exercises Option<T> and Result<T,E> patterns in compiled Rust.
/// At opt-level 2 these may reduce to simple comparisons or remain as
/// extractvalue/insertvalue on { i32, i32 } aggregates.

fn checked_divide(a: i32, b: i32) -> Option<i32> {
    if b == 0 { None } else { Some(a / b) }
}

fn safe_add(a: i32, b: i32) -> Result<i32, i32> {
    let result = a.checked_add(b);
    match result {
        Some(v) => Ok(v),
        None => Err(-1),
    }
}

/// Returns: checked_divide(100, x) unwrap_or(0) + safe_add(x, 40) unwrap_or(0)
/// For x=5: 100/5=20, 5+40=45 => 65
#[unsafe(no_mangle)]
pub extern "C" fn option_result_probe(x: i32) -> i32 {
    let div_result = checked_divide(100, x).unwrap_or(0);
    let add_result = safe_add(x, 40).unwrap_or(0);
    div_result + add_result
}
