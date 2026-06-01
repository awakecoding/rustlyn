using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using System.Collections.Concurrent;
using Rustlyn.Interop;

namespace Rustlyn.PowerShellSupport;

public static class PowerShellGeneratedCmdletInvoker
{
    private static readonly Dictionary<string, Assembly> LoadedAssemblies = new(StringComparer.OrdinalIgnoreCase);
    private static readonly ConcurrentDictionary<LifecycleMethodCacheKey, LifecycleMethodInvoker> LifecycleMethodCache = new();

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
        var enginePath = ResolveEnginePath(engineFileName);
        var cacheKey = new LifecycleMethodCacheKey(enginePath, typeName, methodName);
        var method = LifecycleMethodCache.GetOrAdd(cacheKey, static key => CreateLifecycleMethodInvoker(key));
        try
        {
            method.Invoke(contextHandle);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
    }

    private static LifecycleMethodInvoker CreateLifecycleMethodInvoker(LifecycleMethodCacheKey key)
    {
        var assembly = LoadEngineAssembly(key.EnginePath);
        var type = assembly.GetType(key.TypeName, throwOnError: true)
            ?? throw new TypeLoadException($"Rust PowerShell engine '{Path.GetFileName(key.EnginePath)}' does not contain type '{key.TypeName}'.");
        var method = type.GetMethod(key.MethodName, BindingFlags.Public | BindingFlags.Static)
            ?? throw new MissingMethodException(type.FullName, key.MethodName);
        var parameters = method.GetParameters();
        if (parameters.Length != 1 || parameters[0].ParameterType != typeof(int))
        {
            throw new MissingMethodException(type.FullName, $"{key.MethodName}(int cmdletContextHandle)");
        }

        if (method.ReturnType == typeof(void))
        {
            var action = method.CreateDelegate<Action<int>>();
            return new LifecycleMethodInvoker(
                key.TypeName,
                key.MethodName,
                invoke: contextHandle =>
                {
                    action(contextHandle);
                    return 0;
                });
        }

        if (method.ReturnType == typeof(int))
        {
            var func = method.CreateDelegate<Func<int, int>>();
            return new LifecycleMethodInvoker(key.TypeName, key.MethodName, func);
        }

        throw new InvalidOperationException($"Rust PowerShell lifecycle method '{type.FullName}.{key.MethodName}' must return void or int.");
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

    private readonly record struct LifecycleMethodCacheKey(string EnginePath, string TypeName, string MethodName);

    private sealed class LifecycleMethodInvoker(string typeName, string methodName, Func<int, int> invoke)
    {
        public void Invoke(int contextHandle)
        {
            var status = invoke(contextHandle);
            if (status != 0)
            {
                throw new InvalidOperationException($"Rust PowerShell lifecycle method '{typeName}.{methodName}' failed with status {status.ToString(CultureInfo.InvariantCulture)}.");
            }
        }
    }
}
