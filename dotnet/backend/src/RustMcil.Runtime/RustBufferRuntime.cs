using System.Runtime.InteropServices;

namespace RustMcil.Runtime;

public static class RustBufferRuntime
{
    public static void InitializeBufferFromBytes(IntPtr destination, IntPtr source, long length)
    {
        var buffer = CopyToUnmanaged(source, length);
        WriteBuffer(destination, length, buffer, length);
    }

    public static IntPtr CopyToUnmanaged(IntPtr source, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Copy length must be non-negative.");
        }

        if (length == 0)
        {
            return IntPtr.Zero;
        }

        var destination = MemoryRuntime.Alloc(new IntPtr(checked((int)length)));
        CopyToUnmanaged(source, length, destination);
        return destination;
    }

    public static void CopyToUnmanaged(IntPtr source, long length, IntPtr destination)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Copy length must be non-negative.");
        }

        if (length == 0)
        {
            return;
        }

        var bytes = new byte[checked((int)length)];
        Marshal.Copy(source, bytes, 0, bytes.Length);
        Marshal.Copy(bytes, 0, destination, bytes.Length);
    }

    public static void WriteBuffer(IntPtr destination, long capacity, IntPtr pointer, long length)
    {
        Marshal.WriteInt64(destination, 0, capacity);
        Marshal.WriteIntPtr(destination, 8, pointer);
        Marshal.WriteInt64(destination, 16, length);
    }
}
