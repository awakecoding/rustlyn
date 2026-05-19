namespace RustMcil.Backend;

public sealed record RustBitcodeBuildOptions
{
    public bool Release { get; init; } = true;

    public string? OutputBitcodePath { get; init; }

    public string? BinaryTargetName { get; init; }

    public string? Toolchain { get; init; }

    public string? Target { get; init; }

    public string? BuildStd { get; init; }

    public bool PanicAbort { get; init; } = true;
}