using System.Management.Automation;
using System.Globalization;

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
        var delimiter = ResolveDelimiter(Delimiter, UseCulture.IsPresent, MyInvocation.BoundParameters.ContainsKey(nameof(Delimiter)));
        var request = CsvProjection.CreateToCsvRequest(
            _input,
            delimiter,
            IncludeTypeInformation.IsPresent,
            NoHeader.IsPresent,
            QuoteFields,
            UseQuotes);
        var linesJson = RustEngineInvoker.TransformUtf8("csv_engine.dll", "csv_json_to_csv_len", "csv_json_to_csv_copy", request);
        CsvProjection.WriteCsvLines(this, linesJson);
    }

    private static char ResolveDelimiter(char delimiter, bool useCulture, bool delimiterWasBound)
        => delimiterWasBound
            ? delimiter
            : useCulture
                ? CultureInfo.CurrentCulture.TextInfo.ListSeparator.FirstOrDefault(',')
                : ',';
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
        var delimiter = MyInvocation.BoundParameters.ContainsKey(nameof(Delimiter))
            ? Delimiter
            : UseCulture.IsPresent
                ? CultureInfo.CurrentCulture.TextInfo.ListSeparator.FirstOrDefault(',')
                : ',';
        var request = CsvProjection.CreateFromCsvRequest(csv, delimiter, Header);
        var rowsJson = RustEngineInvoker.TransformUtf8("csv_engine.dll", "csv_to_json_len", "csv_to_json_copy", request);
        JsonProjection.WriteFromJson(this, rowsJson);
    }
}
