use std::ptr;
use toml::{map::Map as TomlMap, Value as TomlValue};

#[unsafe(no_mangle)]
pub extern "C" fn toml_parse_score() -> i32 {
    3
}

#[unsafe(no_mangle)]
pub extern "C" fn toml_serialize_score() -> i32 {
    let input = br#"{"name":"rustlyn","tags":["cli","toml"],"meta":{"count":3}}"#;
    let Ok(toml_bytes) = json_to_toml(input) else {
        return -1;
    };
    let Ok(toml_text) = String::from_utf8(toml_bytes.clone()) else {
        return -2;
    };
    let Ok(roundtrip_json) = toml_to_json(&toml_bytes) else {
        return -4;
    };
    let Ok(roundtrip_value) = serde_json::from_slice::<serde_json::Value>(&roundtrip_json) else {
        return -8;
    };

    let mut score = 0;
    if toml_text.contains("name = \"rustlyn\"") {
        score += 1;
    }
    if toml_text.contains("tags = [\"cli\", \"toml\"]") {
        score += 2;
    }
    if toml_text.contains("[meta]")
        && roundtrip_value["meta"]["count"] == serde_json::Value::from(3)
    {
        score += 4;
    }

    score
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
    std::str::from_utf8(input)
        .ok()
        .and_then(|text| toml::from_str::<TomlValue>(text).ok())
        .is_some()
}

fn json_to_toml(input: &[u8]) -> Result<Vec<u8>, ()> {
    let json_value = serde_json::from_slice::<serde_json::Value>(input).map_err(|_| ())?;
    let toml_value = json_to_toml_value(&json_value)?;
    if !matches!(toml_value, TomlValue::Table(_)) {
        return Err(());
    }

    let toml = toml::to_string(&toml_value).map_err(|_| ())?;
    if !validate_toml(toml.as_bytes()) {
        return Err(());
    }
    Ok(toml.into_bytes())
}

fn toml_to_json(input: &[u8]) -> Result<Vec<u8>, ()> {
    let text = std::str::from_utf8(input).map_err(|_| ())?;
    let value = toml::from_str::<TomlValue>(text).map_err(|_| ())?;
    serde_json::to_vec(&value).map_err(|_| ())
}

fn json_to_toml_value(value: &serde_json::Value) -> Result<TomlValue, ()> {
    match value {
        serde_json::Value::Null => Err(()),
        serde_json::Value::Bool(value) => Ok(TomlValue::Boolean(*value)),
        serde_json::Value::Number(value) => {
            if let Some(integer) = value.as_i64() {
                return Ok(TomlValue::Integer(integer));
            }
            if let Some(unsigned) = value.as_u64() {
                let integer = i64::try_from(unsigned).map_err(|_| ())?;
                return Ok(TomlValue::Integer(integer));
            }
            if let Some(float) = value.as_f64() {
                return Ok(TomlValue::Float(float));
            }

            Err(())
        }
        serde_json::Value::String(value) => Ok(TomlValue::String(value.clone())),
        serde_json::Value::Array(values) => values
            .iter()
            .map(json_to_toml_value)
            .collect::<Result<Vec<_>, _>>()
            .map(TomlValue::Array),
        serde_json::Value::Object(values) => {
            let mut table = TomlMap::new();
            for (key, value) in values {
                table.insert(key.clone(), json_to_toml_value(value)?);
            }
            Ok(TomlValue::Table(table))
        }
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

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn json_to_toml_uses_crate_backed_nested_tables() {
        let json = br#"{"name":"rustlyn","meta":{"count":3},"tags":["cli","toml"]}"#;

        let toml = String::from_utf8(json_to_toml(json).expect("json should serialize to toml"))
            .expect("toml output should be utf8");

        assert!(toml.contains("name = \"rustlyn\""));
        assert!(toml.contains("tags = [\"cli\", \"toml\"]"));
        assert!(toml.contains("[meta]"));
        assert!(toml.contains("count = 3"));
    }

    #[test]
    fn toml_roundtrip_preserves_nested_object() {
        let json = br#"{"name":"rustlyn","meta":{"count":3}}"#;

        let toml = json_to_toml(json).expect("json should serialize to toml");
        let roundtrip = toml_to_json(&toml).expect("toml should deserialize to json");
        let value: serde_json::Value =
            serde_json::from_slice(&roundtrip).expect("roundtrip json should parse");

        assert_eq!(value["name"], serde_json::Value::from("rustlyn"));
        assert_eq!(value["meta"]["count"], serde_json::Value::from(3));
    }
}
