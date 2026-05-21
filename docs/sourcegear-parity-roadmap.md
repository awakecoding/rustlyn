# SourceGear Parity Roadmap

This document tracks the work needed to recover the same kind of feature set as Eric Sink's original SourceGear/Llama Rust-on-.NET experiment while keeping the revived repository's cargo-first, test-first workflow.

The goal is feature parity, not implementation parity. The old SDK's MSBuild shell, fake linker, generated bindings, runtime helpers, and OS compatibility layer are requirements evidence; the revived implementation should modernize those ideas around the current backend, .NET 10, current Rust, and focused executable validation.

## Parity Matrix

| Old SourceGear capability | Evidence | Revived owner | Current status | First parity target |
| --- | --- | --- | --- | --- |
| SDK-style `.rsproj` build with `dotnet build` | `artifacts/sdk-0.1.5/extracted/Sdk/`, `artifacts/sdk-0.1.5/extracted/build/` | Future MSBuild wrapper | Not started | Add only after a cargo driver is stable |
| Synthetic Cargo project generation from project metadata | `artifacts/decompiled/rsbuild-program/Program.decompiled.cs` | Future cargo/MSBuild driver | Not started | Generate a temporary Cargo build directory from explicit tool options |
| Custom target/sysroot bitcode flow | `artifacts/decompiled/build-sysroot/build_sysroot.decompiled.cs` | `RustBitcodeCompiler` plus future target tooling | Partial `cargo rustc --emit llvm-bc`; `cargo -Z build-std` rungs for `core`, `core,alloc`, and `std,panic_abort` are validated against focused fixtures; fake-link remains deferred by decision record | Continue std/environment/path/time fixtures before adding custom sysroot tooling |
| Fake linker handoff to `.bc` | `artifacts/sdk-0.1.5/extracted/tools/rsfakelink/` | Optional future `RustMcil.FakeLink` | Deferred by [SourceGear fake-link decision](sourcegear-fake-link-decision.md) | Reopen only when a focused fixture proves Cargo cannot capture the required bitcode |
| LLVM-to-CIL compiler backend | Historical `sgllvm`/`sgcil`/`llvm2cil` notes | `LoweredIrLowerer`, `LoweredAssemblyEmitter` | Active, sample-driven | Keep expanding data-model coverage with permanent fixtures |
| Runtime helpers for LLVM semantics | Historical `sgrt.dll` notes | Future `RustMcil.Runtime` | Partial helpers embedded in backend | Split memops, overflow, vector, atomics, and entry wrappers into runtime support |
| Managed object and exception ABI | `tools/crates/sgrust_core`, generated `rs_dotnet` crates | `RustMcil.Interop` | Started | Stable object handles, exception handles, string/type handle helpers, drop/release |
| Generated Rust bindings for .NET APIs | `artifacts/sdk-0.1.5/extracted/tools/crates/rs_dotnet/` | `RustMcil.Bindings` plus backend managed glue | Started with a tiny generator that emits fixture Rust wrappers for console, directory, file line reads, strings, and string arrays | Generate wrappers and glue for a small `System.Runtime`/`System.IO` subset |
| Win32/OS compatibility layer for Rust `std` | Historical `sgwin32.dll` notes | Future `RustMcil.Os` | Partial targeted path/file helpers | Add APIs only as `std` fixtures require them |
| Real workloads beyond hello world | Blog evidence and old lousygrep/resvg notes | Samples plus smoke/tests | Started generated-bindings lousygrep core flow, plus focused samples and Avalonia bridge proof | Expand generated-bindings lousygrep to replace the primitive handwritten bridge sample, then choose a harder crate workload |
| Packaged SDK/tool distribution | NuGet package layout | Future packaging | Not started | Package after CLI/runtime/bindings have versioned contracts |

## Recovery Order

1. Keep the current cargo-first backend as the control plane.
2. Split runtime concepts into explicit projects instead of growing one-off bridge helpers forever.
3. Build a general interop ABI before attempting generated .NET bindings.
4. Use modern `cargo -Z build-std` before rebuilding the old fake-linker/sysroot machinery.
5. Recover generated BCL bindings with a tiny subset before trying a wide API surface.
6. Prove old-style workloads through samples and smoke tests.
7. Add cargo-driver UX, then make MSBuild an optional wrapper.
8. Package only after the ABI and generated binding contracts are stable.

## Near-Term Implementation Slices

