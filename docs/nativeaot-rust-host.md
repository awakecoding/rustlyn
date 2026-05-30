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

1. **Split data contracts from scanners.** Move or duplicate only the binding manifest DTOs needed by `EmitOptions` and `LoweredAssemblyEmitter` into an AOT-safe assembly. Leave `BindingSurface`, scanners, `MetadataLoadContext`, Avalonia, and PowerShell manifest factories in `Rustlyn.Bindings`.
2. **Stop auto-creating optional manifests in the emitter.** The emitter should consume explicit binding manifests from options or generated static data. Avalonia and PowerShell should be supplied by the CLI or host when requested, not pulled in by the core emitter.
3. **Replace reflection-based runtime bridge map construction.** `BuildRuntimeBridgeMap` currently reflects over `RuntimeBridgeHelpers` and re-creates binding manifests at runtime. Replace this with generated/static symbol-to-helper tables that can be rooted for trimming.
4. **Replace support-assembly discovery.** `CopyRuntimeSupportAssemblies` currently uses `typeof(...).Assembly.Location`. Under NativeAOT or single-file assumptions, support assets need to come from explicit host-provided paths, a manifest, or a release layout known to the Rust host.
5. **Add a shared-library NativeAOT spike.** Start with a `.dll`/`.so`/`.dylib` export before static linking. Use `[UnmanagedCallersOnly]`, coarse JSON options, source-generated JSON serializers, top-level exception capture, and a paired `rustlyn_free`.
6. **Add a Rust FFI caller spike.** Validate UTF-8 path/options passing, result ownership, structured diagnostics, and failure behavior.
7. **Attempt static library linkage last.** Only after the shared library path works should the Rust executable link a NativeAOT `.lib`/`.a`, because static NativeAOT linkage needs platform-specific linker configuration and runtime/system libraries.

## Intended FFI shape

Keep the ABI small and stable. Do not mirror C# classes over FFI.

```c
int32_t rustlyn_emit(
    const uint8_t* options_json,
    int32_t options_len,
    uint8_t** result_json,
    int32_t* result_len);

void rustlyn_free(uint8_t* ptr);
```

The options JSON should carry input bitcode or lowered IR path, output assembly path, PDB flag, strict-mode flag, LLVM/root behavior if the C# side still owns that step, and explicit runtime/support asset paths. The result JSON should carry success/failure, diagnostics, produced files, copied runtime assets, and stable error codes.

## Non-goals for the first spike

- Do not static-link first.
- Do not port the whole CLI to Rust before proving the library boundary.
- Do not include Avalonia, PowerShell, or binding scanners in the NativeAOT library.
- Do not require generated managed assemblies to be NativeAOT themselves. Compiler-host NativeAOT and output-assembly NativeAOT are separate workstreams.

## First build seam

`Rustlyn.Backend` now has a `RustlynBackendIncludeOptionalBindings` MSBuild property. The default product build keeps optional Avalonia and PowerShell binding glue enabled. Setting the property to `false` excludes those optional generated glue files and project references, which gives the NativeAOT work a narrower backend build to harden before introducing FFI.
