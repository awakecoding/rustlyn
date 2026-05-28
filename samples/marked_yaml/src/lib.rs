use std::{collections::HashMap, mem::forget, ptr, str};

use marked_yaml::{
    FromYamlError, LoadError, LoaderOptions, Span, Spanned, from_node, from_yaml,
    from_yaml_with_options, parse_yaml, parse_yaml_with_options,
};
use serde::Deserialize;

const CHARACTER_DOC: &str = r#"# some comment

key:       value   #   another  comment
"#;

const CHARACTER_REWRITE: &str = r#"# some comment

key:       new_value   #   another  comment
"#;

const TEST_DOC: &str = r#"# Line one is a comment
top:
  - level
  - is always
  - two
  - strings
u8s: [ 0, 1, 2, "255" ]
i8s: [ -128, 0, 127 ]
u32s: [ 65537 ]
thingy: blue
outcome: 
    bad: stuff
looksee: { ugly: [ first, second ] }
known: { unknown: { name: Jeff, age: 14 } }
kvs:
    first: one
    second: two
    third: banana
falsy: false
truthy: true
yes: true
"#;

#[derive(Debug, Deserialize)]
#[allow(dead_code)]
struct FullTest {
    top: Vec<Spanned<String>>,
    u8s: Spanned<Vec<u8>>,
    i8s: Vec<i8>,
    u32s: Vec<u32>,
    missing: Option<String>,
    thingy: Colour,
    outcome: EnumCheck,
    looksee: EnumCheck,
    known: EnumCheck,
    kvs: HashMap<Spanned<String>, Spanned<String>>,
    falsy: Spanned<bool>,
    truthy: Spanned<bool>,
    yes: Spanned<bool>,
}

#[derive(Debug, Deserialize)]
#[allow(dead_code)]
enum EnumCheck {
    #[serde(alias = "good")]
    Good(String),
    #[serde(alias = "bad")]
    Bad(Spanned<String>),
    #[serde(alias = "ugly")]
    Ugly(String, String),
    #[serde(alias = "unknown")]
    Unknown { name: Spanned<String>, age: i64 },
}

