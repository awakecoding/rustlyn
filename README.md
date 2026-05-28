# rustlyn

Rustlyn translates Rust-produced LLVM bitcode into managed .NET assemblies — like [Roslyn](https://github.com/dotnet/roslyn) is the .NET compiler platform, Rustlyn aims to be the Rust-to-.NET compiler platform.

Heavily inspired by Eric Sink's [SourceGear Rust.NET SDK](https://ericsink.com/entries/sg_rust_dotnet_preview.html) (2020–2021), which proved that LLVM-to-CIL translation is viable. Rustlyn reconstructs and modernizes that approach on .NET 10, current Rust toolchains, and LLVM 20 — then goes beyond what the original experiment demonstrated.

**What it does today:**

- Translates Rust crates to runnable .NET assemblies via LLVM bitcode
- Handles a growing fixture-backed backend surface: arithmetic, control flow, structs, closures, trait objects, atomics, and cross-crate LTO
- Carries focused samples for complex enums, iterators, error propagation (`?`), async-like state machines, and generic collections
- Generates Rust bindings for a curated .NET API surface (Console, File, Path, String, Environment)
- Includes a metadata-driven binding scanner/generator foothold for broader .NET APIs
- Emits Portable PDB files today, with real Rust source mapping still tracked as production work
- Provides a preview `rustlyn pack` flow that emits translated assemblies and package metadata
- Tracks translation cache state with `--cache`; cache reuse is still roadmap work
- Provides a preview MSBuild SDK for `dotnet build` on `.rsproj` files
- Bridges to Avalonia through an explicit desktop GUI fixture
- Builds on Windows, Linux, and macOS in CI, with the required smoke matrix still intentionally small

See the [support matrix](docs/support-matrix.md) for supported, preview, fixture-only, planned, and unsupported areas. See the [roadmap](docs/roadmap.md) for what's next.

## What you can do here today

1. Build a Cargo crate into LLVM bitcode plus a managed assembly via `rustlyn cargo build`.
2. Translate a Rust crate into a managed .NET assembly via `rustlyn translate`.
3. Produce translated crate package artifacts via the preview `rustlyn pack` flow.
4. Inspect or lower LLVM bitcode to see the intermediate representation.
5. Generate bindings for any .NET assembly via `rustlyn-bindings scan/bindgen`.
6. Build `.rsproj` projects with `dotnet build` using the Rustlyn SDK.
7. Run generated-bindings workloads (lousygrep) that call .NET APIs from Rust.
8. Run the Avalonia desktop GUI sample entirely from Rust bitcode.
9. Validate with smoke tests and a 18,000+ line regression harness.

## Quick Start

Download the LLVM prebuilt used by the scripts:

```powershell
.\scripts\Get-LlvmPrebuilt.ps1
```

Rustlyn can also use the statically linked Rustlyn LLVM helper when built from an LLVM development subset:

```powershell
.\scripts\Build-RustlynLlvmHelper.ps1 -LlvmDevRoot D:\opt\llvm-subsets\llvm-dev
```

At runtime, `RUSTLYN_LLVM_ROOT` should point at a root containing `bin\rustlyn-llvm.exe` or directly at a directory containing `rustlyn-llvm.exe`. A legacy LLVM tools root containing `bin\llvm-opt.exe` is still accepted as a fallback.

Build a sample crate to `.bc`:

```powershell
.\scripts\Build-SampleBitcode.ps1 -Sample add
```

Run a focused smoke test on that sample:

```powershell
.\scripts\Test-Smoke.ps1 -Sample add -Mode Bitcode
```

Build a Cargo crate in place and emit stable Cargo-profile artifacts:

```powershell
dotnet build .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -c Release
$rustlyn = ".\dotnet\backend\src\Rustlyn.Tool\bin\Release\net10.0\rustlyn.exe"
& $rustlyn cargo build --manifest-path .\samples\add\Cargo.toml
```

This writes `target\debug\<crate>.bc`, `target\debug\<crate>.ll`, `target\debug\<crate>.dll`, and `target\debug\<crate>.pdb` beside Cargo's regular debug output. Published builds use the executable name `rustlyn`, so the same flow from a crate directory is `rustlyn cargo build`. Repo scripts use `scripts\Rustlyn.Cli.ps1` to resolve this local command without requiring a global install.

Run the canonical generated-bindings lousygrep workload:

```powershell
.\scripts\Test-Smoke.ps1 -Sample lousygrep -Mode Cargo -Configuration Release
```

Drive the backend tool directly:

```powershell
& $rustlyn inspect .\artifacts\out\add\add.bc
& $rustlyn lower .\artifacts\out\add\add.bc
& $rustlyn emit .\artifacts\out\add\add.bc --out .\artifacts\out\add\add.generated.dll
& $rustlyn emit .\artifacts\out\add\add.bc --out .\artifacts\out\add\add.generated.dll --pdb
& $rustlyn translate .\samples\add --out .\artifacts\out\add\add.from-cargo.dll --bitcode-out .\artifacts\out\add\add.from-cargo.bc
& $rustlyn translate .\samples\add --out .\artifacts\out\add\add.dll --cache .\artifacts\scratch\.cache.json
& $rustlyn pack .\samples\add --out .\artifacts\out\add --version 1.0.0
```

Run the backend regression harness:

```powershell
dotnet run -c Release --project .\dotnet\backend\tests\Rustlyn.Backend.Tests\Rustlyn.Backend.Tests.csproj
```

Try the first local MSBuild SDK facade:

```powershell
.\scripts\Test-MsBuildSdk.ps1 -Configuration Release
.\scripts\Test-MsBuildSdkBinary.ps1 -Configuration Release
.\scripts\Test-MsBuildSdkBuildStd.ps1 -Configuration Release
.\scripts\Test-MsBuildSdkPackage.ps1 -Configuration Release
```

The first script builds `samples/msbuild_add/msbuild_add.rsproj` with `dotnet build` by resolving the local `Rustlyn.Sdk` from `dotnet/backend/src`, then verifies the generated bitcode still invokes `add_i32(19, 23) => 42`. It also checks SourceGear-style project-system metadata such as `Language=Rust`, `.rs` as the default source extension, and managed target runtime. It builds `samples/msbuild_sourcegear_aliases/msbuild_sourcegear_aliases.rsproj`, proving SourceGear-style `RustToolchain=+nightly` and `RustDebugOrRelease=debug` properties map onto the revived translate path, and `samples/msbuild_generated_cargo/msbuild_generated_cargo.rsproj`, proving the SDK can synthesize a Cargo manifest from `.rsproj` metadata with both local `RustReference` and crates.io `RustCrateReference` dependencies, including inferred path dependency names, `Version`, `DefaultFeatures`, comma-separated `Features` metadata, and SourceGear-style Cargo auto-target guards. The binary script builds `samples/msbuild_bin_trivial/msbuild_bin_trivial.rsproj`, uses `RustlynBinaryTarget`, runs the emitted console assembly, verifies `Clean` removes generated outputs and copied support assemblies, and also checks `samples/msbuild_bin_inferred/msbuild_bin_inferred.rsproj`, where `OutputType=Exe` infers the Cargo binary target from `AssemblyName`. The build-std script builds `samples/msbuild_build_std_core/msbuild_build_std_core.rsproj` with `RustlynToolchain=nightly` and `RustlynBuildStd=core`, matching the first SourceGear sysroot-recovery rung. The package script packs `Rustlyn.Sdk` into `artifacts/scratch/packages`, bundles the published `rustlyn` tool under `tools/net10.0`, and verifies NuGet-style SDK resolution from generated scratch library, inferred-binary, and generated-bindings lousygrep `.rsproj` projects without passing a source-tree tool path. The lousygrep package check also runs the translated console assembly from scratch output, verifies copied support assemblies are present beside it, and verifies `Clean` removes them.

Try the first Avalonia bridge sample:

```powershell
.\scripts\Test-Smoke.ps1 -Sample avalonia_hello -Mode Cargo
dotnet run --project .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -- translate .\samples\avalonia_hello --out .\artifacts\scratch\avalonia_hello.dll --bitcode-out .\artifacts\scratch\avalonia_hello.bc --bin avalonia_hello
dotnet .\artifacts\scratch\avalonia_hello.dll --smoke
dotnet .\artifacts\scratch\avalonia_hello.dll
```

`samples/avalonia_hello` is a Rust-composed UI bridge proof. Rust creates the window, stack panel, text block, and button through backend-recognized Avalonia bridge calls; the support assembly owns the real Avalonia objects, application lifetime, object handles, and click callback dispatch back into generated Rust. The smoke run prints `avalonia:rust-ui:ok`. This is still a narrow bridge fixture, not generated Avalonia bindings, XAML translation, or data binding.

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
