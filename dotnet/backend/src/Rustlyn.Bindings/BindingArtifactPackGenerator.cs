using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rustlyn.Bindings;

public static class BindingArtifactPackGenerator
{
    public const string RustModuleFileName = "system.rs";
    public const string ManagedGlueFileName = "RuntimeBridgeHelpers.Bindings.g.cs";
    public const string TextManifestFileName = "manifest.txt";
    public const string JsonManifestFileName = "manifest.json";
    public const string SummaryFileName = "artifact-summary.json";

    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static void WritePack(BindingSurface surface, string outputDirectory)
    {
        ArgumentNullException.ThrowIfNull(surface);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var outputFullPath = Path.GetFullPath(outputDirectory);
        Directory.CreateDirectory(outputFullPath);
        var manifest = BindingManifestFactory.FromSurface(surface);

        var artifacts = new[]
        {
            new ArtifactContent(RustModuleFileName, "rust-module", RustBindingGenerator.GenerateSystemModule(manifest)),
            new ArtifactContent(ManagedGlueFileName, "managed-glue", ManagedGlueGenerator.GenerateRuntimeBridgePartial(manifest)),
            new ArtifactContent(TextManifestFileName, "text-manifest", BindingManifestGenerator.GenerateText(manifest)),
            new ArtifactContent(JsonManifestFileName, "json-manifest", BindingManifestGenerator.GenerateJson(manifest))
        };

        foreach (var artifact in artifacts)
        {
            File.WriteAllText(Path.Combine(outputFullPath, artifact.Path), artifact.Content, Utf8NoBom);
        }

        var summary = CreateSummary(manifest, artifacts);
        File.WriteAllText(Path.Combine(outputFullPath, SummaryFileName), summary, Utf8NoBom);
    }

    public static string GenerateSummary(BindingSurface surface)
    {
        ArgumentNullException.ThrowIfNull(surface);
        var manifest = BindingManifestFactory.FromSurface(surface);
        var artifacts = new[]
        {
            new ArtifactContent(RustModuleFileName, "rust-module", RustBindingGenerator.GenerateSystemModule(manifest)),
            new ArtifactContent(ManagedGlueFileName, "managed-glue", ManagedGlueGenerator.GenerateRuntimeBridgePartial(manifest)),
            new ArtifactContent(TextManifestFileName, "text-manifest", BindingManifestGenerator.GenerateText(manifest)),
            new ArtifactContent(JsonManifestFileName, "json-manifest", BindingManifestGenerator.GenerateJson(manifest))
        };
        return CreateSummary(manifest, artifacts);
    }

    private static string CreateSummary(BindingManifestDocument manifest, IReadOnlyList<ArtifactContent> artifacts)
    {
        var document = new ArtifactSummary(
            FormatVersion: BindingManifestVersions.PackFormatVersion,
            GeneratedBy: "Rustlyn.Bindings",
            GeneratorVersion: typeof(BindingArtifactPackGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? typeof(BindingArtifactPackGenerator).Assembly.GetName().Version?.ToString()
                ?? "unknown",
            Compatibility: manifest.Compatibility,
            ManagedAssemblies: manifest.ManagedAssemblies,
            OwnershipRules:
            [
                new ArtifactOwnershipRule(
                    "object-handle",
                    "Owned object handles returned by generated bindings must be released with rustlyn_bindgen_system_object_release; borrowed object handles remain owned by the caller."),
                new ArtifactOwnershipRule(
                    "exception-handle",
                    "Non-zero exception handles returned or written by generated bindings must be released with rustlyn_bindgen_system_exception_release after inspection."),
                new ArtifactOwnershipRule(
                    "string-array-element",
                    "ManagedStringArray::get returns a newly-owned ManagedString handle; release element handles and the array handle separately."),
                new ArtifactOwnershipRule(
                    "object-handle-callback",
                    "Object-handle callbacks receive borrowed input handles and must return newly-owned result handles; the managed callback bridge consumes that callback-owned result handle.")
            ],
            Artifacts: artifacts
                .OrderBy(static artifact => artifact.Path, StringComparer.Ordinal)
                .Select(static artifact => new ArtifactHash(
                    artifact.Path,
                    artifact.Kind,
                    ByteCount: Utf8NoBom.GetByteCount(artifact.Content),
                    Sha256: HashContent(artifact.Content)))
                .ToArray());

        return JsonSerializer.Serialize(document, JsonOptions) + Environment.NewLine;
    }

    private static string HashContent(string content)
        => Convert.ToHexString(SHA256.HashData(Utf8NoBom.GetBytes(content))).ToLowerInvariant();

    private sealed record ArtifactContent(string Path, string Kind, string Content);

    private sealed record ArtifactSummary(
        int FormatVersion,
        string GeneratedBy,
        string GeneratorVersion,
        BindingManifestCompatibility Compatibility,
        IReadOnlyList<BindingManifestAssemblyIdentity> ManagedAssemblies,
        IReadOnlyList<ArtifactOwnershipRule> OwnershipRules,
        IReadOnlyList<ArtifactHash> Artifacts);

    private sealed record ArtifactOwnershipRule(string Name, string Rule);

    private sealed record ArtifactHash(string Path, string Kind, int ByteCount, string Sha256);
}
