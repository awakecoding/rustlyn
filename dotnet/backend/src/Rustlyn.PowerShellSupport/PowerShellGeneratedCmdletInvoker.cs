using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using Rustlyn.Interop;

namespace Rustlyn.PowerShellSupport;

public static class PowerShellGeneratedCmdletInvoker
{
    private static readonly Dictionary<string, Assembly> LoadedAssemblies = new(StringComparer.OrdinalIgnoreCase);

    static PowerShellGeneratedCmdletInvoker()
    {
        AssemblyLoadContext.Default.Resolving += ResolveModuleAssembly;
    }

    public static int CreateLifecycleStateHandle()
        => ManagedInteropRuntime.AddObjectHandle(new PowerShellCmdletLifecycleState());

    public static void ReleaseLifecycleStateHandle(int handle)
    {
        if (handle == 0)
        {
            return;
        }

        if (!ManagedInteropRuntime.ReleaseObject(handle))
        {
            throw new InvalidOperationException($"PowerShell cmdlet lifecycle state handle {handle.ToString(CultureInfo.InvariantCulture)} was not active.");
        }
    }

    public static void InvokeLifecycle(
        string engineFileName,
        string typeName,
        string methodName,
        PowerShellCmdletContext context,
        bool checkCancellation = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName);
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        ArgumentNullException.ThrowIfNull(context);

        if (checkCancellation)
        {
            context.Cancellation.ThrowIfCancellationRequested();
        }
        var contextHandle = ManagedInteropRuntime.AddObjectHandle(context);
        try
        {
            InvokeLifecycleMethod(engineFileName, typeName, methodName, contextHandle);
        }
        finally
        {
            ManagedInteropRuntime.ReleaseObject(contextHandle);
        }
    }

    private static void InvokeLifecycleMethod(string engineFileName, string typeName, string methodName, int contextHandle)
    {
        var assembly = LoadEngineAssembly(engineFileName);
        var type = assembly.GetType(typeName, throwOnError: true)
            ?? throw new TypeLoadException($"Rust PowerShell engine '{engineFileName}' does not contain type '{typeName}'.");
        var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
            ?? throw new MissingMethodException(type.FullName, methodName);
        var parameters = method.GetParameters();
        if (parameters.Length != 1 || parameters[0].ParameterType != typeof(int))
        {
            throw new MissingMethodException(type.FullName, $"{methodName}(int cmdletContextHandle)");
        }

        object? result;
        try
        {
            result = method.Invoke(null, [contextHandle]);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }

        if (method.ReturnType == typeof(void))
        {
            return;
        }

        if (method.ReturnType != typeof(int))
        {
            throw new InvalidOperationException($"Rust PowerShell lifecycle method '{type.FullName}.{methodName}' must return void or int.");
        }

        var status = Convert.ToInt32(result, CultureInfo.InvariantCulture);
        if (status != 0)
        {
            throw new InvalidOperationException($"Rust PowerShell lifecycle method '{type.FullName}.{methodName}' failed with status {status.ToString(CultureInfo.InvariantCulture)}.");
        }
    }

    private static Assembly LoadEngineAssembly(string engineFileName)
    {
        var enginePath = ResolveEnginePath(engineFileName);
        if (!File.Exists(enginePath))
        {
            throw new FileNotFoundException($"Rust PowerShell engine assembly '{engineFileName}' was not found in the module folder.", enginePath);
        }

        lock (LoadedAssemblies)
        {
            if (!LoadedAssemblies.TryGetValue(enginePath, out var assembly))
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(enginePath);
                LoadedAssemblies.Add(enginePath, assembly);
            }

            return assembly;
        }
    }

    private static string ResolveEnginePath(string engineFileName)
    {
        var moduleDirectory = Path.GetDirectoryName(typeof(PowerShellGeneratedCmdletInvoker).Assembly.Location)
            ?? throw new InvalidOperationException("PowerShell support assembly location could not be determined.");
        var enginePath = Path.Combine(moduleDirectory, engineFileName);
        if (File.Exists(enginePath))
        {
            return enginePath;
        }

        var parentDirectory = Directory.GetParent(moduleDirectory)?.FullName;
        if (parentDirectory is not null)
        {
            foreach (var siblingDirectory in Directory.EnumerateDirectories(parentDirectory, "*_powershell"))
            {
                var siblingEnginePath = Path.Combine(siblingDirectory, engineFileName);
                if (File.Exists(siblingEnginePath))
                {
                    return siblingEnginePath;
                }
            }
        }

        return enginePath;
    }

    private static Assembly? ResolveModuleAssembly(AssemblyLoadContext context, AssemblyName assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName.Name))
        {
            return null;
        }

        var candidatePath = Path.Combine(GetModuleDirectory(), assemblyName.Name + ".dll");
        return File.Exists(candidatePath)
            ? context.LoadFromAssemblyPath(candidatePath)
            : null;
    }

    private static string GetModuleDirectory()
        => Path.GetDirectoryName(typeof(PowerShellGeneratedCmdletInvoker).Assembly.Location)
            ?? throw new InvalidOperationException("PowerShell support assembly location could not be determined.");
}
