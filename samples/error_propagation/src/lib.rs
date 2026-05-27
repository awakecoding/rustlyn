/// Exercises the ? operator for Result propagation — the most common
/// Rust error handling pattern. Compiles to branch-heavy IR with early returns.

fn parse_digit(c: u8) -> Result<i32, i32> {
    if c >= b'0' && c <= b'9' {
        Ok((c - b'0') as i32)
    } else {
        Err(-1)
    }
}

fn parse_two_digits(high: u8, low: u8) -> Result<i32, i32> {
    let h = parse_digit(high)?;
    let l = parse_digit(low)?;
    Ok(h * 10 + l)
}

fn parse_and_add(a_hi: u8, a_lo: u8, b_hi: u8, b_lo: u8) -> Result<i32, i32> {
    let a = parse_two_digits(a_hi, a_lo)?;
    let b = parse_two_digits(b_hi, b_lo)?;
    Ok(a + b)
}

/// parse_and_add('4','2','1','7') = Ok(42 + 17) = 59
#[unsafe(no_mangle)]
pub extern "C" fn error_prop_success() -> i32 {
    match parse_and_add(b'4', b'2', b'1', b'7') {
        Ok(v) => v,
        Err(e) => e,
    }
}

/// parse_and_add('4','x','1','7') = Err(-1) propagated from parse_digit
#[unsafe(no_mangle)]
pub extern "C" fn error_prop_early_fail() -> i32 {
    match parse_and_add(b'4', b'x', b'1', b'7') {
        Ok(v) => v,
        Err(e) => e,
    }
}

/// Multi-step pipeline with ? at each stage
fn validate_range(v: i32, min: i32, max: i32) -> Result<i32, i32> {
    if v < min { Err(-2) }
    else if v > max { Err(-3) }
    else { Ok(v) }
}

fn pipeline(input: i32) -> Result<i32, i32> {
    let step1 = validate_range(input, 0, 100)?;
    let step2 = validate_range(step1 * 2, 0, 150)?;
    let step3 = validate_range(step2 + 10, 50, 200)?;
    Ok(step3)
}

/// pipeline(30) -> step1=30, step2=60, step3=70 -> Ok(70)
#[unsafe(no_mangle)]
pub extern "C" fn error_prop_pipeline_ok() -> i32 {
    match pipeline(30) {
        Ok(v) => v,
        Err(e) => e,
    }
}

/// pipeline(80) -> step1=80, step2=160 > 150 -> Err(-3)
#[unsafe(no_mangle)]
pub extern "C" fn error_prop_pipeline_fail() -> i32 {
    match pipeline(80) {
        Ok(v) => v,
        Err(e) => e,
    }
}

/// pipeline(-5) -> step1 fails: -5 < 0 -> Err(-2)
#[unsafe(no_mangle)]
pub extern "C" fn error_prop_pipeline_early() -> i32 {
    match pipeline(-5) {
        Ok(v) => v,
        Err(e) => e,
    }
}
