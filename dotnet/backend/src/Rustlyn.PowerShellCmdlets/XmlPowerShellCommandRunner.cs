using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

internal static class XmlPowerShellCommandRunner
{
    public static Collection<PSObject> Invoke(string commandName, IReadOnlyDictionary<string, object?> parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        ArgumentNullException.ThrowIfNull(parameters);

        using var powershell = PowerShell.Create(RunspaceMode.CurrentRunspace);
        powershell.AddCommand(commandName);
        AddParameters(powershell, parameters);

        var result = powershell.Invoke();
        if (powershell.HadErrors)
        {
            var firstError = powershell.Streams.Error.Count > 0
                ? powershell.Streams.Error[0]
                : new ErrorRecord(
                    new RuntimeException($"PowerShell command '{commandName}' failed."),
                    "RustlynNestedCommandFailed",
                    ErrorCategory.NotSpecified,
                    null);
            throw new RuntimeException(firstError.ToString(), firstError.Exception);
        }

        return result;
    }

    public static Collection<PSObject> InvokePipeline(
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
        if (powershell.HadErrors)
        {
            var firstError = powershell.Streams.Error.Count > 0
                ? powershell.Streams.Error[0]
                : new ErrorRecord(
                    new RuntimeException($"PowerShell command '{commandName}' failed."),
                    "RustlynNestedCommandFailed",
                    ErrorCategory.NotSpecified,
                    null);
            throw new RuntimeException(firstError.ToString(), firstError.Exception);
        }

        return result;
    }

    public static string InvokeString(string commandName, IReadOnlyDictionary<string, object?> parameters)
    {
        var result = Invoke(commandName, parameters);
        if (result.Count == 0)
        {
            return string.Empty;
        }

        return string.Concat(result.Select(static item => item.BaseObject?.ToString() ?? string.Empty));
    }

    public static Collection<PSObject> InvokeWithPreferredInputParameter(string commandName, IReadOnlyList<string> parameterNames, object? value)
    {
        var parameterName = ResolveFirstAvailableParameter(commandName, parameterNames);
        return Invoke(commandName, new Dictionary<string, object?> { [parameterName] = value });
    }

    public static string InvokeStringWithPreferredInputParameter(string commandName, IReadOnlyList<string> parameterNames, object? value)
    {
        var result = InvokeWithPreferredInputParameter(commandName, parameterNames, value);
        return result.Count == 0
            ? string.Empty
            : string.Concat(result.Select(static item => item.BaseObject?.ToString() ?? string.Empty));
    }

    public static object? UnwrapOutputObject(PSObject? result)
        => result is null
            ? null
            : result.BaseObject is PSCustomObject
            ? result
            : result.BaseObject;

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

    private static string ResolveFirstAvailableParameter(string commandName, IReadOnlyList<string> parameterNames)
    {
        using var powershell = PowerShell.Create(RunspaceMode.CurrentRunspace);
        var commands = powershell.AddCommand("Microsoft.PowerShell.Core\\Get-Command")
            .AddArgument(commandName)
            .Invoke<CommandInfo>();
        var command = commands.Count == 0
            ? throw new CommandNotFoundException($"Command '{commandName}' was not found.")
            : commands[0];

        foreach (var parameterName in parameterNames)
        {
            if (command.Parameters.ContainsKey(parameterName))
            {
                return parameterName;
            }
        }

        throw new ParameterBindingException(
            $"Command '{commandName}' does not expose any supported input parameter: {string.Join(", ", parameterNames)}.");
    }
}
