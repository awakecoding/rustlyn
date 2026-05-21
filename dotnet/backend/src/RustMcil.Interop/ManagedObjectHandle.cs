namespace RustMcil.Interop;

public readonly record struct ManagedObjectHandle(int Value)
{
    public bool IsNull => Value == 0;
}