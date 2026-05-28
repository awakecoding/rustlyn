using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustJson")]
[OutputType(typeof(string))]
public sealed class ConvertToRustJsonCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    [Parameter]
    public int Depth { get; set; } = 2;

    [Parameter]
    public SwitchParameter Compress { get; set; }

    [Parameter]
    public SwitchParameter EnumsAsStrings { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["Depth"] = Depth,
            ["Compress"] = Compress,
            ["EnumsAsStrings"] = EnumsAsStrings
        };
        var json = _input.Count == 1 && _input.Items[0] is null
            ? "null"
            : _input.Count > 1
            ? string.Concat(PowerShellCommandRunner.InvokePipeline("Microsoft.PowerShell.Utility\\ConvertTo-Json", parameters, _input.Items)
                .Select(static item => item.BaseObject?.ToString() ?? string.Empty))
            : PowerShellCommandRunner.InvokeString(
                "Microsoft.PowerShell.Utility\\ConvertTo-Json",
                new Dictionary<string, object?>(parameters) { ["InputObject"] = _input.ToPowerShellInput() });
        WriteObject(RustEngineInvoker.TransformUtf8("simd_json_engine.dll", "simd_json_echo_utf8_len", "simd_json_echo_utf8_copy", json));
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustJson")]
[OutputType(typeof(PSObject), typeof(IDictionary<string, object>))]
public sealed class ConvertFromRustJsonCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
    public object? InputObject { get; set; }

    [Parameter]
    public SwitchParameter AsHashtable { get; set; }

    [Parameter]
    public SwitchParameter NoEnumerate { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var json = _input.ToText();
        var rustJson = RustEngineInvoker.TransformUtf8("simd_json_engine.dll", "simd_json_echo_utf8_len", "simd_json_echo_utf8_copy", json);
        var results = PowerShellCommandRunner.Invoke(
            "Microsoft.PowerShell.Utility\\ConvertFrom-Json",
            new Dictionary<string, object?>
            {
                ["InputObject"] = rustJson,
                ["AsHashtable"] = AsHashtable,
                ["NoEnumerate"] = NoEnumerate
            });

        foreach (var result in results)
        {
            WriteObject(PowerShellCommandRunner.UnwrapOutputObject(result), enumerateCollection: false);
        }
    }
}
