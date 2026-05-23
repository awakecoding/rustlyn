#![no_std]

/// Maps a grade (0-4) to a score using a match that LLVM may lower to a lookup table.
/// Returns the sum of scores for grades 0..5 = 50+60+70+80+90 = 350.
#[no_mangle]
pub extern "C" fn lookup_table_probe() -> i32 {
    let mut total: i32 = 0;
    let mut i: u8 = 0;
    while i < 5 {
        total += grade_score(i);
        i += 1;
    }
    total
}

#[inline(never)]
fn grade_score(grade: u8) -> i32 {
    match grade {
        0 => 50,
        1 => 60,
        2 => 70,
        3 => 80,
        4 => 90,
        _ => 0,
    }
}
