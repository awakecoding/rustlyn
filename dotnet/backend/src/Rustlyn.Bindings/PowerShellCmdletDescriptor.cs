using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rustlyn.Bindings;

public sealed record PowerShellCmdletEntrypointDescriptor(
    string EngineAssemblyName,
    string TypeName,
    string MethodName);

public sealed record PowerShellCmdletParameterDescriptor(
    string Name,
    string TypeName,
    int? Position = null,
    bool Mandatory = false,
    bool ValueFromPipeline = false,
    bool ValueFromPipelineByPropertyName = false,
    bool AllowEmptyString = false,
    IReadOnlyList<string>? ValidateSet = null,
    string? DefaultValueExpression = null);

public sealed record PowerShellCmdletDescriptor(
    string ClassName,
    string VerbName,
    string VerbExpression,
    string NounName,
    bool SupportsShouldProcess,
    IReadOnlyList<string> OutputTypeExpressions,
    IReadOnlyList<PowerShellCmdletParameterDescriptor> Parameters,
    PowerShellCmdletEntrypointDescriptor? BeginProcessingEntrypoint,
    PowerShellCmdletEntrypointDescriptor? ProcessRecordEntrypoint,
    PowerShellCmdletEntrypointDescriptor? EndProcessingEntrypoint,
    string MigrationStrategy);

public static class PowerShellCmdletMigrationStrategies
{
    public const string GeneratedRust = "generated-rust";
    public const string CompatibilityFallbackPendingRunspacePrototype = "compatibility-fallback-pending-runspace-prototype";
}

public static class PowerShellCmdletDescriptorCatalog
{
    private const string EngineAssemblyName = "rustlyn_powershell_format_cmdlets.dll";
    private const string EngineTypeName = "Rustlyn.GeneratedModule";

    public static IReadOnlyList<PowerShellCmdletDescriptor> CreateCurrentFormatCmdlets()
        =>
        [
            ConvertTo("ConvertToRustJsonCommand", "RustJson", ["string"], [PipelineObject(), Parameter("Depth", "int", defaultValueExpression: "2"), Switch("Compress"), Switch("EnumsAsStrings")]),
            ConvertFrom("ConvertFromRustJsonCommand", "RustJson", ["System.Management.Automation.PSObject", "System.Collections.Generic.IDictionary<string, object>"], [PipelineObject(mandatory: true), Switch("AsHashtable"), Switch("NoEnumerate")]),

            ConvertTo("ConvertToRustCsvCommand", "RustCsv", ["string"], [PipelineObject(), Parameter("Delimiter", "char"), Switch("UseCulture"), Switch("NoTypeInformation"), Switch("IncludeTypeInformation"), Switch("NoHeader"), Parameter("QuoteFields", "string[]?"), Parameter("UseQuotes", "string?", validateSet: ["Always", "AsNeeded", "Never"])]),
            ConvertFrom("ConvertFromRustCsvCommand", "RustCsv", ["System.Management.Automation.PSObject"], [Parameter("InputObject", "string[]?", position: 0, mandatory: true, valueFromPipeline: true, allowEmptyString: true), Parameter("Delimiter", "char"), Switch("UseCulture"), Parameter("Header", "string[]?")]),

            ConvertTo("ConvertToRustTomlCommand", "RustToml", ["string"], [PipelineObject(), Parameter("Depth", "int", defaultValueExpression: "8")]),
            ConvertFrom("ConvertFromRustTomlCommand", "RustToml", ["System.Management.Automation.PSObject", "System.Collections.Generic.IDictionary<string, object>"], [PipelineObject(mandatory: true)]),

            ConvertTo("ConvertToRustXmlCommand", "RustXml", ["string", "System.Xml.XmlDocument", "System.IO.Stream"], [PipelineObject(), Parameter("Depth", "int", defaultValueExpression: "2"), Switch("NoTypeInformation"), Parameter("As", "string", validateSet: ["String", "Document", "Stream"], defaultValueExpression: "\"Document\"")]),
            ConvertFrom("ConvertFromRustXmlCommand", "RustXml", ["System.Xml.XmlDocument"], [PipelineObject(mandatory: true)]),

            ConvertTo("ConvertToRustYamlCommand", "RustYaml", ["string"], [PipelineObject()]),
            ConvertFrom("ConvertFromRustYamlCommand", "RustYaml", ["System.Management.Automation.PSObject", "System.Collections.Generic.IDictionary<string, object>"], [PipelineObject(mandatory: true), Switch("AsHashtable"), Switch("NoEnumerate")]),

            ConvertTo("ConvertToRustBsonCommand", "RustBson", ["byte[]"], [PipelineObject(), Parameter("Depth", "int", defaultValueExpression: "8")]),
            ConvertFrom("ConvertFromRustBsonCommand", "RustBson", ["System.Management.Automation.PSObject", "System.Collections.Generic.IDictionary<string, object>"], [PipelineObject(mandatory: true), Switch("AsHashtable"), Switch("NoEnumerate")]),

            ConvertTo("ConvertToRustCborCommand", "RustCbor", ["byte[]"], [PipelineObject(), Parameter("Depth", "int", defaultValueExpression: "8")]),
            ConvertFrom("ConvertFromRustCborCommand", "RustCbor", ["System.Management.Automation.PSObject", "System.Collections.Generic.IDictionary<string, object>"], [PipelineObject(mandatory: true), Switch("AsHashtable"), Switch("NoEnumerate")])
        ];

