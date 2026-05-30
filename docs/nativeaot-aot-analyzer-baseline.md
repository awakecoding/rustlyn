# NativeAOT / Trimming Analyzer Baseline

This baseline records the SDK trim/AOT analyzer findings for the backend core
build seam. It is the first measurable acceptance check for the NativeAOT spike
(see [NativeAOT Rust host plan](nativeaot-rust-host.md)). The goal of that spike
is to drive the **core-graph** findings to zero (or to justified suppressions)
before attempting a real NativeAOT shared-library publish.

This is analyzer-only validation. It does not require a NativeAOT toolchain or a
trimmed publish; it runs the static IL2xxx/IL3xxx analyzers that ship with the
.NET SDK against the backend core.

## How to reproduce

```powershell
dotnet build .\dotnet\backend\src\Rustlyn.Backend\Rustlyn.Backend.csproj `
  -c Release `
  -p:RustlynBackendIncludeOptionalBindings=false `
  -p:IsAotCompatible=true `
  -p:IsTrimmable=true
```

`RustlynBackendIncludeOptionalBindings=false` excludes the Avalonia/PowerShell
generated glue and their project references, narrowing the graph to the core
`lower`/`emit` path that the future NativeAOT library must harden.

## Current findings (baseline)

Counts are approximate and dominated by the reflection-heavy `Rustlyn.Bindings`
scanner, which is still referenced by the core graph because
`EmitOptions.BindingManifests` is typed against `Rustlyn.Bindings.BindingManifestDocument`.

| Code   | Approx. count | Dominant source | Meaning |
| ------ | ------------- | --------------- | ------- |
| IL2075 | ~40 | `Rustlyn.Bindings` scanner | `Type.GetMethod/GetConstructor` on values without `DynamicallyAccessedMembers`. |
| IL2026 | ~32 | scanner + JSON + assembly load | Calls into `RequiresUnreferencedCode` members. |
| IL2070 | ~30 | `Rustlyn.Bindings` scanner | `Type.GetMethods/GetProperties/GetEvents` on un-annotated `Type` parameters. |
| IL3050 | ~12 | `JsonSerializer.Serialize` | Reflection-based JSON requires `RequiresDynamicCode`. |
| IL2057 | ~8 | `Rustlyn.Bindings.Tool` | `Type.GetType(string)` with non-literal type names. |
| IL3000 | ~2 | `Rustlyn.Bindings` `ExternalPackageBindingSurfaces` | `Assembly.Location` under single-file. |

### Core-emitter findings already addressed

- `LoweredAssemblyEmitter.ResolveSupportAssemblyPath` — the `Assembly.Location`
  access (IL3000) is intentional for the framework-dependent host and is
  covered by an `AppContext.BaseDirectory` fallback. It carries a justified
  `UnconditionalSuppressMessage` for IL3000 and no longer appears in the core
  build output.

## Interpretation

The overwhelming majority of findings originate in `Rustlyn.Bindings`, not in
the core emitter. The highest-leverage next decoupling step is to split the
pure DTO records (`BindingManifestDocument` and friends) out of the
scanner-heavy `Rustlyn.Bindings` project into an AOT-safe contracts assembly,
so the core `lower`/`emit` graph no longer pulls in the reflection scanner,
`System.Reflection.MetadataLoadContext`, and the Avalonia package references.

Remaining genuine core-path concerns to address during the spike:

- `RuntimeBridgeHelpers.IsDefaultI32Formatter` uses
  `AppDomain.CurrentDomain.GetAssemblies()` + `Type.GetMethods` at runtime in
  emitted-program support code.
- The `lower`/`emit` path uses reflection-based `System.Text.Json` (IL2026 /
  IL3050); a source-generated serializer is required for AOT.
- `LoweredAssemblyInvoker` loads emitted assemblies via
  `AssemblyLoadContext.LoadFromStream` + reflection invoke (`invoke` command);
  this is acceptable for the host CLI but must stay outside any AOT static-lib
  surface.
