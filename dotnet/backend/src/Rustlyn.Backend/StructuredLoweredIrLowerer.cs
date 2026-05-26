namespace Rustlyn.Backend;

public static partial class LoweredIrLowerer
{
    private const string LlvmReaderEnvironmentVariable = "RUSTLYN_LLVM_READER";

    private static LoweredModule? TryLowerBitcodeWithStructuredJson(
        string artifactPath,
        string toolchainRoot,
        LlvmReaderMode readerMode)
    {
        var irTool = LlvmNativeLibraryLocator.TryGetIrToolPath(toolchainRoot);
        if (irTool?.Kind != LlvmIrToolKind.RustlynLlvm)
        {
            if (readerMode == LlvmReaderMode.Json)
            {
                throw new InvalidOperationException("RUSTLYN_LLVM_READER=json requires rustlyn-llvm under the configured LLVM root.");
            }

            return null;
        }

        try
        {
            var structuredModule = RustlynLlvmLowerJsonReader.ReadModule(artifactPath, irTool.Path);
            if (readerMode == LlvmReaderMode.Auto && !IsSafeForStructuredLowering(structuredModule))
            {
                return null;
            }

            return LowerStructuredJson(structuredModule);
        }
        catch (Exception exception) when (readerMode == LlvmReaderMode.Auto && CanFallBackFromStructuredJson(exception))
        {
            return null;
        }
    }

    // Opcodes that the structured lowerer can safely handle by re-using the per-instruction text parser.
    // Anything that depends on multi-line context (alloca/store/load chains, switch tables, phi merges,
    // gep field offsets, lifetime intrinsics, indirect calls, atomics, vector ops) is excluded; in auto
    // mode the lowerer falls back to the text reader for those modules. Widen this set only after
    // verifying the structured path produces identical results to the text path for the new opcode.
    private static readonly HashSet<string> SafeStructuredOpcodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "add", "sub", "mul",
        "sdiv", "udiv", "srem", "urem",
        "and", "or", "xor",
        "shl", "lshr", "ashr",
        "icmp",
        "zext", "sext", "trunc",
        "ret",
    };

    private static bool IsSafeForStructuredLowering(RustlynLlvmLowerJsonModule module)
    {
        if (module.Globals.Count > 0 || module.Aliases.Count > 0)
        {
            return false;
        }

        foreach (var function in module.Functions)
        {
            foreach (var block in function.Blocks)
            {
                foreach (var instruction in block.Instructions)
                {
                    if (!SafeStructuredOpcodes.Contains(instruction.Opcode))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private static LoweredModule LowerStructuredJson(RustlynLlvmLowerJsonModule module)
    {
        var functions = module.Functions.Select(LowerStructuredFunction).ToArray();
        return new LoweredModule(functions, []);
    }

    private static LoweredFunction LowerStructuredFunction(RustlynLlvmLowerJsonFunction function)
    {
        var parameters = function.Parameters
            .Select(parameter => new LoweredParameter(
                NormalizeValue(parameter.Name),
                NormalizeType(parameter.Type)))
            .ToArray();

        var blocks = function.Blocks
            .Select(block => new LoweredBlock(
                NormalizeBlockName(block.Name),
                block.Instructions
                    .Select(instruction => ParseInstruction(instruction.Text.Trim()))
                    .ToArray()))
            .ToArray();

        return new LoweredFunction(
            NormalizeFunctionName(function.Name),
            NormalizeType(function.ReturnType),
            parameters,
            blocks);
    }

    private static LlvmReaderMode GetLlvmReaderMode()
    {
        var value = Environment.GetEnvironmentVariable(LlvmReaderEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(value)
            || string.Equals(value, "auto", StringComparison.OrdinalIgnoreCase))
        {
            return LlvmReaderMode.Auto;
        }

        if (string.Equals(value, "text", StringComparison.OrdinalIgnoreCase))
        {
            return LlvmReaderMode.Text;
        }

        if (string.Equals(value, "json", StringComparison.OrdinalIgnoreCase))
        {
            return LlvmReaderMode.Json;
        }

        throw new InvalidOperationException(
            $"{LlvmReaderEnvironmentVariable} must be one of 'auto', 'text', or 'json'.");
    }

    private static bool CanFallBackFromStructuredJson(Exception exception)
    {
        return exception is InvalidDataException
            or InvalidOperationException
            or FileNotFoundException
            or DirectoryNotFoundException
            or System.Text.Json.JsonException;
    }

    private enum LlvmReaderMode
    {
        Auto,
        Text,
        Json
    }
}
