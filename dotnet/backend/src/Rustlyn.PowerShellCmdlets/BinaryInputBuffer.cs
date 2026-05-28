using System.Collections;
using System.Management.Automation;

namespace Rustlyn.PowerShellCmdlets;

internal sealed class BinaryInputBuffer
{
    private readonly List<byte> _bytes = [];

    public void Add(object? value)
    {
        switch (value)
        {
            case null:
                return;
            case PSObject psObject when !ReferenceEquals(psObject.BaseObject, psObject):
                Add(psObject.BaseObject);
                return;
            case byte byteValue:
                _bytes.Add(byteValue);
                return;
            case byte[] byteArray:
                _bytes.AddRange(byteArray);
                return;
            case IEnumerable<byte> byteItems:
                _bytes.AddRange(byteItems);
                return;
            case IEnumerable enumerable and not string:
                foreach (var item in enumerable)
                {
                    Add(item);
                }
                return;
            default:
                throw new ArgumentException($"Expected BSON/CBOR input as byte[] or pipeline bytes, but received '{value.GetType().FullName}'.", nameof(value));
        }
    }

    public byte[] ToArray()
        => _bytes.ToArray();
}
