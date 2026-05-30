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
        var stream = ObjectStreamProjection.ToStream(_input, Depth, EnumsAsStrings.IsPresent);
        var json = RustEngineInvoker.TransformUtf8("simd_json_engine.dll", "simd_json_object_stream_to_json_len", "simd_json_object_stream_to_json_copy", stream);
        WriteObject(Compress.IsPresent ? json : JsonProjection.Format(json));
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
        JsonProjection.WriteFromJson(this, rustJson, AsHashtable.IsPresent, NoEnumerate.IsPresent);
    }
}
