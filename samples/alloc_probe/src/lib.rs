#[unsafe(no_mangle)]
pub extern "C" fn alloc_string_len() -> i32 {
    let mut segments = Vec::with_capacity(3);
    segments.push(String::from("managed"));
    segments.push(String::from("-"));
    segments.push(String::from("rust"));

    segments.concat().len() as i32
}