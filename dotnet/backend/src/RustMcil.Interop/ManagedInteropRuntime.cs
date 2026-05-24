namespace RustMcil.Interop;

public static class ManagedInteropRuntime
{
    private static readonly ManagedHandleStore Store = new();

    public static int HandleCount => Store.Count;

    public static int ObjectHandleCount => Store.Snapshot.ObjectCount;

    public static int ExceptionHandleCount => Store.Snapshot.ExceptionCount;

    public static ManagedHandleSnapshot SnapshotHandles()
        => Store.Snapshot;

    public static ManagedObjectHandle AddObject(object value)
        => Store.AddObject(value);

    public static int AddObjectHandle(object value)
        => AddObject(value).Value;

    public static ManagedExceptionHandle AddException(Exception exception)
        => Store.AddException(exception);

    public static int AddExceptionHandle(Exception exception)
        => AddException(exception).Value;

    public static T GetObject<T>(ManagedObjectHandle handle)
        => Store.GetObject<T>(handle);

    public static T GetObject<T>(int handle)
        => Store.GetObject<T>(handle);

    public static Exception GetException(ManagedExceptionHandle handle)
        => Store.GetException(handle);

    public static Exception GetException(int handle)
        => Store.GetException(handle);

    public static bool ReleaseObject(ManagedObjectHandle handle)
        => Store.Release(handle);

    public static bool ReleaseObject(int handle)
        => Store.Release(handle, ManagedHandleKind.Object);

    public static bool ReleaseException(ManagedExceptionHandle handle)
        => Store.Release(handle);

    public static bool ReleaseException(int handle)
        => Store.Release(handle, ManagedHandleKind.Exception);

    public static string GetExceptionTypeName(ManagedExceptionHandle handle)
    {
        var exceptionType = GetException(handle).GetType();
        return exceptionType.FullName ?? exceptionType.Name;
    }

    public static string GetExceptionTypeName(int handle)
        => GetExceptionTypeName(new ManagedExceptionHandle(handle));

    public static string GetExceptionMessage(ManagedExceptionHandle handle)
        => GetException(handle).Message;

    public static string GetExceptionMessage(int handle)
        => GetExceptionMessage(new ManagedExceptionHandle(handle));

    public static int GetExceptionTypeNameUtf8Length(ManagedExceptionHandle handle)
        => InteropUtf8.GetByteCount(GetExceptionTypeName(handle));

    public static int GetExceptionTypeNameUtf8Length(int handle)
        => GetExceptionTypeNameUtf8Length(new ManagedExceptionHandle(handle));

    public static int CopyExceptionTypeNameUtf8(ManagedExceptionHandle handle, IntPtr destinationPointer, long destinationCapacity)
        => InteropUtf8.CopyString(GetExceptionTypeName(handle), destinationPointer, destinationCapacity);

    public static int CopyExceptionTypeNameUtf8(int handle, IntPtr destinationPointer, long destinationCapacity)
        => CopyExceptionTypeNameUtf8(new ManagedExceptionHandle(handle), destinationPointer, destinationCapacity);

    public static int GetExceptionMessageUtf8Length(ManagedExceptionHandle handle)
        => InteropUtf8.GetByteCount(GetExceptionMessage(handle));

    public static int GetExceptionMessageUtf8Length(int handle)
        => GetExceptionMessageUtf8Length(new ManagedExceptionHandle(handle));

    public static int CopyExceptionMessageUtf8(ManagedExceptionHandle handle, IntPtr destinationPointer, long destinationCapacity)
        => InteropUtf8.CopyString(GetExceptionMessage(handle), destinationPointer, destinationCapacity);

    public static int CopyExceptionMessageUtf8(int handle, IntPtr destinationPointer, long destinationCapacity)
        => CopyExceptionMessageUtf8(new ManagedExceptionHandle(handle), destinationPointer, destinationCapacity);

    public static int GetObjectUtf16CharCount(int handle)
    {
        var value = Store.GetObject<string>(handle);
        return InteropUtf16.GetCharCount(value);
    }

    public static int CopyObjectUtf16(int handle, IntPtr destinationPointer, long destinationCapacityInChars)
    {
        var value = Store.GetObject<string>(handle);
        return InteropUtf16.CopyChars(value, destinationPointer, destinationCapacityInChars);
    }

    public static int[] CreateInt32Array(int first, int second, int third)
        => [first, second, third];

    public static byte[] CreateByteArray(int first, int second, int third)
        => [checked((byte)first), checked((byte)second), checked((byte)third)];

    public static string[] CreateStringArray(string first, string second, string third)
        => [first, second, third];

    public static int CopyInt32Array(int[] values, IntPtr destinationPointer, long destinationCapacity)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentOutOfRangeException.ThrowIfNegative(destinationCapacity);
        if (destinationCapacity > 0 && destinationPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(destinationPointer));
        }

        var count = Math.Min(values.Length, checked((int)destinationCapacity));
        for (var index = 0; index < count; index++)
        {
            System.Runtime.InteropServices.Marshal.WriteInt32(destinationPointer, index * sizeof(int), values[index]);
        }

        return values.Length;
    }

    public static int CopyByteArray(byte[] values, IntPtr destinationPointer, long destinationCapacity)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentOutOfRangeException.ThrowIfNegative(destinationCapacity);
        if (destinationCapacity > 0 && destinationPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(destinationPointer));
        }

        var count = Math.Min(values.Length, checked((int)destinationCapacity));
        if (count > 0)
        {
            System.Runtime.InteropServices.Marshal.Copy(values, 0, destinationPointer, count);
        }

        return values.Length;
    }

    public static void Clear()
        => Store.Clear();
}
