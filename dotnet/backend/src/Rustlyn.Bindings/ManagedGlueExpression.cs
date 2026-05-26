namespace Rustlyn.Bindings;

public abstract record ManagedGlueExpression
{
    public static ManagedGlueExpression Parameter(string name)
        => new ManagedGlueParameterExpression(name);

    public static ManagedGlueExpression EnumValue(Type enumType, string name)
        => new ManagedGlueEnumValueExpression(enumType, name);

    public static ManagedGlueExpression EnumParameter(Type enumType, string parameterName)
        => new ManagedGlueEnumParameterExpression(enumType, parameterName);

    public static ManagedGlueExpression Utf8String(string pointerParameterName, string lengthParameterName)
        => new ManagedGlueUtf8StringExpression(pointerParameterName, lengthParameterName);

    public static ManagedGlueExpression ManagedObject(Type type, string handleParameterName)
        => new ManagedGlueManagedObjectExpression(type, handleParameterName);

    public static ManagedGlueExpression StaticMethod(Type declaringType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => new ManagedGlueStaticMethodExpression(declaringType, methodName, parameterTypes, arguments);

    public static ManagedGlueExpression StaticProperty(Type declaringType, string propertyName)
        => new ManagedGlueStaticPropertyExpression(declaringType, propertyName);

    public static ManagedGlueExpression InstanceMethod(ManagedGlueExpression instance, Type declaringType, string methodName, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => new ManagedGlueInstanceMethodExpression(instance, declaringType, methodName, parameterTypes, arguments);

    public static ManagedGlueExpression InstanceProperty(ManagedGlueExpression instance, Type declaringType, string propertyName)
        => new ManagedGlueInstancePropertyExpression(instance, declaringType, propertyName);

    public static ManagedGlueExpression Utf8ByteCount(ManagedGlueExpression value)
        => new ManagedGlueUtf8ByteCountExpression(value);

    public static ManagedGlueExpression Utf8Copy(ManagedGlueExpression value, string destinationPointerParameterName, string destinationCapacityParameterName)
        => new ManagedGlueUtf8CopyExpression(value, destinationPointerParameterName, destinationCapacityParameterName);

    public static ManagedGlueExpression ArrayLength(ManagedGlueExpression array)
        => new ManagedGlueArrayLengthExpression(array);

    public static ManagedGlueExpression ArrayElement(ManagedGlueExpression array, string indexParameterName)
        => new ManagedGlueArrayElementExpression(array, indexParameterName);

    public static ManagedGlueExpression Constructor(Type declaringType, IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments)
        => new ManagedGlueConstructorExpression(declaringType, parameterTypes, arguments);

    public static ManagedGlueExpression Raw(string code)
        => new ManagedGlueRawExpression(code);

    public abstract string ToCode();

    public virtual void Validate()
    {
    }
}

public sealed record ManagedGlueParameterExpression(string Name) : ManagedGlueExpression
{
    public override string ToCode()
        => Name;
}

public sealed record ManagedGlueEnumValueExpression(Type EnumType, string Name) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{ManagedGlueCode.TypeName(EnumType)}.{Name}";

    public override void Validate()
    {
        if (!EnumType.IsEnum || !Enum.GetNames(EnumType).Contains(Name, StringComparer.Ordinal))
        {
            throw new InvalidOperationException($"Managed glue enum value '{ManagedGlueCode.TypeName(EnumType)}.{Name}' could not be resolved.");
        }
    }
}

public sealed record ManagedGlueEnumParameterExpression(Type EnumType, string ParameterName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"(({ManagedGlueCode.TypeName(EnumType)}){ParameterName})";

    public override void Validate()
    {
        if (!EnumType.IsEnum || Enum.GetUnderlyingType(EnumType) != typeof(int))
        {
            throw new InvalidOperationException($"Managed glue enum parameter '{ManagedGlueCode.TypeName(EnumType)} {ParameterName}' must be an int-backed enum.");
        }
    }
}

public sealed record ManagedGlueUtf8StringExpression(string PointerParameterName, string LengthParameterName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"InteropUtf8.ReadString({PointerParameterName}, {LengthParameterName})";
}

public sealed record ManagedGlueManagedObjectExpression(Type Type, string HandleParameterName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"ManagedInteropRuntime.GetObject<{ManagedGlueCode.TypeName(Type)}>({HandleParameterName})";
}

public sealed record ManagedGlueStaticMethodExpression(
    Type DeclaringType,
    string MethodName,
    IReadOnlyList<Type> ParameterTypes,
    IReadOnlyList<ManagedGlueExpression> Arguments) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}({string.Join(", ", Arguments.Select(argument => argument.ToCode()))})";

    public override void Validate()
    {
        ValidateArguments(ParameterTypes, Arguments, $"{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}");
        var method = DeclaringType.GetMethod(MethodName, ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Managed glue static method '{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}' could not be resolved.");
        if (!method.IsStatic)
        {
            throw new InvalidOperationException($"Managed glue method '{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}' is not static.");
        }
    }

    private static void ValidateArguments(IReadOnlyList<Type> parameterTypes, IReadOnlyList<ManagedGlueExpression> arguments, string displayName)
    {
        if (parameterTypes.Count != arguments.Count)
        {
            throw new InvalidOperationException($"Managed glue call '{displayName}' has {arguments.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)} arguments for {parameterTypes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)} parameters.");
        }

        foreach (var argument in arguments)
        {
            argument.Validate();
        }
    }
}

