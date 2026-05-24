# Rustlyn Roadmap

This document charts the forward direction for Rustlyn — a Rust-to-.NET translation pipeline that compiles Rust crates into managed .NET assemblies via LLVM bitcode.

## Origins & Inspiration

Rustlyn is heavily inspired by Eric Sink's [SourceGear Rust.NET SDK](https://ericsink.com/entries/sg_rust_dotnet_preview.html) (2020–2021), which demonstrated that translating Rust-produced LLVM bitcode into managed CIL is a viable approach. The original experiment shipped as `SourceGear.Rust.NET.Sdk` on NuGet and proved the core idea with F#, Mono.Cecil, LLVMSharp, xargo-era sysroot building, and a fake-linker handoff.

Rustlyn reconstructed and modernized that approach:

- .NET 10 with `System.Reflection.Metadata` (no Mono.Cecil dependency)
- C# backend (no F# dependency)
- LLVM 20 / current stable+nightly Rust toolchains
- `cargo -Z build-std` with LTO staticlib instead of xargo/fake-link
- Metadata-driven binding generation instead of static checked-in crate snapshots
- Sample-driven development with executable regression fixtures

**Parity with the original experiment is complete.** The [historical parity record](sourcegear-parity-roadmap.md) documents what was recovered and how. Everything below is new ground.

## Current Capabilities

| Area | Status |
| --- | --- |
| LLVM IR → CIL translation | Arithmetic, logic, shifts, comparisons, control flow, phi, switches, structs, arrays, tuples, function pointers, atomics, floating point, saturating math, bit ops, pointers, GEP, wide integers, trait objects, closures |
| Cargo integration | Direct bitcode emission, LTO staticlib for cross-crate, `build-std` for core/alloc/std |
| Generated .NET bindings | Console, File, Directory, Path, String, Environment — metadata-driven glue |
| Runtime support | `Rustlyn.Runtime` (LLVM semantics), `Rustlyn.Os` (std compat), `Rustlyn.Interop` (managed handles) |
| MSBuild SDK | Library/binary/build-std/generated-cargo/NuGet-packaged builds via `.rsproj` |
| Desktop bridge | Avalonia GUI from Rust through explicit bridge calls |

## Forward Roadmap

### Deeper Rust Pattern Coverage

These are fundamental Rust patterns that go beyond what the original experiment demonstrated:

| Feature | Challenge | Target |
| --- | --- | --- |
| Enums with data | `Option<T>`, `Result<T,E>`, tagged unions — discriminant checks, payload extraction, match dispatch | Sample + backend support |
| `String` / `Vec<T>` | Heap-owning types with Drop — alloc tracking and destructor semantics in IL | Runtime integration |
| Iterator chains | map/filter/fold/collect — optimized to complex phi/loop/inline-closure IR | Backend IR coverage |
| `?` operator | Result unwrapping with early return — branch-heavy or invoke/landingpad IR | Control flow expansion |
| Async/await | State machine generators (`Future::poll`) — completely novel for Rust→.NET | Research + prototype |
| Generic collections | HashMap, BTreeMap through LTO — monomorphization of stdlib internals | LTO + backend coverage |

### Automated Binding Generation

Move from hand-curated bindings to a fully automated pipeline:

| Feature | Description |
| --- | --- |
| Assembly scanner | Accept any .NET assembly + type filter → produce complete extern/glue/wrapper bindings |
| Instance methods | `self` handle passing for method calls on .NET objects |
| Constructors & overloads | Type construction and overload resolution in generated Rust API |
| Generic types | `List<T>`, `Dictionary<K,V>` with monomorphized Rust wrappers |
| Events & delegates | .NET event subscription and delegate invocation through Rust closures |
| `rustlyn bindgen` CLI | First-class command: `rustlyn bindgen <assembly> --types "Namespace.*" --out dir/` |

### Production Quality

| Feature | Description |
| --- | --- |
| Debug info | Portable PDB emission with source mapping back to `.rs` files |
| Native AOT | Trimming-safe and AOT-compatible emitted assemblies |
| Cross-platform | Linux and macOS CI validation (backend is already pure .NET) |
| Benchmarks | Performance comparison: translated assembly vs native Rust vs C# |
| CLI polish | Structured JSON output, progress, `--verbose`/`--quiet` |
| SDK publish | Public NuGet publication of `Rustlyn.Sdk` with proper metadata |

### Novel Capabilities

Things no prior Rust-to-.NET project has attempted:

| Feature | Description |
| --- | --- |
| Bidirectional interop | .NET code calling Rust-defined types — translated Rust structs exposed as .NET classes with IDisposable |
| Crate-as-NuGet | `rustlyn pack <crate>` → NuGet package with translated assembly + generated bindings |
| Incremental translation | Per-function IR cache for fast re-translation of changed code |
| Multi-target | Single crate translated for multiple TFMs (net8.0, net10.0) |
| WASM target | Rust crate → .NET assembly → Blazor WebAssembly component |
| IDE integration | Language server with cross-boundary diagnostics and navigation |

## Design Principles

These carry forward from the foundation work:

1. **Sample-first** — every new capability starts with a failing fixture, not an abstract design
2. **Cargo-first** — the build pipeline uses standard Cargo; MSBuild is an optional wrapper
3. **Fix the owner** — patch the direct responsible component, don't layer workarounds
4. **Executable validation** — every promoted feature has a smoke test or regression assertion
5. **LTO staticlib only** — no fake-link, no custom targets, no xargo (permanently decided)
6. **Runtime layering** — LLVM semantics in Runtime, OS compat in Os, handle ABI in Interop
7. **Metadata-driven bindings** — generate from reflection, don't maintain static snapshots

## Related Documents

- [Historical parity record](sourcegear-parity-roadmap.md) — what was recovered from the original experiment
- [Revived design](revived-design.md) — current architecture details
- [Original Eric Sink design](original-eric-sink-design.md) — how the 2020–2021 SDK worked
- [Fake-link decision](sourcegear-fake-link-decision.md) — why LTO staticlib replaced fake-link (permanent)
- [Interop handle ABI](interop-handle-abi.md) — managed object/exception handle contract
