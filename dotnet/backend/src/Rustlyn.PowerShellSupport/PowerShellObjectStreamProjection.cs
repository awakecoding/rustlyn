using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Text;

namespace Rustlyn.PowerShellSupport;

internal static class PowerShellObjectStreamProjection
{
    public static void WriteFromObjectStream(PowerShellCmdletContext context, string stream, bool asHashtable, bool noEnumerate)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(stream);

        var reader = new ObjectStreamReader(Encoding.UTF8.GetBytes(stream), asHashtable);
        var projected = reader.ReadValue();
        reader.EnsureEnd();

        if (!noEnumerate && projected is object?[] values)
        {
            foreach (var value in values)
            {
                context.WriteObject(value, enumerateCollection: false);
            }

            return;
        }

        context.WriteObject(projected, enumerateCollection: false);
    }

    private sealed class ObjectStreamReader(byte[] input, bool asHashtable)
    {
        private readonly byte[] _input = input;
        private readonly bool _asHashtable = asHashtable;
        private int _offset;

        public object? ReadValue()
        {
            return ReadChar() switch
            {
                'N' => ReadNull(),
                'T' => ReadBoolean(true),
                'F' => ReadBoolean(false),
                'I' => ReadInteger(),
                'D' => ReadDecimalOrDouble(),
                'S' => ReadString(),
                'A' => ReadArray(),
                'O' => ReadObject(),
                _ => throw CreateParseException()
            };
        }

        public void EnsureEnd()
        {
            if (_offset != _input.Length)
            {
                throw CreateParseException();
            }
        }

        private object? ReadNull()
        {
            Expect(';');
            return null;
        }

        private bool ReadBoolean(bool value)
        {
            Expect(';');
            return value;
        }

        private object ReadInteger()
        {
            var value = ReadNumberText();
            Expect(';');

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
            {
                return intValue;
            }

            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
            {
                return longValue;
            }

            if (decimal.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var decimalValue))
            {
                return decimalValue;
            }

            throw CreateParseException();
        }

        private object ReadDecimalOrDouble()
        {
            var value = ReadNumberText();
            Expect(';');

            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var decimalValue))
            {
                return decimalValue;
            }

            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
            {
                return doubleValue;
            }

            throw CreateParseException();
        }

        private string ReadString()
            => ReadRawString();

        private object?[] ReadArray()
        {
            var count = ReadCount();
            var values = new object?[count];
            for (var index = 0; index < count; index++)
            {
                values[index] = ReadValue();
            }

            return values;
        }

        private object ReadObject()
        {
            var count = ReadCount();
            if (_asHashtable)
            {
                var table = new Hashtable(StringComparer.OrdinalIgnoreCase);
                for (var index = 0; index < count; index++)
                {
                    table[ReadRawString()] = ReadValue();
                }

                return table;
            }

            var psObject = new PSObject();
            for (var index = 0; index < count; index++)
            {
                psObject.Properties.Add(new PSNoteProperty(ReadRawString(), ReadValue()));
            }

            return psObject;
        }

        private int ReadCount()
        {
            var count = ReadUnsignedInteger();
            Expect(':');
            return count;
        }

        private string ReadRawString()
        {
            var length = ReadUnsignedInteger();
            Expect(':');

            var end = checked(_offset + length);
            if (end > _input.Length)
            {
                throw CreateParseException();
            }

            var value = Encoding.UTF8.GetString(_input, _offset, length);
            _offset = end;
            return value;
        }

        private int ReadUnsignedInteger()
        {
            var start = _offset;
            var value = 0;
            while (_offset < _input.Length && char.IsAsciiDigit((char)_input[_offset]))
            {
                value = checked((value * 10) + (_input[_offset] - (byte)'0'));
                _offset++;
            }

            if (_offset == start)
            {
                throw CreateParseException();
            }

            return value;
        }

        private string ReadNumberText()
        {
            var start = _offset;
            while (_offset < _input.Length)
            {
                var ch = (char)_input[_offset];
                if (ch == ';')
                {
                    break;
                }

                if (!char.IsAsciiDigit(ch) && ch is not '-' and not '+' and not '.' and not 'E' and not 'e')
                {
                    throw CreateParseException();
                }

                _offset++;
            }

            if (_offset == start)
            {
                throw CreateParseException();
            }

            return Encoding.ASCII.GetString(_input, start, _offset - start);
        }

        private char ReadChar()
        {
            if (_offset >= _input.Length)
            {
                throw CreateParseException();
            }

            return (char)_input[_offset++];
        }

        private void Expect(char expected)
        {
            if (ReadChar() != expected)
            {
                throw CreateParseException();
            }
        }

        private InvalidOperationException CreateParseException()
            => new("Invalid PowerShell object stream.");
    }
}