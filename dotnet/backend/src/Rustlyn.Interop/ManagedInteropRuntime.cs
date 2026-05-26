namespace Rustlyn.Interop;

public static class ManagedInteropRuntime
{
    private static readonly ManagedHandleStore Store = new();

    public static int HandleCount => Store.Count;

    public static int ObjectHandleCount => Store.Snapshot.ObjectCount;

    public static int ExceptionHandleCount => Store.Snapshot.ExceptionCount;

    public static int TaskHandleCount => Store.Snapshot.TaskCount;

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

    public static ManagedTaskHandle AddTask(Task task)
        => Store.AddTask(task);

    public static int AddTaskHandle(Task task)
        => AddTask(task).Value;

    public static int AddValueTaskHandle(ValueTask task)
        => AddTaskHandle(task.AsTask());

    public static int AddValueTaskHandle<T>(ValueTask<T> task)
        => AddTaskHandle(task.AsTask());

    public static T GetObject<T>(ManagedObjectHandle handle)
        => Store.GetObject<T>(handle);

    public static T GetObject<T>(int handle)
        => Store.GetObject<T>(handle);

    public static Exception GetException(ManagedExceptionHandle handle)
        => Store.GetException(handle);

    public static Exception GetException(int handle)
        => Store.GetException(handle);

    public static Task GetTask(ManagedTaskHandle handle)
        => Store.GetTask(handle);

    public static Task GetTask(int handle)
        => Store.GetTask(handle);

    public static bool ReleaseObject(ManagedObjectHandle handle)
        => Store.Release(handle);

    public static bool ReleaseObject(int handle)
        => Store.Release(handle, ManagedHandleKind.Object);

    public static bool ReleaseException(ManagedExceptionHandle handle)
        => Store.Release(handle);

    public static bool ReleaseException(int handle)
        => Store.Release(handle, ManagedHandleKind.Exception);

    public static bool ReleaseTask(ManagedTaskHandle handle)
        => Store.Release(handle);

    public static bool ReleaseTask(int handle)
        => Store.Release(handle, ManagedHandleKind.Task);

    public static ManagedTaskStatus GetTaskStatus(ManagedTaskHandle handle)
        => GetTaskStatus(Store.GetTask(handle));

    public static int GetTaskStatus(int handle)
        => (int)GetTaskStatus(new ManagedTaskHandle(handle));

    public static int GetTaskExceptionHandle(int handle)
    {
        var task = Store.GetTask(handle);
        return GetTaskStatus(task) switch
        {
            ManagedTaskStatus.Faulted => AddExceptionHandle(GetTaskFailureException(task)),
            ManagedTaskStatus.Canceled => AddExceptionHandle(new TaskCanceledException(task)),
            _ => 0
        };
    }

    public static void EnsureTaskSucceeded(int handle)
        => EnsureTaskSucceeded(Store.GetTask(handle));

    public static int GetTaskInt32Result(int handle)
        => GetTaskResult<int>(Store.GetTask(handle));

    public static long GetTaskInt64Result(int handle)
        => GetTaskResult<long>(Store.GetTask(handle));

    public static float GetTaskSingleResult(int handle)
        => GetTaskResult<float>(Store.GetTask(handle));

    public static double GetTaskDoubleResult(int handle)
        => GetTaskResult<double>(Store.GetTask(handle));

    public static int GetTaskBooleanResult(int handle)
        => GetTaskResult<bool>(Store.GetTask(handle)) ? 1 : 0;

    public static int GetTaskObjectResultHandle(int handle)
    {
        var result = GetTaskResultObject(Store.GetTask(handle));
        return result is null ? 0 : AddObjectHandle(result);
    }

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

    private static ManagedTaskStatus GetTaskStatus(Task task)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (!task.IsCompleted)
        {
            return ManagedTaskStatus.Pending;
        }

        if (task.IsCanceled)
        {
            return ManagedTaskStatus.Canceled;
        }

        return task.IsFaulted ? ManagedTaskStatus.Faulted : ManagedTaskStatus.Succeeded;
    }

    private static void EnsureTaskSucceeded(Task task)
    {
        switch (GetTaskStatus(task))
        {
            case ManagedTaskStatus.Succeeded:
                return;
            case ManagedTaskStatus.Pending:
                throw new InvalidOperationException("Task result cannot be extracted before the task is complete.");
            case ManagedTaskStatus.Canceled:
                throw new TaskCanceledException(task);
            case ManagedTaskStatus.Faulted:
                throw GetTaskFailureException(task);
            default:
                throw new InvalidOperationException($"Task status '{task.Status}' is not supported.");
        }
    }

    private static T GetTaskResult<T>(Task task)
    {
        EnsureTaskSucceeded(task);
        return task is Task<T> typedTask
            ? typedTask.Result
            : throw new InvalidOperationException($"Task handle stores '{task.GetType().FullName}', not 'System.Threading.Tasks.Task<{typeof(T).FullName}>'.");
    }

    private static object? GetTaskResultObject(Task task)
    {
        EnsureTaskSucceeded(task);
        return task switch
        {
            Task<string> stringTask => stringTask.Result,
            Task<string[]> stringArrayTask => stringArrayTask.Result,
            Task<int[]> intArrayTask => intArrayTask.Result,
            Task<byte[]> byteArrayTask => byteArrayTask.Result,
            _ => throw new InvalidOperationException($"Task handle stores '{task.GetType().FullName}', not a supported object-result task type.")
        };
    }

    private static Exception GetTaskFailureException(Task task)
    {
        if (task.Exception?.InnerException is { } innerException)
        {
            return innerException;
        }

        if (task.Exception is { } aggregateException)
        {
            return aggregateException;
        }

        return new InvalidOperationException("Task faulted without an exception.");
    }
}
