# rustlyn

`rustlyn` is a working experiment for turning Rust-produced LLVM bitcode into managed .NET assemblies.

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
dotnet run --project .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -- inspect .\artifacts\out\add\add.bc
dotnet run --project .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -- lower .\artifacts\out\add\add.bc
dotnet run --project .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -- emit .\artifacts\out\add\add.bc --out .\artifacts\out\add\add.generated.dll
dotnet run --project .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -- translate .\samples\add --out .\artifacts\out\add\add.from-cargo.dll --bitcode-out .\artifacts\out\add\add.from-cargo.bc
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

The first script builds `samples/msbuild_add/msbuild_add.rsproj` with `dotnet build` by resolving the local `Rustlyn.Sdk` from `dotnet/backend/src`, then verifies the generated bitcode still invokes `add_i32(19, 23) => 42`. It also checks SourceGear-style project-system metadata such as `Language=Rust`, `.rs` as the default source extension, and managed target runtime. It builds `samples/msbuild_sourcegear_aliases/msbuild_sourcegear_aliases.rsproj`, proving SourceGear-style `RustToolchain=+nightly` and `RustDebugOrRelease=debug` properties map onto the revived translate path, and `samples/msbuild_generated_cargo/msbuild_generated_cargo.rsproj`, proving the SDK can synthesize a Cargo manifest from `.rsproj` metadata with both local `RustReference` and crates.io `RustCrateReference` dependencies, including inferred path dependency names, `Version`, `DefaultFeatures`, comma-separated `Features` metadata, and SourceGear-style Cargo auto-target guards. The binary script builds `samples/msbuild_bin_trivial/msbuild_bin_trivial.rsproj`, uses `RustlynBinaryTarget`, runs the emitted console assembly, verifies `Clean` removes generated outputs and copied support assemblies, and also checks `samples/msbuild_bin_inferred/msbuild_bin_inferred.rsproj`, where `OutputType=Exe` infers the Cargo binary target from `AssemblyName`. The build-std script builds `samples/msbuild_build_std_core/msbuild_build_std_core.rsproj` with `RustlynToolchain=nightly` and `RustlynBuildStd=core`, matching the first SourceGear sysroot-recovery rung. The package script packs `Rustlyn.Sdk` into `artifacts/scratch/packages`, bundles a published `Rustlyn.Tool` under `tools/net10.0`, and verifies NuGet-style SDK resolution from generated scratch library, inferred-binary, and generated-bindings lousygrep `.rsproj` projects without passing a source-tree tool path. The lousygrep package check also runs the translated console assembly from scratch output, verifies copied support assemblies are present beside it, and verifies `Clean` removes them.

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
- [Deep reconstruction notes](docs/reconstruction-notes.md)
- [Contributor and agent workflow](AGENTS.md)

## Repo Map

- `samples/`: narrow Rust crates used as regression fixtures
- `samples/generated_bindings_hello/`: first generated-style .NET binding fixture over console, environment method/property, directory, path, and string method/property APIs
- `samples/generated_bindings_lousygrep/`: canonical lousygrep-style fixture using generated Environment/File/String/Console bindings for the workload
- `scripts/`: repeatable PowerShell entry points for LLVM setup, sample builds, and smoke checks
- `dotnet/backend/src/Rustlyn.Tool/`: CLI for inspect, lower, emit, invoke, and translate flows
- `dotnet/backend/src/Rustlyn.Backend/`: lowering and IL emission logic
- `dotnet/backend/src/Rustlyn.Bindings/`: tiny generated .NET binding prototype for Rust wrapper output, managed glue source, metadata-backed wrapper methods, and managed string handles created from Rust UTF-8 buffers
- `dotnet/backend/src/Rustlyn.Bindings.Tool/`: build-time generator for backend managed binding glue
- `dotnet/backend/src/Rustlyn.Interop/`: reusable managed object/exception handles and UTF-8 interop helpers for future generated bindings
- `dotnet/backend/src/Rustlyn.Runtime/`: future home for LLVM/runtime semantic helpers
- `dotnet/backend/src/Rustlyn.Os/`: future home for host OS and Rust `std` compatibility helpers
- `dotnet/backend/src/Rustlyn.Sdk/`: local and packable SDK-style MSBuild facade that delegates `.rsproj` builds to the backend translate driver
- `dotnet/backend/tests/Rustlyn.Backend.Tests/`: focused regression harness for lowering and runtime behavior

Historical SourceGear package details are summarized in `docs/` and linked to public package/blog sources; extracted package trees and decompiled payloads are intentionally not kept as repo content.

## Current Direction

The revived experiment is cargo-first and backend-first. The repository is deliberately optimized for bringing up one backend slice at a time with executable validation, rather than for presenting a polished `.rsproj` SDK shell.
