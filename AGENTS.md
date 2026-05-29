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

- `dotnet/backend/src/Rustlyn.Tool/`: unified `rustlyn` CLI (`AssemblyName=rustlyn`). Subcommands: `cargo`, `rustc`, `new`, `run`, `inspect`, `lower`, `emit`, `invoke`, `translate`, `pack`, `llvm`, `--help`, `--version`
- `dotnet/backend/src/Rustlyn.Backend/LoweredIrLowerer.cs`: lowered IR parsing and normalization (note: `StripTrailingInstructionMetadata` strips `!dbg` tails using a rightmost top-level comma scan; called once at the top of `ParseInstruction`)
- `dotnet/backend/src/Rustlyn.Backend/LoweredAssemblyEmitter.cs`: IL emission, helper generation, intrinsic dispatch (note: `EmitLoadValue` has a silent `ldc.i4.0` fallback — when an instruction misbehaves with a `0` operand, suspect the lowerer's tokenization first)
- `dotnet/backend/tests/Rustlyn.Backend.Tests/Program.cs`: regression harness and expected-shape assertions
- `scripts/Rustlyn.Cli.ps1`: shared `Resolve-RustlynCli` / `Invoke-RustlynCli` helpers used by every script
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
dotnet run -c Release --project .\dotnet\backend\tests\Rustlyn.Backend.Tests\Rustlyn.Backend.Tests.csproj
```

For direct tool work (prefer the built `rustlyn.exe` over `dotnet run --project ...`):

```powershell
dotnet build .\dotnet\backend\src\Rustlyn.Tool\Rustlyn.Tool.csproj -c Release
$rustlyn = ".\dotnet\backend\src\Rustlyn.Tool\bin\Release\net10.0\rustlyn.exe"
& $rustlyn inspect <bitcode>
& $rustlyn lower <bitcode>
& $rustlyn emit <bitcode> --out <assembly>
& $rustlyn invoke <bitcode> --method <name> --arg i32:3 --arg i32:4
& $rustlyn translate <crate-dir> --out <assembly> --bitcode-out <bitcode>
& $rustlyn cargo build --manifest-path <crate>\Cargo.toml
```

## CLI Gotchas

- `Rustlyn.Tool.csproj` sets `<AssemblyName>rustlyn</AssemblyName>`. Output is `rustlyn.dll` / `rustlyn.exe`, **not** `Rustlyn.Tool.dll`. Any script or CI step hardcoding `Rustlyn.Tool.dll` will break.
- `dotnet pack` on `Rustlyn.Sdk` publishes `Rustlyn.Tool` with `UseAppHost=false`, which deletes the local `rustlyn.exe` apphost mid-script. Scripts that pack the SDK and then invoke the tool must pin `Resolve-RustlynCli` to `-ToolDll <path-to-rustlyn.dll>` (see `Test-MsBuildSdkPackage.ps1`).
- `Invoke-RustlynCli` uses `[ValueFromRemainingArguments]`; pass extra args as positional tokens, not a single quoted string.

## Documentation Split

- Use `README.md` for user-facing value and the shortest path to trying the repo.
- Use `docs/original-eric-sink-design.md` for the historical SDK design and public references.
- Use `docs/revived-design.md` for the architecture being rebuilt in this repo now.
- Keep `docs/reconstruction-notes.md` as the deep notebook, not the front door.

## Cleanup Policy

If you generate outputs while working, validate with them, then leave them untracked. The repo should keep source, tests, docs, and historical text evidence, not local build churn or packaged binary payloads.