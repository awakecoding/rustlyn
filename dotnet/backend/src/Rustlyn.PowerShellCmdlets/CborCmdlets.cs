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
        var json = JsonProjection.ToJson(_input, Depth);
        var bytes = CborProjection.FromJson(json);
        RustEngineInvoker.ValidateBytes("cbor_engine.dll", "cbor_validate_bytes", bytes);
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

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var bytes = _input.ToArray();
        RustEngineInvoker.ValidateBytes("cbor_engine.dll", "cbor_validate_bytes", bytes);
        var json = CborProjection.ToJson(bytes);
        JsonProjection.WriteFromJson(this, json);
    }
}
