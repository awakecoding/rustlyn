namespace RustMcil.Runtime;

public static class PanicRuntime
{
    public static void PreconditionCheck()
    {
    }

    public static void ThrowDivideByZero()
    {
        throw new DivideByZeroException();
    }

    public static void ThrowOverflow()
    {
        throw new OverflowException();
    }

    public static void ThrowOutOfMemory()
    {
        throw new OutOfMemoryException();
    }

    public static void ThrowPanic()
    {
        throw new InvalidOperationException();
    }

    public static void ThrowUnreachable()
    {
        throw new InvalidOperationException();
    }
}
