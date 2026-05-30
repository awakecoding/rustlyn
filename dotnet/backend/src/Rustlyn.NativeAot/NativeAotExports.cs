using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rustlyn.Backend;

namespace Rustlyn.NativeAot;

public static unsafe class NativeAotExports
{
    private const int RustlynOk = 0;
    private const int RustlynOperationFailed = 1;
    private const int RustlynInvalidAbi = -1;
    private const int RustlynResultAllocationFailed = -2;

    private static readonly byte[] ResultSerializationFailedJson =
        "{\"success\":false,\"diagnostics\":[{\"severity\":\"error\",\"message\":\"result serialization failed\"}],\"outputFiles\":[]}"u8.ToArray();

    [UnmanagedCallersOnly(EntryPoint = "rustlyn_emit", CallConvs = [typeof(CallConvCdecl)])]
    public static int Emit(byte* optionsJson, nuint optionsLen, byte** resultJson, nuint* resultLen)
    {
        if (resultJson is null || resultLen is null)
        {
            return RustlynInvalidAbi;
        }

        *resultJson = null;
        *resultLen = 0;

        if (optionsJson is null || optionsLen == 0 || optionsLen > int.MaxValue)
        {
            return RustlynInvalidAbi;
        }

        RustlynEmitResult result;
        var exitCode = RustlynOk;
        try
        {
            var options = JsonSerializer.Deserialize(
                new ReadOnlySpan<byte>(optionsJson, checked((int)optionsLen)),
                RustlynNativeAotJsonContext.Default.RustlynEmitOptions)
                ?? throw new InvalidOperationException("Options JSON did not produce an options object.");

            var inputPath = RequirePath(options.InputPath, nameof(options.InputPath));
            var outputPath = Path.GetFullPath(RequirePath(options.OutputPath, nameof(options.OutputPath)));
            var loweredModule = LoweredIrLowerer.LowerBitcode(inputPath, options.LlvmRoot);
            var emitOptions = new EmitOptions
            {
                EmitPdb = options.EmitPdb,
                StrictUnsupportedIr = options.StrictUnsupportedIr
            };
            LoweredAssemblyEmitter.EmitModule(loweredModule, outputPath, emitOptions);

            result = RustlynEmitResult.CreateSuccess(outputPath, GetOutputFiles(outputPath, options.EmitPdb));
        }
        catch (Exception ex)
        {
            exitCode = RustlynOperationFailed;
            result = RustlynEmitResult.CreateFailure(ex);
        }

        return TryWriteResult(result, resultJson, resultLen)
            ? exitCode
            : RustlynResultAllocationFailed;
    }

    [UnmanagedCallersOnly(EntryPoint = "rustlyn_free", CallConvs = [typeof(CallConvCdecl)])]
    public static void Free(byte* ptr)
    {
        NativeMemory.Free(ptr);
    }

    private static string RequirePath(string? path, string optionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path, optionName);
        return path;
    }

    private static string[] GetOutputFiles(string outputPath, bool emitPdb)
    {
        var files = new List<string> { outputPath };
        var runtimeConfigPath = Path.ChangeExtension(outputPath, ".runtimeconfig.json");
        if (File.Exists(runtimeConfigPath))
        {
            files.Add(runtimeConfigPath);
        }

        if (emitPdb)
        {
            var pdbPath = Path.ChangeExtension(outputPath, ".pdb");
            if (File.Exists(pdbPath))
            {
                files.Add(pdbPath);
            }
        }

        return [.. files];
    }

    private static bool TryWriteResult(RustlynEmitResult result, byte** resultJson, nuint* resultLen)
    {
        byte* buffer = null;
        try
        {
            byte[] bytes;
            try
            {
                bytes = JsonSerializer.SerializeToUtf8Bytes(
                    result,
                    RustlynNativeAotJsonContext.Default.RustlynEmitResult);
            }
            catch
            {
                bytes = ResultSerializationFailedJson;
            }

            buffer = (byte*)NativeMemory.Alloc((nuint)bytes.Length);
            if (buffer is null)
            {
                return false;
            }

            bytes.AsSpan().CopyTo(new Span<byte>(buffer, bytes.Length));
            *resultJson = buffer;
            *resultLen = (nuint)bytes.Length;
            return true;
        }
        catch
        {
            NativeMemory.Free(buffer);
            *resultJson = null;
            *resultLen = 0;
            return false;
        }
    }
}

internal sealed class RustlynEmitOptions
{
    public string? InputPath { get; set; }

    public string? OutputPath { get; set; }

    public string? LlvmRoot { get; set; }

    public bool EmitPdb { get; set; }

    public bool StrictUnsupportedIr { get; set; }
}

internal sealed class RustlynEmitResult
{
    public bool Success { get; set; }

    public string? OutputPath { get; set; }

    public string[] OutputFiles { get; set; } = [];

    public RustlynEmitDiagnostic[] Diagnostics { get; set; } = [];

    public string? ExceptionType { get; set; }

    public static RustlynEmitResult CreateSuccess(string outputPath, string[] outputFiles)
        => new()
        {
            Success = true,
            OutputPath = outputPath,
            OutputFiles = outputFiles
        };

    public static RustlynEmitResult CreateFailure(Exception ex)
        => new()
        {
            Success = false,
            ExceptionType = ex.GetType().FullName,
            Diagnostics =
            [
                new RustlynEmitDiagnostic
                {
                    Severity = "error",
                    Message = ex.Message
                }
            ]
        };
}

internal sealed class RustlynEmitDiagnostic
{
    public string Severity { get; set; } = "";

    public string Message { get; set; } = "";
}

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(RustlynEmitOptions))]
[JsonSerializable(typeof(RustlynEmitResult))]
internal sealed partial class RustlynNativeAotJsonContext : JsonSerializerContext;
