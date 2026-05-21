using RustMcil.Interop;

namespace RustMcil.Os;

public static class HostConsole
{
    public static void WriteLineUtf8(IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine(InteropUtf8.ReadString(valuePointer, valueLength));
    }

    public static void WritePrefixedLineUtf8(IntPtr pathPointer, long pathLength, int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine($"{InteropUtf8.ReadString(pathPointer, pathLength)}:{lineNumber}:{InteropUtf8.ReadString(valuePointer, valueLength)}");
    }

    public static void WritePathLineUtf8(IntPtr pathPointer, long pathLength, IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine($"{InteropUtf8.ReadString(pathPointer, pathLength)}:{InteropUtf8.ReadString(valuePointer, valueLength)}");
    }

    public static void WriteNumberedLineUtf8(int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine($"{lineNumber}:{InteropUtf8.ReadString(valuePointer, valueLength)}");
    }

    public static void WriteI32(int value)
    {
        Console.WriteLine(value);
    }

    public static void WritePathCountUtf8(IntPtr pathPointer, long pathLength, int value)
    {
        Console.WriteLine($"{InteropUtf8.ReadString(pathPointer, pathLength)}:{value}");
    }
}