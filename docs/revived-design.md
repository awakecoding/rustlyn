# Revived Design

This document describes the design being rebuilt in this repository today. It is intentionally separate from the historical SourceGear/Llama SDK design.

## Summary

The revived experiment is a cargo-first, backend-first reconstruction of the core Rust-to-.NET idea:

1. Use ordinary Rust sample crates as the frontend input.
2. Produce LLVM bitcode either directly or through Cargo.
3. Lower LLVM text into a backend-owned reduced IR.
4. Emit managed IL and helper methods from that reduced IR.
5. Validate every newly supported slice with executable smoke tests and regression fixtures.

The goal is not to recreate the old SDK packaging and project-system experience before the compiler path is proven. The goal is to recover and harden the translation pipeline itself.

## Design Priorities

- cargo-first experimentation instead of an MSBuild-first outer shell
- minimal, explicit samples that pin one backend behavior at a time
- executable validation after each narrow change
- permanent regression coverage for every promoted slice
- clear ownership boundaries between parsing/lowering and IL emission
- documentation that keeps the historical design and the revived design distinct

## Current Pipeline In This Repo

### 1. Sample crates

`samples/` contains small Rust crates that isolate specific behaviors such as arithmetic, control flow, comparisons, vector reductions, adjacent transformed loops, and managed runtime bridge calls.

`samples/avalonia_hello` is the first desktop-app bridge fixture. Its Rust entrypoint calls `rust_mcil_avalonia_run_app`, and exported Rust callbacks build a real window with a stack panel, text block, and button through explicit `rust_mcil_avalonia_*` bridge calls. The support assembly owns Avalonia startup, object handles, and event dispatch, while Rust owns UI composition and the click behavior that updates the text. The sample is deliberately scoped to proving the Rust bitcode -> managed assembly -> Avalonia control API path; it is not generated Avalonia bindings, XAML translation, data binding, or general delegate support.

### 2. Bitcode production

`scripts/Build-SampleBitcode.ps1` builds a sample to LLVM bitcode and writes the result under `artifacts/out/`.

For cargo-driven paths, `RustMcil.Tool translate` can build from the crate and emit both the translated assembly and the intermediate bitcode. The translate path also carries SourceGear-recovery options for `--toolchain`, `--target`, `--build-std`, and `--build-std-features`, with `rust-src` preflight diagnostics when build-std is requested. The current fake-link decision keeps the historical `rsfakelink` approach deferred until a focused fixture proves direct Cargo bitcode emission cannot capture the required artifact.

### 3. Inspection and lowering

`dotnet/backend/src/RustMcil.Tool/` exposes the main CLI commands:

- `inspect`
- `lower`
- `emit`
- `invoke`
- `translate`

The lowering stage produces a reduced IR that is easier for the backend tests to reason about than raw LLVM syntax.

### 4. Backend ownership

The current design has two especially important ownership boundaries:

- `LoweredIrLowerer.cs`: responsible for parsing and normalizing the lowered IR surface
- `LoweredAssemblyEmitter.cs`: responsible for managed IL emission, helper generation, intrinsic dispatch, and runtime glue

Runtime bridge support assemblies live beside the backend when a slice needs managed functionality that should not be generated as IL directly. The Avalonia slice uses this pattern so the emitter recognizes the bridge symbol family, adds the appropriate entrypoint metadata, copies the support dependencies with the generated assembly, and leaves Avalonia lifetime/threading details inside the support assembly.

`RustMcil.Interop` is the first recovered SourceGear-parity runtime layer. It owns reusable managed object and exception handles, UTF-8 pointer/length helpers, and exception text queries that future generated .NET bindings can share instead of growing one-off bridge stores per sample.

`RustMcil.Runtime` and `RustMcil.Os` now exist as behavior-preserving project shells for the next split. `RustMcil.Runtime` will own LLVM/runtime semantics such as panic, allocator, memop, vector, atomic, and entry-wrapper helpers. `RustMcil.Os` will own host OS and Rust `std` compatibility helpers such as args, console, file, environment, path, and UTF-8 path/string bridge behavior. The first compatibility facade keeps Backend `RuntimeBridgeHelpers` method names stable while forwarding `rem_euclid` to `RustMcil.Runtime.NumericRuntime`, command-line argument/environment path UTF-8 helpers to `RustMcil.Os.HostEnvironment`, console output helpers to `RustMcil.Os.HostConsole`, `File.ReadAllLines` UTF-8 helpers to `RustMcil.Os.HostFileSystem`, and `Path.Combine` UTF-8 helpers to `RustMcil.Os.HostPath`.

