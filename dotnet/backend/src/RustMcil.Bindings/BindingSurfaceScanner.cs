using System.Reflection;

namespace RustMcil.Bindings;

/// <summary>
/// Scans .NET types via reflection to produce <see cref="ManagedApiRequirement"/> entries.
/// This enables metadata-driven binding surface expansion instead of manual hardcoding.
/// </summary>
public static class BindingSurfaceScanner
{
    /// <summary>
    /// Scans a type for public methods and properties that match supported binding signatures.
    /// Returns requirements for methods whose parameters are all bindable types.
    /// </summary>
    public static IReadOnlyList<ManagedApiRequirement> ScanType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var requirements = new List<ManagedApiRequirement>();

        // Always add a type requirement
        var typeName = type.FullName ?? type.Name;
        requirements.Add(ManagedApiRequirement.ForType(typeName, type));

        // Scan public static and instance methods.
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (method.IsSpecialName) continue; // skip property accessors
            var parameters = method.GetParameters();
            if (!AllParametersBindable(parameters)) continue;
            if (!IsReturnTypeBindable(method.ReturnType)) continue;

            var paramTypes = parameters.Select(p => p.ParameterType).ToArray();
            var displayName = FormatMethodDisplayName(type, method.Name, paramTypes);
            requirements.Add(ManagedApiRequirement.Method(displayName, type, method.Name, paramTypes));
        }

        // Scan public static and instance properties with getters.
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (property.GetMethod is null) continue;
            if (property.GetIndexParameters().Length != 0) continue;
            if (!IsReturnTypeBindable(property.PropertyType)) continue;

            var displayName = $"{typeName}.{property.Name}";
            requirements.Add(ManagedApiRequirement.Property(displayName, type, property.Name));
        }

        return requirements;
    }

    /// <summary>
    /// Scans multiple types and returns a combined, deduplicated list of requirements.
    /// </summary>
    public static IReadOnlyList<ManagedApiRequirement> ScanTypes(params Type[] types)
    {
        var all = new List<ManagedApiRequirement>();
        var seen = new HashSet<string>();

        foreach (var type in types)
        {
            foreach (var req in ScanType(type))
            {
                if (seen.Add(req.DisplayName))
                {
                    all.Add(req);
                }
            }
        }

        return all;
    }

    /// <summary>
    /// Checks whether a parameter type is supported in the current binding model.
    /// Supported: string, int, long, float, double, bool, void, IntPtr, arrays of bindable types, enums with int backing.
    /// </summary>
    public static bool IsTypeBindable(Type type)
    {
        if (type == typeof(string)) return true;
        if (type == typeof(int)) return true;
        if (type == typeof(long)) return true;
        if (type == typeof(float)) return true;
        if (type == typeof(double)) return true;
        if (type == typeof(bool)) return true;
        if (type == typeof(void)) return true;
        if (type == typeof(IntPtr)) return true;
        if (type.IsArray)
        {
            var elementType = type.GetElementType();
            return elementType is not null && IsTypeBindable(elementType);
        }

        if (type.IsEnum && Enum.GetUnderlyingType(type) == typeof(int)) return true;
        return false;
    }

    private static bool AllParametersBindable(ParameterInfo[] parameters)
    {
        return parameters.All(p => IsTypeBindable(p.ParameterType));
    }

    private static bool IsReturnTypeBindable(Type returnType)
    {
        if (returnType == typeof(void)) return true;
        return IsTypeBindable(returnType);
    }

    private static string FormatMethodDisplayName(Type type, string methodName, Type[] paramTypes)
    {
        var typeName = type.FullName ?? type.Name;
        var paramNames = string.Join(", ", paramTypes.Select(FormatTypeName));
        return $"{typeName}.{methodName}({paramNames})";
    }

    private static string FormatTypeName(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(float)) return "float";
        if (type == typeof(double)) return "double";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(void)) return "void";
        if (type == typeof(IntPtr)) return "IntPtr";
        if (type.IsArray)
        {
            var elementType = type.GetElementType()
                ?? throw new InvalidOperationException($"Array type '{type}' does not expose an element type.");
            return $"{FormatTypeName(elementType)}[]";
        }

        return type.Name;
    }
}