public sealed record ManagedGlueStaticPropertyExpression(Type DeclaringType, string PropertyName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}";

    public override void Validate()
    {
        var property = DeclaringType.GetProperty(PropertyName)
            ?? throw new InvalidOperationException($"Managed glue static property '{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}' could not be resolved.");
        if (property.GetIndexParameters().Length != 0)
        {
            throw new InvalidOperationException($"Managed glue property '{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}' is an indexed property.");
        }

        var getter = property.GetMethod;
        if (getter is null || !getter.IsStatic)
        {
            throw new InvalidOperationException($"Managed glue property '{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}' is not a static gettable property.");
        }
    }
}

public sealed record ManagedGlueInstanceMethodExpression(
    ManagedGlueExpression Instance,
    Type DeclaringType,
    string MethodName,
    IReadOnlyList<Type> ParameterTypes,
    IReadOnlyList<ManagedGlueExpression> Arguments) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{Instance.ToCode()}.{MethodName}({string.Join(", ", Arguments.Select(argument => argument.ToCode()))})";

    public override void Validate()
    {
        Instance.Validate();
        if (ParameterTypes.Count != Arguments.Count)
        {
            throw new InvalidOperationException($"Managed glue call '{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}' has {Arguments.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)} arguments for {ParameterTypes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)} parameters.");
        }

        foreach (var argument in Arguments)
        {
            argument.Validate();
        }

        var method = DeclaringType.GetMethod(MethodName, ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Managed glue instance method '{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}' could not be resolved.");
        if (method.IsStatic)
        {
            throw new InvalidOperationException($"Managed glue method '{ManagedGlueCode.TypeName(DeclaringType)}.{MethodName}' is static, not an instance method.");
        }
    }
}

public sealed record ManagedGlueInstancePropertyExpression(ManagedGlueExpression Instance, Type DeclaringType, string PropertyName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{Instance.ToCode()}.{PropertyName}";

    public override void Validate()
    {
        Instance.Validate();
        var property = DeclaringType.GetProperty(PropertyName)
            ?? throw new InvalidOperationException($"Managed glue instance property '{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}' could not be resolved.");
        if (property.GetIndexParameters().Length != 0)
        {
            throw new InvalidOperationException($"Managed glue property '{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}' is an indexed property.");
        }

        var getter = property.GetMethod;
        if (getter is null || getter.IsStatic)
        {
            throw new InvalidOperationException($"Managed glue property '{ManagedGlueCode.TypeName(DeclaringType)}.{PropertyName}' is not an instance gettable property.");
        }
    }
}

public sealed record ManagedGlueUtf8ByteCountExpression(ManagedGlueExpression Value) : ManagedGlueExpression
{
    public override string ToCode()
        => $"InteropUtf8.GetByteCount({Value.ToCode()})";

    public override void Validate()
        => Value.Validate();
}

public sealed record ManagedGlueUtf8CopyExpression(ManagedGlueExpression Value, string DestinationPointerParameterName, string DestinationCapacityParameterName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"InteropUtf8.CopyString({Value.ToCode()}, {DestinationPointerParameterName}, {DestinationCapacityParameterName})";

    public override void Validate()
        => Value.Validate();
}

public sealed record ManagedGlueArrayLengthExpression(ManagedGlueExpression Array) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{Array.ToCode()}.Length";

    public override void Validate()
        => Array.Validate();
}

public sealed record ManagedGlueArrayElementExpression(ManagedGlueExpression Array, string IndexParameterName) : ManagedGlueExpression
{
    public override string ToCode()
        => $"{Array.ToCode()}[{IndexParameterName}]";

    public override void Validate()
        => Array.Validate();
}

public sealed record ManagedGlueConstructorExpression(
    Type DeclaringType,
    IReadOnlyList<Type> ParameterTypes,
    IReadOnlyList<ManagedGlueExpression> Arguments) : ManagedGlueExpression
{
    public override string ToCode()
        => $"new {ManagedGlueCode.TypeName(DeclaringType)}({string.Join(", ", Arguments.Select(a => a.ToCode()))})";

    public override void Validate()
    {
        if (ParameterTypes.Count != Arguments.Count)
        {
            throw new InvalidOperationException($"Managed glue constructor '{ManagedGlueCode.TypeName(DeclaringType)}' has {Arguments.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)} arguments for {ParameterTypes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)} parameters.");
        }

        foreach (var argument in Arguments)
        {
            argument.Validate();
        }

        var ctor = DeclaringType.GetConstructor(ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Managed glue constructor '{ManagedGlueCode.TypeName(DeclaringType)}' could not be resolved.");
    }
}

public sealed record ManagedGlueRawExpression(string Code) : ManagedGlueExpression
{
    public override string ToCode()
        => Code;

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            throw new InvalidOperationException("Managed glue raw expression cannot be empty.");
        }
    }
}

internal static class ManagedGlueCode
{
    public static string TypeName(Type type)
    {
        if (type == typeof(string))
        {
            return "string";
        }

        if (type == typeof(int))
        {
            return "int";
        }

        if (type == typeof(byte))
        {
            return "byte";
        }

        if (type == typeof(long))
        {
            return "long";
        }

        if (type == typeof(float))
        {
            return "float";
        }

        if (type == typeof(double))
        {
            return "double";
        }

        if (type == typeof(bool))
        {
            return "bool";
        }

        if (type == typeof(void))
        {
            return "void";
        }

        if (type == typeof(IntPtr))
        {
            return "IntPtr";
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType()
                ?? throw new InvalidOperationException($"Array type '{type}' does not expose an element type.");
            return $"{TypeName(elementType)}[]";
        }

        if (type.Namespace is "System" or "System.IO" or "Rustlyn.Interop")
        {
            return type.Name;
        }

        return type.FullName?.Replace('+', '.') ?? type.Name;
    }
}
