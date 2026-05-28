using System.Buffers.Binary;
using System.Text;
using System.Text.Json;

namespace Rustlyn.PowerShellCmdlets;

internal static class CborProjection
{
    public static byte[] FromJson(string json)
    {
        using var document = JsonDocument.Parse(json);
        using var stream = new MemoryStream();
        WriteValue(stream, document.RootElement);
        return stream.ToArray();
    }

    public static string ToJson(byte[] bytes)
    {
        var offset = 0;
        var value = ReadValue(bytes, ref offset);
        if (offset != bytes.Length)
        {
            throw new InvalidDataException("CBOR value has trailing bytes.");
        }

        return JsonSerializer.Serialize(value);
    }

    private static void WriteValue(Stream stream, JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                WriteTypeAndLength(stream, 5, value.EnumerateObject().Count());
                foreach (var property in value.EnumerateObject())
                {
                    WriteText(stream, property.Name);
                    WriteValue(stream, property.Value);
                }
                return;
            case JsonValueKind.Array:
                WriteTypeAndLength(stream, 4, value.GetArrayLength());
                foreach (var item in value.EnumerateArray())
                {
                    WriteValue(stream, item);
                }
                return;
            case JsonValueKind.String:
                WriteText(stream, value.GetString() ?? string.Empty);
                return;
            case JsonValueKind.Number when value.TryGetInt64(out var integer):
                if (integer >= 0)
                {
                    WriteTypeAndLength(stream, 0, (ulong)integer);
                }
                else
                {
                    WriteTypeAndLength(stream, 1, (ulong)(-1 - integer));
                }
                return;
            case JsonValueKind.Number:
                stream.WriteByte(0xfb);
                Span<byte> doubleBytes = stackalloc byte[8];
                BinaryPrimitives.WriteDoubleBigEndian(doubleBytes, value.GetDouble());
                stream.Write(doubleBytes);
                return;
            case JsonValueKind.True:
                stream.WriteByte(0xf5);
                return;
            case JsonValueKind.False:
                stream.WriteByte(0xf4);
                return;
            case JsonValueKind.Null:
                stream.WriteByte(0xf6);
                return;
            default:
                throw new InvalidDataException($"Unsupported JSON value kind '{value.ValueKind}' for CBOR.");
        }
    }

    private static object? ReadValue(byte[] bytes, ref int offset)
    {
        var initial = bytes[offset++];
        var major = initial >> 5;
        var additional = initial & 0x1f;
        var argument = ReadArgument(bytes, ref offset, additional);

        return major switch
        {
            0 => argument <= long.MaxValue ? (long)argument : throw new InvalidDataException("CBOR integer is too large."),
            1 => argument < long.MaxValue ? -1L - (long)argument : throw new InvalidDataException("CBOR negative integer is too small."),
            2 => ReadBytes(bytes, ref offset, checked((int)argument)),
            3 => Encoding.UTF8.GetString(ReadBytes(bytes, ref offset, checked((int)argument))),
            4 => ReadArray(bytes, ref offset, checked((int)argument)),
            5 => ReadMap(bytes, ref offset, checked((int)argument)),
            7 when additional == 20 => false,
            7 when additional == 21 => true,
            7 when additional == 22 => null,
            7 when additional == 27 => ReadDouble(bytes, ref offset),
            _ => throw new InvalidDataException($"Unsupported CBOR major type {major} additional {additional}.")
        };
    }

    private static object?[] ReadArray(byte[] bytes, ref int offset, int length)
    {
        var values = new object?[length];
        for (var index = 0; index < length; index++)
        {
            values[index] = ReadValue(bytes, ref offset);
        }
        return values;
    }

    private static Dictionary<string, object?> ReadMap(byte[] bytes, ref int offset, int length)
    {
        var values = new Dictionary<string, object?>(StringComparer.Ordinal);
        for (var index = 0; index < length; index++)
        {
            var key = ReadValue(bytes, ref offset) as string
                ?? throw new InvalidDataException("CBOR map keys must be strings for PowerShell projection.");
            values[key] = ReadValue(bytes, ref offset);
        }
        return values;
    }

    private static void WriteText(Stream stream, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteTypeAndLength(stream, 3, bytes.Length);
        stream.Write(bytes);
    }

    private static void WriteTypeAndLength(Stream stream, int majorType, int length)
        => WriteTypeAndLength(stream, majorType, (ulong)length);

    private static void WriteTypeAndLength(Stream stream, int majorType, ulong length)
    {
        var prefix = majorType << 5;
        if (length < 24)
        {
            stream.WriteByte((byte)(prefix | (int)length));
            return;
        }
        if (length <= byte.MaxValue)
        {
            stream.WriteByte((byte)(prefix | 24));
            stream.WriteByte((byte)length);
            return;
        }
        if (length <= ushort.MaxValue)
        {
            stream.WriteByte((byte)(prefix | 25));
            Span<byte> bytes = stackalloc byte[2];
            BinaryPrimitives.WriteUInt16BigEndian(bytes, (ushort)length);
            stream.Write(bytes);
            return;
        }
        if (length <= uint.MaxValue)
        {
            stream.WriteByte((byte)(prefix | 26));
            Span<byte> bytes = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32BigEndian(bytes, (uint)length);
            stream.Write(bytes);
            return;
        }

        stream.WriteByte((byte)(prefix | 27));
        Span<byte> longBytes = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(longBytes, length);
        stream.Write(longBytes);
    }

    private static ulong ReadArgument(byte[] bytes, ref int offset, int additional)
        => additional switch
        {
            <= 23 => (ulong)additional,
            24 => bytes[offset++],
            25 => ReadUInt16(bytes, ref offset),
            26 => ReadUInt32(bytes, ref offset),
            27 => ReadUInt64(bytes, ref offset),
            _ => throw new InvalidDataException($"Unsupported CBOR additional information {additional}.")
        };

    private static byte[] ReadBytes(byte[] bytes, ref int offset, int length)
    {
        var value = bytes.AsSpan(offset, length).ToArray();
        offset += length;
        return value;
    }

    private static ushort ReadUInt16(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(offset, 2));
        offset += 2;
        return value;
    }

    private static uint ReadUInt32(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadUInt32BigEndian(bytes.AsSpan(offset, 4));
        offset += 4;
        return value;
    }

    private static ulong ReadUInt64(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadUInt64BigEndian(bytes.AsSpan(offset, 8));
        offset += 8;
        return value;
    }

    private static double ReadDouble(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadDoubleBigEndian(bytes.AsSpan(offset, 8));
        offset += 8;
        return value;
    }
}
