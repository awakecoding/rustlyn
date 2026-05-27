namespace Rustlyn.Interop;

public readonly record struct ManagedTaskHandle(int Value)
{
    public bool IsNull => Value == 0;
}
