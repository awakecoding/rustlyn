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
            if (readerMode == LlvmReaderMode.Auto && structuredModule.Globals.Count > 0)
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
