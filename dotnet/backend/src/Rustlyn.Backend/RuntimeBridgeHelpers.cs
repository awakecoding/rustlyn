using System.Runtime.InteropServices;
using System.Globalization;
using System.Numerics;

namespace Rustlyn.Backend;

public static partial class RuntimeBridgeHelpers
{
    private const int RustFormatArgumentSize = 16;
    private static readonly object DefaultI32FormattersLock = new();
    private static readonly HashSet<IntPtr> DefaultI32Formatters = [];

    public static int CommandLineArgCount()
    {
        return Rustlyn.Os.HostEnvironment.CommandLineArgCount();
    }

    public static int IsWindows()
    {
        return OperatingSystem.IsWindows() ? 1 : 0;
    }

    public static int DirectorySeparatorChar()
    {
        return Path.DirectorySeparatorChar;
    }

    public static int PathSeparatorChar()
    {
        return Path.PathSeparator;
    }

    public static int NewlineLength()
    {
        return Environment.NewLine.Length;
    }

    public static int PopCountU32(uint value)
    {
        return BitOperations.PopCount(value);
    }

    public static int CompareBytesI64(IntPtr left, IntPtr right, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Compare length must be non-negative.");
        }

        for (var index = 0L; index < length; index++)
        {
            var diff = Marshal.ReadByte(left, checked((int)index)) - Marshal.ReadByte(right, checked((int)index));
            if (diff != 0)
            {
                return diff;
            }
        }

