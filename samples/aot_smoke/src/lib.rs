// SPDX-License-Identifier: MIT
//
// Tiny Rust library translated to managed IL by Rustlyn, then consumed by
// the AOT smoke host. Keep the surface small so the published binary stays
// minimal and the assertion in Test-AotSmoke.ps1 is easy to maintain.

#[no_mangle]
pub extern "C" fn aot_add(a: i32, b: i32) -> i32 {
    a + b
}

#[no_mangle]
pub extern "C" fn aot_meaning() -> i32 {
    42
}
