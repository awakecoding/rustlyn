using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rustlyn.Bindings;

public static class RuntimeProjectionPackGenerator
{
    public const string ManifestFileName = "manifest.json";
    public const string CoverageTextFileName = "coverage.txt";
    public const string CoverageJsonFileName = "coverage.json";
    public const string UnsupportedFileName = "unsupported.txt";
    public const string RustSystemModuleFileName = "system.rs";
    public const string ManagedGlueFileName = "RuntimeBridgeHelpers.g.cs";
    public const string DiffTextFileName = "diff.txt";
    public const string DiffJsonFileName = "diff.json";
    public const string SummaryFileName = "runtime-pack-summary.json";

    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static void WritePacks(
        RuntimeSurfaceScanSet scanSet,
        string outputDirectory)
    {
        ArgumentNullException.ThrowIfNull(scanSet);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var outputFullPath = Path.GetFullPath(outputDirectory);
        Directory.CreateDirectory(outputFullPath);

        var artifacts = new List<RuntimeProjectionArtifact>();
        foreach (var report in scanSet.Reports)
        {
            var tfmDirectory = GetTargetFrameworkDirectory(outputFullPath, report.TargetFramework);
            Directory.CreateDirectory(tfmDirectory);
            var manifestDocument = RuntimeCallableBindingCompiler.AddCallableBindings(BindingManifestFactory.FromRuntimeSurface(report));

            WriteArtifact(
                artifacts,
                outputFullPath,
                Path.Combine(report.TargetFramework, ManifestFileName),
                "runtime-manifest",
                BindingManifestGenerator.GenerateJson(manifestDocument),
                report.TargetFramework);
            WriteArtifact(
                artifacts,
                outputFullPath,
                Path.Combine(report.TargetFramework, RustSystemModuleFileName),
                "runtime-rust-system-module",
                RustBindingGenerator.GenerateSystemModule(manifestDocument),
                report.TargetFramework);
            WriteArtifact(
                artifacts,
                outputFullPath,
                Path.Combine(report.TargetFramework, ManagedGlueFileName),
                "runtime-managed-glue",
                ManagedGlueGenerator.GenerateRuntimeBridgePartial(manifestDocument),
                report.TargetFramework);
            WriteArtifact(
                artifacts,
                outputFullPath,
                Path.Combine(report.TargetFramework, CoverageTextFileName),
                "coverage-text",
                RuntimeSurfaceReportFormatter.GenerateText(new RuntimeSurfaceScanSet([report], [])),
                report.TargetFramework);
            WriteArtifact(
                artifacts,
                outputFullPath,
                Path.Combine(report.TargetFramework, CoverageJsonFileName),
                "coverage-json",
                RuntimeSurfaceReportFormatter.GenerateJson(new RuntimeSurfaceScanSet([report], [])),
                report.TargetFramework);
            WriteArtifact(
                artifacts,
                outputFullPath,
                Path.Combine(report.TargetFramework, UnsupportedFileName),
                "unsupported-diagnostics",
                GenerateUnsupportedText(report),
                report.TargetFramework);
        }

        if (scanSet.Reports.Count > 1)
        {
            WriteArtifact(
                artifacts,
                outputFullPath,
                DiffTextFileName,
                "tfm-diff-text",
                RuntimeSurfaceReportFormatter.GenerateDiffText(scanSet),
                TargetFramework: null);
            WriteArtifact(
                artifacts,
                outputFullPath,
                DiffJsonFileName,
                "tfm-diff-json",
                RuntimeSurfaceReportFormatter.GenerateDiffJson(scanSet),
                TargetFramework: null);
        }

        var summary = CreateSummary(scanSet, artifacts);
        File.WriteAllText(Path.Combine(outputFullPath, SummaryFileName), summary, Utf8NoBom);
    }

    public static string GenerateSummary(RuntimeSurfaceScanSet scanSet, string outputDirectory)
    {
        ArgumentNullException.ThrowIfNull(scanSet);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        var outputFullPath = Path.GetFullPath(outputDirectory);
        var artifacts = scanSet.Reports
            .SelectMany(static report => new[]
            {
                new RuntimeProjectionArtifact(report.TargetFramework, Path.Combine(report.TargetFramework, ManifestFileName), "runtime-manifest", 0, string.Empty),
                new RuntimeProjectionArtifact(report.TargetFramework, Path.Combine(report.TargetFramework, RustSystemModuleFileName), "runtime-rust-system-module", 0, string.Empty),
                new RuntimeProjectionArtifact(report.TargetFramework, Path.Combine(report.TargetFramework, ManagedGlueFileName), "runtime-managed-glue", 0, string.Empty),
                new RuntimeProjectionArtifact(report.TargetFramework, Path.Combine(report.TargetFramework, CoverageTextFileName), "coverage-text", 0, string.Empty),
                new RuntimeProjectionArtifact(report.TargetFramework, Path.Combine(report.TargetFramework, CoverageJsonFileName), "coverage-json", 0, string.Empty),
                new RuntimeProjectionArtifact(report.TargetFramework, Path.Combine(report.TargetFramework, UnsupportedFileName), "unsupported-diagnostics", 0, string.Empty)
            })
            .Concat(scanSet.Reports.Count > 1
                ? [
                    new RuntimeProjectionArtifact(null, DiffTextFileName, "tfm-diff-text", 0, string.Empty),
                    new RuntimeProjectionArtifact(null, DiffJsonFileName, "tfm-diff-json", 0, string.Empty)
                ]
                : [])
            .Select(artifact =>
            {
                var artifactPath = Path.Combine(outputFullPath, artifact.Path);
                if (!File.Exists(artifactPath))
                {
                    return artifact;
                }

                var bytes = File.ReadAllBytes(artifactPath);
                return artifact with
                {
                    ByteCount = bytes.Length,
                    Sha256 = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant()
                };
            })
            .ToArray();
        return CreateSummary(scanSet, artifacts);
    }

