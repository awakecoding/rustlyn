# Rebuilding Rust-to-.NET IL Today

## Scope

This note documents two things:

1. What the 2020-2021 SourceGear Rust.NET SDK actually shipped and how it worked.
2. How to rebuild the same kind of project from the ground up in 2026.

The evidence for the reverse-engineering in this workspace comes from:

- `artifacts/sdk-0.1.5/extracted/` for the extracted NuGet package
- `artifacts/decompiled/` for decompiled managed assemblies
- Eric Sink's blog posts:
  - `sg_rust_dotnet_preview.html`
  - `llama_rust_013.html`

The package analyzed here is `SourceGear.Rust.NET.Sdk` version `0.1.5`, while the blog posts mainly discuss `0.1.0` and `0.1.3`.

## Executive Summary

The original project was not a Rust-to-C# transpiler.

It was a four-stage pipeline:

1. Use MSBuild as the outer developer experience.
2. Translate `.rsproj` metadata back into a synthetic Cargo project.
3. Compile Rust and its sysroot to LLVM bitcode instead of native code.
4. Translate LLVM bitcode into a managed .NET assembly, with helper runtime libraries filling in LLVM and Win32 behavior.

The 2021 design was technically credible and more advanced than a demo, but it was tightly coupled to:

- nightly Rust
- custom target JSON behavior
- custom sysroot building
- LLVM 10-era assumptions
- a hand-rolled runtime bridge for both LLVM semantics and OS APIs

If rebuilding it today, the core idea is still viable, but the implementation should be modernized around:

- `cargo -Z build-std` instead of a xargo-style sysroot bootstrap
- a dedicated cargo subcommand or rustc driver, not an MSBuild-first architecture
- a clearer internal IR between LLVM and CIL
- generated BCL bindings from reference assemblies, not a checked-in giant static snapshot
- a smaller, more explicit runtime surface
- .NET 10 and current LLVM/Rust toolchains

## What The Original SDK Shipped

## Package layout

The NuGet package contains five main parts:

1. MSBuild glue in `build/` and `Sdk/`
2. `tools/rsbuild/` as the orchestrator and LLVM-to-CIL compiler
3. `tools/rsfakelink/` as the fake linker used during Cargo builds
4. `tools/crates/` containing Rust helper crates and runtime assemblies
5. Bundled native LLVM binaries for Windows, macOS, and Ubuntu 18.04 x64

The `rsbuild` dependency graph shows the main compiler stack clearly:

- `FSharp.Core`
- `LLVMSharp`
- `Mono.Cecil`
- `build_sysroot`
- `exec`
- `llvm2cil`
- `sgllvm`
- `sgcil`
- `sgopt`
- `sgrt`

That means the shipped implementation was primarily F# on .NET 5, using LLVMSharp to read LLVM IR and Mono.Cecil to emit managed assemblies.

## MSBuild layer

The MSBuild SDK is intentionally thin.

`SourceGear.Rust.NET.Core.Sdk.targets` defines `CoreCompile`, computes a few paths, and runs:

`dotnet rsbuild.dll <assembly info> <crate refs> <project refs> ...`

It also injects references to two runtime assemblies into the final .NET project:

- `sgwin32.dll`
- `sgm.dll`

So the MSBuild side is only a facade. The real logic lives in `rsbuild.dll`.

## rsbuild.dll

`rsbuild` is the actual build pipeline controller.

From decompilation, its top-level responsibilities are:

- `write_cargo_toml(...)`
- `write_cil(...)`
- `main(...)`

What `main(...)` does:

1. Parse arguments passed from MSBuild.
2. Create the intermediate directory.
3. Emit a custom target JSON through `build_sysroot.write_custom_target(...)`.
4. Build a custom sysroot with `build_std_sysroot_with_cargo(...)`.
5. Generate a synthetic Cargo project for the user crate.
6. Optionally add a `dotnet` Rust crate that exposes pre-generated .NET bindings.
7. Invoke Cargo to build the user crate as a `cdylib` for the custom target.
8. Read the resulting `.bc` file.
9. Convert that bitcode into a `.dll` by calling the LLVM-to-CIL backend.

This matches the blog posts almost exactly.

## build_sysroot.dll

The sysroot builder hardcodes the target base name:

`aarch64-sourcegear-windows`

It does three important things:

1. Runs `rustc --print sysroot`
2. Synthesizes a Cargo project that depends on `std` from `rust-src`
3. Builds that sysroot using Cargo and copies the results into the layout rustc expects

The target JSON it writes is the key hack.

Important flags:

- `obj-is-bitcode = true`
- `requires-lto = true`
- `dll-suffix = ".bc"`
- `only-cdylib = true`
- `linker = "dotnet"`
- `pre-link-args = { "gcc" : [ ".../rsfakelink.dll" ] }`
- `llvm-target = "aarch64-pc-windows-msvc"`
- `os = "windows"`

This is how the pipeline persuades Rust to stop at LLVM bitcode and pretend the world is Windows, even though the real runtime behavior will be implemented on top of .NET.

## rsfakelink.dll

`rsfakelink` is exactly what its name says.

It scans linker arguments, ignores many of them, and has only one real implemented success path:

- if there is exactly one `.o` input and no `.rlib` inputs, copy that `.o` to the requested destination

That destination is the final `.bc` output. No real native link step happens.

## LLVM-to-CIL compiler

The compiler is split across three assemblies:

- `sgllvm.dll`: thin helpers for reading LLVM modules, functions, globals, types, operands, instructions, and sizes
- `sgcil.dll`: the CIL type model and IL instruction model
- `llvm2cil.dll`: the high-level translation pipeline that builds the final managed assembly

### sgllvm.dll

This is a convenience wrapper over LLVMSharp. It exposes helpers such as:

- `read_file`
- `parse_bitcode`
- `get_functions`
- `get_globals`
- `get_instructions`
- `get_operands`
- `get_struct_element_types`
- `get_size_in_bits`

So this layer is not the compiler; it is the LLVM inspection utility layer.

### sgcil.dll

This assembly defines the managed-side type and instruction vocabulary.

Important responsibilities:

- map LLVM first-class types to .NET/Cecil types
- synthesize managed value types for arrays, structs, vectors, and odd-width integers
- turn abstract `MyInstruction` nodes into actual Cecil IL instructions

Notable behavior from decompilation:

- vectors are mapped to `Vector64<T>`, `Vector128<T>`, or `Vector256<T>` when possible
- unsupported vector layouts fall back to synthesized explicit-layout value types
- non-standard integer widths are represented as explicit-layout byte structs
- structs and arrays are materialized as explicit-layout value types with field offsets

In other words, `sgcil` is where LLVM's data model is translated into CLR-representable shapes.

### llvm2cil.dll

This is the real code generator.

Its main entry point is:

- `gen_assembly(...)`

That method:

1. Creates a new managed assembly with Mono.Cecil.
2. Creates the container type that will hold generated methods and globals.
3. Imports runtime helpers from `sgrt`.
4. Imports copy/reference assemblies such as `sgwin32`, `sgm`, and optional `dotnet` glue.
5. Creates global fields for LLVM globals.
6. Creates managed methods for LLVM functions.
7. Emits function bodies instruction by instruction.
8. Emits a generated static constructor.
9. Emits a managed `Main` wrapper if the output kind is `Exe`.

This split is sound. `sgllvm` reads LLVM, `sgcil` defines the mapping vocabulary, and `llvm2cil` owns assembly generation.

## Rust-side binding layer

The package also ships a large amount of Rust code.

### sgrust_core

`sgrust_core` defines the basic ABI conventions. The important concepts are:

- `SGRustValue`
- `SGRustObjectHandle`
- object lifetime via `__drop_handle`

The bridge model is simple and blunt:

- primitive values are converted to and from `i64`
- reference-like values are represented as opaque handles
- type lookups are done through generated `*_get_type_handle()` externs

This is the base interop contract between translated Rust code and generated .NET glue.

### rs_dotnet

The package ships 104 generated Rust crates under `tools/crates/rs_dotnet`.

These crates wrap pieces of the .NET Base Class Library and expose them as Rust modules containing extern calls and handle wrappers.

For example, a generated wrapper for `System.IO.Directory.CreateDirectory(...)` looks like:

- a Rust function that accepts Rust-side handle wrappers
- a hidden extern named like `System_IO_Directory_DirectoryInfo__CreateDirectory__1__String`
- an out-parameter for thrown exceptions
- conversion from raw handle to Rust wrapper type

