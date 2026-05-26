# Rustlyn LLVM helper

Rustlyn's preferred LLVM boundary is the Rustlyn-owned native helper named `rustlyn-llvm`; generic `llvm-opt.exe` is now treated as a legacy fallback.

The helper is built in Rust and statically links LLVM from a development subset. It is not a C# intermediate and does not change Rustlyn's backend model: Rustlyn still emits CIL/ECMA-335 metadata directly.

## Runtime package

The runtime tool package can be small:

```text
<RUSTLYN_LLVM_ROOT>\
  bin\
    rustlyn-llvm.exe
  VERSION.txt
  LICENSES\
```

For local development, `RUSTLYN_LLVM_ROOT` may also point directly at the directory containing `rustlyn-llvm.exe`.

Legacy roots containing `llvm-opt.exe` or `opt.exe` are still supported as fallbacks, but they cannot provide structured JSON.

## Build-time package

Building the helper requires an LLVM development root:

```text
<RUSTLYN_LLVM_DEV_ROOT>\
  bin\
    llvm-config.exe
  include\
    llvm\
    llvm-c\
  lib\
    LLVM*.lib
```

Build with:

```powershell
.\scripts\Build-RustlynLlvmHelper.ps1 -LlvmDevRoot D:\opt\llvm-subsets\llvm-dev
```

Smoke-test with:

```powershell
.\scripts\Test-LlvmHelper.ps1 -LlvmDevRoot D:\opt\llvm-subsets\llvm-dev -Sample add
```

## Current command contract

The helper's text command is equivalent to Rustlyn's old `llvm-opt` usage:

```powershell
rustlyn-llvm print-ir input.bc --disable-verify --output -
```

It also accepts the current compatibility shape:

```powershell
rustlyn-llvm -disable-verify -S input.bc -o -
```

The module-summary structured command is:

```powershell
rustlyn-llvm inspect-json input.bc --disable-verify
```

Rustlyn prefers this command for module summaries when `rustlyn-llvm` is present.

The first lowered-structure command is:

```powershell
rustlyn-llvm lower-json input.bc --disable-verify
```

It currently emits a conservative schema with globals, functions, parameters, blocks, instruction opcodes, printed instruction text, result names, and operands. Rustlyn can parse this schema into a C# model and use it for simple global-free modules. The `RUSTLYN_LLVM_READER` mode controls the lowerer:

- `auto` or unset: try structured helper lowering for supported simple modules, then fall back to text.
- `json`: require `rustlyn-llvm lower-json`.
- `text`: force the legacy textual IR path.
