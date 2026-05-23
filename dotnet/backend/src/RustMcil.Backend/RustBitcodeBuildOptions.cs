namespace RustMcil.Backend;

public sealed record RustBitcodeBuildOptions
{
    public bool Release { get; init; } = true;

    public string? OutputBitcodePath { get; init; }

    public string? BinaryTargetName { get; init; }

    public string? Toolchain { get; init; }

    public string? Target { get; init; }

    public string? BuildStd { get; init; }

    public string? BuildStdFeatures { get; init; }

    public bool PanicAbort { get; init; } = true;

    /// <summary>
    /// When set, passes -C lto={value} and -C codegen-units=1 to rustc.
    /// Use "fat" to merge all dependencies (including build-std) into one bitcode file.
    /// </summary>
    public string? Lto { get; init; }
}