`RustMcil.Bindings` is the first generated-bindings parity foothold. It emits the generated-style Rust wrapper module checked into `samples/generated_bindings_hello`, owns the `rust_mcil_bindgen_*` managed glue map consumed by the backend emitter, and generates the backend `RuntimeBridgeHelpers.Bindings.g.cs` partial at build time through `RustMcil.Bindings.Tool`. Managed glue is now assembled from per-binding signature, exception convention, result-operation metadata, and reflected managed expression metadata instead of fixed whole-method templates or raw C# value snippets. The mapped helpers return object and exception handles through `RustMcil.Interop`, and the Rust wrapper generator now has metadata-backed method entries for new wrapper sections, including object-handle and boolean-as-int result wrappers. The current fixed BCL surface covers console output, command-line arguments, `Environment.CurrentDirectory` as a static property, directory current-directory method calls, file line reads, `Path.ChangeExtension`, `Path.Combine`, `Path.EndsInDirectorySeparator`, `Path.GetDirectoryName`, `Path.GetExtension`, `Path.GetFileName`, `Path.GetFileNameWithoutExtension`, `Path.GetFullPath`, `Path.GetPathRoot`, `Path.GetRelativePath`, `Path.GetTempPath`, `Path.HasExtension`, `Path.IsPathFullyQualified`, `Path.IsPathRooted`, string arrays, managed strings created from Rust UTF-8 buffers, `String.Length` as an instance property, string UTF-8 copy, and ordinal string contains. `samples/generated_bindings_lousygrep` uses that generated binding surface for the lousygrep workload, including argv, file IO, string matching, and output.

When a slice fails, the intended workflow is to patch the direct owner rather than layering fixes across unrelated surfaces.

### 5. Validation surfaces

Two validation layers matter most:

- `scripts/Test-Smoke.ps1` for focused executable smoke checks
- `dotnet/backend/tests/RustMcil.Backend.Tests/Program.cs` for permanent regression assertions over module summaries, lowered IR, typed instructions, emitted execution, and Cargo-built behavior

## How This Differs From The Original Design

Compared with Eric Sink's original design, this revived repo intentionally changes the center of gravity:

- from MSBuild-first to cargo-first
- from packaged binary SDK distribution to source-first backend development
- from one large end-user shell to many small backend regression fixtures
- from historical proof-of-concept packaging to repeatable local validation in repo
- from implicit compiler behavior to aggressively pinned sample-by-sample coverage

The tracked recovery plan for closing those gaps is [SourceGear parity roadmap](sourcegear-parity-roadmap.md), with the current fake-link gate recorded in [SourceGear fake-link decision](sourcegear-fake-link-decision.md).

## Current Repository Shape

- `samples/`: narrow frontend fixtures
- `samples/generated_bindings_hello/`: generated-style .NET binding fixture over `System.Console`, `System.IO.Directory`, and `System.String`
- `samples/generated_bindings_lousygrep/`: canonical lousygrep-style generated-bindings fixture over `System.Environment.GetCommandLineArgs`, `System.IO.File.ReadAllLines`, `System.String[]`, and `System.String.Contains`
- `scripts/`: repeatable PowerShell workflows
- `dotnet/backend/src/RustMcil.Bindings/`: generated Rust binding prototype and output-shape tests
- `dotnet/backend/src/RustMcil.Backend/`: backend implementation
- `dotnet/backend/src/RustMcil.Interop/`: reusable managed object/exception handles and UTF-8 interop helpers for future generated bindings
- `dotnet/backend/src/RustMcil.Runtime/`: shell for future LLVM/runtime semantic helpers
- `dotnet/backend/src/RustMcil.Os/`: shell for future host OS and Rust `std` compatibility helpers
- `dotnet/backend/src/RustMcil.Tool/`: command-line entry point
- `dotnet/backend/tests/`: backend regression harness
- `artifacts/decompiled/` and `artifacts/sdk-0.1.5/extracted/`: historical reference material only

## Why The Repo Keeps Historical Material

The old design still matters because it answers two questions:

- what the original system actually shipped
- which architectural choices are worth preserving versus replacing

That historical material is kept as reference evidence, not as the primary product surface of the revived experiment.

## Practical Development Loop

The working loop for this repo is:

1. choose one failing or uncovered slice
2. build or translate the smallest sample that exposes it
3. fix the direct owner in lowering or emission
4. validate immediately with the narrowest executable check
5. promote the slice into permanent sample, smoke coverage, regression assertions, and notes

That loop is what keeps the revived experiment moving without reintroducing the complexity of the original SDK too early.