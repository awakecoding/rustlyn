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
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] BindingManifestPackageSurface? PackageSurface = null);

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

public sealed record RuntimeUnsupportedReasonCount(string Reason, int Count);
