using System.Management.Automation;
using System.Xml;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustXml")]
[OutputType(typeof(string), typeof(XmlDocument))]
public sealed class ConvertToRustXmlCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    [Parameter]
    public int Depth { get; set; } = 2;

    [Parameter]
    public SwitchParameter NoTypeInformation { get; set; }

    [Parameter]
    [ValidateSet("String", "Document", "Stream")]
    public string As { get; set; } = "Document";

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["Depth"] = Depth,
            ["NoTypeInformation"] = NoTypeInformation,
            ["As"] = "String"
        };
        var xml = _input.Count > 1
            ? string.Concat(PowerShellCommandRunner.InvokePipeline("Microsoft.PowerShell.Utility\\ConvertTo-Xml", parameters, _input.Items)
                .Select(static item => item.BaseObject?.ToString() ?? string.Empty))
            : PowerShellCommandRunner.InvokeString(
                "Microsoft.PowerShell.Utility\\ConvertTo-Xml",
                new Dictionary<string, object?>(parameters) { ["InputObject"] = _input.ToPowerShellInput() });
        var rustXml = RustEngineInvoker.TransformUtf8("quick_xml_engine.dll", "quick_xml_echo_utf8_len", "quick_xml_echo_utf8_copy", xml);

        if (string.Equals(As, "String", StringComparison.OrdinalIgnoreCase))
        {
            WriteObject(rustXml);
            return;
        }

        if (string.Equals(As, "Stream", StringComparison.OrdinalIgnoreCase))
        {
            WriteObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(rustXml)));
            return;
        }

        var document = new XmlDocument { PreserveWhitespace = true };
        document.LoadXml(rustXml);
        WriteObject(document);
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustXml")]
[OutputType(typeof(XmlDocument))]
public sealed class ConvertFromRustXmlCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
    public object? InputObject { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var xml = _input.ToText();
        RustEngineInvoker.ValidateUtf8("quick_xml_engine.dll", "quick_xml_validate_utf8", xml);
        var document = new XmlDocument { PreserveWhitespace = true };
        document.LoadXml(xml);
        WriteObject(document);
    }
}
