#![no_std]

#[repr(C)]
pub struct PairI64 {
    pub left: i64,
    pub right: i64,
}

#[unsafe(no_mangle)]
pub extern "C" fn second_field_i64(pair: PairI64) -> i64 {
    pair.right
}