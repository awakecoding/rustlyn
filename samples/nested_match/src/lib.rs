#[no_mangle]
pub extern "C" fn nested_match_probe(x: i32, y: i32) -> i32 {
    match x {
        0 => match y {
            0 => 0,
            1 => 10,
            _ => 20,
        },
        1 => match y {
            0 => 100,
            1 => 110,
            _ => 120,
        },
        _ => x * 1000 + y,
    }
}
