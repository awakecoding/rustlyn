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
}