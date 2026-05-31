using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rustlyn.Bindings;

public static class BindingManifestGenerator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string GenerateText(BindingSurface surface)
    {
        ArgumentNullException.ThrowIfNull(surface);
        return GenerateText(BindingManifestFactory.FromSurface(surface));
    }

    public static string GenerateText(BindingManifestDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var builder = new StringBuilder();
        builder.AppendLine("# Rustlyn binding manifest");
        builder.AppendLine($"manifest-schema: {document.FormatVersion.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"handle-abi: {document.Compatibility.HandleAbiVersion.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"symbol-schema: {document.Compatibility.SymbolSchemaVersion.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"projection-model: {document.Compatibility.ProjectionModel}");
        builder.AppendLine($"error-model: {document.Compatibility.ErrorModel}");
        builder.AppendLine($"ownership-model: {document.Compatibility.OwnershipModel}");
        builder.AppendLine($"async-model: {document.Compatibility.AsyncModel}");
        if (document.TargetFramework is not null)
        {
            builder.AppendLine($"target-framework: {document.TargetFramework}");
        }

        builder.AppendLine($"requirements: {document.Requirements.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"enums: {document.EnumProjections.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"bindings: {document.Bindings.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"unsupported-shapes: {document.UnsupportedShapes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        if (document.RuntimeSurface is not null)
        {
            builder.AppendLine($"runtime-types: {document.RuntimeSurface.Types.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            builder.AppendLine($"runtime-projected-members: {document.RuntimeSurface.Coverage.ProjectedMemberCount.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        }

        builder.AppendLine();

        foreach (var enumProjection in document.EnumProjections)
        {
            builder.AppendLine($"enum: {enumProjection.RustName}");
            builder.AppendLine($"  managed-type: {enumProjection.ManagedType}");
            builder.AppendLine($"  backing: {enumProjection.BackingType}");
            builder.AppendLine($"  flags: {enumProjection.IsFlags.ToString(System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant()}");
            foreach (var variant in enumProjection.Variants)
            {
                builder.AppendLine($"  variant: {variant.Name} = {variant.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }

            builder.AppendLine();
        }

        foreach (var binding in document.Bindings)
        {
            builder.AppendLine($"symbol: {binding.Symbol}");
            builder.AppendLine($"  helper: {binding.Helper}");
            builder.AppendLine($"  return: {binding.ReturnType}");
            builder.AppendLine($"  parameters: {FormatParameters(binding.Parameters)}");
            builder.AppendLine($"  exception: {FormatException(binding.Exception)}");
            builder.AppendLine($"  managed-target: {binding.ManagedTarget}");
            builder.AppendLine($"  rust-extern: {binding.RustExtern}");

            foreach (var wrapper in binding.Wrappers)
            {
                builder.AppendLine($"  wrapper: {wrapper.Signature}");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }

    public static string GenerateJson(BindingSurface surface)
        => GenerateJson(surface, []);

    public static string GenerateJson(BindingSurface surface, IReadOnlyList<BindingScanUnsupportedShape> unsupportedShapes)
    {
        ArgumentNullException.ThrowIfNull(surface);
        ArgumentNullException.ThrowIfNull(unsupportedShapes);

        var document = BindingManifestFactory.FromSurface(surface, unsupportedShapes);
        return GenerateJson(document);
    }

    public static string GenerateJson(BindingManifestDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        return JsonSerializer.Serialize(document, JsonOptions) + Environment.NewLine;
    }

    private static string FormatParameters(IReadOnlyList<BindingManifestParameter> parameters)
    {
        if (parameters.Count == 0)
        {
            return "(none)";
        }

        return string.Join(", ", parameters.Select(static parameter =>
            parameter.RustAbiType is null
                ? $"{parameter.Type} {parameter.Name}"
                : $"{parameter.Type} {parameter.Name} rust-abi:{parameter.RustAbiType}"));
    }

    private static string FormatException(BindingManifestExceptionConvention exception)
        => exception.OutParameter is null
            ? exception.Kind
            : $"{exception.Kind}:{exception.OutParameter}";
}
