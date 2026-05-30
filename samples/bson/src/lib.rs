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
pub unsafe extern "C" fn bson_object_stream_to_bson_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(object_stream_to_bson(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_object_stream_to_bson_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(object_stream_to_bson(input), destination_ptr, destination_capacity) }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_to_json_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(bson_to_json(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn bson_to_json_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(bson_to_json(input), destination_ptr, destination_capacity) }
}

fn object_stream_to_bson(input: &[u8]) -> Result<Vec<u8>, ()> {
    let mut reader = ObjectStreamReader::new(input);
    let mut output = Vec::new();
    reader.write_document(&mut output)?;
    if reader.offset != input.len() {
        return Err(());
    }
    Ok(output)
}

fn bson_to_json(input: &[u8]) -> Result<Vec<u8>, ()> {
    let mut offset = 0;
    let mut output = String::new();
    read_document_json(input, &mut offset, &mut output)?;
    if offset != input.len() {
        return Err(());
    }
    Ok(output.into_bytes())
}

struct ObjectStreamReader<'a> {
    input: &'a [u8],
    offset: usize,
}

impl<'a> ObjectStreamReader<'a> {
    fn new(input: &'a [u8]) -> Self {
        Self { input, offset: 0 }
    }

    fn write_document(&mut self, output: &mut Vec<u8>) -> Result<(), ()> {
        if self.read_byte()? != b'O' {
            return Err(());
        }
        let count = self.read_usize()?;
        self.expect(b':')?;
        let start = output.len();
        output.extend_from_slice(&[0, 0, 0, 0]);
        for _ in 0..count {
            let key = self.read_string_bytes()?;
            self.write_value(output, &key)?;
        }
        output.push(0);
        let length = i32::try_from(output.len() - start).map_err(|_| ())?;
        output[start..start + 4].copy_from_slice(&length.to_le_bytes());
        Ok(())
    }

    fn write_value(&mut self, output: &mut Vec<u8>, name: &[u8]) -> Result<(), ()> {
        let tag = self.read_byte()?;
        match tag {
            b'N' => {
                self.expect(b';')?;
                output.push(0x0A);
                write_cstring_bytes(output, name)?;
            }
            b'T' | b'F' => {
                self.expect(b';')?;
                output.push(0x08);
                write_cstring_bytes(output, name)?;
                output.push(u8::from(tag == b'T'));
            }
            b'I' => {
                let value = self.read_i64()?;
                self.expect(b';')?;
                if let Ok(value32) = i32::try_from(value) {
                    output.push(0x10);
                    write_cstring_bytes(output, name)?;
                    output.extend_from_slice(&value32.to_le_bytes());
                } else {
                    output.push(0x12);
                    write_cstring_bytes(output, name)?;
                    output.extend_from_slice(&value.to_le_bytes());
                }
            }
            b'D' => {
                let value = self.read_f64()?;
                self.expect(b';')?;
                output.push(0x01);
                write_cstring_bytes(output, name)?;
                output.extend_from_slice(&value.to_le_bytes());
            }
            b'S' => {
                let value = self.read_string_bytes()?;
                output.push(0x02);
                write_cstring_bytes(output, name)?;
                let length = i32::try_from(value.len() + 1).map_err(|_| ())?;
                output.extend_from_slice(&length.to_le_bytes());
                output.extend_from_slice(&value);
                output.push(0);
            }
            b'A' => {
                let count = self.read_usize()?;
                self.expect(b':')?;
                output.push(0x04);
                write_cstring_bytes(output, name)?;
                let start = output.len();
                output.extend_from_slice(&[0, 0, 0, 0]);
                for index in 0..count {
                    self.write_value(output, index.to_string().as_bytes())?;
                }
                output.push(0);
                let length = i32::try_from(output.len() - start).map_err(|_| ())?;
                output[start..start + 4].copy_from_slice(&length.to_le_bytes());
            }
            b'O' => {
                self.offset -= 1;
                output.push(0x03);
                write_cstring_bytes(output, name)?;
                self.write_document(output)?;
            }
            _ => return Err(()),
        }
        Ok(())
    }

