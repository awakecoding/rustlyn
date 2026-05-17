# AGENTS.md

## Purpose

This repo is a backend reconstruction project for translating Rust-produced LLVM bitcode into managed .NET assemblies. Keep work narrow, executable, and easy to validate.

## Ground Rules

- Prefer fixing the direct owner of a behavior, not layering broad workarounds.
- Treat `samples/` as permanent regression fixtures, not throwaway demos.
- Do not commit generated outputs from `target/`, `bin/`, `obj/`, `artifacts/out/`, or `artifacts/scratch/`.
- Keep the historical SourceGear/Llama material separate from the revived design work.
- Preserve the existing coding and test style; avoid opportunistic refactors.

## Architecture Anchors

- `dotnet/backend/src/RustMcil.Tool/`: CLI entry point for inspect/lower/emit/invoke/translate flows
- `dotnet/backend/src/RustMcil.Backend/LoweredIrLowerer.cs`: lowered IR parsing and normalization
- `dotnet/backend/src/RustMcil.Backend/LoweredAssemblyEmitter.cs`: IL emission, helper generation, and intrinsic dispatch
- `dotnet/backend/tests/RustMcil.Backend.Tests/Program.cs`: regression harness and expected-shape assertions
- `scripts/Build-SampleBitcode.ps1`: direct sample-to-bitcode builder
- `scripts/Test-Smoke.ps1`: focused executable smoke checks

## Preferred Workflow

1. Start from a concrete anchor: a failing sample, a missing intrinsic, or a broken command.
2. Gather only enough local context to form one falsifiable hypothesis.
3. Make the smallest plausible edit.
4. Run the narrowest validation immediately.
5. If the slice is now supported, promote it into permanent sample/tests/smoke/docs.

## Validation Commands

Use these before widening scope:

```powershell
.\scripts\Build-SampleBitcode.ps1 -Sample add
.\scripts\Test-Smoke.ps1 -Sample add -Mode Bitcode
.\scripts\Test-Smoke.ps1 -Sample add -Mode Cargo
dotnet run -c Release --project .\dotnet\backend\tests\RustMcil.Backend.Tests\RustMcil.Backend.Tests.csproj
```

For direct tool work:

```powershell
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- inspect <bitcode>
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- lower <bitcode>
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- emit <bitcode> --out <assembly>
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- invoke <bitcode> --method <name>
dotnet run --project .\dotnet\backend\src\RustMcil.Tool\RustMcil.Tool.csproj -- translate <crate-dir> --out <assembly> --bitcode-out <bitcode>
```

## Documentation Split

- Use `README.md` for user-facing value and the shortest path to trying the repo.
- Use `docs/original-eric-sink-design.md` for the historical SDK design and public references.
- Use `docs/revived-design.md` for the architecture being rebuilt in this repo now.
- Keep `REBUILDING_RUST_TO_DOTNET_TODAY.md` as the deep notebook, not the front door.

## Cleanup Policy

If you generate outputs while working, validate with them, then leave them untracked. The repo should keep source, tests, docs, and historical text evidence, not local build churn or packaged binary payloads.