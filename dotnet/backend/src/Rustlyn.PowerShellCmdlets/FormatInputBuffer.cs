namespace Rustlyn.PowerShellCmdlets;

internal sealed class FormatInputBuffer
{
    private readonly List<object?> _items = [];

    public int Count => _items.Count;

    public IReadOnlyList<object?> Items => _items;

    public void Add(object? value)
        => _items.Add(value);

    public object? ToPowerShellInput()
        => _items.Count switch
        {
            0 => null,
            1 => _items[0],
            _ => _items.ToArray()
        };

    public string ToText()
    {
        var lines = new List<string>();
        foreach (var item in _items)
        {
            if (item is string text)
            {
                lines.Add(text);
                continue;
            }

            if (item is IEnumerable<string> textItems)
            {
                lines.AddRange(textItems);
                continue;
            }

            lines.Add(item?.ToString() ?? string.Empty);
        }

        return string.Join(Environment.NewLine, lines);
    }
}
