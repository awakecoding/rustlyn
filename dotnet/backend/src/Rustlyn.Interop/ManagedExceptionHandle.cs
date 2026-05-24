namespace Rustlyn.Interop;

public readonly record struct ManagedExceptionHandle(int Value)
{
    public bool IsNull => Value == 0;
}