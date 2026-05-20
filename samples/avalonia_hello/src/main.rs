#![no_main]

unsafe extern "C" {
    fn rust_mcil_avalonia_run_app() -> i32;
    fn rust_mcil_avalonia_window_new() -> i32;
    fn rust_mcil_avalonia_stack_panel_new() -> i32;
    fn rust_mcil_avalonia_text_block_new() -> i32;
    fn rust_mcil_avalonia_button_new() -> i32;
    fn rust_mcil_avalonia_window_set_title_utf8(window: i32, title_ptr: *const u8, title_len: i64);
    fn rust_mcil_avalonia_window_set_size(window: i32, width: i32, height: i32);
    fn rust_mcil_avalonia_window_set_content(window: i32, content: i32);
    fn rust_mcil_avalonia_stack_panel_set_spacing(stack_panel: i32, spacing: i32);
    fn rust_mcil_avalonia_stack_panel_set_margin(stack_panel: i32, margin: i32);
    fn rust_mcil_avalonia_stack_panel_add_child(stack_panel: i32, child: i32);
    fn rust_mcil_avalonia_text_block_set_text_utf8(
        text_block: i32,
        text_ptr: *const u8,
        text_len: i64,
    );
    fn rust_mcil_avalonia_button_set_content_utf8(button: i32, content_ptr: *const u8, content_len: i64);
    fn rust_mcil_avalonia_button_set_on_click(button: i32, handler_id: i32, state_handle: i32);
}

#[unsafe(no_mangle)]
pub extern "C" fn main() -> i32 {
    unsafe { rust_mcil_avalonia_run_app() }
}

#[unsafe(no_mangle)]
pub extern "C" fn avalonia_build_ui() -> i32 {
    let window = unsafe { rust_mcil_avalonia_window_new() };
    let stack = unsafe { rust_mcil_avalonia_stack_panel_new() };
    let text = unsafe { rust_mcil_avalonia_text_block_new() };
    let button = unsafe { rust_mcil_avalonia_button_new() };

    let title = b"rust-msil Avalonia Hello";
    unsafe { rust_mcil_avalonia_window_set_title_utf8(window, title.as_ptr(), title.len() as i64) };
    unsafe { rust_mcil_avalonia_window_set_size(window, 640, 360) };
    unsafe { rust_mcil_avalonia_stack_panel_set_margin(stack, 24) };
    unsafe { rust_mcil_avalonia_stack_panel_set_spacing(stack, 12) };

    let initial_text = b"hello world";
    unsafe { rust_mcil_avalonia_text_block_set_text_utf8(text, initial_text.as_ptr(), initial_text.len() as i64) };
    let button_text = b"Click me";
    unsafe { rust_mcil_avalonia_button_set_content_utf8(button, button_text.as_ptr(), button_text.len() as i64) };
    unsafe { rust_mcil_avalonia_button_set_on_click(button, 1, text) };

    unsafe { rust_mcil_avalonia_stack_panel_add_child(stack, text) };
    unsafe { rust_mcil_avalonia_stack_panel_add_child(stack, button) };
    unsafe { rust_mcil_avalonia_window_set_content(window, stack) };

    window
}

#[unsafe(no_mangle)]
pub extern "C" fn avalonia_on_click(handler_id: i32, state_handle: i32) {
    if handler_id == 1 {
        let clicked_text = b"hello world from Rust click";
        unsafe {
            rust_mcil_avalonia_text_block_set_text_utf8(
                state_handle,
                clicked_text.as_ptr(),
                clicked_text.len() as i64,
            )
        };
    }
}