### Slice 1: General Interop Handles

Create `RustMcil.Interop` with a reusable managed handle store for objects and exceptions. This becomes the common base for generated bindings and, later, Avalonia's bridge handles.

Validation:

```powershell
dotnet build .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj
```

### Slice 2: Interop String And Exception Surface

Started with UTF-8 pointer/length helpers, exception message/type queries, integer handle overloads, and explicit typed release behavior. Continue by adding UTF-16 helpers and tests that mirror the old `Result<_, System::Exception>` wrapper shape.

### Slice 3: Generated Binding Prototype

Started with `RustMcil.Bindings`, which emits the `samples/generated_bindings_hello` Rust wrapper module over `rust_mcil_bindgen_*` symbols. The backend maps those symbols to managed glue that uses `RustMcil.Interop` handles, UTF-8 helpers, and exception handles. The current generated surface includes `System.Console.WriteLine`, `System.Environment.CurrentDirectory` as a static property, `System.IO.Directory.GetCurrentDirectory`, `System.IO.File.ReadAllLines`, `System.IO.Path.ChangeExtension`, `System.IO.Path.Combine`, `System.IO.Path.EndsInDirectorySeparator`, `System.IO.Path.GetDirectoryName`, `System.IO.Path.GetExtension`, `System.IO.Path.GetFileName`, `System.IO.Path.GetFileNameWithoutExtension`, `System.IO.Path.GetFullPath`, `System.IO.Path.GetPathRoot`, `System.IO.Path.GetRelativePath`, `System.IO.Path.GetTempPath`, `System.IO.Path.HasExtension`, `System.IO.Path.IsPathFullyQualified`, `System.IO.Path.IsPathRooted`, `System.String`, `System.String.Length` as an instance property, managed string construction from Rust UTF-8 buffers, `System.String[]`, and ordinal `System.String.Contains`. Rust wrapper generation has started moving method entries, beginning with `io::path` wrappers, into binding metadata instead of keeping every wrapper body fixed, and now covers object-handle and boolean-as-int result wrappers.

First target APIs:

- `System.String`
- `System.Console.WriteLine`
- `System.Environment.GetCommandLineArgs`
- `System.Environment.CurrentDirectory`
- `System.IO.File.ReadAllLines`
- `System.IO.Directory.GetCurrentDirectory`
- `System.IO.Path.ChangeExtension`
- `System.IO.Path.Combine`
- `System.IO.Path.EndsInDirectorySeparator`
- `System.IO.Path.GetDirectoryName`
- `System.IO.Path.GetExtension`
- `System.IO.Path.GetFileName`
- `System.IO.Path.GetFileNameWithoutExtension`
- `System.IO.Path.GetFullPath`
- `System.IO.Path.GetPathRoot`
- `System.IO.Path.GetRelativePath`
- `System.IO.Path.GetTempPath`
- `System.IO.Path.HasExtension`
- `System.IO.Path.IsPathFullyQualified`
- `System.IO.Path.IsPathRooted`
- `System.String.Length`

Validation:

```powershell
.\scripts\Test-Smoke.ps1 -Sample generated_bindings_hello -Mode Cargo
```

### Slice 4: Generated-Bindings Lousygrep

Promoted `samples/generated_bindings_lousygrep` as the canonical lousygrep smoke via `-Sample lousygrep`. It uses generated bindings for `Environment.GetCommandLineArgs`, `File.ReadAllLines`, `String.Contains`, string array access, string UTF-8 copy, object release, and console output. `lousygrep_primitive` remains as legacy handwritten bridge coverage while generated bindings take over the main workload validation path. `RustMcil.Bindings` now also owns the `rust_mcil_bindgen_*` to runtime-helper glue map consumed by the backend emitter, and `RustMcil.Bindings.Tool` generates the backend managed helper partial into `obj` during the backend build. Managed helper source is assembled from signature, exception convention, result-operation metadata, and reflected managed expression metadata for calls and properties such as `Console.WriteLine`, `Environment.GetCommandLineArgs`, `Environment.CurrentDirectory`, `Directory.GetCurrentDirectory`, `File.ReadAllLines`, `Path.ChangeExtension`, `Path.Combine`, `Path.EndsInDirectorySeparator`, `Path.GetDirectoryName`, `Path.GetExtension`, `Path.GetFileName`, `Path.GetFileNameWithoutExtension`, `Path.GetFullPath`, `Path.GetPathRoot`, `Path.GetRelativePath`, `Path.GetTempPath`, `Path.HasExtension`, `Path.IsPathFullyQualified`, `Path.IsPathRooted`, `String.Length`, and `String.Contains`. The generated `ManagedString` wrapper can now construct a managed `System.String` handle from Rust-owned UTF-8 bytes, and the Rust wrapper generator has started emitting wrapper methods from metadata with object-handle and boolean-as-int result wrappers. Continue by adding more binding shapes that describe constructors, overload selection, and broader generated Rust wrapper methods from the same metadata.

