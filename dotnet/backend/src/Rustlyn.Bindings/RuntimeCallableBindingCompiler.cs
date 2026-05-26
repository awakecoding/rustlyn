using System.Globalization;

namespace Rustlyn.Bindings;

public static class RuntimeCallableBindingCompiler
{
    private static readonly IReadOnlyDictionary<string, CallableTypePolicy> PromotedTypes =
        new Dictionary<string, CallableTypePolicy>(StringComparer.Ordinal)
        {
            ["System.Math"] = new(nameof(RustWrapperContainer.Math), CallableMemberPolicy.StaticScalarMethods),
            ["System.MathF"] = new(nameof(RustWrapperContainer.MathF), CallableMemberPolicy.StaticScalarMethods),
            ["System.Console"] = new(nameof(RustWrapperContainer.Console), CallableMemberPolicy.StaticSupportedMembers),
            ["System.Convert"] = new(nameof(RustWrapperContainer.Convert), CallableMemberPolicy.StaticSupportedMembers),
            ["System.DateTimeOffset"] = new(nameof(RustWrapperContainer.DateTimeOffset), CallableMemberPolicy.StaticSupportedMembers),
            ["System.Environment"] = new(nameof(RustWrapperContainer.Environment), CallableMemberPolicy.StaticSupportedMembers),
            ["System.GC"] = new(nameof(RustWrapperContainer.Gc), CallableMemberPolicy.StaticSupportedMembers),
            ["System.Guid"] = new(nameof(RustWrapperContainer.Guid), CallableMemberPolicy.StaticSupportedMembers),
            ["System.IO.Directory"] = new(nameof(RustWrapperContainer.IoDirectory), CallableMemberPolicy.StaticSupportedMembers),
            ["System.IO.File"] = new(nameof(RustWrapperContainer.IoFile), CallableMemberPolicy.StaticSupportedMembers),
            ["System.IO.Path"] = new(nameof(RustWrapperContainer.IoPath), CallableMemberPolicy.StaticSupportedMembers),
            ["System.OperatingSystem"] = new(nameof(RustWrapperContainer.OperatingSystem), CallableMemberPolicy.StaticSupportedMembers),
            ["System.Threading.Tasks.Task"] = new(nameof(RustWrapperContainer.Task), CallableMemberPolicy.StaticSupportedMembers),
            ["System.TimeSpan"] = new(nameof(RustWrapperContainer.TimeSpan), CallableMemberPolicy.StaticSupportedMembers),
            ["System.Uri"] = new(nameof(RustWrapperContainer.Uri), CallableMemberPolicy.StaticSupportedMembers)
        };

    public static BindingManifestDocument AddCallableBindings(BindingManifestDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (document.RuntimeSurface is null)
        {
            return document;
        }

        var existingSymbols = new HashSet<string>(document.Bindings.Select(static binding => binding.Symbol), StringComparer.Ordinal);
        var bindings = document.Bindings.ToList();
        foreach (var binding in CreateCallableBindings(document.RuntimeSurface))
        {
            if (existingSymbols.Add(binding.Symbol))
            {
                bindings.Add(binding);
            }
        }

        return document with { Bindings = bindings };
    }

    public static IReadOnlyList<BindingManifestBinding> CreateCallableBindings(BindingManifestRuntimeSurface runtimeSurface)
    {
        ArgumentNullException.ThrowIfNull(runtimeSurface);

        var bindings = new List<BindingManifestBinding>();
        foreach (var runtimeType in runtimeSurface.Types.OrderBy(static type => type.FullName, StringComparer.Ordinal))
        {
            if (!PromotedTypes.TryGetValue(runtimeType.FullName, out var policy))
            {
                continue;
            }

            bindings.AddRange(CreateStaticMemberBindings(runtimeType, policy));
        }

        return bindings;
    }

