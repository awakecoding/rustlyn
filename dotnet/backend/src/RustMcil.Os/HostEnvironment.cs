namespace RustMcil.Os;

public static class HostEnvironment
{
    public static int CommandLineArgCount()
    {
        return Environment.GetCommandLineArgs().Length;
    }
}
