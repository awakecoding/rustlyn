using System.Runtime.InteropServices;

namespace RustMcil.Runtime;

public static class MemoryRuntime
{
    public static IntPtr Alloc(IntPtr size)
    {
        return Marshal.AllocHGlobal(size);
    }

    public static IntPtr Realloc(IntPtr pointer, IntPtr newSize)
    {
        return Marshal.ReAllocHGlobal(pointer, newSize);
    }

    public static void Dealloc(IntPtr pointer)
    {
        Marshal.FreeHGlobal(pointer);
    }

    public static void CopyBytesI64(IntPtr destination, IntPtr source, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Copy length must be non-negative.");
        }

        for (var index = 0L; index < length; index++)
        {
            Marshal.WriteByte(destination, checked((int)index), Marshal.ReadByte(source, checked((int)index)));
        }
    }

    public static void SetBytesI64(IntPtr destination, byte value, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Set length must be non-negative.");
        }

        for (var index = 0L; index < length; index++)
        {
            Marshal.WriteByte(destination, checked((int)index), value);
        }
    }
}
