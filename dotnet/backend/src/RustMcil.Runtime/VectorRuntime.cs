namespace RustMcil.Runtime;

public static class VectorRuntime
{
    public static sbyte __scalar_i8_smax(sbyte left, sbyte right) => left < right ? right : left;
    public static sbyte __scalar_i8_umax(sbyte left, sbyte right) => (byte)left < (byte)right ? right : left;
    public static sbyte __scalar_i8_smin(sbyte left, sbyte right) => left > right ? right : left;
    public static sbyte __scalar_i8_umin(sbyte left, sbyte right) => (byte)left > (byte)right ? right : left;

    public static int __intrinsic_fshl_i32(int left, int right, int amount)
    {
        var shift = amount & 31;
        var inverseShift = (32 - shift) & 31;
        return unchecked((int)(((uint)left << shift) | ((uint)right >> inverseShift)));
    }

    public static sbyte[] __vector_i8x16_add(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, static (x, y) => unchecked((sbyte)(x + y)));
    public static sbyte[] __vector_i8x16_or(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, static (x, y) => (sbyte)(x | y));
    public static sbyte[] __vector_i8x16_xor(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, static (x, y) => (sbyte)(x ^ y));
    public static sbyte __vector_i8x16_reduce_xor(sbyte[] value) => ReduceI8(value, 16, static (x, y) => (sbyte)(x ^ y));
    public static sbyte __vector_i8x16_reduce_add(sbyte[] value) => ReduceI8(value, 16, static (x, y) => unchecked((sbyte)(x + y)));
    public static sbyte __vector_i8x16_reduce_or(sbyte[] value) => ReduceI8(value, 16, static (x, y) => (sbyte)(x | y));
    public static sbyte[] __vector_i8x16_smax(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, __scalar_i8_smax);
    public static sbyte[] __vector_i8x16_umax(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, __scalar_i8_umax);
    public static sbyte[] __vector_i8x16_umin(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, __scalar_i8_umin);
    public static sbyte[] __vector_i8x16_smin(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 16, __scalar_i8_smin);
    public static sbyte __vector_i8x16_reduce_smax(sbyte[] value) => ReduceI8(value, 16, __scalar_i8_smax);
    public static sbyte __vector_i8x16_reduce_smin(sbyte[] value) => ReduceI8(value, 16, __scalar_i8_smin);
    public static sbyte __vector_i8x16_reduce_umax(sbyte[] value) => ReduceI8(value, 16, __scalar_i8_umax);
    public static sbyte __vector_i8x16_reduce_umin(sbyte[] value) => ReduceI8(value, 16, __scalar_i8_umin);

    public static sbyte[] __vector_i8x8_add(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 8, static (x, y) => unchecked((sbyte)(x + y)));
    public static sbyte[] __vector_i8x8_or(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 8, static (x, y) => (sbyte)(x | y));
    public static sbyte[] __vector_i8x8_xor(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 8, static (x, y) => (sbyte)(x ^ y));
    public static sbyte __vector_i8x8_reduce_xor(sbyte[] value) => ReduceI8(value, 8, static (x, y) => (sbyte)(x ^ y));
    public static sbyte __vector_i8x8_reduce_add(sbyte[] value) => ReduceI8(value, 8, static (x, y) => unchecked((sbyte)(x + y)));
    public static sbyte __vector_i8x8_reduce_or(sbyte[] value) => ReduceI8(value, 8, static (x, y) => (sbyte)(x | y));
    public static sbyte[] __vector_i8x8_smax(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 8, __scalar_i8_smax);
    public static sbyte __vector_i8x8_reduce_smax(sbyte[] value) => ReduceI8(value, 8, __scalar_i8_smax);
    public static sbyte[] __vector_i8x8_smin(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 8, __scalar_i8_smin);
    public static sbyte __vector_i8x8_reduce_smin(sbyte[] value) => ReduceI8(value, 8, __scalar_i8_smin);

