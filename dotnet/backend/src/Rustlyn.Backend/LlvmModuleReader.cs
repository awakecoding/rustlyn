using System.Runtime.InteropServices;
using LLVMSharp.Interop;
using static LLVMSharp.Interop.LLVM;

namespace Rustlyn.Backend;

internal static class LlvmModuleReader
{
    public static LlvmModuleSummary? TryReadSummary(string artifactPath, string? llvmRoot)
    {
        var toolchainRoot = LlvmNativeLibraryLocator.TryResolveToolchainRoot(llvmRoot);
        if (toolchainRoot is null)
        {
            return null;
        }

        var irTool = LlvmNativeLibraryLocator.TryGetIrToolPath(toolchainRoot);
        if (irTool?.Kind == LlvmIrToolKind.RustlynLlvm)
        {
            return RustlynLlvmModuleReader.ReadSummary(artifactPath, toolchainRoot, irTool.Path);
        }

        var configuredRoot = LlvmNativeLibraryLocator.TryConfigure(toolchainRoot);
        return configuredRoot is not null
            ? ReadWithInterop(artifactPath, configuredRoot)
            : LlvmToolingModuleReader.ReadSummary(artifactPath, toolchainRoot);
    }

    private static LlvmModuleSummary ReadWithInterop(string artifactPath, string configuredRoot)
    {

        unsafe
        {
            var pathUtf8 = Marshal.StringToCoTaskMemUTF8(artifactPath);
            sbyte* loadError = null;
            LLVMOpaqueMemoryBuffer* memoryBufferHandle = null;
            try
            {
                if (CreateMemoryBufferWithContentsOfFile((sbyte*)pathUtf8, &memoryBufferHandle, &loadError) != 0)
                {
                    throw new InvalidDataException(ConsumeMessage(loadError) ?? $"Failed to create LLVM memory buffer for '{artifactPath}'.");
                }

                LLVMMemoryBufferRef memoryBuffer = memoryBufferHandle;

                try
                {
                    LLVMOpaqueModule* moduleHandle = null;
                    if (ParseBitcode2(memoryBuffer, &moduleHandle) != 0)
                    {
                        throw new InvalidDataException($"Failed to parse LLVM bitcode module '{artifactPath}'.");
                    }

                    LLVMModuleRef module = moduleHandle;

                    try
                    {
                        return BuildSummary(module, configuredRoot);
                    }
                    finally
                    {
                        DisposeModule(module);
                    }
                }
                finally
                {
                    DisposeMemoryBuffer(memoryBuffer);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(pathUtf8);
            }
        }
    }

    private static LlvmModuleSummary BuildSummary(LLVMModuleRef module, string llvmRoot)
    {
        var functions = new List<LlvmFunctionSummary>();
        for (var function = module.FirstFunction; !function.IsNull; function = function.NextFunction)
        {
            var basicBlockCount = 0;
            var instructionCount = 0;

            for (var block = function.FirstBasicBlock; block.Handle != nint.Zero; block = block.Next)
            {
                basicBlockCount++;

                for (var instruction = block.FirstInstruction; !instruction.IsNull; instruction = instruction.NextInstruction)
                {
                    instructionCount++;
                }
            }

            functions.Add(new LlvmFunctionSummary(function.Name, basicBlockCount, instructionCount));
        }

        var globals = new List<LlvmGlobalSummary>();
        for (var global = module.FirstGlobal; !global.IsNull; global = global.NextGlobal)
        {
            globals.Add(new LlvmGlobalSummary(global.Name));
        }

        return new LlvmModuleSummary(llvmRoot, "llvmsharp-interop", functions, [], globals);
    }

    private static unsafe string? ConsumeMessage(sbyte* message)
    {
        if (message is null)
        {
            return null;
        }

        try
        {
            return Marshal.PtrToStringAnsi((nint)message);
        }
        finally
        {
            DisposeMessage(message);
        }
    }
}