There is also an umbrella `dotnet` crate that re-exports all these generated crates as a Rust-friendly namespace tree.

This was the project's strategy for "Rust code calling .NET APIs": not dynamic FFI, but generated Rust wrappers over a known managed surface.

## Runtime support assemblies

The generated code is not self-sufficient. It relies on runtime helper assemblies.

### sgrt.dll

This is the LLVM semantic support runtime.

From reflection, it includes helpers for:

- atomics
- overflow intrinsics
- saturating arithmetic
- vector operations
- memory ops (`memcpy`, `memmove`)
- comparison helpers
- tracing/debugging support
- entry wrappers like `my_main_rs(...)`

This runtime exists because many LLVM IR operations do not map directly to a single CLR instruction.

### sgwin32.dll

This is a Win32-on-.NET compatibility layer.

It exports methods named like Win32 functions, including:

- `CreateFileW`
- `GetFileAttributesW`
- `MapViewOfFile`
- `TlsAlloc`
- `WaitForSingleObject`
- `ReadFile`
- `WriteFile`

The point is not to P/Invoke the real Windows API everywhere. The point is to satisfy the expectations of Rust's standard library and crates that were compiled as if they were targeting Windows.

This matches Eric Sink's blog explanation exactly.

### sgm.dll

This is a much smaller math helper assembly providing functions like:

- `acos`
- `atan2`
- `hypot`
- `log`
- `tan`

It appears to exist mainly to cover math operations that the generated code expects to call through a stable runtime surface.

## What Made The Original Design Work

The design worked because it chose a very specific seam:

- let Rust do parsing, macro expansion, type checking, monomorphization, and codegen to LLVM IR
- replace only the native backend and runtime assumptions

That avoids building a Rust frontend from scratch.

The cost is that you inherit LLVM IR semantics, Rust's codegen habits, and a large amount of runtime emulation work.

## What Would Not Be Rebuilt The Same Way Today

Some parts of the original design were clever in 2021 but should be replaced in a 2026 implementation.

### 1. Do not build sysroot the xargo-era way

The original SDK manually synthesized a Cargo sysroot project and copied artifacts into a rustc-style directory layout.

Today, prefer `cargo +nightly -Z build-std=std,panic_abort` instead.

That still requires nightly, but it is a more direct fit for modern Cargo workflows and avoids a lot of hand-built sysroot plumbing.

### 2. Do not make MSBuild the primary control plane

MSBuild is useful for .NET developer experience, but it is a poor core orchestration layer for a Rust compiler pipeline.

Today the right shape is:

- primary driver: `cargo dotnet-il build`
- optional wrapper: an MSBuild SDK that delegates to that driver

That keeps Rust developers in Cargo while still enabling .NET integration.

### 3. Do not ship a giant static snapshot of generated BCL crates by default

The checked-in `rs_dotnet` tree is useful as proof that the idea works, but it is operationally expensive.

Today the better design is:

- generate bindings from reference assemblies at build time or release time
- version them by target framework
- keep a smaller core pre-generated set for bootstrap only

### 4. Make the internal compiler pipeline explicit

The original code clearly has layers, but the boundaries are mostly implicit in helper functions and shared Cecil state.

A rebuild should formalize:

- LLVM reader IR
- lowered CLR-compatible IR
- CIL emission backend

That makes testing, debugging, and incremental bring-up much easier.

### 5. Reduce platform fiction

Pretending to be Windows was a practical shortcut to get `std` and common crates working.

Today, if the first target is managed .NET on multiple host OSes, define that target explicitly:

- a custom target triple for managed CLR output
- a custom OS family or ABI tag if needed
- an intentional runtime contract instead of a disguised Windows one where practical

That said, keeping a Windows-compat shim for `std` compatibility may still be the fastest way to reach parity early.

## Recommended 2026 Architecture

## High-level shape

If the goal is to rebuild the same kind of project, the recommended architecture is:

1. Cargo-first frontend
2. Nightly Rust for `-Z build-std` and custom target support
3. LLVM bitcode as the compiler handoff format
4. A dedicated .NET-based compiler backend
5. A small managed runtime for LLVM semantics and object/exception glue
6. Generated Rust bindings for selected .NET reference assemblies
7. Optional MSBuild SDK only as an integration shell

## Proposed repository layout

```text
repo/
  cargo-dotnet-il/
    src/
  compiler/
    llvm-reader/
    lower-clr/
    il-model/
    il-backend/
  runtime/
    sgrt/
    sgos/
    sginterop/
  bindings/
    generator/
    generated/
  sdk/
    msbuild/
  samples/
    hello/
    std_fs/
    regex/
    clap/
```

## Recommended implementation languages

### Cargo frontend

Use Rust.

Rationale:

- easiest integration with Cargo metadata and rustup toolchains
- natural place for `cargo dotnet-il build`
- simpler developer story for Rust users

### Compiler backend

Use .NET 10 with either F# or C#.

My recommendation:

- use F# if the team is comfortable with compiler-style code and algebraic data types
- use C# if maintainability by general .NET engineers matters more than concise compiler transforms

The original code benefited from F# for compiler transformations. That is still true.

### Assembly writer

Start with Mono.Cecil.

Rationale:

- proven fit for the original design
- easy method and type construction
- easier than raw `System.Reflection.Metadata` for this use case

Reevaluate later if you need lower-level PE surgery or unusual metadata control.

### LLVM access

Use a current LLVM binding layer on .NET. The closest continuation of the original design is LLVMSharp.

If LLVMSharp becomes a maintenance bottleneck, an alternative is to define a very small native shim over libLLVM and call only the subset you need from managed code.

## Recommended build pipeline today

## Phase 1: Cargo driver

Implement a cargo subcommand:

`cargo dotnet-il build`

Responsibilities:

- resolve target framework and configuration
- create an intermediate directory
- write a custom target JSON if still needed
- invoke Cargo with the required nightly flags
- locate the resulting `.bc` artifact
- invoke the managed backend compiler
- optionally emit an `.deps.json` / runtimeconfig companion if needed

## Phase 2: Rust build strategy

Use nightly Rust, but build std the modern way:

```text
cargo +nightly build \
  -Z build-std=std,panic_abort \
  --target managed-dotnet.json
```

You still need to validate the exact flag combination for whole-program bitcode emission, but the design target should be:

- one bitcode artifact representing the final crate
- dependencies and std built consistently under the same target contract

Early on, it is acceptable to retain the fake-linker trick if that is the fastest path to a single `.bc` file.

## Phase 3: LLVM reader

Read LLVM bitcode and lift it into a compiler-owned IR that captures:

- functions
- globals
- blocks
- values
- control flow
- memory ops
- intrinsics
- type layouts

Do not emit CIL directly from raw LLVM nodes everywhere.

This is the single biggest structural improvement I would make over the 2021 codebase.

## Phase 4: CLR-lowering IR

Create a lowering pass that turns LLVM-specific constructs into a CLR-aware instruction set.

Examples:

- explicit layout structs
- vector values
- odd-width integers
- helper-runtime calls for unsupported arithmetic and memory operations
- bridge calls for exception and object handling
- thunk generation for Rust `main`

By the time code reaches the IL backend, it should already know whether it becomes:

- direct IL
- a synthesized value type
- a runtime helper call
- a bridge import

## Phase 5: IL backend

Emit:

- one managed assembly per compiled crate output
- a container type for generated functions and globals
- a generated `.cctor` for global initialization
- an optional managed entrypoint wrapper for executables

Use three explicit reference categories:

1. copied runtime helpers such as `sgrt`
2. referenced framework glue assemblies such as generated BCL bindings
3. imported user-visible managed assemblies, if you choose to support them

## Phase 6: Runtime layer

Split the runtime intentionally.

### `sgrt`

Own only LLVM semantic support:

- memops
- atomics
- overflow helpers
- saturating arithmetic
- vector helpers
- entry wrappers

### `sginterop`

Own object-handle and exception-handle behavior:

- object retain/release policy
- thrown exception marshalling
- type handles
- string conversion helpers

### `sgos`

Own OS emulation or compatibility behavior.

Initially this may still expose Win32-like APIs if that is the quickest route to Rust `std` compatibility. Over time, it should become a cleaner portability/runtime layer rather than a pure Win32 facade.

## Phase 7: BCL binding generator

Do not hand-maintain a huge set of Rust wrappers.

Instead build a generator that:

1. loads .NET reference assemblies for a target framework
2. walks public types, methods, constructors, fields, and enums
3. emits Rust crates grouped by assembly
4. emits one umbrella crate for ergonomic re-exports
5. emits the managed glue assembly that satisfies the expected extern symbol names

