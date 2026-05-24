using System.Reflection;
using System.Runtime.Loader;
using System.Globalization;

namespace Rustlyn.Backend;

public static class LoweredAssemblyInvoker
{
    private const string GeneratedTypeName = "Rustlyn.GeneratedModule";

    public static object? InvokeBitcode(string artifactPath, string methodName, IReadOnlyList<object?> arguments, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        ArgumentNullException.ThrowIfNull(arguments);

        var emittedAssemblyPath = Path.Combine(Path.GetTempPath(), $"rustlyn-invoke-{Guid.NewGuid():N}.dll");
        try
        {
            LoweredAssemblyEmitter.EmitBitcode(artifactPath, emittedAssemblyPath, llvmRoot);
            return InvokeAssembly(emittedAssemblyPath, GeneratedTypeName, methodName, arguments);
        }
        finally
        {
            if (File.Exists(emittedAssemblyPath))
            {
                File.Delete(emittedAssemblyPath);
            }
        }
    }

    public static object? InvokeAssembly(string assemblyPath, string typeName, string methodName, IReadOnlyList<object?> arguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName);
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        ArgumentNullException.ThrowIfNull(arguments);

        var loadContext = new AssemblyLoadContext($"rustlyn-invoke-{Guid.NewGuid():N}", isCollectible: true);
        try
        {
            using var assemblyStream = new MemoryStream(File.ReadAllBytes(Path.GetFullPath(assemblyPath)));
            var assembly = loadContext.LoadFromStream(assemblyStream);
            var generatedType = assembly.GetType(typeName)
                ?? throw new InvalidOperationException($"Type '{typeName}' was not found in assembly '{assemblyPath}'.");
            var method = generatedType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
                ?? throw new InvalidOperationException($"Method '{methodName}' was not found on type '{typeName}'.");

            var parameters = method.GetParameters();
            if (parameters.Length != arguments.Count)
            {
                throw new InvalidOperationException($"Method '{methodName}' expects {parameters.Length} arguments but {arguments.Count} were provided.");
            }

            var boundArguments = new object?[arguments.Count];
            for (var index = 0; index < arguments.Count; index++)
            {
                boundArguments[index] = ConvertArgument(arguments[index], parameters[index].ParameterType, methodName, index);
            }

            return method.Invoke(null, boundArguments);
        }
        finally
        {
            loadContext.Unload();
        }
    }

    private static object? ConvertArgument(object? value, Type targetType, string methodName, int parameterIndex)
    {
        if (value is null)
        {
            if (targetType.IsValueType)
            {
                throw new InvalidOperationException($"Method '{methodName}' parameter {parameterIndex} requires a non-null value of type '{targetType.Name}'.");
            }

            return null;
        }

        if (targetType.IsInstanceOfType(value))
        {
            return value;
        }

        try
        {
            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Method '{methodName}' parameter {parameterIndex} could not convert '{value}' to '{targetType.Name}'.", ex);
        }
    }
}