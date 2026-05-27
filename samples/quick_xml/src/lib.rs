use quick_xml::{
    Reader, Writer,
    escape::unescape,
    events::{BytesEnd, BytesStart, BytesText, Event},
};
use serde::{Deserialize, Serialize};

const READER_DOC: &str = r#"<?xml version="1.0"?><root><item id="a">one</item><empty flag="yes"/><!--note--><![CDATA[two]]></root>"#;

#[derive(Debug, Deserialize)]
#[allow(dead_code)]
struct SerdePerson {
    active: bool,
}

#[derive(Debug, Serialize)]
#[serde(rename = "person")]
struct SerdePersonOut<'a> {
    active: bool,
    #[serde(skip)]
    _marker: core::marker::PhantomData<&'a ()>,
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_reader_events_score() -> i32 {
    let mut reader = Reader::from_reader(READER_DOC.as_bytes());
    reader.config_mut().trim_text(true);
    let mut buf = Vec::new();
    let mut score = 0;

    loop {
        match reader.read_event_into(&mut buf) {
            Ok(Event::Decl(_)) => score += 1,
            Ok(Event::Start(event)) if event.name().as_ref() == b"root" => score += 2,
            Ok(Event::Start(event)) if event.name().as_ref() == b"item" => score += 4,
            Ok(Event::Text(_)) => score += 8,
            Ok(Event::End(event)) if event.name().as_ref() == b"item" => score += 16,
            Ok(Event::Empty(_)) => score += 32,
            Ok(Event::Comment(event)) if event.as_ref() == b"note" => score += 64,
            Ok(Event::CData(_)) => score += 128,
            Ok(Event::End(event)) if event.name().as_ref() == b"root" => score += 256,
            Ok(Event::Eof) => break,
            Ok(_) => {}
            Err(_) => return -1,
        }
        buf.clear();
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_reader_attributes_score() -> i32 {
    let mut reader = Reader::from_reader(br#"<item id="42" kind="blue" bare='ok'/>"#.as_ref());
    let mut buf = Vec::new();

    match reader.read_event_into(&mut buf) {
        Ok(Event::Empty(event)) => {
            let mut score = 0;
            for attribute in event.attributes().with_checks(true) {
                let Ok(attribute) = attribute else {
                    return -1;
                };
                match (attribute.key.as_ref(), attribute.value.as_ref()) {
                    (b"id", b"42") => score += 1,
                    (b"kind", b"blue") => score += 2,
                    (b"bare", b"ok") => score += 4,
                    _ => {}
                }
            }
            score
        }
        Ok(_) => -2,
        Err(_) => -3,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_reader_errors_score() -> i32 {
    let mut score = 0;
    if read_until_error("<root attr='unterminated></root>") {
        score += 1;
    }
    if read_until_error("<root><item></root>") {
        score += 2;
    }
    if !read_until_error("<root><item/></root>") {
        score += 4;
    }
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_escape_html_score() -> i32 {
    let mut score = 0;

    if unescape("Tom &amp; Jerry &lt; 3").as_deref() == Ok("Tom & Jerry < 3") {
        score += 1;
    }
    if unescape("&copy;&nbsp;").as_deref() == Ok("\u{a9}\u{a0}") {
        score += 2;
    }
    if unescape("plain text").as_deref() == Ok("plain text") {
        score += 4;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_writer_roundtrip_score() -> i32 {
    let xml = match write_sample_xml() {
        Ok(xml) => xml,
        Err(_) => return -1,
    };

    let mut score = 0;
    if xml == r#"<root><item id="42">value</item><empty/></root>"# {
        score += 1;
    }
    if roundtrip_written_xml(&xml) & 7 == 7 {
        score += 2;
    }
    score
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_encoding_detection_score() -> i32 {
    let mut score = 0;

    if detected_encoding_is_ascii_compatible(
        br#"<?xml version="1.0" encoding="ISO-8859-1"?><root/>"#,
    ) {
        score += 1;
    }
    if detected_encoding_is_ascii_compatible(br#"<?xml version="1.0" encoding="UTF-8"?><root/>"#) {
        score += 2;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_serde_deserialize_score() -> i32 {
    let xml = r#"<person><active>true</active></person>"#;
    let person: SerdePerson = match quick_xml::de::from_str(xml) {
        Ok(person) => person,
        Err(_) => return -1,
    };

    let mut score = 0;
    if person.active {
        score += 1;
    }

    score
}

#[unsafe(no_mangle)]
pub extern "C" fn quick_xml_serde_serialize_score() -> i32 {
    let person = SerdePersonOut {
        active: true,
        _marker: core::marker::PhantomData,
    };
    let xml = match quick_xml::se::to_string(&person) {
        Ok(xml) => xml,
        Err(_) => return -1,
    };

    let mut score = 0;
    if xml.starts_with("<person>") {
        score += 1;
    }
    if xml.contains("<active>true</active>") {
        score += 2;
    }

    score
}

fn read_until_error(input: &str) -> bool {
    let mut reader = Reader::from_reader(input.as_bytes());
    let mut buf = Vec::new();

    loop {
        match reader.read_event_into(&mut buf) {
            Ok(Event::Eof) => return false,
            Ok(_) => buf.clear(),
            Err(_) => return true,
        }
    }
}

fn write_sample_xml() -> Result<String, ()> {
    let mut writer = Writer::new(Vec::new());
    writer
        .write_event(Event::Start(BytesStart::new("root")))
        .map_err(|_| ())?;

    let mut item = BytesStart::new("item");
    item.push_attribute(("id", "42"));
    writer.write_event(Event::Start(item)).map_err(|_| ())?;
    writer
        .write_event(Event::Text(BytesText::new("value")))
        .map_err(|_| ())?;
    writer
        .write_event(Event::End(BytesEnd::new("item")))
        .map_err(|_| ())?;
    writer
        .write_event(Event::Empty(BytesStart::new("empty")))
        .map_err(|_| ())?;
    writer
        .write_event(Event::End(BytesEnd::new("root")))
        .map_err(|_| ())?;

    String::from_utf8(writer.into_inner()).map_err(|_| ())
}

fn roundtrip_written_xml(xml: &str) -> i32 {
    let mut reader = Reader::from_reader(xml.as_bytes());
    let mut buf = Vec::new();
    let mut score = 0;

    loop {
        match reader.read_event_into(&mut buf) {
            Ok(Event::Start(event)) if event.name().as_ref() == b"root" => score += 1,
            Ok(Event::Start(event)) if event.name().as_ref() == b"item" => score += 2,
            Ok(Event::Text(_)) => score += 4,
            Ok(Event::Empty(event)) if event.name().as_ref() == b"empty" => score += 8,
            Ok(Event::Eof) => break,
            Ok(_) => {}
            Err(_) => return -1,
        }
        buf.clear();
    }

    score
}

fn detected_encoding_is_ascii_compatible(input: &[u8]) -> bool {
    let mut reader = Reader::from_reader(input);
    let mut buf = Vec::new();
    let _ = reader.read_event_into(&mut buf);
    reader.decoder().encoding().is_ascii_compatible()
}