That gives you a repeatable answer to:

- new target frameworks
- API drift
- trimmed bootstrap surfaces

## Suggested milestones

## Milestone 0: Hello world without std

Goal:

- compile a `no_std` Rust function to a managed DLL
- call it from a tiny managed test harness

This validates the basic LLVM-to-CIL pipeline before touching sysroot and OS compatibility.

## Milestone 1: Executable entrypoint

Goal:

- support `fn main()` and `Result<()>`-style wrappers
- generate a managed `Main` wrapper

## Milestone 2: `core` + `alloc`

Goal:

- handle enough language/runtime semantics for collections and owned strings
- validate memory layout and runtime helper coverage

## Milestone 3: `std` filesystem and environment

Goal:

- `std::fs`
- environment variables
- paths
- console IO

This is where an `sgos` or `sgwin32` equivalent becomes necessary.

## Milestone 4: Common crate ecosystem

Test against representative crates:

- `regex`
- `clap`
- `serde`
- `tokio` only after threading and async support are mature

## Milestone 5: Managed API consumption from Rust

Goal:

- generate Rust wrappers for a targeted .NET API subset
- successfully call managed framework APIs from translated Rust code

## Hard technical problems you should expect

## 1. LLVM IR is a low-level contract, not a friendly language boundary

You will spend most of the time handling:

- layout edge cases
- intrinsics
- vectors
- weird integer widths
- aliasing and address computations
- exception and unwind behavior

The original SDK already contains evidence of all of those problems.

## 2. Rust std compatibility is mostly a runtime project

Getting `std` to compile is not the same thing as making it behave correctly.

File IO, environment access, process behavior, time, TLS, locking, and path handling all become part of your managed runtime contract.

## 3. Odd-width integers and vectors are unavoidable

The original compiler had to synthesize types for them. A modern rebuild will too.

## 4. Exceptions, panics, and unwinding need a deliberate story

Decide early whether you will support:

- panic abort only
- limited exception propagation
- full unwind interoperability

The fastest credible path is still panic-abort plus managed exception marshaling at known bridge points.

## 5. Debuggability matters more than elegance

You need first-class dumps for:

- input LLVM IR
- lowered compiler IR
- generated IL
- generated assembly metadata
- runtime helper call traces

Without that, bring-up will be slow and ambiguous.

## What I would build first if starting now

If rebuilding this from zero today, I would do it in this order:

1. A cargo subcommand that can produce a single `.bc` artifact for a tiny crate.
2. A managed backend that turns a single Rust function with integers and locals into a .NET DLL.
3. A tiny `sgrt` with only memory ops, arithmetic overflow helpers, and entry wrappers.
4. Explicit-layout struct and array lowering.
5. Global initialization and executable entrypoint support.
6. `core` and `alloc` bring-up.
7. A minimal OS compatibility layer for `std` filesystem and environment APIs.
8. Generated .NET binding support for a very small framework subset.
9. Only then an MSBuild SDK wrapper.

That ordering keeps the hardest part in focus: the compiler and runtime contract.

## Recommended product shape

The most sustainable product shape is probably:

- `cargo dotnet-il` for Rust-native workflows
- a reusable managed backend compiler library
- optional MSBuild SDK for `.rsproj` support
- generated target-framework-specific binding packages

Do not make the MSBuild SDK the center of the system. Make it one frontend.

## Bottom line

The original SourceGear project worked by replacing the last mile of Rust code generation, not by replacing Rust itself.

That is still the right idea today.

If rebuilt in 2026, the best version of this project would:

- keep the LLVM-bitcode handoff
- keep a managed backend compiler
- keep a small runtime layer
- modernize sysroot handling with `-Z build-std`
- move to a cargo-first toolchain
- generate BCL bindings instead of shipping a frozen hand-built universe
- formalize the compiler's internal IR and testing story

That would preserve the spirit of the 2021 SDK while making it much more maintainable and realistic to evolve.

## Concrete prototype attempt plan

This section is the execution plan I would actually use to attempt a fresh prototype now.

The goal is not to rebuild the whole 2021 SDK in one shot.

The goal is to prove one hard claim first:

- a small Rust crate can be compiled today into LLVM bitcode
- that bitcode can be read by a managed backend
- the backend can emit a runnable .NET assembly

Everything else should be deferred until that claim is true.

## Prototype definition

The first prototype should only support:

- one tiny Rust crate
- `no_std`
- integers, locals, branches, calls, and returns
- a generated managed class containing translated functions
- a tiny runtime with only the helpers actually needed by the test cases

It should explicitly not support yet:

- `std`
- Cargo dependencies beyond the bootstrap crate
- async
- threads
- panics beyond abort-only policy
- managed API bindings
- an MSBuild SDK

## Success criteria for prototype v0

Prototype v0 is successful if all of the following are true:

1. A Rust crate can be compiled reproducibly to a bitcode artifact.
2. The managed backend can load that bitcode and enumerate functions and globals.
3. At least one function like `add(i32, i32) -> i32` is emitted into a .NET DLL.
4. A .NET test harness can invoke the translated function and get the correct result.
5. The compiler can dump enough diagnostics to explain failures without using a debugger.

If any of those fail, do not move on to `std` support.

## Recommended initial stack

Use this stack unless a blocker appears:

- Rust nightly, pinned by date
- `rust-src`
- .NET 10 SDK
- Mono.Cecil for IL and assembly emission
- LLVMSharp or an equivalent managed libLLVM binding
- PowerShell plus small scripts for orchestration on Windows

Nightly is still the safe assumption because:

- `-Z build-std` is still unstable
- custom target JSON support is still unstable in Cargo
- you may still need nightly-only target plumbing even if the first milestone avoids `std`

## Repository plan

Create the repo in layers, with each layer testable alone.

```text
repo/
  rust/
    cargo-dotnet-il/
    support/
      sgrust_core/
  dotnet/
    backend/
      llvm-reader/
      clr-lower/
      il-emitter/
    runtime/
      sgrt/
    tests/
      harness/
  samples/
    add/
    control_flow/
```

### `cargo-dotnet-il`

Responsibilities:

- invoke Cargo or rustc with the right toolchain
- collect the produced bitcode artifact
- invoke the managed backend
- write outputs to a predictable intermediate directory

### `llvm-reader`

Responsibilities:

- open the bitcode module
- list functions, globals, blocks, and instructions
- expose only the LLVM subset needed by the compiler

### `clr-lower`

Responsibilities:

- convert raw LLVM structures into compiler-owned lowered nodes
- decide what becomes direct IL vs. runtime helper calls

### `il-emitter`

Responsibilities:

- define types and methods in the target assembly
- emit IL with Cecil
- write the `.dll`

### `sgrt`

Responsibilities:

- host only the minimum helper surface required by v0 and v1

## Phase-by-phase plan

## Phase A: establish the Rust artifact contract

Objective:

- prove exactly which Rust command line yields a stable bitcode artifact for a minimal crate

Tasks:

1. Create a tiny `no_std` sample crate with one exported unmangled function.
2. Verify `rustc` or `cargo rustc` can emit LLVM bitcode for it.
3. Freeze the exact command line and output location contract.
4. Capture the emitted bitcode and inspect it with LLVM tooling or the managed reader.

Deliverable:

- one known-good sample and one known-good build command

Gate:

- do not proceed until the bitcode artifact is deterministic and understood

## Phase B: build a module inspector

Objective:

- read a bitcode module from .NET and print a trustworthy structural summary

Tasks:

1. Load the module through libLLVM.
2. Enumerate functions and globals.
3. For each function, list blocks and instruction opcodes.
4. Dump LLVM types and layout information needed later.

Deliverable:

- a CLI like `backend inspect sample.bc`

Gate:

- the inspector output should be sufficient to debug lowering without external tools

## Phase C: lower a tiny subset to CLR IR

Objective:

- define the smallest useful internal IR and lower simple functions into it

Initial supported constructs:

- integer constants
- integer add/sub/mul
- comparisons
- conditional branches
- local variables
- function parameters
- direct calls
- returns

Deliverable:

- a textual dump of lowered IR for simple samples

Gate:

- at least two sample crates must lower without ad hoc special cases in the emitter

## Phase D: emit runnable IL

Objective:

- emit a managed assembly for the narrowed subset

Tasks:

1. Create a container type.
2. Emit translated methods.
3. Map LLVM integer types to CLR integer types.
4. Emit direct IL for the subset from Phase C.
5. Save the assembly and run it from a .NET harness.

