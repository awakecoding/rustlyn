# rustlyn

Rustlyn translates Rust-produced LLVM bitcode into managed .NET assemblies — like [Roslyn](https://github.com/dotnet/roslyn) is the .NET compiler platform, Rustlyn aims to be the Rust-to-.NET compiler platform.

Heavily inspired by Eric Sink's [SourceGear Rust.NET SDK](https://ericsink.com/entries/sg_rust_dotnet_preview.html) (2020–2021), which proved that LLVM-to-CIL translation is viable. Rustlyn reconstructs and modernizes that approach on .NET 10, current Rust toolchains, and LLVM 20 — then goes beyond what the original experiment demonstrated.

## Quick Start

### 1. Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A Rust toolchain via [rustup](https://rustup.rs/) (nightly is used for `build-std` flows)
- An LLVM 20 toolchain. The easiest path is the bundled prebuilt:

  ```powershell
  .\scripts\Get-LlvmPrebuilt.ps1
  ```

  This downloads `clang+llvm-20.1.8` into the repo and sets `RUSTLYN_LLVM_ROOT` for the current session. Alternatively, build the statically linked helper with `.\scripts\Build-RustlynLlvmHelper.ps1 -LlvmDevRoot <path>`, or point `RUSTLYN_LLVM_ROOT` at any directory containing `bin\rustlyn-llvm.exe` (or `bin\llvm-opt.exe` as a legacy fallback).

### 2. Build the `rustlyn` CLI

```powershell
dotnet build .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -c Release
$env:Path = "$PWD\dotnet\backend\src\Rustlyn.Tool\bin\Release\net10.0;$env:Path"
rustlyn --version
rustlyn --help
```

### 3. Translate a Rust crate to a .NET assembly

The fastest "voilà, a managed DLL" path uses the cargo wrapper. From any Cargo crate directory:

```powershell
rustlyn new hello_rustlyn   # scaffold a starter crate
cd hello_rustlyn
rustlyn cargo build         # produces target\debug\hello_rustlyn.{bc,ll,dll,pdb}
rustlyn run                 # cargo build (managed) + dotnet exec on the .dll
```

`rustlyn cargo` is a transparent `cargo` wrapper: it forwards every flag to `cargo` and then emits the bitcode, IL text, managed assembly, and PDB alongside Cargo's regular `target\debug\` (or `target\release\`) output. Run it from a crate directory or pass `--manifest-path` like you would with `cargo`.

### 4. Verify with a sample

```powershell
.\scripts\Build-SampleBitcode.ps1 -Sample add
.\scripts\Test-Smoke.ps1 -Sample add -Mode Bitcode
.\scripts\Test-Smoke.ps1 -Sample add -Mode Cargo
```

## CLI reference

```text
rustlyn new <name>                          scaffold a starter crate
rustlyn cargo <cargo-args>                  cargo wrapper that also emits .bc/.ll/.dll/.pdb
rustlyn rustc <rustc-args>                  rustc wrapper that emits managed output
rustlyn run [--release]                     cargo build + dotnet exec the resulting assembly
rustlyn translate <crate-dir> --out <dll>   crate -> managed assembly
rustlyn pack <crate-dir> --out <dir>        preview: emit translated crate package artifacts
rustlyn emit <bitcode> --out <dll> [--pdb]  bitcode -> managed assembly
rustlyn invoke <bitcode> --method <name> [--arg <type>:<value>]...
                                            JIT a function out of bitcode and print its result
rustlyn inspect <bitcode>                   list functions, globals, metadata
rustlyn lower <bitcode>                     dump the lowered IR Rustlyn produces from LLVM
rustlyn llvm <subcommand>                   helper bridge to the bundled LLVM tools
rustlyn --help | --version
```

Examples driving the backend directly:

```powershell
rustlyn inspect .\artifacts\out\add\add.bc
rustlyn lower   .\artifacts\out\add\add.bc
rustlyn emit    .\artifacts\out\add\add.bc --out .\artifacts\out\add\add.generated.dll --pdb
rustlyn invoke  .\samples\add\target\debug\add.bc --method add_i32 --arg i32:19 --arg i32:23
rustlyn translate .\samples\add --out .\artifacts\out\add\add.dll --bitcode-out .\artifacts\out\add\add.bc
rustlyn translate .\samples\add --out .\artifacts\out\add\add.dll --cache .\artifacts\scratch\.cache.json
rustlyn pack    .\samples\add --out .\artifacts\out\add --version 1.0.0
```

## Going further

Run a generated-bindings workload that calls .NET APIs from Rust:

```powershell
.\scripts\Test-Smoke.ps1 -Sample lousygrep -Mode Cargo -Configuration Release
```

Try the Rust-driven Avalonia desktop fixture:

```powershell
.\scripts\Test-Smoke.ps1 -Sample avalonia_hello -Mode Cargo
rustlyn translate .\samples\avalonia_hello --out .\artifacts\scratch\avalonia_hello.dll --bitcode-out .\artifacts\scratch\avalonia_hello.bc --bin avalonia_hello
dotnet .\artifacts\scratch\avalonia_hello.dll --smoke
dotnet .\artifacts\scratch\avalonia_hello.dll
```

`samples/avalonia_hello` is a Rust-composed UI bridge proof. Rust creates the window, stack panel, text block, and button through backend-recognized Avalonia bridge calls; the support assembly owns the real Avalonia objects, application lifetime, object handles, and click callback dispatch back into generated Rust. The smoke run prints `avalonia:rust-ui:ok`. This is still a narrow bridge fixture, not generated Avalonia bindings, XAML translation, or data binding.

Build `.rsproj` projects with `dotnet build` via the preview MSBuild SDK:

```powershell
.\scripts\Test-MsBuildSdk.ps1          -Configuration Release
.\scripts\Test-MsBuildSdkBinary.ps1    -Configuration Release
.\scripts\Test-MsBuildSdkBuildStd.ps1  -Configuration Release
.\scripts\Test-MsBuildSdkPackage.ps1   -Configuration Release
```

These cover SourceGear-style project metadata (`Language=Rust`, `RustToolchain=+nightly`, `RustDebugOrRelease`), synthesized Cargo manifests with both local `RustReference` and crates.io `RustCrateReference` dependencies, `RustlynBinaryTarget` / inferred binary targets, `RustlynBuildStd=core`, and NuGet-style SDK resolution including the packaged `rustlyn` tool under `tools/net10.0`.

Run the regression harness (18,000+ lines of behavioral assertions):

```powershell
dotnet run -c Release --project .\dotnet\backend\tests\Rustlyn.Backend.Tests\Rustlyn.Backend.Tests.csproj
```

## What works today

- Translates Rust crates to runnable .NET assemblies via LLVM bitcode
- Backend covers arithmetic, control flow, structs, closures, trait objects, atomics, cross-crate LTO
- Focused samples for complex enums, iterators, `?` error propagation, async-like state machines, generic collections
- Generates Rust bindings for a curated .NET API surface (Console, File, Path, String, Environment)
- Metadata-driven binding scanner/generator foothold for broader .NET APIs
- Emits Portable PDB files (real Rust source mapping is roadmap work)
- Preview `rustlyn pack` flow emits translated assemblies and package metadata
- Preview MSBuild SDK for `dotnet build` on `.rsproj` files
- Avalonia desktop bridge via an explicit fixture
- Cross-platform CI: Windows, Linux, macOS

See the [support matrix](docs/support-matrix.md) for supported, preview, fixture-only, planned, and unsupported areas. See the [roadmap](docs/roadmap.md) for what's next.

## Documentation

- [Original Eric Sink design](docs/original-eric-sink-design.md)
- [Revived design in this repo](docs/revived-design.md)
- [SourceGear parity roadmap](docs/sourcegear-parity-roadmap.md)
- [SourceGear fake-link decision](docs/sourcegear-fake-link-decision.md)
- [Support matrix](docs/support-matrix.md)
- [Deep reconstruction notes](docs/reconstruction-notes.md)
- [Contributor and agent workflow](AGENTS.md)

## Repo Map

- `samples/`: narrow Rust crates used as regression fixtures
- `samples/enum_complex/`: multi-field enum variants, nested Option/Result patterns
- `samples/iterator_chain/`: map/filter/sum, zip, chain/take, flat_map adapter chains
- `samples/error_propagation/`: `?` operator with multi-step validation
- `samples/string_vec_ops/`: `Vec<T>` operations with no_std + alloc
- `samples/async_state_machine/`: poll coroutines, cooperative scheduler, retry-with-backoff
- `samples/generic_collections/`: monomorphized FixedStack, RingBuffer, Pair, generic search
- `samples/generated_bindings_hello/`: first generated-style .NET binding fixture over console, environment method/property, directory, path, and string method/property APIs
- `samples/generated_bindings_lousygrep/`: canonical lousygrep-style fixture using generated Environment/File/String/Console bindings for the workload
- `scripts/`: repeatable PowerShell entry points for LLVM setup, sample builds, and smoke checks
- `dotnet/backend/src/Rustlyn.Tool/`: CLI for cargo, llvm, inspect, lower, emit, invoke, translate, and pack flows
- `dotnet/backend/src/Rustlyn.Backend/`: lowering, IL emission, Portable PDB, translation cache, NuGet packaging
- `dotnet/backend/src/Rustlyn.Bindings/`: binding generation — assembly scanner, instance/constructor/generic/delegate/event analysis
- `dotnet/backend/src/Rustlyn.Bindings.Tool/`: CLI for scan, bindgen, analyze-delegate, analyze-events commands
- `dotnet/backend/src/Rustlyn.Interop/`: reusable managed object/exception handles and UTF-8 interop helpers for future generated bindings
- `dotnet/backend/src/Rustlyn.Runtime/`: future home for LLVM/runtime semantic helpers
- `dotnet/backend/src/Rustlyn.Os/`: future home for host OS and Rust `std` compatibility helpers
- `dotnet/backend/src/Rustlyn.Sdk/`: local and packable SDK-style MSBuild facade that delegates `.rsproj` builds to the backend translate driver
- `dotnet/backend/tests/Rustlyn.Backend.Tests/`: focused regression harness for lowering and runtime behavior
- `dotnet/backend/benchmarks/Rustlyn.Benchmarks/`: BenchmarkDotNet suite for translation performance
- `.github/workflows/ci.yml`: cross-platform CI (Windows, Linux, macOS)

Historical SourceGear package details are summarized in `docs/` and linked to public package/blog sources; extracted package trees and decompiled payloads are intentionally not kept as repo content.

## Current Direction

The revived experiment is cargo-first and backend-first. The repository is deliberately optimized for bringing up one backend slice at a time with executable validation, rather than for presenting a polished `.rsproj` SDK shell.
