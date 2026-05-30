using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustBson")]
[OutputType(typeof(byte[]))]
public sealed class ConvertToRustBsonCommand : PSCmdlet
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
        var stream = ObjectStreamProjection.ToStream(_input, Depth);
        var bytes = RustEngineInvoker.TransformUtf8ToBytes("bson_engine.dll", "bson_object_stream_to_bson_len", "bson_object_stream_to_bson_copy", stream);
        WriteObject(bytes, enumerateCollection: false);
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustBson")]
[OutputType(typeof(PSObject), typeof(IDictionary<string, object>))]
public sealed class ConvertFromRustBsonCommand : PSCmdlet
{
    private readonly BinaryInputBuffer _input = new();

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
        var bytes = _input.ToArray();
        var json = RustEngineInvoker.TransformBytesToUtf8("bson_engine.dll", "bson_to_json_len", "bson_to_json_copy", bytes);
        JsonProjection.WriteFromJson(this, json, AsHashtable.IsPresent, NoEnumerate.IsPresent);
    }
}
