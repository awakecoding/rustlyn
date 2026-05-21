using RustMcil.Interop;

namespace RustMcil.Os;

public static class HostPath
{
    public static int Utf8PathGetRootLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return InteropUtf8.GetByteCount(GetRootUtf8Path(pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetRoot(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            GetRootUtf8Path(pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFullPathLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return InteropUtf8.GetByteCount(GetFullUtf8Path(pathPointer, pathLength, basePointer, baseLength));
    }

    public static int CopyUtf8PathGetFullPath(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            GetFullUtf8Path(pathPointer, pathLength, basePointer, baseLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetDirectoryNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return InteropUtf8.GetByteCount(GetDirectoryNameUtf8Path(pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetDirectoryName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            GetDirectoryNameUtf8Path(pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetRelativeLengthUtf8(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return InteropUtf8.GetByteCount(GetRelativeUtf8Path(relativeToPointer, relativeToLength, pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetRelative(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(
            GetRelativeUtf8Path(relativeToPointer, relativeToLength, pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

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

    private static string GetDirectoryNameUtf8Path(IntPtr pathPointer, long pathLength)
    {
        return Path.GetDirectoryName(InteropUtf8.ReadString(pathPointer, pathLength)) ?? string.Empty;
    }

    private static string GetFullUtf8Path(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return Path.GetFullPath(
            InteropUtf8.ReadString(pathPointer, pathLength),
            InteropUtf8.ReadString(basePointer, baseLength));
    }

    private static string GetRootUtf8Path(IntPtr pathPointer, long pathLength)
    {
        return Path.GetPathRoot(InteropUtf8.ReadString(pathPointer, pathLength)) ?? string.Empty;
    }

    private static string GetRelativeUtf8Path(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return Path.GetRelativePath(
            InteropUtf8.ReadString(relativeToPointer, relativeToLength),
            InteropUtf8.ReadString(pathPointer, pathLength));
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