    public static string CreateCurrentFormatCmdletJson()
        => JsonSerializer.Serialize(CreateCurrentFormatCmdlets(), new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        });

    private static PowerShellCmdletDescriptor ConvertTo(
        string className,
        string nounName,
        IReadOnlyList<string> outputTypes,
        IReadOnlyList<PowerShellCmdletParameterDescriptor> parameters,
        string migrationStrategy = PowerShellCmdletMigrationStrategies.GeneratedRust)
        => Cmdlet(className, "ConvertTo", "VerbsData.ConvertTo", nounName, outputTypes, parameters, migrationStrategy);

    private static PowerShellCmdletDescriptor ConvertFrom(
        string className,
        string nounName,
        IReadOnlyList<string> outputTypes,
        IReadOnlyList<PowerShellCmdletParameterDescriptor> parameters,
        string migrationStrategy = PowerShellCmdletMigrationStrategies.GeneratedRust)
        => Cmdlet(className, "ConvertFrom", "VerbsData.ConvertFrom", nounName, outputTypes, parameters, migrationStrategy);

    private static PowerShellCmdletDescriptor Cmdlet(
        string className,
        string verbName,
        string verbExpression,
        string nounName,
        IReadOnlyList<string> outputTypes,
        IReadOnlyList<PowerShellCmdletParameterDescriptor> parameters,
        string migrationStrategy)
    {
        var prefix = ToSnakeCase(className.Replace("Command", string.Empty, StringComparison.Ordinal));
        return new PowerShellCmdletDescriptor(
            className,
            verbName,
            verbExpression,
            nounName,
            SupportsShouldProcess: false,
            OutputTypeExpressions: outputTypes,
            Parameters: parameters,
            BeginProcessingEntrypoint: null,
            ProcessRecordEntrypoint: Entrypoint($"{prefix}_process_record"),
            EndProcessingEntrypoint: Entrypoint($"{prefix}_end_processing"),
            MigrationStrategy: migrationStrategy);
    }

    private static PowerShellCmdletEntrypointDescriptor Entrypoint(string methodName)
        => new(EngineAssemblyName, EngineTypeName, methodName);

    private static PowerShellCmdletParameterDescriptor PipelineObject(bool mandatory = false)
        => Parameter("InputObject", "object?", position: 0, mandatory: mandatory, valueFromPipeline: true);

    private static PowerShellCmdletParameterDescriptor Switch(string name)
        => Parameter(name, "SwitchParameter");

    private static PowerShellCmdletParameterDescriptor Parameter(
        string name,
        string typeName,
        int? position = null,
        bool mandatory = false,
        bool valueFromPipeline = false,
        bool allowEmptyString = false,
        IReadOnlyList<string>? validateSet = null,
        string? defaultValueExpression = null)
        => new(
            name,
            typeName,
            position,
            mandatory,
            valueFromPipeline,
            ValueFromPipelineByPropertyName: false,
            AllowEmptyString: allowEmptyString,
            ValidateSet: validateSet,
            DefaultValueExpression: defaultValueExpression);

    private static string ToSnakeCase(string value)
    {
        var builder = new System.Text.StringBuilder(value.Length + 8);
        for (var index = 0; index < value.Length; index++)
        {
            var c = value[index];
            if (char.IsUpper(c) && index > 0)
            {
                builder.Append('_');
            }

            builder.Append(char.ToLowerInvariant(c));
        }

        return builder.ToString();
    }
}
