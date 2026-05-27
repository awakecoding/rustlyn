use serde::{Deserialize, Serialize};
use simd_json::{
    Deserializer, Implementation, Node, OwnedValue, StaticNode, prelude::*, to_borrowed_value,
    to_owned_value,
};

const OBJECT_DOC: &str = r#"{"name":"rustlyn","active":true,"items":[1,2,3],"nested":{"flag":null}}"#;

#[derive(Debug, Deserialize)]
struct SerdePayload {
    name: String,
    count: i64,
    features: Vec<String>,
    flag: Option<bool>,
}

#[derive(Debug, Deserialize)]
struct SerdeEnvelope {
    success: bool,
    payload: SerdePayload,
}

#[derive(Debug, Deserialize, Serialize)]
struct SerdeOutput {
    active: bool,
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_runtime_detection_score() -> i32 {
    let mut score = 0;

    match Deserializer::algorithm() {
        Implementation::Native | Implementation::SSE42 | Implementation::AVX2 => score += 1,
        _ => return -1,
    }

    let mut tape_input = br#"{"runtime":[1,2,3]}"#.to_vec();
    if Deserializer::from_slice(&mut tape_input).is_ok() {
        score += 2;
    }

    let mut value_input = br#"{"runtime":true}"#.to_vec();
    if to_owned_value(&mut value_input).is_ok() {
        score += 4;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_tape_shape_score() -> i32 {
    let mut score = 0;

    if matches!(first_node("[]"), Some(Node::Array { len: 0, count: 0 })) {
        score += 1;
    }
    if matches!(first_node("[1]"), Some(Node::Array { len: 1, count: 1 })) {
        score += 2;
    }
    if nested_array_counts_match() {
        score += 4;
    }
    if object_tape_matches() {
        score += 8;
    }
    if escaped_object_key_matches() {
        score += 16;
    }
    if string_value_matches() {
        score += 32;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_owned_borrowed_score() -> i32 {
    let mut borrowed_input = OBJECT_DOC.as_bytes().to_vec();
    let mut owned_input = OBJECT_DOC.as_bytes().to_vec();

    let borrowed = match to_borrowed_value(&mut borrowed_input) {
        Ok(value) => value,
        Err(_) => return -1,
    };
    let owned = match to_owned_value(&mut owned_input) {
        Ok(value) => value,
        Err(_) => return -2,
    };

    let mut score = 0;

    if owned.get("name").and_then(ValueAsScalar::as_str) == Some("rustlyn") {
        score += 1;
    }
    if borrowed.get("name").and_then(ValueAsScalar::as_str) == Some("rustlyn") {
        score += 2;
    }
    if owned
        .get("items")
        .and_then(ValueAsArray::as_array)
        .is_some_and(|items| items.len() == 3)
        && borrowed
            .get("items")
            .and_then(ValueAsArray::as_array)
            .is_some_and(|items| items.len() == 3)
    {
        score += 4;
    }
    if owned.get("active").and_then(ValueAsScalar::as_bool) == Some(true)
        && borrowed.get("active").and_then(ValueAsScalar::as_bool) == Some(true)
    {
        score += 8;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_number_score() -> i32 {
    let mut score = 0;

    if parse_owned_scalar("0").and_then(|value| value.as_i64()) == Some(0) {
        score += 1;
    }
    if parse_owned_scalar("-1").and_then(|value| value.as_i64()) == Some(-1) {
        score += 2;
    }
    if parse_owned_scalar("42").and_then(|value| value.as_i64()) == Some(42)
    {
        score += 4;
    }
    if parse_owned_scalar("3.5")
        .and_then(|value| value.cast_f64())
        .is_some_and(|value| (value - 3.5).abs() < f64::EPSILON)
    {
        score += 8;
    }
    if parse_owned_scalar("[1.]").is_none() {
        score += 16;
    }
    if parse_owned_scalar("9007199254740991").and_then(|value| value.as_i64())
        == Some(9_007_199_254_740_991)
    {
        score += 32;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_string_unicode_score() -> i32 {
    let mut score = 0;

    if parse_owned_scalar(r#""""#).and_then(|value| value.as_str().map(str::len)) == Some(0) {
        score += 1;
    }
    if parse_owned_scalar(r#""quote: \" slash: \\""#).and_then(|value| {
        value
            .as_str()
            .map(|text| text == r#"quote: " slash: \"#)
    }) == Some(true)
    {
        score += 2;
    }
    if parse_owned_scalar(r#""\u2603""#).and_then(|value| {
        value
            .as_str()
            .map(|text| text.chars().next() == Some('\u{2603}'))
    }) == Some(true)
    {
        score += 4;
    }
    if parse_owned_scalar(r#""\uDDDD""#).is_none() {
        score += 8;
    }
    if parse_owned_scalar(r#""\u00e5""#)
        .and_then(|value| value.as_str().map(|text| text == "\u{00e5}"))
        == Some(true)
    {
        score += 16;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_error_score() -> i32 {
    let mut score = 0;

    if parse_owned_scalar("").is_none() {
        score += 1;
    }
    if parse_owned_scalar(r#"""#).is_none() {
        score += 2;
    }
    if parse_owned_scalar("[1,]").is_none() {
        score += 4;
    }
    if parse_owned_scalar("nul").is_none() {
        score += 8;
    }
    if parse_owned_scalar(r#"{"a":1"#).is_none() {
        score += 16;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_serde_deserialize_score() -> i32 {
    let mut input =
        br#"{"success":true,"payload":{"name":"rustlyn","count":3,"features":["yaml","xml","json"],"flag":null}}"#
            .to_vec();
    let envelope: SerdeEnvelope = match simd_json::serde::from_slice(&mut input) {
        Ok(value) => value,
        Err(_) => return -1,
    };

    let mut score = 0;
    if envelope.success {
        score += 1;
    }
    if envelope.payload.name == "rustlyn" {
        score += 2;
    }
    if envelope.payload.count == 3 {
        score += 4;
    }
    if envelope.payload.features.len() == 3 && envelope.payload.features[2] == "json" {
        score += 8;
    }
    if envelope.payload.flag.is_none() {
        score += 16;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn simd_json_serde_serialize_score() -> i32 {
    let mut score = 0;

    if simd_json::to_string(&true).as_deref() == Ok("true") {
        score += 1;
    }
    if simd_json::to_string(&false).as_deref() == Ok("false") {
        score += 2;
    }

    let output = SerdeOutput {
        active: true,
    };
    let encoded = match simd_json::to_string(&output) {
        Ok(value) => value,
        Err(_) => return -1,
    };

    if encoded == r#"{"active":true}"# {
        score += 4;
    }

    score
}

fn first_node(input: &str) -> Option<Node<'static>> {
    let mut input = input.as_bytes().to_vec();
    let tape = Deserializer::from_slice(&mut input).ok()?.into_tape();
    tape.0.first().copied().map(static_node)
}

fn nested_array_counts_match() -> bool {
    let mut input = b" [ 1 , [ 3 ] , 2 ]".to_vec();
    let tape = match Deserializer::from_slice(&mut input) {
        Ok(deserializer) => deserializer.into_tape(),
        Err(_) => return false,
    };

    matches!(tape.0.first(), Some(Node::Array { len: 3, count: 4 }))
        && matches!(tape.0.get(2), Some(Node::Array { len: 1, count: 1 }))
}

fn object_tape_matches() -> bool {
    let mut input = br#" { "hello": 1 , "b": 1 }"#.to_vec();
    let tape = match Deserializer::from_slice(&mut input) {
        Ok(deserializer) => deserializer.into_tape(),
        Err(_) => return false,
    };

    matches!(tape.0.first(), Some(Node::Object { len: 2, count: 4 }))
        && matches!(tape.0.get(1), Some(Node::String("hello")))
        && matches!(tape.0.get(2), Some(Node::Static(value)) if static_one(*value))
        && matches!(tape.0.get(3), Some(Node::String("b")))
        && matches!(tape.0.get(4), Some(Node::Static(value)) if static_one(*value))
}

fn escaped_object_key_matches() -> bool {
    let mut input = br#" { "hell\"o": 1 , "b": [ 1, 2, 3 ] }"#.to_vec();
    let tape = match Deserializer::from_slice(&mut input) {
        Ok(deserializer) => deserializer.into_tape(),
        Err(_) => return false,
    };

    matches!(tape.0.first(), Some(Node::Object { len: 2, count: 7 }))
        && matches!(tape.0.get(1), Some(Node::String("hell\"o")))
        && matches!(tape.0.get(4), Some(Node::Array { len: 3, count: 3 }))
}

fn string_value_matches() -> bool {
    let mut input = br#""{\"arg\":\"test\"}""#.to_vec();
    let tape = match Deserializer::from_slice(&mut input) {
        Ok(deserializer) => deserializer.into_tape(),
        Err(_) => return false,
    };

    matches!(tape.0.first(), Some(Node::String("{\"arg\":\"test\"}")))
}

fn parse_owned_scalar(input: &str) -> Option<OwnedValue> {
    let mut input = input.as_bytes().to_vec();
    to_owned_value(&mut input).ok()
}

fn static_node(node: Node<'_>) -> Node<'static> {
    match node {
        Node::String(value) => Node::String(Box::leak(value.to_owned().into_boxed_str())),
        Node::Object { len, count } => Node::Object { len, count },
        Node::Array { len, count } => Node::Array { len, count },
        Node::Static(value) => Node::Static(value),
    }
}

fn static_one(value: StaticNode) -> bool {
    matches!(value, StaticNode::I64(1) | StaticNode::U64(1))
}
