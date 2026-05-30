use csv::{ReaderBuilder, StringRecord, WriterBuilder};
use serde_json::{Map, Value};
use std::ptr;

#[unsafe(no_mangle)]
pub extern "C" fn csv_reader_records_score() -> i32 {
    if validate_csv(b"name,count\nrustlyn,3\ncsv,4\n") {
        1
    } else {
        -1
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn csv_writer_roundtrip_score() -> i32 {
    let mut output = Vec::new();
    {
        let mut writer = WriterBuilder::new().from_writer(&mut output);
        if writer
            .write_record(["name", "note"])
            .and_then(|_| writer.write_record(["rustlyn", "comma, quote \" and newline\nok"]))
            .is_err()
        {
            return -1;
        }
        if writer.flush().is_err() {
            return -2;
        }
    }

    let mut score = 0;
    if validate_csv(&output) {
        score += 1;
    }
    if output.windows(b"\"\"".len()).any(|window| window == b"\"\"") {
        score += 2;
    }
    if output.windows(b"\n".len()).any(|window| window == b"\n") {
        score += 4;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn csv_error_score() -> i32 {
    let mut score = 0;
    if validate_csv(b"name\n\"unterminated") {
        score += 1;
    }
    if validate_csv(b"name,count\nrustlyn,3\n") {
        score += 2;
    }
    if validate_csv(b"name\n\nvalue\n") {
        score += 4;
    }
    score
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_validate_utf8(input_ptr: *const u8, input_len: i64) -> i32 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    i32::from(validate_csv(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_echo_utf8_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    if validate_csv(input) { input_len } else { -2 }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_echo_utf8_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    if !validate_csv(input) {
        return -2;
    }
    unsafe { copy_input(input_ptr, input_len, destination_ptr, destination_capacity) }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_json_to_csv_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(json_to_csv(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_json_to_csv_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(json_to_csv(input), destination_ptr, destination_capacity) }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_to_json_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    output_len(csv_to_json(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn csv_to_json_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_bytes(input_ptr, input_len) }) else {
        return -1;
    };
    unsafe { copy_output(csv_to_json(input), destination_ptr, destination_capacity) }
}

fn json_to_csv(input: &[u8]) -> Result<Vec<u8>, ()> {
    let request: Value = serde_json::from_slice(input).map_err(|_| ())?;
    let delimiter = request_char(&request, "delimiter")?;
    let include_type_information = request_bool(&request, "includeTypeInformation");
    let no_header = request_bool(&request, "noHeader");
    let quote_fields = request
        .get("quoteFields")
        .and_then(Value::as_array)
        .map(|values| {
            values
                .iter()
                .filter_map(Value::as_str)
                .map(str::to_owned)
                .collect::<Vec<_>>()
        })
        .unwrap_or_default();
    let use_quotes = request
        .get("useQuotes")
        .and_then(Value::as_str)
        .unwrap_or("Always");
    let type_name = request
        .get("typeName")
        .and_then(Value::as_str)
        .unwrap_or("System.Management.Automation.PSCustomObject");
    let headers = request
        .get("headers")
        .and_then(Value::as_array)
        .ok_or(())?
        .iter()
        .map(|value| value.as_str().map(str::to_owned).ok_or(()))
        .collect::<Result<Vec<_>, _>>()?;
    let rows = request.get("rows").and_then(Value::as_array).ok_or(())?;
    let mut lines = Vec::new();

    if include_type_information {
        let mut line = String::from("#TYPE ");
        line.push_str(type_name);
        lines.push(line);
    }
    if !no_header {
        lines.push(format_record(&headers, delimiter, use_quotes, &quote_fields));
    }
    for row in rows {
        let values = row
            .as_array()
            .ok_or(())?
            .iter()
            .map(|value| value.as_str().map(str::to_owned).ok_or(()))
            .collect::<Result<Vec<_>, _>>()?;
        lines.push(format_record(&values, delimiter, use_quotes, &quote_fields_for_values(&headers, &quote_fields)));
    }

    serde_json::to_vec(&lines).map_err(|_| ())
}

fn csv_to_json(input: &[u8]) -> Result<Vec<u8>, ()> {
    let request: Value = serde_json::from_slice(input).map_err(|_| ())?;
    let csv = request.get("csv").and_then(Value::as_str).ok_or(())?;
    let delimiter = request_char(&request, "delimiter")?;
    let mut records = parse_csv_records(csv, delimiter);
    records.retain(|record| !(record.len() == 1 && record[0].is_empty()));

    let headers = match request.get("header") {
        Some(Value::Array(values)) => values
            .iter()
            .map(|value| value.as_str().map(str::to_owned).ok_or(()))
            .collect::<Result<Vec<_>, _>>()?,
        _ => {
            if records.is_empty() {
                return serde_json::to_vec(&Vec::<Value>::new()).map_err(|_| ());
            }
            records.remove(0)
        }
    };

    let mut rows = Vec::new();
    for record in records {
        let mut row = Map::new();
        for (index, header) in headers.iter().enumerate() {
            row.insert(
                header.clone(),
                Value::String(record.get(index).cloned().unwrap_or_default()),
            );
        }
        rows.push(Value::Object(row));
    }

    serde_json::to_vec(&Value::Array(rows)).map_err(|_| ())
}

fn validate_csv(input: &[u8]) -> bool {
    let mut reader = ReaderBuilder::new()
        .has_headers(false)
        .flexible(true)
        .from_reader(input);

    let mut record = StringRecord::new();
    loop {
        match reader.read_record(&mut record) {
            Ok(true) => {}
            Ok(false) => return true,
            Err(_) => return false,
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

fn request_char(request: &Value, name: &str) -> Result<char, ()> {
    request
        .get(name)
        .and_then(Value::as_str)
        .and_then(|value| value.chars().next())
        .ok_or(())
}

fn request_bool(request: &Value, name: &str) -> bool {
    request.get(name).and_then(Value::as_bool).unwrap_or(false)
}

fn quote_fields_for_values(headers: &[String], quote_fields: &[String]) -> Vec<String> {
    headers
        .iter()
        .enumerate()
        .filter_map(|(index, header)| {
            quote_fields
                .iter()
                .any(|field| field == header)
                .then(|| index.to_string())
        })
        .collect()
}

fn format_record(values: &[String], delimiter: char, use_quotes: &str, quote_fields: &[String]) -> String {
    let mut output = String::new();
    let mut index = 0usize;
    while index < values.len() {
        if index > 0 {
            output.push(delimiter);
        }
        let mut force_quote = false;
        let mut quote_index = 0usize;
        while quote_index < quote_fields.len() {
            if quote_fields[quote_index] == index.to_string() || quote_fields[quote_index] == values[index] {
                force_quote = true;
                break;
            }
            quote_index += 1;
        }
        push_field(&mut output, &values[index], delimiter, use_quotes, force_quote);
        index += 1;
    }
    output
}

fn push_field(output: &mut String, value: &str, delimiter: char, use_quotes: &str, force_quote: bool) {
    let should_quote = match use_quotes {
        "Never" => false,
        "AsNeeded" => force_quote || needs_quotes(value, delimiter),
        _ => true,
    };
    if !should_quote {
        output.push_str(value);
        return;
    }

    output.push('"');
    for character in value.chars() {
        if character == '"' {
            output.push('"');
            output.push('"');
        } else {
            output.push(character);
        }
    }
    output.push('"');
}

fn needs_quotes(value: &str, delimiter: char) -> bool {
    for character in value.chars() {
        if character == delimiter || character == '"' || character == '\r' || character == '\n' {
            return true;
        }
    }
    false
}

fn parse_csv_records(input: &str, delimiter: char) -> Vec<Vec<String>> {
    let mut records = Vec::new();
    let mut record = Vec::new();
    let mut field = String::new();
    let mut chars = input.chars().peekable();
    let mut at_field_start = true;
    let mut in_quotes = false;

    while let Some(ch) = chars.next() {
        if in_quotes {
            if ch == '"' {
                if chars.peek() == Some(&'"') {
                    chars.next();
                    field.push('"');
                } else {
                    in_quotes = false;
                }
            } else {
                field.push(ch);
            }
            continue;
        }

        if at_field_start && ch == '"' {
            in_quotes = true;
            at_field_start = false;
            continue;
        }

        if ch == delimiter {
            record.push(std::mem::take(&mut field));
            at_field_start = true;
            continue;
        }

        if ch == '\n' || ch == '\r' {
            if ch == '\r' && chars.peek() == Some(&'\n') {
                chars.next();
            }
            record.push(std::mem::take(&mut field));
            records.push(std::mem::take(&mut record));
            at_field_start = true;
            continue;
        }

        field.push(ch);
        at_field_start = false;
    }

    if in_quotes || !field.is_empty() || !record.is_empty() || input.is_empty() {
        record.push(field);
        records.push(record);
    }

    records
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
