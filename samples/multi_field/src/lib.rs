#![no_std]

/// Exercises structs with more than two fields and nested aggregates.
/// Returns 300 if all checks pass.
#[no_mangle]
pub extern "C" fn multi_field_probe(x: i32) -> i32 {
    let mut total: i32 = 0;

    // 4-field struct via tuple
    let quad = (x, x * 2, x * 3, x * 4);
    total += quad.0 + quad.1 + quad.2 + quad.3; // 1+2+3+4 = 10

    // Nested struct: tuple of tuples
    let nested = ((x * 10, x * 20), (x * 30, x * 40));
    total += nested.0 .0 + nested.0 .1 + nested.1 .0 + nested.1 .1; // 10+20+30+40 = 100

    // Array indexing
    let arr = [x * 5, x * 15, x * 25, x * 45];
    total += arr[0] + arr[1] + arr[2] + arr[3]; // 5+15+25+45 = 90

    // Compute with struct fields
    let pair = (x * 50, x * 50);
    total += pair.0 + pair.1; // 50+50 = 100

    total // 10 + 100 + 90 + 100 = 300
}
