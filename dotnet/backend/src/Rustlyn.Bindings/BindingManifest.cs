using System.Reflection;
using System.Text.Json.Serialization;

namespace Rustlyn.Bindings;

public static class BindingManifestVersions
{
    public const int ManifestFormatVersion = 3;
    public const int HandleAbiVersion = 1;
    public const int SymbolSchemaVersion = 1;
    public const int PackFormatVersion = 3;
}

public sealed record BindingManifestDocument(
    int FormatVersion,
    string GeneratedBy,
    BindingManifestCompatibility Compatibility,
    IReadOnlyList<BindingManifestAssemblyIdentity> ManagedAssemblies,
    IReadOnlyList<BindingManifestRequirement> Requirements,
    IReadOnlyList<BindingManifestEnumProjection> EnumProjections,
    IReadOnlyList<BindingManifestBinding> Bindings,
    IReadOnlyList<BindingManifestUnsupportedShape> UnsupportedShapes,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? TargetFramework = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] BindingManifestRuntimeSurface? RuntimeSurface = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] BindingManifestPackageSurface? PackageSurface = null)
{
    public static BindingManifestDocument FromSurface(BindingSurface surface)
        => FromSurface(surface, []);

    public static BindingManifestDocument FromSurface(
        BindingSurface surface,
        IReadOnlyList<BindingScanUnsupportedShape> unsupportedShapes)
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
            .Select(static (method, index) => (Method: method, Order: index))
            .GroupBy(static item => item.Method.ExternSymbol, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.ToArray(), StringComparer.Ordinal);
        var requirements = surface.Requirements
            .Select(static requirement => new BindingManifestRequirement(
                requirement.DisplayName,
                requirement.Kind.ToString(),
                BindingManifestFormatting.FormatTypeName(requirement.Type),
                BindingManifestAssemblyIdentity.From(requirement.Type.Assembly.GetName()),
                requirement.MemberName,
                requirement.ParameterTypes.Select(BindingManifestFormatting.FormatTypeName).ToArray()))
            .ToArray();
        var managedAssemblies = requirements
            .Select(static requirement => requirement.Assembly)
            .Distinct()
            .OrderBy(static assembly => assembly.Name, StringComparer.Ordinal)
            .ThenBy(static assembly => assembly.Version, StringComparer.Ordinal)
            .ToArray();
        var enumProjections = surface.RustEnumProjections
            .Select(static enumProjection => new BindingManifestEnumProjection(
                enumProjection.RustName,
                BindingManifestFormatting.FormatTypeName(enumProjection.ManagedType),
                "int",
                enumProjection.IsFlags,
                enumProjection.Variants.Select(static variant => new BindingManifestEnumVariant(variant.Name, variant.Value)).ToArray()))
            .ToArray();
        var bindings = surface.ManagedGlueBindings
            .Select(binding => CreateBinding(binding, wrappersBySymbol))
            .ToArray();

        return new BindingManifestDocument(
            BindingManifestVersions.ManifestFormatVersion,
            "Rustlyn.Bindings",
            BindingManifestCompatibility.Current,
            managedAssemblies,
            requirements,
            enumProjections,
            bindings,
            unsupportedShapes.Select(static shape => new BindingManifestUnsupportedShape(
                shape.DisplayName,
                shape.Kind.ToString(),
                shape.MemberName,
                shape.Reason)).ToArray());
    }

    public static BindingManifestDocument FromExternalPackageSurface(
        BindingSurface surface,
        BindingManifestPackageSurface packageSurface,
        IReadOnlyList<BindingScanUnsupportedShape>? unsupportedShapes = null)
    {
        ArgumentNullException.ThrowIfNull(packageSurface);
        return FromSurface(surface, unsupportedShapes ?? []) with { PackageSurface = packageSurface };
    }

    public static BindingManifestDocument FromRuntimeSurface(RuntimeSurfaceScanReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        var managedAssemblies = report.Assemblies
            .Select(static assembly => assembly.Identity)
            .Distinct()
            .OrderBy(static assembly => assembly.Name, StringComparer.Ordinal)
            .ThenBy(static assembly => assembly.Version, StringComparer.Ordinal)
            .ToArray();
        var runtimeAssemblies = report.Assemblies
            .Select(static assembly => new BindingManifestRuntimeAssembly(
                assembly.AssemblyName,
                assembly.Identity,
                assembly.AssemblyPath,
                assembly.ExportedTypeCount,
                assembly.ScannedTypeCount,
                assembly.SkippedTypeCount,
                assembly.ProjectedMemberCount,
                assembly.UnsupportedShapeCount,
                assembly.LoadDiagnostic))
            .OrderBy(static assembly => assembly.Name, StringComparer.Ordinal)
            .ToArray();
        var runtimeSurface = new BindingManifestRuntimeSurface(
            report.TargetFramework,
            report.PackVersion,
            report.PackRoot,
            report.RefDirectory,
            new BindingManifestRuntimeCoverage(
                report.AssemblyCount,
                report.ExportedTypeCount,
                report.ScannedTypeCount,
                report.SkippedTypeCount,
                report.PublicMethodCount,
                report.PublicPropertyCount,
                report.PublicEventCount,
                report.PublicConstructorCount,
                report.ProjectedRequirementCount,
                report.ProjectedMemberCount,
                report.UnsupportedShapeCount,
                report.SkippedTypesByReason
                    .OrderBy(static pair => pair.Key, StringComparer.Ordinal)
                    .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal),
                report.UnsupportedShapesByReason),
            runtimeAssemblies,
            report.Types);

        return new BindingManifestDocument(
            BindingManifestVersions.ManifestFormatVersion,
            "Rustlyn.Bindings",
            BindingManifestCompatibility.Current,
            managedAssemblies,
            Requirements: [],
            EnumProjections: [],
            Bindings: [],
            report.UnsupportedShapes.Select(static shape => new BindingManifestUnsupportedShape(
                shape.DisplayName,
                shape.Kind.ToString(),
                shape.MemberName,
                shape.Reason)).ToArray(),
            report.TargetFramework,
            runtimeSurface);
    }

    private static BindingManifestBinding CreateBinding(
        ManagedGlueBinding binding,
        IReadOnlyDictionary<string, (RustWrapperMethod Method, int Order)[]> wrappersBySymbol)
    {
        binding.Validate();
        var externBinding = RustExternBinding.FromManagedGlueBinding(binding);
        var wrappers = wrappersBySymbol.TryGetValue(binding.Symbol, out var bindingWrappers)
            ? bindingWrappers.Select(static item => CreateWrapper(item.Method, item.Order)).ToArray()
            : [];

        return new BindingManifestBinding(
            binding.Symbol,
            binding.RuntimeBridgeHelperMethodName,
            binding.ReturnType,
            BindingManifestFormatting.FormatManagedResultKind(binding.Operation.Result),
            binding.Parameters.Select(static parameter => new BindingManifestParameter(parameter.TypeName, parameter.Name, parameter.RustAbiType)).ToArray(),
            new BindingManifestExceptionConvention(
                BindingManifestFormatting.FormatExceptionConventionKind(binding.Operation.ExceptionConvention),
                binding.Operation.ExceptionOutParameterName),
            BindingManifestFormatting.FormatManagedTarget(binding.Operation.Result),
            BindingManifestFormatting.FormatRustExternSignature(externBinding),
                externBinding.SignatureLines,
                wrappers);
    }

    private static BindingManifestWrapper CreateWrapper(RustWrapperMethod wrapper, int order)
    {
        return new BindingManifestWrapper(
            wrapper.Container.ToString(),
            BindingManifestFormatting.FormatRustWrapperPath(wrapper.Container, BindingManifestFormatting.ExtractRustWrapperMethodName(wrapper.Signature)),
            wrapper.Signature,
            wrapper.Arguments,
            wrapper.ResultVariableName,
            wrapper.Result.Kind.ToString(),
            wrapper.Result.RustType,
            order);
    }
}

