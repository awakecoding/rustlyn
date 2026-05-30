using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Text;

namespace Rustlyn.PowerShellCmdlets;

internal static class ObjectStreamProjection
{
    public static string ToStream(FormatInputBuffer input, int depth)
        => ToStream(input, depth, enumsAsStrings: false);

    public static string ToStream(FormatInputBuffer input, int depth, bool enumsAsStrings)
    {
        ArgumentNullException.ThrowIfNull(input);
        return ToStream(input.ToPowerShellInput(), depth, enumsAsStrings);
    }

    public static string ToStream(object? value, int depth)
        => ToStream(value, depth, enumsAsStrings: false);

    public static string ToStream(object? value, int depth, bool enumsAsStrings)
    {
        var builder = new StringBuilder();
        WriteValue(builder, value, depth <= 0 ? 64 : depth, 0, enumsAsStrings, new HashSet<object>(ReferenceEqualityComparer.Instance));
        return builder.ToString();
    }

    private static void WriteValue(StringBuilder builder, object? value, int maxDepth, int depth, bool enumsAsStrings, HashSet<object> seen)
    {
        value = UnwrapPowerShellObject(value);
        if (value is null)
        {
            builder.Append("N;");
            return;
        }

        switch (value)
        {
            case bool boolean:
                builder.Append(boolean ? "T;" : "F;");
                return;
            case string text:
                WriteString(builder, text);
                return;
            case char character:
                WriteString(builder, character.ToString());
                return;
            case byte or sbyte or short or ushort or int or uint or long:
                builder.Append('I');
                builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
                builder.Append(';');
                return;
            case ulong unsigned when unsigned <= long.MaxValue:
                builder.Append('I');
                builder.Append(unsigned.ToString(CultureInfo.InvariantCulture));
                builder.Append(';');
                return;
            case float or double or decimal:
                builder.Append('D');
                builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
                builder.Append(';');
                return;
            case Enum enumValue when enumsAsStrings:
                WriteString(builder, enumValue.ToString());
                return;
            case Enum enumValue:
                builder.Append('I');
                builder.Append(Convert.ToInt64(enumValue, CultureInfo.InvariantCulture));
                builder.Append(';');
                return;
            case DateTime dateTime:
                WriteString(builder, dateTime.ToString("O", CultureInfo.InvariantCulture));
                return;
            case DateTimeOffset dateTime:
                WriteString(builder, dateTime.ToString("O", CultureInfo.InvariantCulture));
                return;
            case Guid guid:
                WriteString(builder, guid.ToString());
                return;
        }

        if (depth > maxDepth)
        {
            WriteString(builder, PSObject.AsPSObject(value).ToString());
            return;
        }

        if (!seen.Add(value))
        {
            throw new InvalidOperationException("Cannot serialize cyclic PowerShell object graphs.");
        }

        try
        {
            if (value is IDictionary dictionary)
            {
                var entries = new List<(string Name, object? Value)>();
                foreach (DictionaryEntry entry in dictionary)
                {
                    if (entry.Key is not null)
                    {
                        entries.Add((Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? string.Empty, entry.Value));
                    }
                }

                WriteObject(builder, entries, maxDepth, depth, enumsAsStrings, seen);
                return;
            }

            if (value is IEnumerable enumerable and not string)
            {
                var values = enumerable.Cast<object?>().ToArray();
                builder.Append('A');
                builder.Append(values.Length.ToString(CultureInfo.InvariantCulture));
                builder.Append(':');
                foreach (var item in values)
                {
                    WriteValue(builder, item, maxDepth, depth + 1, enumsAsStrings, seen);
                }

                return;
            }

            WriteObject(builder, EnumerateProperties(value).ToArray(), maxDepth, depth, enumsAsStrings, seen);
        }
        finally
        {
            seen.Remove(value);
        }
    }

    private static void WriteObject(
        StringBuilder builder,
        IReadOnlyList<(string Name, object? Value)> entries,
        int maxDepth,
        int depth,
        bool enumsAsStrings,
        HashSet<object> seen)
    {
        builder.Append('O');
        builder.Append(entries.Count.ToString(CultureInfo.InvariantCulture));
        builder.Append(':');
        foreach (var entry in entries)
        {
            WriteRawString(builder, entry.Name);
            WriteValue(builder, entry.Value, maxDepth, depth + 1, enumsAsStrings, seen);
        }
    }

    private static void WriteString(StringBuilder builder, string value)
    {
        builder.Append('S');
        WriteRawString(builder, value);
    }

    private static void WriteRawString(StringBuilder builder, string value)
    {
        builder.Append(Encoding.UTF8.GetByteCount(value).ToString(CultureInfo.InvariantCulture));
        builder.Append(':');
        builder.Append(value);
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
        var psObject = PSObject.AsPSObject(value);
        foreach (var property in psObject.Properties)
        {
            if (property.IsGettable)
            {
                yield return (property.Name, property.Value);
            }
        }
    }

    private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
    {
        public static readonly ReferenceEqualityComparer Instance = new();

        public new bool Equals(object? x, object? y)
            => ReferenceEquals(x, y);

        public int GetHashCode(object obj)
            => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
    }
}
