enum Op {
    Add(i32),
    Mul(i32),
    Negate,
}

fn apply_op(op: Op, x: i32) -> i32 {
    match op {
        Op::Add(v) => x + v,
        Op::Mul(v) => x * v,
        Op::Negate => -x,
    }
}

#[no_mangle]
pub extern "C" fn enum_match_probe(x: i32) -> i32 {
    let a = apply_op(Op::Add(10), x);
    let b = apply_op(Op::Mul(3), a);
    apply_op(Op::Negate, b)
}
