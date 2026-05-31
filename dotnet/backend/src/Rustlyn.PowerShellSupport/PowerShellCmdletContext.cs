using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;

namespace Rustlyn.PowerShellSupport;

public sealed class PowerShellCmdletContext
{
    private readonly Action<object?> _writeObject;
    private readonly Action<object?, bool> _writeObjectEnumerated;
    private readonly Action<string> _writeVerbose;
    private readonly Action<string> _writeWarning;
    private readonly Action<string> _writeErrorString;
    private readonly Action<ErrorRecord> _writeError;
    private readonly Action<ErrorRecord> _throwTerminatingError;
    private readonly Func<string, bool> _shouldProcess;
    private readonly Func<string, string?, bool> _shouldProcessWithAction;
    private readonly IReadOnlyDictionary<string, object?> _boundParameters;

    public PowerShellCmdletContext(
        Action<object?> writeObject,
        Action<string> writeVerbose,
        Action<string> writeWarning,
        Action<string> writeErrorString,
        Func<string, bool> shouldProcess,
        IReadOnlyDictionary<string, object?>? boundParameters,
        object? inputObject)
        : this(
            writeObject,
            (value, _) => writeObject(value),
            writeVerbose,
            writeWarning,
            writeErrorString,
            errorRecord => writeErrorString(errorRecord.ToString()),
            errorRecord => throw errorRecord.Exception,
            shouldProcess,
            (target, _) => shouldProcess(target),
            boundParameters,
            inputObject,
            new PowerShellCmdletCancellation(),
            lifecycleStateHandle: 0)
    {
    }

    public PowerShellCmdletContext(
        Action<object?> writeObject,
        Action<object?, bool> writeObjectEnumerated,
        Action<string> writeVerbose,
        Action<string> writeWarning,
        Action<string> writeErrorString,
        Action<ErrorRecord> writeError,
        Action<ErrorRecord> throwTerminatingError,
        Func<string, bool> shouldProcess,
        Func<string, string?, bool> shouldProcessWithAction,
        IReadOnlyDictionary<string, object?>? boundParameters,
        object? inputObject,
        PowerShellCmdletCancellation cancellation,
        int lifecycleStateHandle = 0)
    {
        ArgumentNullException.ThrowIfNull(writeObject);
        ArgumentNullException.ThrowIfNull(writeObjectEnumerated);
        ArgumentNullException.ThrowIfNull(writeVerbose);
        ArgumentNullException.ThrowIfNull(writeWarning);
        ArgumentNullException.ThrowIfNull(writeErrorString);
        ArgumentNullException.ThrowIfNull(writeError);
        ArgumentNullException.ThrowIfNull(throwTerminatingError);
        ArgumentNullException.ThrowIfNull(shouldProcess);
        ArgumentNullException.ThrowIfNull(shouldProcessWithAction);
        ArgumentNullException.ThrowIfNull(cancellation);

        _writeObject = writeObject;
        _writeObjectEnumerated = writeObjectEnumerated;
        _writeVerbose = writeVerbose;
        _writeWarning = writeWarning;
        _writeErrorString = writeErrorString;
        _writeError = writeError;
        _throwTerminatingError = throwTerminatingError;
        _shouldProcess = shouldProcess;
        _shouldProcessWithAction = shouldProcessWithAction;
        _boundParameters = boundParameters is null
            ? ReadOnlyDictionary<string, object?>.Empty
            : new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>(boundParameters, StringComparer.OrdinalIgnoreCase));
        InputObject = inputObject;
        Cancellation = cancellation;
        LifecycleStateHandle = lifecycleStateHandle;
    }

    public object? InputObject { get; }

    public PowerShellCmdletCancellation Cancellation { get; }

    public int LifecycleStateHandle { get; }

    public void WriteObject(object? value)
        => _writeObject(value);

    public void WriteObject(object? value, bool enumerateCollection)
        => _writeObjectEnumerated(value, enumerateCollection);

    public void WriteVerbose(string message)
        => _writeVerbose(message);

    public void WriteWarning(string message)
        => _writeWarning(message);

    public void WriteErrorString(string message)
        => _writeErrorString(message);

    public void WriteError(PowerShellErrorRecord errorRecord)
    {
        ArgumentNullException.ThrowIfNull(errorRecord);
        _writeError(errorRecord.ToErrorRecord());
    }

    public void ThrowTerminatingError(PowerShellErrorRecord errorRecord)
    {
        ArgumentNullException.ThrowIfNull(errorRecord);
        _throwTerminatingError(errorRecord.ToErrorRecord());
    }

    public bool ShouldProcess(string target)
        => _shouldProcess(target);

    public bool ShouldProcess(string target, string? action)
        => string.IsNullOrWhiteSpace(action)
            ? _shouldProcess(target)
            : _shouldProcessWithAction(target, action);

    public string GetInputObjectString()
        => ConvertToInvariantString(InputObject);

    public string GetInputObjectSnapshotJson(int maxDepth = 8)
        => PowerShellObjectSnapshot.ToJson(InputObject, maxDepth);

    public string GetCurrentCultureListSeparator()
        => CultureInfo.CurrentCulture.TextInfo.ListSeparator;

    public object? GetBoundParameterObject(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _boundParameters.TryGetValue(name, out var value)
            ? value
            : null;
    }

    public bool HasBoundParameter(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _boundParameters.ContainsKey(name);
    }

    public string GetBoundParameterString(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _boundParameters.TryGetValue(name, out var value)
            ? ConvertToInvariantString(value)
            : string.Empty;
    }

    public bool GetBoundParameterBoolean(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _boundParameters.TryGetValue(name, out var value)
            && ConvertToBoolean(value);
    }

    public int GetBoundParameterInt32(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return _boundParameters.TryGetValue(name, out var value)
            ? ConvertToInt32(value)
            : 0;
    }

    public int GetBoundParameterChar(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!_boundParameters.TryGetValue(name, out var value))
        {
            return 0;
        }

        value = UnwrapPowerShellObject(value);
        return value switch
        {
            null => 0,
            char character => character,
            string { Length: > 0 } text => text[0],
            _ => Convert.ToChar(value, CultureInfo.InvariantCulture)
        };
    }

    public string GetBoundParameterSnapshotJson(string name, int maxDepth = 8)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return PowerShellObjectSnapshot.ToJson(GetBoundParameterObject(name), maxDepth);
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

        value = UnwrapPowerShellObject(value);
        if (value is null)
        {
            return string.Empty;
        }

        return value is IFormattable formattable
            ? formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty
            : value.ToString() ?? string.Empty;
    }

    private static bool ConvertToBoolean(object? value)
    {
        value = UnwrapPowerShellObject(value);
        return value switch
        {
            null => false,
            SwitchParameter switchParameter => switchParameter.IsPresent,
            bool boolean => boolean,
            _ => Convert.ToBoolean(value, CultureInfo.InvariantCulture)
        };
    }

    private static int ConvertToInt32(object? value)
    {
        value = UnwrapPowerShellObject(value);
        return value switch
        {
            null => 0,
            int integer => integer,
            _ => Convert.ToInt32(value, CultureInfo.InvariantCulture)
        };
    }

    private static object? UnwrapPowerShellObject(object? value)
        => value is PSObject psObject && !ReferenceEquals(psObject.BaseObject, psObject)
            ? psObject.BaseObject
            : value;
}
