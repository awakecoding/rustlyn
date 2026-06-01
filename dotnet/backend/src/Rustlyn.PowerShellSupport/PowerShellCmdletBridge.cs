using System.Management.Automation;
using System.Text;
using System.Xml;

namespace Rustlyn.PowerShellSupport;

public static class PowerShellCmdletBridge
{
    private const int XmlOutputString = 0;
    private const int XmlOutputDocument = 1;
    private const int XmlOutputStream = 2;

    public static void WriteObject(PowerShellCmdletContext context, string value)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteObject(value);
    }

    public static void WriteObject(PowerShellCmdletContext context, object? value, bool enumerateCollection)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteObject(value, enumerateCollection);
    }

    public static void WriteObjectBytes(PowerShellCmdletContext context, IntPtr bytesPointer, long byteLength)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentOutOfRangeException.ThrowIfNegative(byteLength);
        if (byteLength > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(byteLength), "PowerShell byte output exceeds the maximum managed array length.");
        }
        if (byteLength > 0 && bytesPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(bytesPointer));
        }

        var bytes = new byte[(int)byteLength];
        if (bytes.Length > 0)
        {
            System.Runtime.InteropServices.Marshal.Copy(bytesPointer, bytes, 0, bytes.Length);
        }

        context.WriteObject(bytes, enumerateCollection: false);
    }

    public static void WriteJson(PowerShellCmdletContext context, string json, bool asHashtable, bool noEnumerate)
    {
        ArgumentNullException.ThrowIfNull(context);
        PowerShellJsonProjection.WriteFromJson(context, json, asHashtable, noEnumerate);
    }

    public static void AddXmlInput(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        GetLifecycleState(context).AddXmlInput(context.InputObject);
    }

    public static void AddXmlTextInput(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var value = context.InputObject is PSObject psObject
            ? psObject.BaseObject?.ToString()
            : context.InputObject?.ToString();
        GetLifecycleState(context).AddXmlTextInput(value ?? string.Empty);
    }

    public static string ConvertXmlInputsToString(PowerShellCmdletContext context, int depth, bool noTypeInformation)
    {
        ArgumentNullException.ThrowIfNull(context);

        var state = GetLifecycleState(context);
        try
        {
            var parameters = new Dictionary<string, object?>
            {
                ["Depth"] = depth,
                ["As"] = "String"
            };
            if (noTypeInformation)
            {
                parameters["NoTypeInformation"] = SwitchParameter.Present;
            }

            return state.XmlInputs.Count > 1
                ? string.Concat(InvokePipeline("Microsoft.PowerShell.Utility\\ConvertTo-Xml", parameters, state.XmlInputs)
                    .Select(static item => item.BaseObject?.ToString() ?? string.Empty))
                : InvokeString(
                    "Microsoft.PowerShell.Utility\\ConvertTo-Xml",
                    new Dictionary<string, object?>(parameters) { ["InputObject"] = state.XmlInputs.Count == 0 ? null : state.XmlInputs[0] });
        }
        finally
        {
            state.ClearXmlInputs();
        }
    }

    public static void WriteConvertedXmlInputs(PowerShellCmdletContext context, int depth, bool noTypeInformation, int outputMode)
    {
        var xml = ConvertXmlInputsToString(context, depth, noTypeInformation);
        WriteXml(context, xml, outputMode);
    }

    public static void WriteXmlTextInputsAsDocument(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var state = GetLifecycleState(context);
        try
        {
            WriteXml(context, string.Join("\r\n", state.XmlTextInputs), XmlOutputDocument);
        }
        finally
        {
            state.ClearXmlTextInputs();
        }
    }

    public static void WriteXml(PowerShellCmdletContext context, string xml, int outputMode)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(xml);

        switch (outputMode)
        {
            case XmlOutputString:
                context.WriteObject(xml);
                break;

            case XmlOutputDocument:
                var document = new XmlDocument { PreserveWhitespace = true };
                document.LoadXml(xml);
                context.WriteObject(document);
                break;

            case XmlOutputStream:
                GetLifecycleState(context).AddPendingXmlStream(Encoding.UTF8.GetBytes(xml));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(outputMode), $"Unsupported XML output mode '{outputMode}'.");
        }
    }

    public static void FlushPendingOutputs(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (var output in GetLifecycleState(context).DrainPendingXmlStreams())
        {
            context.WriteObject(new MemoryStream(output), enumerateCollection: false);
        }
    }

    public static void WriteVerbose(PowerShellCmdletContext context, string message)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteVerbose(message);
    }

    public static void WriteWarning(PowerShellCmdletContext context, string message)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteWarning(message);
    }

    public static void WriteErrorString(PowerShellCmdletContext context, string message)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteErrorString(message);
    }

    public static void WriteErrorRecordString(PowerShellCmdletContext context, string message, string fullyQualifiedErrorId, int category, object? targetObject)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteError(new PowerShellErrorRecord(
            message,
            fullyQualifiedErrorId,
            (System.Management.Automation.ErrorCategory)category,
            targetObject));
    }

    public static void ThrowTerminatingErrorRecordString(PowerShellCmdletContext context, string message, string fullyQualifiedErrorId, int category, object? targetObject)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.ThrowTerminatingError(new PowerShellErrorRecord(
            message,
            fullyQualifiedErrorId,
            (System.Management.Automation.ErrorCategory)category,
            targetObject));
    }

    public static string GetBoundParameterString(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterString(name);
    }

    public static bool HasBoundParameter(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.HasBoundParameter(name);
    }

    public static bool GetBoundParameterBoolean(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterBoolean(name);
    }

    public static int GetBoundParameterInt32(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterInt32(name);
    }

    public static int GetBoundParameterChar(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterChar(name);
    }

    public static string GetInputString(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetInputObjectString();
    }

    public static string GetCurrentCultureListSeparator(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetCurrentCultureListSeparator();
    }

    public static string GetInputSnapshotJson(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetInputObjectSnapshotJson();
    }

    public static string GetBoundParameterSnapshotJson(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterSnapshotJson(name);
    }

    public static bool ShouldProcess(PowerShellCmdletContext context, string target)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.ShouldProcess(target);
    }

    public static bool ShouldProcess(PowerShellCmdletContext context, string target, string action)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.ShouldProcess(target, action);
    }

    public static bool IsCancellationRequested(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Cancellation.IsCancellationRequested;
    }

    public static int GetLifecycleStateHandle(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.LifecycleStateHandle;
    }

    private static PowerShellCmdletLifecycleState GetLifecycleState(PowerShellCmdletContext context)
    {
        if (context.LifecycleStateHandle == 0)
        {
            throw new InvalidOperationException("PowerShell cmdlet lifecycle state is not available.");
        }

        return Rustlyn.Interop.ManagedInteropRuntime.GetObject<PowerShellCmdletLifecycleState>(context.LifecycleStateHandle);
    }

    private static ICollection<PSObject> Invoke(string commandName, IReadOnlyDictionary<string, object?> parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        ArgumentNullException.ThrowIfNull(parameters);

        using var powershell = PowerShell.Create(RunspaceMode.CurrentRunspace);
        powershell.AddCommand(commandName);
        AddParameters(powershell, parameters);

        var result = powershell.Invoke();
        ThrowIfHadErrors(commandName, powershell);
        return result;
    }

    private static ICollection<PSObject> InvokePipeline(
        string commandName,
        IReadOnlyDictionary<string, object?> parameters,
        IEnumerable<object?> input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(input);

        using var powershell = PowerShell.Create(RunspaceMode.CurrentRunspace);
        powershell.AddCommand(commandName);
        AddParameters(powershell, parameters);

        var result = powershell.Invoke(input);
        ThrowIfHadErrors(commandName, powershell);
        return result;
    }

    private static string InvokeString(string commandName, IReadOnlyDictionary<string, object?> parameters)
    {
        var result = Invoke(commandName, parameters);
        return result.Count == 0
            ? string.Empty
            : string.Concat(result.Select(static item => item.BaseObject?.ToString() ?? string.Empty));
    }

    private static void AddParameters(PowerShell powershell, IReadOnlyDictionary<string, object?> parameters)
    {
        foreach (var (name, value) in parameters)
        {
            if (value is SwitchParameter switchParameter)
            {
                if (switchParameter.IsPresent)
                {
                    powershell.AddParameter(name);
                }

                continue;
            }

            powershell.AddParameter(name, value);
        }
    }

    private static void ThrowIfHadErrors(string commandName, PowerShell powershell)
    {
        if (!powershell.HadErrors)
        {
            return;
        }

        var firstError = powershell.Streams.Error.Count > 0
            ? powershell.Streams.Error[0]
            : new ErrorRecord(
                new RuntimeException($"PowerShell command '{commandName}' failed."),
                "RustlynNestedCommandFailed",
                ErrorCategory.NotSpecified,
                null);
        throw new RuntimeException(firstError.ToString(), firstError.Exception);
    }
}
