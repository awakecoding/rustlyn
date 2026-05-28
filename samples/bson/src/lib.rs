use std::ptr;

const VALID_BSON: &[u8] = &[
    23, 0, 0, 0, 0x02, b'n', b'a', b'm', b'e', 0, 8, 0, 0, 0, b'r', b'u', b's', b't', b'l',
    b'y', b'n', 0, 0,
];

#[unsafe(no_mangle)]
pub extern "C" fn bson_document_score() -> i32 {
    let _crate_anchor = bson::Bson::String("rustlyn".to_owned());
    let mut score = 0;
    if !VALID_BSON.is_empty() {
        score += 1;
    }
    if validate_bson(VALID_BSON) {
        score += 2;
    }
    if !validate_bson(&[0, 1, 2]) {
        score += 4;
    }
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn bson_error_score() -> i32 {
    let mut score = 0;
    if !validate_bson(&[0, 1, 2]) {
        score += 1;
    }
    if validate_bson(VALID_BSON) {
        score += 2;
    }
    score
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_validate_bytes(input_ptr: *const u8, input_len: i64) -> i32 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    i32::from(validate_bson(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_json_to_bson_len(_input_ptr: *const u8, _input_len: i64) -> i64 {
    -2
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_json_to_bson_copy(
    _input_ptr: *const u8,
    _input_len: i64,
    _destination_ptr: *mut u8,
    _destination_capacity: i64,
) -> i64 {
    -2
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_to_json_len(_input_ptr: *const u8, _input_len: i64) -> i64 {
    -2
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_to_json_copy(
    _input_ptr: *const u8,
    _input_len: i64,
    _destination_ptr: *mut u8,
    _destination_capacity: i64,
) -> i64 {
    -2
}

fn validate_bson(input: &[u8]) -> bool {
    validate_document(input).is_some()
}

fn validate_document(input: &[u8]) -> Option<usize> {
    if input.len() < 5 {
        return None;
    }
    let length = read_i32(input, 0)? as usize;
    if length > input.len() || length < 5 || input[length - 1] != 0 {
        return None;
    }

    let mut offset = 4;
    while offset < length - 1 {
        let element_type = input[offset];
        offset += 1;
        read_cstring(input, &mut offset, length)?;
        validate_value(element_type, input, &mut offset, length)?;
    }

    (offset == length - 1).then_some(length)
}

fn validate_value(element_type: u8, input: &[u8], offset: &mut usize, limit: usize) -> Option<()> {
    match element_type {
        0x01 => advance(offset, 8, limit),
        0x02 => {
            let length = read_i32(input, *offset)? as usize;
            advance(offset, 4, limit)?;
            if length == 0 || offset.checked_add(length)? > limit || input[*offset + length - 1] != 0 {
                return None;
            }
            std::str::from_utf8(&input[*offset..*offset + length - 1]).ok()?;
            advance(offset, length, limit)
        }
        0x03 | 0x04 => {
            let consumed = validate_document(input.get(*offset..limit)?)?;
            advance(offset, consumed, limit)
        }
        0x08 => advance(offset, 1, limit),
        0x0A => Some(()),
        0x10 => advance(offset, 4, limit),
        0x12 => advance(offset, 8, limit),
        _ => None,
    }
}

fn read_cstring(input: &[u8], offset: &mut usize, limit: usize) -> Option<()> {
    while *offset < limit && input[*offset] != 0 {
        *offset += 1;
    }
    if *offset >= limit {
        return None;
    }
    *offset += 1;
    Some(())
}

fn advance(offset: &mut usize, count: usize, limit: usize) -> Option<()> {
    *offset = offset.checked_add(count)?;
    (*offset <= limit).then_some(())
}

fn read_i32(input: &[u8], offset: usize) -> Option<i32> {
    Some(i32::from_le_bytes(input.get(offset..offset + 4)?.try_into().ok()?))
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
