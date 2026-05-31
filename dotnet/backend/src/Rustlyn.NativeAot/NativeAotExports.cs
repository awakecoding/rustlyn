using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Rustlyn.Backend;

namespace Rustlyn.NativeAot;

public static unsafe class NativeAotExports
{
    private const int RustlynOk = 0;
    private const int RustlynOperationFailed = 1;
    private const int RustlynUsageError = 2;
    private const int RustlynUnsupportedIr = 4;
    private const int RustlynInvalidAbi = -1;
    private const int RustlynResultAllocationFailed = -2;
    private const int CurrentAbiSchemaVersion = 1;
    private const int CurrentHostCallbackSchemaVersion = 1;

    private static readonly byte[] ResultSerializationFailedJson =
        "{\"schemaVersion\":1,\"success\":false,\"exitCode\":1,\"diagnostics\":[{\"severity\":\"error\",\"code\":\"result-serialization-failed\",\"message\":\"result serialization failed\"}],\"outputFiles\":[]}"u8.ToArray();

    [UnmanagedCallersOnly(EntryPoint = "rustlyn_register_host_callbacks", CallConvs = [typeof(CallConvCdecl)])]
    public static int RegisterHostCallbacks(int schemaVersion, RustlynHostCallbacks* callbacks)
    {
        if (callbacks is null || schemaVersion != CurrentHostCallbackSchemaVersion ||
            callbacks->SchemaVersion != CurrentHostCallbackSchemaVersion ||
            callbacks->PrintIr is null ||
            callbacks->Free is null)
        {
            return RustlynInvalidAbi;
        }

        InProcessLlvmModuleReader.RegisterHostCallbacks(callbacks->PrintIr, callbacks->Free);
        return RustlynOk;
    }

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

            RequireSchemaVersion(options.SchemaVersion);

