namespace Rustlyn.Bindings;

public static class RuntimeTypeProjectionClassifier
{
    public static RuntimeTypeProjection Classify(string typeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName);

        if (typeName.EndsWith("[]", StringComparison.Ordinal))
        {
            var element = Classify(typeName[..^2]);
            if (!element.IsCurrentBindingSupported)
            {
                return new RuntimeTypeProjection(
                    "array",
                    typeName,
                    RustType: null,
                    AbiType: "i32",
                    IsCurrentBindingSupported: false,
                    $"array element type '{element.ManagedType}' is unsupported: {element.UnsupportedReason}",
                    element);
            }

            return new RuntimeTypeProjection(
                "array",
                typeName,
                RustType: $"{element.RustType}Array",
                AbiType: "i32",
                IsCurrentBindingSupported: true,
                UnsupportedReason: null,
                element);
        }

        return typeName switch
        {
            "System.Void" => new RuntimeTypeProjection("void", typeName, RustType: "()", AbiType: "void", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null),
            "System.String" => new RuntimeTypeProjection("string", typeName, RustType: "ManagedString", AbiType: "i32", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null),
            "System.Guid" => ValueWrapper(typeName, "ManagedGuid", isCurrentBindingSupported: true),
            "System.TimeSpan" => ValueWrapper(typeName, "ManagedTimeSpan", isCurrentBindingSupported: true),
            "System.DateTimeOffset" => ValueWrapper(typeName, "ManagedDateTimeOffset", isCurrentBindingSupported: true),
            "System.Int32" => Scalar(typeName, "i32", "int", isCurrentBindingSupported: true),
            "System.Int64" => Scalar(typeName, "i64", "long", isCurrentBindingSupported: true),
            "System.Single" => Scalar(typeName, "f32", "float", isCurrentBindingSupported: true),
            "System.Double" => Scalar(typeName, "f64", "double", isCurrentBindingSupported: true),
            "System.Boolean" => Scalar(typeName, "bool", "int", isCurrentBindingSupported: true),
            "System.IntPtr" => Scalar(typeName, "isize", "IntPtr", isCurrentBindingSupported: true),
            "System.Byte" => Scalar(typeName, "u8", "byte", isCurrentBindingSupported: false, "byte scalars are not emitted by the current binding ABI yet"),
            _ when TaskFutureProjectionPolicy.AnalyzeReturnType(typeName) is { IsTaskLike: true } taskAnalysis
                => ClassifyFuture(typeName, taskAnalysis),
            _ when typeName.Contains('&', StringComparison.Ordinal) => Unsupported(typeName, "by-reference types are not supported"),
            _ when typeName.Contains('*', StringComparison.Ordinal) => Unsupported(typeName, "pointer types are not supported"),
            _ when typeName.Contains('<', StringComparison.Ordinal) || typeName.Contains('`', StringComparison.Ordinal) => Unsupported(typeName, "generic or open types are not supported"),
            _ => new RuntimeTypeProjection("object", typeName, RustType: $"Managed{typeName.Split('.').Last()}", AbiType: "i32", IsCurrentBindingSupported: false, "object handles are not emitted by the current runtime binding policy yet", Element: null)
        };
    }

    public static RuntimeTypeProjection Classify(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.IsByRef)
        {
            return Unsupported(type, "by-reference types are not supported");
        }

        if (type.IsByRefLike)
        {
            return Unsupported(type, "by-ref-like types are not supported");
        }

        if (IsKnownType(type, "System.Void"))
        {
            return new RuntimeTypeProjection("void", ManagedTypeName(type), RustType: "()", AbiType: "void", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null);
        }

        if (IsKnownType(type, "System.String"))
        {
            return new RuntimeTypeProjection("string", ManagedTypeName(type), RustType: "ManagedString", AbiType: "i32", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null);
        }

        if (IsKnownType(type, "System.Guid"))
        {
            return ValueWrapper(type, "ManagedGuid", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.TimeSpan"))
        {
            return ValueWrapper(type, "ManagedTimeSpan", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.DateTimeOffset"))
        {
            return ValueWrapper(type, "ManagedDateTimeOffset", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.Int32"))
        {
            return Scalar(type, "i32", "int", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.Int64"))
        {
            return Scalar(type, "i64", "long", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.Single"))
        {
            return Scalar(type, "f32", "float", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.Double"))
        {
            return Scalar(type, "f64", "double", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.Boolean"))
        {
            return Scalar(type, "bool", "int", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.IntPtr"))
        {
            return Scalar(type, "isize", "IntPtr", isCurrentBindingSupported: true);
        }

        if (IsKnownType(type, "System.Byte"))
        {
            return Scalar(type, "u8", "byte", isCurrentBindingSupported: false, "byte scalars are not emitted by the current binding ABI yet");
        }

        if (TaskFutureProjectionPolicy.AnalyzeReturnType(type) is { IsTaskLike: true } taskAnalysis)
        {
            var resultProjection = taskAnalysis.IsGeneric
                ? Classify(type.GetGenericArguments()[0])
                : new RuntimeTypeProjection("void", "System.Void", RustType: "()", AbiType: "void", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null);
            var isSupported = resultProjection.IsCurrentBindingSupported;
            return new RuntimeTypeProjection(
                "future",
                ManagedTypeName(type),
                RustType: $"TaskFuture<{resultProjection.RustType}>",
                AbiType: "i32",
                IsCurrentBindingSupported: isSupported,
                isSupported ? null : TaskFutureProjectionPolicy.CreateDeferredReason(taskAnalysis),
                resultProjection);
        }

        if (type.IsArray)
        {
            return ClassifyArray(type);
        }

        if (type.IsEnum)
        {
            var underlying = Enum.GetUnderlyingType(type);
            if (IsKnownType(underlying, "System.Int32"))
            {
                return new RuntimeTypeProjection("enum", ManagedTypeName(type), RustType: type.Name, AbiType: "i32", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null);
            }

            return Unsupported(type, $"enum backing type '{ManagedTypeName(underlying)}' is not supported");
        }

        if (IsDelegateType(type))
        {
            return Unsupported(type, "delegate types are not supported; use explicit callback ABI metadata");
        }

        if (type.ContainsGenericParameters)
        {
            return Unsupported(type, "generic or open types are not supported");
        }

        if (type.IsGenericType)
        {
            var genericArguments = string.Join(", ", type.GetGenericArguments().Select(ManagedTypeName));
            return Unsupported(type, $"generic constructed types are not supported; generic arguments: {genericArguments}");
        }

        if (type.IsValueType)
        {
            return new RuntimeTypeProjection("value-wrapper", ManagedTypeName(type), RustType: $"Managed{type.Name}", AbiType: "i32", IsCurrentBindingSupported: false, "value-type wrappers require validated ownership/layout policy", Element: null);
        }

        return new RuntimeTypeProjection("object", ManagedTypeName(type), RustType: $"Managed{type.Name}", AbiType: "i32", IsCurrentBindingSupported: false, "object handles are not emitted by the current runtime binding policy yet", Element: null);
    }

    private static RuntimeTypeProjection ClassifyArray(Type type)
    {
        if (!type.IsSZArray)
        {
            return Unsupported(type, $"array rank {type.GetArrayRank().ToString(System.Globalization.CultureInfo.InvariantCulture)} is not supported");
        }

        var elementType = type.GetElementType();
        if (elementType is null)
        {
            return Unsupported(type, "array element type could not be resolved");
        }

        var element = Classify(elementType);
        if (!element.IsCurrentBindingSupported)
        {
            return new RuntimeTypeProjection(
                "array",
                ManagedTypeName(type),
                RustType: null,
                AbiType: "i32",
                IsCurrentBindingSupported: false,
                $"array element type '{element.ManagedType}' is unsupported: {element.UnsupportedReason}",
                element);
        }

        return new RuntimeTypeProjection(
            "array",
            ManagedTypeName(type),
            RustType: $"{element.RustType}Array",
            AbiType: "i32",
            IsCurrentBindingSupported: true,
            UnsupportedReason: null,
            element);
    }

    private static RuntimeTypeProjection ClassifyFuture(string typeName, TaskFutureProjectionAnalysis analysis)
    {
        var resultProjection = analysis is { IsGeneric: true, ResultType: not null }
            ? Classify(analysis.ResultType)
            : new RuntimeTypeProjection("void", "System.Void", RustType: "()", AbiType: "void", IsCurrentBindingSupported: true, UnsupportedReason: null, Element: null);
        var isSupported = resultProjection.IsCurrentBindingSupported;
        return new RuntimeTypeProjection(
            "future",
            typeName,
            RustType: $"TaskFuture<{resultProjection.RustType}>",
            AbiType: "i32",
            IsCurrentBindingSupported: isSupported,
            isSupported ? null : TaskFutureProjectionPolicy.CreateDeferredReason(analysis),
            resultProjection);
    }

    private static RuntimeTypeProjection Scalar(
        Type type,
        string rustType,
        string abiType,
        bool isCurrentBindingSupported,
        string? unsupportedReason = null)
        => new("scalar", ManagedTypeName(type), rustType, abiType, isCurrentBindingSupported, unsupportedReason, Element: null);

    private static RuntimeTypeProjection Scalar(
        string typeName,
        string rustType,
        string abiType,
        bool isCurrentBindingSupported,
        string? unsupportedReason = null)
        => new("scalar", typeName, rustType, abiType, isCurrentBindingSupported, unsupportedReason, Element: null);

    private static RuntimeTypeProjection ValueWrapper(
        Type type,
        string rustType,
        bool isCurrentBindingSupported,
        string? unsupportedReason = null)
        => new("value-wrapper", ManagedTypeName(type), rustType, "i32", isCurrentBindingSupported, unsupportedReason, Element: null);

    private static RuntimeTypeProjection ValueWrapper(
        string typeName,
        string rustType,
        bool isCurrentBindingSupported,
        string? unsupportedReason = null)
        => new("value-wrapper", typeName, rustType, "i32", isCurrentBindingSupported, unsupportedReason, Element: null);

    private static RuntimeTypeProjection Unsupported(Type type, string reason)
        => new("unsupported", ManagedTypeName(type), RustType: null, AbiType: null, IsCurrentBindingSupported: false, reason, Element: null);

    private static RuntimeTypeProjection Unsupported(string typeName, string reason)
        => new("unsupported", typeName, RustType: null, AbiType: null, IsCurrentBindingSupported: false, reason, Element: null);

    private static string ManagedTypeName(Type type)
        => type.FullName?.Replace('+', '.') ?? type.Name;

    private static bool IsKnownType(Type type, string fullName)
        => string.Equals(type.FullName, fullName, StringComparison.Ordinal);

    private static bool IsDelegateType(Type type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (IsKnownType(current, "System.Delegate") || IsKnownType(current, "System.MulticastDelegate"))
            {
                return true;
            }
        }

        return false;
    }
}

public sealed record RuntimeTypeProjection(
    string Kind,
    string ManagedType,
    string? RustType,
    string? AbiType,
    bool IsCurrentBindingSupported,
    string? UnsupportedReason,
    RuntimeTypeProjection? Element);
