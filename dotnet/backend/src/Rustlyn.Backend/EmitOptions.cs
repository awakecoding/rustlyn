namespace Rustlyn.Backend;

/// <summary>
/// Options controlling assembly emission behavior.
/// </summary>
public sealed record EmitOptions
{
    /// <summary>Default options: no PDB emission.</summary>
    public static readonly EmitOptions Default = new();

    /// <summary>Whether to emit a Portable PDB alongside the assembly.</summary>
    public bool EmitPdb { get; init; }
}
