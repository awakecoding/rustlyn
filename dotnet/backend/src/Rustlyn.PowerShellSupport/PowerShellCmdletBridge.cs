namespace Rustlyn.PowerShellSupport;

public static class PowerShellCmdletBridge
{
    public static void WriteObject(PowerShellCmdletContext context, string value)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteObject(value);
    }

    public static void WriteObject(PowerShellCmdletContext context, object? value, bool enumerateCollection)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteObject(value, enumerateCollection);
    }

    public static void WriteObjectBytes(PowerShellCmdletContext context, IntPtr bytesPointer, long byteLength)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentOutOfRangeException.ThrowIfNegative(byteLength);
        if (byteLength > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(byteLength), "PowerShell byte output exceeds the maximum managed array length.");
        }
        if (byteLength > 0 && bytesPointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(bytesPointer));
        }

        var bytes = new byte[(int)byteLength];
        if (bytes.Length > 0)
        {
            System.Runtime.InteropServices.Marshal.Copy(bytesPointer, bytes, 0, bytes.Length);
        }

        context.WriteObject(bytes, enumerateCollection: false);
    }

    public static void WriteJson(PowerShellCmdletContext context, string json, bool asHashtable, bool noEnumerate)
    {
        ArgumentNullException.ThrowIfNull(context);
        PowerShellJsonProjection.WriteFromJson(context, json, asHashtable, noEnumerate);
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

    public static void WriteErrorRecordString(PowerShellCmdletContext context, string message, string fullyQualifiedErrorId, int category, object? targetObject)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.WriteError(new PowerShellErrorRecord(
            message,
            fullyQualifiedErrorId,
            (System.Management.Automation.ErrorCategory)category,
            targetObject));
    }

    public static void ThrowTerminatingErrorRecordString(PowerShellCmdletContext context, string message, string fullyQualifiedErrorId, int category, object? targetObject)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.ThrowTerminatingError(new PowerShellErrorRecord(
            message,
            fullyQualifiedErrorId,
            (System.Management.Automation.ErrorCategory)category,
            targetObject));
    }

    public static string GetBoundParameterString(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterString(name);
    }

    public static bool HasBoundParameter(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.HasBoundParameter(name);
    }

    public static bool GetBoundParameterBoolean(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterBoolean(name);
    }

    public static int GetBoundParameterInt32(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterInt32(name);
    }

    public static int GetBoundParameterChar(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterChar(name);
    }

    public static string GetInputString(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetInputObjectString();
    }

    public static string GetCurrentCultureListSeparator(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetCurrentCultureListSeparator();
    }

    public static string GetInputSnapshotJson(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetInputObjectSnapshotJson();
    }

    public static string GetBoundParameterSnapshotJson(PowerShellCmdletContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetBoundParameterSnapshotJson(name);
    }

    public static bool ShouldProcess(PowerShellCmdletContext context, string target)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.ShouldProcess(target);
    }

    public static bool ShouldProcess(PowerShellCmdletContext context, string target, string action)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.ShouldProcess(target, action);
    }

    public static bool IsCancellationRequested(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Cancellation.IsCancellationRequested;
    }

    public static int GetLifecycleStateHandle(PowerShellCmdletContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.LifecycleStateHandle;
    }
}
