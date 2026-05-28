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
        var json = JsonProjection.ToJson(_input, Depth);
        var bytes = BsonProjection.FromJson(json);
        RustEngineInvoker.ValidateBytes("bson_engine.dll", "bson_validate_bytes", bytes);
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

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var bytes = _input.ToArray();
        RustEngineInvoker.ValidateBytes("bson_engine.dll", "bson_validate_bytes", bytes);
        var json = BsonProjection.ToJson(bytes);
        JsonProjection.WriteFromJson(this, json);
    }
}
