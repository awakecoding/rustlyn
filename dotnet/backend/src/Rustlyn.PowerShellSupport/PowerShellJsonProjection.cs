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

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        if (!noEnumerate && root.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in root.EnumerateArray())
            {
                context.WriteObject(ProjectElement(item, asHashtable), enumerateCollection: false);
            }

            return;
        }

        context.WriteObject(ProjectElement(root, asHashtable), enumerateCollection: false);
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
            JsonValueKind.Array => ProjectArray(element, asHashtable),
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

    private static object?[] ProjectArray(JsonElement element, bool asHashtable)
    {
        var values = new object?[element.GetArrayLength()];
        var index = 0;
        foreach (var item in element.EnumerateArray())
        {
            values[index++] = ProjectElement(item, asHashtable);
        }

        return values;
    }

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
