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
        var stream = ObjectStreamProjection.ToStream(_input, depth: 64);
        WriteObject(RustEngineInvoker.TransformUtf8("marked_yaml_engine.dll", "marked_yaml_object_stream_to_yaml_len", "marked_yaml_object_stream_to_yaml_copy", stream));
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustYaml")]
[OutputType(typeof(PSObject), typeof(IDictionary<string, object>))]
public sealed class ConvertFromRustYamlCommand : PSCmdlet
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
        var yaml = _input.ToText();
        var json = RustEngineInvoker.TransformUtf8("marked_yaml_engine.dll", "marked_yaml_to_json_len", "marked_yaml_to_json_copy", yaml);
        JsonProjection.WriteFromJson(this, json, AsHashtable.IsPresent, NoEnumerate.IsPresent);
    }
}
