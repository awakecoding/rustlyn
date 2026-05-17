using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RustMcil.Backend;

internal static partial class LlvmToolingModuleReader
{
    public static LlvmModuleSummary ReadSummary(string artifactPath, string toolchainRoot)
    {
        var llvmIr = LlvmToolingDisassembler.ReadLlvmIr(artifactPath, toolchainRoot);
        return ParseSummary(toolchainRoot, llvmIr);
    }

    private static LlvmModuleSummary ParseSummary(string toolchainRoot, string llvmIr)
    {
        var functions = new List<LlvmFunctionSummary>();
        var aliases = new List<LlvmAliasSummary>();
        var globals = new List<LlvmGlobalSummary>();
        FunctionAccumulator? currentFunction = null;

        foreach (var rawLine in ReadLines(llvmIr))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';'))
            {
                continue;
            }

            if (currentFunction is null)
            {
                var functionMatch = FunctionHeaderRegex().Match(line);
                if (functionMatch.Success)
                {
                    currentFunction = new FunctionAccumulator(functionMatch.Groups["name"].Value);
                    continue;
                }

                var aliasMatch = AliasRegex().Match(line);
                if (aliasMatch.Success)
                {
                    aliases.Add(new LlvmAliasSummary(
                        aliasMatch.Groups["name"].Value,
                        aliasMatch.Groups["target"].Value,
                        aliasMatch.Groups["signature"].Value.Trim()));
                    continue;
                }

                var globalMatch = GlobalRegex().Match(line);
                if (globalMatch.Success)
                {
                    globals.Add(new LlvmGlobalSummary(globalMatch.Groups["name"].Value));
                }

                continue;
            }

            if (line == "}")
            {
                functions.Add(currentFunction.ToSummary());
                currentFunction = null;
                continue;
            }

            if (BasicBlockRegex().IsMatch(line))
            {
                currentFunction.BasicBlockCount++;
                continue;
            }

            if (IsInstructionLine(line))
            {
                currentFunction.InstructionCount++;
            }
        }

        if (currentFunction is not null)
        {
            functions.Add(currentFunction.ToSummary());
        }

        return new LlvmModuleSummary(toolchainRoot, "llvm-opt-text", functions, aliases, globals);
    }

    private static IEnumerable<string> ReadLines(string text)
    {
        using var reader = new StringReader(text);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }

    private static bool IsInstructionLine(string line)
    {
        if (line.StartsWith("attributes ", StringComparison.Ordinal) || line.StartsWith("declare ", StringComparison.Ordinal))
        {
            return false;
        }

        if (line.Contains(" = ", StringComparison.Ordinal))
        {
            return true;
        }

        return line.StartsWith("ret ", StringComparison.Ordinal)
            || line.StartsWith("br ", StringComparison.Ordinal)
            || line.StartsWith("switch ", StringComparison.Ordinal)
            || line.StartsWith("invoke ", StringComparison.Ordinal)
            || line.StartsWith("call ", StringComparison.Ordinal)
            || line.StartsWith("unreachable", StringComparison.Ordinal)
            || line.StartsWith("resume ", StringComparison.Ordinal);
    }

    [GeneratedRegex("^define\\b.*@(?<name>[^\\s(]+)\\(", RegexOptions.CultureInvariant)]
    private static partial Regex FunctionHeaderRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:[^=]+\\s+)?alias\\s+(?<signature>[^,]+(?:\\([^)]*\\))?),\\s+ptr\\s+@(?<target>[^\\s,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex AliasRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=", RegexOptions.CultureInvariant)]
    private static partial Regex GlobalRegex();

    [GeneratedRegex("^[A-Za-z$._][-A-Za-z$._0-9]*:", RegexOptions.CultureInvariant)]
    private static partial Regex BasicBlockRegex();

    private sealed class FunctionAccumulator(string name)
    {
        public string Name { get; } = name;

        public int BasicBlockCount { get; set; }

        public int InstructionCount { get; set; }

        public LlvmFunctionSummary ToSummary()
        {
            return new LlvmFunctionSummary(Name, BasicBlockCount, InstructionCount);
        }
    }
}