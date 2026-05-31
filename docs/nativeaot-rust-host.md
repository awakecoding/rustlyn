# NativeAOT Rust host plan

This document captures the lowest-risk path toward a single Rust `rustlyn` executable that reuses the managed Rustlyn backend through a NativeAOT library and FFI boundary.

The goal is not to rewrite the backend in Rust. The goal is to keep the C# lowerer/emitter where it is strongest, make the non-optional compiler core NativeAOT-friendly, and let a Rust host own CLI, Cargo/Rust toolchain orchestration, and LLVM integration.

## Current implementation status

The first unified-host slice now exists under `native\rustlyn`. On Windows x64 it builds a Rust `rustlyn.exe` that statically links LLVM and owns CLI dispatch, Cargo/Rust toolchain orchestration, and in-process LLVM inspection/printing. The managed backend is compiled with NativeAOT and currently supports two host modes:

- **Shared mode** (`RUSTLYN_NATIVEAOT_LINK_MODE=shared`, default): the Rust executable loads `rustlyn_nativeaot.dll` from beside itself and calls exported backend functions through resolved function pointers.
- **Static mode** (`RUSTLYN_NATIVEAOT_LINK_MODE=static`): the Rust executable links `rustlyn_nativeaot.lib` plus NativeAOT runtime-pack static libraries. This remains the final single-executable target, but it requires LLVM and NativeAOT static inputs to agree on the MSVC CRT model.

The managed NativeAOT library exposes a small versioned JSON ABI for `emit`, `lower`, and `pack`. In shared mode it does not import LLVM symbols from the Rust `.exe`; instead the Rust host registers callback pointers for Rust-owned LLVM services at startup.

The native host currently covers `new`, `rustc`, `diagnose`, `inspect`, `llvm`, `cargo build`, `translate`, `emit`, `lower`, and `pack`. `run` and `invoke` remain intentionally outside the NativeAOT surface because they depend on dynamic assembly loading and reflection.

Repeatable validation lives in `scripts\Test-NativeHostParity.ps1`; the zip staging path lives in `scripts\Build-RustlynNativeDistribution.ps1`. The shared mode can build with a dynamic-CRT LLVM static package because NativeAOT runtime static libraries are no longer linked into the Rust executable. Final static CI wiring is still blocked on a reproducible `/MT` static LLVM prebuilt for each RID.

## Current lower/emit dependency inventory

The non-optional translation path is centered on these backend files:

| Area | Current owner | Notes for NativeAOT |
| --- | --- | --- |
| Lowered IR model and text lowering | `LoweredIrLowerer.cs`, `LoweredTypes.cs`, instruction/layout helpers | Mostly ordinary managed code. Keep this in the AOT-safe core. |
| LLVM bitcode to textual/JSON IR | `LlvmNativeLibraryLocator.cs`, `LlvmToolingDisassembler.cs`, `RustlynLlvmLowerJsonReader.cs`, `RustlynLlvmOptimizer.cs` | Shelling out to `rustlyn-llvm` is AOT-friendly. `LLVMSharp.Interop` fallback is not part of the desired Rust-host core. |
| Assembly/IL emission | `LoweredAssemblyEmitter.cs`, `PortablePdbEmitter.cs` | Uses `System.Reflection.Metadata`, not Microsoft.CodeAnalysis/Roslyn. This is favorable for NativeAOT, but reflection-based helper discovery must be removed or rooted. |
| Runtime helpers called by generated IL | `RuntimeBridgeHelpers.cs`, generated `RuntimeBridgeHelpers.*.g.cs`, `RustStdShimManifest.cs` | Needs explicit trimming roots because generated IL calls are invisible to the NativeAOT linker. |
| Runtime support assemblies | `Rustlyn.Runtime`, `Rustlyn.Os`, `Rustlyn.Interop` | Still needed by emitted managed assemblies unless the output model changes. Discovery/copying must not depend on `Assembly.Location`. |

The current `Rustlyn.Backend` project also references surfaces that should stay outside the NativeAOT compiler core:

