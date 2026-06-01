namespace Rustlyn.PowerShellSupport;

public sealed class PowerShellCmdletLifecycleState
{
    private readonly List<object?> _xmlInputs = [];
    private readonly List<string> _xmlTextInputs = [];
    private readonly List<byte[]> _pendingXmlStreams = [];

    public IReadOnlyList<object?> XmlInputs => _xmlInputs;
    public IReadOnlyList<string> XmlTextInputs => _xmlTextInputs;

    public void AddXmlInput(object? value)
        => _xmlInputs.Add(value);

    public void ClearXmlInputs()
        => _xmlInputs.Clear();

    public void AddXmlTextInput(string value)
        => _xmlTextInputs.Add(value);

    public void ClearXmlTextInputs()
        => _xmlTextInputs.Clear();

    public void AddPendingXmlStream(byte[] value)
        => _pendingXmlStreams.Add(value);

    public IReadOnlyList<byte[]> DrainPendingXmlStreams()
    {
        var outputs = _pendingXmlStreams.ToArray();
        _pendingXmlStreams.Clear();
        return outputs;
    }
}
