# NativeAOT Rust host plan

This document captures the lowest-risk path toward a single Rust `rustlyn` executable that reuses the managed Rustlyn backend through a NativeAOT library and FFI boundary.

The goal is not to rewrite the backend in Rust. The goal is to keep the C# lowerer/emitter where it is strongest, make the non-optional compiler core NativeAOT-friendly, and let a Rust host own CLI, Cargo/Rust toolchain orchestration, and LLVM integration.

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

void rustlyn_free(uint8_t* ptr);
```

The initial shared-library spike implements this shape in `Rustlyn.NativeAot` with cdecl exports:

| Export | Return code | Ownership |
| --- | --- | --- |
| `rustlyn_emit(options_json, options_len, result_json, result_len)` | `0` success, `1` operation failure with result JSON, `-1` invalid ABI arguments, `-2` result allocation failure | On non-negative returns, `*result_json` is owned by the caller and must be released with `rustlyn_free`. On negative returns, `*result_json == null` and `*result_len == 0`. |
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

## Shared-library spike

`Rustlyn.NativeAot` publishes a NativeAOT shared library named `rustlyn_nativeaot`:

```powershell
dotnet publish .\dotnet\backend\src\Rustlyn.NativeAot\Rustlyn.NativeAot.csproj `
  -c Release `
  -r win-x64 `
  --self-contained `
  -p:PublishAot=true
```

Build this project standalone. It references `Rustlyn.Backend` with `RustlynBackendIncludeOptionalBindings=false`; adding it to a broader solution build before the backend is split can create duplicate backend build graphs with different generated glue inputs.

Current validation:

- `dotnet publish` produces `dotnet\backend\src\Rustlyn.NativeAot\bin\Release\net10.0\win-x64\publish\rustlyn_nativeaot.dll`.
- A PowerShell P/Invoke smoke called `rustlyn_emit` against `artifacts\out\add\add.bc` and produced a managed output assembly with `code=0`.
- Publish currently succeeds with trim/AOT warnings. The remaining warnings are concentrated in `RuntimeBridgeHelpers.IsDefaultI32Formatter` and the still-referenced `Rustlyn.Bindings` graph.

Current packaging caveat: even with optional backend bindings disabled, the publish directory still includes Avalonia/Skia/PowerShell-adjacent native assets because `Rustlyn.Backend` still references `Rustlyn.Bindings`, and `Rustlyn.Bindings` references scanner/support projects and packages. Splitting the binding-manifest DTOs from the scanner-heavy project is required before this shared library becomes a realistic release artifact.

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