    public static sbyte[] __vector_i8x4_add(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 4, static (x, y) => unchecked((sbyte)(x + y)));
    public static sbyte[] __vector_i8x4_and(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 4, static (x, y) => (sbyte)(x & y));
    public static sbyte[] __vector_i8x4_or(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 4, static (x, y) => (sbyte)(x | y));
    public static sbyte[] __vector_i8x4_xor(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 4, static (x, y) => (sbyte)(x ^ y));
    public static sbyte[] __vector_i8x4_umax(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 4, __scalar_i8_umax);
    public static sbyte[] __vector_i8x4_umin(sbyte[] left, sbyte[] right) => BinaryI8(left, right, 4, __scalar_i8_umin);
    public static sbyte __vector_i8x4_reduce_xor(sbyte[] value) => ReduceI8(value, 4, static (x, y) => (sbyte)(x ^ y));
    public static sbyte __vector_i8x4_reduce_add(sbyte[] value) => ReduceI8(value, 4, static (x, y) => unchecked((sbyte)(x + y)));
    public static sbyte __vector_i8x4_reduce_or(sbyte[] value) => ReduceI8(value, 4, static (x, y) => (sbyte)(x | y));
    public static sbyte __vector_i8x4_reduce_and(sbyte[] value) => ReduceI8(value, 4, static (x, y) => (sbyte)(x & y));
    public static sbyte __vector_i8x4_reduce_umax(sbyte[] value) => ReduceI8(value, 4, __scalar_i8_umax);
    public static sbyte __vector_i8x4_reduce_umin(sbyte[] value) => ReduceI8(value, 4, __scalar_i8_umin);

    public static short[] __vector_i16x8_add(short[] left, short[] right) => BinaryI16(left, right, 8, static (x, y) => unchecked((short)(x + y)));
    public static short[] __vector_i16x8_xor(short[] left, short[] right) => BinaryI16(left, right, 8, static (x, y) => (short)(x ^ y));
    public static short[] __vector_i16x8_and(short[] left, short[] right) => BinaryI16(left, right, 8, static (x, y) => (short)(x & y));
    public static short[] __vector_i16x8_or(short[] left, short[] right) => BinaryI16(left, right, 8, static (x, y) => (short)(x | y));
    public static short[] __vector_i16x8_smin(short[] left, short[] right) => BinaryI16(left, right, 8, static (x, y) => Math.Min(x, y));
    public static short[] __vector_i16x8_smax(short[] left, short[] right) => BinaryI16(left, right, 8, static (x, y) => Math.Max(x, y));
    public static short[] __vector_i16x8_umax(short[] left, short[] right) => BinaryI16(left, right, 8, UMaxI16);
    public static short[] __vector_i16x8_umin(short[] left, short[] right) => BinaryI16(left, right, 8, UMinI16);
    public static short __vector_i16x8_reduce_smin(short[] value) => ReduceI16(value, 8, static (x, y) => Math.Min(x, y));
    public static short __vector_i16x8_reduce_smax(short[] value) => ReduceI16(value, 8, static (x, y) => Math.Max(x, y));
    public static short __vector_i16x8_reduce_umax(short[] value) => ReduceI16(value, 8, UMaxI16);
    public static short __vector_i16x8_reduce_umin(short[] value) => ReduceI16(value, 8, UMinI16);
    public static short __vector_i16x8_reduce_xor(short[] value) => ReduceI16(value, 8, static (x, y) => (short)(x ^ y));
    public static short __vector_i16x8_reduce_add(short[] value) => ReduceI16(value, 8, static (x, y) => unchecked((short)(x + y)));
    public static short __vector_i16x8_reduce_or(short[] value) => ReduceI16(value, 8, static (x, y) => (short)(x | y));
    public static short __vector_i16x8_reduce_and(short[] value) => ReduceI16(value, 8, static (x, y) => (short)(x & y));