    fn read_string_bytes(&mut self) -> Result<Vec<u8>, ()> {
        let length = self.read_usize()?;
        self.expect(b':')?;
        let end = self.offset.checked_add(length).ok_or(())?;
        if end > self.input.len() {
            return Err(());
        }
        let value = self.input[self.offset..end].to_vec();
        self.offset = end;
        Ok(value)
    }

    fn read_i64(&mut self) -> Result<i64, ()> {
        let mut sign = 1i64;
        if self.offset < self.input.len() && self.input[self.offset] == b'-' {
            sign = -1;
            self.offset += 1;
        }

        let mut value = 0i64;
        let start = self.offset;
        while self.offset < self.input.len() {
            let byte = self.input[self.offset];
            if !byte.is_ascii_digit() {
                break;
            }
            value = value
                .checked_mul(10)
                .and_then(|current| current.checked_add((byte - b'0') as i64))
                .ok_or(())?;
            self.offset += 1;
        }

        if self.offset == start {
            return Err(());
        }

        value.checked_mul(sign).ok_or(())
    }

    fn read_f64(&mut self) -> Result<f64, ()> {
        let start = self.offset;
        while self.offset < self.input.len() && self.input[self.offset] != b';' {
            self.offset += 1;
        }
        let text = std::str::from_utf8(&self.input[start..self.offset]).map_err(|_| ())?;
        text.parse::<f64>().map_err(|_| ())
    }

    fn read_usize(&mut self) -> Result<usize, ()> {
        let mut value = 0usize;
        let start = self.offset;
        while self.offset < self.input.len() && self.input[self.offset].is_ascii_digit() {
            value = value
                .checked_mul(10)
                .and_then(|current| current.checked_add((self.input[self.offset] - b'0') as usize))
                .ok_or(())?;
            self.offset += 1;
        }
        if self.offset == start {
            return Err(());
        }
        Ok(value)
    }

    fn expect(&mut self, expected: u8) -> Result<(), ()> {
        if self.read_byte()? == expected {
            Ok(())
        } else {
            Err(())
        }
    }

    fn read_byte(&mut self) -> Result<u8, ()> {
        let value = *self.input.get(self.offset).ok_or(())?;
        self.offset += 1;
        Ok(value)
    }
}

fn write_cstring_bytes(output: &mut Vec<u8>, value: &[u8]) -> Result<(), ()> {
    let mut index = 0;
    while index < value.len() {
        let byte = value[index];
        if byte == 0 || byte >= 0x80 {
            return Err(());
        }
        output.push(byte);
        index += 1;
    }
    output.push(0);
    Ok(())
}

fn read_document_json(input: &[u8], offset: &mut usize, output: &mut String) -> Result<(), ()> {
    let start = *offset;
    let length = read_i32(input, *offset).ok_or(())? as usize;
    let end = start.checked_add(length).ok_or(())?;
    if length < 5 || end > input.len() || input[end - 1] != 0 {
        return Err(());
    }
    *offset += 4;

    output.push('{');
    let mut first = true;
    while *offset < end - 1 {
        let element_type = *input.get(*offset).ok_or(())?;
        *offset += 1;
        let name = read_cstring_value(input, offset, end)?;
        if !first {
            output.push(',');
        }
        first = false;
        push_json_string(output, &name);
        output.push(':');
        read_bson_json(element_type, input, offset, end, output)?;
    }
    *offset = end;
    output.push('}');
    Ok(())
}

fn read_bson_json(element_type: u8, input: &[u8], offset: &mut usize, limit: usize, output: &mut String) -> Result<(), ()> {
    match element_type {
        0x01 => {
            let bytes: [u8; 8] = input.get(*offset..*offset + 8).ok_or(())?.try_into().map_err(|_| ())?;
            *offset += 8;
            output.push_str(&f64::from_le_bytes(bytes).to_string());
        }
        0x02 => push_json_string(output, &read_string_value(input, offset, limit)?),
        0x03 => read_document_json(input, offset, output)?,
        0x04 => read_array_json(input, offset, output)?,
        0x08 => {
            let value = *input.get(*offset).ok_or(())? != 0;
            *offset += 1;
            output.push_str(if value { "true" } else { "false" });
        }
        0x0A => output.push_str("null"),
        0x10 => {
            let value = read_i32(input, *offset).ok_or(())?;
            *offset += 4;
            push_i64(output, value as i64);
        }
        0x12 => {
            let bytes: [u8; 8] = input.get(*offset..*offset + 8).ok_or(())?.try_into().map_err(|_| ())?;
            *offset += 8;
            push_i64(output, i64::from_le_bytes(bytes));
        }
        _ => return Err(()),
    }
    Ok(())
}

