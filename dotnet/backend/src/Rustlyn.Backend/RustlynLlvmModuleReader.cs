using System.Diagnostics;
using System.Text.Json;

namespace Rustlyn.Backend;

internal static class RustlynLlvmModuleReader
{
    public static LlvmModuleSummary ReadSummary(string artifactPath, string toolchainRoot, string helperPath)
    {
        var json = RunInspectJson(artifactPath, helperPath);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        var schemaVersion = root.GetProperty("schemaVersion").GetInt32();
        if (schemaVersion != 1)
        {
            throw new InvalidDataException($"Unsupported rustlyn-llvm inspect-json schema version '{schemaVersion}'.");
        }

        var readerKind = root.GetProperty("readerKind").GetString() ?? "rustlyn-llvm-json";
        var module = root.GetProperty("module");

        return new LlvmModuleSummary(
            toolchainRoot,
            readerKind,
            ReadFunctions(module.GetProperty("functions")),
            ReadAliases(module.GetProperty("aliases")),
            ReadGlobals(module.GetProperty("globals")));
    }

    private static string RunInspectJson(string artifactPath, string helperPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = helperPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Directory.GetCurrentDirectory()
        };
        startInfo.ArgumentList.Add("inspect-json");
        startInfo.ArgumentList.Add(artifactPath);
        startInfo.ArgumentList.Add("--disable-verify");

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Failed to start LLVM helper '{helperPath}'.");

        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var message = string.IsNullOrWhiteSpace(standardError)
                ? $"{Path.GetFileName(helperPath)} inspect-json failed with exit code {process.ExitCode}."
                : standardError.Trim();
            throw new InvalidDataException(message);
        }

        return standardOutput;
    }

    private static IReadOnlyList<LlvmFunctionSummary> ReadFunctions(JsonElement functions)
    {
        var summaries = new List<LlvmFunctionSummary>();
        foreach (var function in functions.EnumerateArray())
        {
            summaries.Add(new LlvmFunctionSummary(
                function.GetProperty("name").GetString() ?? string.Empty,
                function.GetProperty("basicBlockCount").GetInt32(),
                function.GetProperty("instructionCount").GetInt32()));
        }

        return summaries;
    }

    private static IReadOnlyList<LlvmAliasSummary> ReadAliases(JsonElement aliases)
    {
        var summaries = new List<LlvmAliasSummary>();
        foreach (var alias in aliases.EnumerateArray())
        {
            summaries.Add(new LlvmAliasSummary(
                alias.GetProperty("name").GetString() ?? string.Empty,
                alias.GetProperty("target").GetString() ?? string.Empty,
                alias.GetProperty("signature").GetString() ?? string.Empty));
        }

        return summaries;
    }

    private static IReadOnlyList<LlvmGlobalSummary> ReadGlobals(JsonElement globals)
    {
        var summaries = new List<LlvmGlobalSummary>();
        foreach (var global in globals.EnumerateArray())
        {
            summaries.Add(new LlvmGlobalSummary(global.GetProperty("name").GetString() ?? string.Empty));
        }

        return summaries;
    }
}
