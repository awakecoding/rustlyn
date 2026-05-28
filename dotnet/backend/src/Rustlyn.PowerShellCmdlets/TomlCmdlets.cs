using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustToml")]
[OutputType(typeof(string))]
public sealed class ConvertToRustTomlCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    [Parameter]
    public int Depth { get; set; } = 8;

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var json = JsonProjection.ToJson(_input, Depth);
        var toml = TomlProjection.FromJson(json);
        WriteObject(toml);
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustToml")]
[OutputType(typeof(PSObject), typeof(IDictionary<string, object>))]
public sealed class ConvertFromRustTomlCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
    public object? InputObject { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var toml = _input.ToText();
        var json = TomlProjection.ToJson(toml);
        JsonProjection.WriteFromJson(this, json);
    }
}

internal static class TomlEngineValidator
{
    public static void ValidateLines(string toml)
    {
        foreach (var line in toml.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            RustEngineInvoker.ValidateUtf8("toml_engine.dll", "toml_validate_utf8", line);
        }
    }
}
