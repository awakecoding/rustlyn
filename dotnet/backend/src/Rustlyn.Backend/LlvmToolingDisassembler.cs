using System.Diagnostics;

namespace Rustlyn.Backend;

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
        var irTool = LlvmNativeLibraryLocator.GetIrToolPath(toolchainRoot);
        var processStartInfo = new ProcessStartInfo
        {
            FileName = irTool.Path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Directory.GetCurrentDirectory()
        };

        if (irTool.Kind == LlvmIrToolKind.RustlynLlvm)
        {
            processStartInfo.ArgumentList.Add("print-ir");
            processStartInfo.ArgumentList.Add(artifactPath);
            processStartInfo.ArgumentList.Add("--disable-verify");
            processStartInfo.ArgumentList.Add("--output");
            processStartInfo.ArgumentList.Add("-");
        }
        else
        {
            processStartInfo.ArgumentList.Add("-disable-verify");
            processStartInfo.ArgumentList.Add("-S");
            processStartInfo.ArgumentList.Add(artifactPath);
            processStartInfo.ArgumentList.Add("-o");
            processStartInfo.ArgumentList.Add("-");
        }

        using var process = Process.Start(processStartInfo)
            ?? throw new InvalidOperationException($"Failed to start LLVM tool '{irTool.Path}'.");

        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var message = string.IsNullOrWhiteSpace(standardError)
                ? $"{Path.GetFileName(irTool.Path)} failed with exit code {process.ExitCode}."
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
