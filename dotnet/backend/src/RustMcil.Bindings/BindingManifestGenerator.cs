using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RustMcil.Bindings;

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

        foreach (var requirement in surface.Requirements)
        {
            requirement.Validate();
        }

        foreach (var enumProjection in surface.RustEnumProjections)
        {
            enumProjection.Validate();
        }

        var wrappersBySymbol = surface.RustWrapperMethods
            .GroupBy(static method => method.ExternSymbol, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.ToArray(), StringComparer.Ordinal);

        var builder = new StringBuilder();
        builder.AppendLine("# RustMcil binding manifest");
        builder.AppendLine($"requirements: {surface.Requirements.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"enums: {surface.RustEnumProjections.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"bindings: {surface.ManagedGlueBindings.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine();

        foreach (var enumProjection in surface.RustEnumProjections)
        {
            builder.AppendLine($"enum: {enumProjection.RustName}");
            builder.AppendLine($"  managed-type: {FormatTypeName(enumProjection.ManagedType)}");
            builder.AppendLine("  backing: int");
            builder.AppendLine($"  flags: {enumProjection.IsFlags.ToString(System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant()}");
            foreach (var variant in enumProjection.Variants)
            {
                builder.AppendLine($"  variant: {variant.Name} = {variant.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }

            builder.AppendLine();
        }

        foreach (var binding in surface.ManagedGlueBindings)
        {
            binding.Validate();
            var externBinding = RustExternBinding.FromManagedGlueBinding(binding);

            builder.AppendLine($"symbol: {binding.Symbol}");
            builder.AppendLine($"  helper: {binding.RuntimeBridgeHelperMethodName}");
            builder.AppendLine($"  return: {binding.ReturnType}");
            builder.AppendLine($"  parameters: {FormatParameters(binding.Parameters)}");
            builder.AppendLine($"  exception: {FormatExceptionConvention(binding.Operation)}");
            builder.AppendLine($"  managed-target: {FormatManagedTarget(binding.Operation.Result)}");
            builder.AppendLine($"  rust-extern: {string.Join(" ", externBinding.SignatureLines.Select(static line => line.Trim()))}");

            if (wrappersBySymbol.TryGetValue(binding.Symbol, out var wrappers))
            {
                foreach (var wrapper in wrappers)
                {
                    builder.AppendLine($"  wrapper: {wrapper.Signature}");
                }
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

        foreach (var requirement in surface.Requirements)
        {
            requirement.Validate();
        }

        foreach (var enumProjection in surface.RustEnumProjections)
        {
            enumProjection.Validate();
        }

        var wrappersBySymbol = surface.RustWrapperMethods
            .GroupBy(static method => method.ExternSymbol, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.ToArray(), StringComparer.Ordinal);
        var requirements = surface.Requirements
            .Select(static requirement => new JsonRequirement(
                requirement.DisplayName,
                requirement.Kind.ToString(),
                FormatTypeName(requirement.Type),
                JsonAssemblyIdentity.From(requirement.Type.Assembly.GetName()),
                requirement.MemberName,
                requirement.ParameterTypes.Select(FormatTypeName).ToArray()))
            .ToArray();
        var managedAssemblies = requirements
            .Select(static requirement => requirement.Assembly)
            .Distinct()
            .OrderBy(static assembly => assembly.Name, StringComparer.Ordinal)
            .ThenBy(static assembly => assembly.Version, StringComparer.Ordinal)
            .ToArray();
        var bindings = surface.ManagedGlueBindings
            .Select(binding => CreateJsonBinding(binding, wrappersBySymbol))
            .ToArray();
        var enumProjections = surface.RustEnumProjections
            .Select(static enumProjection => new JsonEnumProjection(
                enumProjection.RustName,
                FormatTypeName(enumProjection.ManagedType),
                "int",
                enumProjection.IsFlags,
                enumProjection.Variants.Select(static variant => new JsonEnumVariant(variant.Name, variant.Value)).ToArray()))
            .ToArray();
        var document = new JsonManifest(
            FormatVersion: 1,
            GeneratedBy: "RustMcil.Bindings",
            ManagedAssemblies: managedAssemblies,
            Requirements: requirements,
            EnumProjections: enumProjections,
            Bindings: bindings,
            UnsupportedShapes: unsupportedShapes.Select(static shape => new JsonUnsupportedShape(
                shape.DisplayName,
                shape.Kind.ToString(),
                shape.MemberName,
                shape.Reason)).ToArray());

        return JsonSerializer.Serialize(document, JsonOptions) + Environment.NewLine;
    }

    private static JsonBinding CreateJsonBinding(ManagedGlueBinding binding, IReadOnlyDictionary<string, RustWrapperMethod[]> wrappersBySymbol)
    {
        binding.Validate();
        var externBinding = RustExternBinding.FromManagedGlueBinding(binding);
        var wrappers = wrappersBySymbol.TryGetValue(binding.Symbol, out var bindingWrappers)
            ? bindingWrappers.Select(CreateJsonWrapper).ToArray()
            : [];

        return new JsonBinding(
            binding.Symbol,
            binding.RuntimeBridgeHelperMethodName,
            binding.ReturnType,
            binding.Parameters.Select(static parameter => new JsonParameter(parameter.TypeName, parameter.Name, parameter.RustAbiType)).ToArray(),
            new JsonExceptionConvention(
                FormatExceptionConventionKind(binding.Operation.ExceptionConvention),
                binding.Operation.ExceptionOutParameterName),
            FormatManagedTarget(binding.Operation.Result),
            FormatRustExternSignature(externBinding),
            wrappers);
    }

    private static JsonWrapper CreateJsonWrapper(RustWrapperMethod wrapper)
    {
        return new JsonWrapper(
            wrapper.Container.ToString(),
            FormatRustWrapperPath(wrapper.Container, ExtractRustWrapperMethodName(wrapper.Signature)),
            wrapper.Signature,
            wrapper.Result.Kind.ToString(),
            wrapper.Result.RustType);
    }

    private static string FormatParameters(IReadOnlyList<ManagedGlueParameter> parameters)
    {
        if (parameters.Count == 0)
        {
            return "(none)";
        }

        return string.Join(", ", parameters.Select(static parameter =>
            parameter.RustAbiType is null
                ? $"{parameter.TypeName} {parameter.Name}"
                : $"{parameter.TypeName} {parameter.Name} rust-abi:{parameter.RustAbiType}"));
    }

    private static string FormatExceptionConvention(ManagedGlueOperation operation)
    {
        return operation.ExceptionConvention switch
        {
            ManagedGlueExceptionConvention.ReturnExceptionHandle => "return-exception-handle",
            ManagedGlueExceptionConvention.WriteExceptionOut => $"write-exception-out:{operation.ExceptionOutParameterName}",
            _ => throw new NotSupportedException($"Managed glue exception convention '{operation.ExceptionConvention}' is not supported.")
        };
    }

    private static string FormatExceptionConventionKind(ManagedGlueExceptionConvention convention)
    {
        return convention switch
        {
            ManagedGlueExceptionConvention.ReturnExceptionHandle => "return-exception-handle",
            ManagedGlueExceptionConvention.WriteExceptionOut => "write-exception-out",
            _ => throw new NotSupportedException($"Managed glue exception convention '{convention}' is not supported.")
        };
    }

    private static string FormatManagedTarget(ManagedGlueResult result)
    {
        return result switch
        {
            ManagedGlueVoidCallResult voidCall => voidCall.CallExpression.ToCode(),
            ManagedGlueObjectHandleResult objectHandle => objectHandle.ValueExpression.ToCode(),
            ManagedGlueIntResult integer => integer.ValueExpression.ToCode(),
            ManagedGlueLongResult integer64 => integer64.ValueExpression.ToCode(),
            ManagedGlueFloatResult single => single.ValueExpression.ToCode(),
            ManagedGlueDoubleResult doublePrecision => doublePrecision.ValueExpression.ToCode(),
            ManagedGlueBooleanAsIntResult boolean => boolean.ValueExpression.ToCode(),
            ManagedGlueReleaseObjectResult releaseObject => $"ManagedInteropRuntime.ReleaseObject({releaseObject.HandleExpression})",
            ManagedGlueReleaseExceptionResult releaseException => $"ManagedInteropRuntime.ReleaseException({releaseException.HandleExpression})",
            _ => throw new NotSupportedException($"Managed glue result '{result.GetType().Name}' is not supported.")
        };
    }

    private static string FormatRustWrapperPath(RustWrapperContainer container, string methodName)
    {
        return container switch
        {
            RustWrapperContainer.Callback => $"system::callback::{methodName}",
            RustWrapperContainer.Console => $"system::console::{methodName}",
            RustWrapperContainer.Environment => $"system::environment::{methodName}",
            RustWrapperContainer.Exception => $"system::Exception::{methodName}",
            RustWrapperContainer.Math => $"system::math::{methodName}",
            RustWrapperContainer.MathF => $"system::mathf::{methodName}",
            RustWrapperContainer.TimeSpan => $"system::time_span::{methodName}",
            RustWrapperContainer.DateTimeOffset => $"system::date_time_offset::{methodName}",
            RustWrapperContainer.Guid => $"system::guid::{methodName}",
            RustWrapperContainer.IoDirectory => $"system::io::directory::{methodName}",
            RustWrapperContainer.IoFile => $"system::io::file::{methodName}",
            RustWrapperContainer.IoPath => $"system::io::path::{methodName}",
            RustWrapperContainer.ManagedString => $"system::ManagedString::{methodName}",
            RustWrapperContainer.ManagedStringArray => $"system::ManagedStringArray::{methodName}",
            RustWrapperContainer.ManagedIntArray => $"system::ManagedIntArray::{methodName}",
            RustWrapperContainer.ManagedByteArray => $"system::ManagedByteArray::{methodName}",
            RustWrapperContainer.ManagedTimeSpan => $"system::ManagedTimeSpan::{methodName}",
            RustWrapperContainer.ManagedDateTimeOffset => $"system::ManagedDateTimeOffset::{methodName}",
            RustWrapperContainer.ManagedGuid => $"system::ManagedGuid::{methodName}",
            _ => throw new NotSupportedException($"Rust wrapper container '{container}' is not supported.")
        };
    }

    private static string FormatRustExternSignature(RustExternBinding externBinding)
    {
        if (externBinding.SignatureLines.Count == 1)
        {
            return externBinding.SignatureLines[0];
        }

        var firstLine = externBinding.SignatureLines[0].TrimEnd();
        var lastLine = externBinding.SignatureLines[^1].TrimStart();
        var parameters = externBinding.SignatureLines
            .Skip(1)
            .Take(externBinding.SignatureLines.Count - 2)
            .Select(static line => line.Trim().TrimEnd(','));
        return $"{firstLine}{string.Join(", ", parameters)}{lastLine}";
    }

    private static string ExtractRustWrapperMethodName(string signature)
    {
        const string prefix = "pub fn ";
        if (!signature.StartsWith(prefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Rust wrapper signature '{signature}' does not start with '{prefix}'.");
        }

        var start = prefix.Length;
        var end = signature.IndexOf('(', start);
        if (end < 0)
        {
            throw new InvalidOperationException($"Rust wrapper signature '{signature}' does not include a parameter list.");
        }

        return signature[start..end];
    }

    private static string FormatTypeName(Type type)
        => type.FullName?.Replace('+', '.') ?? type.Name;

    private sealed record JsonManifest(
        int FormatVersion,
        string GeneratedBy,
        IReadOnlyList<JsonAssemblyIdentity> ManagedAssemblies,
        IReadOnlyList<JsonRequirement> Requirements,
        IReadOnlyList<JsonEnumProjection> EnumProjections,
        IReadOnlyList<JsonBinding> Bindings,
        IReadOnlyList<JsonUnsupportedShape> UnsupportedShapes);

    private sealed record JsonAssemblyIdentity(
        string Name,
        string? Version,
        string? CultureName,
        string PublicKeyToken)
    {
        public static JsonAssemblyIdentity From(System.Reflection.AssemblyName assemblyName)
        {
            var publicKeyToken = assemblyName.GetPublicKeyToken();
            return new JsonAssemblyIdentity(
                assemblyName.Name ?? string.Empty,
                assemblyName.Version?.ToString(),
                assemblyName.CultureName,
                publicKeyToken is { Length: > 0 }
                    ? Convert.ToHexString(publicKeyToken).ToLowerInvariant()
                    : string.Empty);
        }
    }

    private sealed record JsonRequirement(
        string DisplayName,
        string Kind,
        string Type,
        JsonAssemblyIdentity Assembly,
        string? MemberName,
        IReadOnlyList<string> ParameterTypes);

    private sealed record JsonEnumProjection(
        string RustName,
        string ManagedType,
        string BackingType,
        bool IsFlags,
        IReadOnlyList<JsonEnumVariant> Variants);

    private sealed record JsonEnumVariant(string Name, int Value);

    private sealed record JsonBinding(
        string Symbol,
        string Helper,
        string ReturnType,
        IReadOnlyList<JsonParameter> Parameters,
        JsonExceptionConvention Exception,
        string ManagedTarget,
        string RustExtern,
        IReadOnlyList<JsonWrapper> Wrappers);

    private sealed record JsonParameter(string Type, string Name, [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? RustAbiType);

    private sealed record JsonExceptionConvention(string Kind, string? OutParameter);

    private sealed record JsonWrapper(
        string Container,
        string Path,
        string Signature,
        string ResultKind,
        string? RustType);

    private sealed record JsonUnsupportedShape(
        string DisplayName,
        string Kind,
        string? MemberName,
        string Reason);
}
