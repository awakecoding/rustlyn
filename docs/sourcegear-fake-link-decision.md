# SourceGear Fake-Link Decision

Status: **permanently deferred** — LTO staticlib is sufficient.

Date: 2026-05-21 (initial), 2026-05-22 (updated with LTO evidence)

## Decision

Do not implement a revived `Rustlyn.FakeLink` or reintroduce SourceGear-style target `pre-link-args`. The LTO staticlib approach provides whole-program bitcode through standard Cargo without linker interception.

## Evidence

### Direct emission is insufficient for dependencies

Direct `cargo rustc --emit llvm-bc` without LTO captures only the top crate. Dependency functions appear as `declare` (extern references) with no body — insufficient for translation.

### LTO staticlib merges all dependencies

When using `crate-type = ["staticlib"]` with `lto = "fat"` in the release profile, Cargo produces a single bitcode file containing all dependency code as `define internal` functions. File size: 3 KB (top-crate only) → 728 KB (with all deps + std merged).

### End-to-end proof: `dep_heavy` sample

The `dep_heavy` fixture validates cross-crate function calls through LTO:

- `dep_heavy_probe()` calls `dep_lib::fibonacci(10)` + `dep_lib::collatz_steps(27)`
- Both dependency functions are merged into the single bitcode file
- The `DepHeavyCrosscrateCallsResolveThroughLto` test passes end-to-end: compile → lower → emit → invoke → result = 166

### Build-std ladder

Three executable rungs prove build-std compatibility without fake-link:

- `build_std_core_probe`: nightly `--build-std core`, invokes `add_i32(19, 23) == 42`
- `build_std_alloc_probe`: nightly `--build-std core,alloc`, invokes `alloc_vec_capacity_score() == 4`
- `build_std_std_probe`: nightly `--build-std std,panic_abort`, invokes `std_fs_line_count() == 3`

### Command-shape test

The permanent backend test `RustBitcodeBuildStdArgumentsAvoidFakeLinker` pins the command shape: build-std uses direct LLVM bitcode emission and does not add `rsfakelink`, `linker`, or replacement-linker arguments.

## Advantages over fake-link

1. No custom target JSON or linker wrapper
2. No fragile linker interception that breaks across toolchain versions
3. Works with standard `cargo build` — no special invocation
4. Handles dependencies, build-std, and cross-crate generics naturally
5. Produces a single merged bitcode file (no artifact stitching)

## Trade-offs

1. Requires `crate-type = ["staticlib"]` — the crate must be a library, not a binary
2. Requires `lto = "fat"` in the release profile — incremental builds are slower
3. OS FFI calls (e.g., `GetProcessHeap`) remain as `declare` — resolved by `Rustlyn.Os` at runtime

## Fixture

The permanent fixture proving this decision lives at `samples/dep_heavy/`:

```
samples/dep_heavy/
├── Cargo.toml          # workspace with staticlib + LTO
├── src/lib.rs          # exported probe calling dep_lib
└── dep_lib/
    ├── Cargo.toml      # local dependency crate
    └── src/lib.rs      # fibonacci + collatz_steps
```

## Reopen Criteria

Reopen only if a promoted fixture proves:

- LTO staticlib cannot produce discoverable whole-program bitcode for a required shape
- A real workload (not hypothetical) fails specifically because this approach is insufficient
- A custom target JSON is the only way to collect needed bitcode

## If Reopened

Start with the failing fixture and make it permanent first. Design the narrowest fake-link path around that evidence, keeping any custom target/sysroot behavior opt-in.