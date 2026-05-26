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

## Optimization passes

`rustlyn-llvm opt` runs LLVM's new pass manager (`LLVMRunPasses`) over a bitcode module. The default pipeline is intentionally narrow and aimed at debug bitcode:

```powershell
rustlyn-llvm opt input.bc --output input.opt.bc
# default --passes mem2reg,sroa,simplifycfg

# Stronger debug-friendly canonicalization:
rustlyn-llvm opt input.bc --passes "mem2reg,sroa,early-cse,instcombine,simplifycfg" --output input.opt.bc

# Inspect the optimized IR without writing a file:
rustlyn-llvm opt input.bc --emit-llvm-ir --output -
```

The helper also accepts `--verify-each` to enable LLVM's per-pass verifier, and `--disable-verify` to skip the pre/post `LLVMVerifyModule` check.

### What it is for

- Best wins on debug bitcode (`-C opt-level=0`), where allocas dominate and `mem2reg`/`sroa` give the Rustlyn lowerer much cleaner IR.
- Useful canonicalization (`simplifycfg`, `instcombine`, `early-cse`) for lowering experiments.

### What it is not for

- Release bitcode is already optimized by rustc; re-running heavy pipelines has limited upside and risks hiding patterns the lowerer relies on.
- Optimization cannot recover Rust-level information (drop glue, niche layout, MIR semantics) — that is sidecar territory, not opt territory.
- Don't default to `default<O2>`/`default<O3>` for Rustlyn; pick named, reviewed pipelines.

### Driving opt from Rustlyn

Setting `RUSTLYN_LLVM_OPT_PASSES` makes `LoweredIrLowerer.LowerBitcode` run the helper as an opt pre-pass before reading the module:

```powershell
$env:RUSTLYN_LLVM_OPT_PASSES = 'mem2reg,sroa,simplifycfg'
dotnet run --project .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -- invoke .\artifacts\out\add\add.bc --method add_i32 --arg i32:2 --arg i32:3
```

`Rustlyn.Tool diagnose` reports the active pipeline under `llvm-opt-passes`. When the env var is unset, lowering reads the original bitcode unchanged. When `rustlyn-llvm.exe` is not available the pre-pass is silently skipped — the env var is a best-effort canonicalization hint, not a hard dependency.

### LLVM version skew (important)

`rustlyn-llvm opt` requires the helper's bundled LLVM to be **>= rustc's bundled LLVM**. Recent rustc versions (1.95+) ship LLVM 22; if the helper is built against LLVM 20, opt will fail with errors like:

```
error: Unknown attribute kind (105) (Producer: 'LLVM22.1.2-rust-1.95.0-stable' Reader: 'LLVM 20.1.8')
immarg operand has non-immediate parameter
  call void @llvm.lifetime.start.p0(ptr nonnull %r)
```

These are not Rustlyn bugs — newer rustc bitcode embeds attributes and intrinsic signatures that the older LLVM C API cannot parse. The lowerer itself can still read these modules via its text path; only the opt pre-pass fails.

To keep the env var on across mixed-LLVM environments, set `RUSTLYN_LLVM_OPT_BEST_EFFORT=1`. Rustlyn will then emit a warning when opt fails and continue lowering the original unoptimized bitcode instead of aborting.

### Determinism caveats

- Pin the helper + LLVM version: optimized output is reproducible only for the exact LLVM version that produced it.
- The datalayout must remain stable between opt and lowering; the helper does not change triples.
- `mem2reg`/`sroa` preserve `!dbg` metadata; avoid pipelines containing `strip-debug` if debug info matters.

