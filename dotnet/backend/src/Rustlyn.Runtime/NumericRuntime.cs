namespace Rustlyn.Runtime;

public static class NumericRuntime
{
    public static int RemEuclidI32(int left, int right)
    {
        if (right == 0)
        {
            throw new DivideByZeroException();
        }

        var remainder = left % right;
        return remainder < 0
            ? remainder + Math.Abs(right)
            : remainder;
    }

    public static long RemEuclidI64(long left, long right)
    {
        if (right == 0)
        {
            throw new DivideByZeroException();
        }

        var remainder = left % right;
        return remainder < 0
            ? remainder + Math.Abs(right)
            : remainder;
    }
}