Validation:

```powershell
.\scripts\Test-Smoke.ps1 -Sample lousygrep -Mode Cargo -Configuration Release
```

### Slice 5: `std` Compatibility Ladder

Promote normal Rust source using `core`, then `alloc`, then `std::fs`, environment, paths, console IO, and time. Add `RustMcil.Os` APIs only when a fixture exposes the missing behavior.

The cargo driver now accepts `--toolchain`, `--target`, `--build-std`, and `--build-std-features`, validates `rust-src` before build-std runs, and exposes the same options through smoke descriptors. This makes the ladder measurable without making nightly or `rust-src` a default test dependency.

The first rung is now promoted as `build_std_core_probe`, reusing the `#![no_std]` `samples/add` crate with `--toolchain nightly --build-std core`. It validates bitcode capture, inspection, lowering, emission, and invocation with `add_i32(19, 23) == 42`.

The second rung is now promoted as `build_std_alloc_probe`, using `samples/alloc_only_probe` with `#![no_std]`, `extern crate alloc`, a tiny fixed global allocator, and `--toolchain nightly --build-std core,alloc`. It validates alloc shim capture plus emitted invocation with `alloc_vec_capacity_score() == 4`. This rung also promoted support for optimized functions whose first lowered block has a non-canonical label instead of literally `start` or `entry`.

The third rung is now promoted as `build_std_std_probe`, reusing `samples/std_fs` with `--toolchain nightly --build-std std,panic_abort`. It validates std-backed path construction, `std::fs::read_to_string`, bitcode inspection/lowering, and emitted invocation with `std_fs_line_count() == 3`.

The fake-link decision gate is now closed for this phase: [SourceGear fake-link decision](sourcegear-fake-link-decision.md) keeps `RustMcil.FakeLink` deferred until a focused fixture proves direct Cargo bitcode emission plus build-std is insufficient.

Validation:

```powershell
dotnet run -c Release --project .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj -- AddSampleBuildsWithBuildStdCore
.\scripts\Test-Smoke.ps1 -Sample build_std_core_probe -Mode Cargo -Configuration Release
dotnet run -c Release --project .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj -- AllocOnlyProbeBuildsWithBuildStdAlloc
.\scripts\Test-Smoke.ps1 -Sample build_std_alloc_probe -Mode Cargo -Configuration Release
dotnet run -c Release --project .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj -- StdFsBuildsWithBuildStdStd
.\scripts\Test-Smoke.ps1 -Sample build_std_std_probe -Mode Cargo -Configuration Release
```

### Slice 6: Runtime And OS Assembly Split

Started with behavior-preserving project shells: `RustMcil.Runtime` and `RustMcil.Os` now exist beside `RustMcil.Interop`, and `RustMcil.Backend` references both. No helper methods have moved yet. This establishes the dependency direction for later migration slices while keeping the existing emitter mappings and support assembly copying unchanged.

Intended ownership:

- `RustMcil.Runtime`: LLVM/runtime semantics such as panic-abort helpers, allocator shims, memops, vector helpers, atomics, and future entry wrappers.
- `RustMcil.Os`: host OS and Rust `std` compatibility helpers such as command-line arguments, console output, file reads, environment directories, path operations, and UTF-8 path/string bridge helpers.
- `RustMcil.Interop`: reusable managed object handles, exception handles, UTF-8 primitives, and release semantics.

Validation:

```powershell
dotnet build -c Release .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj
```

## Non-Goals For The First Recovery Pass

- Recreating the old MSBuild SDK before the compiler/runtime/bindings work is stable.
- Checking generated binding crates into the repo as a huge static snapshot.
- Cloning the old xargo-era sysroot design if `cargo -Z build-std` works.
- Treating the current Avalonia bridge as a substitute for generated managed API bindings.