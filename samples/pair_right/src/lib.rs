#![no_std]

#[repr(C)]
pub struct PairI32 {
    pub left: i32,
    pub right: i32,
}

#[unsafe(no_mangle)]
pub extern "C" fn second_field_i32(pair: PairI32) -> i32 {
    pair.right
}