            var inputPath = RequirePath(options.InputPath, nameof(options.InputPath));
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("Bitcode artifact was not found.", inputPath);
            }

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
            exitCode = MapExceptionToExitCode(ex);
            result = RustlynEmitResult.CreateFailure(ex, exitCode, GetDiagnosticCode(ex));
        }

        return TryWriteJsonResult(result, RustlynNativeAotJsonContext.Default.RustlynEmitResult, resultJson, resultLen)
            ? exitCode
            : RustlynResultAllocationFailed;
    }

    [UnmanagedCallersOnly(EntryPoint = "rustlyn_lower", CallConvs = [typeof(CallConvCdecl)])]
    public static int Lower(byte* optionsJson, nuint optionsLen, byte** resultJson, nuint* resultLen)
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

        RustlynLowerResult result;
        var exitCode = RustlynOk;
        try
        {
            var options = JsonSerializer.Deserialize(
                new ReadOnlySpan<byte>(optionsJson, checked((int)optionsLen)),
                RustlynNativeAotJsonContext.Default.RustlynLowerOptions)
                ?? throw new InvalidOperationException("Options JSON did not produce an options object.");

            RequireSchemaVersion(options.SchemaVersion);

            var inputPath = RequirePath(options.InputPath, nameof(options.InputPath));
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("Bitcode artifact was not found.", inputPath);
            }

            var loweredModule = LoweredIrLowerer.LowerBitcode(inputPath, null);
            result = RustlynLowerResult.CreateSuccess(LoweredIrLowerer.Dump(loweredModule));
        }
        catch (Exception ex)
        {
            exitCode = MapExceptionToExitCode(ex);
            result = RustlynLowerResult.CreateFailure(ex, exitCode, GetDiagnosticCode(ex));
        }

        return TryWriteJsonResult(result, RustlynNativeAotJsonContext.Default.RustlynLowerResult, resultJson, resultLen)
            ? exitCode
            : RustlynResultAllocationFailed;
    }

    [UnmanagedCallersOnly(EntryPoint = "rustlyn_pack", CallConvs = [typeof(CallConvCdecl)])]
    public static int Pack(byte* optionsJson, nuint optionsLen, byte** resultJson, nuint* resultLen)
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

        RustlynPackResult result;
        var exitCode = RustlynOk;
        try
        {
            var options = JsonSerializer.Deserialize(
                new ReadOnlySpan<byte>(optionsJson, checked((int)optionsLen)),
                RustlynNativeAotJsonContext.Default.RustlynPackOptions)
                ?? throw new InvalidOperationException("Options JSON did not produce an options object.");

            RequireSchemaVersion(options.SchemaVersion);

            var crateName = RequireValue(options.CrateName, nameof(options.CrateName));
            var version = RequireValue(options.Version, nameof(options.Version));
            var assemblyPath = Path.GetFullPath(RequirePath(options.AssemblyPath, nameof(options.AssemblyPath)));
            var outputDir = Path.GetFullPath(RequirePath(options.OutputDir, nameof(options.OutputDir)));
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException("Translated assembly was not found.", assemblyPath);
            }

            Directory.CreateDirectory(outputDir);
            var spec = NuGetPackager.CreatePackSpec(crateName, version, assemblyPath);
            var nuspecPath = Path.Combine(outputDir, $"{spec.PackageId}.nuspec");
            File.WriteAllText(nuspecPath, NuGetPackager.GenerateNuspec(spec));

            var nupkgPath = Path.Combine(outputDir, $"{spec.PackageId}.{spec.Version}.nupkg");
            NuGetPackager.WriteNupkg(spec, nupkgPath);

            result = RustlynPackResult.CreateSuccess(
                spec.PackageId,
                spec.Version,
                assemblyPath,
                nuspecPath,
                nupkgPath,
                spec.Files.Count);
        }
        catch (Exception ex)
        {
            exitCode = MapExceptionToExitCode(ex);
            result = RustlynPackResult.CreateFailure(ex, exitCode, GetDiagnosticCode(ex));
        }

        return TryWriteJsonResult(result, RustlynNativeAotJsonContext.Default.RustlynPackResult, resultJson, resultLen)
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

    private static string RequireValue(string? value, string optionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, optionName);
        return value;
    }

    private static void RequireSchemaVersion(int schemaVersion)
    {
        if (schemaVersion != CurrentAbiSchemaVersion)
        {
            throw new InvalidOperationException(
                $"Unsupported rustlyn NativeAOT ABI schema version {schemaVersion}; expected {CurrentAbiSchemaVersion}.");
        }
    }

    private static int MapExceptionToExitCode(Exception ex)
        => ex switch
        {
            UnsupportedIrException => RustlynUnsupportedIr,
            NotSupportedException => RustlynUnsupportedIr,
            FileNotFoundException => RustlynUsageError,
            DirectoryNotFoundException => RustlynUsageError,
            InvalidOperationException invalidOperation
                when invalidOperation.Message.StartsWith("Unsupported rustlyn NativeAOT ABI schema version ", StringComparison.Ordinal)
                => RustlynUsageError,
            InvalidOperationException invalidOperation
                when invalidOperation.Message.Equals("Rustlyn host callbacks were not registered.", StringComparison.Ordinal)
                => RustlynUsageError,
            ArgumentException => RustlynUsageError,
            _ => RustlynOperationFailed
        };

    private static string GetDiagnosticCode(Exception ex)
        => ex switch
        {
            UnsupportedIrException => "unsupported-ir",
            NotSupportedException => "unsupported-ir",
            FileNotFoundException => "missing-artifact",
            DirectoryNotFoundException => "missing-artifact",
            InvalidOperationException invalidOperation
                when invalidOperation.Message.StartsWith("Unsupported rustlyn NativeAOT ABI schema version ", StringComparison.Ordinal)
                => "unsupported-abi-schema",
            InvalidOperationException invalidOperation
                when invalidOperation.Message.Equals("Rustlyn host callbacks were not registered.", StringComparison.Ordinal)
                => "host-callbacks-not-registered",
            ArgumentException => "invalid-request",
            _ => "operation-failed"
        };

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

    private static bool TryWriteJsonResult<T>(T result, JsonTypeInfo<T> jsonTypeInfo, byte** resultJson, nuint* resultLen)
    {
        byte* buffer = null;
        try
        {
            byte[] bytes;
            try
            {
                bytes = JsonSerializer.SerializeToUtf8Bytes(
                    result,
                    jsonTypeInfo);
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

[StructLayout(LayoutKind.Sequential)]
public unsafe struct RustlynHostCallbacks
{
    public int SchemaVersion;

    public delegate* unmanaged[Cdecl]<byte*, nuint, byte**, nuint*, int> PrintIr;

    public delegate* unmanaged[Cdecl]<byte*, void> Free;
}

internal sealed class RustlynEmitOptions
{
    public int SchemaVersion { get; set; }

    public string? InputPath { get; set; }

    public string? OutputPath { get; set; }

    public string? LlvmRoot { get; set; }

    public bool EmitPdb { get; set; }

    public bool StrictUnsupportedIr { get; set; }
}

internal sealed class RustlynLowerOptions
{
    public int SchemaVersion { get; set; }

    public string? InputPath { get; set; }
}

internal sealed class RustlynPackOptions
{
    public int SchemaVersion { get; set; }

    public string? CrateName { get; set; }

    public string? Version { get; set; }

    public string? AssemblyPath { get; set; }

    public string? OutputDir { get; set; }
}

internal sealed class RustlynEmitResult
{
    public int SchemaVersion { get; set; } = 1;

    public bool Success { get; set; }

    public int ExitCode { get; set; }

    public string? OutputPath { get; set; }

    public string[] OutputFiles { get; set; } = [];

    public RustlynEmitDiagnostic[] Diagnostics { get; set; } = [];

    public string? ExceptionType { get; set; }

    public static RustlynEmitResult CreateSuccess(string outputPath, string[] outputFiles)
        => new()
        {
            Success = true,
            ExitCode = 0,
            OutputPath = outputPath,
            OutputFiles = outputFiles
        };

    public static RustlynEmitResult CreateFailure(Exception ex, int exitCode, string diagnosticCode)
        => new()
        {
            Success = false,
            ExitCode = exitCode,
            ExceptionType = ex.GetType().FullName,
            Diagnostics =
            [
                new RustlynEmitDiagnostic
                {
                    Severity = "error",
                    Code = diagnosticCode,
                    Message = ex.Message
                }
            ]
        };
}

internal sealed class RustlynLowerResult
{
    public int SchemaVersion { get; set; } = 1;

    public bool Success { get; set; }

    public int ExitCode { get; set; }

    public string? LoweredIr { get; set; }

    public RustlynEmitDiagnostic[] Diagnostics { get; set; } = [];

    public string? ExceptionType { get; set; }

    public static RustlynLowerResult CreateSuccess(string loweredIr)
        => new()
        {
            Success = true,
            ExitCode = 0,
            LoweredIr = loweredIr
        };

    public static RustlynLowerResult CreateFailure(Exception ex, int exitCode, string diagnosticCode)
        => new()
        {
            Success = false,
            ExitCode = exitCode,
            ExceptionType = ex.GetType().FullName,
            Diagnostics =
            [
                new RustlynEmitDiagnostic
                {
                    Severity = "error",
                    Code = diagnosticCode,
                    Message = ex.Message
                }
            ]
        };
}

internal sealed class RustlynPackResult
{
    public int SchemaVersion { get; set; } = 1;

    public bool Success { get; set; }

    public int ExitCode { get; set; }

    public string? PackageId { get; set; }

    public string? Version { get; set; }

    public string? AssemblyPath { get; set; }

    public string? NuspecPath { get; set; }

    public string? NupkgPath { get; set; }

    public int Files { get; set; }

    public RustlynEmitDiagnostic[] Diagnostics { get; set; } = [];

    public string? ExceptionType { get; set; }

    public static RustlynPackResult CreateSuccess(
        string packageId,
        string version,
        string assemblyPath,
        string nuspecPath,
        string nupkgPath,
        int files)
        => new()
        {
            Success = true,
            ExitCode = 0,
            PackageId = packageId,
            Version = version,
            AssemblyPath = assemblyPath,
            NuspecPath = nuspecPath,
            NupkgPath = nupkgPath,
            Files = files
        };

    public static RustlynPackResult CreateFailure(Exception ex, int exitCode, string diagnosticCode)
        => new()
        {
            Success = false,
            ExitCode = exitCode,
            ExceptionType = ex.GetType().FullName,
            Diagnostics =
            [
                new RustlynEmitDiagnostic
                {
                    Severity = "error",
                    Code = diagnosticCode,
                    Message = ex.Message
                }
            ]
        };
}

internal sealed class RustlynEmitDiagnostic
{
    public string Severity { get; set; } = "";

    public string Code { get; set; } = "";

    public string Message { get; set; } = "";
}

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(RustlynEmitOptions))]
[JsonSerializable(typeof(RustlynEmitResult))]
[JsonSerializable(typeof(RustlynLowerOptions))]
[JsonSerializable(typeof(RustlynLowerResult))]
[JsonSerializable(typeof(RustlynPackOptions))]
[JsonSerializable(typeof(RustlynPackResult))]
internal sealed partial class RustlynNativeAotJsonContext : JsonSerializerContext;
