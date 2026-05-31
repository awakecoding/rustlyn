#if RUSTLYN_BACKEND_IN_PROCESS_LLVM
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Rustlyn.Backend;

public static unsafe class InProcessLlvmModuleReader
{
    private static nint s_printIr;
    private static nint s_free;

    public static void RegisterHostCallbacks(
        delegate* unmanaged[Cdecl]<byte*, nuint, byte**, nuint*, int> printIr,
        delegate* unmanaged[Cdecl]<byte*, void> free)
    {
        if (printIr is null)
        {
            throw new ArgumentNullException(nameof(printIr));
        }

        if (free is null)
        {
            throw new ArgumentNullException(nameof(free));
        }

        Volatile.Write(ref s_printIr, (nint)printIr);
        Volatile.Write(ref s_free, (nint)free);
    }

    public static string ReadLlvmIr(string artifactPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);

        var printIr = (delegate* unmanaged[Cdecl]<byte*, nuint, byte**, nuint*, int>)
            Volatile.Read(ref s_printIr);
        var free = (delegate* unmanaged[Cdecl]<byte*, void>)
            Volatile.Read(ref s_free);
        if (printIr is null || free is null)
        {
            throw new InvalidOperationException("Rustlyn host callbacks were not registered.");
        }

        var pathBytes = Encoding.UTF8.GetBytes(Path.GetFullPath(artifactPath));
        byte* resultPtr = null;
        nuint resultLen = 0;

        fixed (byte* pathPtr = pathBytes)
        {
            var code = printIr(pathPtr, (nuint)pathBytes.Length, &resultPtr, &resultLen);
            try
            {
                var result = ReadResult(resultPtr, resultLen);
                if (code == 0)
                {
                    return result;
                }

                throw new InvalidDataException(string.IsNullOrWhiteSpace(result)
                    ? $"In-process LLVM print-ir failed with code {code}."
                    : result);
            }
            finally
            {
                free(resultPtr);
            }
        }
    }

    private static string ReadResult(byte* ptr, nuint len)
    {
        if (ptr is null || len == 0)
        {
            return string.Empty;
        }

        if (len > int.MaxValue)
        {
            throw new InvalidDataException("In-process LLVM returned a result that is too large.");
        }

        return Marshal.PtrToStringUTF8((nint)ptr, checked((int)len)) ?? string.Empty;
    }
}
#endif