    private static IReadOnlyList<BindingManifestBinding> CreateStaticMemberBindings(BindingManifestRuntimeType runtimeType, CallableTypePolicy policy)
    {
        var overloadCounts = runtimeType.Members
            .Where(IsCallableMemberKind)
            .GroupBy(static member => member.Name, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.Count(), StringComparer.Ordinal);
        var supportedMethods = runtimeType.Members
            .Where(IsCallableMemberKind)
            .Where(static member => member.IsStatic)
            .Where(static member => member.GenericArity == 0)
            .Where(member => IsStaticMemberSupported(member, policy.MemberPolicy))
            .OrderBy(static member => member.Name, StringComparer.Ordinal)
            .ThenBy(static member => member.Identity.SignatureKey, StringComparer.Ordinal)
            .ToArray();

        return supportedMethods
            .Select((member, index) => CreateStaticMemberBinding(runtimeType, member, policy.Container, overloadCounts[member.Name] > 1, index))
            .ToArray();
    }

    private static bool IsCallableMemberKind(BindingManifestRuntimeMember member)
        => string.Equals(member.Kind, nameof(ManagedApiRequirementKind.Method), StringComparison.Ordinal)
            || string.Equals(member.Kind, nameof(ManagedApiRequirementKind.Property), StringComparison.Ordinal);

