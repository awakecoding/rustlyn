#![no_main]

mod avalonia;

unsafe extern "C" {
    fn rustlyn_avalonia_run_app() -> i32;
}

#[unsafe(no_mangle)]
pub extern "C" fn main() -> i32 {
    unsafe { rustlyn_avalonia_run_app() }
}

#[unsafe(no_mangle)]
pub extern "C" fn avalonia_build_ui() -> i32 {
    let window = expect(avalonia::Window::new());
    let stack = expect(avalonia::StackPanel::new());
    let text = expect(avalonia::TextBlock::new());
    let button = expect(avalonia::Button::new());

    let title = b"rustlyn Avalonia Hello";
    expect(window.set_title_utf8_parts(title.as_ptr(), title.len() as i64));
    expect(window.set_size(640.0, 360.0));
    let margin = expect(avalonia::Thickness::new(24.0));
    expect(stack.set_margin(&margin));
    expect(stack.set_spacing(12.0));

    let initial_text = b"hello world";
    expect(text.set_text_utf8_parts(initial_text.as_ptr(), initial_text.len() as i64));
    let button_text = b"Click me";
    expect(button.set_content_utf8_parts(button_text.as_ptr(), button_text.len() as i64));
    let _subscription = expect(button.subscribe_click(1, &text));

    expect(stack.add_child(&text));
    expect(stack.add_child(&button));
    expect(window.set_content(&stack));

    window.into_handle()
}

#[unsafe(no_mangle)]
pub extern "C" fn avalonia_on_click(handler_id: i32, state_handle: i32) {
    if handler_id == 1 {
        let clicked_text = b"hello world from Rust click";
        let text = unsafe { avalonia::TextBlock::from_borrowed_handle(state_handle) };
        let _ = text.set_text_utf8_parts(clicked_text.as_ptr(), clicked_text.len() as i64);
    }
}

fn expect<T>(result: Result<T, avalonia::Exception>) -> T {
    match result {
        Ok(value) => value,
        Err(_) => unsafe { core::hint::unreachable_unchecked() },
    }
}
