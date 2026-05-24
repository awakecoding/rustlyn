/// Exercises complex enum patterns beyond simple Option/Result:
/// - Multi-field variants
/// - Nested enums
/// - Exhaustive matching with payload extraction
/// - Enum-returning functions composed together

#[repr(C)]
struct Point {
    x: i32,
    y: i32,
}

enum Shape {
    Circle { radius: i32 },
    Rectangle { width: i32, height: i32 },
    Triangle { base: i32, height: i32 },
    Empty,
}

fn area(shape: &Shape) -> i32 {
    match shape {
        Shape::Circle { radius } => radius * radius * 3,
        Shape::Rectangle { width, height } => width * height,
        Shape::Triangle { base, height } => base * height / 2,
        Shape::Empty => 0,
    }
}

enum Expr {
    Literal(i32),
    Add(i32, i32),
    Mul(i32, i32),
    Negate(i32),
}

fn eval(expr: &Expr) -> i32 {
    match expr {
        Expr::Literal(v) => *v,
        Expr::Add(a, b) => a + b,
        Expr::Mul(a, b) => a * b,
        Expr::Negate(v) => -*v,
    }
}

/// area(Circle{5}) + area(Rect{3,4}) + area(Tri{6,4}) + area(Empty)
/// = 75 + 12 + 12 + 0 = 99
#[unsafe(no_mangle)]
pub extern "C" fn enum_complex_shapes() -> i32 {
    let shapes = [
        Shape::Circle { radius: 5 },
        Shape::Rectangle { width: 3, height: 4 },
        Shape::Triangle { base: 6, height: 4 },
        Shape::Empty,
    ];
    let mut total = 0i32;
    for shape in &shapes {
        total += area(shape);
    }
    total
}

/// eval(Literal(10)) + eval(Add(3,4)) + eval(Mul(2,5)) + eval(Negate(3))
/// = 10 + 7 + 10 + (-3) = 24
#[unsafe(no_mangle)]
pub extern "C" fn enum_complex_exprs() -> i32 {
    let exprs = [
        Expr::Literal(10),
        Expr::Add(3, 4),
        Expr::Mul(2, 5),
        Expr::Negate(3),
    ];
    let mut total = 0i32;
    for expr in &exprs {
        total += eval(expr);
    }
    total
}

/// Nested Option matching: Some(Some(x)) -> x, Some(None) -> -1, None -> -2
#[unsafe(no_mangle)]
pub extern "C" fn enum_complex_nested_option(x: i32) -> i32 {
    let nested: Option<Option<i32>> = if x > 0 {
        Some(Some(x * 10))
    } else if x == 0 {
        Some(None)
    } else {
        None
    };

    match nested {
        Some(Some(v)) => v,
        Some(None) => -1,
        None => -2,
    }
}

/// Result chain: divide then add, propagating errors as discriminant values
fn checked_div(a: i32, b: i32) -> Result<i32, i32> {
    if b == 0 { Err(-1) } else { Ok(a / b) }
}

fn chain_ops(a: i32, b: i32, c: i32) -> Result<i32, i32> {
    let step1 = checked_div(a, b)?;
    let step2 = checked_div(step1, c)?;
    Ok(step2 + 1)
}

/// chain_ops(100, 5, 2) = Ok(100/5=20, 20/2=10, 10+1=11) -> 11
/// chain_ops(100, 0, 2) = Err(-1) -> -1
#[unsafe(no_mangle)]
pub extern "C" fn enum_complex_result_chain(divisor: i32) -> i32 {
    match chain_ops(100, divisor, 2) {
        Ok(v) => v,
        Err(e) => e,
    }
}