Deliverable:

- one `.dll` that can be loaded and invoked by a test harness

Gate:

- passing end-to-end sample tests from source Rust to running managed code

## Phase E: add only the minimum runtime helpers

Objective:

- introduce runtime support only when IL cannot express a required semantic

Likely first helpers:

- entry wrappers for executable tests
- `memcpy` and `memmove`
- overflow helpers for instructions that do not map cleanly

Deliverable:

- a tiny `sgrt` assembly with a deliberately tiny surface

Rule:

- every new helper must be justified by a failing test or unsupported LLVM instruction

## Immediate work plan

If starting today, I would sequence the first attempt like this:

### Current repo status

As of May 2026, this workspace has a working milestone-0 prototype for the first sample matrix.

What is implemented now:

- `scripts/Build-SampleBitcode.ps1` builds LLVM bitcode for the current samples
- `RustMcil.Tool inspect` reads `.bc` artifacts and prints module summaries
- `RustMcil.Tool lower` lowers LLVM text IR into a compiler-owned typed IR dump
- `RustMcil.Tool emit` emits a runnable managed DLL with Mono.Cecil
- `RustMcil.Tool invoke` emits and immediately executes a chosen generated method
- `RustMcil.Tool translate` builds a Cargo library crate to LLVM bitcode and emits a managed DLL in one step
- `scripts/Test-Smoke.ps1 -Sample add,sub,max,call_chain,global_init` validates inspect, lower, emit, and execution end to end
- `scripts/Test-Smoke.ps1 -Mode Cargo -Sample add,sub,max,call_chain,global_init` validates the crate-path `translate` workflow end to end across the same sample matrix

Current supported sample matrix:

