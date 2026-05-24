# Original Eric Sink Design

This document describes the 2020-2021 SourceGear/Llama Rust-on-.NET experiment as it originally shipped, independent of the revived implementation work in this repository.

## Summary

Eric Sink's original design was an MSBuild SDK wrapped around a Rust-to-LLVM-to-CIL pipeline.

At a high level it worked like this:

1. The developer worked with an SDK-style `.rsproj` and `dotnet build`.
2. The SDK translated project metadata into synthetic Cargo manifests.
3. Rust and the sysroot were built to LLVM bitcode with a custom target and fake linker.
4. A .NET compiler stack consumed the bitcode and emitted managed assemblies.
5. Helper runtime libraries bridged LLVM semantics and operating-system behavior on top of .NET.

The result was not a Rust-to-C# transpiler. It was a bitcode-to-managed-assembly pipeline with a thin MSBuild shell.

## What Shipped

From the published `SourceGear.Rust.NET.Sdk` package and the decompilation work in this repo, the original system shipped these main pieces:

- MSBuild glue in `build/` and `Sdk/`
- `tools/rsbuild/` as the orchestrator and LLVM-to-CIL compiler stack
- `tools/rsfakelink/` as the custom fake linker used during Cargo builds
- `tools/crates/` for helper crates and runtime assemblies
- bundled native LLVM binaries for Windows, macOS, and Ubuntu 18.04 x64

The implementation was primarily F# on .NET, with LLVMSharp reading LLVM IR and Mono.Cecil emitting managed assemblies.

## Design Characteristics

The original experiment had a few defining choices:

- MSBuild-first user experience
- nightly Rust requirement
- custom target JSON and fake-linker flow
- custom sysroot bootstrap rather than a modern `cargo -Z build-std` flow
- package-distributed compiler/runtime binaries
- generated Rust bindings for .NET assemblies
- runtime bridges such as `sgwin32` to stand in for platform behavior

## Strengths And Constraints

The design was technically serious and farther along than a toy demo, but it also carried tight coupling to the toolchain assumptions of its time.

Strengths:

- a plausible end-to-end Rust-to-.NET build flow
- direct integration with familiar `dotnet build` ergonomics
- support for real walkthroughs beyond `Hello World`
- evidence of a meaningful runtime and binding-generation strategy

Constraints:

- pinned nightly Rust assumptions
- LLVM 10-era packaging and APIs
- heavy reliance on custom build plumbing
- large binary package payloads
- proof-of-concept quality and explicit non-production positioning

## Historical Evidence

This repo keeps the summarized historical analysis that is most useful for study:

- the long-form reconstruction notebook in [reconstruction-notes.md](reconstruction-notes.md)
- public SourceGear package and blog links below

The extracted package tree, decompiled source views, and published binary payloads are intentionally documented by link and summary rather than retained as regular repo content.

## Eric Sink Blog Posts

These are the core public posts that document the evolution of the original experiment and its adjacent Llama work:

- [My exploration of Rust and .NET](https://ericsink.com/entries/dotnet_rust.html) (2020-03-10): first public write-up of the unnamed Rust-to-.NET compiler and binding-generator experiment.
- [SourceGear.Rust.NET preview 0.1.0](https://ericsink.com/entries/sg_rust_dotnet_preview.html) (2020-04-20): first public preview release, package names, prerequisites, and limitations.
- [Llama Rust SDK preview 0.1.3](https://ericsink.com/entries/llama_rust_013.html) (2021-01-31): walkthrough using a real crate, plus a detailed explanation of the generated Cargo/sysroot pipeline.
- [Llofty Ambitions](https://ericsink.com/entries/llofty_ambitions.html) (2021-02-09): broader framing of Llama as a path to bringing more languages to .NET.
- [Llama Rust SDK preview 0.1.4](https://ericsink.com/entries/llama_rust_resvg.html) (2021-02-23): another preview step using a harder SVG rendering workload.
- [Calling .NET APIs from Rust](https://ericsink.com/entries/lousygrep.html) (2021-03-10): focused example of the generated Rust-to-.NET bindings side of the experiment.

## Published Packages And Public Artifacts

- [SourceGear.Rust.NET.Sdk 0.1.5 package page](https://www.nuget.org/packages/SourceGear.Rust.NET.Sdk/0.1.5)
- [SourceGear.Rust.NET.Sdk 0.1.5 direct download](https://www.nuget.org/api/v2/package/SourceGear.Rust.NET.Sdk/0.1.5)
- [SourceGear.Rust.NET.Templates package page](https://www.nuget.org/packages/SourceGear.Rust.NET.Templates)
- [SourceGear.Rust.NET.Templates 0.1.1 direct download](https://www.nuget.org/api/v2/package/SourceGear.Rust.NET.Templates/0.1.1)
- [SourceGear NuGet profile](https://www.nuget.org/profiles/SourceGear)
- [SourceGear project site](https://sourcegear.com/)

## Relationship To This Repo

The current repository does not try to reproduce the original SDK shell first.

Instead, it uses the historical package and blog posts as evidence while rebuilding the core compiler ideas with a smaller, test-first, cargo-first backend workflow. See [revived-design.md](revived-design.md) for that design.
