using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

internal static class JsonProjection
{
    public static string ToJson(FormatInputBuffer input, int depth)
    {
        ArgumentNullException.ThrowIfNull(input);

        var parameters = new Dictionary<string, object?>
        {
            ["Depth"] = depth,
            ["Compress"] = new SwitchParameter(true)
        };

        return input.Count == 1 && input.Items[0] is null
            ? "null"
            : PowerShellCommandRunner.InvokeString(
                "Microsoft.PowerShell.Utility\\ConvertTo-Json",
                new Dictionary<string, object?>(parameters) { ["InputObject"] = input.ToPowerShellInput() });
    }

    public static void WriteFromJson(PSCmdlet cmdlet, string json)
    {
        ArgumentNullException.ThrowIfNull(cmdlet);
        ArgumentNullException.ThrowIfNull(json);

        var results = PowerShellCommandRunner.Invoke(
            "Microsoft.PowerShell.Utility\\ConvertFrom-Json",
            new Dictionary<string, object?> { ["InputObject"] = json });
        foreach (var result in results)
        {
            cmdlet.WriteObject(PowerShellCommandRunner.UnwrapOutputObject(result), enumerateCollection: false);
        }
    }
}