| Dependency | Why it is outside the core |
| --- | --- |
| `Rustlyn.Bindings` | Owns binding surface generation, reflection scanning, runtime manifests, and `MetadataLoadContext` usage. Its generated outputs can feed the core, but the scanner should not be in the AOT runtime graph. |
| `Rustlyn.AvaloniaSupport` | Fixture-specific desktop bridge and Avalonia package dependency. Keep as optional support, not a core compiler dependency. |
| `Rustlyn.PowerShellSupport` | Optional package surface with `System.Management.Automation`. Keep outside the core dependency graph. |
| `LLVMSharp.Interop` | Used by inspection/fallback paths. The Rust host should own LLVM and call the existing `rustlyn-llvm` helper or in-process Rust LLVM layer instead. |

## Lowest-risk work order

1. **Split data contracts from scanners.** Move or duplicate only the binding manifest DTOs needed by `EmitOptions` and `LoweredAssemblyEmitter` into an AOT-safe assembly. Leave `BindingSurface`, scanners, `MetadataLoadContext`, Avalonia, and PowerShell manifest factories in `Rustlyn.Bindings`. _(Next: the analyzer baseline shows `Rustlyn.Bindings` dominates the core-graph findings; this is now the highest-leverage decoupling step.)_
2. **Stop auto-creating optional manifests in the emitter.** ✅ Done. The emitter no longer auto-creates Avalonia/PowerShell manifests; the CLI supplies explicit binding manifests per lowered module via `CreateEmitOptions(..., LoweredModule)`.
3. **Replace reflection-based runtime bridge map construction.** `BuildRuntimeBridgeMap` currently reflects over `RuntimeBridgeHelpers` and re-creates binding manifests at runtime. Replace this with generated/static symbol-to-helper tables that can be rooted for trimming. _(Deferred: a source-structure regression test guards this method and there is no consuming AOT/trimmed publish yet to prove the change matters. Tackle alongside the shared-library spike.)_
4. **Replace support-assembly discovery.** ✅ Done. `CopyRuntimeSupportAssemblies` now resolves through `ResolveSupportAssemblyPath`, which keeps `Assembly.Location` for the framework-dependent host and falls back to `AppContext.BaseDirectory` for single-file/NativeAOT layouts. The intentional `Location` access carries a justified IL3000 suppression.
5. **Add a shared-library NativeAOT spike.** Start with a `.dll`/`.so`/`.dylib` export before static linking. Use `[UnmanagedCallersOnly]`, coarse JSON options, source-generated JSON serializers, top-level exception capture, and a paired `rustlyn_free`. _(First acceptance check: drive the [analyzer baseline](nativeaot-aot-analyzer-baseline.md) core-graph findings toward zero.)_
6. **Add a Rust FFI caller spike.** Validate UTF-8 path/options passing, result ownership, structured diagnostics, and failure behavior.
7. **Attempt static library linkage last.** Only after the shared library path works should the Rust executable link a NativeAOT `.lib`/`.a`, because static NativeAOT linkage needs platform-specific linker configuration and runtime/system libraries.

Items 1, 3, 5, 6, and 7 form a cohesive NativeAOT spike milestone and should be tackled together, starting by standing up the minimal shared-library publish so that trimming roots and the contract split can actually be validated. The analyzer baseline ([`nativeaot-aot-analyzer-baseline.md`](nativeaot-aot-analyzer-baseline.md)) is the measurable entry check for that milestone.

## Intended FFI shape

Keep the ABI small and stable. Do not mirror C# classes over FFI.

```c
int32_t rustlyn_emit(
    const uint8_t* options_json,
    size_t options_len,
    uint8_t** result_json,
    size_t* result_len);

int32_t rustlyn_register_host_callbacks(
    int32_t schema_version,
    const RustlynHostCallbacks* callbacks);

void rustlyn_free(uint8_t* ptr);
```

The shared-library path implements this shape in `Rustlyn.NativeAot` with cdecl exports:

| Export | Return code | Ownership |
| --- | --- | --- |
| `rustlyn_register_host_callbacks(schema_version, callbacks)` | `0` success, `-1` invalid callback ABI | Registers Rust-owned callback pointers before any lower/emit command. The callback table is copied by value; the functions must remain valid for process lifetime. |
| `rustlyn_emit(options_json, options_len, result_json, result_len)` | `0` success, `1` operation failure with result JSON, `-1` invalid ABI arguments, `-2` result allocation failure | On non-negative returns, `*result_json` is owned by the caller and must be released with `rustlyn_free`. On negative returns, `*result_json == null` and `*result_len == 0`. |
| `rustlyn_lower(options_json, options_len, result_json, result_len)` | Same shape as `rustlyn_emit` | Returns lowered IR text in result JSON. |
| `rustlyn_pack(options_json, options_len, result_json, result_len)` | Same shape as `rustlyn_emit` | Packs an emitted assembly into a `.nuspec` and `.nupkg`. |
| `rustlyn_free(ptr)` | `void` | Frees memory returned by `rustlyn_emit`; `null` is accepted as a no-op. |

The spike's options JSON is intentionally small and bitcode-only:

```json
{
  "inputPath": "path/to/input.bc",
  "outputPath": "path/to/output.dll",
  "llvmRoot": "optional/path/to/llvm",
  "emitPdb": false,
  "strictUnsupportedIr": true
}
```

The result JSON carries `success`, `outputPath`, `outputFiles`, `diagnostics`, and `exceptionType`. The export catches managed exceptions and returns them as diagnostics; process-corrupting failures such as stack overflows, access violations, or unrecoverable allocation failures can still terminate the host process.

## Non-goals for the first spike

- Do not static-link first.
- Do not port the whole CLI to Rust before proving the library boundary.
- Do not include Avalonia, PowerShell, or binding scanners in the NativeAOT library.
- Do not require generated managed assemblies to be NativeAOT themselves. Compiler-host NativeAOT and output-assembly NativeAOT are separate workstreams.

## Shared NativeAOT host mode

`Rustlyn.NativeAot` publishes a NativeAOT shared library named `rustlyn_nativeaot`:

```powershell
dotnet publish .\dotnet\backend\src\Rustlyn.NativeAot\Rustlyn.NativeAot.csproj `
  -c Release `
  -r win-x64 `
  --self-contained `
  -p:PublishAot=true `
  -p:NativeLib=Shared
```

Build this project standalone. It references `Rustlyn.Backend` with `RustlynBackendIncludeOptionalBindings=false`; adding it to a broader solution build before the backend is split can create duplicate backend build graphs with different generated glue inputs.

Build the Rust host in shared mode:

```powershell
$env:RUSTLYN_NATIVEAOT_LINK_MODE = "shared"
$env:RUSTLYN_NATIVEAOT_LIB_DIR = (Resolve-Path .\dotnet\backend\src\Rustlyn.NativeAot\bin\Release\net10.0\win-x64\publish).Path
$env:RUSTLYN_LLVM_ROOT = "<llvm-root>"
cargo build --manifest-path .\native\rustlyn\Cargo.toml --release
```

The build stages `rustlyn_nativeaot.dll` beside `native\rustlyn\target\release\rustlyn.exe`. At runtime the Rust host loads the DLL, resolves `rustlyn_register_host_callbacks`, `rustlyn_emit`, `rustlyn_lower`, `rustlyn_pack`, and `rustlyn_free`, then registers callbacks for `rustlyn_llvm_print_ir` and `rustlyn_llvm_free`.

Current validation:

- `dotnet publish -p:NativeLib=Shared` produces `rustlyn_nativeaot.dll`.
- `rustlyn lower artifacts\out\add\add.bc` exercises the registered LLVM callback path and prints lowered IR.
- `rustlyn emit artifacts\out\add\add.bc --out artifacts\scratch\shared-nativeaot\add.dll` produces a managed output assembly.
- `scripts\Test-NativeHostParity.ps1 -Sample add -Configuration Release -LlvmRoot D:\opt\llvm -SkipBuild -SkipSdkPackage` passes with the shared DLL layout when `D:\opt\llvm` points at LLVM 22.1.4.

Version caveat: the LLVM package used by `RUSTLYN_LLVM_ROOT` must be new enough to read bitcode emitted by the installed Rust compiler. The v2026.1.0 llvm-prebuilt package provides LLVM 22.1.4, which is compatible with rustc 1.95's LLVM 22.1.2 bitcode.

## Rust FFI caller spike

`native\rustlyn-nativeaot-smoke` is a dependency-free Rust caller for the shared-library ABI. It dynamically loads the NativeAOT library by path, resolves `rustlyn_emit` / `rustlyn_free`, sends UTF-8 JSON options, copies the returned JSON into Rust-owned memory, and frees the native buffer through `rustlyn_free`.

Example:

```powershell
cargo run --manifest-path .\native\rustlyn-nativeaot-smoke\Cargo.toml -- `
  --library .\dotnet\backend\src\Rustlyn.NativeAot\bin\Release\net10.0\win-x64\publish\rustlyn_nativeaot.dll `
  --input .\artifacts\out\add\add.bc `
  --output $env:TEMP\rustlyn-add.dll `
  --llvm-root D:\opt\llvm
