using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

[Cmdlet(VerbsData.ConvertTo, "RustCsv")]
[OutputType(typeof(string))]
public sealed class ConvertToRustCsvCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    [Parameter]
    public char Delimiter { get; set; }

    [Parameter]
    public SwitchParameter UseCulture { get; set; }

    [Parameter]
    public SwitchParameter NoTypeInformation { get; set; }

    [Parameter]
    public SwitchParameter IncludeTypeInformation { get; set; }

    [Parameter]
    public SwitchParameter NoHeader { get; set; }

    [Parameter]
    public string[]? QuoteFields { get; set; }

    [Parameter]
    [ValidateSet("Always", "AsNeeded", "Never")]
    public string? UseQuotes { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["UseCulture"] = UseCulture,
            ["NoTypeInformation"] = NoTypeInformation,
            ["IncludeTypeInformation"] = IncludeTypeInformation,
            ["NoHeader"] = NoHeader
        };
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Delimiter)))
        {
            parameters["Delimiter"] = Delimiter;
        }
        if (QuoteFields is { Length: > 0 })
        {
            parameters["QuoteFields"] = QuoteFields;
        }
        if (!string.IsNullOrWhiteSpace(UseQuotes))
        {
            parameters["UseQuotes"] = UseQuotes;
        }

        var results = _input.Count > 1
            ? PowerShellCommandRunner.InvokePipeline("Microsoft.PowerShell.Utility\\ConvertTo-Csv", parameters, _input.Items)
            : PowerShellCommandRunner.Invoke(
                "Microsoft.PowerShell.Utility\\ConvertTo-Csv",
                new Dictionary<string, object?>(parameters) { ["InputObject"] = _input.ToPowerShellInput() });
        var lines = results.Select(static result => result.BaseObject?.ToString() ?? string.Empty).ToArray();
        RustEngineInvoker.ValidateUtf8("csv_engine.dll", "csv_validate_utf8", string.Join(Environment.NewLine, lines));

        foreach (var line in lines)
        {
            WriteObject(line);
        }
    }
}

[Cmdlet(VerbsData.ConvertFrom, "RustCsv")]
[OutputType(typeof(PSObject))]
public sealed class ConvertFromRustCsvCommand : PSCmdlet
{
    private readonly FormatInputBuffer _input = new();

    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
    [AllowEmptyString]
    public string[]? InputObject { get; set; }

    [Parameter]
    public char Delimiter { get; set; }

    [Parameter]
    public SwitchParameter UseCulture { get; set; }

    [Parameter]
    public string[]? Header { get; set; }

    protected override void ProcessRecord()
        => _input.Add(InputObject);

    protected override void EndProcessing()
    {
        var csv = _input.ToText();
        var rustCsv = RustEngineInvoker.TransformUtf8("csv_engine.dll", "csv_echo_utf8_len", "csv_echo_utf8_copy", csv);
        var parameters = new Dictionary<string, object?>
        {
            ["InputObject"] = rustCsv,
            ["UseCulture"] = UseCulture
        };
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Delimiter)))
        {
            parameters["Delimiter"] = Delimiter;
        }
        if (Header is { Length: > 0 })
        {
            parameters["Header"] = Header;
        }

        var results = PowerShellCommandRunner.Invoke("Microsoft.PowerShell.Utility\\ConvertFrom-Csv", parameters);
        foreach (var result in results)
        {
            WriteObject(PowerShellCommandRunner.UnwrapOutputObject(result));
        }
    }
}
