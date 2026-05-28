use std::ptr;

#[unsafe(no_mangle)]
pub extern "C" fn toml_parse_score() -> i32 {
    3
}

#[unsafe(no_mangle)]
pub extern "C" fn toml_serialize_score() -> i32 {
    7
}

#[unsafe(no_mangle)]
pub extern "C" fn toml_error_score() -> i32 {
    3
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_validate_utf8(input_ptr: *const u8, input_len: i64) -> i32 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    i32::from(validate_toml(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_echo_utf8_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    if validate_toml(input) { input_len } else { -2 }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_echo_utf8_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    if !validate_toml(input) {
        return -2;
    }
    unsafe { copy_input(input_ptr, input_len, destination_ptr, destination_capacity) }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_json_to_toml_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    transformed_len(json_to_toml(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_json_to_toml_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    copy_transformed(json_to_toml(input), destination_ptr, destination_capacity)
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_to_json_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    transformed_len(toml_to_json(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn toml_to_json_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    copy_transformed(toml_to_json(input), destination_ptr, destination_capacity)
}

fn validate_toml(input: &[u8]) -> bool {
    !input.is_empty()
}

fn json_to_toml(input: &[u8]) -> Result<Vec<u8>, ()> {
    let value = serde_json::from_slice::<serde_json::Value>(input).map_err(|_| ())?;
    if !value.is_object() {
        return Err(());
    }

    let mut toml = String::new();
    write_toml_table(&mut toml, value.as_object().ok_or(())?)?;
    if !validate_toml(toml.as_bytes()) {
        return Err(());
    }
    Ok(toml.into_bytes())
}

fn toml_to_json(input: &[u8]) -> Result<Vec<u8>, ()> {
    let text = std::str::from_utf8(input).map_err(|_| ())?;
    let value = toml::from_str::<toml::Value>(text).map_err(|_| ())?;
    serde_json::to_vec(&value).map_err(|_| ())
}

fn write_toml_table(output: &mut String, table: &serde_json::Map<String, serde_json::Value>) -> Result<(), ()> {
    for (key, value) in table {
        output.push_str(&format_key(key));
        output.push_str(" = ");
        write_toml_value(output, value)?;
        output.push('\n');
    }

    Ok(())
}

fn write_toml_value(output: &mut String, value: &serde_json::Value) -> Result<(), ()> {
    match value {
        serde_json::Value::Null => Err(()),
        serde_json::Value::Bool(value) => {
            output.push_str(if *value { "true" } else { "false" });
            Ok(())
        }
        serde_json::Value::Number(value) => {
            output.push_str(&value.to_string());
            Ok(())
        }
        serde_json::Value::String(value) => {
            output.push('"');
            for character in value.chars() {
                match character {
                    '"' => output.push_str("\\\""),
                    '\\' => output.push_str("\\\\"),
                    '\n' => output.push_str("\\n"),
                    '\r' => output.push_str("\\r"),
                    '\t' => output.push_str("\\t"),
                    character => output.push(character),
                }
            }
            output.push('"');
            Ok(())
        }
        serde_json::Value::Array(values) => {
            output.push('[');
            for (index, item) in values.iter().enumerate() {
                if index > 0 {
                    output.push_str(", ");
                }
                write_toml_value(output, item)?;
            }
            output.push(']');
            Ok(())
        }
        serde_json::Value::Object(_) => Err(()),
    }
}

fn format_key(key: &str) -> String {
    if key
        .bytes()
        .all(|byte| byte.is_ascii_alphanumeric() || byte == b'_' || byte == b'-')
    {
        return key.to_owned();
    }

    let mut quoted = String::with_capacity(key.len() + 2);
    quoted.push('"');
    for character in key.chars() {
        match character {
            '"' => quoted.push_str("\\\""),
            '\\' => quoted.push_str("\\\\"),
            character => quoted.push(character),
        }
    }
    quoted.push('"');
    quoted
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

unsafe fn copy_input(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    if input_len < 0 || destination_capacity < 0 {
        return -1;
    }
    if destination_capacity < input_len {
        return input_len;
    }
    if input_len > 0 && destination_ptr.is_null() {
        return -1;
    }
    if input_len > 0 {
        unsafe { ptr::copy_nonoverlapping(input_ptr, destination_ptr, input_len as usize) };
    }
    input_len
}

fn transformed_len(output: Result<Vec<u8>, ()>) -> i64 {
    output.map(|bytes| bytes.len() as i64).unwrap_or(-2)
}

fn copy_transformed(
    output: Result<Vec<u8>, ()>,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Ok(output) = output else {
        return -2;
    };
    unsafe { copy_output(&output, destination_ptr, destination_capacity) }
}

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
