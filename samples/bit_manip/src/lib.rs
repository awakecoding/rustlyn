#![no_std]

/// Exercises bit manipulation intrinsics: reverse_bits, saturating_sub.
/// Call with input=1 to get 200.
#[no_mangle]
pub extern "C" fn bit_manip_probe(input: u32) -> i32 {
    let mut total: i32 = 0;

    // reverse_bits: (0x80000000 * input).reverse_bits() == input
    let reversed = (0x80000000u32.wrapping_mul(input)).reverse_bits();
    if reversed == input {
        total += 50;
    }

    // reverse_bits: input.reverse_bits() == 0x80000000
    let reversed2 = input.reverse_bits();
    if reversed2 == 0x80000000 {
        total += 50;
    }

    // saturating_sub: (10 * input).saturating_sub(3) == 7
    let sat_sub = (10u32.wrapping_mul(input)).saturating_sub(3);
    if sat_sub == 7 {
        total += 25;
    }

    // saturating_sub underflow clamps to 0: (5 * input).saturating_sub(100) == 0
    let sat_sub_clamp = (5u32.wrapping_mul(input)).saturating_sub(100);
    if sat_sub_clamp == 0 {
        total += 25;
    }

    // saturating_sub signed: (-100 * input as i32).saturating_sub(i32::MAX) == i32::MIN
    let sat_sub_signed = (-100i32).wrapping_mul(input as i32).saturating_sub(i32::MAX);
    if sat_sub_signed == i32::MIN {
        total += 50;
    }

    total
}
