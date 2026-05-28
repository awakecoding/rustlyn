namespace Rustlyn.PowerShellSupport;

public static class PowerShellCmdletBridge
{
    public static void WriteObject(PowerShellCmdletContext context, string value)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteObject(value);
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

    public static string GetBoundParameterString(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterString(name);
    }

    public static string GetInputString(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetInputObjectString();
    }

    public static bool ShouldProcess(PowerShellCmdletContext context, string target)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.ShouldProcess(target);
    }
}