1. `add_i32`
2. `and_i32`
3. `shl_i32`
4. `ashr_i32`
5. `lshr_i32`
6. `or_i32`
7. `xor_i32`
8. `mul_i32`
9. `div_i32`
10. `div_i64`
11. `div_u32`
12. `rem_i32`
13. `rem_u32`
14. `rem_i64`
15. `rem_u64`
16. `sext_i32_to_i64`
17. `select_i32`
18. `mask_lt_i64`
19. `merge_call_i32` through a direct branch/call merge and an optimized Cargo `phi`
20. `xor_fold_i32` through a direct stack-based loop and a de-vectorized optimized Cargo loop with scalar `phi` and `poison`
21. `xor_fold_i32` through the standard optimized Cargo vector loop with `<4 x i32>` vector `phi`, vector `add`/`xor`, and `llvm.vector.reduce.xor.v4i32`
22. `or_fold_i32` through a direct stack-based loop and the standard optimized Cargo vector loop with `<4 x i32>` vector `phi`, vector `add`/`or`, and `llvm.vector.reduce.or.v4i32`
23. `and_fold_i32` through a direct stack-based loop and the standard optimized Cargo vector loop with `<4 x i32>` vector `phi`, vector `add`/`and`, and `llvm.vector.reduce.and.v4i32`
24. `max_xor_i16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` signed-max main vector loop, a `<4 x i16>` signed-max vector epilog, `llvm.smax.v8i16`, `llvm.vector.reduce.smax.v8i16`, `llvm.smax.v4i16`, `llvm.vector.reduce.smax.v4i16`, and broadcast prep lowered as raw `insertelement`/`shufflevector`
25. `max_xor_u16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` unsigned-max main vector loop, a `<4 x i16>` unsigned-max vector epilog, `llvm.umax.v8i16`, `llvm.vector.reduce.umax.v8i16`, `llvm.umax.v4i16`, `llvm.vector.reduce.umax.v4i16`, and broadcast prep lowered as raw `insertelement`/`shufflevector`
26. `min_xor_u16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` unsigned-min main vector loop, a `<4 x i16>` unsigned-min vector epilog, `llvm.umin.v8i16`, `llvm.vector.reduce.umin.v8i16`, `llvm.umin.v4i16`, `llvm.vector.reduce.umin.v4i16`, and broadcast prep lowered as raw `insertelement`/`shufflevector`
27. `xor_fold_u16` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<8 x i16>` xor-reduction main vector loop, a `<4 x i16>` xor-reduction vector epilog, `llvm.vector.reduce.xor.v8i16`, `llvm.vector.reduce.xor.v4i16`, and xor-specific epilog seed prep lowered as raw `insertelement`/`shufflevector`
28. `min_xor_i16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` signed-min main vector loop, a `<4 x i16>` signed-min vector epilog, `llvm.smin.v8i16`, `llvm.vector.reduce.smin.v8i16`, `llvm.smin.v4i16`, `llvm.vector.reduce.smin.v4i16`, and broadcast prep lowered as raw `insertelement`/`shufflevector`
29. `xor_fold_i16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` xor-reduction main vector loop, a `<4 x i16>` xor-reduction vector epilog, `llvm.vector.reduce.xor.v8i16`, `llvm.vector.reduce.xor.v4i16`, and xor-specific epilog seed prep lowered as raw `insertelement`/`shufflevector`
30. `sum_xor_i16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` add-reduction main vector loop, a `<4 x i16>` add-reduction vector epilog, `llvm.vector.reduce.add.v8i16`, `llvm.vector.reduce.add.v4i16`, and epilog seed prep lowered as raw `insertelement`/`shufflevector`
31. `or_fold_i16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` or-reduction main vector loop, a `<4 x i16>` or-reduction vector epilog, `llvm.vector.reduce.or.v8i16`, `llvm.vector.reduce.or.v4i16`, and epilog seed prep lowered as raw `insertelement`/`shufflevector`
32. `and_fold_i16` through a direct stack-based loop and an optimized Cargo path with a `<8 x i16>` and-reduction main vector loop, a `<4 x i16>` and-reduction vector epilog, `llvm.vector.reduce.and.v8i16`, `llvm.vector.reduce.and.v4i16`, and all-ones epilog seed prep lowered as raw `insertelement`/`shufflevector`
33. `max_xor_i32` through a direct stack-based loop and the standard optimized Cargo vector loop with `<4 x i32>` lane-wise `llvm.smax.v4i32` and `llvm.vector.reduce.smax.v4i32`
34. `min_xor_i32` through a direct stack-based loop and the standard optimized Cargo vector loop with `<4 x i32>` lane-wise `llvm.smin.v4i32` and `llvm.vector.reduce.smin.v4i32`
35. `max_xor_u32` through a direct stack-based loop and the standard optimized Cargo vector loop with `<4 x i32>` lane-wise `llvm.umax.v4i32` and `llvm.vector.reduce.umax.v4i32`
36. `min_xor_u32` through a direct stack-based loop and the standard optimized Cargo vector loop with `<4 x i32>` lane-wise `llvm.umin.v4i32` and `llvm.vector.reduce.umin.v4i32`
37. `sum_fold_i32` through a direct stack-based loop and an optimized Cargo arithmetic-series rewrite with `i33` `zext`, `mul`, `lshr`, and `trunc`
38. `sum_xor_i32` through a direct stack-based loop and the standard optimized Cargo vector loop with `llvm.vector.reduce.add.v4i32`
39. `sum_xor_i64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lanes and `llvm.vector.reduce.add.v2i64`
40. `xor_fold_i64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lanes and `llvm.vector.reduce.xor.v2i64`
41. `or_fold_i64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lanes and `llvm.vector.reduce.or.v2i64`
42. `and_fold_i64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lanes and `llvm.vector.reduce.and.v2i64`
43. `max_xor_i64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lane-wise `llvm.smax.v2i64` and `llvm.vector.reduce.smax.v2i64`
44. `min_xor_i64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lane-wise `llvm.smin.v2i64` and `llvm.vector.reduce.smin.v2i64`
45. `max_xor_u64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lane-wise `llvm.umax.v2i64` and `llvm.vector.reduce.umax.v2i64`
46. `min_xor_u64` through a direct stack-based loop and the standard optimized Cargo vector loop with `<2 x i64>` lane-wise `llvm.umin.v2i64` and `llvm.vector.reduce.umin.v2i64`
47. `eq_i32`
48. `ne_i32`
49. `sub_i64`
50. `max_i32`
51. `max_eq_i32`
52. `max_u32`
53. `max_eq_u32`
54. `min_i32`
55. `min_eq_i32`
56. `min_u32`
57. `min_eq_u32`
58. `call_chain_i32` plus `add_one_i32`
59. `read_global_i32`
60. `read_second_i32` from a constant global integer array
61. `second_of_pair_i32` from a stack-local integer array
62. `second_field_i32` from a by-value `repr(C)` pair on both the direct-bitcode and Cargo release paths
63. `second_field_i64` from a by-value `repr(C)` pair lowered on the direct-bitcode path as a `ptr` parameter plus fixed-offset field load
64. `sum_xor_u16` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<8 x i16>` add-reduction main vector loop, a `<4 x i16>` add-reduction vector epilog, `llvm.vector.reduce.add.v8i16`, `llvm.vector.reduce.add.v4i16`, and epilog seed prep lowered as raw `insertelement`/`shufflevector`
65. `or_fold_u16` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<8 x i16>` or-reduction main vector loop, a `<4 x i16>` or-reduction vector epilog, `llvm.vector.reduce.or.v8i16`, `llvm.vector.reduce.or.v4i16`, and epilog seed prep lowered as raw `insertelement`/`shufflevector`
66. `and_fold_u16` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<8 x i16>` and-reduction main vector loop, a `<4 x i16>` and-reduction vector epilog, `llvm.vector.reduce.and.v8i16`, `llvm.vector.reduce.and.v4i16`, and all-ones epilog seed prep lowered as raw `insertelement`/`shufflevector`
67. `xor_fold_u32` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<4 x i32>` xor-reduction main vector loop, scalar tail recovery through a typed `phi`, and `llvm.vector.reduce.xor.v4i32`
68. `sum_xor_u32` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<4 x i32>` add-reduction main vector loop, scalar tail recovery through a typed `phi`, and `llvm.vector.reduce.add.v4i32`
69. `or_fold_u32` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<4 x i32>` or-reduction main vector loop, scalar tail recovery through a typed `phi`, and `llvm.vector.reduce.or.v4i32`
70. `and_fold_u32` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<4 x i32>` and-reduction main vector loop, scalar tail recovery through a typed `phi`, `llvm.vector.reduce.and.v4i32`, and all-ones vector initialization through `splat (i32 -1)`
71. `xor_fold_u64` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<2 x i64>` xor-reduction main vector loop, scalar tail recovery through a typed `phi`, and `llvm.vector.reduce.xor.v2i64`
72. `sum_xor_u64` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<2 x i64>` add-reduction main vector loop, scalar tail recovery through a typed `phi`, and `llvm.vector.reduce.add.v2i64`
73. `or_fold_u64` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<2 x i64>` or-reduction main vector loop, scalar tail recovery through a typed `phi`, and `llvm.vector.reduce.or.v2i64`
74. `and_fold_u64` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with a `<2 x i64>` and-reduction main vector loop, scalar tail recovery through a typed `phi`, `llvm.vector.reduce.and.v2i64`, and all-ones vector initialization through `splat (i64 -1)`
75. `xor_fold_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with `llvm.fshl.i32`, raw multi-line `switch` dispatch in `vector.ph`, a `<16 x i8>` xor-reduction merge in `middle.block`, a `<4 x i8>` xor-reduction epilog, and `llvm.vector.reduce.xor.v16i8` plus `llvm.vector.reduce.xor.v4i8`
76. `sum_xor_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with `llvm.fshl.i32`, raw multi-line `switch` dispatch in `vector.ph`, a `<16 x i8>` add-reduction merge in `middle.block`, a `<4 x i8>` xor-then-add epilog, and `llvm.vector.reduce.add.v16i8` plus `llvm.vector.reduce.add.v4i8`
77. `or_fold_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path with `llvm.fshl.i32`, raw multi-line `switch` dispatch in `vector.ph`, a `<16 x i8>` or-reduction merge in `middle.block`, a `<4 x i8>` or-reduction epilog, and `llvm.vector.reduce.or.v16i8` plus `llvm.vector.reduce.or.v4i8`
78. `and_fold_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks and an optimized Cargo path that skips the main vector loop, carries an all-ones epilog seed through raw `insertelement`/`shufflevector`, uses a `<4 x i8>` and-reduction epilog, and ends at `llvm.vector.reduce.and.v4i8`
79. `xor_fold_i8` through a direct stack-based loop with signed `icmp slt` induction checks and an optimized Cargo path with a signed positive-trip-count guard, `llvm.fshl.i32`, raw multi-line `switch` dispatch in `vector.ph`, a `<8 x i8>` xor-reduction merge in `middle.block`, a `<4 x i8>` xor-reduction epilog, and `llvm.vector.reduce.xor.v8i8` plus `llvm.vector.reduce.xor.v4i8`
80. `sum_xor_i8` through a direct stack-based loop with signed `icmp slt` induction checks and an optimized Cargo path with a signed positive-trip-count guard, `llvm.fshl.i32`, raw multi-line `switch` dispatch in `vector.ph`, a `<8 x i8>` add-reduction merge in `middle.block`, a `<4 x i8>` xor-then-add epilog, and `llvm.vector.reduce.add.v8i8` plus `llvm.vector.reduce.add.v4i8`
81. `or_fold_i8` through a direct stack-based loop with signed `icmp slt` induction checks and an optimized Cargo path with a signed positive-trip-count guard, `llvm.fshl.i32`, raw multi-line `switch` dispatch in `vector.ph`, a `<8 x i8>` or-reduction merge in `middle.block`, a `<4 x i8>` or-reduction epilog, and `llvm.vector.reduce.or.v8i8` plus `llvm.vector.reduce.or.v4i8`
82. `and_fold_i8` through a direct stack-based loop with signed `icmp slt` induction checks and an optimized Cargo path with a signed positive-trip-count guard that skips the main vector body, carries an all-ones epilog seed through raw `insertelement`/`shufflevector`, uses a `<4 x i8>` and-reduction epilog, and ends at `llvm.vector.reduce.and.v4i8`
83. `max_xor_i8` through a direct stack-based loop with signed `icmp slt` induction checks, an `i8::MIN` accumulator seed, and a branchy signed compare/update, plus an optimized Cargo path with a signed positive-trip-count guard, a raw `switch` in `vector.ph`, a `<16 x i8>` signed-max main loop via `llvm.smax.v16i8` and `llvm.vector.reduce.smax.v16i8`, a `<8 x i8>` xor-plus-signed-max epilog via `llvm.smax.v8i8` and `llvm.vector.reduce.smax.v8i8`, and a scalar tail through `llvm.smax.i8`
84. `min_xor_i8` through a direct stack-based loop with signed `icmp slt` induction checks, an `i8::MAX` accumulator seed, and a branchy signed compare/update, plus an optimized Cargo path with a signed positive-trip-count guard that skips the main vector body, a `<8 x i8>` xor-plus-signed-min epilog via `llvm.smin.v8i8` and `llvm.vector.reduce.smin.v8i8`, and a scalar tail through `llvm.smin.i8`
85. `max_xor_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks, a zero accumulator seed, and a branchy unsigned compare/update, plus an optimized Cargo path with `llvm.fshl.i32`, a raw `switch` in `vector.ph`, a `<16 x i8>` unsigned-max main loop via `llvm.umax.v16i8` and `llvm.vector.reduce.umax.v16i8`, a `<4 x i8>` xor-plus-unsigned-max epilog via `llvm.umax.v4i8` and `llvm.vector.reduce.umax.v4i8`, and a scalar tail through `llvm.umax.i8`
86. `min_xor_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks, a `u8::MAX` accumulator seed, and a branchy unsigned compare/update, plus an optimized Cargo path that skips the main vector body, carries an all-ones `<4 x i8>` epilog seed through raw `insertelement`/`shufflevector`, uses `llvm.umin.v4i8` with `llvm.vector.reduce.umin.v4i8` in the epilog, and finishes through a scalar `llvm.umin.i8` tail
87. `min_xor_sub_i8` through a direct stack-based loop with signed `icmp slt` induction checks, an `i8::MAX` accumulator seed, a xor-plus-sub transform, and a branchy signed compare/update, plus an optimized Cargo path with a signed positive-trip-count guard, a raw `switch` in `vector.ph`, a `<16 x i8>` signed-min main loop via `llvm.smin.v16i8` and `llvm.vector.reduce.smin.v16i8`, a `<8 x i8>` xor-plus-add-`-7` epilog via `llvm.smin.v8i8` and `llvm.vector.reduce.smin.v8i8`, and a scalar tail through `llvm.smin.i8`; the hardened regression cases now also pin `n = 100`, where the direct emitted/invoked path widens the low byte as `249` while the optimized Cargo helper path preserves the signed `-7`
88. `min_add_sub_i8` through a direct stack-based loop with signed `icmp slt` induction checks, an `i8::MAX` accumulator seed, a wrapping add-then-sub transform, and a branchy signed compare/update, plus an optimized Cargo path with a signed positive-trip-count guard, a raw `switch` in `vector.ph`, a `<16 x i8>` signed-min main loop via `llvm.smin.v16i8` and `llvm.vector.reduce.smin.v16i8` whose second argument is an inline vector literal `<i8 38, ..., i8 53>`, a `<8 x i8>` add-38 epilog via `llvm.smin.v8i8` and `llvm.vector.reduce.smin.v8i8`, and a scalar tail through `llvm.smin.i8`
89. `max_add_sub_i8` through a direct stack-based loop with signed `icmp slt` induction checks, an `i8::MIN` accumulator seed, a wrapping add-then-sub transform, and a branchy signed compare/update, plus an optimized Cargo path with a signed positive-trip-count guard, a raw `switch` in `vector.ph`, a `<16 x i8>` signed-max main loop via `llvm.smax.v16i8` and `llvm.vector.reduce.smax.v16i8`, a `<8 x i8>` add-38 epilog via `llvm.smax.v8i8` and `llvm.vector.reduce.smax.v8i8`, and a scalar tail through `llvm.smax.i8`
90. `max_xor_sub_i8` through a direct stack-based loop with signed `icmp slt` induction checks, an `i8::MIN` accumulator seed, a xor-plus-sub transform, and a branchy signed compare/update, plus an optimized Cargo path with a signed positive-trip-count guard, a `<16 x i8>` signed-max main loop via `llvm.smax.v16i8` and `llvm.vector.reduce.smax.v16i8`, a pair of `<16 x i8>` vector-literal `select` merges in `vector.ph`, a `<8 x i8>` xor-plus-add-`-7` epilog via `llvm.smax.v8i8` and `llvm.vector.reduce.smax.v8i8`, and a scalar tail through `llvm.smax.i8`
91. `min_xor_sub_u8` through a direct stack-based loop with unsigned `icmp ult` induction checks, a `u8::MAX` accumulator seed, a xor-plus-sub transform, and a branchy unsigned compare/update, plus an optimized Cargo path with a raw `switch` in `vector.ph`, a `<16 x i8>` unsigned-min main loop via `llvm.umin.v16i8` and `llvm.vector.reduce.umin.v16i8`, a `<4 x i8>` xor-plus-add-`-7` epilog via `llvm.umin.v4i8` and `llvm.vector.reduce.umin.v4i8`, and a scalar tail through `llvm.umin.i8`

