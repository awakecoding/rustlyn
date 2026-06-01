using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rustlyn.PowerShellSupport;

public sealed record PowerShellPropertySnapshot(string Name, PowerShellObjectSnapshot Value);

public sealed record PowerShellObjectSnapshot(
    string Kind,
    string? TypeName,
    string? ScalarValue,
    IReadOnlyList<PowerShellObjectSnapshot> Items,
    IReadOnlyList<PowerShellPropertySnapshot> Properties)
{
    private const int DefaultMaxDepth = 8;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static PowerShellObjectSnapshot FromObject(object? value, int maxDepth = DefaultMaxDepth)
        => FromObject(value, maxDepth, 0, new HashSet<object>(ReferenceEqualityComparer.Instance));

    public static string ToJson(object? value, int maxDepth = DefaultMaxDepth)
        => JsonSerializer.Serialize(FromObject(value, maxDepth), JsonOptions);

    private static PowerShellObjectSnapshot FromObject(object? value, int maxDepth, int depth, HashSet<object> seen)
    {
        if (value is null)
        {
            return Create("null", null, null);
        }

        if (depth >= maxDepth)
        {
            return Create("truncated", value.GetType().FullName, ConvertToInvariantString(value));
        }

        if (value is PSObject psObject)
        {
            return FromPsObject(psObject, maxDepth, depth, seen);
        }

        var type = value.GetType();
        var trackReference = !type.IsValueType && value is not string;
        if (trackReference && !seen.Add(value))
        {
            return Create("cycle", type.FullName, null);
        }

        try
        {
            if (value is DateTime or DateTimeOffset)
            {
                return Create("datetime", type.FullName, ConvertToInvariantString(value));
            }

            if (value is string or char or bool or byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal or Guid)
            {
                return Create("scalar", type.FullName, ConvertToInvariantString(value));
            }

            if (value is Enum)
            {
                return Create("enum", type.FullName, ConvertToInvariantString(value));
            }

            if (value is byte[] bytes)
            {
                return Create("bytes", type.FullName, Convert.ToBase64String(bytes));
            }

            if (value is IDictionary dictionary)
            {
                var properties = new List<PowerShellPropertySnapshot>();
                foreach (DictionaryEntry entry in dictionary)
                {
                    properties.Add(new PowerShellPropertySnapshot(
                        ConvertToInvariantString(entry.Key),
                        FromObject(entry.Value, maxDepth, depth + 1, seen)));
                }

                return new PowerShellObjectSnapshot("dictionary", type.FullName, null, [], properties);
            }

            if (value is IEnumerable enumerable)
            {
                var items = new List<PowerShellObjectSnapshot>();
                foreach (var item in enumerable)
                {
                    items.Add(FromObject(item, maxDepth, depth + 1, seen));
                }

                return new PowerShellObjectSnapshot("array", type.FullName, null, items, []);
            }

            return Create("object", type.FullName, ConvertToInvariantString(value));
        }
        finally
        {
            if (trackReference)
            {
                seen.Remove(value);
            }
        }
    }

    private static PowerShellObjectSnapshot FromPsObject(PSObject psObject, int maxDepth, int depth, HashSet<object> seen)
    {
        if (!seen.Add(psObject))
        {
            return Create("cycle", typeof(PSObject).FullName, null);
        }

        try
        {
            if (ShouldSnapshotBaseObjectDirectly(psObject.BaseObject))
            {
                return FromObject(psObject.BaseObject, maxDepth, depth + 1, seen);
            }

            var properties = new List<PowerShellPropertySnapshot>();
            foreach (var property in psObject.Properties)
            {
                if (!property.IsGettable)
                {
                    continue;
                }

                properties.Add(new PowerShellPropertySnapshot(
                    property.Name,
                    FromObject(property.Value, maxDepth, depth + 1, seen)));
            }

            if (properties.Count > 0)
            {
                return new PowerShellObjectSnapshot("psobject", psObject.BaseObject?.GetType().FullName, null, [], properties);
            }

            return FromObject(psObject.BaseObject, maxDepth, depth + 1, seen);
        }
        finally
        {
            seen.Remove(psObject);
        }
    }

    private static bool ShouldSnapshotBaseObjectDirectly(object? baseObject)
        => baseObject is not null
            && baseObject is not PSCustomObject
            && (baseObject is string or char or bool or byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal or DateTime or DateTimeOffset or Guid or Enum or byte[] or IDictionary
                || (baseObject is IEnumerable && baseObject is not string));

    private static PowerShellObjectSnapshot Create(string kind, string? typeName, string? scalarValue)
        => new(kind, typeName, scalarValue, [], []);

    private static string ConvertToInvariantString(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (value is string text)
        {
            return text;
        }

        if (value is DateTime dateTime)
        {
            return dateTime.ToString("o", CultureInfo.InvariantCulture);
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("o", CultureInfo.InvariantCulture);
        }

        return value is IFormattable formattable
            ? formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty
            : value.ToString() ?? string.Empty;
    }
}
