using System.Runtime.InteropServices;
using System.Text;

namespace RustMcil.Interop;

public static class InteropUtf8
{
    public static string ReadString(IntPtr pointer, long length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "UTF-8 string lengths must be non-negative.");
        }

        if (length == 0)
        {
            return string.Empty;
        }

        if (pointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(pointer), "A non-empty UTF-8 string cannot be read from a null pointer.");
        }

        if (length > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "UTF-8 strings larger than Int32.MaxValue bytes are not supported yet.");
        }

        var bytes = new byte[(int)length];
        Marshal.Copy(pointer, bytes, 0, bytes.Length);
        return Encoding.UTF8.GetString(bytes);
    }

    public static int GetByteCount(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Encoding.UTF8.GetByteCount(value);
    }

    public static int CopyString(string value, IntPtr destinationPointer, long destinationCapacity)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (destinationCapacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(destinationCapacity), "Destination capacity must be non-negative.");
        }

        var bytes = Encoding.UTF8.GetBytes(value);
        if (destinationPointer != IntPtr.Zero && destinationCapacity > 0 && bytes.Length > 0)
        {
            var bytesToCopy = Math.Min(bytes.Length, checked((int)Math.Min(destinationCapacity, int.MaxValue)));
            Marshal.Copy(bytes, 0, destinationPointer, bytesToCopy);
        }

        return bytes.Length;
    }
}