Current emitter coverage in this repo:

- integer `add` and `sub`
- integer comparison lowering through direct `icmp eq`/`icmp ne` branching, signed relational `icmp slt`/`icmp sle`/`icmp sgt`/`icmp sge`, and unsigned relational `icmp ult`/`icmp ule`/`icmp ugt`/`icmp uge`; optimized boolean-to-integer equality results also lower through typed `zext`, signed widening casts now lower through typed `sext`, and optimized unsigned min/max route through `llvm.umin`/`llvm.umax`
- simple conditional value selection now also has direct branch-and-merge regression coverage plus optimized Cargo lowering through typed `select`
- signed comparison masks now also have direct branch-and-merge coverage plus optimized Cargo lowering through typed `sext i1 -> i64`
- scalar branch merges now also have optimized Cargo regression coverage through typed `phi`, with edge-based value copies in the IL emitter and a direct branch/call merge sample covering the same source-level shape before SSA formation
- de-vectorized optimized scalar loops now also have direct and Cargo regression coverage through `xor_fold_i32`, including integer `poison` placeholders, loop-metadata-tolerant branch parsing, and returning blocks that are not last in printed CFG order
- standard optimized loops now also have regression coverage through a vectorized `xor_fold_i32` sample, with `<4 x i32>` type preservation in the lowerer, vector literal and splat parsing, minimal vector `add`/`xor` emission, and `llvm.vector.reduce.xor.v4i32` support
- standard optimized or-reduction loops now also have regression coverage through a vectorized `or_fold_i32` sample, extending the same `<4 x i32>` path to vector `or` and `llvm.vector.reduce.or.v4i32`
- standard optimized and-reduction loops now also have regression coverage through a vectorized `and_fold_i32` sample, extending the same `<4 x i32>` path to vector `and` and `llvm.vector.reduce.and.v4i32`
- optimized 16-bit signed-max loops now also have regression coverage through `max_xor_i16`, extending the vector path down to a `<8 x i16>` main loop, a `<4 x i16>` vector epilog, raw `insertelement`/`shufflevector` broadcast handling, and scalar `llvm.smax.i16`
- optimized 16-bit unsigned-max loops now also have regression coverage through `max_xor_u16`, extending the vector path down to a `<8 x i16>` main loop, a `<4 x i16>` vector epilog, raw `insertelement`/`shufflevector` broadcast handling, and scalar `llvm.umax.i16`
- optimized 16-bit unsigned-min loops now also have regression coverage through `min_xor_u16`, extending the vector path down to a `<8 x i16>` main loop, a `<4 x i16>` vector epilog, raw `insertelement`/`shufflevector` broadcast handling, and scalar `llvm.umin.i16`
- optimized 16-bit unsigned xor-reduction loops now also have regression coverage through `xor_fold_u16`, reusing the same `<8 x i16>` and `<4 x i16>` helper path as signed xor-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- optimized 16-bit unsigned add-reduction loops now also have regression coverage through `sum_xor_u16`, reusing the same `<8 x i16>` and `<4 x i16>` helper path as signed add-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- optimized 16-bit unsigned or-reduction loops now also have regression coverage through `or_fold_u16`, reusing the same `<8 x i16>` and `<4 x i16>` helper path as signed or-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- optimized 16-bit unsigned and-reduction loops now also have regression coverage through `and_fold_u16`, reusing the same `<8 x i16>` and `<4 x i16>` helper path as signed and-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned xor-reduction loops now also have regression coverage through `xor_fold_u32`, reusing the same `<4 x i32>` helper path as signed xor-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned add-reduction loops now also have regression coverage through `sum_xor_u32`, reusing the same `<4 x i32>` helper path as signed add-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned or-reduction loops now also have regression coverage through `or_fold_u32`, reusing the same `<4 x i32>` helper path as signed or-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned and-reduction loops now also have regression coverage through `and_fold_u32`, reusing the same `<4 x i32>` helper path as signed and-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering and all-ones accumulator initialization
- standard optimized unsigned 64-bit xor-reduction loops now also have regression coverage through `xor_fold_u64`, reusing the same `<2 x i64>` helper path as signed xor-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned 64-bit add-reduction loops now also have regression coverage through `sum_xor_u64`, reusing the same `<2 x i64>` helper path as signed add-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned 64-bit or-reduction loops now also have regression coverage through `or_fold_u64`, reusing the same `<2 x i64>` helper path as signed or-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering
- standard optimized unsigned 64-bit and-reduction loops now also have regression coverage through `and_fold_u64`, reusing the same `<2 x i64>` helper path as signed and-reduction while also pinning the direct unoptimized `icmp ult` loop-header lowering and all-ones accumulator initialization
- optimized 8-bit unsigned xor-reduction loops now also have regression coverage through `xor_fold_u8`, extending the backend with `llvm.fshl.i32`, raw multi-line `switch` emission with phi-edge copies, and minimal `<16 x i8>` / `<4 x i8>` xor helper support while the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit unsigned add-reduction loops now also have regression coverage through `sum_xor_u8`, reusing the same `llvm.fshl.i32` plus raw-switch path while extending the backend to `<16 x i8>` vector add and `llvm.vector.reduce.add.v16i8` / `llvm.vector.reduce.add.v4i8`; the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit unsigned or-reduction loops now also have regression coverage through `or_fold_u8`, reusing the same `llvm.fshl.i32` plus raw-switch path while extending the backend to `<16 x i8>` / `<4 x i8>` vector or and `llvm.vector.reduce.or.v16i8` / `llvm.vector.reduce.or.v4i8`; the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit unsigned and-reduction loops now also have regression coverage through `and_fold_u8`, extending the same epilog-only `u8` path to `<4 x i8>` vector and plus `llvm.vector.reduce.and.v4i8`; this optimized shape bypasses the main vector loop entirely and still widens the managed emitted and invoked signature to `Int32`
- optimized 8-bit unsigned max loops now also have regression coverage through `max_xor_u8`, extending the backend to scalar `llvm.umax.i8`, `<16 x i8>` `llvm.umax.v16i8` plus `llvm.vector.reduce.umax.v16i8`, and `<4 x i8>` `llvm.umax.v4i8` plus `llvm.vector.reduce.umax.v4i8`; narrow integer call results now normalize before storing so Cargo-built `u8` max results like `199` and `255` stay widened as `Int32`
- optimized 8-bit unsigned min loops now also have regression coverage through `min_xor_u8`, extending the backend to scalar `llvm.umin.i8`, epilog `<4 x i8>` `llvm.umin.v4i8` plus `llvm.vector.reduce.umin.v4i8`, and inline unsigned 8-bit min helper comparisons; the optimized shape still bypasses the main vector body and the empty `u8::MAX` seed continues to widen as `-1`
- optimized 8-bit signed min loops now also have regression coverage through `min_xor_sub_i8`, extending the backend to the missing main-loop `<16 x i8>` `llvm.smin.v16i8` plus `llvm.vector.reduce.smin.v16i8`; this shape also required the phi-edge copier to tolerate duplicate identical incoming entries from the same source block, which shows up after the raw `switch` in `vector.ph` folds multiple cases into `middle.block`, signed narrow min/max helper call results to stop being re-masked as unsigned on the CLR stack, and direct sub-32-bit compare operands to be normalized before signed predicate emission so the negative minimum no longer collapses to `0`
- optimized 8-bit signed min loops now also have regression coverage through `min_add_sub_i8`, pinning the lowerer fix for call arguments whose values are inline vector literals; call argument parsing now splits only on top-level commas and reads the argument type prefix structurally instead of with a last-space heuristic, which preserves `<16 x i8> <i8 38, ..., i8 53>` as a single `llvm.smin.v16i8` operand for the emitter
- optimized 8-bit signed max loops now also have regression coverage through `max_add_sub_i8`, proving the existing signed `i8` max helpers also cover the adjacent add-then-sub transformed main-loop shape end to end: the optimized Cargo lowering keeps the raw `switch` in `vector.ph`, preserves two `<16 x i8>` main-loop phis seeded from inline `<i8 38, ..., i8 53>` and `<i8 54, ..., i8 69>` literals, reduces through `llvm.smax.v16i8` and `llvm.vector.reduce.smax.v16i8`, finishes the epilog through `llvm.smax.v8i8` / `llvm.vector.reduce.smax.v8i8`, and stays stable through `n = 100` on both the direct and Cargo paths
- optimized 8-bit signed max loops now also have regression coverage through `max_xor_sub_i8`, pinning the adjacent xor-plus-sub transformed main-loop shape and the lowerer fix for `select` operands whose values are inline vector literals; `select` parsing now follows the same structural top-level-comma splitting as calls, so `<16 x i8>` true and false operands with embedded commas survive lowering intact before `llvm.smax.v16i8` consumes them
- optimized 8-bit unsigned min loops now also have regression coverage through `min_xor_sub_u8`, extending the backend to the missing main-loop `<16 x i8>` `llvm.umin.v16i8` plus `llvm.vector.reduce.umin.v16i8` path while reusing the existing `<4 x i8>` epilog and scalar unsigned-min support; the root cause was that only the unsigned `v16i8` max helper/reduce pair had been wired, so the emitter now also generates and dispatches the matching unsigned `i8x16` min helper bodies
- optimized 8-bit signed xor-reduction loops now also have regression coverage through `xor_fold_i8`, reusing the same `llvm.fshl.i32` plus raw-switch path while extending the backend to recognize `<8 x i8>` vector literals, `<8 x i8>` vector xor, and `llvm.vector.reduce.xor.v8i8`; the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit signed add-reduction loops now also have regression coverage through `sum_xor_i8`, reusing the same `llvm.fshl.i32` plus raw-switch path while extending the backend to `<8 x i8>` vector add and `llvm.vector.reduce.add.v8i8`; the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit signed or-reduction loops now also have regression coverage through `or_fold_i8`, reusing the same `llvm.fshl.i32` plus raw-switch path while extending the backend to `<8 x i8>` vector or and `llvm.vector.reduce.or.v8i8`; the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit signed and-reduction loops now also have regression coverage through `and_fold_i8`, reusing the existing epilog-only `<4 x i8>` and plus `llvm.vector.reduce.and.v4i8` path under a signed positive-trip-count guard; this optimized shape also bypasses the main vector loop and still widens the managed emitted and invoked signature to `Int32`
- optimized 8-bit signed max loops now also have regression coverage through `max_xor_i8`, extending the backend to signed `i8` max across scalar `llvm.smax.i8`, `<16 x i8>` `llvm.smax.v16i8` plus `llvm.vector.reduce.smax.v16i8`, and `<8 x i8>` `llvm.smax.v8i8` plus `llvm.vector.reduce.smax.v8i8`; the managed emitted and invoked signature still widens to `Int32`
- optimized 8-bit signed min loops now also have regression coverage through `min_xor_i8`, extending the backend to signed `i8` min across scalar `llvm.smin.i8`, `<8 x i8>` `llvm.smin.v8i8`, and `llvm.vector.reduce.smin.v8i8`; this optimized shape bypasses the main vector body and the managed emitted and invoked signature still widens to `Int32`
- optimized 16-bit signed-min loops now also have regression coverage through `min_xor_i16`, extending the vector path down to a `<8 x i16>` main loop, a `<4 x i16>` vector epilog, raw `insertelement`/`shufflevector` broadcast handling, and scalar `llvm.smin.i16`
- optimized 16-bit xor-reduction loops now also have regression coverage through `xor_fold_i16`, extending the same `<8 x i16>` and `<4 x i16>` helper path to `llvm.vector.reduce.xor.v8i16`, `llvm.vector.reduce.xor.v4i16`, numeric raw SSA seed normalization, and xor-specific raw `insertelement` epilog seed handling
- optimized 16-bit add-reduction loops now also have regression coverage through `sum_xor_i16`, extending the same `<8 x i16>` and `<4 x i16>` helper path to `llvm.vector.reduce.add.v8i16`, `llvm.vector.reduce.add.v4i16`, and 16-bit vector add reduction helper emission
- optimized 16-bit or-reduction loops now also have regression coverage through `or_fold_i16`, extending the same `<8 x i16>` and `<4 x i16>` helper path to vector `or`, `llvm.vector.reduce.or.v8i16`, `llvm.vector.reduce.or.v4i16`, and 16-bit vector or reduction helper emission
- optimized 16-bit and-reduction loops now also have regression coverage through `and_fold_i16`, extending the same `<8 x i16>` and `<4 x i16>` helper path to vector `and`, `llvm.vector.reduce.and.v8i16`, `llvm.vector.reduce.and.v4i16`, and all-ones raw `insertelement` epilog seed handling
- standard optimized signed-max loops now also have regression coverage through a vectorized `max_xor_i32` sample, extending the same `<4 x i32>` path to lane-wise `llvm.smax.v4i32` and `llvm.vector.reduce.smax.v4i32`
- standard optimized signed-min loops now also have regression coverage through a vectorized `min_xor_i32` sample, extending the same `<4 x i32>` path to lane-wise `llvm.smin.v4i32` and `llvm.vector.reduce.smin.v4i32`
- standard optimized unsigned-max loops now also have regression coverage through a vectorized `max_xor_u32` sample, extending the same `<4 x i32>` path to lane-wise `llvm.umax.v4i32` and `llvm.vector.reduce.umax.v4i32`
- standard optimized unsigned-min loops now also have regression coverage through a vectorized `min_xor_u32` sample, extending the same `<4 x i32>` path to lane-wise `llvm.umin.v4i32` and `llvm.vector.reduce.umin.v4i32`
- optimized integer loops now also have regression coverage through `sum_fold_i32`, where LLVM rewrites the loop into a closed-form arithmetic series that uses non-native `i33` intermediates rather than a loop body at all
- standard optimized reduction loops now also have regression coverage through `sum_xor_i32`, extending the vector helper path to `llvm.vector.reduce.add.v4i32` in addition to the earlier xor reduction support
- 64-bit optimized reduction loops now also have regression coverage through `sum_xor_i64`, extending the helper path from `<4 x i32>` to `<2 x i64>` vector constants, vector `add`/`xor`, and `llvm.vector.reduce.add.v2i64`
- 64-bit optimized xor-reduction loops now also have regression coverage through `xor_fold_i64`, extending the same `<2 x i64>` helper path to `llvm.vector.reduce.xor.v2i64`
- 64-bit optimized or-reduction loops now also have regression coverage through `or_fold_i64`, extending the same `<2 x i64>` helper path to vector `or` and `llvm.vector.reduce.or.v2i64`
- 64-bit optimized and-reduction loops now also have regression coverage through `and_fold_i64`, extending the same `<2 x i64>` helper path to vector `and` and `llvm.vector.reduce.and.v2i64`
- 64-bit optimized signed-max loops now also have regression coverage through `max_xor_i64`, extending the same `<2 x i64>` path to lane-wise `llvm.smax.v2i64` and `llvm.vector.reduce.smax.v2i64`
- 64-bit optimized signed-min loops now also have regression coverage through `min_xor_i64`, extending the same `<2 x i64>` path to lane-wise `llvm.smin.v2i64` and `llvm.vector.reduce.smin.v2i64`
- 64-bit optimized unsigned-max loops now also have regression coverage through `max_xor_u64`, extending the same `<2 x i64>` path to lane-wise `llvm.umax.v2i64` and `llvm.vector.reduce.umax.v2i64`
- 64-bit optimized unsigned-min loops now also have regression coverage through `min_xor_u64`, extending the same `<2 x i64>` path to lane-wise `llvm.umin.v2i64` and `llvm.vector.reduce.umin.v2i64`
- integer arithmetic and bitwise lowering now includes direct and optimized `and`/`or`/`xor`/`mul`, plus direct and optimized `shl`/`ashr` with the Rust shift-count masking shape, alongside `add`, `sub`, and the existing shift-based aggregate extraction path
- standalone logical right shift lowering now also has direct and Cargo regression coverage through the same Rust masked shift-count shape already used by aggregate extraction
- guarded signed division lowering now covers the direct and Cargo `sdiv` shape for `i32`, including Rust's injected divide-by-zero and overflow checks, typed `i1` guards, panic calls, and `unreachable` blocks mapped to managed exceptions
- the same guarded signed division support is now also regression-tested for `i64`, including the 64-bit overflow sentinel path and managed exception mapping
- guarded unsigned division lowering now covers the direct and Cargo `udiv` shape for `u32`, including Rust's injected divide-by-zero check and panic-to-exception mapping
- guarded signed and unsigned remainder lowering now covers the direct and Cargo `srem`/`urem` shapes, including Rust's injected divide-by-zero checks and the signed remainder overflow path mapped to managed exceptions
- the same guarded remainder support is now also regression-tested for `i64` and `u64`, including the 64-bit signed overflow sentinel path
- direct translated-to-translated calls
- multi-block control flow with `icmp`, conditional branch, and jump
- stack-slot locals through `alloca`, `store`, and `load`
- simple 4-byte and 8-byte constant globals initialized in a generated static constructor
- constant global element reads lowered from `getelementptr` into indexed byte-slice decoding for `i32` and `i64` elements
- stack-local indexed element reads and writes lowered from `getelementptr` address temporaries into scalarized local slots
- by-value struct field extraction lowered through both direct `llvm.memcpy` plus byte-offset packed-local projection and Cargo-optimized packed `i64` ABI values, `lshr`, and `trunc`
- fixed-offset field loads from by-value aggregate parameters that arrive as `ptr` in direct bitcode

