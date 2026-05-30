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
pub unsafe extern "C" fn cbor_json_to_cbor_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(json_to_cbor(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_json_to_cbor_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(json_to_cbor(input), destination_ptr, destination_capacity) }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_object_stream_to_cbor_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(object_stream_to_cbor(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_object_stream_to_cbor_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(object_stream_to_cbor(input), destination_ptr, destination_capacity) }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_to_json_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(cbor_to_json(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn cbor_to_json_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(cbor_to_json(input), destination_ptr, destination_capacity) }
}

fn json_to_cbor(input: &[u8]) -> Result<Vec<u8>, ()> {
    let value: serde_json::Value = serde_json::from_slice(input).map_err(|_| ())?;
    let mut output = Vec::new();
    ciborium::ser::into_writer(&value, &mut output).map_err(|_| ())?;
    Ok(output)
}

fn object_stream_to_cbor(input: &[u8]) -> Result<Vec<u8>, ()> {
    let mut reader = ObjectStreamReader::new(input);
    let mut output = Vec::new();
    reader.write_cbor_value(&mut output)?;
    if reader.offset != input.len() {
        return Err(());
    }
    Ok(output)
}

struct ObjectStreamReader<'a> {
    input: &'a [u8],
    offset: usize,
}

impl<'a> ObjectStreamReader<'a> {
    fn new(input: &'a [u8]) -> Self {
        Self { input, offset: 0 }
    }

    fn write_cbor_value(&mut self, output: &mut Vec<u8>) -> Result<(), ()> {
        let tag = self.read_byte()?;
        match tag {
            b'N' => {
                self.expect(b';')?;
                output.push(0xf6);
            }
            b'T' => {
                self.expect(b';')?;
                output.push(0xf5);
            }
            b'F' => {
                self.expect(b';')?;
                output.push(0xf4);
            }
            b'I' => {
                let value = self.read_i64()?;
                self.expect(b';')?;
                if value >= 0 {
                    write_cbor_argument(output, 0, value as u64);
                } else {
                    write_cbor_argument(output, 1, (-1 - value) as u64);
                }
            }
            b'D' => {
                let value = self.read_f64()?;
                self.expect(b';')?;
                output.push(0xfb);
                output.extend_from_slice(&value.to_be_bytes());
            }
            b'S' => {
                let value = self.read_string_bytes()?;
                write_cbor_argument(output, 3, value.len() as u64);
                output.extend_from_slice(&value);
            }
            b'A' => {
                let count = self.read_usize()?;
                self.expect(b':')?;
                write_cbor_argument(output, 4, count as u64);
                let mut index = 0usize;
                while index < count {
                    self.write_cbor_value(output)?;
                    index += 1;
                }
            }
            b'O' => {
                let count = self.read_usize()?;
                self.expect(b':')?;
                write_cbor_argument(output, 5, count as u64);
                let mut index = 0usize;
                while index < count {
                    let key = self.read_string_bytes()?;
                    write_cbor_argument(output, 3, key.len() as u64);
                    output.extend_from_slice(&key);
                    self.write_cbor_value(output)?;
                    index += 1;
                }
            }
            _ => return Err(()),
        }
        Ok(())
    }

    fn read_string_bytes(&mut self) -> Result<Vec<u8>, ()> {
        let length = self.read_usize()?;
        self.expect(b':')?;
        let end = self.offset.checked_add(length).ok_or(())?;
        if end > self.input.len() || std::str::from_utf8(&self.input[self.offset..end]).is_err() {
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

fn write_cbor_argument(output: &mut Vec<u8>, major: u8, value: u64) {
    let prefix = major << 5;
    if value <= 23 {
        output.push(prefix | value as u8);
    } else if value <= u8::MAX as u64 {
        output.push(prefix | 24);
        output.push(value as u8);
    } else if value <= u16::MAX as u64 {
        output.push(prefix | 25);
        output.extend_from_slice(&(value as u16).to_be_bytes());
    } else if value <= u32::MAX as u64 {
        output.push(prefix | 26);
        output.extend_from_slice(&(value as u32).to_be_bytes());
    } else {
        output.push(prefix | 27);
        output.extend_from_slice(&value.to_be_bytes());
    }
}

fn cbor_to_json(input: &[u8]) -> Result<Vec<u8>, ()> {
    let mut offset = 0usize;
    let mut output = String::new();
    read_cbor_json(input, &mut offset, &mut output)?;
    if offset != input.len() {
        return Err(());
    }
    Ok(output.into_bytes())
}

fn read_cbor_json(input: &[u8], offset: &mut usize, output: &mut String) -> Result<(), ()> {
    let initial = *input.get(*offset).ok_or(())?;
    *offset += 1;
    let major = initial >> 5;
    let additional = initial & 0x1f;
    if additional == 31 {
        return read_indefinite_cbor_json(major, input, offset, output);
    }
    let (argument, next_offset) = read_argument(input, *offset, additional).ok_or(())?;
    *offset = next_offset;

    match major {
        0 => push_u64(output, argument),
        1 => {
            output.push('-');
            push_u64(output, argument.checked_add(1).ok_or(())?);
        }
        2 => {
            output.push('[');
            let length = usize::try_from(argument).map_err(|_| ())?;
            let end = (*offset).checked_add(length).ok_or(())?;
            if end > input.len() {
                return Err(());
            }
            let mut index = 0usize;
            while index < length {
                if index > 0 {
                    output.push(',');
                }
                push_u64(output, input[*offset + index] as u64);
                index += 1;
            }
            *offset = end;
            output.push(']');
        }
        3 => {
            let length = usize::try_from(argument).map_err(|_| ())?;
            let end = (*offset).checked_add(length).ok_or(())?;
            if end > input.len() {
                return Err(());
            }
            let value = std::str::from_utf8(&input[*offset..end]).map_err(|_| ())?;
            push_json_string(output, value);
            *offset = end;
        }
        4 => {
            output.push('[');
            let mut index = 0u64;
            while index < argument {
                if index > 0 {
                    output.push(',');
                }
                read_cbor_json(input, offset, output)?;
                index += 1;
            }
            output.push(']');
        }
        5 => {
            output.push('{');
            let mut index = 0u64;
            while index < argument {
                if index > 0 {
                    output.push(',');
                }
                read_cbor_json(input, offset, output)?;
                output.push(':');
                read_cbor_json(input, offset, output)?;
                index += 1;
            }
            output.push('}');
        }
        7 => match additional {
            20 => output.push_str("false"),
            21 => output.push_str("true"),
            22 => output.push_str("null"),
            26 => {
                let bytes: [u8; 4] = input.get(*offset..*offset + 4).ok_or(())?.try_into().map_err(|_| ())?;
                *offset += 4;
                output.push_str(&f32::from_be_bytes(bytes).to_string());
            }
            27 => {
                let bytes: [u8; 8] = input.get(*offset..*offset + 8).ok_or(())?.try_into().map_err(|_| ())?;
                *offset += 8;
                output.push_str(&f64::from_be_bytes(bytes).to_string());
            }
            _ => return Err(()),
        },
        _ => return Err(()),
    }

    Ok(())
}

fn read_indefinite_cbor_json(major: u8, input: &[u8], offset: &mut usize, output: &mut String) -> Result<(), ()> {
    match major {
        4 => {
            output.push('[');
            let mut first = true;
            loop {
                if *input.get(*offset).ok_or(())? == 0xff {
                    *offset += 1;
                    break;
                }
                if !first {
                    output.push(',');
                }
                first = false;
                read_cbor_json(input, offset, output)?;
            }
            output.push(']');
            Ok(())
        }
        5 => {
            output.push('{');
            let mut first = true;
            loop {
                if *input.get(*offset).ok_or(())? == 0xff {
                    *offset += 1;
                    break;
                }
                if !first {
                    output.push(',');
                }
                first = false;
                read_cbor_json(input, offset, output)?;
                output.push(':');
                read_cbor_json(input, offset, output)?;
            }
            output.push('}');
            Ok(())
        }
        _ => Err(()),
    }
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

fn push_u64(output: &mut String, value: u64) {
    if value == 0 {
        output.push('0');
        return;
    }

    let mut remaining = value;
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
