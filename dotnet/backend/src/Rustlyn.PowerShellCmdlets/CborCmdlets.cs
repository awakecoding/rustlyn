using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustCbor")]
[OutputType(typeof(byte[]))]
public sealed class ConvertToRustCborCommand : PSCmdlet
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
        var bytes = RustEngineInvoker.TransformUtf8ToBytes("cbor_engine.dll", "cbor_object_stream_to_cbor_len", "cbor_object_stream_to_cbor_copy", stream);
        WriteObject(bytes, enumerateCollection: false);
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustCbor")]
[OutputType(typeof(PSObject), typeof(IDictionary<string, object>))]
public sealed class ConvertFromRustCborCommand : PSCmdlet
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
        var json = RustEngineInvoker.TransformBytesToUtf8("cbor_engine.dll", "cbor_to_json_len", "cbor_to_json_copy", bytes);
        JsonProjection.WriteFromJson(this, json, AsHashtable.IsPresent, NoEnumerate.IsPresent);
    }
}
