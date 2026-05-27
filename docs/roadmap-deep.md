# Deep roadmap — items that require native dependencies

The Tier 1 and Tier 2 work in this repository focuses on slices that can be
delivered with the .NET-only backend, the existing text-driven LLVM lowering
path, and the runtime/SDK/test infrastructure we already have. Three first-
class-language items genuinely require external dependencies — native LLVM
bindings, Rust personality wiring, or a language-server runtime — and have
been split out here so contributors can pick them up with the right scope
already framed.

Each section captures:

- **External dependency** — the new native library, runtime, or service we
  would take on.
- **Scope** — what "done" looks like in concrete terms.
- **Milestone breakdown** — the smallest verifiable slices.
- **Validation strategy** — how we will know each slice works.

---

## 1. `semantic-llvm-reader` — replace regex lowering with a real LLVM IR reader

### External dependency

The current pipeline shells out to `llvm-dis`, then parses textual IR with
regular expressions in `LoweredIrLowerer`. That is the right starting point
for a sample-driven reconstruction, but it is the single biggest reason the
backend silently degrades on real Rust crates: anything outside the regex
catalogue becomes a `LoweredRawInstruction` and falls through to ad-hoc
handling in the emitter.

A first-class reader needs to consume LLVM bitcode through the LLVM C API
directly. Practical options:

- **Llvm.NET** (LLVMSharp / Ubiquity.NET.Llvm) — managed bindings over a
  pinned LLVM version. Trade-off: large native dependency, version coupling,
  AOT-unfriendly today.
- **A small in-tree native shim** that exposes only the subset of LLVMC we
  consume (module → functions → blocks → instructions → operands +
  datalayout/triple/metadata/attributes). Trade-off: we ship and version a
  native artifact ourselves but stay narrow.

Both require committing to one LLVM major version per release.

### Scope

`LoweredIrLowerer.LowerBitcode(path)` is replaced by a reader that:

1. Reads the module's datalayout and target triple from the bitcode header
   and surfaces them on `LoweredModule`.
2. Walks every function, block, and instruction through the LLVM API instead
   of regex. Every instruction lands as a *typed* `LoweredXyzInstruction`
   record with operand metadata, ordering, address-space, alignment,
   `nounwind`/`nocapture`/`noalias`/`sret`/`byval` attributes, debug
   locations, and constant expressions preserved.
3. Surfaces unmodeled constructs as `LoweredUnsupportedInstruction(kind,
   text)` so strict mode rejects them cleanly instead of dropping silently.

### Milestone breakdown

1. **M1 — Module-level metadata.** Introduce the native shim (or take the
   LLVMSharp dependency behind a feature flag). Wire datalayout/triple
   round-tripping. Validate by comparing against `llvm-dis` text output.
2. **M2 — Function and block iteration.** Replace the text-scanning loop
   with API calls. Keep emitting `LoweredRawInstruction` for instruction
   bodies. Validate by diffing the produced module shape against today's
   text path on every registered sample.
3. **M3 — Typed instruction records.** Promote one opcode family per slice
   (binary ops → memory ops → control flow → calls → exception pads →
   atomics → vector). Each slice deletes the regex equivalent and adds a
   targeted fixture.
4. **M4 — Attribute and metadata preservation.** Surface call-site
   attributes (`sret`, `byval`, alignment), function attributes (`nounwind`,
   `noreturn`), and debug locations on every typed record.
5. **M5 — Decommission the text path.** Once every fixture passes through
   the API reader, delete the regex lowerer.

### Validation strategy

- Per-milestone: golden IR tests that compare typed records to a hand-checked
  expected shape.
- Cross-milestone: re-run the full sample matrix and the doc-claims audit
  with `--strict` enabled on both paths and require parity.
- After M5: register at least one large real-world crate (e.g., `regex` or
  `serde_json`) in the extended CI job to catch regressions the curated
  fixtures miss.

---

## 2. `panic-unwind-regions` — first-class Rust panic + drop on unwind

### External dependency

Rust's default `panic = unwind` model relies on a personality function
(`rust_eh_personality`) and LSDA tables that LLVM emits as part of every
`invoke` / `landingpad` / `resume` triplet. On .NET the equivalent contract
is exception handlers (`try`/`catch`/`finally`/`filter`) emitted into method
bodies. Bridging those two worlds requires:

- A managed exception type (`Rustlyn.Runtime.RustPanic`) that carries the
  payload from `panic!` (the formatted string, location, and backtrace).
- An emitter pass that, for every Rust function, converts the `invoke` /
  `landingpad` / cleanup-pad graph into `.try`/`.catch`/`.finally` regions
  in IL.
- A runtime hook that intercepts the personality call path and rewrites it
  into a managed throw so cleanup blocks run as `.finally` regions.
- A `Drop` strategy that ties Rust's destructor calls to managed `.finally`
  regions, including double-drop avoidance and pinned-borrow safety.

### Scope

After this milestone, a Rust function that calls `panic!()`:

