using RustMcil.Interop;

namespace RustMcil.Os;

public static class HostPath
{
    public static int Utf8PathCombine3LengthUtf8(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return InteropUtf8.GetByteCount(CombineUtf8Paths(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength));
    }

    public static int CopyUtf8PathCombine3(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            CombineUtf8Paths(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathCombineLengthUtf8(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return InteropUtf8.GetByteCount(CombineUtf8Paths(leftPointer, leftLength, rightPointer, rightLength));
    }

    public static int CopyUtf8PathCombine(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            CombineUtf8Paths(leftPointer, leftLength, rightPointer, rightLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathChangeExtensionLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return InteropUtf8.GetByteCount(ChangeExtensionUtf8Path(pathPointer, pathLength, extensionPointer, extensionLength));
    }

    public static int CopyUtf8PathChangeExtension(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            ChangeExtensionUtf8Path(pathPointer, pathLength, extensionPointer, extensionLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFileNameWithoutExtensionLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return InteropUtf8.GetByteCount(GetFileNameWithoutExtensionUtf8Path(pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetFileNameWithoutExtension(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            GetFileNameWithoutExtensionUtf8Path(pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFileNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return InteropUtf8.GetByteCount(GetFileNameUtf8Path(pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetFileName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            GetFileNameUtf8Path(pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFileNameLength(IntPtr pathPointer, long pathLength)
    {
        return GetFileNameUtf8Path(pathPointer, pathLength).Length;
    }

    private static string CombineUtf8Paths(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return Path.Combine(
            InteropUtf8.ReadString(leftPointer, leftLength),
            InteropUtf8.ReadString(rightPointer, rightLength));
    }

    private static string CombineUtf8Paths(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return Path.Combine(
            InteropUtf8.ReadString(firstPointer, firstLength),
            InteropUtf8.ReadString(secondPointer, secondLength),
            InteropUtf8.ReadString(thirdPointer, thirdLength));
    }

    private static string ChangeExtensionUtf8Path(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return Path.ChangeExtension(
            InteropUtf8.ReadString(pathPointer, pathLength),
            InteropUtf8.ReadString(extensionPointer, extensionLength))
            ?? string.Empty;
    }

    private static string GetFileNameUtf8Path(IntPtr pathPointer, long pathLength)
    {
        return Path.GetFileName(InteropUtf8.ReadString(pathPointer, pathLength)) ?? string.Empty;
    }

    private static string GetFileNameWithoutExtensionUtf8Path(IntPtr pathPointer, long pathLength)
    {
        return Path.GetFileNameWithoutExtension(InteropUtf8.ReadString(pathPointer, pathLength)) ?? string.Empty;
    }
}