#[derive(Debug, Deserialize)]
enum Colour {
    #[serde(alias = "blue")]
    Blue,
    #[serde(alias = "red")]
    Red,
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_parse_only_ok() -> i32 {
    match parse_yaml(0, CHARACTER_DOC) {
        Ok(document) => {
            forget(document);
            1
        }
        Err(err) => {
            forget(err);
            0
        }
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_character_span_start() -> i32 {
    character_span()
        .map(|(start, _)| start as i32)
        .unwrap_or(-1)
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_character_span_end() -> i32 {
    character_span().map(|(_, end)| end as i32).unwrap_or(-1)
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_character_rewrite_matches() -> i32 {
    match character_span() {
        Some((start, end)) => {
            i32::from(character_rewrite_hash(start, end) == hash_chars(CHARACTER_REWRITE.chars()))
        }
        None => -1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_read_everything_score() -> i32 {
    let nodes = match parse_yaml(0, TEST_DOC) {
        Ok(nodes) => nodes,
        Err(_) => return -1,
    };
    let doc: FullTest = match from_node(&nodes) {
        Ok(doc) => doc,
        Err(_) => return -2,
    };

    let mut score = 0;
    if doc.top[0].as_str() == "level" {
        score += 1;
    }
    if doc.falsy == false {
        score += 2;
    }
    if doc.truthy == true {
        score += 4;
    }
    if doc.truthy != doc.falsy {
        score += 8;
    }
    if doc.truthy == doc.yes {
        score += 16;
    }
    if doc.top[2] == "two" {
        score += 32;
    }
    let s = String::from("s");
    if doc.top[3] != "two" && doc.top[0] != s {
        score += 64;
    }
    forget(doc);
    forget(nodes);
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_ergonomics_score() -> i32 {
    let nodes = match parse_yaml(0, TEST_DOC) {
        Ok(nodes) => nodes,
        Err(_) => return -1,
    };
    let doc: FullTest = match from_node(&nodes) {
        Ok(doc) => doc,
        Err(_) => {
            forget(nodes);
            return -1;
        }
    };

    let mut score = 0;
    if doc.kvs.get("first").map(|s| s.as_str()) == Some("one") {
        score += 1;
    }
    let k1 = Spanned::new(Span::new_blank(), "k1");
    let mut map = HashMap::new();
    map.insert(k1, "v1");
    let k2 = Spanned::new(Span::new_blank(), "k2");
    if !map.contains_key(&k2) {
        score += 2;
    }
    if map.contains_key("k1") {
        score += 4;
    }
    forget(map);
    forget(doc);
    forget(nodes);
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_parse_fails_score() -> i32 {
    let mut score = 0;

    if matches!(
        from_yaml::<FullTest>(0, "hello world").err(),
        Some(FromYamlError::ParseYaml(_))
    ) {
        score += 1;
    }

    if let Some(err) = from_yaml::<FullTest>(0, "hello: world").err() {
        if matches!(err, FromYamlError::FromNode(_)) {
            score += 2;
        }
        if format!("{err}").starts_with("missing field") {
            score += 4;
        }
    }

    #[derive(Deserialize)]
    #[allow(dead_code)]
    struct MiniDoc {
        colour: Colour,
    }

    if let Some(err) = from_yaml::<MiniDoc>(0, "colour: {Red: optional}").err() {
        if format!("{err}") == "colour.Red: invalid type: map, expected String" {
            score += 8;
        }
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_parse_fails_coerce_score() -> i32 {
    let options = LoaderOptions::default().prevent_coercion(true);
    let err = match from_yaml_with_options::<FullTest>(0, TEST_DOC, options).err() {
        Some(err) => err,
        None => return -1,
    };
    let text = format!("{err}");
    let mut score = 0;
    if text.starts_with("u8s[3]") {
        score += 1;
    }
    let text = text.strip_prefix("u8s[3]: ").unwrap_or(text.as_str());
    if text.starts_with("invalid type: string") {
        score += 2;
    }
    if text.contains("expected u8") {
        score += 4;
    }
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_empty_scalar_map_ok() -> i32 {
    #[allow(dead_code)]
    #[derive(Debug, Deserialize)]
    struct Foo {
        foo: HashMap<String, usize>,
    }
    i32::from(from_yaml::<Foo>(0, "foo:").is_ok())
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_empty_scalar_seq_ok() -> i32 {
    #[allow(dead_code)]
    #[derive(Debug, Deserialize)]
    struct Foo {
        foo: Vec<String>,
    }
    i32::from(from_yaml::<Foo>(0, "foo:").is_ok())
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_empty_scalar_unit_ok() -> i32 {
    #[allow(dead_code)]
    #[derive(Debug, Deserialize)]
    struct Foo {
        foo: (),
    }
    i32::from(from_yaml::<Foo>(0, "foo:").is_ok())
}

#[unsafe(no_mangle)]
pub extern "C" fn marked_yaml_serde_empty_scalar_struct_with_default_ok() -> i32 {
    #[allow(dead_code)]
    #[derive(Default, Debug, Deserialize)]
    struct Bar {
        buz: Option<String>,
    }

    #[allow(dead_code)]
    #[derive(Debug, Deserialize)]
    struct Foo {
        #[serde(default)]
        bar: Bar,
    }
    i32::from(from_yaml::<Foo>(0, "bar:").is_ok())
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn marked_yaml_validate_utf8(input_ptr: *const u8, input_len: i64) -> i32 {
    let Some(input) = (unsafe { input_str(input_ptr, input_len) }) else {
        return -1;
    };
    i32::from(validate_yaml(input))
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn marked_yaml_echo_utf8_len(input_ptr: *const u8, input_len: i64) -> i64 {
    let Some(input) = (unsafe { input_str(input_ptr, input_len) }) else {
        return -1;
    };
    if validate_yaml(input) {
        input_len
    } else {
        -2
    }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn marked_yaml_echo_utf8_copy(
    input_ptr: *const u8,
    input_len: i64,
    destination_ptr: *mut u8,
    destination_capacity: i64,
) -> i64 {
    let Some(input) = (unsafe { input_str(input_ptr, input_len) }) else {
        return -1;
    };
    if !validate_yaml(input) {
        return -2;
    }
    unsafe { copy_input(input_ptr, input_len, destination_ptr, destination_capacity) }
}

fn character_span() -> Option<(usize, usize)> {
    let document = match parse_yaml(0, CHARACTER_DOC) {
        Ok(document) => document,
        Err(_) => return None,
    };
    let mapping = match document.as_mapping() {
        Some(mapping) => mapping,
        None => return None,
    };
    let node = match mapping.get_scalar("key") {
        Some(node) => node,
        None => return None,
    };
    let span = node.span();
    let start = match span.start() {
        Some(marker) => marker.character(),
        None => return None,
    };
    let end = match span.end() {
        Some(marker) => marker.character(),
        None => start + node.as_str().chars().count(),
    };
    forget(document);
    Some((start, end))
}

unsafe fn input_str<'a>(input_ptr: *const u8, input_len: i64) -> Option<&'a str> {
    if input_len < 0 || (input_len > 0 && input_ptr.is_null()) {
        return None;
    }
    let bytes = unsafe { std::slice::from_raw_parts(input_ptr, input_len as usize) };
    str::from_utf8(bytes).ok()
}

fn validate_yaml(input: &str) -> bool {
    match parse_yaml(0, input) {
        Ok(_) => true,
        Err(LoadError::TopLevelMustBeMapping(_)) => {
            match parse_yaml_with_options(0, input, LoaderOptions::default().toplevel_sequence()) {
                Ok(_) | Err(LoadError::TopLevelMustBeMapping(_)) | Err(LoadError::TopLevelMustBeSequence(_)) => true,
                Err(_) => false,
            }
        }
        Err(_) => false,
    }
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

fn character_rewrite_hash(start: usize, end: usize) -> u64 {
    let mut hash = fnv_offset();
    for ch in CHARACTER_DOC.chars().take(start) {
        hash = hash_char(hash, ch);
    }
    for ch in "new_value".chars() {
        hash = hash_char(hash, ch);
    }
    for ch in CHARACTER_DOC.chars().skip(end) {
        hash = hash_char(hash, ch);
    }
    hash
}

fn hash_chars(chars: impl Iterator<Item = char>) -> u64 {
    let mut hash = fnv_offset();
    for ch in chars {
        hash = hash_char(hash, ch);
    }
    hash
}

fn fnv_offset() -> u64 {
    0xcbf29ce484222325
}

fn hash_char(mut hash: u64, ch: char) -> u64 {
    let value = ch as u32;
    for shift in [0, 8, 16, 24] {
        hash ^= ((value >> shift) & 0xff) as u64;
        hash = hash.wrapping_mul(0x100000001b3);
    }
    hash
}
