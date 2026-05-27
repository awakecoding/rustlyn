# aot_smoke

Native AOT/trimming smoke scaffold for Rustlyn-translated assemblies.

The pipeline is:

1. Translate `src/lib.rs` to a managed assembly with `rustlyn translate`.
2. The `host/` C# console references the translated assembly.
3. `dotnet publish host -p:PublishAot=true -r <rid>` produces a native binary.
4. Run the binary and assert the expected stdout.

Use `scripts/Test-AotSmoke.ps1` to drive the whole flow end-to-end.

**Status:** Tier 2 scaffold. Not wired to CI yet because Native AOT capacity
is platform-dependent and the bitcode pipeline needs an LLVM toolchain on
the runner. The scaffold is committed so contributors can iterate.
