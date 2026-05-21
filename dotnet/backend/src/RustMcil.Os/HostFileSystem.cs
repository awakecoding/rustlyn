using RustMcil.Interop;

namespace RustMcil.Os;

public static class HostFileSystem
{
    public static int Utf8ReadAllLinesCount(IntPtr pathPointer, long pathLength)
    {
        return File.ReadAllLines(InteropUtf8.ReadString(pathPointer, pathLength)).Length;
    }

    public static int Utf8ReadAllLinesLineLength(IntPtr pathPointer, long pathLength, int index)
    {
        return InteropUtf8.GetByteCount(GetUtf8FileLine(pathPointer, pathLength, index));
    }

    public static int CopyUtf8ReadAllLinesLine(IntPtr pathPointer, long pathLength, int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(GetUtf8FileLine(pathPointer, pathLength, index), destinationPointer, destinationCapacity);
    }

    private static string GetUtf8FileLine(IntPtr pathPointer, long pathLength, int index)
    {
        var lines = File.ReadAllLines(InteropUtf8.ReadString(pathPointer, pathLength));
        if ((uint)index >= (uint)lines.Length)
        {
            throw new IndexOutOfRangeException($"File line index {index} was outside the available range 0..{lines.Length - 1}.");
        }

        return lines[index];
    }
}