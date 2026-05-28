using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustYaml")]
[OutputType(typeof(string))]
public sealed class ConvertToRustYamlCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var yaml = _input.Count > 1
            ? string.Concat(PowerShellCommandRunner.InvokePipeline("ConvertTo-Yaml", new Dictionary<string, object?>(), _input.Items)
                .Select(static item => item.BaseObject?.ToString() ?? string.Empty))
            : PowerShellCommandRunner.InvokeStringWithPreferredInputParameter(
                "ConvertTo-Yaml",
                ["InputObject", "Data"],
                _input.ToPowerShellInput());
        WriteObject(RustEngineInvoker.TransformUtf8("marked_yaml_engine.dll", "marked_yaml_echo_utf8_len", "marked_yaml_echo_utf8_copy", yaml));
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustYaml")]
[OutputType(typeof(PSObject), typeof(IDictionary<string, object>))]
public sealed class ConvertFromRustYamlCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
    public object? InputObject { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var yaml = _input.ToText();
        var rustYaml = RustEngineInvoker.TransformUtf8("marked_yaml_engine.dll", "marked_yaml_echo_utf8_len", "marked_yaml_echo_utf8_copy", yaml);
        var results = PowerShellCommandRunner.InvokeWithPreferredInputParameter(
            "ConvertFrom-Yaml",
            ["InputObject", "Yaml"],
            rustYaml);

        foreach (var result in results)
        {
            WriteObject(PowerShellCommandRunner.UnwrapOutputObject(result));
        }
    }
}
