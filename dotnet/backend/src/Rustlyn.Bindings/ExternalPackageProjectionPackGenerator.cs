using System.Security.Cryptography;
using System.Text;

namespace Rustlyn.Bindings;

public static class ExternalPackageProjectionPackGenerator
{
    public static void WriteAvaloniaHelloPack(string outputDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        Directory.CreateDirectory(outputDirectory);

        var manifest = ExternalPackageBindingSurfaces.CreateAvaloniaHelloManifest();
        var rustModule = AvaloniaRustBindingGenerator.GenerateHelloModule(manifest);
        var managedGlue = ManagedGlueGenerator.GenerateRuntimeBridgePartial(manifest);
        var manifestJson = BindingManifestGenerator.GenerateJson(manifest);

        WriteFile(Path.Combine(outputDirectory, "avalonia.rs"), rustModule);
        WriteFile(Path.Combine(outputDirectory, "RuntimeBridgeHelpers.AvaloniaBindings.g.cs"), managedGlue);
        WriteFile(Path.Combine(outputDirectory, "binding-manifest.json"), manifestJson);
        WriteFile(Path.Combine(outputDirectory, "summary.txt"), CreateSummary(manifest, rustModule, managedGlue, manifestJson));
    }

    public static void WritePowerShellCmdletPack(string outputDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        Directory.CreateDirectory(outputDirectory);

        var manifest = ExternalPackageBindingSurfaces.CreatePowerShellCmdletManifest();
        var rustModule = PowerShellRustBindingGenerator.GenerateCmdletModule(manifest);
        var managedGlue = ManagedGlueGenerator.GenerateRuntimeBridgePartial(manifest);
        var cmdletDescriptors = PowerShellCmdletDescriptorCatalog.CreateCurrentFormatCmdlets();
        var generatedCmdletCount = cmdletDescriptors.Count(static descriptor => descriptor.MigrationStrategy == PowerShellCmdletMigrationStrategies.GeneratedRust);
        var cmdletDescriptorJson = PowerShellCmdletDescriptorCatalog.CreateCurrentFormatCmdletJson();
        var cmdletShim = PowerShellCmdletShimGenerator.GenerateCSharp(cmdletDescriptors);
        var manifestJson = BindingManifestGenerator.GenerateJson(manifest);

        WriteFile(Path.Combine(outputDirectory, "powershell_cmdlet.rs"), rustModule);
        WriteFile(Path.Combine(outputDirectory, "RuntimeBridgeHelpers.PowerShellCmdletBindings.g.cs"), managedGlue);
        WriteFile(Path.Combine(outputDirectory, "powershell-cmdlets.json"), cmdletDescriptorJson);
        WriteFile(Path.Combine(outputDirectory, "Rustlyn.PowerShellCmdlets.Generated.g.cs"), cmdletShim);
        WriteFile(Path.Combine(outputDirectory, "binding-manifest.json"), manifestJson);
        WriteFile(Path.Combine(outputDirectory, "summary.txt"), CreateSummary(manifest, rustModule, managedGlue, manifestJson, generatedCmdletCount));
    }

    private static void WriteFile(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException("Output directory could not be determined."));
        File.WriteAllText(path, content);
    }

    private static string CreateSummary(BindingManifestDocument manifest, string rustModule, string managedGlue, string manifestJson)
        => CreateSummary(manifest, rustModule, managedGlue, manifestJson, cmdletCount: null);

    private static string CreateSummary(BindingManifestDocument manifest, string rustModule, string managedGlue, string manifestJson, int? cmdletCount)
    {
        var package = manifest.PackageSurface
            ?? throw new InvalidOperationException("External package pack manifests must include package metadata.");
        var builder = new StringBuilder();
        builder.AppendLine($"package: {package.PackageId}");
        builder.AppendLine($"version: {package.PackageVersion}");
        builder.AppendLine($"target-framework: {package.TargetFramework}");
        builder.AppendLine($"bindings: {manifest.Bindings.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        builder.AppendLine($"rust-module-sha256: {Sha256(rustModule)}");
        builder.AppendLine($"managed-glue-sha256: {Sha256(managedGlue)}");
        builder.AppendLine($"manifest-sha256: {Sha256(manifestJson)}");
        if (cmdletCount is not null)
        {
            builder.AppendLine($"cmdlets: {cmdletCount.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        }

        foreach (var assembly in package.Assemblies.OrderBy(static assembly => assembly.Name, StringComparer.Ordinal))
        {
            builder.AppendLine($"assembly: {assembly.Name} role={assembly.Role}");
        }

        return builder.ToString();
    }

    private static string Sha256(string content)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content))).ToLowerInvariant();
}
