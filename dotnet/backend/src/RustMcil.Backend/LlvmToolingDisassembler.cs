using System.Diagnostics;

namespace RustMcil.Backend;

internal static class LlvmToolingDisassembler
{
    public static string ReadLlvmIr(string artifactPath, string toolchainRoot)
    {
        var siblingLlvmIrPath = Path.ChangeExtension(artifactPath, ".ll");
        try
        {
            return ReadOptimizedLlvmIr(artifactPath, toolchainRoot);
        }
        catch (Exception exception) when (CanFallBackToSiblingLlvmIr(exception) && File.Exists(siblingLlvmIrPath))
        {
            return File.ReadAllText(siblingLlvmIrPath);
        }
    }

    private static string ReadOptimizedLlvmIr(string artifactPath, string toolchainRoot)
    {
        var llvmOptPath = LlvmNativeLibraryLocator.GetToolPath(toolchainRoot, "llvm-opt.exe");
        var processStartInfo = new ProcessStartInfo
        {
            FileName = llvmOptPath,
            Arguments = $"-disable-verify -S \"{artifactPath}\" -o -",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Directory.GetCurrentDirectory()
        };

        using var process = Process.Start(processStartInfo)
            ?? throw new InvalidOperationException($"Failed to start LLVM tool '{llvmOptPath}'.");

        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var message = string.IsNullOrWhiteSpace(standardError)
                ? $"llvm-opt.exe failed with exit code {process.ExitCode}."
                : standardError.Trim();
            throw new InvalidDataException(message);
        }

        return standardOutput;
    }

    private static bool CanFallBackToSiblingLlvmIr(Exception exception)
    {
        return exception is InvalidDataException
            or FileNotFoundException
            or DirectoryNotFoundException
            or InvalidOperationException;
    }
}
