using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Text.Json;

namespace Rustlyn.PowerShellCmdlets;

internal static class CsvProjection
{
    public static string CreateToCsvRequest(
        FormatInputBuffer input,
        char delimiter,
        bool includeTypeInformation,
        bool noHeader,
        string[]? quoteFields,
        string? useQuotes)
    {
        ArgumentNullException.ThrowIfNull(input);

        var rows = input.Items.Select(CreateRow).ToArray();
        var headers = rows.FirstOrDefault()?.Fields.Select(static field => field.Name).ToArray() ?? [];
        var request = new Dictionary<string, object?>
        {
            ["delimiter"] = delimiter.ToString(),
            ["includeTypeInformation"] = includeTypeInformation,
            ["noHeader"] = noHeader,
            ["quoteFields"] = quoteFields ?? [],
            ["useQuotes"] = string.IsNullOrWhiteSpace(useQuotes) ? "Always" : useQuotes,
            ["typeName"] = input.Items.Count > 0 && input.Items[0] is not null
                ? PSObject.AsPSObject(input.Items[0]).TypeNames.FirstOrDefault() ?? string.Empty
                : string.Empty,
            ["headers"] = headers,
            ["rows"] = rows.Select(row => headers.Select(header => row.Values.TryGetValue(header, out var value) ? value : string.Empty).ToArray()).ToArray()
        };

        return JsonProjection.ToJson(request, depth: 8);
    }

    public static string CreateFromCsvRequest(string csv, char delimiter, string[]? header)
    {
        var request = new Dictionary<string, object?>
        {
            ["csv"] = csv,
            ["delimiter"] = delimiter.ToString(),
            ["header"] = header is { Length: > 0 } ? header : null
        };

        return JsonProjection.ToJson(request, depth: 4);
    }

    public static void WriteCsvLines(PSCmdlet cmdlet, string json)
    {
        ArgumentNullException.ThrowIfNull(cmdlet);
        ArgumentNullException.ThrowIfNull(json);

        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException("Rust CSV engine returned a non-array line payload.");
        }

        foreach (var item in document.RootElement.EnumerateArray())
        {
            cmdlet.WriteObject(item.GetString() ?? string.Empty);
        }
    }

    private static CsvRow CreateRow(object? value)
    {
        if (value is null)
        {
            return new CsvRow([new CsvField("Length", "0")]);
        }

        if (value is IDictionary dictionary)
        {
            var fields = new List<CsvField>();
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is not null)
                {
                    fields.Add(new CsvField(Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? string.Empty, ToCsvString(entry.Value)));
                }
            }

            return new CsvRow(fields);
        }

        var psObject = PSObject.AsPSObject(value);
        var properties = psObject.Properties
            .Where(static property => property.IsGettable)
            .Select(static property => new CsvField(property.Name, ToCsvString(property.Value)))
            .ToArray();

        return properties.Length > 0
            ? new CsvRow(properties)
            : new CsvRow([new CsvField("Length", ToCsvString(value))]);
    }

    private static string ToCsvString(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (value is PSObject psObject)
        {
            value = psObject.BaseObject;
        }

        return (string?)LanguagePrimitives.ConvertTo(value, typeof(string), CultureInfo.CurrentCulture) ?? string.Empty;
    }

    private sealed record CsvField(string Name, string Value);

    private sealed class CsvRow(IEnumerable<CsvField> fields)
    {
        public IReadOnlyList<CsvField> Fields { get; } = fields.ToArray();

        public IReadOnlyDictionary<string, string> Values { get; } = fields.ToDictionary(static field => field.Name, static field => field.Value, StringComparer.Ordinal);
    }
}
