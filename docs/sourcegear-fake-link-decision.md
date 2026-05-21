# SourceGear Fake-Link Decision

Status: deferred, with explicit reopen criteria.

Date: 2026-05-21

## Decision

Do not implement a revived `RustMcil.FakeLink` or reintroduce SourceGear-style target `pre-link-args` yet.

The current revived path stays cargo-first: `RustMcil.Tool translate` uses `cargo rustc --emit llvm-bc`, and SourceGear recovery uses nightly `cargo -Z build-std` only when a fixture explicitly asks for `core`, `core,alloc`, or `std,panic_abort` coverage.

This is not a claim that modern Cargo has already replaced every old whole-program/sysroot trick. It is a scoped decision: the current executable parity fixtures no longer justify building fake-linker machinery before the runtime split, generated bindings, and broader workload validation advance further.

## Evidence

The build-std ladder now has three focused, executable rungs:

- `build_std_core_probe` builds `samples/add` with nightly `--build-std core`, inspects and lowers the bitcode, and invokes `add_i32(19, 23) == 42`.
- `build_std_alloc_probe` builds `samples/alloc_only_probe` with nightly `--build-std core,alloc`, captures the Rust allocator shim, and invokes `alloc_vec_capacity_score() == 4`.
- `build_std_std_probe` builds `samples/std_fs` with nightly `--build-std std,panic_abort`, preserves the `std::fs::read_to_string` path in lowered IR, and invokes `std_fs_line_count() == 3`.

The permanent backend test `RustBitcodeBuildStdArgumentsAvoidFakeLinker` pins the intended command shape: build-std uses direct LLVM bitcode emission and does not add `rsfakelink`, `linker`, or replacement-linker arguments.

## Reopen Criteria

Reopen `RustMcil.FakeLink` design only when a focused fixture proves one of these failures:

- `cargo rustc --emit llvm-bc` plus relevant `-Z build-std` components cannot produce a discoverable `.bc` artifact for a required library or binary shape.
- A promoted fixture needs dependency or sysroot bitcode that Cargo does not expose through the current artifact discovery path.
- A custom target JSON can only collect the needed bitcode by replacing the final linker step.
- A real workload, not a hypothetical SDK concern, fails specifically because the revived path lacks a whole-program bitcode capture mechanism.

## If Reopened

Start with the failing fixture and make it permanent first. Then design the narrowest fake-link path around that evidence, preserving the existing cargo-first API and keeping any custom target/sysroot behavior opt-in.