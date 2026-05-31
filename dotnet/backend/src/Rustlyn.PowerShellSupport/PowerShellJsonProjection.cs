using System.Collections;
using System.Management.Automation;
using System.Text.Json;

namespace Rustlyn.PowerShellSupport;

public static class PowerShellJsonProjection
{
    public static void WriteFromJson(PowerShellCmdletContext context, string json, bool asHashtable, bool noEnumerate)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(json);

        var projected = Project(json, asHashtable);
        if (!noEnumerate && projected is object?[] values)
        {
            foreach (var value in values)
            {
                context.WriteObject(value, enumerateCollection: false);
            }

            return;
        }

        context.WriteObject(projected, enumerateCollection: false);
    }

    public static object? Project(string json, bool asHashtable = false)
    {
        ArgumentNullException.ThrowIfNull(json);
        using var document = JsonDocument.Parse(json);
        return ProjectElement(document.RootElement, asHashtable);
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
}