public sealed record BindingManifestCompatibility(
    int HandleAbiVersion,
    int SymbolSchemaVersion,
    string ProjectionModel,
    string ErrorModel,
    string OwnershipModel,
    string AsyncModel)
{
    public static BindingManifestCompatibility Current { get; } = new(
        BindingManifestVersions.HandleAbiVersion,
        BindingManifestVersions.SymbolSchemaVersion,
        "thin-projection-plus-optional-facade",
        "result-by-default",
        "explicit-owned-and-borrowed-handles",
        "task-future-via-task-handle-and-waker-bridge");
}

public sealed record BindingManifestAssemblyIdentity(
    string Name,
    string? Version,
    string? CultureName,
    string PublicKeyToken)
{
    public static BindingManifestAssemblyIdentity From(AssemblyName assemblyName)
    {
        var publicKeyToken = assemblyName.GetPublicKeyToken();
        return new BindingManifestAssemblyIdentity(
            assemblyName.Name ?? string.Empty,
            assemblyName.Version?.ToString(),
            assemblyName.CultureName,
            publicKeyToken is { Length: > 0 }
                ? Convert.ToHexString(publicKeyToken).ToLowerInvariant()
                : string.Empty);
    }
}

public sealed record BindingManifestRequirement(
    string DisplayName,
    string Kind,
    string Type,
    BindingManifestAssemblyIdentity Assembly,
    string? MemberName,
    IReadOnlyList<string> ParameterTypes);