fn read_array_json(input: &[u8], offset: &mut usize, output: &mut String) -> Result<(), ()> {
    let start = *offset;
    let length = read_i32(input, *offset).ok_or(())? as usize;
    let end = start.checked_add(length).ok_or(())?;
    if length < 5 || end > input.len() || input[end - 1] != 0 {
        return Err(());
    }
    *offset += 4;
    output.push('[');
    let mut first = true;
    while *offset < end - 1 {
        let element_type = *input.get(*offset).ok_or(())?;
        *offset += 1;
        read_cstring(input, offset, end).ok_or(())?;
        if !first {
            output.push(',');
        }
        first = false;
        read_bson_json(element_type, input, offset, end, output)?;
    }
    *offset = end;
    output.push(']');
    Ok(())
}

fn read_string_value(input: &[u8], offset: &mut usize, limit: usize) -> Result<String, ()> {
    let length = read_i32(input, *offset).ok_or(())? as usize;
    *offset += 4;
    if length == 0 || (*offset).checked_add(length).ok_or(())? > limit || input[*offset + length - 1] != 0 {
        return Err(());
    }
    let value = std::str::from_utf8(&input[*offset..*offset + length - 1]).map_err(|_| ())?;
    *offset += length;
    Ok(value.to_owned())
}

fn read_cstring_value(input: &[u8], offset: &mut usize, limit: usize) -> Result<String, ()> {
    let start = *offset;
    read_cstring(input, offset, limit).ok_or(())?;
    std::str::from_utf8(&input[start..*offset - 1])
        .map(str::to_owned)
        .map_err(|_| ())
}

fn push_json_string(output: &mut String, value: &str) {
    output.push('"');
    for character in value.chars() {
        match character {
            '"' => output.push_str("\\\""),
            '\\' => output.push_str("\\\\"),
            '\n' => output.push_str("\\n"),
            '\r' => output.push_str("\\r"),
            '\t' => output.push_str("\\t"),
            '\u{08}' => output.push_str("\\b"),
            '\u{0C}' => output.push_str("\\f"),
            character if character < ' ' => {
                output.push_str("\\u00");
                let value = character as u8;
                push_hex(output, value >> 4);
                push_hex(output, value & 0x0F);
            }
            _ => output.push(character),
        }
    }
    output.push('"');
}

fn push_hex(output: &mut String, value: u8) {
    output.push(if value < 10 {
        (b'0' + value) as char
    } else {
        (b'a' + value - 10) as char
    });
}

fn push_i64(output: &mut String, value: i64) {
    if value == 0 {
        output.push('0');
        return;
    }

    let mut remaining = value;
    if remaining < 0 {
        output.push('-');
        remaining = remaining.saturating_abs();
    }

    let mut digits = [0u8; 20];
    let mut count = 0usize;
    while remaining > 0 {
        digits[count] = (remaining % 10) as u8;
        remaining /= 10;
        count += 1;
    }

    while count > 0 {
        count -= 1;
        output.push((b'0' + digits[count]) as char);
    }
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
            if length == 0 || (*offset).checked_add(length)? > limit || input[*offset + length - 1] != 0 {
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
    *offset = (*offset).checked_add(count)?;
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

fn output_len(output: Result<Vec<u8>, ()>) -> i64 {
    output.map(|bytes| bytes.len() as i64).unwrap_or(-2)
}

unsafe fn copy_output(output: Result<Vec<u8>, ()>, destination_ptr: *mut u8, destination_capacity: i64) -> i64 {
    let Ok(output) = output else {
        return -2;
    };
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
