# rust-msil

`rust-msil` is a working experiment for turning Rust-produced LLVM bitcode into managed .NET assemblies.

The immediate value of the repo is practical rather than aspirational:

- inspect LLVM bitcode coming out of small Rust crates
- lower that bitcode into a simpler backend-owned IR
- emit runnable managed assemblies from that IR
- keep growing a regression corpus of narrow Rust samples that pin backend behavior

This is not trying to recreate the original SourceGear SDK developer experience first. The current repository is focused on recovering the compiler pipeline, validating it end to end, and documenting what was different about the original Eric Sink experiment versus the revived design in this repo.

## What you can do here today

1. Build sample bitcode from a crate in `samples/`.
2. Inspect or lower the resulting LLVM bitcode.
3. Emit a managed assembly or invoke a method directly from bitcode.
4. Run smoke checks and focused backend regression tests while bringing up new slices.

## Quick Start

Download the LLVM prebuilt used by the scripts:

```powershell
.\scripts\Get-LlvmPrebuilt.ps1
```

Build a sample crate to `.bc`:

```powershell
.\scripts\Build-SampleBitcode.ps1 -Sample add
```

Run a focused smoke test on that sample:

```powershell
.\scripts\Test-Smoke.ps1 -Sample add -Mode Bitcode
```

Run the canonical generated-bindings lousygrep workload:

```powershell
.\scripts\Test-Smoke.ps1 -Sample lousygrep -Mode Cargo -Configuration Release
```

Drive the backend tool directly:

```powershell
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- inspect .\artifacts\out\add\add.bc
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- lower .\artifacts\out\add\add.bc
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- emit .\artifacts\out\add\add.bc --out .\artifacts\out\add\add.generated.dll
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- translate .\samples\add --out .\artifacts\out\add\add.from-cargo.dll --bitcode-out .\artifacts\out\add\add.from-cargo.bc
```

Run the backend regression harness:

```powershell
dotnet run -c Release --project .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj
```

Try the first local MSBuild SDK facade:

```powershell
.\scripts\Test-MsBuildSdk.ps1 -Configuration Release
.\scripts\Test-MsBuildSdkBinary.ps1 -Configuration Release
.\scripts\Test-MsBuildSdkBuildStd.ps1 -Configuration Release
.\scripts\Test-MsBuildSdkPackage.ps1 -Configuration Release
```

The first script builds `samples/msbuild_add/msbuild_add.rsproj` with `dotnet build` by resolving the local `RustMcil.Sdk` from `dotnet/backend/src`, then verifies the generated bitcode still invokes `add_i32(19, 23) => 42`. The binary script builds `samples/msbuild_bin_trivial/msbuild_bin_trivial.rsproj`, uses `RustMcilBinaryTarget`, runs the emitted console assembly, verifies `Clean` removes generated outputs, and also checks `samples/msbuild_bin_inferred/msbuild_bin_inferred.rsproj`, where `OutputType=Exe` infers the Cargo binary target from `AssemblyName`. The build-std script builds `samples/msbuild_build_std_core/msbuild_build_std_core.rsproj` with `RustMcilToolchain=nightly` and `RustMcilBuildStd=core`, matching the first SourceGear sysroot-recovery rung. The package script packs `RustMcil.Sdk` into `artifacts/scratch/packages`, bundles a published `RustMcil.Tool` under `tools/net10.0`, and verifies NuGet-style SDK resolution from generated scratch library and inferred-binary `.rsproj` projects without passing a source-tree tool path.

Try the first Avalonia bridge sample:

```powershell
.\scripts\Test-Smoke.ps1 -Sample avalonia_hello -Mode Cargo
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- translate .\samples\avalonia_hello --out .\artifacts\scratch\avalonia_hello.dll --bitcode-out .\artifacts\scratch\avalonia_hello.bc --bin avalonia_hello
dotnet .\artifacts\scratch\avalonia_hello.dll --smoke
dotnet .\artifacts\scratch\avalonia_hello.dll
```

`samples/avalonia_hello` is a Rust-composed UI bridge proof. Rust creates the window, stack panel, text block, and button through backend-recognized Avalonia bridge calls; the support assembly owns the real Avalonia objects, application lifetime, object handles, and click callback dispatch back into generated Rust. The smoke run prints `avalonia:rust-ui:ok`. This is still a narrow bridge fixture, not generated Avalonia bindings, XAML translation, or data binding.

## Documentation

- [Original Eric Sink design](docs/original-eric-sink-design.md)
- [Revived design in this repo](docs/revived-design.md)
- [SourceGear parity roadmap](docs/sourcegear-parity-roadmap.md)
- [SourceGear fake-link decision](docs/sourcegear-fake-link-decision.md)
- [Deep reconstruction notes](docs/reconstruction-notes.md)
- [Contributor and agent workflow](AGENTS.md)

## Repo Map

- `samples/`: narrow Rust crates used as regression fixtures
- `samples/generated_bindings_hello/`: first generated-style .NET binding fixture over console, environment method/property, directory, path, and string method/property APIs
- `samples/generated_bindings_lousygrep/`: canonical lousygrep-style fixture using generated Environment/File/String/Console bindings for the workload
- `scripts/`: repeatable PowerShell entry points for LLVM setup, sample builds, and smoke checks
- `dotnet/backend/src/RustMcil.Tool/`: CLI for inspect, lower, emit, invoke, and translate flows
- `dotnet/backend/src/RustMcil.Backend/`: lowering and IL emission logic
- `dotnet/backend/src/RustMcil.Bindings/`: tiny generated .NET binding prototype for Rust wrapper output, managed glue source, metadata-backed wrapper methods, and managed string handles created from Rust UTF-8 buffers
- `dotnet/backend/src/RustMcil.Bindings.Tool/`: build-time generator for backend managed binding glue
- `dotnet/backend/src/RustMcil.Interop/`: reusable managed object/exception handles and UTF-8 interop helpers for future generated bindings
- `dotnet/backend/src/RustMcil.Runtime/`: future home for LLVM/runtime semantic helpers
- `dotnet/backend/src/RustMcil.Os/`: future home for host OS and Rust `std` compatibility helpers
- `dotnet/backend/src/RustMcil.Sdk/`: local and packable SDK-style MSBuild facade that delegates `.rsproj` builds to the backend translate driver
- `dotnet/backend/tests/RustMcil.Backend.Tests/`: focused regression harness for lowering and runtime behavior
- `artifacts/decompiled/`: decompiled text extracted from the historical SDK for reference
- `artifacts/sdk-0.1.5/extracted/`: kept textual package contents and metadata from the published SDK snapshot

## Current Direction

The revived experiment is cargo-first and backend-first. The repository is deliberately optimized for bringing up one backend slice at a time with executable validation, rather than for presenting a polished `.rsproj` SDK shell.