public sealed record BindingManifestEnumProjection(
    string RustName,
    string ManagedType,
    string BackingType,
    bool IsFlags,
    IReadOnlyList<BindingManifestEnumVariant> Variants);

public sealed record BindingManifestEnumVariant(string Name, int Value);

public sealed record BindingManifestBinding(
    string Symbol,
    string Helper,
    string ReturnType,
    string ManagedResultKind,
    IReadOnlyList<BindingManifestParameter> Parameters,
    BindingManifestExceptionConvention Exception,
    string ManagedTarget,
    string RustExtern,
    IReadOnlyList<string> RustExternLines,
    IReadOnlyList<BindingManifestWrapper> Wrappers);

public sealed record BindingManifestParameter(
    string Type,
    string Name,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? RustAbiType);

public sealed record BindingManifestExceptionConvention(string Kind, string? OutParameter);

public sealed record BindingManifestWrapper(
    string Container,
    string Path,
    string Signature,
    IReadOnlyList<string> CallArguments,
    string ResultVariableName,
    string ResultKind,
    string? RustType,
    int Order);

public sealed record BindingManifestUnsupportedShape(
    string DisplayName,
    string Kind,
    string? MemberName,
    string Reason);

public sealed record BindingManifestRuntimeSurface(
    string TargetFramework,
    string ReferencePackVersion,
    string ReferencePackRoot,
    string ReferenceDirectory,
    BindingManifestRuntimeCoverage Coverage,
    IReadOnlyList<BindingManifestRuntimeAssembly> Assemblies,
    IReadOnlyList<BindingManifestRuntimeType> Types);

public sealed record BindingManifestRuntimeCoverage(
    int AssemblyCount,
    int ExportedTypeCount,
    int ScannedTypeCount,
    int SkippedTypeCount,
    int PublicMethodCount,
    int PublicPropertyCount,
    int PublicEventCount,
    int PublicConstructorCount,
    int ProjectedRequirementCount,
    int ProjectedMemberCount,
    int UnsupportedShapeCount,
    IReadOnlyDictionary<string, int> SkippedTypesByReason,
    IReadOnlyList<RuntimeUnsupportedReasonCount> UnsupportedShapesByReason);

public sealed record BindingManifestRuntimeAssembly(
    string Name,
    BindingManifestAssemblyIdentity Identity,
    string Path,
    int ExportedTypeCount,
    int ScannedTypeCount,
    int SkippedTypeCount,
    int ProjectedMemberCount,
    int UnsupportedShapeCount,
    string? LoadDiagnostic);

public sealed record BindingManifestPackageSurface(
    string PackageId,
    string PackageVersion,
    string TargetFramework,
    IReadOnlyList<BindingManifestPackageAssembly> Assemblies,
    IReadOnlyList<BindingManifestPackageDependency> Dependencies,
    IReadOnlyList<string> RuntimeAssetPatterns);

public sealed record BindingManifestPackageAssembly(
    string Name,
    BindingManifestAssemblyIdentity Identity,
    string Role,
    string Path);

public sealed record BindingManifestPackageDependency(
    string PackageId,
    string Version);

public sealed record BindingManifestRuntimeType(
    BindingManifestRuntimeTypeIdentity Identity,
    string Namespace,
    string Name,
    string FullName,
    string Kind,
    int GenericArity,
    bool IsNested,
    string ProjectionStatus,
    string ProjectionPolicy,
    string? UnsupportedReason,
    IReadOnlyList<string> Attributes,
    IReadOnlyList<BindingManifestRuntimeMember> Members);

