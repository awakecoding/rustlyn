using Rustlyn.Interop;

namespace Rustlyn.Os;

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

    public static int Utf8DocumentsLength()
    {
        return InteropUtf8.GetByteCount(GetSpecialFolder(Environment.SpecialFolder.MyDocuments));
    }

    public static int CopyUtf8Documents(IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(GetSpecialFolder(Environment.SpecialFolder.MyDocuments), destinationPointer, destinationCapacity);
    }

    public static int Utf8TempPathLength()
    {
        return InteropUtf8.GetByteCount(Path.GetTempPath());
    }

    public static int CopyUtf8TempPath(IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(Path.GetTempPath(), destinationPointer, destinationCapacity);
    }

    public static int Utf8UserProfileLength()
    {
        return InteropUtf8.GetByteCount(GetSpecialFolder(Environment.SpecialFolder.UserProfile));
    }

    public static int CopyUtf8UserProfile(IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(GetSpecialFolder(Environment.SpecialFolder.UserProfile), destinationPointer, destinationCapacity);
    }

    public static int Utf8CurrentDirectoryLength()
    {
        return InteropUtf8.GetByteCount(Environment.CurrentDirectory);
    }

    public static int CopyUtf8CurrentDirectory(IntPtr destinationPointer, long destinationCapacity)
    {
        return InteropUtf8.CopyString(Environment.CurrentDirectory, destinationPointer, destinationCapacity);
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

    private static string GetSpecialFolder(Environment.SpecialFolder folder)
    {
        return Environment.GetFolderPath(folder);
    }
}
