#[no_mangle]
pub extern "C" fn bitops_probe(x: u32) -> i32 {
    let pop = x.count_ones() as i32;
    let lead = x.leading_zeros() as i32;
    let trail = x.trailing_zeros() as i32;
    pop + lead + trail
}
