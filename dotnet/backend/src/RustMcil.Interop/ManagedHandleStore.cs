namespace RustMcil.Interop;

public sealed class ManagedHandleStore
{
    private readonly object gate = new();
    private readonly Dictionary<int, HandleEntry> entries = new();
    private int nextHandle = 1;

    public int Count
    {
        get
        {
            lock (gate)
            {
                return entries.Count;
            }
        }
    }

    public ManagedHandleSnapshot Snapshot
    {
        get
        {
            lock (gate)
            {
                var objectCount = 0;
                var exceptionCount = 0;
                foreach (var entry in entries.Values)
                {
                    if (entry.Kind == ManagedHandleKind.Object)
                    {
                        objectCount++;
                    }
                    else if (entry.Kind == ManagedHandleKind.Exception)
                    {
                        exceptionCount++;
                    }
                }

                return new ManagedHandleSnapshot(objectCount, exceptionCount);
            }
        }
    }

    public ManagedObjectHandle AddObject(object value)
        => new(Add(value, ManagedHandleKind.Object));

    public ManagedExceptionHandle AddException(Exception exception)
        => new(Add(exception, ManagedHandleKind.Exception));

    public T GetObject<T>(ManagedObjectHandle handle)
        => GetObject<T>(handle.Value);

    public T GetObject<T>(int handle)
        => Get<T>(handle, ManagedHandleKind.Object);

    public Exception GetException(ManagedExceptionHandle handle)
        => GetException(handle.Value);

    public Exception GetException(int handle)
        => Get<Exception>(handle, ManagedHandleKind.Exception);

    public bool TryGetObject<T>(ManagedObjectHandle handle, out T? value)
        => TryGet(handle.Value, ManagedHandleKind.Object, out value);

    public bool TryGetException(ManagedExceptionHandle handle, out Exception? value)
        => TryGet(handle.Value, ManagedHandleKind.Exception, out value);

    public bool Release(ManagedObjectHandle handle)
        => Release(handle.Value, ManagedHandleKind.Object);

    public bool Release(ManagedExceptionHandle handle)
        => Release(handle.Value, ManagedHandleKind.Exception);

    public bool Release(int handle)
    {
        lock (gate)
        {
            return entries.Remove(handle);
        }
    }

    public bool Release(int handle, ManagedHandleKind expectedKind)
    {
        lock (gate)
        {
            if (!entries.TryGetValue(handle, out var entry))
            {
                return false;
            }

            if (entry.Kind != expectedKind)
            {
                throw new InvalidOperationException($"Managed handle {handle} is a {entry.Kind} handle, not a {expectedKind} handle.");
            }

            return entries.Remove(handle);
        }
    }

    public void Clear()
    {
        lock (gate)
        {
            entries.Clear();
        }
    }

    private int Add(object value, ManagedHandleKind kind)
    {
        ArgumentNullException.ThrowIfNull(value);

        lock (gate)
        {
            if (nextHandle == int.MaxValue)
            {
                throw new InvalidOperationException("The managed handle store exhausted its handle range.");
            }

            var handle = nextHandle++;
            entries.Add(handle, new HandleEntry(value, kind));
            return handle;
        }
    }

    private T Get<T>(int handle, ManagedHandleKind expectedKind)
    {
        var entry = GetEntry(handle, expectedKind);
        return entry.Value is T typedValue
            ? typedValue
            : throw new InvalidOperationException($"Managed handle {handle} stores '{entry.Value.GetType().FullName}', not '{typeof(T).FullName}'.");
    }

    private bool TryGet<T>(int handle, ManagedHandleKind expectedKind, out T? value)
    {
        lock (gate)
        {
            if (entries.TryGetValue(handle, out var entry)
                && entry.Kind == expectedKind
                && entry.Value is T typedValue)
            {
                value = typedValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    private HandleEntry GetEntry(int handle, ManagedHandleKind expectedKind)
    {
        if (handle <= 0)
        {
            throw new KeyNotFoundException($"Managed handle {handle} is not valid.");
        }

        lock (gate)
        {
            if (!entries.TryGetValue(handle, out var entry))
            {
                throw new KeyNotFoundException($"Managed handle {handle} was not found.");
            }

            if (entry.Kind != expectedKind)
            {
                throw new InvalidOperationException($"Managed handle {handle} is a {entry.Kind} handle, not a {expectedKind} handle.");
            }

            return entry;
        }
    }

    private sealed record HandleEntry(object Value, ManagedHandleKind Kind);
}

public sealed record ManagedHandleSnapshot(int ObjectCount, int ExceptionCount)
{
    public int TotalCount => ObjectCount + ExceptionCount;
}