Current limit for that slice: the prototype now covers the exact local packed-struct `llvm.memcpy` case emitted by the direct `pair_right` sample and the fixed-offset `ptr` parameter field-read case emitted by the direct `pair_i64` sample, but it does not yet implement a general aggregate memory model, broad pointer semantics, or general `llvm.memcpy` handling beyond those validated forms.

Useful commands in the current repo:

```text
dotnet run --project ./dotnet/backend/src/RustMcil.Tool/RustMcil.Tool.csproj -- inspect ./artifacts/out/add/add.bc
dotnet run --project ./dotnet/backend/src/RustMcil.Tool/RustMcil.Tool.csproj -- lower ./artifacts/out/max/max.bc
dotnet run --project ./dotnet/backend/src/RustMcil.Tool/RustMcil.Tool.csproj -- emit ./artifacts/out/call_chain/call_chain.bc --out ./artifacts/out/call_chain/call_chain.generated.dll
dotnet run --project ./dotnet/backend/src/RustMcil.Tool/RustMcil.Tool.csproj -- invoke ./artifacts/out/global_init/global_init.bc --method read_global_i32
dotnet run --project ./dotnet/backend/src/RustMcil.Tool/RustMcil.Tool.csproj -- translate ./samples/add --out ./artifacts/out/add/add.from-cargo.dll --bitcode-out ./artifacts/out/add/add.from-cargo.bc
```