1. Throws a `RustPanic` whose `Message` matches the formatted panic text and
   whose `Location` carries the original `file:line:col`.
2. Runs every `Drop` impl on every live local in unwind order, exactly once.
3. Is catchable with managed `try { ... } catch (RustPanic) { ... }`.
4. Re-throws cleanly through nested `catch_unwind` boundaries.

### Milestone breakdown

1. **M1 — `panic = abort` baseline.** Make sure abort calls throw a
   distinguished managed exception (`RustAbort`) with the panic payload.
   No unwind regions yet.
2. **M2 — Single-frame unwind.** Convert `invoke` + `landingpad` pairs in
   leaf functions to `.try`/`.catch` regions that re-throw `RustPanic`.
3. **M3 — Drop cleanup regions.** When the landingpad's cleanup BB calls a
   destructor, emit a matching `.finally` block instead of plain catch.
   Validate ordering against `Drop` fixtures.
4. **M4 — `catch_unwind`.** Implement the libstd intrinsic by lowering to
   a managed `try`/`catch` around the closure invocation.
5. **M5 — Cross-FFI propagation.** Decide and document what happens when a
   panic escapes through a Rust→.NET callback boundary (probably: convert
   to `RustPanic` and rethrow on the .NET side).

### Validation strategy

- Per milestone: a focused fixture sample under `samples/panic_*` that
  asserts a specific Drop trace and exception type.
- Cross-cutting: a `panic_matrix` smoke that exercises every combination of
  nested panics, nested `catch_unwind`, and Drops with side effects.
- IL-level: a verifier-style test that confirms each emitted method has
  well-formed `.try`/`.catch`/`.finally` regions (no overlapping handlers,
  correct stack height on entry/exit).

---

## 3. `language-service` — IDE integration for Rustlyn projects

### External dependency

A real IDE story needs at least one of:

- **rust-analyzer** as a sub-process, talking LSP, so we get Rust-level
  diagnostics/navigation/refactoring on the source side. Pro: free
  industrial-grade Rust intelligence. Con: large process, needs lifetime
  management and crate-roots wiring.
- **A .NET language server** built on `Microsoft.CodeAnalysis.LanguageServer`
  that adds Rustlyn-specific actions (jump from a managed call site to the
  Rust source, surface translate diagnostics inline, expose IL preview).
  Pro: deep integration with the .NET tooling story. Con: we write and ship
  it from scratch.

The first-class plan is to do *both*: rust-analyzer for Rust semantics, a
thin Rustlyn LSP shim for the cross-boundary story.

### Scope

After this milestone, the IDE experience for a `.rsproj` includes:

1. Rust syntax highlighting, completion, diagnostics, and go-to-definition
   inside `src/**/*.rs`.
2. Inline display of `rustlyn translate` diagnostics (unsupported IR
   records, fallback warnings, strict-mode failures) as squigglies on the
   originating Rust line.
3. "Go to managed binding" from a Rust `pub extern "C"` function jumps to
   the generated .NET wrapper.
4. "Go to Rust source" from a managed call site (via PortablePDB
   `.rs` mapping) jumps to the Rust function.
5. A live "IL preview" pane showing the emitter output for the function
   under the cursor.

### Milestone breakdown

1. **M1 — Capabilities manifest.** Define the project-system capabilities
   for `.rsproj` so VS / Rider / VS Code recognise it as a managed project
   that contains Rust source.
2. **M2 — rust-analyzer embedding.** Launch `rust-analyzer` on the
   generated Cargo workspace, forward LSP messages, expose its diagnostics
   in the editor.
3. **M3 — Translate-diagnostics bridge.** Re-emit `rustlyn translate`
   warnings in LSP format with the right `file:line:col` (sourced from
   M1/M2 of `semantic-llvm-reader` so the locations are real).
4. **M4 — Cross-boundary navigation.** Implement go-to-managed-binding
   and go-to-Rust-source using the PortablePDB source map + the
   generated-bindings symbol table.
5. **M5 — IL preview.** Surface the emitter output for the function under
   the cursor in a side pane. Initially read-only.

### Validation strategy

- LSP conformance tests against the published Microsoft LSP test harness.
- Manual scenarios: open the `samples/aot_smoke` and `samples/dep_heavy`
  projects in VS Code with the extension, walk through each milestone's
  acceptance criteria.
- A CI job that boots the language server, opens a known project, and
  asserts a fixed set of diagnostics appear in the LSP message stream.

---

## Cross-cutting notes

- All three workstreams must keep the existing fast-text path operational
  until at least one full release cycle has passed, so contributors without
  the native dependency can still build and validate the rest of the
  pipeline.
- Each native dependency landing must come with a `RUSTLYN_FEATURE_*` opt-in
  flag and a graceful fall-back diagnostic when the feature is not built in.
- Documentation: as each milestone ships, promote the corresponding row in
  `docs/support-matrix.md` from `Planned` / `Preview` to `Supported`, and
  update `docs/roadmap.md` to point readers here from the high-level plan.
