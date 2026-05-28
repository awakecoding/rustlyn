using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Rustlyn.PowerShellCmdlets;

internal static class RustEngineInvoker
{
    private static readonly Dictionary<string, Assembly> LoadedAssemblies = new(StringComparer.OrdinalIgnoreCase);

    static RustEngineInvoker()
    {
        AssemblyLoadContext.Default.Resolving += ResolveModuleAssembly;
    }

    public static string TransformUtf8(string engineFileName, string lengthMethodName, string copyMethodName, string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lengthMethodName);
        ArgumentException.ThrowIfNullOrWhiteSpace(copyMethodName);
        ArgumentNullException.ThrowIfNull(input);

        var inputBytes = Encoding.UTF8.GetBytes(input);
        var outputBytes = TransformBytes(engineFileName, lengthMethodName, copyMethodName, inputBytes);
        return Encoding.UTF8.GetString(outputBytes);
    }

    public static byte[] TransformUtf8ToBytes(string engineFileName, string lengthMethodName, string copyMethodName, string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lengthMethodName);
        ArgumentException.ThrowIfNullOrWhiteSpace(copyMethodName);
        ArgumentNullException.ThrowIfNull(input);

        return TransformBytes(engineFileName, lengthMethodName, copyMethodName, Encoding.UTF8.GetBytes(input));
    }

    public static string TransformBytesToUtf8(string engineFileName, string lengthMethodName, string copyMethodName, byte[] inputBytes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lengthMethodName);
        ArgumentException.ThrowIfNullOrWhiteSpace(copyMethodName);
        ArgumentNullException.ThrowIfNull(inputBytes);

        var outputBytes = TransformBytes(engineFileName, lengthMethodName, copyMethodName, inputBytes);
        return Encoding.UTF8.GetString(outputBytes);
    }

    public static void ValidateBytes(string engineFileName, string validateMethodName, byte[] inputBytes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(validateMethodName);
        ArgumentNullException.ThrowIfNull(inputBytes);

        var status = InvokeInt32(engineFileName, validateMethodName, inputBytes);
        if (status != 1)
        {
            throw new InvalidDataException($"Rust engine '{engineFileName}' rejected the input with status {status}.");
        }
    }

    private static byte[] TransformBytes(string engineFileName, string lengthMethodName, string copyMethodName, byte[] inputBytes)
    {
        var length = InvokeInt64(engineFileName, lengthMethodName, inputBytes);
        if (length < 0)
        {
            throw new InvalidDataException($"Rust engine '{engineFileName}' rejected the input with status {length}.");
        }

        if (length == 0)
        {
            return [];
        }

        if (length > int.MaxValue)
        {
            throw new InvalidDataException($"Rust engine '{engineFileName}' produced {length} bytes, which exceeds the supported managed buffer size.");
        }

        var outputBytes = new byte[(int)length];
        var copied = InvokeCopy(engineFileName, copyMethodName, inputBytes, outputBytes);
        if (copied < 0)
        {
            throw new InvalidDataException($"Rust engine '{engineFileName}' failed to copy output with status {copied}.");
        }

        if (copied > outputBytes.Length)
        {
            throw new InvalidDataException($"Rust engine '{engineFileName}' required {copied} output bytes after reporting {length}.");
        }

        if (copied == outputBytes.Length)
        {
            return outputBytes;
        }

        return outputBytes[..(int)copied];
    }

    public static void ValidateUtf8(string engineFileName, string validateMethodName, string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(validateMethodName);
        ArgumentNullException.ThrowIfNull(input);

        var inputBytes = Encoding.UTF8.GetBytes(input);
        var status = InvokeInt32(engineFileName, validateMethodName, inputBytes);
        if (status != 1)
        {
            throw new InvalidDataException($"Rust engine '{engineFileName}' rejected the input with status {status}.");
        }
    }

    private static unsafe int InvokeInt32(string engineFileName, string methodName, byte[] inputBytes)
    {
        fixed (byte* input = inputBytes)
        {
            var inputPointer = inputBytes.Length == 0 ? IntPtr.Zero : (IntPtr)input;
            return Convert.ToInt32(Invoke(engineFileName, methodName, inputPointer, (long)inputBytes.Length), System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    private static unsafe long InvokeInt64(string engineFileName, string methodName, byte[] inputBytes)
    {
        fixed (byte* input = inputBytes)
        {
            var inputPointer = inputBytes.Length == 0 ? IntPtr.Zero : (IntPtr)input;
            return Convert.ToInt64(Invoke(engineFileName, methodName, inputPointer, (long)inputBytes.Length), System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    private static unsafe long InvokeCopy(string engineFileName, string methodName, byte[] inputBytes, byte[] outputBytes)
    {
        fixed (byte* input = inputBytes)
        fixed (byte* output = outputBytes)
        {
            var inputPointer = inputBytes.Length == 0 ? IntPtr.Zero : (IntPtr)input;
            var outputPointer = outputBytes.Length == 0 ? IntPtr.Zero : (IntPtr)output;
            return Convert.ToInt64(
                Invoke(engineFileName, methodName, inputPointer, (long)inputBytes.Length, outputPointer, (long)outputBytes.Length),
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    private static object? Invoke(string engineFileName, string methodName, params object[] arguments)
    {
        var type = LoadGeneratedModule(engineFileName);
        var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
            ?? throw new MissingMethodException(type.FullName, methodName);
        return method.Invoke(null, arguments);
    }

    private static Type LoadGeneratedModule(string engineFileName)
    {
        var assembly = LoadEngineAssembly(engineFileName);
        return assembly.GetType("Rustlyn.GeneratedModule", throwOnError: true)
            ?? throw new TypeLoadException($"Rust engine '{engineFileName}' does not contain Rustlyn.GeneratedModule.");
    }

    private static Assembly LoadEngineAssembly(string engineFileName)
    {
        var enginePath = ResolveEnginePath(engineFileName);
        if (!File.Exists(enginePath))
        {
            throw new FileNotFoundException($"Rust engine assembly '{engineFileName}' was not found in the PowerShell module folder or sibling generated module folders.", enginePath);
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
        var moduleDirectory = GetModuleDirectory();
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
        => Path.GetDirectoryName(typeof(RustEngineInvoker).Assembly.Location)
            ?? throw new InvalidOperationException("PowerShell cmdlet assembly location could not be determined.");
}
