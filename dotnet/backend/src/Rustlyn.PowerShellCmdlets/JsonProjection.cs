using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

internal static class JsonProjection
{
    private static readonly JsonWriterOptions CompressedWriterOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static readonly JsonWriterOptions IndentedWriterOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Indented = true
    };

    public static string ToJson(FormatInputBuffer input, int depth)
        => ToJson(input, depth, compress: true, enumsAsStrings: false);

    public static string ToJson(FormatInputBuffer input, int depth, bool compress, bool enumsAsStrings)
    {
        ArgumentNullException.ThrowIfNull(input);

        return ToJson(input.ToPowerShellInput(), depth, compress, enumsAsStrings);
    }

    public static string ToJson(object? value, int depth, bool compress = true, bool enumsAsStrings = false)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, compress ? CompressedWriterOptions : IndentedWriterOptions))
        {
            WriteValue(writer, value, depth <= 0 ? 64 : depth, depth: 0, enumsAsStrings, new HashSet<object>(ReferenceEqualityComparer.Instance));
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static void WriteFromJson(PSCmdlet cmdlet, string json)
        => WriteFromJson(cmdlet, json, asHashtable: false, noEnumerate: false);

    public static void WriteFromJson(PSCmdlet cmdlet, string json, bool asHashtable, bool noEnumerate)
    {
        ArgumentNullException.ThrowIfNull(cmdlet);
        ArgumentNullException.ThrowIfNull(json);

        var projected = Project(json, asHashtable);
        if (!noEnumerate && projected is object?[] values)
        {
            foreach (var value in values)
            {
                cmdlet.WriteObject(value, enumerateCollection: false);
            }

            return;
        }

        cmdlet.WriteObject(projected, enumerateCollection: false);
    }

    public static object? Project(string json, bool asHashtable = false)
    {
        using var document = JsonDocument.Parse(json);
        return ProjectElement(document.RootElement, asHashtable);
    }

    public static string Format(string json)
    {
        using var document = JsonDocument.Parse(json);
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, IndentedWriterOptions))
        {
            document.WriteTo(writer);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void WriteValue(
        Utf8JsonWriter writer,
        object? value,
        int maxDepth,
        int depth,
        bool enumsAsStrings,
        HashSet<object> seen)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        value = UnwrapPowerShellObject(value);

        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (TryWriteScalar(writer, value, enumsAsStrings))
        {
            return;
        }

        if (depth > maxDepth)
        {
            writer.WriteStringValue(ToPowerShellString(value));
            return;
        }

        if (!seen.Add(value))
        {
            throw new InvalidOperationException("Cannot serialize cyclic PowerShell object graphs to JSON.");
        }

        try
        {
            if (value is IDictionary dictionary)
            {
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in dictionary)
                {
                    if (entry.Key is null)
                    {
                        continue;
                    }

                    writer.WritePropertyName(Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? string.Empty);
                    WriteValue(writer, entry.Value, maxDepth, depth + 1, enumsAsStrings, seen);
                }

                writer.WriteEndObject();
                return;
            }

            if (value is IEnumerable enumerable and not string)
            {
                writer.WriteStartArray();
                foreach (var item in enumerable)
                {
                    WriteValue(writer, item, maxDepth, depth + 1, enumsAsStrings, seen);
                }

                writer.WriteEndArray();
                return;
            }

            writer.WriteStartObject();
            foreach (var property in EnumerateProperties(value))
            {
                writer.WritePropertyName(property.Name);
                WriteValue(writer, property.Value, maxDepth, depth + 1, enumsAsStrings, seen);
            }

            writer.WriteEndObject();
        }
        finally
        {
            seen.Remove(value);
        }
    }

    private static bool TryWriteScalar(Utf8JsonWriter writer, object value, bool enumsAsStrings)
    {
        switch (value)
        {
            case string text:
                writer.WriteStringValue(text);
                return true;
            case char character:
                writer.WriteStringValue(character.ToString());
                return true;
            case bool boolean:
                writer.WriteBooleanValue(boolean);
                return true;
            case byte number:
                writer.WriteNumberValue(number);
                return true;
            case sbyte number:
                writer.WriteNumberValue(number);
                return true;
            case short number:
                writer.WriteNumberValue(number);
                return true;
            case ushort number:
                writer.WriteNumberValue(number);
                return true;
            case int number:
                writer.WriteNumberValue(number);
                return true;
            case uint number:
                writer.WriteNumberValue(number);
                return true;
            case long number:
                writer.WriteNumberValue(number);
                return true;
            case ulong number:
                writer.WriteNumberValue(number);
                return true;
            case float number:
                writer.WriteNumberValue(number);
                return true;
            case double number:
                writer.WriteNumberValue(number);
                return true;
            case decimal number:
                writer.WriteNumberValue(number);
                return true;
            case Enum enumValue when enumsAsStrings:
                writer.WriteStringValue(enumValue.ToString());
                return true;
            case Enum enumValue:
                writer.WriteNumberValue(Convert.ToInt64(enumValue, CultureInfo.InvariantCulture));
                return true;
            case DateTime dateTime:
                writer.WriteStringValue(dateTime);
                return true;
            case DateTimeOffset dateTime:
                writer.WriteStringValue(dateTime);
                return true;
            case Guid guid:
                writer.WriteStringValue(guid);
                return true;
            default:
                return false;
        }
    }

    private static object? ProjectElement(JsonElement element, bool asHashtable)
        => element.ValueKind switch
        {
            JsonValueKind.Object => ProjectObject(element, asHashtable),
            JsonValueKind.Array => element.EnumerateArray().Select(item => ProjectElement(item, asHashtable)).ToArray(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt32(out var intValue) => intValue,
            JsonValueKind.Number when element.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when element.TryGetDecimal(out var decimalValue) => decimalValue,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => null
        };

    private static object ProjectObject(JsonElement element, bool asHashtable)
    {
        if (asHashtable)
        {
            var table = new Hashtable(StringComparer.OrdinalIgnoreCase);
            foreach (var property in element.EnumerateObject())
            {
                table[property.Name] = ProjectElement(property.Value, asHashtable);
            }

            return table;
        }

        var psObject = new PSObject();
        foreach (var property in element.EnumerateObject())
        {
            psObject.Properties.Add(new PSNoteProperty(property.Name, ProjectElement(property.Value, asHashtable)));
        }

        return psObject;
    }

    private static object? UnwrapPowerShellObject(object? value)
    {
        if (value is not PSObject psObject)
        {
            return value;
        }

        return psObject.BaseObject is PSCustomObject
            ? psObject
            : psObject.BaseObject;
    }

    private static IEnumerable<(string Name, object? Value)> EnumerateProperties(object value)
    {
        if (value is PSObject psObject)
        {
            foreach (var property in psObject.Properties)
            {
                if (property.IsGettable)
                {
                    yield return (property.Name, property.Value);
                }
            }

            yield break;
        }

        var shellObject = PSObject.AsPSObject(value);
        if (shellObject.BaseObject is PSCustomObject)
        {
            foreach (var property in shellObject.Properties)
            {
                if (property.IsGettable)
                {
                    yield return (property.Name, property.Value);
                }
            }

            yield break;
        }

        foreach (var property in value.GetType().GetProperties())
        {
            if (property.GetIndexParameters().Length == 0)
            {
                yield return (property.Name, property.GetValue(value));
            }
        }
    }

    private static string ToPowerShellString(object value)
        => PSObject.AsPSObject(value).ToString();

    private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
    {
        public static readonly ReferenceEqualityComparer Instance = new();

        public new bool Equals(object? x, object? y)
            => ReferenceEquals(x, y);

        public int GetHashCode(object obj)
            => RuntimeHelpers.GetHashCode(obj);
    }
}