    public static short[] __vector_i16x4_add(short[] left, short[] right) => BinaryI16(left, right, 4, static (x, y) => unchecked((short)(x + y)));
    public static short[] __vector_i16x4_xor(short[] left, short[] right) => BinaryI16(left, right, 4, static (x, y) => (short)(x ^ y));
    public static short[] __vector_i16x4_and(short[] left, short[] right) => BinaryI16(left, right, 4, static (x, y) => (short)(x & y));
    public static short[] __vector_i16x4_or(short[] left, short[] right) => BinaryI16(left, right, 4, static (x, y) => (short)(x | y));
    public static short[] __vector_i16x4_smin(short[] left, short[] right) => BinaryI16(left, right, 4, static (x, y) => Math.Min(x, y));
    public static short[] __vector_i16x4_smax(short[] left, short[] right) => BinaryI16(left, right, 4, static (x, y) => Math.Max(x, y));
    public static short[] __vector_i16x4_umax(short[] left, short[] right) => BinaryI16(left, right, 4, UMaxI16);
    public static short[] __vector_i16x4_umin(short[] left, short[] right) => BinaryI16(left, right, 4, UMinI16);
    public static short __vector_i16x4_reduce_smin(short[] value) => ReduceI16(value, 4, static (x, y) => Math.Min(x, y));
    public static short __vector_i16x4_reduce_smax(short[] value) => ReduceI16(value, 4, static (x, y) => Math.Max(x, y));
    public static short __vector_i16x4_reduce_umax(short[] value) => ReduceI16(value, 4, UMaxI16);
    public static short __vector_i16x4_reduce_umin(short[] value) => ReduceI16(value, 4, UMinI16);
    public static short __vector_i16x4_reduce_xor(short[] value) => ReduceI16(value, 4, static (x, y) => (short)(x ^ y));
    public static short __vector_i16x4_reduce_add(short[] value) => ReduceI16(value, 4, static (x, y) => unchecked((short)(x + y)));
    public static short __vector_i16x4_reduce_or(short[] value) => ReduceI16(value, 4, static (x, y) => (short)(x | y));
    public static short __vector_i16x4_reduce_and(short[] value) => ReduceI16(value, 4, static (x, y) => (short)(x & y));

    public static int[] __vector_i32_add(int[] left, int[] right) => BinaryI32(left, right, 4, static (x, y) => unchecked(x + y));
    public static int[] __vector_i32_xor(int[] left, int[] right) => BinaryI32(left, right, 4, static (x, y) => x ^ y);
    public static int[] __vector_i32_and(int[] left, int[] right) => BinaryI32(left, right, 4, static (x, y) => x & y);
    public static int[] __vector_i32_or(int[] left, int[] right) => BinaryI32(left, right, 4, static (x, y) => x | y);
    public static int[] __vector_i32_smax(int[] left, int[] right) => BinaryI32(left, right, 4, Math.Max);
    public static int[] __vector_i32_smin(int[] left, int[] right) => BinaryI32(left, right, 4, Math.Min);
    public static int[] __vector_i32_umax(int[] left, int[] right) => BinaryI32(left, right, 4, UMaxI32);
    public static int[] __vector_i32_umin(int[] left, int[] right) => BinaryI32(left, right, 4, UMinI32);
    public static int __vector_i32_reduce_xor(int[] value) => ReduceI32(value, 4, static (x, y) => x ^ y);
    public static int __vector_i32_reduce_add(int[] value) => ReduceI32(value, 4, static (x, y) => unchecked(x + y));
    public static int __vector_i32_reduce_or(int[] value) => ReduceI32(value, 4, static (x, y) => x | y);
    public static int __vector_i32_reduce_and(int[] value) => ReduceI32(value, 4, static (x, y) => x & y);
    public static int __vector_i32_reduce_smax(int[] value) => ReduceI32(value, 4, Math.Max);
    public static int __vector_i32_reduce_smin(int[] value) => ReduceI32(value, 4, Math.Min);
    public static int __vector_i32_reduce_umax(int[] value) => ReduceI32(value, 4, UMaxI32);
    public static int __vector_i32_reduce_umin(int[] value) => ReduceI32(value, 4, UMinI32);

