using RustMcil.Interop;

namespace RustMcil.Os;

public static class HostEnvironment
{
    public static int CommandLineArgCount()
    {
        return Environment.GetCommandLineArgs().Length;
    }

    public static int Utf8CommandLineArgLength(int index)
    {
        return InteropUtf8.GetByteCount(GetCommandLineArg(index));
    }

    public static int CopyUtf8CommandLineArg(int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(GetCommandLineArg(index), destinationPointer, destinationCapacity);
    }

    private static string GetCommandLineArg(int index)
    {
        var args = Environment.GetCommandLineArgs();
        if ((uint)index >= (uint)args.Length)
        {
            throw new IndexOutOfRangeException($"Command line argument index {index} was outside the available range 0..{args.Length - 1}.");
        }

        return args[index];
    }
}