    private static bool IsStaticMemberSupported(BindingManifestRuntimeMember member, CallableMemberPolicy policy)
    {
        if (member.ReturnType is null)
        {
            return false;
        }

        var returnProjection = RuntimeTypeProjectionClassifier.Classify(member.ReturnType);
        if (!IsSupportedReturnProjection(returnProjection, policy))
        {
            return false;
        }

        foreach (var parameter in member.Parameters)
        {
            if (parameter.IsOut)
            {
                return false;
            }

            var parameterProjection = RuntimeTypeProjectionClassifier.Classify(parameter.Type);
            if (!IsSupportedParameterProjection(parameterProjection, policy))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSupportedScalarProjection(RuntimeTypeProjection projection)
        => string.Equals(projection.Kind, "scalar", StringComparison.Ordinal)
            && projection.IsCurrentBindingSupported
            && projection.RustType is "i32" or "i64" or "f32" or "f64"
            && projection.AbiType is "int" or "long" or "float" or "double";

    private static bool IsSupportedReturnProjection(RuntimeTypeProjection projection, CallableMemberPolicy policy)
    {
        if (policy == CallableMemberPolicy.StaticScalarMethods)
        {
            return IsSupportedScalarProjection(projection);
        }

        return IsSupportedScalarProjection(projection)
            || IsSupportedVoidProjection(projection)
            || IsSupportedBooleanProjection(projection)
            || IsSupportedObjectHandleProjection(projection)
            || IsSupportedFutureProjection(projection);
    }

    private static bool IsSupportedParameterProjection(RuntimeTypeProjection projection, CallableMemberPolicy policy)
    {
        if (policy == CallableMemberPolicy.StaticScalarMethods)
        {
            return IsSupportedScalarProjection(projection);
        }

        return IsSupportedScalarProjection(projection)
            || IsSupportedBooleanProjection(projection)
            || IsSupportedObjectHandleProjection(projection);
    }

    private static bool IsSupportedBooleanProjection(RuntimeTypeProjection projection)
        => string.Equals(projection.Kind, "scalar", StringComparison.Ordinal)
            && projection.IsCurrentBindingSupported
            && projection.RustType is "bool"
            && projection.AbiType is "int";

    private static bool IsSupportedVoidProjection(RuntimeTypeProjection projection)
        => string.Equals(projection.Kind, "void", StringComparison.Ordinal)
            && projection.IsCurrentBindingSupported;

    private static bool IsSupportedObjectHandleProjection(RuntimeTypeProjection projection)
        => (string.Equals(projection.Kind, "string", StringComparison.Ordinal)
                || string.Equals(projection.Kind, "array", StringComparison.Ordinal)
                || string.Equals(projection.Kind, "value-wrapper", StringComparison.Ordinal))
            && projection.IsCurrentBindingSupported
            && projection.AbiType is "i32";

    private static bool IsSupportedFutureProjection(RuntimeTypeProjection projection)
        => string.Equals(projection.Kind, "future", StringComparison.Ordinal)
            && projection.IsCurrentBindingSupported
            && projection.AbiType is "i32";

    private static BindingManifestBinding CreateStaticMemberBinding(
        BindingManifestRuntimeType runtimeType,
        BindingManifestRuntimeMember member,
        string container,
        bool isOverloaded,
        int order)
    {
        var parameters = member.Parameters;
        var parameterNames = CreateStableParameterNames(parameters.Count);
        var parameterProjections = parameters
            .Select(static parameter => RuntimeTypeProjectionClassifier.Classify(parameter.Type))
            .ToArray();
        var returnProjection = RuntimeTypeProjectionClassifier.Classify(member.ReturnType!);
        var symbol = CreateSymbol(runtimeType.FullName, member.Name, parameterProjections);
        var helper = CreateHelperMethodName(symbol);
        var managedParameters = parameterNames
            .Zip(parameterProjections, static (name, projection) => new BindingManifestParameter(ToManagedGlueParameterType(projection), ToManagedGlueParameterName(name, projection), RustAbiType: null))
            .Append(new BindingManifestParameter("IntPtr", "exceptionOutPointer", RustAbiType: null))
            .ToArray();
        var managedTarget = CreateManagedTarget(runtimeType, member, parameterNames, parameterProjections);
        var managedReturnType = ToManagedGlueReturnType(returnProjection);
        var managedResultKind = ToManagedResultKind(returnProjection);
        var rustExternLines = CreateRustExternLines(symbol, managedParameters, managedReturnType);
        var wrapperName = CreateWrapperMethodName(member, parameterProjections, isOverloaded);
        var wrapperParameters = parameterNames
            .Zip(parameterProjections, static (name, projection) => $"{ToSnakeCase(name)}: {ToRustWrapperParameterType(projection)}")
            .ToArray();
        var wrapperCallArguments = parameterNames
            .Zip(parameterProjections, static (name, projection) => ToRustWrapperCallArgument(name, projection))
            .ToArray();
        var resultKind = ToRustWrapperResultKind(returnProjection);

        return new BindingManifestBinding(
            symbol,
            helper,
            managedReturnType,
            managedResultKind,
            managedParameters,
            new BindingManifestExceptionConvention("write-exception-out", "exceptionOutPointer"),
            managedTarget,
            BindingManifestFormatting.FormatRustExternSignature(new RustExternBinding(symbol, rustExternLines)),
            rustExternLines,
            [
                new BindingManifestWrapper(
                    container,
                    BindingManifestFormatting.FormatRustWrapperPath(Enum.Parse<RustWrapperContainer>(container), wrapperName),
                    $"pub fn {wrapperName}({string.Join(", ", wrapperParameters)}) -> Result<{returnProjection.RustType}, Exception>",
                    wrapperCallArguments,
                    parameterNames.Length == 1 ? parameterNames[0] : "value",
                    resultKind,
                    string.Equals(resultKind, nameof(RustWrapperResultKind.ObjectHandle), StringComparison.Ordinal) ? returnProjection.RustType : null,
                    order)
            ]);
    }

    private static string CreateManagedTarget(
        BindingManifestRuntimeType runtimeType,
        BindingManifestRuntimeMember member,
        IReadOnlyList<string> parameterNames,
        IReadOnlyList<RuntimeTypeProjection> parameterProjections)
    {
        if (string.Equals(member.Kind, nameof(ManagedApiRequirementKind.Property), StringComparison.Ordinal))
        {
            return $"global::{runtimeType.FullName}.{member.Name}";
        }

        var arguments = parameterNames
            .Zip(parameterProjections, static (name, projection) => ToManagedArgumentExpression(name, projection))
            .ToArray();
        return $"global::{runtimeType.FullName}.{member.Name}({string.Join(", ", arguments)})";
    }

    private static string ToManagedArgumentExpression(string name, RuntimeTypeProjection projection)
    {
        return projection.Kind switch
        {
            "string" => $"ManagedInteropRuntime.GetObject<string>({ToManagedGlueParameterName(name, projection)})",
            "array" => $"ManagedInteropRuntime.GetObject<{ToManagedArrayTypeName(projection)}>({ToManagedGlueParameterName(name, projection)})",
            "value-wrapper" => $"ManagedInteropRuntime.GetObject<global::{projection.ManagedType}>({ToManagedGlueParameterName(name, projection)})",
            _ when IsSupportedBooleanProjection(projection) => $"{name} != 0",
            _ => name
        };
    }

    private static string ToManagedArrayTypeName(RuntimeTypeProjection projection)
    {
        if (projection.Element is null)
        {
            throw new NotSupportedException($"Array projection '{projection.ManagedType}' does not carry an element projection.");
        }

        return projection.Element.ManagedType switch
        {
            "System.String" => "string[]",
            "System.Int32" => "int[]",
            "System.Byte" => "byte[]",
            _ => throw new NotSupportedException($"Array projection '{projection.ManagedType}' is not supported in runtime callable bindings.")
        };
    }

    private static IReadOnlyList<string> CreateRustExternLines(
        string symbol,
        IReadOnlyList<BindingManifestParameter> parameters,
        string returnType)
    {
        var rustParameters = parameters
            .Select(static parameter => $"{ToRustParameterName(parameter.Name)}: {ToRustParameterType(parameter.Type)}")
            .ToArray();
        var rustReturnType = ToRustReturnType(returnType);
        var singleLine = $"fn {symbol}({string.Join(", ", rustParameters)}) -> {rustReturnType};";
        if (singleLine.Length <= 120)
        {
            return [singleLine];
        }

        return [$"fn {symbol}(", .. rustParameters.Select(static parameter => $"    {parameter},"), $") -> {rustReturnType};"];
    }

    private static string CreateSymbol(
        string declaringType,
        string methodName,
        IReadOnlyList<RuntimeTypeProjection> parameterProjections)
    {
        var typePart = string.Join("_", declaringType.Split('.').Select(static part => part.ToLowerInvariant()));
        var suffix = parameterProjections.Count == 0
            ? "void"
            : string.Join("_", parameterProjections.Select(ToSymbolTypeSuffix));
        return $"rustlyn_bindgen_{typePart}_{ToSnakeCase(methodName)}_{suffix}";
    }

    private static string CreateHelperMethodName(string symbol)
    {
        const string prefix = "rustlyn_";
        var name = symbol.StartsWith(prefix, StringComparison.Ordinal)
            ? symbol[prefix.Length..]
            : symbol;
        return string.Concat(name.Split('_').Select(static part => char.ToUpperInvariant(part[0]) + part[1..]));
    }

    private static string CreateWrapperMethodName(
        BindingManifestRuntimeMember member,
        IReadOnlyList<RuntimeTypeProjection> parameterProjections,
        bool isOverloaded)
    {
        var name = ToSnakeCase(member.Name);
        if (!isOverloaded)
        {
            return name;
        }

        var suffix = parameterProjections.Count == 0
            ? "void"
            : string.Join("_", parameterProjections.Select(ToSymbolTypeSuffix));
        return $"{name}_{suffix}";
    }

    private static string[] CreateStableParameterNames(int parameterCount)
    {
        return parameterCount switch
        {
            0 => [],
            1 => ["value"],
            2 => ["x", "y"],
            _ => Enumerable.Range(0, parameterCount).Select(static index => $"value{(index + 1).ToString(CultureInfo.InvariantCulture)}").ToArray()
        };
    }

    private static string ToRustParameterType(string managedType)
    {
        return managedType switch
        {
            "int" => "i32",
            "long" => "i64",
            "float" => "f32",
            "double" => "f64",
            "IntPtr" => "*mut i32",
            _ => throw new NotSupportedException($"Managed glue parameter type '{managedType}' is not supported in runtime callable bindings.")
        };
    }

    private static string ToRustReturnType(string managedType)
    {
        return managedType switch
        {
            "int" => "i32",
            "long" => "i64",
            "float" => "f32",
            "double" => "f64",
            _ => throw new NotSupportedException($"Managed glue return type '{managedType}' is not supported in runtime callable bindings.")
        };
    }

    private static string ToManagedGlueParameterType(RuntimeTypeProjection projection)
        => IsSupportedObjectHandleProjection(projection) || IsSupportedBooleanProjection(projection)
            ? "int"
            : projection.AbiType ?? throw new NotSupportedException($"Projection '{projection.ManagedType}' does not expose an ABI type.");

    private static string ToManagedGlueReturnType(RuntimeTypeProjection projection)
        => IsSupportedObjectHandleProjection(projection) || IsSupportedBooleanProjection(projection) || IsSupportedFutureProjection(projection) || IsSupportedVoidProjection(projection)
            ? "int"
            : projection.AbiType ?? throw new NotSupportedException($"Projection '{projection.ManagedType}' does not expose an ABI type.");

    private static string ToManagedGlueParameterName(string name, RuntimeTypeProjection projection)
        => IsSupportedObjectHandleProjection(projection)
            ? $"{name}Handle"
            : name;

    private static string ToManagedResultKind(RuntimeTypeProjection projection)
    {
        if (IsSupportedObjectHandleProjection(projection))
        {
            return "object-handle";
        }

        if (IsSupportedFutureProjection(projection))
        {
            return projection.ManagedType.StartsWith("System.Threading.Tasks.ValueTask", StringComparison.Ordinal)
                ? "value-task-handle"
                : "task-handle";
        }

        if (IsSupportedBooleanProjection(projection))
        {
            return "boolean-as-int";
        }

        if (IsSupportedVoidProjection(projection))
        {
            return "void-call";
        }

        return projection.AbiType ?? throw new NotSupportedException($"Projection '{projection.ManagedType}' does not expose an ABI type.");
    }

    private static string ToRustWrapperParameterType(RuntimeTypeProjection projection)
        => IsSupportedObjectHandleProjection(projection)
            ? $"&{projection.RustType}"
            : projection.RustType ?? throw new NotSupportedException($"Projection '{projection.ManagedType}' does not expose a Rust type.");

    private static string ToRustWrapperCallArgument(string name, RuntimeTypeProjection projection)
        => IsSupportedObjectHandleProjection(projection)
            ? $"{ToSnakeCase(name)}.handle()"
            : IsSupportedBooleanProjection(projection)
                ? $"if {ToSnakeCase(name)} {{ 1 }} else {{ 0 }}"
            : ToSnakeCase(name);

    private static string ToRustWrapperResultKind(RuntimeTypeProjection projection)
    {
        if (IsSupportedObjectHandleProjection(projection))
        {
            return nameof(RustWrapperResultKind.ObjectHandle);
        }

        if (IsSupportedFutureProjection(projection))
        {
            return nameof(RustWrapperResultKind.Future);
        }

        if (IsSupportedBooleanProjection(projection))
        {
            return nameof(RustWrapperResultKind.Boolean);
        }

        if (IsSupportedVoidProjection(projection))
        {
            return nameof(RustWrapperResultKind.Void);
        }

        return nameof(RustWrapperResultKind.Scalar);
    }

    private static string ToSymbolTypeSuffix(RuntimeTypeProjection projection)
    {
        if (string.Equals(projection.Kind, "array", StringComparison.Ordinal))
        {
            if (projection.Element is null)
            {
                throw new NotSupportedException($"Array projection '{projection.ManagedType}' does not carry an element projection.");
            }

            return $"{ToSymbolTypeSuffix(projection.Element)}_array";
        }

        return projection.Kind switch
        {
            "string" => "string",
            "scalar" => projection.RustType ?? throw new NotSupportedException($"Projection '{projection.ManagedType}' does not expose a Rust type."),
            "value-wrapper" => ToSnakeCase(projection.ManagedType.Split('.').Last()),
            _ => projection.RustType ?? throw new NotSupportedException($"Projection '{projection.ManagedType}' does not expose a Rust type.")
        };
    }

    private static string ToRustParameterName(string managedName)
    {
        var snake = ToSnakeCase(managedName);
        return string.Equals(snake, "exception_out_pointer", StringComparison.Ordinal)
            ? "exception_out"
            : snake;
    }

    private static string ToSnakeCase(string value)
    {
        var chars = new List<char>(value.Length + 8);
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];
            if (char.IsUpper(current))
            {
                if (index > 0)
                {
                    chars.Add('_');
                }

                chars.Add(char.ToLowerInvariant(current));
            }
            else
            {
                chars.Add(current);
            }
        }

        return new string(chars.ToArray());
    }

    private sealed record CallableTypePolicy(string Container, CallableMemberPolicy MemberPolicy);

    private enum CallableMemberPolicy
    {
        StaticScalarMethods,
        StaticSupportedMembers
    }
}
