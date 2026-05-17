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

## Documentation

- [Original Eric Sink design](docs/original-eric-sink-design.md)
- [Revived design in this repo](docs/revived-design.md)
- [Deep reconstruction notes](docs/reconstruction-notes.md)
- [Contributor and agent workflow](AGENTS.md)

## Repo Map

- `samples/`: narrow Rust crates used as regression fixtures
- `scripts/`: repeatable PowerShell entry points for LLVM setup, sample builds, and smoke checks
- `dotnet/backend/src/RustMcil.Tool/`: CLI for inspect, lower, emit, invoke, and translate flows
- `dotnet/backend/src/RustMcil.Backend/`: lowering and IL emission logic
- `dotnet/backend/tests/RustMcil.Backend.Tests/`: focused regression harness for lowering and runtime behavior
- `artifacts/decompiled/`: decompiled text extracted from the historical SDK for reference
- `artifacts/sdk-0.1.5/extracted/`: kept textual package contents and metadata from the published SDK snapshot

## Current Direction

The revived experiment is cargo-first and backend-first. The repository is deliberately optimized for bringing up one backend slice at a time with executable validation, rather than for presenting a polished `.rsproj` SDK shell.