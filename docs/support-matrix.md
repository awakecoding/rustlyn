# Rustlyn support matrix

This matrix distinguishes fixture-backed behavior from preview and planned work. It is intentionally conservative: a feature is **supported** only when it is exercised by committed samples, backend assertions, smoke scripts, or CI. Capabilities that exist in prototype form but are not broadly enforced are marked **preview** or **fixture-only**.

## Status legend

| Status | Meaning |
| --- | --- |
| Supported | Covered by committed samples/tests or smoke scripts and expected to keep working for the documented shape. |
| Preview | Implemented enough to experiment with, but coverage or diagnostics are not yet broad enough for general use. |
| Fixture-only | A narrow sample proves one path; adjacent real-world uses may still be unsupported. |
| Planned | Roadmap item with design direction but no general support claim. |
| Unsupported | Known gap that should fail clearly once no-silent-success diagnostics are in place. |

## Compiler and backend

| Area | Status | Evidence | First-class gap |
| --- | --- | --- | --- |
| LLVM bitcode inspection | Supported | `Rustlyn.Tool inspect`, `BitcodeArtifactInspector`, smoke scripts | Broader structured output and diagnostics. |
| LLVM text lowering | Preview | `LoweredIrLowerer.cs` and many lowered-shape tests | Replace regex/text parsing with semantic LLVM traversal that preserves datalayout, attributes, metadata, volatile/orderings, address spaces, and exception constructs. |
| Arithmetic, comparisons, branches, phi, loops | Supported | Primitive samples and backend regression harness | Keep as the fast baseline while unsupported fallbacks are removed. |
| Switch lowering | Preview | Raw switch lowering path and `switch_control` fixture | Model switch as a typed lowered instruction instead of a raw-instruction special case. |
| Structs, arrays, tuples, aggregate returns | Preview | Focused aggregate samples | Add a datalayout-driven layout/ABI engine and remove packing/sret heuristics. |
| Function pointers and indirect calls | Preview | Function-pointer samples | Formalize calli signatures, closure/vtable ABI, and unresolved-call diagnostics. |
| Atomics | Fixture-only | `atomicrmw`/`cmpxchg` lowering paths and atomics samples | Implement LLVM ordering/fence/volatile semantics with `Interlocked`/`Volatile` or fail clearly. |
| Floating point and math intrinsics | Preview | Float/math fixtures and intrinsic dispatch | Expand libm coverage and edge-case conformance. |
| Wide integers and non-standard widths | Preview | `i128_ops` and integer-width handling | Formalize exact integer storage/signature behavior, including unsigned intent. |
| Panic abort | Preview | Selected panic/div/rem paths | Stabilize panic runtime and entrypoint behavior. |
| Panic unwind and cleanup/drop unwinding | Unsupported | `invoke`, `landingpad`, `resume`, personality functions are not modeled as first-class lowered instructions | Add exception regions, cleanup/drop glue, and Rust/.NET panic-exception boundary policy. |
| Debug information | Preview | `emit --pdb` emits Portable PDBs | Replace synthetic function-index sequence points with LLVM `!dbg` source documents, lines, locals, scopes, inlining, and async state. |
| Incremental translation | Preview | `--cache` records function hashes and prints stats | Make the cache affect actual translation/emission; hash full IR content, not just instruction kinds. |

## Rust language and library surface

| Area | Status | Evidence | First-class gap |
| --- | --- | --- | --- |
| `no_std` arithmetic/control samples | Supported | `samples/add` and primitive fixtures | Maintain as regression seed. |
| `core`/`alloc`/`std` build-std rungs | Preview | build-std samples and MSBuild build-std script | Expand `std` compatibility beyond targeted fixtures. |
| Enums, `Option`, `Result`, match | Preview | enum and option/result samples | Register all advanced samples in main test/smoke loops and formalize niche/discriminant layout. |
| `?` error propagation | Fixture-only | `samples/error_propagation` exists | Promote into required tests and add panic/unwind/drop edge cases. |
| Iterators and closures | Fixture-only | iterator/closure samples | Add move/ref capture, drop-sensitive captures, and more adapter chains. |
| Generic collections | Fixture-only | `samples/generic_collections` exists | Promote tests, then expand to std collections through LTO. |
| Async state machines | Fixture-only | `samples/async_state_machine` is a manual state-machine fixture | Add real `async fn`, `Future::poll`, `Pin`, wakers, and executor boundaries. |
| `Vec`/`String` ownership and drop | Fixture-only | `samples/string_vec_ops` exists | Stabilize allocator/drop/runtime behavior and register fixture coverage. |
| Threads, mutexes, channels, TLS | Planned | No required regression coverage | Add std concurrency fixtures after atomics/orderings are real. |
| Common ecosystem crates | Planned | `dep_heavy` proves a cross-crate workload | Add workspace/features/build-script/common-crate tiers subject to dependency policy. |

## .NET integration and runtime

| Area | Status | Evidence | First-class gap |
| --- | --- | --- | --- |
| Managed object, string, exception handles | Supported for current ABI | `Rustlyn.Interop` and generated-binding fixtures | Keep ABI versioned and stable as binding breadth grows. |
| Runtime/OS helper split | Preview | `Rustlyn.Runtime`, `Rustlyn.Os`, facade methods in `RuntimeBridgeHelpers` | Move remaining helpers to owned runtime projects and expand only from failing fixtures. |
| Generated BCL bindings | Preview | `Rustlyn.Bindings`, generated lousygrep, scanner/bindgen commands | Generalize constructors, overloads, generics, delegates, events, reverse callbacks, and exception policy. |
| Avalonia bridge | Fixture-only | `samples/avalonia_hello` and support assembly | It proves explicit bridge calls, not generated Avalonia bindings, XAML, data binding, or general delegates. |
| Rust APIs exposed as .NET types | Planned | Roadmap item | Design metadata, ownership, `IDisposable`, and exception semantics. |

## Tooling and product surface

| Area | Status | Evidence | First-class gap |
| --- | --- | --- | --- |
| CLI translate/inspect/lower/emit/invoke | Supported for documented examples | `Rustlyn.Tool` and smoke scripts | Add command framework, per-command help, JSON/progress/verbosity, and complete exit-code docs. |
| CLI pack | Preview | `rustlyn pack` emits assembly/PDB and `.nuspec` | Produce real `.nupkg`/`.snupkg` with source/PDB/runtime dependencies and package validation. |
| MSBuild SDK | Preview | `.rsproj` scripts and packable SDK facade | Add incremental `Inputs`/`Outputs`, design-time behavior, PDB/cache properties, reference/target-path compatibility, and structured logging. |
| Templates | Planned | No template package | Add `dotnet new` templates for library, console, generated-Cargo, and binding-enabled projects. |
| IDE/language service | Planned | Project-system metadata only | Add language server, diagnostics, navigation, and generated binding awareness. |
| Cross-platform CI | Preview | CI builds on Windows/Linux/macOS and smokes `add` | Expand CI tiers beyond the `add` bitcode smoke path. |
| Native AOT/trimming | Planned | Design intent | Add emitted-assembly compatibility tests before claiming support. |

## Immediate promotion targets

1. Register existing advanced samples in the backend and smoke matrix or explicitly mark them preview-only.
2. Add no-silent-success diagnostics so unsupported IR cannot pass through as `nop`, zero, or stubbed methods.
3. Replace README capability claims with links to this matrix when behavior is preview, fixture-only, or planned.
4. Expand CI from the `add` smoke path to a small but representative required tier.

