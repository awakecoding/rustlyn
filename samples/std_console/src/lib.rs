use std::io::Write;

/// Exercises stdout, stderr, and formatting. Returns 0 on success, negative on failure.
#[unsafe(no_mangle)]
pub extern "C" fn std_console_probe() -> i32 {
    // println! to stdout
    println!("std_console: hello stdout");

    // eprintln! to stderr
    eprintln!("std_console: hello stderr");

    // format! and write!
    let formatted = format!("value={}", 42);
    if formatted != "value=42" {
        return -1;
    }

    // Direct stdout write
    let stdout_result = std::io::stdout().write_all(b"std_console: direct write\n");
    if stdout_result.is_err() {
        return -2;
    }

    // Direct stderr write
    let stderr_result = std::io::stderr().write_all(b"std_console: direct stderr\n");
    if stderr_result.is_err() {
        return -3;
    }

    // Flush stdout
    if std::io::stdout().flush().is_err() {
        return -4;
    }

    // format! with multiple arguments
    let multi = format!("{} + {} = {}", 1, 2, 3);
    if multi != "1 + 2 = 3" {
        return -5;
    }

    0
}

/// Exercises println! with a value that cannot be folded into a literal fmt string.
#[unsafe(no_mangle)]
pub extern "C" fn std_console_runtime_value_probe(value: i32) -> i32 {
    println!("std_console: runtime={} doubled={}", value, value * 2);
    0
}