    private static void WriteArtifact(
        ICollection<RuntimeProjectionArtifact> artifacts,
        string root,
        string relativePath,
        string kind,
        string content,
        string? TargetFramework)
    {
        var fullPath = Path.Combine(root, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException("Artifact directory could not be determined."));
        File.WriteAllText(fullPath, content, Utf8NoBom);
        artifacts.Add(new RuntimeProjectionArtifact(
            TargetFramework,
            relativePath,
            kind,
            Utf8NoBom.GetByteCount(content),
            Convert.ToHexString(SHA256.HashData(Utf8NoBom.GetBytes(content))).ToLowerInvariant()));
    }

    private static string CreateSummary(
        RuntimeSurfaceScanSet scanSet,
        IReadOnlyList<RuntimeProjectionArtifact> artifacts)
    {
        var summary = new RuntimeProjectionPackSummary(
            BindingManifestVersions.PackFormatVersion,
            "Rustlyn.Bindings",
            typeof(RuntimeProjectionPackGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? typeof(RuntimeProjectionPackGenerator).Assembly.GetName().Version?.ToString()
                ?? "unknown",
            BindingManifestCompatibility.Current,
            scanSet.Reports
                .Select(static report =>
                {
                    var manifestDocument = RuntimeCallableBindingCompiler.AddCallableBindings(BindingManifestFactory.FromRuntimeSurface(report));
                    var callableCoverage = RuntimeCallableCoverageGate.Measure(manifestDocument);
                    return new RuntimeProjectionPackTarget(
                        report.TargetFramework,
                        report.PackVersion,
                        report.RefDirectory,
                        report.AssemblyCount,
                        report.ExportedTypeCount,
                        report.ProjectedMemberCount,
                        report.UnsupportedShapeCount,
                        callableCoverage.CallableBindingCount,
                        callableCoverage.Namespaces);
                })
                .ToArray(),
            scanSet.MissingTargets,
            artifacts
                .OrderBy(static artifact => artifact.Path, StringComparer.Ordinal)
                .ToArray());

        return JsonSerializer.Serialize(summary, JsonOptions) + Environment.NewLine;
    }

    private static string GenerateUnsupportedText(RuntimeSurfaceScanReport report)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Target framework: {report.TargetFramework}");
        builder.AppendLine($"Unsupported shapes: {report.UnsupportedShapeCount.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        foreach (var reason in report.UnsupportedShapesByReason)
        {
            builder.AppendLine($"reason: {reason.Reason} count: {reason.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        }

        foreach (var shape in report.UnsupportedShapes)
        {
            builder.AppendLine($"{shape.DisplayName}: {shape.Reason}");
        }

        return builder.ToString();
    }

    private static string GetTargetFrameworkDirectory(string root, string targetFramework)
    {
        if (targetFramework.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new InvalidOperationException($"Target framework '{targetFramework}' is not safe for a projection pack directory name.");
        }

        return Path.Combine(root, targetFramework);
    }

    private sealed record RuntimeProjectionPackSummary(
        int FormatVersion,
        string GeneratedBy,
        string GeneratorVersion,
        BindingManifestCompatibility Compatibility,
        IReadOnlyList<RuntimeProjectionPackTarget> Targets,
        IReadOnlyList<RuntimeReferencePackMissingTarget> MissingTargets,
        IReadOnlyList<RuntimeProjectionArtifact> Artifacts);

    private sealed record RuntimeProjectionPackTarget(
        string TargetFramework,
        string PackVersion,
        string RefDirectory,
        int AssemblyCount,
        int ExportedTypeCount,
        int ProjectedMemberCount,
        int UnsupportedShapeCount,
        int CallableBindingCount,
        IReadOnlyList<RuntimeCallableNamespaceCoverage> CallableNamespaces);

    private sealed record RuntimeProjectionArtifact(
        string? TargetFramework,
        string Path,
        string Kind,
        int ByteCount,
        string Sha256);
}
