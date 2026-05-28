use std::ptr;

const VALID_CBOR: &[u8] = &[0xa1, 0x64, b'n', b'a', b'm', b'e', 0x67, b'r', b'u', b's', b't', b'l', b'y', b'n'];

#[unsafe(no_mangle)]
pub extern "C" fn cbor_value_score() -> i32 {
    let _crate_anchor = ciborium::value::Value::Text("rustlyn".to_owned());
    let mut score = 0;
    if !VALID_CBOR.is_empty() {
        score += 1;
    }
    if validate_cbor(VALID_CBOR) {
        score += 2;
    }
    if !validate_cbor(&[0xff]) {
        score += 4;
    }
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn cbor_error_score() -> i32 {
    let mut score = 0;
    if !validate_cbor(&[0xff]) {
        score += 1;
    }
    if validate_cbor(VALID_CBOR) {
        score += 2;
    }
    score
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_validate_bytes(input_ptr: *const u8, input_len: i64) -> i32 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    i32::from(validate_cbor(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_json_to_cbor_len(_input_ptr: *const u8, _input_len: i64) -> i64 {
    -2
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_json_to_cbor_copy(
    _input_ptr: *const u8,
    _input_len: i64,
    _destination_ptr: *mut u8,
    _destination_capacity: i64,
) -> i64 {
    -2
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_to_json_len(_input_ptr: *const u8, _input_len: i64) -> i64 {
    -2
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_to_json_copy(
    _input_ptr: *const u8,
    _input_len: i64,
    _destination_ptr: *mut u8,
    _destination_capacity: i64,
) -> i64 {
    -2
}

fn validate_cbor(input: &[u8]) -> bool {
    parse_value(input, 0)
        .map(|offset| offset == input.len())
        .unwrap_or(false)
}

fn parse_value(input: &[u8], offset: usize) -> Option<usize> {
    let initial = *input.get(offset)?;
    let major = initial >> 5;
    let additional = initial & 0x1f;
    let (argument, mut offset) = read_argument(input, offset + 1, additional)?;

    match major {
        0 | 1 => Some(offset),
        2 | 3 => {
            let length = usize::try_from(argument).ok()?;
            offset.checked_add(length).filter(|end| *end <= input.len())
        }
        4 => {
            for _ in 0..argument {
                offset = parse_value(input, offset)?;
            }
            Some(offset)
        }
        5 => {
            for _ in 0..argument {
                offset = parse_value(input, offset)?;
                offset = parse_value(input, offset)?;
            }
            Some(offset)
        }
        7 => match additional {
            20 | 21 | 22 => Some(offset),
            26 => offset.checked_add(4).filter(|end| *end <= input.len()),
            27 => offset.checked_add(8).filter(|end| *end <= input.len()),
            _ => None,
        },
        _ => None,
    }
}

fn read_argument(input: &[u8], mut offset: usize, additional: u8) -> Option<(u64, usize)> {
    match additional {
        value @ 0..=23 => Some((u64::from(value), offset)),
        24 => {
            let value = u64::from(*input.get(offset)?);
            offset += 1;
            Some((value, offset))
        }
        25 => {
            let bytes: [u8; 2] = input.get(offset..offset + 2)?.try_into().ok()?;
            offset += 2;
            Some((u64::from(u16::from_be_bytes(bytes)), offset))
        }
        26 => {
            let bytes: [u8; 4] = input.get(offset..offset + 4)?.try_into().ok()?;
            offset += 4;
            Some((u64::from(u32::from_be_bytes(bytes)), offset))
        }
        27 => {
            let bytes: [u8; 8] = input.get(offset..offset + 8)?.try_into().ok()?;
            offset += 8;
            Some((u64::from_be_bytes(bytes), offset))
        }
        _ => None,
    }
}

unsafe fn input_bytes<'a>(input_ptr: *const u8, input_len: i64) -> Option<&'a [u8]> {
    if input_len < 0 {
        return None;
    }
    if input_len == 0 {
        return Some(&[]);
    }
    if input_ptr.is_null() {
        return None;
    }
    Some(unsafe { std::slice::from_raw_parts(input_ptr, input_len as usize) })
}

#[allow(dead_code)]
unsafe fn copy_output(output: &[u8], destination_ptr: *mut u8, destination_capacity: i64) -> i64 {
    if destination_capacity < 0 {
        return -1;
    }
    let output_len = output.len() as i64;
    if destination_capacity < output_len {
        return output_len;
    }
    if !output.is_empty() && destination_ptr.is_null() {
        return -1;
    }
    if !output.is_empty() {
        unsafe { ptr::copy_nonoverlapping(output.as_ptr(), destination_ptr, output.len()) };
    }
    output_len
}
