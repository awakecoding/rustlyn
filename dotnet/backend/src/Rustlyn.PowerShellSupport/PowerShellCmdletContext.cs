using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;

namespace Rustlyn.PowerShellSupport;

public sealed class PowerShellCmdletContext
{
    private readonly Action<object?> _writeObject;
    private readonly Action<string> _writeVerbose;
    private readonly Action<string> _writeWarning;
    private readonly Action<string> _writeErrorString;
    private readonly Func<string, bool> _shouldProcess;
    private readonly IReadOnlyDictionary<string, object?> _boundParameters;

    public PowerShellCmdletContext(
        Action<object?> writeObject,
        Action<string> writeVerbose,
        Action<string> writeWarning,
        Action<string> writeErrorString,
        Func<string, bool> shouldProcess,
        IReadOnlyDictionary<string, object?>? boundParameters,
        object? inputObject)
    {
        ArgumentNullException.ThrowIfNull(writeObject);
        ArgumentNullException.ThrowIfNull(writeVerbose);
        ArgumentNullException.ThrowIfNull(writeWarning);
        ArgumentNullException.ThrowIfNull(writeErrorString);
        ArgumentNullException.ThrowIfNull(shouldProcess);

        _writeObject = writeObject;
        _writeVerbose = writeVerbose;
        _writeWarning = writeWarning;
        _writeErrorString = writeErrorString;
        _shouldProcess = shouldProcess;
        _boundParameters = boundParameters is null
            ? ReadOnlyDictionary<string, object?>.Empty
            : new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>(boundParameters, StringComparer.OrdinalIgnoreCase));
        InputObject = inputObject;
    }

    public object? InputObject { get; }

    public void WriteObject(object? value)
        => _writeObject(value);

    public void WriteVerbose(string message)
        => _writeVerbose(message);

    public void WriteWarning(string message)
        => _writeWarning(message);

    public void WriteErrorString(string message)
        => _writeErrorString(message);

    public bool ShouldProcess(string target)
        => _shouldProcess(target);

    public string GetInputObjectString()
        => ConvertToInvariantString(InputObject);

    public string GetBoundParameterString(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _boundParameters.TryGetValue(name, out var value)
            ? ConvertToInvariantString(value)
            : string.Empty;
    }

    private static string ConvertToInvariantString(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (value is string text)
        {
            return text;
        }

        if (value is PSObject psObject)
        {
            return ConvertToInvariantString(psObject.BaseObject);
        }

        return value is IFormattable formattable
            ? formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty
            : value.ToString() ?? string.Empty;
    }
}
