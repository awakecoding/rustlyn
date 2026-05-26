use saphyr::{LoadableYamlNode, Scalar, Yaml};

const SAMPLE: &str = r#"
name: rustlyn
enabled: true
numbers: [2, 3, 5]
nested:
  answer: 42
"#;

fn score_yaml(value: &Yaml<'_>) -> i32 {
    match value {
        Yaml::Representation(raw, _, _) => raw.len() as i32,
        Yaml::Value(Scalar::Null) => 0,
        Yaml::Value(Scalar::Boolean(value)) => i32::from(*value),
        Yaml::Value(Scalar::Integer(value)) => *value as i32,
        Yaml::Value(Scalar::FloatingPoint(value)) => value.into_inner() as i32,
        Yaml::Value(Scalar::String(value)) => value.len() as i32,
        Yaml::Sequence(values) => 100 + values.len() as i32 + values.iter().map(score_yaml).sum::<i32>(),
        Yaml::Mapping(values) => {
            200 + values.len() as i32
                + values
                    .iter()
                    .map(|(key, value)| score_yaml(key) + score_yaml(value))
                    .sum::<i32>()
        }
        Yaml::Tagged(_, value) => 300 + score_yaml(value),
        Yaml::Alias(value) => *value as i32,
        Yaml::BadValue => -1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_score() -> i32 {
    let documents = match Yaml::load_from_str(SAMPLE) {
        Ok(value) => value,
        Err(_) => return -1000,
    };

    if documents.len() != 1 {
        return -1001;
    }

    score_yaml(&documents[0])
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_error_code() -> i32 {
    match Yaml::load_from_str(SAMPLE) {
        Ok(_) => 0,
        Err(error) => match error.info() {
            "while parsing a block mapping, did not find expected key" => 1,
            "while parsing a block collection, did not find expected '-' indicator" => 2,
            "while scanning a plain scalar, found a tab" => 3,
            "unexpected end of plain scalar" => 4,
            "mapping values are not allowed in this context" => 5,
            "did not find expected next token" => 6,
            "did not find expected <document-start>" => 7,
            "while parsing a node, did not find expected node content" => 8,
            _ => -1,
        },
    }
}

fn parse_probe(input: &str) -> i32 {
    match Yaml::load_from_str(input) {
        Ok(documents) => documents.len() as i32,
        Err(_) => -1,
    }
}

fn parse_probe_err_code(input: &str) -> i32 {
    match Yaml::load_from_str(input) {
        Ok(documents) => documents.len() as i32,
        Err(error) => {
            let info = error.info();
            if info.contains("expected key") { -10 }
            else if info.contains("expected '-' indicator") { -11 }
            else if info.contains("tab") { -12 }
            else if info.contains("end of plain scalar") { -13 }
            else if info.contains("not allowed") { -14 }
            else if info.contains("next token") { -15 }
            else if info.contains("document-start") { -16 }
            else if info.contains("node content") { -17 }
            else if info.contains("flow") { -18 }
            else if info.contains("block") { -19 }
            else { -1 }
        },
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_empty() -> i32 {
    parse_probe("")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_scalar() -> i32 {
    parse_probe("rustlyn\n")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_mapping() -> i32 {
    parse_probe("name: rustlyn\n")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_sequence() -> i32 {
    parse_probe("- a\n- b\n")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_two_scalars() -> i32 {
    parse_probe("---\nhello\n---\nworld\n")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_quoted() -> i32 {
    parse_probe("\"hello\"\n")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_single_seq() -> i32 {
    parse_probe_err_code("- a\n")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_just_a() -> i32 {
    parse_probe("a")
}

#[unsafe(no_mangle)]
pub extern "C" fn yaml_saphyr_probe_multiline() -> i32 {
    parse_probe_err_code("a: b\n")
}
