# Binding Generation Policy

Rustlyn should prefer generated bindings for public .NET APIs. Handwritten bridges are allowed only when the code models runtime ownership, Rust compiler/runtime behavior, Rust `std` ABI/layout compatibility, or framework lifecycle bootstrapping.

## Classifications

| Classification | Keep handwritten? | Applies to |
| --- | --- | --- |
| `generated-candidate` | No, replace with generated manifests when supported | Public .NET API calls, BCL helper glue, sample extern declarations |
| `core-specialized` | Yes | object, exception, and task handle stores; ownership/release semantics; UTF string copy primitives |
| `std-abi-specialized` | Yes, but generate registration/forwarding where possible | Rust `std` layout writes, `std::fmt` argument decoding, path buffer/slice ABI shims |
| `runtime-specialized` | Yes | allocator, panic, memory, atomic, vector, numeric, and entry-wrapper runtime semantics |
| `framework-bootstrap` | Yes for lifecycle only | Avalonia app startup and generated callback discovery |
| `temporary-specialized` | Only with a documented generated replacement path | narrow fixtures that unblock a test before the generator supports the shape |

## Current surfaces

| Surface | Classification | Replacement direction |
| --- | --- | --- |
| `BindingSurface.CreateTinyBclSurface()` BCL entries | `generated-candidate` | Runtime reference manifests should generate public .NET API glue. |
| `LoweredAssemblyEmitter.RuntimeBridgeAliases` generated binding entries | `generated-candidate` | Binding manifests should own `rustlyn_bindgen_*` symbol mapping. |
| `RuntimeBridgeHelpers` BCL forwarding methods | `generated-candidate` | Generated managed glue should call public .NET APIs directly. |
| `Rustlyn.Os.HostEnvironment`, `HostPath`, `HostFileSystem` length/copy pairs | `generated-candidate` | A UTF-8 adapter policy should generate pointer/length and length/copy wrappers. |
| `ManagedInteropRuntime` handle storage | `core-specialized` | Keep as the runtime ownership primitive used by generated wrappers. |
| Rust `std` path/io/env/time helpers | `std-abi-specialized` | Keep ABI/layout behavior handwritten in `RuntimeBridgeHelpers`; register symbols through `RustStdShimManifest`. |
| `Rustlyn.AvaloniaSupport.AvaloniaBridge` app startup | `framework-bootstrap` | Keep startup specialized; generate controls/properties/events later. |
| direct `samples/dotnet_runtime_*` extern declarations | `generated-candidate` | Import generated projection modules once equivalent wrappers exist. |

## Gate rule

New handwritten bridge symbols must be classified in this document before they are added. If they are public .NET API calls rather than ownership/runtime/ABI/bootstrap code, they should be generated from a manifest instead.

## Sample migration rule

New samples that demonstrate ordinary public .NET API calls should use generated projection modules, following `samples/generated_bindings_hello` and `samples/generated_bindings_lousygrep`. Direct `extern "C"` declarations for .NET APIs should be limited to legacy regression fixtures, runtime ABI fixtures, or slices whose generated projection shape is not available yet. Once an equivalent generated module exists, the direct extern sample should be converted or retired instead of duplicated.

## Avalonia projection boundary

`Rustlyn.AvaloniaSupport.AvaloniaBridge` remains `framework-bootstrap` only for application lifetime, generated-module discovery, and dispatching Rust callbacks into the UI startup flow. Avalonia controls, properties, methods, routed events, and ordinary object APIs should not be added to that bridge as bespoke helpers.

The generated path for Avalonia should be a separate projection pack after these policies are stable:

| Avalonia surface | Classification | Generated prerequisite |
| --- | --- | --- |
| control constructors and regular methods | `generated-candidate` | object-handle ownership and constructor policy |
| gettable/settable properties | `generated-candidate` | property projection and nullable/object result policy |
| events and delegates | `generated-candidate` | RAII subscription tokens and delegate trampoline policy |
| app builder/startup lifecycle | `framework-bootstrap` | keep specialized |
