using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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
        Console.WriteLine(ReadUtf8String(valuePointer, valueLength));
    }

    public static void ConsoleWritePrefixedLineUtf8(IntPtr pathPointer, long pathLength, int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine($"{ReadUtf8String(pathPointer, pathLength)}:{lineNumber}:{ReadUtf8String(valuePointer, valueLength)}");
    }

    public static void ConsoleWritePathLineUtf8(IntPtr pathPointer, long pathLength, IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine($"{ReadUtf8String(pathPointer, pathLength)}:{ReadUtf8String(valuePointer, valueLength)}");
    }

    public static void ConsoleWriteNumberedLineUtf8(int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Console.WriteLine($"{lineNumber}:{ReadUtf8String(valuePointer, valueLength)}");
    }

    public static void ConsoleWriteI32(int value)
    {
        Console.WriteLine(value);
    }

    public static void ConsoleWritePathCountUtf8(IntPtr pathPointer, long pathLength, int value)
    {
        Console.WriteLine($"{ReadUtf8String(pathPointer, pathLength)}:{value}");
    }

    public static int Utf8ReadAllLinesCount(IntPtr pathPointer, long pathLength)
    {
        return File.ReadAllLines(ReadUtf8String(pathPointer, pathLength)).Length;
    }

    public static int Utf8ReadAllLinesLineLength(IntPtr pathPointer, long pathLength, int index)
    {
        return Encoding.UTF8.GetByteCount(GetUtf8FileLine(pathPointer, pathLength, index));
    }

    public static int CopyUtf8ReadAllLinesLine(IntPtr pathPointer, long pathLength, int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(GetUtf8FileLine(pathPointer, pathLength, index), destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRootLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Encoding.UTF8.GetByteCount(GetRootUtf8Path(pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetRoot(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            GetRootUtf8Path(pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFullPathLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return Encoding.UTF8.GetByteCount(GetFullUtf8Path(pathPointer, pathLength, basePointer, baseLength));
    }

    public static int CopyUtf8PathGetFullPath(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            GetFullUtf8Path(pathPointer, pathLength, basePointer, baseLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetDirectoryNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Encoding.UTF8.GetByteCount(GetDirectoryNameUtf8Path(pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetDirectoryName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            GetDirectoryNameUtf8Path(pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetRelativeLengthUtf8(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return Encoding.UTF8.GetByteCount(GetRelativeUtf8Path(relativeToPointer, relativeToLength, pathPointer, pathLength));
    }

    public static int CopyUtf8PathGetRelative(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            GetRelativeUtf8Path(relativeToPointer, relativeToLength, pathPointer, pathLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8DocumentsLength()
    {
        return Encoding.UTF8.GetByteCount(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
    }

    public static int CopyUtf8Documents(IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8TempPathLength()
    {
        return Encoding.UTF8.GetByteCount(Path.GetTempPath());
    }

    public static int CopyUtf8TempPath(IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(Path.GetTempPath(), destinationPointer, destinationCapacity);
    }

    public static int Utf8UserProfileLength()
    {
        return Encoding.UTF8.GetByteCount(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }

    public static int CopyUtf8UserProfile(IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8CurrentDirectoryLength()
    {
        return Encoding.UTF8.GetByteCount(Environment.CurrentDirectory);
    }

    public static int CopyUtf8CurrentDirectory(IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(Environment.CurrentDirectory, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombine3LengthUtf8(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return Encoding.UTF8.GetByteCount(CombineUtf8Paths(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength));
    }

    public static int CopyUtf8PathCombine3(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            CombineUtf8Paths(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathCombineLengthUtf8(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return Encoding.UTF8.GetByteCount(CombineUtf8Paths(leftPointer, leftLength, rightPointer, rightLength));
    }

    public static int CopyUtf8PathCombine(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            CombineUtf8Paths(leftPointer, leftLength, rightPointer, rightLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathChangeExtensionLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return Encoding.UTF8.GetByteCount(ChangeExtensionUtf8Path(pathPointer, pathLength, extensionPointer, extensionLength));
    }

    public static int CopyUtf8PathChangeExtension(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            ChangeExtensionUtf8Path(pathPointer, pathLength, extensionPointer, extensionLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFileNameWithoutExtensionLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Encoding.UTF8.GetByteCount(Path.GetFileNameWithoutExtension(ReadUtf8String(pathPointer, pathLength)));
    }

    public static int CopyUtf8PathGetFileNameWithoutExtension(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            Path.GetFileNameWithoutExtension(ReadUtf8String(pathPointer, pathLength)),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8StringReplaceLength(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength)
    {
        return Encoding.UTF8.GetByteCount(ReplaceUtf8String(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength));
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
        return WriteUtf8String(
            ReplaceUtf8String(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8PathGetFileNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Encoding.UTF8.GetByteCount(Path.GetFileName(ReadUtf8String(pathPointer, pathLength)));
    }

    public static int CopyUtf8PathGetFileName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return WriteUtf8String(
            Path.GetFileName(ReadUtf8String(pathPointer, pathLength)),
            destinationPointer,
            destinationCapacity);
    }

    public static int Utf8StringContains(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return ReadUtf8String(haystackPointer, haystackLength).Contains(
            ReadUtf8String(needlePointer, needleLength),
            StringComparison.Ordinal)
            ? 1
            : 0;
    }

    public static int Utf8StringIndexOf(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return ReadUtf8String(haystackPointer, haystackLength).IndexOf(
            ReadUtf8String(needlePointer, needleLength),
            StringComparison.Ordinal);
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
        return Path.GetFileName(ReadUtf8String(pathPointer, pathLength)).Length;
    }

    public static void InitializeWtf8PathBuffer(IntPtr destination, IntPtr source, long length)
    {
        var buffer = CopyToUnmanaged(source, length);
        WriteRustBuffer(destination, length, buffer, length);
    }

    public static void AppendPathSegment(IntPtr destination, IntPtr source, long length)
    {
        if (length == 0)
        {
            return;
        }

        var currentCapacity = Marshal.ReadInt64(destination, 0);
        var currentPointer = Marshal.ReadIntPtr(destination, 8);
        var currentLength = Marshal.ReadInt64(destination, 16);
        var needsSeparator = currentLength > 0
            && !IsPathSeparator(Marshal.ReadByte(source, 0));
        var separatorLength = needsSeparator ? 1L : 0L;
        var newLength = checked(currentLength + separatorLength + length);
        var newPointer = currentCapacity == 0 || currentPointer == IntPtr.Zero
            ? Marshal.AllocHGlobal(ToNativeInt(newLength))
            : Marshal.ReAllocHGlobal(currentPointer, ToNativeInt(newLength));

        if (needsSeparator)
        {
            Marshal.WriteByte(newPointer, checked((int)currentLength), (byte)'\\');
        }

        CopyToUnmanaged(source, length, IntPtr.Add(newPointer, checked((int)(currentLength + separatorLength))));
        WriteRustBuffer(destination, newLength, newPointer, newLength);
    }

    public static void ReadFileToRustString(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        var managedPath = ReadUtf8String(pathPointer, pathLength);
        var bytes = File.ReadAllBytes(managedPath);
        var buffer = bytes.Length == 0
            ? IntPtr.Zero
            : Marshal.AllocHGlobal(bytes.Length);

        if (bytes.Length > 0)
        {
            Marshal.Copy(bytes, 0, buffer, bytes.Length);
        }

        WriteRustBuffer(destination, bytes.LongLength, buffer, bytes.LongLength);
    }

    private static string ReadUtf8String(IntPtr pointer, long length)
    {
        if (length == 0)
        {
            return string.Empty;
        }

        var bytes = new byte[checked((int)length)];
        Marshal.Copy(pointer, bytes, 0, bytes.Length);
        return Encoding.UTF8.GetString(bytes);
    }

    private static string GetUtf8FileLine(IntPtr pathPointer, long pathLength, int index)
    {
        var lines = File.ReadAllLines(ReadUtf8String(pathPointer, pathLength));
        if ((uint)index >= (uint)lines.Length)
        {
            throw new IndexOutOfRangeException($"File line index {index} was outside the available range 0..{lines.Length - 1}.");
        }

        return lines[index];
    }

    private static int WriteUtf8String(string value, IntPtr destinationPointer, long destinationCapacity)
    {
        var bytes = Encoding.UTF8.GetBytes(value);

        if (destinationPointer != IntPtr.Zero && destinationCapacity > 0 && bytes.Length > 0)
        {
            var bytesToCopy = Math.Min(bytes.Length, checked((int)destinationCapacity));
            Marshal.Copy(bytes, 0, destinationPointer, bytesToCopy);
        }

        return bytes.Length;
    }

    private static void WriteExceptionOut(IntPtr exceptionOutPointer, int exceptionHandle)
    {
        if (exceptionOutPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(exceptionOutPointer), "Generated binding calls must provide an exception out pointer.");
        }

        Marshal.WriteInt32(exceptionOutPointer, exceptionHandle);
    }

    private static string ReplaceUtf8String(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength)
    {
        return ReadUtf8String(sourcePointer, sourceLength).Replace(
            ReadUtf8String(oldPointer, oldLength),
            ReadUtf8String(newPointer, newLength),
            StringComparison.Ordinal);
    }

    private static string ChangeExtensionUtf8Path(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return Path.ChangeExtension(
            ReadUtf8String(pathPointer, pathLength),
            ReadUtf8String(extensionPointer, extensionLength))
            ?? string.Empty;
    }

    private static string GetDirectoryNameUtf8Path(IntPtr pathPointer, long pathLength)
    {
        return Path.GetDirectoryName(ReadUtf8String(pathPointer, pathLength)) ?? string.Empty;
    }

    private static string GetFullUtf8Path(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return Path.GetFullPath(
            ReadUtf8String(pathPointer, pathLength),
            ReadUtf8String(basePointer, baseLength));
    }

    private static string GetRootUtf8Path(IntPtr pathPointer, long pathLength)
    {
        return Path.GetPathRoot(ReadUtf8String(pathPointer, pathLength)) ?? string.Empty;
    }

    private static string GetRelativeUtf8Path(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return Path.GetRelativePath(
            ReadUtf8String(relativeToPointer, relativeToLength),
            ReadUtf8String(pathPointer, pathLength));
    }

    private static string CombineUtf8Paths(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return Path.Combine(
            ReadUtf8String(leftPointer, leftLength),
            ReadUtf8String(rightPointer, rightLength));
    }

    private static string CombineUtf8Paths(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return Path.Combine(
            ReadUtf8String(firstPointer, firstLength),
            ReadUtf8String(secondPointer, secondLength),
            ReadUtf8String(thirdPointer, thirdLength));
    }

    private static IntPtr CopyToUnmanaged(IntPtr source, long length)
    {
        if (length == 0)
        {
            return IntPtr.Zero;
        }

        var destination = Marshal.AllocHGlobal(ToNativeInt(length));
        CopyToUnmanaged(source, length, destination);
        return destination;
    }

    private static void CopyToUnmanaged(IntPtr source, long length, IntPtr destination)
    {
        if (length == 0)
        {
            return;
        }

        var bytes = new byte[checked((int)length)];
        Marshal.Copy(source, bytes, 0, bytes.Length);
        Marshal.Copy(bytes, 0, destination, bytes.Length);
    }

    private static void WriteRustBuffer(IntPtr destination, long capacity, IntPtr pointer, long length)
    {
        Marshal.WriteInt64(destination, 0, capacity);
        Marshal.WriteIntPtr(destination, 8, pointer);
        Marshal.WriteInt64(destination, 16, length);
    }

    private static bool IsPathSeparator(byte value)
    {
        return value == (byte)'\\' || value == (byte)'/';
    }

    private static IntPtr ToNativeInt(long value)
    {
        return new IntPtr(value);
    }
}