```

Current validation:

- Success path: emitted the `add` sample through `rustlyn_emit` and returned result JSON with `"success":true`.
- Failure path: a missing bitcode input returned exit code `1`, result JSON with `"success":false`, and a diagnostic from the managed export.

## Static-link spike

`native\rustlyn-nativeaot-static-smoke` proves the Windows x64 static-link path. It links directly against `rustlyn_nativeaot.lib` plus the NativeAOT runtime pack libraries and `bootstrapperdll.obj`.

Publish the static library:

```powershell
dotnet publish .\dotnet\backend\src\Rustlyn.NativeAot\Rustlyn.NativeAot.csproj `
  -c Release `
  -r win-x64 `
  --self-contained `
  -p:PublishAot=true `
  -p:NativeLib=Static
```

Set the required linker inputs and build/run the Rust smoke:

```powershell
$env:RUSTLYN_NATIVEAOT_LIB_DIR = (Resolve-Path .\dotnet\backend\src\Rustlyn.NativeAot\bin\Release\net10.0\win-x64\publish).Path
$nugetRoot = (dotnet nuget locals global-packages -l) -replace "^global-packages:\s*", ""
$env:RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR = Join-Path $nugetRoot "microsoft.netcore.app.runtime.nativeaot.win-x64\10.0.5\runtimes\win-x64\native"

cargo run --manifest-path .\native\rustlyn-nativeaot-static-smoke\Cargo.toml -- `
  --input .\artifacts\out\add\add.bc `
  --output $env:TEMP\rustlyn-add-static.dll `
  --llvm-root D:\opt\llvm
```

Current validation: the statically linked Rust executable emitted the `add` sample and returned result JSON with `"success":true`.

Important linker note: linking only `rustlyn_nativeaot.lib` is not enough. On Windows x64, the Rust build must also link the NativeAOT runtime pack libraries and `bootstrapperdll.obj`; otherwise it either fails with unresolved `Rh*`/Win32 symbols or crashes during startup before returning JSON. The smoke crate's `build.rs` wires those inputs from `RUSTLYN_NATIVEAOT_RUNTIME_LIB_DIR`.

## First build seam

`Rustlyn.Backend` now has a `RustlynBackendIncludeOptionalBindings` MSBuild property. The default product build keeps optional Avalonia and PowerShell binding glue enabled. Setting the property to `false` excludes those optional generated glue files and project references, which gives the NativeAOT work a narrower backend build to harden before introducing FFI.

## Analyzer validation seam

The SDK trim/AOT analyzers can already be run against the narrowed core build without a NativeAOT toolchain:

```powershell
dotnet build .\dotnet\backend\src\Rustlyn.Backend\Rustlyn.Backend.csproj `
  -c Release `
  -p:RustlynBackendIncludeOptionalBindings=false `
  -p:IsAotCompatible=true `
  -p:IsTrimmable=true
```

The recorded findings live in [`nativeaot-aot-analyzer-baseline.md`](nativeaot-aot-analyzer-baseline.md) and are the first acceptance check for the NativeAOT spike. The baseline confirms the remaining findings are dominated by the `Rustlyn.Bindings` scanner still present in the core graph, which makes the DTO/scanner split (work item 1) the highest-leverage next step.
