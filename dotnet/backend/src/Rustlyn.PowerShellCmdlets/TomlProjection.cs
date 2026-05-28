using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Rustlyn.PowerShellCmdlets;

internal static class TomlProjection
{
    public static string FromJson(string json)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("ConvertTo-RustToml currently supports TOML document objects.");
        }

        var builder = new StringBuilder();
        foreach (var property in document.RootElement.EnumerateObject())
        {
            builder.Append(FormatKey(property.Name));
            builder.Append(" = ");
            AppendValue(builder, property.Value);
            builder.AppendLine();
        }

        return builder.ToString();
    }

    public static string ToJson(string toml)
    {
        var values = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var rawLine in toml.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n'))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }
            if (line.StartsWith('['))
            {
                throw new InvalidDataException("ConvertFrom-RustToml currently supports flat TOML documents.");
            }

            var separator = line.IndexOf('=');
            if (separator <= 0)
            {
                throw new InvalidDataException($"Invalid TOML key/value line: '{line}'.");
            }

            var key = ParseKey(line[..separator].Trim());
            values[key] = ParseValue(line[(separator + 1)..].Trim());
        }

        return JsonSerializer.Serialize(values);
    }

    private static void AppendValue(StringBuilder builder, JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.String:
                AppendQuotedString(builder, value.GetString() ?? string.Empty);
                return;
            case JsonValueKind.Number:
                builder.Append(value.GetRawText());
                return;
            case JsonValueKind.True:
                builder.Append("true");
                return;
            case JsonValueKind.False:
                builder.Append("false");
                return;
            case JsonValueKind.Array:
                builder.Append('[');
                var first = true;
                foreach (var item in value.EnumerateArray())
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }
                    AppendValue(builder, item);
                    first = false;
                }
                builder.Append(']');
                return;
            default:
                throw new InvalidDataException($"TOML projection does not support JSON value kind '{value.ValueKind}'.");
        }
    }

    private static object? ParseValue(string value)
    {
        if (value.StartsWith('"') && value.EndsWith('"') && value.Length >= 2)
        {
            return JsonSerializer.Deserialize<string>(value);
        }
        if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        if (value.StartsWith('[') && value.EndsWith(']'))
        {
            var inner = value[1..^1].Trim();
            if (inner.Length == 0)
            {
                return Array.Empty<object?>();
            }

            return SplitArray(inner).Select(ParseValue).ToArray();
        }
        if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var integer))
        {
            return integer;
        }
        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
        {
            return number;
        }

        throw new InvalidDataException($"Unsupported TOML value: '{value}'.");
    }

    private static IEnumerable<string> SplitArray(string inner)
    {
        var start = 0;
        var inString = false;
        var escaped = false;
        for (var index = 0; index < inner.Length; index++)
        {
            var character = inner[index];
            if (escaped)
            {
                escaped = false;
                continue;
            }
            if (character == '\\' && inString)
            {
                escaped = true;
                continue;
            }
            if (character == '"')
            {
                inString = !inString;
                continue;
            }
            if (character == ',' && !inString)
            {
                yield return inner[start..index].Trim();
                start = index + 1;
            }
        }

        yield return inner[start..].Trim();
    }

    private static string FormatKey(string key)
        => key.All(static character => char.IsAsciiLetterOrDigit(character) || character is '_' or '-')
            ? key
            : JsonSerializer.Serialize(key);

    private static string ParseKey(string key)
        => key.StartsWith('"') && key.EndsWith('"')
            ? JsonSerializer.Deserialize<string>(key) ?? string.Empty
            : key;

    private static void AppendQuotedString(StringBuilder builder, string value)
        => builder.Append(JsonSerializer.Serialize(value));
}