        return 0;
    }

    public static int MathMaxI32(int left, int right)
    {
        return Math.Max(left, right);
    }

    public static int MathMinI32(int left, int right)
    {
        return Math.Min(left, right);
    }

    public static int Utf8CommandLineArgLength(int index)
    {
        return Rustlyn.Os.HostEnvironment.Utf8CommandLineArgLength(index);
    }

    public static int CopyUtf8CommandLineArg(int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8CommandLineArg(index, destinationPointer, destinationCapacity);
    }

    public static void ConsoleWriteLineUtf8(IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WriteLineUtf8(valuePointer, valueLength);
    }

    public static void ConsoleWritePrefixedLineUtf8(IntPtr pathPointer, long pathLength, int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WritePrefixedLineUtf8(pathPointer, pathLength, lineNumber, valuePointer, valueLength);
    }

    public static void ConsoleWritePathLineUtf8(IntPtr pathPointer, long pathLength, IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WritePathLineUtf8(pathPointer, pathLength, valuePointer, valueLength);
    }

    public static void ConsoleWriteNumberedLineUtf8(int lineNumber, IntPtr valuePointer, long valueLength)
    {
        Rustlyn.Os.HostConsole.WriteNumberedLineUtf8(lineNumber, valuePointer, valueLength);
    }

    public static void ConsoleWriteI32(int value)
    {
        Rustlyn.Os.HostConsole.WriteI32(value);
    }

    public static void ConsoleWritePathCountUtf8(IntPtr pathPointer, long pathLength, int value)
    {
        Rustlyn.Os.HostConsole.WritePathCountUtf8(pathPointer, pathLength, value);
    }

    public static void StdIoPrint(IntPtr argumentsPointer)
    {
        Console.Out.Write(ReadFormatLiteralPieces(argumentsPointer));
    }

    public static void StdIoEPrint(IntPtr argumentsPointer)
    {
        Console.Error.Write(ReadFormatLiteralPieces(argumentsPointer));
    }

    public static void RegisterDefaultI32Formatter(IntPtr formatterPointer)
    {
        if (formatterPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("The generated default i32 formatter pointer cannot be zero.");
        }

        lock (DefaultI32FormattersLock)
        {
            DefaultI32Formatters.Add(formatterPointer);
        }
    }

    public static IntPtr StdIoStdout()
    {
        return new IntPtr(1);
    }

    public static IntPtr StdIoStdoutWriteAll(IntPtr stdoutPointer, IntPtr valuePointer, long valueLength)
    {
        Console.Out.Write(Rustlyn.Interop.InteropUtf8.ReadString(valuePointer, valueLength));
        return IntPtr.Zero;
    }

    public static IntPtr StdIoStderrWriteAll(IntPtr stderrPointer, IntPtr valuePointer, long valueLength)
    {
        Console.Error.Write(Rustlyn.Interop.InteropUtf8.ReadString(valuePointer, valueLength));
        return IntPtr.Zero;
    }

    public static IntPtr StdIoStdoutFlush(IntPtr stdoutPointer)
    {
        Console.Out.Flush();
        return IntPtr.Zero;
    }

    public static void StdTimeInstantNow(IntPtr resultPointer)
    {
        Marshal.WriteInt64(resultPointer, 0, 0);
        Marshal.WriteInt32(resultPointer, 8, 0);
    }

    public static void StdTimeInstantElapsed(IntPtr resultPointer, IntPtr instantPointer)
    {
        Marshal.WriteInt64(resultPointer, 0, 0);
        Marshal.WriteInt32(resultPointer, 8, 0);
    }

    public static void StdEnvCurrentDir(IntPtr destination)
    {
        WriteRustBufferFromString(destination, Directory.GetCurrentDirectory());
    }

    public static void StdEnvTempDir(IntPtr destination)
    {
        WriteRustBufferFromString(destination, Path.GetTempPath());
    }

    public static void StdEnvVar(IntPtr destination, IntPtr namePointer, long nameLength)
    {
        var name = Rustlyn.Interop.InteropUtf8.ReadString(namePointer, nameLength);
        var value = Environment.GetEnvironmentVariable(name);
        if (value is null)
        {
            Marshal.WriteInt64(destination, 0, 0);
            return;
        }

        Marshal.WriteInt64(destination, 0, -9223372036854775807L);
        WriteRustBufferFromString(IntPtr.Add(destination, 8), value);
    }

    public static void StdPathFileName(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var trimmed = value.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var fileName = Path.GetFileName(trimmed);
        WriteRustSliceFromString(destination, fileName);
    }

    public static void StdPathFileStem(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var trimmed = value.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var fileStem = Path.GetFileNameWithoutExtension(trimmed);
        WriteRustSliceFromString(destination, fileStem);
    }

    public static void StdPathExtension(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var extension = Path.GetExtension(value);
        if (!string.IsNullOrEmpty(extension) && extension[0] == '.')
        {
            extension = extension[1..];
        }

        WriteRustSliceFromString(destination, extension);
    }

    public static void StdPathParent(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            WriteRustSliceFromString(destination, null);
            return;
        }

        var trimmed = value.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var parent = Path.GetDirectoryName(trimmed);
        WriteRustSliceFromString(destination, parent);
    }

    public static void StdPathComponents(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        for (var offset = 0; offset < 64; offset += sizeof(long))
        {
            Marshal.WriteInt64(destination, offset, 0);
        }

        Marshal.WriteIntPtr(destination, pathPointer);
        Marshal.WriteInt64(destination, 8, pathLength);
    }

    public static int StdPathIsAbsolute(IntPtr pathPointer, long pathLength)
    {
        if (!TryReadUtf8Path(pathPointer, pathLength, out var value))
        {
            return 0;
        }

        return IsRootedPath(value) ? 1 : 0;
    }

    public static int StdPathComponentsEq(IntPtr leftPointer, IntPtr rightPointer)
    {
        var left = ReadComponentPath(leftPointer);
        var right = ReadComponentPath(rightPointer);
        return string.Equals(NormalizePathForComparison(left), NormalizePathForComparison(right), StringComparison.Ordinal) ? 1 : 0;
    }

    public static int StdPathEq(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        var left = TryReadUtf8Path(leftPointer, leftLength, out var leftValue) ? leftValue : string.Empty;
        var right = TryReadUtf8Path(rightPointer, rightLength, out var rightValue) ? rightValue : string.Empty;
        return string.Equals(NormalizePathForComparison(left), NormalizePathForComparison(right), StringComparison.Ordinal) ? 1 : 0;
    }

    public static int StdPathStartsWith(IntPtr pathPointer, long pathLength, IntPtr prefixPointer, long prefixLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? NormalizePathForComparison(pathValue) : string.Empty;
        var prefix = TryReadUtf8Path(prefixPointer, prefixLength, out var prefixValue) ? NormalizePathForComparison(prefixValue) : string.Empty;
        return path.StartsWith(prefix, StringComparison.Ordinal) ? 1 : 0;
    }

    public static int StdPathEndsWith(IntPtr pathPointer, long pathLength, IntPtr suffixPointer, long suffixLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? NormalizePathForComparison(pathValue) : string.Empty;
        var suffix = TryReadUtf8Path(suffixPointer, suffixLength, out var suffixValue) ? NormalizePathForComparison(suffixValue) : string.Empty;
        return path.EndsWith(suffix, StringComparison.Ordinal) ? 1 : 0;
    }

    public static void StdPathJoin(IntPtr destination, IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        var left = TryReadUtf8Path(leftPointer, leftLength, out var leftValue) ? leftValue : string.Empty;
        var right = TryReadUtf8Path(rightPointer, rightLength, out var rightValue) ? rightValue : string.Empty;
        WriteRustBufferFromString(destination, CombinePathStrings(left, right));
    }

    public static void StdPathWithExtension(IntPtr destination, IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? pathValue : string.Empty;
        var extension = TryReadUtf8Path(extensionPointer, extensionLength, out var extensionValue) ? extensionValue : string.Empty;
        WriteRustBufferFromString(destination, ChangePathExtension(path, extension));
    }

    public static void StdPathWithFileName(IntPtr destination, IntPtr pathPointer, long pathLength, IntPtr fileNamePointer, long fileNameLength)
    {
        var path = TryReadUtf8Path(pathPointer, pathLength, out var pathValue) ? pathValue : string.Empty;
        var fileName = TryReadUtf8Path(fileNamePointer, fileNameLength, out var fileNameValue) ? fileNameValue : string.Empty;
        var parent = ParentPathString(path);
        WriteRustBufferFromString(destination, string.IsNullOrEmpty(parent) ? fileName : CombinePathStrings(parent, fileName));
    }

    public static long StdPathComponentCount(IntPtr componentsPointer)
    {
        var value = ReadComponentPath(componentsPointer);
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }

        var trimmed = value.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var count = trimmed.Length == 0
            ? 0
            : trimmed.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).LongLength;
        return IsRootedPath(value) ? count + 1 : count;
    }

    private static string ReadFormatLiteralPieces(IntPtr argumentsPointer)
    {
        if (argumentsPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(argumentsPointer));
        }

        var piecesPointer = Marshal.ReadIntPtr(argumentsPointer);
        var piecesLength = Marshal.ReadInt64(argumentsPointer, 8);
        var argsPointer = Marshal.ReadIntPtr(argumentsPointer, 16);
        var argsLength = Marshal.ReadInt64(argumentsPointer, 24);
        var formatSpecsPointer = Marshal.ReadIntPtr(argumentsPointer, 32);
        var formatSpecsLength = Marshal.ReadInt64(argumentsPointer, 40);
        if (formatSpecsPointer != IntPtr.Zero || formatSpecsLength != 0)
        {
            throw new NotSupportedException("std::fmt arguments with non-default formatting specs are not supported by the console bridge yet.");
        }

        if (piecesLength == 0)
        {
            return string.Empty;
        }

        if (piecesPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("std::fmt arguments contained a null pieces pointer.");
        }

        if (piecesLength < 0 || piecesLength > 256)
        {
            throw new NotSupportedException($"std::fmt pieces length {piecesLength} is outside the supported bridge range.");
        }

        if (argsLength < 0 || argsLength > 256)
        {
            throw new NotSupportedException($"std::fmt runtime argument length {argsLength} is outside the supported bridge range.");
        }

        if (argsLength != 0 && argsPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("std::fmt arguments contained a null runtime argument pointer.");
        }

        if (argsLength > 0 && piecesLength != argsLength && piecesLength != argsLength + 1)
        {
            throw new NotSupportedException($"std::fmt pieces length {piecesLength} is not compatible with runtime argument length {argsLength}.");
        }

        var builder = new System.Text.StringBuilder();
        for (var index = 0; index < piecesLength; index++)
        {
            var piecePointer = Marshal.ReadIntPtr(piecesPointer, checked((int)(index * 16)));
            var pieceLength = Marshal.ReadInt64(piecesPointer, checked((int)(index * 16 + 8)));
            builder.Append(Rustlyn.Interop.InteropUtf8.ReadString(piecePointer, pieceLength));
            if (index < argsLength)
            {
                builder.Append(ReadDefaultFormatArgument(argsPointer, index));
            }
        }

        return builder.ToString();
    }

    private static string ReadDefaultFormatArgument(IntPtr argsPointer, long index)
    {
        var offset = checked((int)(index * RustFormatArgumentSize));
        var valuePointer = Marshal.ReadIntPtr(argsPointer, offset);
        var formatterPointer = Marshal.ReadIntPtr(argsPointer, offset + IntPtr.Size);
        if (valuePointer == IntPtr.Zero)
        {
            throw new InvalidOperationException($"std::fmt runtime argument {index.ToString(CultureInfo.InvariantCulture)} contained a null value pointer.");
        }

        if (formatterPointer == IntPtr.Zero)
        {
            throw new InvalidOperationException("std::fmt runtime argument contained a null formatter pointer.");
        }

        if (!IsDefaultI32Formatter(formatterPointer))
        {
            throw new NotSupportedException($"std::fmt runtime formatter 0x{formatterPointer.ToInt64().ToString("x", CultureInfo.InvariantCulture)} is not supported by the console bridge yet.");
        }

        return Marshal.ReadInt32(valuePointer).ToString(CultureInfo.InvariantCulture);
    }

    private static bool IsDefaultI32Formatter(IntPtr formatterPointer)
    {
        lock (DefaultI32FormattersLock)
        {
            return DefaultI32Formatters.Contains(formatterPointer);
        }
    }

    private static void WriteRustBufferFromString(IntPtr destination, string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var buffer = IntPtr.Zero;
        if (bytes.Length > 0)
        {
            buffer = Rustlyn.Runtime.MemoryRuntime.Alloc(new IntPtr(bytes.Length));
            Marshal.Copy(bytes, 0, buffer, bytes.Length);
        }

        Rustlyn.Runtime.RustBufferRuntime.WriteBuffer(destination, bytes.LongLength, buffer, bytes.LongLength);
    }

    private static void WriteRustSliceFromString(IntPtr destination, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Marshal.WriteIntPtr(destination, IntPtr.Zero);
            Marshal.WriteInt64(destination, 8, 0);
            return;
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var buffer = Rustlyn.Runtime.MemoryRuntime.Alloc(new IntPtr(bytes.Length));
        Marshal.Copy(bytes, 0, buffer, bytes.Length);
        Marshal.WriteIntPtr(destination, buffer);
        Marshal.WriteInt64(destination, 8, bytes.LongLength);
    }

    private static bool TryReadUtf8Path(IntPtr pathPointer, long pathLength, out string value)
    {
        if (pathPointer == IntPtr.Zero || pathLength <= 0)
        {
            value = string.Empty;
            return false;
        }

        value = Rustlyn.Interop.InteropUtf8.ReadString(pathPointer, pathLength);
        return value.Length > 0;
    }

    private static string ReadComponentPath(IntPtr componentsPointer)
    {
        if (componentsPointer == IntPtr.Zero)
        {
            return string.Empty;
        }

        var pathPointer = Marshal.ReadIntPtr(componentsPointer);
        var pathLength = Marshal.ReadInt64(componentsPointer, 8);
        return TryReadUtf8Path(pathPointer, pathLength, out var value) ? value : string.Empty;
    }

    private static string NormalizePathForComparison(string value)
    {
        return value.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            .TrimEnd(Path.DirectorySeparatorChar);
    }

    private static bool IsRootedPath(string value)
    {
        return value.StartsWith(Path.DirectorySeparatorChar)
            || value.StartsWith(Path.AltDirectorySeparatorChar)
            || Path.IsPathRooted(value);
    }

    private static string CombinePathStrings(string left, string right)
    {
        if (string.IsNullOrEmpty(left))
        {
            return right;
        }

        if (string.IsNullOrEmpty(right))
        {
            return left;
        }

        if (IsRootedPath(right))
        {
            return right;
        }

        var separator = PreferredSeparator(left);
        return left.TrimEnd('\\', '/') + separator + right.TrimStart('\\', '/');
    }

    private static string ChangePathExtension(string path, string extension)
    {
        var separatorIndex = LastSeparatorIndex(path);
        var dotIndex = path.LastIndexOf('.');
        var stemEnd = dotIndex > separatorIndex ? dotIndex : path.Length;
        if (string.IsNullOrEmpty(extension))
        {
            return path[..stemEnd];
        }

        return path[..stemEnd] + "." + extension.TrimStart('.');
    }

    private static string ParentPathString(string path)
    {
        var trimmed = path.TrimEnd('\\', '/');
        var separatorIndex = LastSeparatorIndex(trimmed);
        if (separatorIndex < 0)
        {
            return string.Empty;
        }

        if (separatorIndex == 0)
        {
            return trimmed[..1];
        }

        return trimmed[..separatorIndex];
    }

    private static int LastSeparatorIndex(string value)
    {
        return Math.Max(value.LastIndexOf('\\'), value.LastIndexOf('/'));
    }

    private static char PreferredSeparator(string value)
    {
        return value.Contains('/') && !value.Contains('\\') ? '/' : Path.DirectorySeparatorChar;
    }

    public static int Utf8ReadAllLinesCount(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostFileSystem.Utf8ReadAllLinesCount(pathPointer, pathLength);
    }

    public static int Utf8ReadAllLinesLineLength(IntPtr pathPointer, long pathLength, int index)
    {
        return Rustlyn.Os.HostFileSystem.Utf8ReadAllLinesLineLength(pathPointer, pathLength, index);
    }

    public static int CopyUtf8ReadAllLinesLine(IntPtr pathPointer, long pathLength, int index, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostFileSystem.CopyUtf8ReadAllLinesLine(pathPointer, pathLength, index, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRootLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetRootLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetRoot(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetRoot(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFullPathLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFullPathLengthUtf8(pathPointer, pathLength, basePointer, baseLength);
    }

    public static int CopyUtf8PathGetFullPath(IntPtr pathPointer, long pathLength, IntPtr basePointer, long baseLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetFullPath(pathPointer, pathLength, basePointer, baseLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetDirectoryNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetDirectoryNameLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetDirectoryName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetDirectoryName(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetRelativeLengthUtf8(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetRelativeLengthUtf8(relativeToPointer, relativeToLength, pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetRelative(IntPtr relativeToPointer, long relativeToLength, IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetRelative(relativeToPointer, relativeToLength, pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8DocumentsLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8DocumentsLength();
    }

    public static int CopyUtf8Documents(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8Documents(destinationPointer, destinationCapacity);
    }

    public static int Utf8TempPathLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8TempPathLength();
    }

    public static int CopyUtf8TempPath(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8TempPath(destinationPointer, destinationCapacity);
    }

    public static int Utf8UserProfileLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8UserProfileLength();
    }

    public static int CopyUtf8UserProfile(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8UserProfile(destinationPointer, destinationCapacity);
    }

    public static int Utf8CurrentDirectoryLength()
    {
        return Rustlyn.Os.HostEnvironment.Utf8CurrentDirectoryLength();
    }

    public static int CopyUtf8CurrentDirectory(IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostEnvironment.CopyUtf8CurrentDirectory(destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombine3LengthUtf8(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathCombine3LengthUtf8(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength);
    }

    public static int CopyUtf8PathCombine3(IntPtr firstPointer, long firstLength, IntPtr secondPointer, long secondLength, IntPtr thirdPointer, long thirdLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathCombine3(firstPointer, firstLength, secondPointer, secondLength, thirdPointer, thirdLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathCombineLengthUtf8(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathCombineLengthUtf8(leftPointer, leftLength, rightPointer, rightLength);
    }

    public static int CopyUtf8PathCombine(IntPtr leftPointer, long leftLength, IntPtr rightPointer, long rightLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathCombine(leftPointer, leftLength, rightPointer, rightLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathChangeExtensionLengthUtf8(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathChangeExtensionLengthUtf8(pathPointer, pathLength, extensionPointer, extensionLength);
    }

    public static int CopyUtf8PathChangeExtension(IntPtr pathPointer, long pathLength, IntPtr extensionPointer, long extensionLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathChangeExtension(pathPointer, pathLength, extensionPointer, extensionLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFileNameWithoutExtensionLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFileNameWithoutExtensionLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetFileNameWithoutExtension(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetFileNameWithoutExtension(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8StringReplaceLength(
        IntPtr sourcePointer,
        long sourceLength,
        IntPtr oldPointer,
        long oldLength,
        IntPtr newPointer,
        long newLength)
    {
        return Rustlyn.Interop.InteropUtf8.ReplaceByteCount(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength);
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
        return Rustlyn.Interop.InteropUtf8.CopyReplace(sourcePointer, sourceLength, oldPointer, oldLength, newPointer, newLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8PathGetFileNameLengthUtf8(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFileNameLengthUtf8(pathPointer, pathLength);
    }

    public static int CopyUtf8PathGetFileName(IntPtr pathPointer, long pathLength, IntPtr destinationPointer, long destinationCapacity)
    {
        return Rustlyn.Os.HostPath.CopyUtf8PathGetFileName(pathPointer, pathLength, destinationPointer, destinationCapacity);
    }

    public static int Utf8StringContains(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return Rustlyn.Interop.InteropUtf8.ContainsOrdinal(haystackPointer, haystackLength, needlePointer, needleLength);
    }

    public static int Utf8StringIndexOf(IntPtr haystackPointer, long haystackLength, IntPtr needlePointer, long needleLength)
    {
        return Rustlyn.Interop.InteropUtf8.IndexOfOrdinal(haystackPointer, haystackLength, needlePointer, needleLength);
    }

    public static int RemEuclidI32(int left, int right)
    {
        return Rustlyn.Runtime.NumericRuntime.RemEuclidI32(left, right);
    }

    public static long RemEuclidI64(long left, long right)
    {
        return Rustlyn.Runtime.NumericRuntime.RemEuclidI64(left, right);
    }

    public static int Utf8PathGetFileNameLength(IntPtr pathPointer, long pathLength)
    {
        return Rustlyn.Os.HostPath.Utf8PathGetFileNameLength(pathPointer, pathLength);
    }

    public static void InitializeWtf8PathBuffer(IntPtr destination, IntPtr source, long length)
    {
        Rustlyn.Runtime.RustBufferRuntime.InitializeBufferFromBytes(destination, source, length);
    }

    public static void AppendPathSegment(IntPtr destination, IntPtr source, long length)
    {
        Rustlyn.Os.HostPath.AppendPathSegment(destination, source, length);
    }

    public static void ReadFileToRustString(IntPtr destination, IntPtr pathPointer, long pathLength)
    {
        Rustlyn.Os.HostFileSystem.ReadFileToRustString(destination, pathPointer, pathLength);
    }

    public static long MemchrAligned(byte needle, IntPtr source, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Search length must be non-negative.");
        }

        for (var index = 0L; index < length; index++)
        {
            if (System.Runtime.InteropServices.Marshal.ReadByte(source, checked((int)index)) == needle)
            {
                return (index << 32) | 1L;
            }
        }

        return 0;
    }

    public static int BindgenTaskStatus(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskStatus(taskHandle);
    }

    public static int BindgenTaskExceptionHandle(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskExceptionHandle(taskHandle);
    }

    public static int BindgenTaskRelease(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.ReleaseTask(taskHandle) ? 1 : 0;
    }

    public static int BindgenTaskGetI32Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskInt32Result(taskHandle);
    }

    public static long BindgenTaskGetI64Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskInt64Result(taskHandle);
    }

    public static float BindgenTaskGetF32Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskSingleResult(taskHandle);
    }

    public static double BindgenTaskGetF64Result(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskDoubleResult(taskHandle);
    }

    public static int BindgenTaskGetBoolResult(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskBooleanResult(taskHandle);
    }

    public static int BindgenTaskGetObjectResultHandle(int taskHandle)
    {
        return Rustlyn.Interop.ManagedInteropRuntime.GetTaskObjectResultHandle(taskHandle);
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