One concrete difference between direct `rustc` artifacts and `cargo rustc` artifacts in this repo is that Cargo-built LLVM IR currently includes ABI attributes such as `noundef` in function signatures and argument lists. The lowered IR layer now strips those attributes before type matching so the emitter can stay focused on semantic types rather than frontend-specific ABI decoration.

Another concrete difference in Cargo release builds is that LLVM may optimize source-level control flow and direct calls into other IR forms before the managed backend sees them. The current prototype now handles two such shapes that were not present in the earlier script-built bitcode path:

- `tail call @llvm.smax.i32` and related signed min/max intrinsics, which are mapped to `System.Math`
- exported function aliases such as `@call_chain_i32 = alias ... @add_one_i32`, which are lowered into forwarding managed methods

The inspector path also now classifies those aliases explicitly in module summaries instead of reporting them as globals, so optimized Cargo artifacts are easier to reason about during debugging.

The next technical gaps are no longer basic emission. They are broader data-model and runtime work such as richer globals, aggregate types, pointer semantics, runtime helpers, and eventually `core`/`alloc` and `std` support.

### Step 1: lock the environment

- choose a pinned nightly toolchain date
- install `rust-src`
- choose the .NET SDK version
- choose the LLVM binding version and matching libLLVM version

Output:

- one checked-in environment note with exact versions

### Step 2: build the smallest Rust sample

Sample shape:

```rust
#![no_std]

#[no_mangle]
pub extern "C" fn add_i32(a: i32, b: i32) -> i32 {
    a + b
}
```

Output:

- one bitcode artifact and one text dump describing its functions and instructions

### Step 3: implement `inspect`

Command shape:

```text
cargo dotnet-il inspect samples/add
```

Output:

- function list
- global list
- block list
- instruction list

### Step 4: implement `translate`

Command shape:

```text
cargo dotnet-il translate samples/add --out out/add_i32.dll
```

Output:

- a managed DLL
- a textual dump of lowered IR
- a textual dump of emitted methods

### Step 5: add test harness

The harness should:

- load the generated assembly
- call `add_i32`
- verify the output
- fail with readable diagnostics

## First sample matrix

Use this exact progression.

1. `add_i32`
2. `sub_i64`
3. `max_i32` using conditional branches
4. `call_chain` with one translated function calling another
5. `global_init` with one simple static global

Do not introduce arrays, structs, or pointers until these pass.

## Explicit non-goals for the first attempt

These are the things most likely to blow up the schedule if introduced too early:

- `std`
- custom managed API bindings
- copying the old `sgwin32` concept
- support for arbitrary Cargo dependencies
- support for Rust panics beyond abort
- support for unwinding
- support for trait-heavy or async-heavy crates

## Risks and fallback decisions

## Risk: stable toolchain may not be enough

Fallback:

- keep prototype v0 on nightly from the beginning to avoid churn

## Risk: getting a single `.bc` artifact from Cargo is awkward

Fallback:

- begin with direct `rustc` invocation for milestone 0
- move to Cargo orchestration only after the backend path works

## Risk: LLVMSharp version friction

Fallback:

- isolate LLVM access behind a tiny adapter assembly
- if necessary, replace the binding later without changing the rest of the compiler

## Risk: IL emission gets coupled to LLVM details too early

Fallback:

- stop and strengthen the lowered IR before adding more instruction support

## What should exist by the end of the first serious attempt

By the end of the first real attempt, the repository should have:

- one sample crate compiled to bitcode
- one managed module inspector
- one minimal lowered IR
- one IL emitter for integer-only samples
- one tiny runtime assembly
- one green end-to-end test from Rust source to running managed DLL

That is the correct bar for the first prototype.

It is high enough to validate the architecture and low enough to keep the effort under control.

## Recommendation

Attempt the prototype in two passes.

Pass 1:

- no_std
- one sample
- no custom target if you can avoid it
- no std-aware Cargo

Pass 2:

- introduce nightly-only target plumbing
- add `core` and `alloc`
- only then start the path toward `std`

That gives you the fastest route to a technically meaningful proof, while deferring the historically expensive parts of the problem.