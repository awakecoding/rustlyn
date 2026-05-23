using System.Runtime.InteropServices;

namespace RustMcil.Backend;

public static partial class RuntimeBridgeHelpers
{
    public static int CommandLineArgCount()
    {
        return RustMcil.Os.HostEnvironment.CommandLineArgCount();
    }

    public static int Utf8CommandLineArgLength(int index)
    {
        return RustMcil.Os.HostEnvironment.Utf8CommandLineArgLength(index);
    }

    public static int CopyUtf8CommandLineArg(int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostEnvironment.CopyUtf8CommandLineArg(index, destinationPointer, destinationCapacity);
    }

    public static void ConsoleWriteLineUtf8(IntPtr valuePointer, long valueLength)
    {
        RustMcil.Os.HostConsole.WriteLineUtf8(valuePointer, valueLength);
    }

    public static void ConsoleWritePrefixedLineUtf8(IntPtr pathPointer, long pathLength, int lineNumber, IntPtr valuePointer, long valueLength)
    {
        RustMcil.Os.HostConsole.WritePrefixedLineUtf8(pathPointer, pathLength, lineNumber, valuePointer, valueLength);
    }

    public static void ConsoleWritePathLineUtf8(IntPtr pathPointer, long pathLength, IntPtr valuePointer, long valueLength)
    {
        RustMcil.Os.HostConsole.WritePathLineUtf8(pathPointer, pathLength, valuePointer, valueLength);
    }

    public static void ConsoleWriteNumberedLineUtf8(int lineNumber, IntPtr valuePointer, long valueLength)
    {
        RustMcil.Os.HostConsole.WriteNumberedLineUtf8(lineNumber, valuePointer, valueLength);
    }

    public static void ConsoleWriteI32(int value)
    {
        RustMcil.Os.HostConsole.WriteI32(value);
    }

    public static void ConsoleWritePathCountUtf8(IntPtr pathPointer, long pathLength, int value)
    {
        RustMcil.Os.HostConsole.WritePathCountUtf8(pathPointer, pathLength, value);
    }

    public static int Utf8ReadAllLinesCount(IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostFileSystem.Utf8ReadAllLinesCount(pathPointer, pathLength);
    }

    public static int Utf8ReadAllLinesLineLength(IntPtr pathPointer, long pathLength, int index)
    {
        return RustMcil.Os.HostFileSystem.Utf8ReadAllLinesLineLength(pathPointer, pathLength, index);
    }

    public static int CopyUtf8ReadAllLinesLine(IntPtr pathPointer, long pathLength, int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostFileSystem.CopyUtf8ReadAllLinesLine(pathPointer, pathLength, index, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRootLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetRootLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetRoot(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathGetRoot(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFullPathLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetFullPathLengthUtf8(pathPointer, pathLength, basePointer, baseLength);
    }

    public static int CopyUtf8PathGetFullPath(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathGetFullPath(pathPointer, pathLength, basePointer, baseLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetDirectoryNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetDirectoryNameLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetDirectoryName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathGetDirectoryName(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRelativeLengthUtf8(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetRelativeLengthUtf8(relativeToPointer, relativeToLength, pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetRelative(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathGetRelative(relativeToPointer, relativeToLength, pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8DocumentsLength()
    {
        return RustMcil.Os.HostEnvironment.Utf8DocumentsLength();
    }

    public static int CopyUtf8Documents(IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostEnvironment.CopyUtf8Documents(destinationPointer, destinationCapacity);
    }

    public static int Utf8TempPathLength()
    {
        return RustMcil.Os.HostEnvironment.Utf8TempPathLength();
    }

    public static int CopyUtf8TempPath(IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostEnvironment.CopyUtf8TempPath(destinationPointer, destinationCapacity);
    }

    public static int Utf8UserProfileLength()
    {
        return RustMcil.Os.HostEnvironment.Utf8UserProfileLength();
    }

    public static int CopyUtf8UserProfile(IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostEnvironment.CopyUtf8UserProfile(destinationPointer, destinationCapacity);
    }

    public static int Utf8CurrentDirectoryLength()
    {
        return RustMcil.Os.HostEnvironment.Utf8CurrentDirectoryLength();
    }

    public static int CopyUtf8CurrentDirectory(IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostEnvironment.CopyUtf8CurrentDirectory(destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombine3LengthUtf8(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return RustMcil.Os.HostPath.Utf8PathCombine3LengthUtf8(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength);
    }

    public static int CopyUtf8PathCombine3(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathCombine3(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombineLengthUtf8(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return RustMcil.Os.HostPath.Utf8PathCombineLengthUtf8(leftPointer, leftLength, rightPointer, rightLength);
    }

    public static int CopyUtf8PathCombine(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathCombine(leftPointer, leftLength, rightPointer, rightLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathChangeExtensionLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return RustMcil.Os.HostPath.Utf8PathChangeExtensionLengthUtf8(pathPointer, pathLength, extensionPointer, extensionLength);
    }

    public static int CopyUtf8PathChangeExtension(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathChangeExtension(pathPointer, pathLength, extensionPointer, extensionLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFileNameWithoutExtensionLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetFileNameWithoutExtensionLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetFileNameWithoutExtension(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathGetFileNameWithoutExtension(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8StringReplaceLength(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength)
    {
        return RustMcil.Interop.InteropUtf8.ReplaceByteCount(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength);
    }

    public static int CopyUtf8StringReplace(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength,
        IntPtr destinationPointer,
        long destinationCapacity)
    {
        return RustMcil.Interop.InteropUtf8.CopyReplace(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFileNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetFileNameLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetFileName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return RustMcil.Os.HostPath.CopyUtf8PathGetFileName(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8StringContains(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return RustMcil.Interop.InteropUtf8.ContainsOrdinal(haystackPointer, haystackLength, needlePointer, needleLength);
    }

    public static int Utf8StringIndexOf(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return RustMcil.Interop.InteropUtf8.IndexOfOrdinal(haystackPointer, haystackLength, needlePointer, needleLength);
    }

    public static int RemEuclidI32(int left, int right)
    {
        return RustMcil.Runtime.NumericRuntime.RemEuclidI32(left, right);
    }

    public static long RemEuclidI64(long left, long right)
    {
        return RustMcil.Runtime.NumericRuntime.RemEuclidI64(left, right);
    }

    public static int Utf8PathGetFileNameLength(IntPtr pathPointer, long pathLength)
    {
        return RustMcil.Os.HostPath.Utf8PathGetFileNameLength(pathPointer, pathLength);
    }

    public static void InitializeWtf8PathBuffer(IntPtr destination, IntPtr source, long length)
    {
        RustMcil.Runtime.RustBufferRuntime.InitializeBufferFromBytes(destination, source, length);
    }

    public static void AppendPathSegment(IntPtr destination, IntPtr source, long length)
    {
        RustMcil.Os.HostPath.AppendPathSegment(destination, source, length);
    }

    public static void ReadFileToRustString(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        RustMcil.Os.HostFileSystem.ReadFileToRustString(destination, pathPointer, pathLength);
    }

    private static void WriteExceptionOut(IntPtr exceptionOutPointer, int exceptionHandle)
    {
        if (exceptionOutPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(exceptionOutPointer), "Generated binding calls must provide an exception out pointer.");
        }

        Marshal.WriteInt32(exceptionOutPointer, exceptionHandle);
    }
}