public sealed record BindingManifestRuntimeTypeIdentity(
    string AssemblyName,
    string FullName,
    int MetadataToken,
    int GenericArity);

public sealed record BindingManifestRuntimeMember(
    BindingManifestRuntimeMemberIdentity Identity,
    string Kind,
    string Name,
    string DisplayName,
    string Signature,
    string ProjectionStatus,
    string ProjectionPolicy,
    string? UnsupportedReason,
    string? UnsupportedReasonCode,
    bool IsStatic,
    int GenericArity,
    string? ReturnType,
    string ReturnNullability,
    IReadOnlyList<BindingManifestRuntimeParameter> Parameters,
    IReadOnlyList<string> Attributes);

public sealed record BindingManifestRuntimeMemberIdentity(
    string DeclaringType,
    string Name,
    string Kind,
    int MetadataToken,
    string SignatureKey);

public sealed record BindingManifestRuntimeParameter(
    string Name,
    int Position,
    string Type,
    string Nullability,
    bool IsOut,
    bool IsOptional,
    IReadOnlyList<string> Attributes);

internal static class BindingManifestFormatting
{
    public static string FormatExceptionConvention(ManagedGlueOperation operation)
    {
        return operation.ExceptionConvention switch
        {
            ManagedGlueExceptionConvention.ReturnExceptionHandle => "return-exception-handle",
            ManagedGlueExceptionConvention.WriteExceptionOut => $"write-exception-out:{operation.ExceptionOutParameterName}",
            _ => throw new NotSupportedException($"Managed glue exception convention '{operation.ExceptionConvention}' is not supported.")
        };
    }

    public static string FormatExceptionConventionKind(ManagedGlueExceptionConvention convention)
    {
        return convention switch
        {
            ManagedGlueExceptionConvention.ReturnExceptionHandle => "return-exception-handle",
            ManagedGlueExceptionConvention.WriteExceptionOut => "write-exception-out",
            _ => throw new NotSupportedException($"Managed glue exception convention '{convention}' is not supported.")
        };
    }

    public static string FormatManagedTarget(ManagedGlueResult result)
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

    public static string FormatManagedResultKind(ManagedGlueResult result)
    {
        return result switch
        {
            ManagedGlueVoidCallResult => "void-call",
            ManagedGlueObjectHandleResult => "object-handle",
            ManagedGlueIntResult => "int",
            ManagedGlueLongResult => "long",
            ManagedGlueFloatResult => "float",
            ManagedGlueDoubleResult => "double",
            ManagedGlueBooleanAsIntResult => "boolean-as-int",
            ManagedGlueReleaseObjectResult => "release-object",
            ManagedGlueReleaseExceptionResult => "release-exception",
            _ => throw new NotSupportedException($"Managed glue result '{result.GetType().Name}' is not supported.")
        };
    }

    public static string FormatRustWrapperPath(RustWrapperContainer container, string methodName)
    {
        return container switch
        {
            RustWrapperContainer.Callback => $"system::callback::{methodName}",
            RustWrapperContainer.Console => $"system::console::{methodName}",
            RustWrapperContainer.Convert => $"system::convert::{methodName}",
            RustWrapperContainer.Environment => $"system::environment::{methodName}",
            RustWrapperContainer.Exception => $"system::Exception::{methodName}",
            RustWrapperContainer.Math => $"system::math::{methodName}",
            RustWrapperContainer.MathF => $"system::mathf::{methodName}",
            RustWrapperContainer.TimeSpan => $"system::time_span::{methodName}",
            RustWrapperContainer.DateTimeOffset => $"system::date_time_offset::{methodName}",
            RustWrapperContainer.Gc => $"system::gc::{methodName}",
            RustWrapperContainer.Guid => $"system::guid::{methodName}",
            RustWrapperContainer.Task => $"system::task::{methodName}",
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
            RustWrapperContainer.OperatingSystem => $"system::operating_system::{methodName}",
            RustWrapperContainer.Uri => $"system::uri::{methodName}",
            _ => throw new NotSupportedException($"Rust wrapper container '{container}' is not supported.")
        };
    }

    public static string FormatRustExternSignature(RustExternBinding externBinding)
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

    public static string ExtractRustWrapperMethodName(string signature)
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

    public static string FormatTypeName(Type type)
        => type.FullName?.Replace('+', '.') ?? type.Name;
}
