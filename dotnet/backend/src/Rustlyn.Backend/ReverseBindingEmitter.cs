namespace Rustlyn.Backend;

/// <summary>
/// Generates .NET wrapper classes that expose translated Rust types as idiomatic .NET APIs.
/// This enables .NET code to consume Rust-translated assemblies naturally — with constructors,
/// properties, IDisposable patterns, and ToString overrides.
/// </summary>
public static class ReverseBindingEmitter
{
    /// <summary>
    /// Analyzes a set of translated functions and infers which ones form a coherent "type" API.
    /// Groups functions by naming convention: prefix_method → Type.Method.
    /// </summary>
    public static IReadOnlyList<InferredRustType> InferTypes(IReadOnlyList<LoweredFunction> functions)
    {
        ArgumentNullException.ThrowIfNull(functions);

        var groups = new Dictionary<string, List<LoweredFunction>>(StringComparer.Ordinal);

        foreach (var fn in functions)
        {
            var parts = fn.Name.Split('_', 2);
            if (parts.Length < 2) continue;

            var prefix = parts[0];
            if (!groups.ContainsKey(prefix))
                groups[prefix] = [];
            groups[prefix].Add(fn);
        }

        var result = new List<InferredRustType>();
        foreach (var (prefix, methods) in groups.OrderBy(kv => kv.Key, StringComparer.Ordinal))
        {
            if (methods.Count < 2) continue; // Need at least 2 methods to form a type

            var hasNew = methods.Any(m => m.Name.EndsWith("_new", StringComparison.Ordinal));
            var hasDrop = methods.Any(m => m.Name.EndsWith("_drop", StringComparison.Ordinal)
                || m.Name.EndsWith("_free", StringComparison.Ordinal));

            result.Add(new InferredRustType(
                RustPrefix: prefix,
                SuggestedDotNetName: ToPascalCase(prefix),
                Methods: methods,
                HasConstructor: hasNew,
                HasDestructor: hasDrop,
                ShouldImplementDisposable: hasDrop));
        }

        return result;
    }

    /// <summary>
    /// Generates a .NET class wrapper specification for an inferred Rust type.
    /// The specification describes what the wrapper class would look like
    /// without actually emitting IL (that's for a future EmitWrapperAssembly step).
    /// </summary>
    public static WrapperClassSpec GenerateWrapperSpec(InferredRustType inferredType)
    {
        var methods = new List<WrapperMethodSpec>();

        foreach (var fn in inferredType.Methods)
        {
            var shortName = fn.Name[(inferredType.RustPrefix.Length + 1)..];
            var dotNetName = ToPascalCase(shortName);

            var kind = shortName switch
            {
                "new" => WrapperMethodKind.Constructor,
                "drop" or "free" => WrapperMethodKind.Dispose,
                _ when fn.Parameters.Count == 0 && !string.Equals(fn.ReturnType, "void", StringComparison.Ordinal)
                    => WrapperMethodKind.Property,
                _ => WrapperMethodKind.Method,
            };

            methods.Add(new WrapperMethodSpec(
                RustFunctionName: fn.Name,
                DotNetName: dotNetName,
                Kind: kind,
                ParameterCount: fn.Parameters.Count,
                ReturnType: fn.ReturnType));
        }

        return new WrapperClassSpec(
            Namespace: "Rustlyn.Generated",
            ClassName: inferredType.SuggestedDotNetName,
            ImplementsDisposable: inferredType.ShouldImplementDisposable,
            Methods: methods);
    }

    private static string ToPascalCase(string snakeCase)
    {
        var parts = snakeCase.Split('_', StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(parts.Select(p =>
            p.Length == 0 ? "" : char.ToUpperInvariant(p[0]) + p[1..]));
    }
}

/// <summary>
/// A Rust type inferred from translated function naming patterns.
/// </summary>
public sealed record InferredRustType(
    string RustPrefix,
    string SuggestedDotNetName,
    IReadOnlyList<LoweredFunction> Methods,
    bool HasConstructor,
    bool HasDestructor,
    bool ShouldImplementDisposable);

/// <summary>
/// Specification for a .NET wrapper class wrapping a translated Rust type.
/// </summary>
public sealed record WrapperClassSpec(
    string Namespace,
    string ClassName,
    bool ImplementsDisposable,
    IReadOnlyList<WrapperMethodSpec> Methods);

/// <summary>
/// Specification for a single method in a wrapper class.
/// </summary>
public sealed record WrapperMethodSpec(
    string RustFunctionName,
    string DotNetName,
    WrapperMethodKind Kind,
    int ParameterCount,
    string ReturnType);

/// <summary>
/// The kind of wrapper method (determines how it's exposed in .NET).
/// </summary>
public enum WrapperMethodKind
{
    Constructor,
    Method,
    Property,
    Dispose,
}
