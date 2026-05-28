use csv::{ReaderBuilder, StringRecord, WriterBuilder};
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
