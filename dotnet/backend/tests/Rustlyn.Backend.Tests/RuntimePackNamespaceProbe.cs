namespace Rustlyn.Backend.Tests.Projections;

public sealed class RuntimePackNamespaceProbe
{
    public string Echo(string value)
        => value;

    public System.Threading.Tasks.Task<int> AsyncEcho(int value)
        => System.Threading.Tasks.Task.FromResult(value);
}

public static class RuntimePackStaticProbe
{
    public static int Add(int x, int y)
        => x + y;
}
