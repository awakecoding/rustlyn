using System.Diagnostics;
using System.Text.Json;

namespace Rustlyn.Backend;

internal static class RustlynLlvmLowerJsonReader
{
    public static RustlynLlvmLowerJsonModule ReadModule(string artifactPath, string helperPath)
    {
        var json = RunLowerJson(artifactPath, helperPath);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        var schemaVersion = root.GetProperty("schemaVersion").GetInt32();
        if (schemaVersion != 1)
        {
            throw new InvalidDataException($"Unsupported rustlyn-llvm lower-json schema version '{schemaVersion}'.");
        }

        var module = root.GetProperty("module");
        return new RustlynLlvmLowerJsonModule(
            module.GetProperty("sourcePath").GetString() ?? string.Empty,
            ReadGlobals(module.GetProperty("globals")),
            ReadFunctions(module.GetProperty("functions")));
    }

    private static string RunLowerJson(string artifactPath, string helperPath)
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
        startInfo.ArgumentList.Add("lower-json");
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
                ? $"{Path.GetFileName(helperPath)} lower-json failed with exit code {process.ExitCode}."
                : standardError.Trim();
            throw new InvalidDataException(message);
        }

        return standardOutput;
    }

    private static IReadOnlyList<RustlynLlvmLowerJsonFunction> ReadFunctions(JsonElement functions)
    {
        var result = new List<RustlynLlvmLowerJsonFunction>();
        foreach (var function in functions.EnumerateArray())
        {
            result.Add(new RustlynLlvmLowerJsonFunction(
                function.GetProperty("name").GetString() ?? string.Empty,
                function.GetProperty("returnType").GetString() ?? string.Empty,
                ReadParameters(function.GetProperty("parameters")),
                ReadBlocks(function.GetProperty("blocks"))));
        }

        return result;
    }

    private static IReadOnlyList<RustlynLlvmLowerJsonGlobal> ReadGlobals(JsonElement globals)
    {
        var result = new List<RustlynLlvmLowerJsonGlobal>();
        foreach (var global in globals.EnumerateArray())
        {
            result.Add(new RustlynLlvmLowerJsonGlobal(global.GetProperty("name").GetString() ?? string.Empty));
        }

        return result;
    }

    private static IReadOnlyList<RustlynLlvmLowerJsonParameter> ReadParameters(JsonElement parameters)
    {
        var result = new List<RustlynLlvmLowerJsonParameter>();
        foreach (var parameter in parameters.EnumerateArray())
        {
            result.Add(new RustlynLlvmLowerJsonParameter(
                parameter.GetProperty("name").GetString() ?? string.Empty,
                parameter.GetProperty("type").GetString() ?? string.Empty));
        }

        return result;
    }

    private static IReadOnlyList<RustlynLlvmLowerJsonBlock> ReadBlocks(JsonElement blocks)
    {
        var result = new List<RustlynLlvmLowerJsonBlock>();
        foreach (var block in blocks.EnumerateArray())
        {
            result.Add(new RustlynLlvmLowerJsonBlock(
                block.GetProperty("name").GetString() ?? string.Empty,
                ReadInstructions(block.GetProperty("instructions"))));
        }

        return result;
    }

    private static IReadOnlyList<RustlynLlvmLowerJsonInstruction> ReadInstructions(JsonElement instructions)
    {
        var result = new List<RustlynLlvmLowerJsonInstruction>();
        foreach (var instruction in instructions.EnumerateArray())
        {
            result.Add(new RustlynLlvmLowerJsonInstruction(
                instruction.GetProperty("opcode").GetString() ?? string.Empty,
                instruction.GetProperty("text").GetString() ?? string.Empty,
                instruction.GetProperty("result").GetString() ?? string.Empty,
                ReadOperands(instruction.GetProperty("operands"))));
        }

        return result;
    }

    private static IReadOnlyList<RustlynLlvmLowerJsonOperand> ReadOperands(JsonElement operands)
    {
        var result = new List<RustlynLlvmLowerJsonOperand>();
        foreach (var operand in operands.EnumerateArray())
        {
            result.Add(new RustlynLlvmLowerJsonOperand(
                operand.GetProperty("text").GetString() ?? string.Empty,
                operand.GetProperty("type").GetString() ?? string.Empty));
        }

        return result;
    }
}

internal sealed record RustlynLlvmLowerJsonModule(
    string SourcePath,
    IReadOnlyList<RustlynLlvmLowerJsonGlobal> Globals,
    IReadOnlyList<RustlynLlvmLowerJsonFunction> Functions);

internal sealed record RustlynLlvmLowerJsonGlobal(
    string Name);

internal sealed record RustlynLlvmLowerJsonFunction(
    string Name,
    string ReturnType,
    IReadOnlyList<RustlynLlvmLowerJsonParameter> Parameters,
    IReadOnlyList<RustlynLlvmLowerJsonBlock> Blocks);

internal sealed record RustlynLlvmLowerJsonParameter(
    string Name,
    string Type);

internal sealed record RustlynLlvmLowerJsonBlock(
    string Name,
    IReadOnlyList<RustlynLlvmLowerJsonInstruction> Instructions);

internal sealed record RustlynLlvmLowerJsonInstruction(
    string Opcode,
    string Text,
    string Result,
    IReadOnlyList<RustlynLlvmLowerJsonOperand> Operands);

internal sealed record RustlynLlvmLowerJsonOperand(
    string Text,
    string Type);