    public static long[] __vector_i64_add(long[] left, long[] right) => BinaryI64(left, right, 2, static (x, y) => unchecked(x + y));
    public static long[] __vector_i64_xor(long[] left, long[] right) => BinaryI64(left, right, 2, static (x, y) => x ^ y);
    public static long[] __vector_i64_and(long[] left, long[] right) => BinaryI64(left, right, 2, static (x, y) => x & y);
    public static long[] __vector_i64_or(long[] left, long[] right) => BinaryI64(left, right, 2, static (x, y) => x | y);
    public static long[] __vector_i64_smax(long[] left, long[] right) => BinaryI64(left, right, 2, Math.Max);
    public static long[] __vector_i64_smin(long[] left, long[] right) => BinaryI64(left, right, 2, Math.Min);
    public static long[] __vector_i64_umax(long[] left, long[] right) => BinaryI64(left, right, 2, UMaxI64);
    public static long[] __vector_i64_umin(long[] left, long[] right) => BinaryI64(left, right, 2, UMinI64);
    public static long __vector_i64_reduce_add(long[] value) => ReduceI64(value, 2, static (x, y) => unchecked(x + y));
    public static long __vector_i64_reduce_xor(long[] value) => ReduceI64(value, 2, static (x, y) => x ^ y);
    public static long __vector_i64_reduce_or(long[] value) => ReduceI64(value, 2, static (x, y) => x | y);
    public static long __vector_i64_reduce_smax(long[] value) => ReduceI64(value, 2, Math.Max);
    public static long __vector_i64_reduce_smin(long[] value) => ReduceI64(value, 2, Math.Min);
    public static long __vector_i64_reduce_umax(long[] value) => ReduceI64(value, 2, UMaxI64);
    public static long __vector_i64_reduce_umin(long[] value) => ReduceI64(value, 2, UMinI64);
    public static long __vector_i64_reduce_and(long[] value) => ReduceI64(value, 2, static (x, y) => x & y);

    private static sbyte[] BinaryI8(sbyte[] left, sbyte[] right, int count, Func<sbyte, sbyte, sbyte> operation)
    {
        var result = new sbyte[count];
        for (var index = 0; index < count; index++)
        {
            result[index] = operation(left[index], right[index]);
        }

        return result;
    }

    private static short[] BinaryI16(short[] left, short[] right, int count, Func<short, short, short> operation)
    {
        var result = new short[count];
        for (var index = 0; index < count; index++)
        {
            result[index] = operation(left[index], right[index]);
        }

        return result;
    }

    private static int[] BinaryI32(int[] left, int[] right, int count, Func<int, int, int> operation)
    {
        var result = new int[count];
        for (var index = 0; index < count; index++)
        {
            result[index] = operation(left[index], right[index]);
        }

        return result;
    }

    private static long[] BinaryI64(long[] left, long[] right, int count, Func<long, long, long> operation)
    {
        var result = new long[count];
        for (var index = 0; index < count; index++)
        {
            result[index] = operation(left[index], right[index]);
        }

        return result;
    }

    private static sbyte ReduceI8(sbyte[] value, int count, Func<sbyte, sbyte, sbyte> operation)
    {
        var result = value[0];
        for (var index = 1; index < count; index++)
        {
            result = operation(result, value[index]);
        }

        return result;
    }

    private static short ReduceI16(short[] value, int count, Func<short, short, short> operation)
    {
        var result = value[0];
        for (var index = 1; index < count; index++)
        {
            result = operation(result, value[index]);
        }

        return result;
    }

    private static int ReduceI32(int[] value, int count, Func<int, int, int> operation)
    {
        var result = value[0];
        for (var index = 1; index < count; index++)
        {
            result = operation(result, value[index]);
        }

        return result;
    }

    private static long ReduceI64(long[] value, int count, Func<long, long, long> operation)
    {
        var result = value[0];
        for (var index = 1; index < count; index++)
        {
            result = operation(result, value[index]);
        }

        return result;
    }

    private static short UMaxI16(short left, short right) => (ushort)left < (ushort)right ? right : left;
    private static short UMinI16(short left, short right) => (ushort)left > (ushort)right ? right : left;
    private static int UMaxI32(int left, int right) => (uint)left < (uint)right ? right : left;
    private static int UMinI32(int left, int right) => (uint)left > (uint)right ? right : left;
    private static long UMaxI64(long left, long right) => (ulong)left < (ulong)right ? right : left;
    private static long UMinI64(long left, long right) => (ulong)left > (ulong)right ? right : left;
}
