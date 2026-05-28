using System.Buffers.Binary;
using System.Text;
using System.Text.Json;

namespace Rustlyn.PowerShellCmdlets;

internal static class BsonProjection
{
    public static byte[] FromJson(string json)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("ConvertTo-RustBson requires a document object.");
        }

        return WriteDocument(document.RootElement.EnumerateObject());
    }

    public static string ToJson(byte[] bytes)
    {
        var offset = 0;
        var value = ReadDocument(bytes, ref offset);
        if (offset != bytes.Length)
        {
            throw new InvalidDataException("BSON document has trailing bytes.");
        }

        return JsonSerializer.Serialize(value);
    }

    private static byte[] WriteDocument(IEnumerable<JsonProperty> properties)
    {
        using var stream = new MemoryStream();
        stream.Write(new byte[4]);
        foreach (var property in properties)
        {
            WriteElement(stream, property.Name, property.Value);
        }
        stream.WriteByte(0);
        var bytes = stream.ToArray();
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(0, 4), bytes.Length);
        return bytes;
    }

    private static void WriteElement(Stream stream, string name, JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.String:
                stream.WriteByte(0x02);
                WriteCString(stream, name);
                WriteString(stream, value.GetString() ?? string.Empty);
                return;
            case JsonValueKind.Number when value.TryGetInt32(out var integer32):
                stream.WriteByte(0x10);
                WriteCString(stream, name);
                WriteInt32(stream, integer32);
                return;
            case JsonValueKind.Number when value.TryGetInt64(out var integer64):
                stream.WriteByte(0x12);
                WriteCString(stream, name);
                WriteInt64(stream, integer64);
                return;
            case JsonValueKind.Number:
                stream.WriteByte(0x01);
                WriteCString(stream, name);
                WriteDouble(stream, value.GetDouble());
                return;
            case JsonValueKind.True:
            case JsonValueKind.False:
                stream.WriteByte(0x08);
                WriteCString(stream, name);
                stream.WriteByte(value.GetBoolean() ? (byte)1 : (byte)0);
                return;
            case JsonValueKind.Null:
                stream.WriteByte(0x0A);
                WriteCString(stream, name);
                return;
            case JsonValueKind.Object:
                stream.WriteByte(0x03);
                WriteCString(stream, name);
                stream.Write(WriteDocument(value.EnumerateObject()));
                return;
            case JsonValueKind.Array:
                stream.WriteByte(0x04);
                WriteCString(stream, name);
                stream.Write(WriteArrayDocument(value.EnumerateArray()));
                return;
            default:
                throw new InvalidDataException($"Unsupported JSON value kind '{value.ValueKind}' for BSON.");
        }
    }

    private static byte[] WriteArrayDocument(IEnumerable<JsonElement> values)
    {
        using var stream = new MemoryStream();
        stream.Write(new byte[4]);
        var index = 0;
        foreach (var value in values)
        {
            WriteElement(stream, index.ToString(System.Globalization.CultureInfo.InvariantCulture), value);
            index++;
        }
        stream.WriteByte(0);
        var bytes = stream.ToArray();
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(0, 4), bytes.Length);
        return bytes;
    }

    private static Dictionary<string, object?> ReadDocument(byte[] bytes, ref int offset)
    {
        var start = offset;
        var length = ReadInt32(bytes, ref offset);
        var end = start + length;
        if (length < 5 || end > bytes.Length || bytes[end - 1] != 0)
        {
            throw new InvalidDataException("Invalid BSON document length.");
        }

        var result = new Dictionary<string, object?>(StringComparer.Ordinal);
        while (offset < end - 1)
        {
            var type = bytes[offset++];
            var name = ReadCString(bytes, ref offset, end);
            result[name] = ReadValue(type, bytes, ref offset, end);
        }
        offset = end;
        return result;
    }

    private static object? ReadValue(byte type, byte[] bytes, ref int offset, int limit)
        => type switch
        {
            0x01 => ReadDouble(bytes, ref offset),
            0x02 => ReadString(bytes, ref offset, limit),
            0x03 => ReadDocument(bytes, ref offset),
            0x04 => ReadArray(bytes, ref offset),
            0x08 => bytes[offset++] != 0,
            0x0A => null,
            0x10 => ReadInt32(bytes, ref offset),
            0x12 => ReadInt64(bytes, ref offset),
            _ => throw new InvalidDataException($"Unsupported BSON element type 0x{type:x2}.")
        };

    private static object?[] ReadArray(byte[] bytes, ref int offset)
    {
        var document = ReadDocument(bytes, ref offset);
        var values = new object?[document.Count];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = document[index.ToString(System.Globalization.CultureInfo.InvariantCulture)];
        }
        return values;
    }

    private static void WriteCString(Stream stream, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        if (bytes.Contains((byte)0))
        {
            throw new InvalidDataException("BSON names cannot contain null bytes.");
        }
        stream.Write(bytes);
        stream.WriteByte(0);
    }

    private static void WriteString(Stream stream, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteInt32(stream, bytes.Length + 1);
        stream.Write(bytes);
        stream.WriteByte(0);
    }

    private static string ReadString(byte[] bytes, ref int offset, int limit)
    {
        var length = ReadInt32(bytes, ref offset);
        if (length <= 0 || offset + length > limit || bytes[offset + length - 1] != 0)
        {
            throw new InvalidDataException("Invalid BSON string length.");
        }
        var value = Encoding.UTF8.GetString(bytes, offset, length - 1);
        offset += length;
        return value;
    }

    private static string ReadCString(byte[] bytes, ref int offset, int limit)
    {
        var start = offset;
        while (offset < limit && bytes[offset] != 0)
        {
            offset++;
        }
        if (offset >= limit)
        {
            throw new InvalidDataException("Unterminated BSON cstring.");
        }
        var value = Encoding.UTF8.GetString(bytes, start, offset - start);
        offset++;
        return value;
    }

    private static void WriteInt32(Stream stream, int value)
    {
        Span<byte> bytes = stackalloc byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
        stream.Write(bytes);
    }

    private static void WriteInt64(Stream stream, long value)
    {
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
        stream.Write(bytes);
    }

    private static void WriteDouble(Stream stream, double value)
    {
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleLittleEndian(bytes, value);
        stream.Write(bytes);
    }

    private static int ReadInt32(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(offset, 4));
        offset += 4;
        return value;
    }

    private static long ReadInt64(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(offset, 8));
        offset += 8;
        return value;
    }

    private static double ReadDouble(byte[] bytes, ref int offset)
    {
        var value = BinaryPrimitives.ReadDoubleLittleEndian(bytes.AsSpan(offset, 8));
        offset += 8;
        return value;
    }
}
