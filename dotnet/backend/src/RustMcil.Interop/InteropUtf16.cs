using System.Runtime.InteropServices;

namespace RustMcil.Interop;

public static class InteropUtf16
{
    public static int GetCharCount(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Length;
    }

    public static int CopyChars(string value, IntPtr destinationPointer, long destinationCapacityInChars)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (destinationCapacityInChars < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(destinationCapacityInChars), "Destination capacity must be non-negative.");
        }

        if (destinationPointer != IntPtr.Zero && destinationCapacityInChars > 0 && value.Length > 0)
        {
            var charsToCopy = (int)Math.Min(value.Length, Math.Min(destinationCapacityInChars, int.MaxValue));
            unsafe
            {
                fixed (char* source = value)
                {
                    Buffer.MemoryCopy(source, (void*)destinationPointer, charsToCopy * 2L, charsToCopy * 2L);
                }
            }
        }

        return value.Length;
    }

    public static string ReadString(IntPtr pointer, long lengthInChars)
    {
        if (lengthInChars < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lengthInChars), "UTF-16 string lengths must be non-negative.");
        }

        if (lengthInChars == 0)
        {
            return string.Empty;
        }

        if (pointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(pointer), "A non-empty UTF-16 string cannot be read from a null pointer.");
        }

        if (lengthInChars > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(lengthInChars), "UTF-16 strings larger than Int32.MaxValue chars are not supported.");
        }

        unsafe
        {
            return new string((char*)pointer, 0, (int)lengthInChars);
        }
    }
}
