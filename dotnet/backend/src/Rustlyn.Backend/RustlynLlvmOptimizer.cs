using System.Diagnostics;

namespace Rustlyn.Backend;

internal static class RustlynLlvmOptimizer
{
    private const string OptPassesEnvVar = "RUSTLYN_LLVM_OPT_PASSES";

    public static string? GetConfiguredPasses()
    {
        var raw = Environment.GetEnvironmentVariable(OptPassesEnvVar);
        return string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();
    }

    /// <summary>
    /// If RUSTLYN_LLVM_OPT_PASSES is set and the rustlyn-llvm helper is available, run the configured
    /// pipeline on <paramref name="bitcodePath"/> and return a path to the optimized bitcode. Otherwise
    /// returns the original path. The optimized file is written under the system temp directory and the
    /// caller is responsible for cleanup (or it can be left for the OS to reclaim).
    /// </summary>
    public static string MaybeOptimize(string bitcodePath, string toolchainRoot)
    {
        var passes = GetConfiguredPasses();
        if (passes is null)
        {
            return bitcodePath;
        }

        var tool = LlvmNativeLibraryLocator.TryGetIrToolPath(toolchainRoot);
        if (tool is null || tool.Kind != LlvmIrToolKind.RustlynLlvm)
        {
            // Helper not available - optimization pre-pass is a no-op rather than a hard failure.
            return bitcodePath;
        }

        var outputDir = Path.Combine(Path.GetTempPath(), "rustlyn-llvm-opt");
        Directory.CreateDirectory(outputDir);
        var outputName = Path.GetFileNameWithoutExtension(bitcodePath) + "." + Guid.NewGuid().ToString("N").Substring(0, 8) + ".opt.bc";
        var outputPath = Path.Combine(outputDir, outputName);

        var psi = new ProcessStartInfo(tool.Path)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        psi.ArgumentList.Add("opt");
        psi.ArgumentList.Add(bitcodePath);
        psi.ArgumentList.Add("--passes");
        psi.ArgumentList.Add(passes);
        psi.ArgumentList.Add("--output");
        psi.ArgumentList.Add(outputPath);

        using var process = Process.Start(psi)
            ?? throw new InvalidOperationException($"Failed to start rustlyn-llvm at '{tool.Path}'.");
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0 || !File.Exists(outputPath))
        {
            throw new InvalidDataException(
                $"rustlyn-llvm opt failed (exit {process.ExitCode}) for passes '{passes}':{Environment.NewLine}{stderr}");
        }

        return outputPath;
    }
}
