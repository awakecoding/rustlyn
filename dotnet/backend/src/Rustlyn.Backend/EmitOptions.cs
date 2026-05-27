namespace Rustlyn.Backend;

/// <summary>
/// Options controlling assembly emission behavior.
/// </summary>
public sealed record EmitOptions
{
    /// <summary>Default options: no PDB emission, permissive mode.</summary>
    public static readonly EmitOptions Default = new();

    /// <summary>Whether to emit a Portable PDB alongside the assembly.</summary>
    public bool EmitPdb { get; init; }

    /// <summary>
    /// When true, unsupported IR causes emission to fail with a structured diagnostic
    /// instead of stubbing function bodies with a throwing <see cref="System.NotSupportedException"/>.
    /// Use this for production translation; keep it off for exploratory/fixture work.
    /// </summary>
    public bool StrictUnsupportedIr { get; init; }

    /// <summary>
    /// Additional binding manifests whose generated symbols should be mapped to runtime bridge helpers.
    /// The generated helper implementations must be supplied by the runtime support assembly used to run
    /// the emitted program.
    /// </summary>
    public System.Collections.Generic.IReadOnlyList<Rustlyn.Bindings.BindingManifestDocument> BindingManifests { get; init; } = [];
}

/// <summary>
/// Aggregated diagnostic raised when <see cref="EmitOptions.StrictUnsupportedIr"/> is set and the
/// backend encountered IR shapes it cannot translate. The message lists every offending function
/// with the inner reason so users can map failures back to fixtures or roadmap entries.
/// </summary>
public sealed class UnsupportedIrException : System.Exception
{
    public UnsupportedIrException(System.Collections.Generic.IReadOnlyList<UnsupportedIrFunction> functions)
        : base(BuildMessage(functions))
    {
        Functions = functions;
    }

    public System.Collections.Generic.IReadOnlyList<UnsupportedIrFunction> Functions { get; }

    private static string BuildMessage(System.Collections.Generic.IReadOnlyList<UnsupportedIrFunction> functions)
    {
        var builder = new System.Text.StringBuilder();
        builder.Append(functions.Count);
        builder.Append(functions.Count == 1 ? " function" : " functions");
        builder.AppendLine(" could not be translated (strict mode):");
        foreach (var function in functions)
        {
            builder.Append("  ");
            builder.Append(function.Name);
            builder.Append(": ");
            builder.AppendLine(function.Reason);
        }
        return builder.ToString().TrimEnd();
    }
}

public sealed record UnsupportedIrFunction(string Name, string Reason);
