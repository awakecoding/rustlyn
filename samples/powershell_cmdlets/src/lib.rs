#![allow(static_mut_refs)]

mod simd_json_engine {
    include!("../../simd_json/src/lib.rs");
}

mod marked_yaml_engine {
    include!("../../marked_yaml/src/lib.rs");
}

mod csv_engine {
    include!("../../csv/src/lib.rs");
}

mod bson_engine {
    include!("../../bson/src/lib.rs");
}

mod cbor_engine {
    include!("../../cbor/src/lib.rs");
}

use serde::Deserialize;
use serde_json::{Map, Value};
use std::iter::Peekable;
use std::str::Chars;

#[cfg(not(test))]
unsafe extern "C" {
    fn rustlyn_bindgen_powershell_string_from_utf8(
        value_ptr: *const u8,
        value_len: i64,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_string_utf8_len(
        string_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_string_copy_utf8(
        string_handle: i32,
        destination_ptr: *mut u8,
        destination_capacity: i64,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_input_snapshot_json(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_input_string(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_input_string_base64(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_parameter_string(
        cmdlet_context_handle: i32,
        name_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_has_parameter(
        cmdlet_context_handle: i32,
        name_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_parameter_bool(
        cmdlet_context_handle: i32,
        name_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_parameter_i32(
        cmdlet_context_handle: i32,
        name_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_parameter_char(
        cmdlet_context_handle: i32,
        name_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_parameter_snapshot_json(
        cmdlet_context_handle: i32,
        name_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_current_culture_list_separator(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_object_string(
        cmdlet_context_handle: i32,
        value_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_object_bytes(
        cmdlet_context_handle: i32,
        bytes_ptr: *const u8,
        byte_len: i64,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_json_string(
        cmdlet_context_handle: i32,
        json_handle: i32,
        as_hashtable: i32,
        no_enumerate: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_json_bytes(
        cmdlet_context_handle: i32,
        bytes_ptr: *const u8,
        byte_len: i64,
        as_hashtable: i32,
        no_enumerate: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_object_stream_string(
        cmdlet_context_handle: i32,
        stream_handle: i32,
        as_hashtable: i32,
        no_enumerate: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_add_xml_input(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_add_xml_text_input(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_converted_xml_inputs(
        cmdlet_context_handle: i32,
        depth: i32,
        no_type_information: i32,
        output_mode: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_write_xml_text_inputs_as_document(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_is_cancellation_requested(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_cmdlet_get_lifecycle_state_handle(
        cmdlet_context_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
    fn rustlyn_bindgen_powershell_object_release(
        object_handle: i32,
        exception_out: *mut i32,
    ) -> i32;
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_string_from_utf8(
    _value_ptr: *const u8,
    _value_len: i64,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    1
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_string_utf8_len(
    _string_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_string_copy_utf8(
    _string_handle: i32,
    _destination_ptr: *mut u8,
    _destination_capacity: i64,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_input_snapshot_json(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_input_string(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_input_string_base64(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_parameter_string(
    _cmdlet_context_handle: i32,
    _name_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_has_parameter(
    _cmdlet_context_handle: i32,
    _name_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_parameter_bool(
    _cmdlet_context_handle: i32,
    _name_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_parameter_i32(
    _cmdlet_context_handle: i32,
    _name_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_parameter_char(
    _cmdlet_context_handle: i32,
    _name_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_parameter_snapshot_json(
    _cmdlet_context_handle: i32,
    _name_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_current_culture_list_separator(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_object_string(
    _cmdlet_context_handle: i32,
    _value_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_object_bytes(
    _cmdlet_context_handle: i32,
    _bytes_ptr: *const u8,
    _byte_len: i64,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_json_string(
    _cmdlet_context_handle: i32,
    _json_handle: i32,
    _as_hashtable: i32,
    _no_enumerate: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_json_bytes(
    _cmdlet_context_handle: i32,
    _bytes_ptr: *const u8,
    _byte_len: i64,
    _as_hashtable: i32,
    _no_enumerate: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_object_stream_string(
    _cmdlet_context_handle: i32,
    _stream_handle: i32,
    _as_hashtable: i32,
    _no_enumerate: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_add_xml_input(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_add_xml_text_input(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_converted_xml_inputs(
    _cmdlet_context_handle: i32,
    _depth: i32,
    _no_type_information: i32,
    _output_mode: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_write_xml_text_inputs_as_document(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_is_cancellation_requested(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    0
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_cmdlet_get_lifecycle_state_handle(
    _cmdlet_context_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    1
}

#[cfg(test)]
unsafe extern "C" fn rustlyn_bindgen_powershell_object_release(
    _object_handle: i32,
    exception_out: *mut i32,
) -> i32 {
    unsafe {
        *exception_out = 0;
    }
    1
}

type RuntimeResult<T> = Result<T, i32>;

const STATUS_EXCEPTION: i32 = -1;
const STATUS_CANCELLED: i32 = -2;
const STATUS_INVALID_STATE: i32 = -3;
const STATUS_PARSE: i32 = -4;
const STATUS_HOST_WRITE: i32 = -5;
const STATUS_TRANSFORM: i32 = -6;
const XML_OUTPUT_STRING: i32 = 0;
const XML_OUTPUT_DOCUMENT: i32 = 1;

fn utf8_string_from_bytes(bytes: Vec<u8>) -> RuntimeResult<String> {
    String::from_utf8(bytes).map_err(|_| STATUS_PARSE)
}

#[derive(Clone, Debug, Deserialize)]
struct PowerShellPropertySnapshot {
    name: String,
    value: PowerShellObjectSnapshot,
}

#[derive(Clone, Debug, Deserialize)]
struct PowerShellObjectSnapshot {
    kind: String,
    #[serde(rename = "typeName")]
    type_name: Option<String>,
    #[serde(rename = "scalarValue")]
    scalar_value: Option<String>,
    #[serde(default)]
    items: Vec<PowerShellObjectSnapshot>,
    #[serde(default)]
    properties: Vec<PowerShellPropertySnapshot>,
}

#[derive(Default)]
struct FormatState {
    snapshots: Vec<PowerShellObjectSnapshot>,
    text_items: Vec<String>,
    bytes: Vec<u8>,
}

struct StateEntry {
    handle: i32,
    state: FormatState,
}

static mut STATES: Vec<StateEntry> = Vec::new();

struct ManagedString {
    handle: i32,
}

impl ManagedString {
    fn from_utf8(value: &str) -> RuntimeResult<Self> {
        let mut exception_handle = 0;
        let handle = unsafe {
            rustlyn_bindgen_powershell_string_from_utf8(
                value.as_ptr(),
                value.len() as i64,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        if handle == 0 {
            return Err(STATUS_EXCEPTION);
        }

        Ok(Self { handle })
    }

    unsafe fn from_handle(handle: i32) -> Self {
        Self { handle }
    }

    fn to_utf8_string(&self) -> RuntimeResult<String> {
        let mut exception_handle = 0;
        let length = unsafe {
            rustlyn_bindgen_powershell_string_utf8_len(self.handle, &mut exception_handle)
        };
        exception_to_result(exception_handle)?;
        if length < 0 {
            return Err(STATUS_EXCEPTION);
        }

        let mut buffer = vec![0u8; length as usize];
        let copied = unsafe {
            rustlyn_bindgen_powershell_string_copy_utf8(
                self.handle,
                buffer.as_mut_ptr(),
                buffer.len() as i64,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        if copied < 0 {
            return Err(STATUS_EXCEPTION);
        }

        buffer.truncate(copied as usize);
        utf8_string_from_bytes(buffer)
    }

    fn release(self) -> RuntimeResult<()> {
        release_handle(self.handle)
    }
}

#[derive(Clone, Copy)]
struct CmdletContext {
    handle: i32,
}

impl CmdletContext {
    fn from_handle(handle: i32) -> Self {
        Self { handle }
    }

    fn lifecycle_state_handle(&self) -> RuntimeResult<i32> {
        let mut exception_handle = 0;
        let handle = unsafe {
            rustlyn_bindgen_powershell_cmdlet_get_lifecycle_state_handle(
                self.handle,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        if handle == 0 {
            return Err(STATUS_INVALID_STATE);
        }

        Ok(handle)
    }

    fn throw_if_cancelled(&self) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        let cancelled = unsafe {
            rustlyn_bindgen_powershell_cmdlet_is_cancellation_requested(
                self.handle,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        if cancelled != 0 {
            Err(STATUS_CANCELLED)
        } else {
            Ok(())
        }
    }

    fn input_snapshot(&self) -> RuntimeResult<PowerShellObjectSnapshot> {
        let mut exception_handle = 0;
        let snapshot_handle = unsafe {
            rustlyn_bindgen_powershell_cmdlet_get_input_snapshot_json(
                self.handle,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        if snapshot_handle == 0 {
            return Err(STATUS_EXCEPTION);
        }

        let snapshot = unsafe { ManagedString::from_handle(snapshot_handle) };
        let json = snapshot.to_utf8_string();
        let release = snapshot.release();
        release?;
        parse_snapshot_json(&json?)
    }

    fn input_string(&self) -> RuntimeResult<String> {
        let bytes = self.input_bytes()?;
        utf8_string_from_bytes(bytes)
    }

    fn input_bytes(&self) -> RuntimeResult<Vec<u8>> {
        let mut exception_handle = 0;
        let string_handle = unsafe {
            rustlyn_bindgen_powershell_cmdlet_get_input_string_base64(
                self.handle,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        if string_handle == 0 {
            return Err(STATUS_EXCEPTION);
        }

        let value = unsafe { ManagedString::from_handle(string_handle) };
        let text = value.to_utf8_string();
        let release = value.release();
        release?;
        let base64 = text?;
        let bytes = decode_base64(&base64);
        if !base64.is_empty() && bytes.is_empty() {
            return Err(STATUS_PARSE);
        }

        Ok(bytes)
    }

    fn input_text(&self) -> RuntimeResult<String> {
        let snapshot = self.input_snapshot()?;
        if snapshot.kind == "array" {
            return Ok(join_text_items(&snapshot_to_string_array(&snapshot)));
        }

        if let Some(text) = snapshot_scalar_text(&snapshot) {
            return Ok(text);
        }

        self.input_string()
    }

    fn has_parameter(&self, name: &str) -> RuntimeResult<bool> {
        with_managed_string(name, |name| {
            let mut exception_handle = 0;
            let result = unsafe {
                rustlyn_bindgen_powershell_cmdlet_has_parameter(
                    self.handle,
                    name.handle,
                    &mut exception_handle,
                )
            };
            exception_to_result(exception_handle)?;
            Ok(result != 0)
        })
    }

    fn parameter_bool(&self, name: &str) -> RuntimeResult<bool> {
        with_managed_string(name, |name| {
            let mut exception_handle = 0;
            let result = unsafe {
                rustlyn_bindgen_powershell_cmdlet_get_parameter_bool(
                    self.handle,
                    name.handle,
                    &mut exception_handle,
                )
            };
            exception_to_result(exception_handle)?;
            Ok(result != 0)
        })
    }

    fn parameter_i32_or(&self, name: &str, default_value: i32) -> RuntimeResult<i32> {
        if !self.has_parameter(name)? {
            return Ok(default_value);
        }

        with_managed_string(name, |name| {
            let mut exception_handle = 0;
            let result = unsafe {
                rustlyn_bindgen_powershell_cmdlet_get_parameter_i32(
                    self.handle,
                    name.handle,
                    &mut exception_handle,
                )
            };
            exception_to_result(exception_handle)?;
            Ok(result)
        })
    }

    fn parameter_char_or(&self, name: &str, default_value: char) -> RuntimeResult<char> {
        if !self.has_parameter(name)? {
            return Ok(default_value);
        }

        with_managed_string(name, |name| {
            let mut exception_handle = 0;
            let result = unsafe {
                rustlyn_bindgen_powershell_cmdlet_get_parameter_char(
                    self.handle,
                    name.handle,
                    &mut exception_handle,
                )
            };
            exception_to_result(exception_handle)?;
            char::from_u32(result as u32).ok_or(STATUS_PARSE)
        })
    }

    fn parameter_string_or(&self, name: &str, default_value: &str) -> RuntimeResult<String> {
        if !self.has_parameter(name)? {
            return Ok(default_value.to_owned());
        }

        with_managed_string(name, |name| {
            let mut exception_handle = 0;
            let result = unsafe {
                rustlyn_bindgen_powershell_cmdlet_get_parameter_string(
                    self.handle,
                    name.handle,
                    &mut exception_handle,
                )
            };
            exception_to_result(exception_handle)?;
            if result == 0 {
                return Ok(String::new());
            }

            let value = unsafe { ManagedString::from_handle(result) };
            let text = value.to_utf8_string();
            let release = value.release();
            release?;
            text
        })
    }

    fn parameter_string_array(&self, name: &str) -> RuntimeResult<Vec<String>> {
        if !self.has_parameter(name)? {
            return Ok(Vec::new());
        }

        with_managed_string(name, |name| {
            let mut exception_handle = 0;
            let result = unsafe {
                rustlyn_bindgen_powershell_cmdlet_get_parameter_snapshot_json(
                    self.handle,
                    name.handle,
                    &mut exception_handle,
                )
            };
            exception_to_result(exception_handle)?;
            if result == 0 {
                return Ok(Vec::new());
            }

            let value = unsafe { ManagedString::from_handle(result) };
            let text = value.to_utf8_string();
            let release = value.release();
            release?;
            let snapshot = parse_snapshot_json(&text?)?;
            Ok(snapshot_to_string_array(&snapshot))
        })
    }

    fn current_culture_list_separator(&self) -> RuntimeResult<String> {
        let mut exception_handle = 0;
        let result = unsafe {
            rustlyn_bindgen_powershell_cmdlet_get_current_culture_list_separator(
                self.handle,
                &mut exception_handle,
            )
        };
        exception_to_result(exception_handle)?;
        let value = unsafe { ManagedString::from_handle(result) };
        let text = value.to_utf8_string();
        let release = value.release();
        release?;
        text
    }

    fn write_string(&self, value: &str) -> RuntimeResult<()> {
        with_managed_string(value, |value| {
            let mut exception_handle = 0;
            unsafe {
                rustlyn_bindgen_powershell_cmdlet_write_object_string(
                    self.handle,
                    value.handle,
                    &mut exception_handle,
                );
            }
            exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
        })
    }

    fn write_json(&self, json: &str, as_hashtable: bool, no_enumerate: bool) -> RuntimeResult<()> {
        with_managed_string(json, |json| {
            let mut exception_handle = 0;
            unsafe {
                rustlyn_bindgen_powershell_cmdlet_write_json_string(
                    self.handle,
                    json.handle,
                    if as_hashtable { 1 } else { 0 },
                    if no_enumerate { 1 } else { 0 },
                    &mut exception_handle,
                );
            }
            exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
        })
    }

    fn write_json_bytes(
        &self,
        bytes: &[u8],
        as_hashtable: bool,
        no_enumerate: bool,
    ) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        let bytes_ptr = if bytes.is_empty() {
            std::ptr::null()
        } else {
            bytes.as_ptr()
        };
        unsafe {
            rustlyn_bindgen_powershell_cmdlet_write_json_bytes(
                self.handle,
                bytes_ptr,
                bytes.len() as i64,
                if as_hashtable { 1 } else { 0 },
                if no_enumerate { 1 } else { 0 },
                &mut exception_handle,
            );
        }
        exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
    }

    fn write_object_stream(
        &self,
        stream: &str,
        as_hashtable: bool,
        no_enumerate: bool,
    ) -> RuntimeResult<()> {
        with_managed_string(stream, |stream| {
            let mut exception_handle = 0;
            unsafe {
                rustlyn_bindgen_powershell_cmdlet_write_object_stream_string(
                    self.handle,
                    stream.handle,
                    if as_hashtable { 1 } else { 0 },
                    if no_enumerate { 1 } else { 0 },
                    &mut exception_handle,
                );
            }
            exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
        })
    }

    fn add_xml_input(&self) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        unsafe {
            rustlyn_bindgen_powershell_cmdlet_add_xml_input(self.handle, &mut exception_handle);
        }
        exception_to_result(exception_handle)
    }

    fn add_xml_text_input(&self) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        unsafe {
            rustlyn_bindgen_powershell_cmdlet_add_xml_text_input(
                self.handle,
                &mut exception_handle,
            );
        }
        exception_to_result(exception_handle)
    }

    fn write_converted_xml_inputs(
        &self,
        depth: i32,
        no_type_information: bool,
        output_mode: i32,
    ) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        unsafe {
            rustlyn_bindgen_powershell_cmdlet_write_converted_xml_inputs(
                self.handle,
                depth,
                if no_type_information { 1 } else { 0 },
                output_mode,
                &mut exception_handle,
            );
        }
        exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
    }

    fn write_xml_text_inputs_as_document(&self) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        unsafe {
            rustlyn_bindgen_powershell_cmdlet_write_xml_text_inputs_as_document(
                self.handle,
                &mut exception_handle,
            );
        }
        exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
    }

    fn write_bytes(&self, bytes: &[u8]) -> RuntimeResult<()> {
        let mut exception_handle = 0;
        unsafe {
            rustlyn_bindgen_powershell_cmdlet_write_object_bytes(
                self.handle,
                bytes.as_ptr(),
                bytes.len() as i64,
                &mut exception_handle,
            );
        }
        exception_to_result(exception_handle).map_err(|_| STATUS_HOST_WRITE)
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn powershell_format_state_count() -> i32 {
    unsafe { STATES.len() as i32 }
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_json_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_snapshot(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_json_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_snapshots(cmdlet_context_handle, |context, snapshots| {
        let depth = context.parameter_i32_or("Depth", 2)?;
        let enums_as_strings = context.parameter_bool("EnumsAsStrings")?;
        let json = snapshots_to_json_text(
            &snapshots,
            depth,
            enums_as_strings,
            !context.parameter_bool("Compress")?,
        );
        context.write_string(&json)
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_json_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_text_bytes(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_json_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_bytes(cmdlet_context_handle, |context, bytes| {
        let json = transform_bytes(
            &bytes,
            simd_json_engine::simd_json_echo_utf8_len,
            simd_json_engine::simd_json_echo_utf8_copy,
        )?;
        context.write_json_bytes(
            &json,
            context.parameter_bool("AsHashtable")?,
            context.parameter_bool("NoEnumerate")?,
        )
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_yaml_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_snapshot(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_yaml_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_snapshots(cmdlet_context_handle, |context, snapshots| {
        let stream = snapshots_to_object_stream(&snapshots, 64, false);
        let yaml = transform_utf8(
            &stream,
            marked_yaml_engine::marked_yaml_object_stream_to_yaml_len,
            marked_yaml_engine::marked_yaml_object_stream_to_yaml_copy,
        )?;
        context.write_string(&yaml)
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_yaml_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_text(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_yaml_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_text(cmdlet_context_handle, |context, text| {
        let json = transform_utf8(
            &text,
            marked_yaml_engine::marked_yaml_to_json_len,
            marked_yaml_engine::marked_yaml_to_json_copy,
        )?;
        context.write_json(
            &json,
            context.parameter_bool("AsHashtable")?,
            context.parameter_bool("NoEnumerate")?,
        )
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_toml_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_snapshot(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_toml_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_snapshots(cmdlet_context_handle, |context, snapshots| {
        let depth = context.parameter_i32_or("Depth", 8)?;
        let toml = snapshots_to_toml_text(&snapshots, depth)?;
        context.write_string(&toml)
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_toml_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_text(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_toml_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_text(cmdlet_context_handle, |context, text| {
        let json = toml_text_to_json_text(&text)?;
        context.write_json(&json, false, false)
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_bson_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_snapshot(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_bson_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_snapshots(cmdlet_context_handle, |context, snapshots| {
        let depth = context.parameter_i32_or("Depth", 8)?;
        let stream = snapshots_to_object_stream(&snapshots, depth, false);
        let bytes = transform_utf8_to_bytes(
            &stream,
            bson_engine::bson_object_stream_to_bson_len,
            bson_engine::bson_object_stream_to_bson_copy,
        )?;
        context.write_bytes(&bytes)
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_bson_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_bytes(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_bson_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_bytes(cmdlet_context_handle, |context, bytes| {
        let json = transform_bytes_to_utf8(
            &bytes,
            bson_engine::bson_to_json_len,
            bson_engine::bson_to_json_copy,
        )?;
        context.write_json(
            &json,
            context.parameter_bool("AsHashtable")?,
            context.parameter_bool("NoEnumerate")?,
        )
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_cbor_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_snapshot(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_cbor_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_snapshots(cmdlet_context_handle, |context, snapshots| {
        let depth = context.parameter_i32_or("Depth", 8)?;
        let stream = snapshots_to_object_stream(&snapshots, depth, false);
        let bytes = transform_utf8_to_bytes(
            &stream,
            cbor_engine::cbor_object_stream_to_cbor_len,
            cbor_engine::cbor_object_stream_to_cbor_copy,
        )?;
        context.write_bytes(&bytes)
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_cbor_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_bytes(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_cbor_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_bytes(cmdlet_context_handle, |context, bytes| {
        let json = transform_bytes_to_utf8(
            &bytes,
            cbor_engine::cbor_to_json_len,
            cbor_engine::cbor_to_json_copy,
        )?;
        context.write_json(
            &json,
            context.parameter_bool("AsHashtable")?,
            context.parameter_bool("NoEnumerate")?,
        )
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_xml_process_record(cmdlet_context_handle: i32) -> i32 {
    let context = CmdletContext::from_handle(cmdlet_context_handle);
    let result = (|| {
        context.throw_if_cancelled()?;
        context.lifecycle_state_handle()?;
        context.add_xml_input()
    })();
    match result {
        Ok(()) => 0,
        Err(status) => status,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_xml_end_processing(cmdlet_context_handle: i32) -> i32 {
    let context = CmdletContext::from_handle(cmdlet_context_handle);
    let result = (|| {
        context.throw_if_cancelled()?;
        let depth = context.parameter_i32_or("Depth", 2)?;
        let no_type_information = context.parameter_bool("NoTypeInformation")?;
        let output_mode = xml_output_mode(&context)?;
        context.write_converted_xml_inputs(depth, no_type_information, output_mode)
    })();
    match result {
        Ok(()) => 0,
        Err(status) => status,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_xml_process_record(cmdlet_context_handle: i32) -> i32 {
    let context = CmdletContext::from_handle(cmdlet_context_handle);
    let result = (|| {
        context.throw_if_cancelled()?;
        context.lifecycle_state_handle()?;
        context.add_xml_text_input()
    })();
    match result {
        Ok(()) => 0,
        Err(status) => status,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_xml_end_processing(cmdlet_context_handle: i32) -> i32 {
    let context = CmdletContext::from_handle(cmdlet_context_handle);
    let result = (|| {
        context.throw_if_cancelled()?;
        context.write_xml_text_inputs_as_document()
    })();
    match result {
        Ok(()) => 0,
        Err(status) => status,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_csv_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_snapshot(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_to_rust_csv_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_snapshots(cmdlet_context_handle, |context, snapshots| {
        let lines = snapshots_to_csv_lines(&context, &snapshots)?;
        for line in lines {
            context.write_string(&line)?;
        }
        Ok(())
    })
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_csv_process_record(cmdlet_context_handle: i32) -> i32 {
    collect_text(cmdlet_context_handle)
}

#[unsafe(no_mangle)]
pub extern "C" fn convert_from_rust_csv_end_processing(cmdlet_context_handle: i32) -> i32 {
    finish_with_text(cmdlet_context_handle, |context, text| {
        let request = create_from_csv_request(context, &text)?;
        let json = transform_utf8(
            &request,
            csv_engine::csv_to_json_len,
            csv_engine::csv_to_json_copy,
        )?;
        let stream = json_to_object_stream(&json)?;
        context.write_object_stream(&stream, false, false)
    })
}

macro_rules! cleanup_export {
    ($name:ident) => {
        #[unsafe(no_mangle)]
        pub extern "C" fn $name(cmdlet_context_handle: i32) -> i32 {
            match CmdletContext::from_handle(cmdlet_context_handle).lifecycle_state_handle() {
                Ok(handle) => {
                    clear_state(handle);
                    0
                }
                Err(status) => status,
            }
        }
    };
}

cleanup_export!(convert_to_rust_json_cleanup);
cleanup_export!(convert_from_rust_json_cleanup);
cleanup_export!(convert_to_rust_yaml_cleanup);
cleanup_export!(convert_from_rust_yaml_cleanup);
cleanup_export!(convert_to_rust_toml_cleanup);
cleanup_export!(convert_from_rust_toml_cleanup);
cleanup_export!(convert_to_rust_bson_cleanup);
cleanup_export!(convert_from_rust_bson_cleanup);
cleanup_export!(convert_to_rust_cbor_cleanup);
cleanup_export!(convert_from_rust_cbor_cleanup);
cleanup_export!(convert_to_rust_xml_cleanup);
cleanup_export!(convert_from_rust_xml_cleanup);
cleanup_export!(convert_to_rust_csv_cleanup);
cleanup_export!(convert_from_rust_csv_cleanup);

fn xml_output_mode(context: &CmdletContext) -> RuntimeResult<i32> {
    match context.parameter_string_or("As", "Document")?.as_str() {
        value if value.eq_ignore_ascii_case("String") => Ok(XML_OUTPUT_STRING),
        value if value.eq_ignore_ascii_case("Document") => Ok(XML_OUTPUT_DOCUMENT),
        value if value.eq_ignore_ascii_case("Stream") => Ok(XML_OUTPUT_STRING),
        _ => Err(STATUS_PARSE),
    }
}

fn collect_snapshot(cmdlet_context_handle: i32) -> i32 {
    run_collect(cmdlet_context_handle, |context, state| {
        state.snapshots.push(context.input_snapshot()?);
        Ok(())
    })
}

fn collect_text(cmdlet_context_handle: i32) -> i32 {
    run_collect(cmdlet_context_handle, |context, state| {
        state.text_items.push(context.input_text()?);
        Ok(())
    })
}

fn collect_text_bytes(cmdlet_context_handle: i32) -> i32 {
    run_collect(cmdlet_context_handle, |context, state| {
        let bytes = context.input_bytes()?;
        if !state.bytes.is_empty() {
            state.bytes.extend_from_slice(b"\r\n");
        }
        state.bytes.extend_from_slice(&bytes);
        Ok(())
    })
}

fn collect_bytes(cmdlet_context_handle: i32) -> i32 {
    run_collect(cmdlet_context_handle, |context, state| {
        let snapshot = context.input_snapshot()?;
        append_bytes(&snapshot, &mut state.bytes)
    })
}

fn run_collect(
    cmdlet_context_handle: i32,
    collect: impl FnOnce(CmdletContext, &mut FormatState) -> RuntimeResult<()>,
) -> i32 {
    let context = CmdletContext::from_handle(cmdlet_context_handle);
    let result = (|| {
        context.throw_if_cancelled()?;
        let state_handle = context.lifecycle_state_handle()?;
        with_state(state_handle, |state| collect(context, state))
    })();
    match result {
        Ok(()) => 0,
        Err(status) => status,
    }
}

fn finish_with_snapshots(
    cmdlet_context_handle: i32,
    finish: impl FnOnce(CmdletContext, Vec<PowerShellObjectSnapshot>) -> RuntimeResult<()>,
) -> i32 {
    finish_with_state(cmdlet_context_handle, |context, state| {
        finish(context, state.snapshots)
    })
}

fn finish_with_text(
    cmdlet_context_handle: i32,
    finish: impl FnOnce(CmdletContext, String) -> RuntimeResult<()>,
) -> i32 {
    finish_with_state(cmdlet_context_handle, |context, state| {
        finish(context, join_text_items(&state.text_items))
    })
}

fn finish_with_bytes(
    cmdlet_context_handle: i32,
    finish: impl FnOnce(CmdletContext, Vec<u8>) -> RuntimeResult<()>,
) -> i32 {
    finish_with_state(cmdlet_context_handle, |context, state| {
        finish(context, state.bytes)
    })
}

fn finish_with_state(
    cmdlet_context_handle: i32,
    finish: impl FnOnce(CmdletContext, FormatState) -> RuntimeResult<()>,
) -> i32 {
    let context = CmdletContext::from_handle(cmdlet_context_handle);
    let result = (|| {
        context.throw_if_cancelled()?;
        let state_handle = context.lifecycle_state_handle()?;
        let state = take_state(state_handle);
        finish(context, state)
    })();
    match result {
        Ok(()) => 0,
        Err(status) => status,
    }
}

fn with_state<T>(
    handle: i32,
    action: impl FnOnce(&mut FormatState) -> RuntimeResult<T>,
) -> RuntimeResult<T> {
    unsafe {
        if let Some(index) = STATES.iter().position(|entry| entry.handle == handle) {
            return action(&mut STATES[index].state);
        }

        STATES.push(StateEntry {
            handle,
            state: FormatState::default(),
        });
        let last = STATES.len() - 1;
        action(&mut STATES[last].state)
    }
}

fn take_state(handle: i32) -> FormatState {
    unsafe {
        if let Some(index) = STATES.iter().position(|entry| entry.handle == handle) {
            STATES.remove(index).state
        } else {
            FormatState::default()
        }
    }
}

fn clear_state(handle: i32) {
    unsafe {
        if let Some(index) = STATES.iter().position(|entry| entry.handle == handle) {
            STATES.remove(index);
        }
    }
}

fn snapshots_to_json_value(
    snapshots: &[PowerShellObjectSnapshot],
    max_depth: i32,
    enums_as_strings: bool,
) -> Value {
    match snapshots {
        [] => Value::Null,
        [single] => snapshot_to_json_value(single, max_depth, 0, enums_as_strings),
        _ => Value::Array(
            snapshots
                .iter()
                .map(|snapshot| snapshot_to_json_value(snapshot, max_depth, 0, enums_as_strings))
                .collect(),
        ),
    }
}

fn snapshots_to_json_text(
    snapshots: &[PowerShellObjectSnapshot],
    max_depth: i32,
    enums_as_strings: bool,
    pretty: bool,
) -> String {
    let mut output = String::with_capacity(snapshots.len().saturating_mul(128).max(4));
    match snapshots {
        [] => output.push_str("null"),
        [single] => write_snapshot_json(
            &mut output,
            single,
            max_depth,
            0,
            enums_as_strings,
            pretty,
            0,
        ),
        _ => write_snapshot_array(
            &mut output,
            snapshots,
            max_depth,
            0,
            enums_as_strings,
            pretty,
            0,
        ),
    }

    output
}

fn write_snapshot_json(
    output: &mut String,
    snapshot: &PowerShellObjectSnapshot,
    max_depth: i32,
    depth: i32,
    enums_as_strings: bool,
    pretty: bool,
    indent: usize,
) {
    if depth > max_depth.max(0) {
        write_json_string(output, &snapshot_to_string(snapshot));
        return;
    }

    match snapshot.kind.as_str() {
        "null" => output.push_str("null"),
        "datetime" => write_datetime_json(output, snapshot),
        "scalar" => write_scalar_json(output, snapshot),
        "enum" if enums_as_strings => {
            write_json_string(output, snapshot.scalar_value.as_deref().unwrap_or_default())
        }
        "enum" => write_scalar_json(output, snapshot),
        "bytes" => {
            let bytes = decode_base64(snapshot.scalar_value.as_deref().unwrap_or_default());
            write_json_array_start(output, pretty);
            for (index, byte) in bytes.iter().enumerate() {
                write_json_item_prefix(output, index, pretty, indent + 1);
                output.push_str(&byte.to_string());
            }
            write_json_array_end(output, !bytes.is_empty(), pretty, indent);
        }
        "array" => write_snapshot_array(
            output,
            &snapshot.items,
            max_depth,
            depth + 1,
            enums_as_strings,
            pretty,
            indent,
        ),
        "dictionary" | "psobject" => {
            write_json_object_start(output, pretty);
            for (index, property) in snapshot.properties.iter().enumerate() {
                write_json_item_prefix(output, index, pretty, indent + 1);
                write_json_string(output, &property.name);
                if pretty {
                    output.push_str(": ");
                } else {
                    output.push(':');
                }
                write_snapshot_json(
                    output,
                    &property.value,
                    max_depth,
                    depth + 1,
                    enums_as_strings,
                    pretty,
                    indent + 1,
                );
            }
            write_json_object_end(output, !snapshot.properties.is_empty(), pretty, indent);
        }
        "cycle" | "truncated" => write_json_string(output, &snapshot_to_string(snapshot)),
        _ => write_json_string(output, &snapshot_to_string(snapshot)),
    }
}

fn write_snapshot_array(
    output: &mut String,
    snapshots: &[PowerShellObjectSnapshot],
    max_depth: i32,
    depth: i32,
    enums_as_strings: bool,
    pretty: bool,
    indent: usize,
) {
    write_json_array_start(output, pretty);
    for (index, item) in snapshots.iter().enumerate() {
        write_json_item_prefix(output, index, pretty, indent + 1);
        write_snapshot_json(
            output,
            item,
            max_depth,
            depth,
            enums_as_strings,
            pretty,
            indent + 1,
        );
    }
    write_json_array_end(output, !snapshots.is_empty(), pretty, indent);
}

fn write_scalar_json(output: &mut String, snapshot: &PowerShellObjectSnapshot) {
    let value = snapshot.scalar_value.as_deref().unwrap_or_default();
    let type_name = snapshot.type_name.as_deref().unwrap_or_default();
    match type_name {
        "System.Boolean" | "Deserialized.System.Boolean" if is_true_text(value) => {
            output.push_str("true")
        }
        "System.Boolean" | "Deserialized.System.Boolean" if is_false_text(value) => {
            output.push_str("false")
        }
        "System.Byte" | "Deserialized.System.Byte"
        | "System.SByte" | "Deserialized.System.SByte"
        | "System.Int16" | "Deserialized.System.Int16"
        | "System.UInt16" | "Deserialized.System.UInt16"
        | "System.Int32" | "Deserialized.System.Int32"
        | "System.UInt32" | "Deserialized.System.UInt32"
        | "System.Int64" | "Deserialized.System.Int64"
            if is_json_integer_literal(value) =>
        {
            output.push_str(value)
        }
        "System.UInt64" | "Deserialized.System.UInt64" if is_unsigned_integer_literal(value) => {
            output.push_str(value)
        }
        "System.Single" | "Deserialized.System.Single"
        | "System.Double" | "Deserialized.System.Double"
            if !is_non_finite_float_text(value) =>
        {
            if trim_ascii(value) == "-0" {
                output.push_str("-0.0")
            } else {
                output.push_str(value)
            }
        }
        "System.Decimal" | "Deserialized.System.Decimal" if !is_non_finite_float_text(value) => {
            output.push_str(value)
        }
        _ => write_json_string(output, value),
    }
}

fn write_datetime_json(output: &mut String, snapshot: &PowerShellObjectSnapshot) {
    let value = snapshot.scalar_value.as_deref().unwrap_or_default();
    write_json_string(output, &normalize_roundtrip_datetime_text(value));
}

fn is_json_integer_literal(value: &str) -> bool {
    let text = trim_ascii(value);
    if text.is_empty() {
        return false;
    }

    let digits = if let Some(rest) = text.strip_prefix('-') {
        rest
    } else {
        text
    };

    !digits.is_empty() && digits.bytes().all(|byte| byte.is_ascii_digit())
}

fn is_true_text(value: &str) -> bool {
    matches!(value, "True" | "true")
}

fn is_false_text(value: &str) -> bool {
    matches!(value, "False" | "false")
}

fn is_non_finite_float_text(value: &str) -> bool {
    matches!(trim_ascii(value), "NaN" | "nan" | "Infinity" | "infinity" | "-Infinity" | "-infinity")
}

fn normalize_roundtrip_datetime_text(value: &str) -> String {
    let bytes = value.as_bytes();
    let Some(dot_index) = bytes.iter().position(|byte| *byte == b'.') else {
        return value.to_owned();
    };

    let mut timezone_index = bytes.len();
    let mut scan_index = dot_index + 1;
    while scan_index < bytes.len() {
        if matches!(bytes[scan_index], b'Z' | b'+' | b'-') {
            timezone_index = scan_index;
            break;
        }
        scan_index += 1;
    }

    let mut fractional_end = timezone_index;
    while fractional_end > dot_index + 1 && bytes[fractional_end - 1] == b'0' {
        fractional_end -= 1;
    }

    if fractional_end == timezone_index {
        return value.to_owned();
    }

    let mut normalized = String::with_capacity(value.len());
    normalized.push_str(&value[..dot_index]);
    if fractional_end > dot_index + 1 {
        normalized.push('.');
        normalized.push_str(&value[dot_index + 1..fractional_end]);
    }
    normalized.push_str(&value[timezone_index..]);
    normalized
}

fn write_json_array_start(output: &mut String, _pretty: bool) {
    output.push('[');
}

fn write_json_array_end(output: &mut String, has_items: bool, pretty: bool, indent: usize) {
    if has_items && pretty {
        write_json_newline(output, indent);
    }
    output.push(']');
}

fn write_json_object_start(output: &mut String, _pretty: bool) {
    output.push('{');
}

fn write_json_object_end(output: &mut String, has_items: bool, pretty: bool, indent: usize) {
    if has_items && pretty {
        write_json_newline(output, indent);
    }
    output.push('}');
}

fn write_json_item_prefix(output: &mut String, index: usize, pretty: bool, indent: usize) {
    if index > 0 {
        output.push(',');
    }
    if pretty {
        write_json_newline(output, indent);
    }
}

fn write_json_newline(output: &mut String, indent: usize) {
    output.push('\n');
    for _ in 0..(indent * 2) {
        output.push(' ');
    }
}

fn write_json_string(output: &mut String, value: &str) {
    const HEX: &[u8; 16] = b"0123456789abcdef";

    output.push('"');
    for ch in value.chars() {
        match ch {
            '"' => output.push_str("\\\""),
            '\\' => output.push_str("\\\\"),
            '\n' => output.push_str("\\n"),
            '\r' => output.push_str("\\r"),
            '\t' => output.push_str("\\t"),
            '\u{08}' => output.push_str("\\b"),
            '\u{0c}' => output.push_str("\\f"),
            '\u{2028}' => output.push_str("\\u2028"),
            '\u{2029}' => output.push_str("\\u2029"),
            ch if (ch as u32) < 0x20 => {
                let value = ch as u32;
                output.push_str("\\u00");
                output.push(HEX[((value >> 4) & 0xf) as usize] as char);
                output.push(HEX[(value & 0xf) as usize] as char);
            }
            ch => output.push(ch),
        }
    }
    output.push('"');
}

fn snapshot_to_json_value(
    snapshot: &PowerShellObjectSnapshot,
    max_depth: i32,
    depth: i32,
    enums_as_strings: bool,
) -> Value {
    if depth > max_depth.max(0) {
        return Value::String(snapshot_to_string(snapshot));
    }

    match snapshot.kind.as_str() {
        "null" => Value::Null,
        "datetime" => Value::String(normalize_roundtrip_datetime_text(
            snapshot.scalar_value.as_deref().unwrap_or_default(),
        )),
        "scalar" => scalar_snapshot_to_json(snapshot),
        "enum" if enums_as_strings => {
            Value::String(snapshot.scalar_value.clone().unwrap_or_default())
        }
        "enum" => scalar_snapshot_to_json(snapshot),
        "bytes" => Value::Array(
            decode_base64(snapshot.scalar_value.as_deref().unwrap_or_default())
                .into_iter()
                .map(|byte| Value::Number(byte.into()))
                .collect(),
        ),
        "array" => Value::Array(
            snapshot
                .items
                .iter()
                .map(|item| snapshot_to_json_value(item, max_depth, depth + 1, enums_as_strings))
                .collect(),
        ),
        "dictionary" | "psobject" => {
            let mut object = Map::new();
            for property in &snapshot.properties {
                object.insert(
                    property.name.clone(),
                    snapshot_to_json_value(&property.value, max_depth, depth + 1, enums_as_strings),
                );
            }
            Value::Object(object)
        }
        "cycle" | "truncated" => Value::String(snapshot_to_string(snapshot)),
        _ => Value::String(snapshot_to_string(snapshot)),
    }
}

fn scalar_snapshot_to_json(snapshot: &PowerShellObjectSnapshot) -> Value {
    let value = snapshot.scalar_value.as_deref().unwrap_or_default();
    match snapshot.type_name.as_deref().unwrap_or_default() {
        "System.Boolean" | "Deserialized.System.Boolean" => {
            Value::Bool(value.eq_ignore_ascii_case("true"))
        }
        "System.Byte" | "Deserialized.System.Byte"
        | "System.SByte" | "Deserialized.System.SByte"
        | "System.Int16" | "Deserialized.System.Int16"
        | "System.UInt16" | "Deserialized.System.UInt16"
        | "System.Int32" | "Deserialized.System.Int32"
        | "System.UInt32" | "Deserialized.System.UInt32"
        | "System.Int64" | "Deserialized.System.Int64" => value
            .parse::<i64>()
            .map(|number| Value::Number(number.into()))
            .unwrap_or_else(|_| Value::String(value.to_owned())),
        "System.UInt64" | "Deserialized.System.UInt64" => value
            .parse::<u64>()
            .map(|number| Value::Number(number.into()))
            .unwrap_or_else(|_| Value::String(value.to_owned())),
        "System.Single" | "Deserialized.System.Single"
        | "System.Double" | "Deserialized.System.Double"
        | "System.Decimal" | "Deserialized.System.Decimal" => value
            .parse::<f64>()
            .ok()
            .and_then(serde_json::Number::from_f64)
            .map(Value::Number)
            .unwrap_or_else(|| Value::String(value.to_owned())),
        _ => Value::String(value.to_owned()),
    }
}

fn snapshots_to_object_stream(
    snapshots: &[PowerShellObjectSnapshot],
    max_depth: i32,
    enums_as_strings: bool,
) -> String {
    let mut output = String::new();
    match snapshots {
        [] => output.push_str("N;"),
        [single] => write_object_stream_value(&mut output, single, max_depth, 0, enums_as_strings),
        _ => {
            output.push('A');
            output.push_str(&snapshots.len().to_string());
            output.push(':');
            for snapshot in snapshots {
                write_object_stream_value(&mut output, snapshot, max_depth, 1, enums_as_strings);
            }
        }
    }
    output
}

fn json_to_object_stream(input: &str) -> RuntimeResult<String> {
    let value: Value = serde_json::from_str(input).map_err(|_| STATUS_PARSE)?;
    let mut output = String::new();
    write_json_value_as_object_stream(&mut output, &value)?;
    Ok(output)
}

fn write_json_value_as_object_stream(output: &mut String, value: &Value) -> RuntimeResult<()> {
    match value {
        Value::Null => output.push_str("N;"),
        Value::Bool(true) => output.push_str("T;"),
        Value::Bool(false) => output.push_str("F;"),
        Value::Number(number) => write_json_number_as_object_stream(output, number)?,
        Value::String(text) => write_object_stream_string(output, text),
        Value::Array(items) => {
            output.push('A');
            output.push_str(&items.len().to_string());
            output.push(':');
            for item in items {
                write_json_value_as_object_stream(output, item)?;
            }
        }
        Value::Object(object) => {
            output.push('O');
            output.push_str(&object.len().to_string());
            output.push(':');
            for (name, item) in object {
                write_raw_object_stream_string(output, name);
                write_json_value_as_object_stream(output, item)?;
            }
        }
    }

    Ok(())
}

fn write_json_number_as_object_stream(
    output: &mut String,
    number: &serde_json::Number,
) -> RuntimeResult<()> {
    let value = number.to_string();

    if is_integer_literal(&value) || is_unsigned_integer_literal(&value) {
        output.push('I');
        output.push_str(&value);
        output.push(';');
        return Ok(());
    }

    if !is_float_literal(&value) {
        return Err(STATUS_PARSE);
    }

    output.push('D');
    output.push_str(&value);
    output.push(';');
    Ok(())
}

fn write_object_stream_value(
    output: &mut String,
    snapshot: &PowerShellObjectSnapshot,
    max_depth: i32,
    depth: i32,
    enums_as_strings: bool,
) {
    if depth > max_depth.max(0) {
        write_object_stream_string(output, &snapshot_to_string(snapshot));
        return;
    }

    match snapshot.kind.as_str() {
        "null" => output.push_str("N;"),
        "datetime" => write_object_stream_string(
            output,
            &normalize_roundtrip_datetime_text(
                snapshot.scalar_value.as_deref().unwrap_or_default(),
            ),
        ),
        "scalar" => write_scalar_object_stream(output, snapshot),
        "enum" if enums_as_strings => {
            write_object_stream_string(output, snapshot.scalar_value.as_deref().unwrap_or_default())
        }
        "enum" => write_scalar_object_stream(output, snapshot),
        "bytes" => {
            let bytes = decode_base64(snapshot.scalar_value.as_deref().unwrap_or_default());
            output.push('A');
            output.push_str(&bytes.len().to_string());
            output.push(':');
            for byte in bytes {
                output.push('I');
                output.push_str(&byte.to_string());
                output.push(';');
            }
        }
        "array" => {
            output.push('A');
            output.push_str(&snapshot.items.len().to_string());
            output.push(':');
            for item in &snapshot.items {
                write_object_stream_value(output, item, max_depth, depth + 1, enums_as_strings);
            }
        }
        "dictionary" | "psobject" => {
            output.push('O');
            output.push_str(&snapshot.properties.len().to_string());
            output.push(':');
            for property in &snapshot.properties {
                write_raw_object_stream_string(output, &property.name);
                write_object_stream_value(
                    output,
                    &property.value,
                    max_depth,
                    depth + 1,
                    enums_as_strings,
                );
            }
        }
        _ => write_object_stream_string(output, &snapshot_to_string(snapshot)),
    }
}

fn write_scalar_object_stream(output: &mut String, snapshot: &PowerShellObjectSnapshot) {
    let value = snapshot.scalar_value.as_deref().unwrap_or_default();
    match snapshot.type_name.as_deref().unwrap_or_default() {
        "System.Boolean" | "Deserialized.System.Boolean" if is_true_text(value) => {
            output.push_str("T;")
        }
        "System.Boolean" | "Deserialized.System.Boolean" if is_false_text(value) => {
            output.push_str("F;")
        }
        "System.Byte" | "Deserialized.System.Byte"
        | "System.SByte" | "Deserialized.System.SByte"
        | "System.Int16" | "Deserialized.System.Int16"
        | "System.UInt16" | "Deserialized.System.UInt16"
        | "System.Int32" | "Deserialized.System.Int32"
        | "System.UInt32" | "Deserialized.System.UInt32"
        | "System.Int64" | "Deserialized.System.Int64"
        | "System.UInt64" | "Deserialized.System.UInt64" => {
            output.push('I');
            output.push_str(value);
            output.push(';');
        }
        "System.Single" | "Deserialized.System.Single"
        | "System.Double" | "Deserialized.System.Double"
        | "System.Decimal" | "Deserialized.System.Decimal" => {
            output.push('D');
            output.push_str(value);
            output.push(';');
        }
        _ => write_object_stream_string(output, value),
    }
}

fn write_object_stream_string(output: &mut String, value: &str) {
    output.push('S');
    write_raw_object_stream_string(output, value);
}

fn write_raw_object_stream_string(output: &mut String, value: &str) {
    output.push_str(&value.len().to_string());
    output.push(':');
    output.push_str(value);
}

fn snapshots_to_toml_json_value(
    snapshots: &[PowerShellObjectSnapshot],
    max_depth: i32,
) -> RuntimeResult<Value> {
    let [snapshot] = snapshots else {
        return Err(STATUS_PARSE);
    };
    match snapshot.kind.as_str() {
        "dictionary" | "psobject" => {
            let mut object = Map::new();
            for property in &snapshot.properties {
                object.insert(
                    property.name.clone(),
                    snapshot_to_toml_json_scalar(&property.value, max_depth, 1),
                );
            }
            Ok(Value::Object(object))
        }
        _ => Err(STATUS_PARSE),
    }
}

fn snapshots_to_toml_text(
    snapshots: &[PowerShellObjectSnapshot],
    max_depth: i32,
) -> RuntimeResult<String> {
    let value = snapshots_to_toml_json_value(snapshots, max_depth)?;
    let object = value.as_object().ok_or(STATUS_PARSE)?;
    let mut output = String::new();
    for (key, value) in object {
        write_toml_key(&mut output, key);
        output.push_str(" = ");
        write_json_value_as_toml(&mut output, value)?;
        output.push('\n');
    }
    Ok(output)
}

fn snapshot_to_toml_json_scalar(
    snapshot: &PowerShellObjectSnapshot,
    max_depth: i32,
    depth: i32,
) -> Value {
    if depth > max_depth.max(0) {
        return Value::String(snapshot_to_string(snapshot));
    }

    match snapshot.kind.as_str() {
        "null" => Value::String(String::new()),
        "datetime" => Value::String(normalize_roundtrip_datetime_text(
            snapshot.scalar_value.as_deref().unwrap_or_default(),
        )),
        "scalar" | "enum" => snapshot_scalar_toml_value(snapshot),
        "bytes" => Value::Array(
            decode_base64(snapshot.scalar_value.as_deref().unwrap_or_default())
                .into_iter()
                .map(|byte| Value::Number(serde_json::Number::from(byte)))
                .collect(),
        ),
        "array" => Value::Array(
            snapshot
                .items
                .iter()
                .map(|item| snapshot_to_toml_json_scalar(item, max_depth, depth + 1))
                .collect(),
        ),
        "dictionary" | "psobject" => Value::String(snapshot_to_string(snapshot)),
        "cycle" | "truncated" => Value::String(snapshot_to_string(snapshot)),
        _ => Value::String(snapshot_to_string(snapshot)),
    }
}

fn snapshot_scalar_toml_value(snapshot: &PowerShellObjectSnapshot) -> Value {
    let value = snapshot.scalar_value.as_deref().unwrap_or_default();
    match snapshot.type_name.as_deref().unwrap_or_default() {
        "System.Boolean" | "Deserialized.System.Boolean" if value.eq_ignore_ascii_case("true") => {
            Value::Bool(true)
        }
        "System.Boolean" | "Deserialized.System.Boolean" if value.eq_ignore_ascii_case("false") => {
            Value::Bool(false)
        }
        "System.Byte" | "Deserialized.System.Byte"
        | "System.SByte" | "Deserialized.System.SByte"
        | "System.Int16" | "Deserialized.System.Int16"
        | "System.UInt16" | "Deserialized.System.UInt16"
        | "System.Int32" | "Deserialized.System.Int32"
        | "System.UInt32" | "Deserialized.System.UInt32"
        | "System.Int64" | "Deserialized.System.Int64"
            if value.parse::<i64>().is_ok() =>
        {
            Value::Number(serde_json::Number::from(value.parse::<i64>().unwrap_or_default()))
        }
        "System.UInt64" | "Deserialized.System.UInt64" if value.parse::<u64>().is_ok() => Value::Number(
            serde_json::Number::from(value.parse::<u64>().unwrap_or_default()),
        ),
        "System.Single" | "Deserialized.System.Single"
        | "System.Double" | "Deserialized.System.Double"
        | "System.Decimal" | "Deserialized.System.Decimal"
            if value.parse::<f64>().is_ok_and(f64::is_finite) =>
        {
            serde_json::Number::from_f64(value.parse::<f64>().unwrap_or_default())
                .map(Value::Number)
                .unwrap_or_else(|| Value::String(value.to_owned()))
        }
        _ => Value::String(value.to_owned()),
    }
}

fn write_json_value_as_toml(output: &mut String, value: &Value) -> RuntimeResult<()> {
    match value {
        Value::Null => {
            write_json_string(output, "");
            Ok(())
        }
        Value::Bool(flag) => {
            output.push_str(if *flag { "true" } else { "false" });
            Ok(())
        }
        Value::Number(number) => {
            output.push_str(&number.to_string());
            Ok(())
        }
        Value::String(text) => {
            write_json_string(output, text);
            Ok(())
        }
        Value::Array(items) => {
            output.push('[');
            for (index, item) in items.iter().enumerate() {
                if index > 0 {
                    output.push_str(", ");
                }
                write_json_value_as_toml(output, item)?;
            }
            output.push(']');
            Ok(())
        }
        Value::Object(_) => {
            write_json_string(output, &value.to_string());
            Ok(())
        }
    }
}

fn write_toml_snapshot_value(
    output: &mut String,
    snapshot: &PowerShellObjectSnapshot,
    max_depth: i32,
    depth: i32,
) -> RuntimeResult<()> {
    if depth > max_depth.max(0) {
        write_json_string(output, &snapshot_to_string(snapshot));
        return Ok(());
    }

    match snapshot.kind.as_str() {
        "datetime" => {
            write_json_string(
                output,
                &normalize_roundtrip_datetime_text(
                    snapshot.scalar_value.as_deref().unwrap_or_default(),
                ),
            );
            Ok(())
        }
        "scalar" | "enum" => {
            let value = snapshot.scalar_value.as_deref().unwrap_or_default();
            match snapshot.type_name.as_deref().unwrap_or_default() {
                "System.Boolean" | "Deserialized.System.Boolean" if value.eq_ignore_ascii_case("true") => {
                    output.push_str("true")
                }
                "System.Boolean" | "Deserialized.System.Boolean" if value.eq_ignore_ascii_case("false") => {
                    output.push_str("false")
                }
                "System.Byte" | "Deserialized.System.Byte"
                | "System.SByte" | "Deserialized.System.SByte"
                | "System.Int16" | "Deserialized.System.Int16"
                | "System.UInt16" | "Deserialized.System.UInt16"
                | "System.Int32" | "Deserialized.System.Int32"
                | "System.UInt32" | "Deserialized.System.UInt32"
                | "System.Int64" | "Deserialized.System.Int64"
                    if is_integer_literal(value) =>
                {
                    output.push_str(value);
                }
                "System.UInt64" | "Deserialized.System.UInt64" if is_unsigned_integer_literal(value) => {
                    output.push_str(value)
                }
                "System.Single" | "Deserialized.System.Single"
                | "System.Double" | "Deserialized.System.Double"
                | "System.Decimal" | "Deserialized.System.Decimal"
                    if is_float_literal(value) || is_integer_literal(value) =>
                {
                    output.push_str(value);
                }
                _ => write_json_string(output, value),
            }
            Ok(())
        }
        "array" => {
            output.push('[');
            for (index, item) in snapshot.items.iter().enumerate() {
                if index > 0 {
                    output.push_str(", ");
                }
                write_toml_snapshot_value(output, item, max_depth, depth + 1)?;
            }
            output.push(']');
            Ok(())
        }
        "null" | "dictionary" | "psobject" | "bytes" => Err(STATUS_PARSE),
        "cycle" | "truncated" => {
            write_json_string(output, &snapshot_to_string(snapshot));
            Ok(())
        }
        _ => Err(STATUS_PARSE),
    }
}

fn write_toml_key(output: &mut String, key: &str) {
    if !key.is_empty()
        && key.chars().all(|character| {
            character.is_ascii_alphanumeric() || character == '_' || character == '-'
        })
    {
        output.push_str(key);
    } else {
        write_json_string(output, key);
    }
}

fn toml_text_to_json_text(text: &str) -> RuntimeResult<String> {
    let mut output = String::new();
    output.push('{');
    let mut first = true;

    let mut line = String::new();
    for character in text.chars() {
        if character == '\n' {
            append_toml_line_as_json(&line, &mut output, &mut first)?;
            line.clear();
        } else if character != '\r' {
            line.push(character);
        }
    }
    append_toml_line_as_json(&line, &mut output, &mut first)?;

    output.push('}');
    Ok(output)
}

fn append_toml_line_as_json(
    raw_line: &str,
    output: &mut String,
    first: &mut bool,
) -> RuntimeResult<()> {
    let line = trim_ascii(raw_line);
    if line.is_empty() || line.starts_with('#') {
        return Ok(());
    }
    if line.starts_with('[') {
        return Err(STATUS_PARSE);
    }

    let separator = find_byte(line, b'=').ok_or(STATUS_PARSE)?;
    if separator == 0 {
        return Err(STATUS_PARSE);
    }

    let key = parse_toml_key(trim_ascii(&line[..separator]))?;
    let value = trim_ascii(&line[(separator + 1)..]);

    if !*first {
        output.push(',');
    }
    write_json_string(output, &key);
    output.push(':');
    write_toml_value_as_json(output, value)?;
    *first = false;
    Ok(())
}

fn parse_toml_key(key: &str) -> RuntimeResult<String> {
    if let Some(inner) = quoted_inner(key, b'"') {
        parse_toml_basic_string(inner)
    } else if let Some(inner) = quoted_inner(key, b'\'') {
        Ok(inner.to_owned())
    } else {
        Ok(key.to_owned())
    }
}

fn write_toml_value_as_json(output: &mut String, value: &str) -> RuntimeResult<()> {
    if let Some(inner) = quoted_inner(value, b'"') {
        write_json_string(output, &parse_toml_basic_string(inner)?);
        return Ok(());
    }
    if let Some(inner) = quoted_inner(value, b'\'') {
        write_json_string(output, inner);
        return Ok(());
    }
    if value.eq_ignore_ascii_case("true") {
        output.push_str("true");
        return Ok(());
    }
    if value.eq_ignore_ascii_case("false") {
        output.push_str("false");
        return Ok(());
    }
    if let Some(inner) = bracketed_inner(value, b'[', b']') {
        output.push('[');
        let items = split_toml_array_items(inner)?;
        let mut index = 0usize;
        while index < items.len() {
            if index > 0 {
                output.push(',');
            }
            write_toml_value_as_json(output, trim_ascii(&items[index]))?;
            index += 1;
        }
        output.push(']');
        return Ok(());
    }
    if is_integer_literal(value) || is_unsigned_integer_literal(value) || is_float_literal(value) {
        output.push_str(value);
        return Ok(());
    }

    Err(STATUS_PARSE)
}

fn is_integer_literal(value: &str) -> bool {
    let bytes = value.as_bytes();
    if bytes.is_empty() {
        return false;
    }

    let mut index = 0usize;
    if bytes[0] == b'+' || bytes[0] == b'-' {
        index = 1;
    }
    if index == bytes.len() {
        return false;
    }

    while index < bytes.len() {
        if !bytes[index].is_ascii_digit() {
            return false;
        }
        index += 1;
    }
    true
}

fn is_unsigned_integer_literal(value: &str) -> bool {
    let bytes = value.as_bytes();
    if bytes.is_empty() {
        return false;
    }

    let mut index = 0usize;
    while index < bytes.len() {
        if !bytes[index].is_ascii_digit() {
            return false;
        }
        index += 1;
    }
    true
}

fn is_float_literal(value: &str) -> bool {
    let bytes = value.as_bytes();
    if bytes.is_empty() {
        return false;
    }

    let mut index = 0usize;
    let mut saw_digit = false;
    let mut saw_decimal = false;
    let mut saw_exponent = false;
    if bytes[0] == b'+' || bytes[0] == b'-' {
        index = 1;
    }

    while index < bytes.len() {
        let byte = bytes[index];
        if byte.is_ascii_digit() {
            saw_digit = true;
        } else if byte == b'.' && !saw_decimal && !saw_exponent {
            saw_decimal = true;
        } else if (byte == b'e' || byte == b'E') && !saw_exponent && saw_digit {
            saw_exponent = true;
            saw_digit = false;
            if index + 1 < bytes.len() && (bytes[index + 1] == b'+' || bytes[index + 1] == b'-') {
                index += 1;
            }
        } else {
            return false;
        }
        index += 1;
    }

    saw_digit && (saw_decimal || saw_exponent)
}

fn quoted_inner(value: &str, quote: u8) -> Option<&str> {
    let bytes = value.as_bytes();
    if bytes.len() >= 2 && bytes[0] == quote && bytes[bytes.len() - 1] == quote {
        Some(&value[1..(value.len() - 1)])
    } else {
        None
    }
}

fn bracketed_inner(value: &str, open: u8, close: u8) -> Option<&str> {
    let bytes = value.as_bytes();
    if bytes.len() >= 2 && bytes[0] == open && bytes[bytes.len() - 1] == close {
        Some(&value[1..(value.len() - 1)])
    } else {
        None
    }
}

fn trim_ascii(value: &str) -> &str {
    let bytes = value.as_bytes();
    let mut start = 0usize;
    let mut end = bytes.len();
    while start < end && bytes[start].is_ascii_whitespace() {
        start += 1;
    }
    while end > start && bytes[end - 1].is_ascii_whitespace() {
        end -= 1;
    }
    &value[start..end]
}

fn find_byte(value: &str, needle: u8) -> Option<usize> {
    let bytes = value.as_bytes();
    let mut index = 0usize;
    while index < bytes.len() {
        if bytes[index] == needle {
            return Some(index);
        }
        index += 1;
    }
    None
}

fn parse_toml_basic_string(value: &str) -> RuntimeResult<String> {
    let mut output = String::new();
    let mut escaped = false;
    for character in value.chars() {
        if escaped {
            match character {
                '"' => output.push('"'),
                '\\' => output.push('\\'),
                'n' => output.push('\n'),
                'r' => output.push('\r'),
                't' => output.push('\t'),
                'b' => output.push('\u{08}'),
                'f' => output.push('\u{0c}'),
                _ => return Err(STATUS_PARSE),
            }
            escaped = false;
            continue;
        }

        if character == '\\' {
            escaped = true;
        } else {
            output.push(character);
        }
    }

    if escaped {
        Err(STATUS_PARSE)
    } else {
        Ok(output)
    }
}

fn split_toml_array_items(inner: &str) -> RuntimeResult<Vec<String>> {
    let mut items = Vec::new();
    let mut start = 0usize;
    let mut in_basic_string = false;
    let mut in_literal_string = false;
    let mut escaped = false;
    let bytes = inner.as_bytes();
    let mut index = 0usize;
    while index < bytes.len() {
        let byte = bytes[index];
        if escaped {
            escaped = false;
            index += 1;
            continue;
        }

        match byte {
            b'\\' if in_basic_string => escaped = true,
            b'"' if !in_literal_string => in_basic_string = !in_basic_string,
            b'\'' if !in_basic_string => in_literal_string = !in_literal_string,
            b',' if !in_basic_string && !in_literal_string => {
                items.push(trim_ascii(&inner[start..index]).to_owned());
                start = index + 1;
            }
            _ => {}
        }
        index += 1;
    }

    if in_basic_string || in_literal_string || escaped {
        return Err(STATUS_PARSE);
    }

    let tail = trim_ascii(&inner[start..]);
    if !tail.is_empty() {
        items.push(tail.to_owned());
    }
    Ok(items)
}

fn append_bytes(snapshot: &PowerShellObjectSnapshot, bytes: &mut Vec<u8>) -> RuntimeResult<()> {
    match snapshot.kind.as_str() {
        "bytes" => {
            bytes.extend(decode_base64(
                snapshot.scalar_value.as_deref().unwrap_or_default(),
            ));
            Ok(())
        }
        "scalar" if snapshot.type_name.as_deref() == Some("System.Byte") => {
            let value = snapshot
                .scalar_value
                .as_deref()
                .unwrap_or_default()
                .parse::<u8>()
                .map_err(|_| STATUS_PARSE)?;
            bytes.push(value);
            Ok(())
        }
        "array" => {
            for item in &snapshot.items {
                append_bytes(item, bytes)?;
            }
            Ok(())
        }
        _ => Err(STATUS_PARSE),
    }
}

fn snapshot_to_string(snapshot: &PowerShellObjectSnapshot) -> String {
    match snapshot.kind.as_str() {
        "null" => String::new(),
        "scalar" | "enum" | "datetime" | "truncated" | "cycle" => {
            snapshot.scalar_value.clone().unwrap_or_default()
        }
        "bytes" => snapshot.scalar_value.clone().unwrap_or_default(),
        "array" => snapshot
            .items
            .iter()
            .map(snapshot_to_string)
            .collect::<Vec<_>>()
            .join(" "),
        "dictionary" | "psobject" => {
            let content = snapshot
                .properties
                .iter()
                .map(|property| {
                    let value = match property.value.kind.as_str() {
                        "null" => String::new(),
                        "scalar" | "enum" | "datetime" | "bytes" | "truncated" | "cycle" => {
                            snapshot_to_string(&property.value)
                        }
                        _ => String::new(),
                    };
                    format!("{}={}", property.name, value)
                })
                .collect::<Vec<_>>()
                .join("; ");
            format!("@{{{}}}", content)
        }
        _ => snapshot.scalar_value.clone().unwrap_or_default(),
    }
}

fn snapshot_scalar_text(snapshot: &PowerShellObjectSnapshot) -> Option<String> {
    match snapshot.kind.as_str() {
        "null" => Some(String::new()),
        "scalar" | "enum" | "datetime" | "bytes" | "truncated" | "cycle" => {
            Some(snapshot.scalar_value.clone().unwrap_or_default())
        }
        _ => None,
    }
}

fn join_text_items(items: &[String]) -> String {
    let mut output = String::new();
    for (index, item) in items.iter().enumerate() {
        if index > 0 {
            output.push_str("\r\n");
        }
        output.push_str(item);
    }
    output
}

fn snapshot_to_string_array(snapshot: &PowerShellObjectSnapshot) -> Vec<String> {
    match snapshot.kind.as_str() {
        "null" => Vec::new(),
        "array" => snapshot.items.iter().map(snapshot_to_string).collect(),
        _ => vec![snapshot_to_string(snapshot)],
    }
}

fn create_to_csv_request(
    context: CmdletContext,
    snapshots: &[PowerShellObjectSnapshot],
) -> RuntimeResult<String> {
    let delimiter = resolve_delimiter(&context)?;
    let rows = snapshots
        .iter()
        .map(csv_row_from_snapshot)
        .collect::<Vec<_>>();
    let headers = rows
        .first()
        .map(|row| row.iter().map(|field| field.0.clone()).collect::<Vec<_>>())
        .unwrap_or_default();
    let row_values = rows
        .iter()
        .map(|row| {
            headers
                .iter()
                .map(|header| {
                    row.iter()
                        .find(|field| &field.0 == header)
                        .map(|field| Value::String(field.1.clone()))
                        .unwrap_or_else(|| Value::String(String::new()))
                })
                .collect::<Vec<_>>()
        })
        .collect::<Vec<_>>();

    let type_name = snapshots
        .first()
        .and_then(|snapshot| snapshot.type_name.clone())
        .unwrap_or_default();
    let mut request = Map::new();
    request.insert("delimiter".to_owned(), Value::String(delimiter.to_string()));
    request.insert(
        "includeTypeInformation".to_owned(),
        Value::Bool(context.parameter_bool("IncludeTypeInformation")?),
    );
    request.insert(
        "noHeader".to_owned(),
        Value::Bool(context.parameter_bool("NoHeader")?),
    );
    request.insert(
        "quoteFields".to_owned(),
        Value::Array(
            context
                .parameter_string_array("QuoteFields")?
                .into_iter()
                .map(Value::String)
                .collect(),
        ),
    );
    request.insert(
        "useQuotes".to_owned(),
        Value::String(context.parameter_string_or("UseQuotes", "Always")?),
    );
    request.insert("typeName".to_owned(), Value::String(type_name));
    request.insert(
        "headers".to_owned(),
        Value::Array(headers.into_iter().map(Value::String).collect()),
    );
    request.insert(
        "rows".to_owned(),
        Value::Array(row_values.into_iter().map(Value::Array).collect()),
    );

    serde_json::to_string(&Value::Object(request)).map_err(|_| STATUS_PARSE)
}

fn snapshots_to_csv_lines(
    context: &CmdletContext,
    snapshots: &[PowerShellObjectSnapshot],
) -> RuntimeResult<Vec<String>> {
    let delimiter = resolve_delimiter(context)?;
    let include_type_information = context.parameter_bool("IncludeTypeInformation")?;
    let no_header = context.parameter_bool("NoHeader")?;
    let quote_fields = context.parameter_string_array("QuoteFields")?;
    let use_quotes = if context.has_parameter("UseQuotes")? {
        context.parameter_string_or("UseQuotes", "Always")?
    } else if quote_fields.is_empty() {
        String::from("Always")
    } else {
        String::from("Never")
    };

    let mut headers = Vec::new();
    if !snapshots.is_empty() {
        let first_row = csv_row_values_from_snapshot(&snapshots[0]);
        for field in first_row {
            headers.push(field.0.clone());
        }
    }

    let mut lines = Vec::new();
    if include_type_information {
        let mut line = String::from("#TYPE ");
        if !snapshots.is_empty() {
            if let Some(type_name) = snapshots[0].type_name.as_deref() {
                line.push_str(type_name);
            }
        }
        lines.push(line);
    }

    if !no_header {
        let mut header_values = Vec::new();
        for header in &headers {
            header_values.push(CsvFieldValue::new(header.clone(), false));
        }
        lines.push(format_csv_record(
            &header_values,
            delimiter,
            &use_quotes,
            &quote_fields,
        ));
    }

    let quote_field_indexes = csv_quote_field_indexes(&headers, &quote_fields);
    for snapshot in snapshots {
        let row = csv_row_values_from_snapshot(snapshot);
        let mut values: Vec<CsvFieldValue> = Vec::new();
        for header in &headers {
            let mut value = CsvFieldValue::new(String::new(), true);
            for field in &row {
                if &field.0 == header {
                    value = field.1.clone();
                    break;
                }
            }
            values.push(value);
        }
        lines.push(format_csv_record(
            &values,
            delimiter,
            &use_quotes,
            &quote_field_indexes,
        ));
    }

    Ok(lines)
}

fn csv_quote_field_indexes(headers: &[String], quote_fields: &[String]) -> Vec<String> {
    let mut values = Vec::new();
    let mut index = 0usize;
    while index < headers.len() {
        let mut quote_index = 0usize;
        while quote_index < quote_fields.len() {
            if quote_fields[quote_index] == headers[index] {
                values.push(index.to_string());
                break;
            }
            quote_index += 1;
        }
        index += 1;
    }
    values
}

#[derive(Clone)]
struct CsvFieldValue {
    text: String,
    is_null: bool,
}

impl CsvFieldValue {
    fn new(text: String, is_null: bool) -> Self {
        Self { text, is_null }
    }
}

fn csv_row_values_from_snapshot(snapshot: &PowerShellObjectSnapshot) -> Vec<(String, CsvFieldValue)> {
    match snapshot.kind.as_str() {
        "null" => vec![(
            "Length".to_owned(),
            CsvFieldValue::new(String::new(), true),
        )],
        "dictionary" | "psobject" if !snapshot.properties.is_empty() => {
            let mut values = Vec::new();
            for property in &snapshot.properties {
                values.push((
                    property.name.clone(),
                    csv_field_value_from_snapshot(&property.value),
                ));
            }
            values
        }
        _ => vec![(
            "Length".to_owned(),
            csv_field_value_from_snapshot(snapshot),
        )],
    }
}

fn csv_field_value_from_snapshot(snapshot: &PowerShellObjectSnapshot) -> CsvFieldValue {
    CsvFieldValue::new(snapshot_to_string(snapshot), snapshot.kind == "null")
}

fn format_csv_record(
    values: &[CsvFieldValue],
    delimiter: char,
    use_quotes: &str,
    quote_fields: &[String],
) -> String {
    let mut output = String::new();
    let mut index = 0usize;
    while index < values.len() {
        if index > 0 {
            output.push(delimiter);
        }

        let mut force_quote = false;
        let index_text = index.to_string();
        let mut quote_index = 0usize;
        while quote_index < quote_fields.len() {
            if quote_fields[quote_index] == index_text
                || quote_fields[quote_index] == values[index].text
            {
                force_quote = true;
                break;
            }
            quote_index += 1;
        }

        write_csv_field(&mut output, &values[index], delimiter, use_quotes, force_quote);
        index += 1;
    }
    output
}

fn write_csv_field(
    output: &mut String,
    value: &CsvFieldValue,
    delimiter: char,
    use_quotes: &str,
    force_quote: bool,
) {
    let should_quote = if force_quote {
        true
    } else if value.is_null {
        false
    } else {
        match use_quotes {
            "Never" => false,
            "AsNeeded" => csv_field_needs_quotes(&value.text, delimiter),
            _ => true,
        }
    };

    if !should_quote {
        output.push_str(&value.text);
        return;
    }

    output.push('"');
    for ch in value.text.chars() {
        if ch == '"' {
            output.push('"');
            output.push('"');
        } else {
            output.push(ch);
        }
    }
    output.push('"');
}

fn csv_field_needs_quotes(value: &str, delimiter: char) -> bool {
    for ch in value.chars() {
        if ch == delimiter || ch == '"' || ch == '\r' || ch == '\n' {
            return true;
        }
    }
    false
}

fn parse_snapshot_json(input: &str) -> RuntimeResult<PowerShellObjectSnapshot> {
    SnapshotJsonParser::new(input).parse_snapshot()
}

struct SnapshotJsonParser<'a> {
    chars: Peekable<Chars<'a>>,
}

impl<'a> SnapshotJsonParser<'a> {
    fn new(input: &'a str) -> Self {
        Self {
            chars: input.chars().peekable(),
        }
    }

    fn parse_snapshot(mut self) -> RuntimeResult<PowerShellObjectSnapshot> {
        self.skip_whitespace();
        let snapshot = self.parse_snapshot_object()?;
        self.skip_whitespace();
        if self.chars.next().is_some() {
            return Err(STATUS_PARSE);
        }
        Ok(snapshot)
    }

    fn parse_snapshot_object(&mut self) -> RuntimeResult<PowerShellObjectSnapshot> {
        self.expect_char('{')?;
        let mut kind = None;
        let mut type_name = None;
        let mut scalar_value = None;
        let mut items = Vec::new();
        let mut properties = Vec::new();

        loop {
            self.skip_whitespace();
            if self.consume_char('}') {
                break;
            }

            let name = self.parse_string()?;
            self.expect_char(':')?;
            match name.as_str() {
                "kind" => kind = Some(self.parse_string()?),
                "typeName" => type_name = self.parse_nullable_string()?,
                "scalarValue" => scalar_value = self.parse_nullable_string()?,
                "items" => items = self.parse_snapshot_array()?,
                "properties" => properties = self.parse_property_array()?,
                _ => return Err(STATUS_PARSE),
            }

            self.skip_whitespace();
            if self.consume_char('}') {
                break;
            }
            self.expect_char(',')?;
        }

        Ok(PowerShellObjectSnapshot {
            kind: kind.ok_or(STATUS_PARSE)?,
            type_name,
            scalar_value,
            items,
            properties,
        })
    }

    fn parse_property_object(&mut self) -> RuntimeResult<PowerShellPropertySnapshot> {
        self.expect_char('{')?;
        let mut name = None;
        let mut value = None;

        loop {
            self.skip_whitespace();
            if self.consume_char('}') {
                break;
            }

            let field = self.parse_string()?;
            self.expect_char(':')?;
            match field.as_str() {
                "name" => name = Some(self.parse_string()?),
                "value" => value = Some(self.parse_snapshot_object()?),
                _ => return Err(STATUS_PARSE),
            }

            self.skip_whitespace();
            if self.consume_char('}') {
                break;
            }
            self.expect_char(',')?;
        }

        Ok(PowerShellPropertySnapshot {
            name: name.ok_or(STATUS_PARSE)?,
            value: value.ok_or(STATUS_PARSE)?,
        })
    }

    fn parse_snapshot_array(&mut self) -> RuntimeResult<Vec<PowerShellObjectSnapshot>> {
        self.expect_char('[')?;
        let mut items = Vec::new();
        loop {
            self.skip_whitespace();
            if self.consume_char(']') {
                break;
            }
            items.push(self.parse_snapshot_object()?);
            self.skip_whitespace();
            if self.consume_char(']') {
                break;
            }
            self.expect_char(',')?;
        }
        Ok(items)
    }

    fn parse_property_array(&mut self) -> RuntimeResult<Vec<PowerShellPropertySnapshot>> {
        self.expect_char('[')?;
        let mut properties = Vec::new();
        loop {
            self.skip_whitespace();
            if self.consume_char(']') {
                break;
            }
            properties.push(self.parse_property_object()?);
            self.skip_whitespace();
            if self.consume_char(']') {
                break;
            }
            self.expect_char(',')?;
        }
        Ok(properties)
    }

    fn parse_nullable_string(&mut self) -> RuntimeResult<Option<String>> {
        self.skip_whitespace();
        if self.consume_literal("null") {
            return Ok(None);
        }
        Ok(Some(self.parse_string()?))
    }

    fn parse_string(&mut self) -> RuntimeResult<String> {
        self.expect_char('"')?;
        let mut output = String::new();
        loop {
            let Some(ch) = self.chars.next() else {
                return Err(STATUS_PARSE);
            };
            match ch {
                '"' => return Ok(output),
                '\\' => output.push(self.parse_escape_sequence()?),
                ch if (ch as u32) < 0x20 => return Err(STATUS_PARSE),
                ch => output.push(ch),
            }
        }
    }

    fn parse_escape_sequence(&mut self) -> RuntimeResult<char> {
        let Some(escape) = self.chars.next() else {
            return Err(STATUS_PARSE);
        };
        match escape {
            '"' => Ok('"'),
            '\\' => Ok('\\'),
            '/' => Ok('/'),
            'b' => Ok('\u{08}'),
            'f' => Ok('\u{0c}'),
            'n' => Ok('\n'),
            'r' => Ok('\r'),
            't' => Ok('\t'),
            'u' => self.parse_unicode_escape(),
            _ => Err(STATUS_PARSE),
        }
    }

    fn parse_unicode_escape(&mut self) -> RuntimeResult<char> {
        let first = self.parse_hex_u16()?;
        if (0xd800..=0xdbff).contains(&first) {
            if self.chars.next() != Some('\\') || self.chars.next() != Some('u') {
                return Err(STATUS_PARSE);
            }
            let second = self.parse_hex_u16()?;
            if !(0xdc00..=0xdfff).contains(&second) {
                return Err(STATUS_PARSE);
            }
            let scalar = 0x10000 + (((first as u32 - 0xd800) << 10) | (second as u32 - 0xdc00));
            return char::from_u32(scalar).ok_or(STATUS_PARSE);
        }
        if (0xdc00..=0xdfff).contains(&first) {
            return Err(STATUS_PARSE);
        }
        char::from_u32(first as u32).ok_or(STATUS_PARSE)
    }

    fn parse_hex_u16(&mut self) -> RuntimeResult<u16> {
        let mut value = 0u16;
        for _ in 0..4 {
            let Some(ch) = self.chars.next() else {
                return Err(STATUS_PARSE);
            };
            let Some(digit) = ch.to_digit(16) else {
                return Err(STATUS_PARSE);
            };
            value = (value << 4) | digit as u16;
        }
        Ok(value)
    }

    fn consume_literal(&mut self, expected: &str) -> bool {
        let mut clone = self.chars.clone();
        for expected_char in expected.chars() {
            if clone.next() != Some(expected_char) {
                return false;
            }
        }
        self.chars = clone;
        true
    }

    fn expect_char(&mut self, expected: char) -> RuntimeResult<()> {
        self.skip_whitespace();
        match self.chars.next() {
            Some(ch) if ch == expected => Ok(()),
            _ => Err(STATUS_PARSE),
        }
    }

    fn consume_char(&mut self, expected: char) -> bool {
        self.skip_whitespace();
        matches!(self.chars.peek().copied(), Some(ch) if ch == expected)
            .then(|| self.chars.next())
            .is_some()
    }

    fn skip_whitespace(&mut self) {
        while matches!(self.chars.peek(), Some(ch) if ch.is_ascii_whitespace()) {
            self.chars.next();
        }
    }
}

fn create_from_csv_request(context: CmdletContext, csv: &str) -> RuntimeResult<String> {
    let delimiter = resolve_delimiter(&context)?;
    let header = context.parameter_string_array("Header")?;
    let mut request = Map::new();
    request.insert("csv".to_owned(), Value::String(csv.to_owned()));
    request.insert("delimiter".to_owned(), Value::String(delimiter.to_string()));
    request.insert(
        "header".to_owned(),
        if header.is_empty() {
            Value::Null
        } else {
            Value::Array(header.into_iter().map(Value::String).collect())
        },
    );

    serde_json::to_string(&Value::Object(request)).map_err(|_| STATUS_PARSE)
}

fn resolve_delimiter(context: &CmdletContext) -> RuntimeResult<char> {
    if context.has_parameter("Delimiter")? {
        return context.parameter_char_or("Delimiter", ',');
    }
    if context.parameter_bool("UseCulture")? {
        return Ok(context
            .current_culture_list_separator()?
            .chars()
            .next()
            .unwrap_or(','));
    }
    Ok(',')
}

fn csv_row_from_snapshot(snapshot: &PowerShellObjectSnapshot) -> Vec<(String, String)> {
    match snapshot.kind.as_str() {
        "null" => vec![("Length".to_owned(), "0".to_owned())],
        "dictionary" | "psobject" if !snapshot.properties.is_empty() => snapshot
            .properties
            .iter()
            .map(|property| (property.name.clone(), snapshot_to_string(&property.value)))
            .collect(),
        _ => vec![("Length".to_owned(), snapshot_to_string(snapshot))],
    }
}

fn transform_utf8(
    input: &str,
    len: unsafe extern "C" fn(*const u8, i64) -> i64,
    copy: unsafe extern "C" fn(*const u8, i64, *mut u8, i64) -> i64,
) -> RuntimeResult<String> {
    String::from_utf8(transform_utf8_to_bytes(input, len, copy)?).map_err(|_| STATUS_TRANSFORM)
}

fn transform_utf8_to_bytes(
    input: &str,
    len: unsafe extern "C" fn(*const u8, i64) -> i64,
    copy: unsafe extern "C" fn(*const u8, i64, *mut u8, i64) -> i64,
) -> RuntimeResult<Vec<u8>> {
    transform_bytes(input.as_bytes(), len, copy)
}

fn transform_bytes_to_utf8(
    input: &[u8],
    len: unsafe extern "C" fn(*const u8, i64) -> i64,
    copy: unsafe extern "C" fn(*const u8, i64, *mut u8, i64) -> i64,
) -> RuntimeResult<String> {
    String::from_utf8(transform_bytes(input, len, copy)?).map_err(|_| STATUS_TRANSFORM)
}

fn transform_bytes(
    input: &[u8],
    len: unsafe extern "C" fn(*const u8, i64) -> i64,
    copy: unsafe extern "C" fn(*const u8, i64, *mut u8, i64) -> i64,
) -> RuntimeResult<Vec<u8>> {
    let input_ptr = if input.is_empty() {
        std::ptr::null()
    } else {
        input.as_ptr()
    };
    let length = unsafe { len(input_ptr, input.len() as i64) };
    if length < 0 {
        return Err(STATUS_TRANSFORM);
    }

    let mut output = vec![0u8; length as usize];
    let output_ptr = if output.is_empty() {
        std::ptr::null_mut()
    } else {
        output.as_mut_ptr()
    };
    let copied = unsafe {
        copy(
            input_ptr,
            input.len() as i64,
            output_ptr,
            output.len() as i64,
        )
    };
    if copied < 0 {
        return Err(STATUS_TRANSFORM);
    }
    output.truncate(copied as usize);
    Ok(output)
}

fn with_managed_string<T>(
    value: &str,
    action: impl FnOnce(&ManagedString) -> RuntimeResult<T>,
) -> RuntimeResult<T> {
    let managed = ManagedString::from_utf8(value)?;
    let result = action(&managed);
    let release = managed.release();
    release?;
    result
}

fn release_handle(handle: i32) -> RuntimeResult<()> {
    let mut exception_handle = 0;
    let released =
        unsafe { rustlyn_bindgen_powershell_object_release(handle, &mut exception_handle) };
    exception_to_result(exception_handle)?;
    if released == 0 {
        Err(STATUS_EXCEPTION)
    } else {
        Ok(())
    }
}

fn exception_to_result(exception_handle: i32) -> RuntimeResult<()> {
    if exception_handle == 0 {
        Ok(())
    } else {
        Err(STATUS_EXCEPTION)
    }
}

fn decode_base64(value: &str) -> Vec<u8> {
    const INVALID: u8 = 255;
    let mut output = Vec::new();
    let mut block = [0u8; 4];
    let mut block_len = 0usize;
    for byte in value.bytes().filter(|byte| !byte.is_ascii_whitespace()) {
        block[block_len] = match byte {
            b'A'..=b'Z' => byte - b'A',
            b'a'..=b'z' => byte - b'a' + 26,
            b'0'..=b'9' => byte - b'0' + 52,
            b'+' => 62,
            b'/' => 63,
            b'=' => INVALID,
            _ => return Vec::new(),
        };
        block_len += 1;
        if block_len == 4 {
            if block[0] == INVALID || block[1] == INVALID {
                return Vec::new();
            }
            output.push((block[0] << 2) | (block[1] >> 4));
            if block[2] != INVALID {
                output.push((block[1] << 4) | (block[2] >> 2));
            }
            if block[3] != INVALID {
                output.push((block[2] << 6) | block[3]);
            }
            block_len = 0;
        }
    }

    output
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn snapshot_json_projection_preserves_order_and_scalars() {
        let snapshot = sample_snapshot();
        let json = snapshots_to_json_value(&[snapshot.clone()], 8, false);

        assert_eq!(json.to_string(), r#"{"name":"rustlyn","count":3}"#);
        assert_eq!(
            snapshots_to_json_text(&[snapshot], 8, false, false),
            r#"{"name":"rustlyn","count":3}"#
        );
    }

    #[test]
    fn object_stream_projection_feeds_text_and_binary_engines() {
        let snapshot = sample_snapshot();
        let stream = snapshots_to_object_stream(&[snapshot], 8, false);

        assert_eq!(stream, "O2:4:nameS7:rustlyn5:countI3;");

        let yaml = transform_utf8(
            &stream,
            marked_yaml_engine::marked_yaml_object_stream_to_yaml_len,
            marked_yaml_engine::marked_yaml_object_stream_to_yaml_copy,
        )
        .expect("yaml transform should succeed");
        assert!(yaml.contains("name: \"rustlyn\""));
        assert!(yaml.contains("count: 3"));

        let bson = transform_utf8_to_bytes(
            &stream,
            bson_engine::bson_object_stream_to_bson_len,
            bson_engine::bson_object_stream_to_bson_copy,
        )
        .expect("bson transform should succeed");
        let bson_json = transform_bytes_to_utf8(
            &bson,
            bson_engine::bson_to_json_len,
            bson_engine::bson_to_json_copy,
        )
        .expect("bson json transform should succeed");
        assert_eq!(bson_json, r#"{"name":"rustlyn","count":3}"#);

        let cbor = transform_utf8_to_bytes(
            &stream,
            cbor_engine::cbor_object_stream_to_cbor_len,
            cbor_engine::cbor_object_stream_to_cbor_copy,
        )
        .expect("cbor transform should succeed");
        let cbor_json = transform_bytes_to_utf8(
            &cbor,
            cbor_engine::cbor_to_json_len,
            cbor_engine::cbor_to_json_copy,
        )
        .expect("cbor json transform should succeed");
        assert_eq!(cbor_json, r#"{"name":"rustlyn","count":3}"#);
    }

    #[test]
    fn json_number_object_stream_preserves_negative_decimal_text() {
        assert_eq!(
            json_to_object_stream("-3.5").expect("decimal stream should succeed"),
            "D-3.5;"
        );
    }

    #[test]
    fn roundtrip_datetime_text_trims_only_insignificant_fractional_zeros() {
        assert_eq!(
            normalize_roundtrip_datetime_text("2024-01-01T22:04:05.0000000-05:00"),
            "2024-01-01T22:04:05-05:00"
        );
        assert_eq!(
            normalize_roundtrip_datetime_text("2024-01-02T03:04:05.1200000+02:30"),
            "2024-01-02T03:04:05.12+02:30"
        );
        assert_eq!(
            normalize_roundtrip_datetime_text("2024-01-02T03:04:05.1234567+02:30"),
            "2024-01-02T03:04:05.1234567+02:30"
        );
    }

    #[test]
    fn snapshot_parser_preserves_literal_and_escaped_astral_unicode() {
        let literal = parse_snapshot_json(
            r#"{"kind":"scalar","typeName":"System.String","scalarValue":"emoji 🚀 𐐷 𝄞","items":[],"properties":[]}"#,
        )
        .expect("literal astral snapshot should parse");
        assert_eq!(
            literal.scalar_value.as_deref(),
            Some("emoji 🚀 𐐷 𝄞")
        );

        let escaped = parse_snapshot_json(
            r#"{"kind":"scalar","typeName":"System.String","scalarValue":"emoji \uD83D\uDE80 \uD801\uDC37 \uD834\uDD1E","items":[],"properties":[]}"#,
        )
        .expect("escaped astral snapshot should parse");
        assert_eq!(
            escaped.scalar_value.as_deref(),
            Some("emoji 🚀 𐐷 𝄞")
        );
    }

    #[test]
    fn json_writer_matches_negative_zero_and_separator_escaping() {
        let mut output = String::new();
        write_json_string(&mut output, "\u{2028}line\u{2029}para");
        assert_eq!(output, r#""\u2028line\u2029para""#);

        let negative_zero = PowerShellObjectSnapshot {
            kind: "scalar".to_owned(),
            type_name: Some("System.Double".to_owned()),
            scalar_value: Some("-0".to_owned()),
            items: Vec::new(),
            properties: Vec::new(),
        };
        assert_eq!(snapshots_to_json_text(&[negative_zero], 8, false, false), "-0.0");
    }

    #[test]
    fn deserialized_numeric_scalar_types_stay_numeric() {
        let snapshot = PowerShellObjectSnapshot {
            kind: "dictionary".to_owned(),
            type_name: Some("System.Collections.Specialized.OrderedDictionary".to_owned()),
            scalar_value: None,
            items: Vec::new(),
            properties: vec![PowerShellPropertySnapshot {
                name: "count".to_owned(),
                value: PowerShellObjectSnapshot {
                    kind: "scalar".to_owned(),
                    type_name: Some("Deserialized.System.Int32".to_owned()),
                    scalar_value: Some("3".to_owned()),
                    items: Vec::new(),
                    properties: Vec::new(),
                },
            }],
        };

        assert_eq!(snapshots_to_json_text(&[snapshot.clone()], 8, false, false), r#"{"count":3}"#);
        assert_eq!(snapshots_to_toml_text(&[snapshot], 8).expect("toml transform should succeed"), "count = 3\n");
    }

    #[test]
    fn text_format_engines_round_trip_through_json_projection() {
        let toml =
            snapshots_to_toml_text(&[sample_snapshot()], 8).expect("toml transform should succeed");
        assert!(toml.contains("name = \"rustlyn\""));
        assert!(toml.contains("count = 3"));

        let toml_json = toml_text_to_json_text(&toml).expect("toml json transform should succeed");
        assert_eq!(toml_json, r#"{"name":"rustlyn","count":3}"#);

        let yaml_json = transform_utf8(
            "name: rustlyn\ncount: 3\n",
            marked_yaml_engine::marked_yaml_to_json_len,
            marked_yaml_engine::marked_yaml_to_json_copy,
        )
        .expect("yaml json transform should succeed");
        assert_eq!(yaml_json, r#"{"name":"rustlyn","count":"3"}"#);
    }

    #[test]
    fn text_input_fallback_uses_scalar_snapshot() {
        let snapshot = PowerShellObjectSnapshot {
            kind: "scalar".to_owned(),
            type_name: Some("System.String".to_owned()),
            scalar_value: Some("name = \"rustlyn\"\ncount = 3\n".to_owned()),
            items: Vec::new(),
            properties: Vec::new(),
        };

        assert_eq!(
            snapshot_scalar_text(&snapshot).as_deref(),
            Some("name = \"rustlyn\"\ncount = 3\n")
        );
        assert!(snapshot_scalar_text(&sample_snapshot()).is_none());
    }

    #[test]
    fn snapshot_parser_preserves_json_text_with_unicode_and_escapes() {
        let json_text = "{\"unicode\":\"emoji 🚀 café\",\"escaped\":\"quote \\\" slash \\\\ newline\\n\"}";
        let snapshot_json = r#"{"kind":"scalar","typeName":"System.String","scalarValue":"{\u0022unicode\u0022:\u0022emoji \uD83D\uDE80 caf\u00E9\u0022,\u0022escaped\u0022:\u0022quote \\\u0022 slash \\\\ newline\\n\u0022}","items":[],"properties":[]}"#;

        let snapshot = parse_snapshot_json(&snapshot_json)
            .expect("json text snapshot should parse");

        assert_eq!(snapshot_scalar_text(&snapshot).as_deref(), Some(json_text));
    }

    #[test]
    fn text_items_join_without_slice_join_intrinsic() {
        let items = vec!["name = \"rustlyn\"".to_owned(), "count = 3".to_owned()];
        assert_eq!(join_text_items(&items), "name = \"rustlyn\"\r\ncount = 3");
    }

    #[test]
    fn csv_engine_uses_rust_built_request_shape() {
        let request = r#"{"delimiter":",","includeTypeInformation":false,"noHeader":false,"quoteFields":[],"useQuotes":"AsNeeded","typeName":"","headers":["name","count"],"rows":[["rustlyn","3"]]}"#;
        let csv_lines = transform_utf8(
            request,
            csv_engine::csv_json_to_csv_len,
            csv_engine::csv_json_to_csv_copy,
        )
        .expect("csv transform should succeed");
        assert_eq!(csv_lines, r#"["name,count","rustlyn,3"]"#);

        let parse_request = r#"{"csv":"name,count\r\nrustlyn,3","delimiter":",","header":null}"#;
        let csv_json = transform_utf8(
            parse_request,
            csv_engine::csv_to_json_len,
            csv_engine::csv_to_json_copy,
        )
        .expect("csv json transform should succeed");
        assert_eq!(csv_json, r#"[{"name":"rustlyn","count":"3"}]"#);
    }

    fn sample_snapshot() -> PowerShellObjectSnapshot {
        PowerShellObjectSnapshot {
            kind: "dictionary".to_owned(),
            type_name: Some("System.Collections.Specialized.OrderedDictionary".to_owned()),
            scalar_value: None,
            items: Vec::new(),
            properties: vec![
                PowerShellPropertySnapshot {
                    name: "name".to_owned(),
                    value: PowerShellObjectSnapshot {
                        kind: "scalar".to_owned(),
                        type_name: Some("System.String".to_owned()),
                        scalar_value: Some("rustlyn".to_owned()),
                        items: Vec::new(),
                        properties: Vec::new(),
                    },
                },
                PowerShellPropertySnapshot {
                    name: "count".to_owned(),
                    value: PowerShellObjectSnapshot {
                        kind: "scalar".to_owned(),
                        type_name: Some("System.Int32".to_owned()),
                        scalar_value: Some("3".to_owned()),
                        items: Vec::new(),
                        properties: Vec::new(),
                    },
                },
            ],
        }
    }
}
