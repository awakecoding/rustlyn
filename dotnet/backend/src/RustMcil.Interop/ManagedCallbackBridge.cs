namespace RustMcil.Interop;

public static class ManagedCallbackBridge
{
    public static unsafe int InvokeI32(IntPtr callbackPointer, int value)
    {
        if (callbackPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(callbackPointer));
        }

        var callback = (delegate* managed<int, int>)callbackPointer.ToPointer();
        return callback(value);
    }

    public static unsafe int InvokeI32I32(IntPtr callbackPointer, int left, int right)
    {
        if (callbackPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(callbackPointer));
        }

        var callback = (delegate* managed<int, int, int>)callbackPointer.ToPointer();
        return callback(left, right);
    }

    public static unsafe object InvokeObjectHandleTransform(IntPtr callbackPointer, int objectHandle)
    {
        if (callbackPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(callbackPointer));
        }

        _ = ManagedInteropRuntime.GetObject<object>(objectHandle);
        var callback = (delegate* managed<int, int>)callbackPointer.ToPointer();
        var resultHandle = callback(objectHandle);
        if (resultHandle == objectHandle)
        {
            throw new InvalidOperationException("Object-handle callbacks must return a newly-owned managed object handle, not the borrowed input handle.");
        }

        var result = ManagedInteropRuntime.GetObject<object>(resultHandle);
        if (!ManagedInteropRuntime.ReleaseObject(resultHandle))
        {
            throw new InvalidOperationException($"Object-handle callback result handle {resultHandle} could not be released.");
        }

        return result;
    }
}
