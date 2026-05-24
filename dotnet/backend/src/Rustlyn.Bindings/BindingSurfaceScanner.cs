using System.Reflection;

namespace Rustlyn.Bindings;

/// <summary>
/// Scans .NET types via reflection to produce <see cref="ManagedApiRequirement"/> entries.
/// This enables metadata-driven binding surface expansion instead of manual hardcoding.
/// </summary>
public static class BindingSurfaceScanner
{
    public static IReadOnlyList<ScannedBinding> CreateStaticScalarMethodBindings(params StaticScalarMethodBindingRequest[] requests)
    {
        ArgumentNullException.ThrowIfNull(requests);
        return requests.Select(CreateStaticScalarMethodBinding).ToArray();
    }

    public static IReadOnlyList<ScannedBinding> CreateStaticObjectHandleMethodBindings(params StaticObjectHandleMethodBindingRequest[] requests)
    {
        ArgumentNullException.ThrowIfNull(requests);
        return requests.Select(CreateStaticObjectHandleMethodBinding).ToArray();
    }

    public static ScannedBinding CreateStaticScalarMethodBinding(StaticScalarMethodBindingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = request.DeclaringType.GetMethod(request.MethodName, BindingFlags.Public | BindingFlags.Static, request.ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Static scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' could not be resolved.");
        if (!method.IsStatic)
        {
            throw new InvalidOperationException($"Static scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' is not static.");
        }

        if (!IsScalarBindingType(method.ReturnType))
        {
            throw new InvalidOperationException($"Static scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported return type '{method.ReturnType}'.");
        }

        foreach (var parameterType in request.ParameterTypes)
        {
            if (!IsScalarBindingType(parameterType))
            {
                throw new InvalidOperationException($"Static scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported parameter type '{parameterType}'.");
            }
        }

        var parameterNames = CreateStableParameterNames(request.ParameterTypes.Count);
        var symbol = CreateSymbol(method, request.ParameterTypes);
        var helperMethodName = CreateHelperMethodName(symbol);
        var glueParameters = parameterNames.Zip(request.ParameterTypes, static (name, type) => new ManagedGlueParameter(ToManagedGlueTypeName(type), name)).ToArray();
        var arguments = glueParameters.Select(static parameter => ManagedGlueExpression.Parameter(parameter.Name)).ToArray();
        var result = CreateScalarResult(
            method.ReturnType,
            ManagedGlueExpression.StaticMethod(request.DeclaringType, request.MethodName, request.ParameterTypes, arguments));
        var operation = ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", result);
        var parametersWithException = glueParameters.Append(new ManagedGlueParameter("IntPtr", "exceptionOutPointer")).ToArray();
        var glue = new ManagedGlueBinding(symbol, helperMethodName, parametersWithException, operation);
        var rustParameterList = parameterNames.Zip(request.ParameterTypes, static (name, type) => $"{name}: {ToRustTypeName(type)}");
        var wrapper = new RustWrapperMethod(
            request.Container,
            $"pub fn {ToSnakeCase(request.MethodName)}({string.Join(", ", rustParameterList)}) -> Result<{ToRustTypeName(method.ReturnType)}, Exception>",
            symbol,
            parameterNames,
            parameterNames.Length == 1 ? parameterNames[0] : "value",
            RustWrapperResult.Scalar(ToRustTypeName(method.ReturnType)));

        var requirement = ManagedApiRequirement.Method(
            FormatMethodDisplayName(request.DeclaringType, request.MethodName, request.ParameterTypes.ToArray()),
            request.DeclaringType,
            request.MethodName,
            request.ParameterTypes);
        return new ScannedBinding(requirement, glue, wrapper);
    }

    public static ScannedBinding CreateStaticObjectHandleMethodBinding(StaticObjectHandleMethodBindingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = request.DeclaringType.GetMethod(request.MethodName, BindingFlags.Public | BindingFlags.Static, request.ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Static object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' could not be resolved.");
        if (!method.IsStatic)
        {
            throw new InvalidOperationException($"Static object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' is not static.");
        }

        if (!IsObjectHandleBindingType(method.ReturnType))
        {
            throw new InvalidOperationException($"Static object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported return type '{method.ReturnType}'.");
        }

        foreach (var parameterType in request.ParameterTypes)
        {
            if (!IsObjectHandleBindingType(parameterType))
            {
                throw new InvalidOperationException($"Static object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported parameter type '{parameterType}'.");
            }
        }

        var discoveredParameterNames = method.GetParameters()
            .Select(static parameter => string.IsNullOrWhiteSpace(parameter.Name) ? null : parameter.Name)
            .ToArray();
        var managedParameterNames = new string[discoveredParameterNames.Length];
        for (var index = 0; index < discoveredParameterNames.Length; index++)
        {
            managedParameterNames[index] = discoveredParameterNames[index]
                ?? $"value{(index + 1).ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        var symbol = CreateSymbol(method, request.ParameterTypes);
        var helperMethodName = CreateHelperMethodName(symbol);
        var glueParameters = managedParameterNames.Zip(request.ParameterTypes, static (name, type) => new ManagedGlueParameter("int", $"{name}Handle")).ToArray();
        var arguments = glueParameters.Zip(request.ParameterTypes, static (parameter, type) => ManagedGlueExpression.ManagedObject(type, parameter.Name)).ToArray();
        var result = ManagedGlueResult.ObjectHandle(
            ManagedGlueExpression.StaticMethod(request.DeclaringType, request.MethodName, request.ParameterTypes, arguments));
        var operation = ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", result);
        var parametersWithException = glueParameters.Append(new ManagedGlueParameter("IntPtr", "exceptionOutPointer")).ToArray();
        var glue = new ManagedGlueBinding(symbol, helperMethodName, parametersWithException, operation);

        var rustParameterNames = managedParameterNames.Select(ToSnakeCase).ToArray();
        var rustParameters = rustParameterNames.Zip(request.ParameterTypes, static (name, type) => $"{name}: &{ToRustObjectHandleTypeName(type)}").ToArray();
        var wrapper = new RustWrapperMethod(
            request.Container,
            $"pub fn {ToSnakeCase(request.MethodName)}({string.Join(", ", rustParameters)}) -> Result<{ToRustObjectHandleTypeName(method.ReturnType)}, Exception>",
            symbol,
            rustParameterNames.Select(static name => $"{name}.handle()").ToArray(),
            "object_handle",
            RustWrapperResult.ObjectHandle(ToRustObjectHandleTypeName(method.ReturnType)));

        var requirement = ManagedApiRequirement.Method(
            FormatMethodDisplayName(request.DeclaringType, request.MethodName, request.ParameterTypes.ToArray()),
            request.DeclaringType,
            request.MethodName,
            request.ParameterTypes);
        return new ScannedBinding(requirement, glue, wrapper);
    }

    /// <summary>
    /// Creates a binding for an instance method that takes scalar parameters and returns a scalar.
    /// The instance is received as an object handle (GC-tracked int).
    /// </summary>
    public static ScannedBinding CreateInstanceScalarMethodBinding(InstanceScalarMethodBindingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = request.DeclaringType.GetMethod(request.MethodName, BindingFlags.Public | BindingFlags.Instance, request.ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Instance scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' could not be resolved.");
        if (method.IsStatic)
        {
            throw new InvalidOperationException($"Instance scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' is static.");
        }

        if (!IsScalarBindingType(method.ReturnType))
        {
            throw new InvalidOperationException($"Instance scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported return type '{method.ReturnType}'.");
        }

        foreach (var parameterType in request.ParameterTypes)
        {
            if (!IsScalarBindingType(parameterType))
            {
                throw new InvalidOperationException($"Instance scalar binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported parameter type '{parameterType}'.");
            }
        }

        var parameterNames = CreateStableParameterNames(request.ParameterTypes.Count);
        var symbol = CreateInstanceSymbol(method, request.ParameterTypes);
        var helperMethodName = CreateHelperMethodName(symbol);

        var selfParameter = new ManagedGlueParameter("int", "selfHandle");
        var selfExpression = ManagedGlueExpression.ManagedObject(request.DeclaringType, "selfHandle");
        var glueParameters = parameterNames.Zip(request.ParameterTypes, static (name, type) => new ManagedGlueParameter(ToManagedGlueTypeName(type), name)).ToArray();
        var arguments = glueParameters.Select(static parameter => ManagedGlueExpression.Parameter(parameter.Name)).ToArray();

        var result = CreateScalarResult(
            method.ReturnType,
            ManagedGlueExpression.InstanceMethod(selfExpression, request.DeclaringType, request.MethodName, request.ParameterTypes, arguments));
        var operation = ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", result);
        var allGlueParameters = new[] { selfParameter }.Concat(glueParameters).Append(new ManagedGlueParameter("IntPtr", "exceptionOutPointer")).ToArray();
        var glue = new ManagedGlueBinding(symbol, helperMethodName, allGlueParameters, operation);

        var rustSelfParam = $"self_handle: &{ToRustObjectHandleTypeName(request.DeclaringType)}";
        var rustParameterList = parameterNames.Zip(request.ParameterTypes, static (name, type) => $"{name}: {ToRustTypeName(type)}");
        var allRustParams = new[] { rustSelfParam }.Concat(rustParameterList);
        var rustCallArgs = new[] { "self_handle.handle()" }.Concat(parameterNames);

        var wrapper = new RustWrapperMethod(
            request.Container,
            $"pub fn {ToSnakeCase(request.MethodName)}({string.Join(", ", allRustParams)}) -> Result<{ToRustTypeName(method.ReturnType)}, Exception>",
            symbol,
            rustCallArgs.ToArray(),
            parameterNames.Length == 1 ? parameterNames[0] : "value",
            RustWrapperResult.Scalar(ToRustTypeName(method.ReturnType)));

        var requirement = ManagedApiRequirement.Method(
            FormatMethodDisplayName(request.DeclaringType, request.MethodName, request.ParameterTypes.ToArray()),
            request.DeclaringType,
            request.MethodName,
            request.ParameterTypes);
        return new ScannedBinding(requirement, glue, wrapper);
    }

    /// <summary>
    /// Creates a binding for an instance method that takes scalar/object-handle parameters and returns an object handle.
    /// The instance is received as an object handle (GC-tracked int).
    /// </summary>
    public static ScannedBinding CreateInstanceObjectHandleMethodBinding(InstanceObjectHandleMethodBindingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = request.DeclaringType.GetMethod(request.MethodName, BindingFlags.Public | BindingFlags.Instance, request.ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Instance object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' could not be resolved.");
        if (method.IsStatic)
        {
            throw new InvalidOperationException($"Instance object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' is static.");
        }

        if (!IsObjectHandleBindingType(method.ReturnType))
        {
            throw new InvalidOperationException($"Instance object-handle binding target '{request.DeclaringType.FullName}.{request.MethodName}' has unsupported return type '{method.ReturnType}'.");
        }

        var discoveredParameterNames = method.GetParameters()
            .Select(static parameter => string.IsNullOrWhiteSpace(parameter.Name) ? null : parameter.Name)
            .ToArray();
        var managedParameterNames = new string[discoveredParameterNames.Length];
        for (var index = 0; index < discoveredParameterNames.Length; index++)
        {
            managedParameterNames[index] = discoveredParameterNames[index]
                ?? $"value{(index + 1).ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        var symbol = CreateInstanceSymbol(method, request.ParameterTypes);
        var helperMethodName = CreateHelperMethodName(symbol);

        var selfParameter = new ManagedGlueParameter("int", "selfHandle");
        var selfExpression = ManagedGlueExpression.ManagedObject(request.DeclaringType, "selfHandle");
        var glueParameters = managedParameterNames.Zip(request.ParameterTypes, static (name, type) => new ManagedGlueParameter("int", $"{name}Handle")).ToArray();
        var arguments = glueParameters.Zip(request.ParameterTypes, static (parameter, type) => ManagedGlueExpression.ManagedObject(type, parameter.Name)).ToArray();

        var result = ManagedGlueResult.ObjectHandle(
            ManagedGlueExpression.InstanceMethod(selfExpression, request.DeclaringType, request.MethodName, request.ParameterTypes, arguments));
        var operation = ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", result);
        var allGlueParameters = new[] { selfParameter }.Concat(glueParameters).Append(new ManagedGlueParameter("IntPtr", "exceptionOutPointer")).ToArray();
        var glue = new ManagedGlueBinding(symbol, helperMethodName, allGlueParameters, operation);

        var rustParameterNames = managedParameterNames.Select(ToSnakeCase).ToArray();
        var rustSelfParam = $"self_handle: &{ToRustObjectHandleTypeName(request.DeclaringType)}";
        var rustParameters = rustParameterNames.Zip(request.ParameterTypes, static (name, type) => $"{name}: &{ToRustObjectHandleTypeName(type)}").ToArray();
        var allRustParams = new[] { rustSelfParam }.Concat(rustParameters);
        var rustCallArgs = new[] { "self_handle.handle()" }.Concat(rustParameterNames.Select(static name => $"{name}.handle()"));

        var wrapper = new RustWrapperMethod(
            request.Container,
            $"pub fn {ToSnakeCase(request.MethodName)}({string.Join(", ", allRustParams)}) -> Result<{ToRustObjectHandleTypeName(method.ReturnType)}, Exception>",
            symbol,
            rustCallArgs.ToArray(),
            "object_handle",
            RustWrapperResult.ObjectHandle(ToRustObjectHandleTypeName(method.ReturnType)));

        var requirement = ManagedApiRequirement.Method(
            FormatMethodDisplayName(request.DeclaringType, request.MethodName, request.ParameterTypes.ToArray()),
            request.DeclaringType,
            request.MethodName,
            request.ParameterTypes);
        return new ScannedBinding(requirement, glue, wrapper);
    }

    /// <summary>
    /// Creates a binding for a public constructor that takes scalar/object-handle parameters.
    /// Returns an object handle representing the new instance.
    /// </summary>
    public static ScannedBinding CreateConstructorBinding(ConstructorBindingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var ctor = request.DeclaringType.GetConstructor(request.ParameterTypes.ToArray())
            ?? throw new InvalidOperationException($"Constructor binding target '{request.DeclaringType.FullName}' with parameter types [{string.Join(", ", request.ParameterTypes.Select(FormatDiagnosticTypeName))}] could not be resolved.");

        foreach (var parameterType in request.ParameterTypes)
        {
            if (!IsScalarBindingType(parameterType) && !IsObjectHandleBindingType(parameterType))
            {
                throw new InvalidOperationException($"Constructor binding target '{request.DeclaringType.FullName}' has unsupported parameter type '{parameterType}'.");
            }
        }

        var discoveredParameterNames = ctor.GetParameters()
            .Select(static parameter => string.IsNullOrWhiteSpace(parameter.Name) ? null : parameter.Name)
            .ToArray();
        var managedParameterNames = new string[discoveredParameterNames.Length];
        for (var index = 0; index < discoveredParameterNames.Length; index++)
        {
            managedParameterNames[index] = discoveredParameterNames[index]
                ?? $"value{(index + 1).ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        }

        var fullName = request.DeclaringType.FullName
            ?? throw new InvalidOperationException($"Constructor target type does not expose a full name.");
        var typePart = string.Join("_", fullName.Split('.').Select(static part => part.ToLowerInvariant()));
        var suffix = request.ParameterTypes.Count > 0
            ? string.Join("_", request.ParameterTypes.Select(ToSymbolTypeSuffix))
            : "void";
        var symbol = $"rustlyn_bindgen_{typePart}_new_{suffix}";
        var helperMethodName = CreateHelperMethodName(symbol);

        var glueParameters = new List<ManagedGlueParameter>();
        var arguments = new List<ManagedGlueExpression>();
        for (var i = 0; i < request.ParameterTypes.Count; i++)
        {
            var paramType = request.ParameterTypes[i];
            var paramName = managedParameterNames[i];
            if (IsScalarBindingType(paramType))
            {
                glueParameters.Add(new ManagedGlueParameter(ToManagedGlueTypeName(paramType), paramName));
                arguments.Add(ManagedGlueExpression.Parameter(paramName));
            }
            else
            {
                glueParameters.Add(new ManagedGlueParameter("int", $"{paramName}Handle"));
                arguments.Add(ManagedGlueExpression.ManagedObject(paramType, $"{paramName}Handle"));
            }
        }

        var constructorExpression = ManagedGlueExpression.Constructor(request.DeclaringType, request.ParameterTypes, arguments);
        var result = ManagedGlueResult.ObjectHandle(constructorExpression);
        var operation = ManagedGlueOperation.WriteExceptionOut("exceptionOutPointer", result);
        var allGlueParameters = glueParameters.Append(new ManagedGlueParameter("IntPtr", "exceptionOutPointer")).ToArray();
        var glue = new ManagedGlueBinding(symbol, helperMethodName, allGlueParameters, operation);

        var rustParameterNames = managedParameterNames.Select(ToSnakeCase).ToArray();
        var rustParams = new List<string>();
        var rustCallArgs = new List<string>();
        for (var i = 0; i < request.ParameterTypes.Count; i++)
        {
            var paramType = request.ParameterTypes[i];
            var rustName = rustParameterNames[i];
            if (IsScalarBindingType(paramType))
            {
                rustParams.Add($"{rustName}: {ToRustTypeName(paramType)}");
                rustCallArgs.Add(rustName);
            }
            else
            {
                rustParams.Add($"{rustName}: &{ToRustObjectHandleTypeName(paramType)}");
                rustCallArgs.Add($"{rustName}.handle()");
            }
        }

        var rustReturnType = ToRustObjectHandleTypeName(request.DeclaringType);
        var wrapper = new RustWrapperMethod(
            request.Container,
            $"pub fn new({string.Join(", ", rustParams)}) -> Result<{rustReturnType}, Exception>",
            symbol,
            rustCallArgs.ToArray(),
            "object_handle",
            RustWrapperResult.ObjectHandle(rustReturnType));

        var displayName = $"{request.DeclaringType.FullName}..ctor({string.Join(", ", request.ParameterTypes.Select(FormatTypeName))})";
        var requirement = ManagedApiRequirement.Constructor(displayName, request.DeclaringType, request.ParameterTypes);
        return new ScannedBinding(requirement, glue, wrapper);
    }

    /// <summary>
    /// Scans a type for public methods and properties that match supported binding signatures.
    /// Returns requirements for methods whose parameters are all bindable types.
    /// </summary>
    public static IReadOnlyList<ManagedApiRequirement> ScanType(Type type)
    {
        return ScanTypeWithDiagnostics(type).Requirements;
    }

    public static BindingSurfaceScanResult ScanTypeWithDiagnostics(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var requirements = new List<ManagedApiRequirement>();
        var unsupportedShapes = new List<BindingScanUnsupportedShape>();

        // Always add a type requirement
        var typeName = type.FullName ?? type.Name;
        requirements.Add(ManagedApiRequirement.ForType(typeName, type));

        var supportedMethodCandidates = new List<(MethodInfo Method, string DisplayName, ManagedApiRequirement Requirement)>();

        // Scan public static and instance methods.
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(static method => method.Name, StringComparer.Ordinal)
            .ThenBy(static method => FormatParameterList(method.GetParameters().Select(static parameter => parameter.ParameterType)), StringComparer.Ordinal))
        {
            if (method.IsSpecialName) continue; // skip property accessors
            var parameters = method.GetParameters();
            var paramTypes = parameters.Select(p => p.ParameterType).ToArray();
            var displayName = FormatMethodDisplayName(type, method.Name, paramTypes);
            var unsupportedReason = GetUnsupportedMethodReason(method, parameters);
            if (unsupportedReason is not null)
            {
                unsupportedShapes.Add(new BindingScanUnsupportedShape(displayName, ManagedApiRequirementKind.Method, method.Name, unsupportedReason));
                continue;
            }

            supportedMethodCandidates.Add((method, displayName, ManagedApiRequirement.Method(displayName, type, method.Name, paramTypes)));
        }

        foreach (var collisionGroup in supportedMethodCandidates
            .GroupBy(static candidate => CreateProjectedRustMethodSignatureKey(candidate.Method), StringComparer.Ordinal)
            .Where(static group => group.Count() > 1))
        {
            var collidingMembers = string.Join(", ", collisionGroup.Select(static candidate => candidate.DisplayName).Order(StringComparer.Ordinal));
            foreach (var candidate in collisionGroup)
            {
                unsupportedShapes.Add(new BindingScanUnsupportedShape(
                    candidate.DisplayName,
                    ManagedApiRequirementKind.Method,
                    candidate.Method.Name,
                    $"overload projects to duplicate Rust wrapper signature '{collisionGroup.Key}' with {collidingMembers}"));
            }
        }

        var collisionKeys = supportedMethodCandidates
            .GroupBy(static candidate => CreateProjectedRustMethodSignatureKey(candidate.Method), StringComparer.Ordinal)
            .Where(static group => group.Count() > 1)
            .Select(static group => group.Key)
            .ToHashSet(StringComparer.Ordinal);
        foreach (var candidate in supportedMethodCandidates)
        {
            if (!collisionKeys.Contains(CreateProjectedRustMethodSignatureKey(candidate.Method)))
            {
                requirements.Add(candidate.Requirement);
            }
        }

        // Scan public static and instance properties with getters.
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(static property => property.Name, StringComparer.Ordinal))
        {
            var displayName = $"{typeName}.{property.Name}";
            var unsupportedReason = GetUnsupportedPropertyReason(property);
            if (unsupportedReason is not null)
            {
                unsupportedShapes.Add(new BindingScanUnsupportedShape(displayName, ManagedApiRequirementKind.Property, property.Name, unsupportedReason));
                continue;
            }

            requirements.Add(ManagedApiRequirement.Property(displayName, type, property.Name));
        }

        foreach (var eventInfo in type.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(static eventInfo => eventInfo.Name, StringComparer.Ordinal))
        {
            var displayName = $"{typeName}.{eventInfo.Name}";
            unsupportedShapes.Add(new BindingScanUnsupportedShape(
                displayName,
                ManagedApiRequirementKind.Event,
                eventInfo.Name,
                "events are not supported; project explicit delegate registration methods instead"));
        }

        return new BindingSurfaceScanResult(requirements, unsupportedShapes);
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
    /// Scans an assembly loaded from a file path, filtering types by namespace pattern.
    /// Returns all bindable requirements and diagnostics for unsupported shapes.
    /// The namespace filter supports trailing wildcard (e.g., "System.IO.*") or exact match.
    /// </summary>
    public static AssemblyScanResult ScanAssembly(string assemblyPath, string? namespaceFilter = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath);

        var fullPath = Path.GetFullPath(assemblyPath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Assembly not found: {fullPath}", fullPath);
        }

        var assembly = Assembly.LoadFrom(fullPath);
        return ScanLoadedAssembly(assembly, namespaceFilter);
    }

    /// <summary>
    /// Scans a loaded assembly, filtering types by namespace pattern.
    /// </summary>
    public static AssemblyScanResult ScanLoadedAssembly(Assembly assembly, string? namespaceFilter = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var allRequirements = new List<ManagedApiRequirement>();
        var allUnsupported = new List<BindingScanUnsupportedShape>();
        var scannedTypes = new List<Type>();

        foreach (var type in assembly.GetExportedTypes()
            .Where(t => !t.IsGenericTypeDefinition && !t.IsInterface && !t.IsAbstract)
            .Where(t => MatchesNamespaceFilter(t, namespaceFilter))
            .OrderBy(t => t.FullName, StringComparer.Ordinal))
        {
            var result = ScanTypeWithDiagnostics(type);
            allRequirements.AddRange(result.Requirements);
            allUnsupported.AddRange(result.UnsupportedShapes);
            scannedTypes.Add(type);
        }

        return new AssemblyScanResult(
            assembly.GetName().Name ?? assembly.FullName ?? "Unknown",
            scannedTypes,
            allRequirements,
            allUnsupported);
    }

    /// <summary>
    /// Scans an assembly and produces complete ScannedBinding entries for all supported
    /// static methods — ready for code generation without manual binding request construction.
    /// </summary>
    public static IReadOnlyList<ScannedBinding> AutoGenerateBindings(Assembly assembly, string? namespaceFilter = null, RustWrapperContainer? container = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var targetContainer = container ?? RustWrapperContainer.Math;
        var bindings = new List<ScannedBinding>();

        foreach (var type in assembly.GetExportedTypes()
            .Where(t => !t.IsGenericTypeDefinition && !t.IsInterface && !t.IsAbstract)
            .Where(t => MatchesNamespaceFilter(t, namespaceFilter))
            .OrderBy(t => t.FullName, StringComparer.Ordinal))
        {
            // Static methods
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName && !m.IsGenericMethodDefinition && !m.ContainsGenericParameters)
                .OrderBy(m => m.Name, StringComparer.Ordinal))
            {
                var parameters = method.GetParameters();
                if (parameters.Any(p => p.ParameterType.IsByRef)) continue;

                var paramTypes = parameters.Select(p => p.ParameterType).ToList();

                if (paramTypes.All(IsScalarBindingType) && IsScalarBindingType(method.ReturnType))
                {
                    try
                    {
                        var request = new StaticScalarMethodBindingRequest(type, method.Name, paramTypes, targetContainer);
                        bindings.Add(CreateStaticScalarMethodBinding(request));
                    }
                    catch (InvalidOperationException) { }
                    catch (NotSupportedException) { }
                }
                else if (paramTypes.All(t => IsObjectHandleBindingType(t) || IsScalarBindingType(t))
                    && IsObjectHandleBindingType(method.ReturnType))
                {
                    try
                    {
                        var request = new StaticObjectHandleMethodBindingRequest(type, method.Name, paramTypes, targetContainer);
                        bindings.Add(CreateStaticObjectHandleMethodBinding(request));
                    }
                    catch (InvalidOperationException) { }
                    catch (NotSupportedException) { }
                }
            }

            // Instance methods
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName && !m.IsGenericMethodDefinition && !m.ContainsGenericParameters)
                .OrderBy(m => m.Name, StringComparer.Ordinal))
            {
                var parameters = method.GetParameters();
                if (parameters.Any(p => p.ParameterType.IsByRef)) continue;

                var paramTypes = parameters.Select(p => p.ParameterType).ToList();

                if (paramTypes.All(IsScalarBindingType) && IsScalarBindingType(method.ReturnType))
                {
                    try
                    {
                        var request = new InstanceScalarMethodBindingRequest(type, method.Name, paramTypes, targetContainer);
                        bindings.Add(CreateInstanceScalarMethodBinding(request));
                    }
                    catch (InvalidOperationException) { }
                    catch (NotSupportedException) { }
                }
                else if (paramTypes.All(t => IsObjectHandleBindingType(t) || IsScalarBindingType(t))
                    && IsObjectHandleBindingType(method.ReturnType))
                {
                    try
                    {
                        var request = new InstanceObjectHandleMethodBindingRequest(type, method.Name, paramTypes, targetContainer);
                        bindings.Add(CreateInstanceObjectHandleMethodBinding(request));
                    }
                    catch (InvalidOperationException) { }
                    catch (NotSupportedException) { }
                }
            }

            // Constructors
            foreach (var ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Where(c => !c.ContainsGenericParameters)
                .OrderBy(c => c.GetParameters().Length))
            {
                var parameters = ctor.GetParameters();
                if (parameters.Any(p => p.ParameterType.IsByRef)) continue;

                var paramTypes = parameters.Select(p => p.ParameterType).ToList();
                if (paramTypes.All(t => IsScalarBindingType(t) || IsObjectHandleBindingType(t)))
                {
                    try
                    {
                        var request = new ConstructorBindingRequest(type, paramTypes, targetContainer);
                        bindings.Add(CreateConstructorBinding(request));
                    }
                    catch (InvalidOperationException) { }
                    catch (NotSupportedException) { }
                }
            }

            // Events — record delegate analysis for each event but don't emit bindings yet
            // (event subscription requires callback bridge infrastructure, which is tracked separately)
            foreach (var evt in type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .OrderBy(e => e.Name, StringComparer.Ordinal))
            {
                if (evt.EventHandlerType is null) continue;
                var analysis = AnalyzeEvent(evt);
                if (analysis.DelegateAnalysis.IsBindable && analysis.HasPublicAdd && analysis.HasPublicRemove)
                {
                    // Event is bindable — will be emitted when callback bridge generation is implemented
                    // For now, this validates the event surface is reachable
                }
            }
        }

        return bindings;
    }

    /// <summary>
    /// Generates bindings for a single concrete type (including closed generic types like List&lt;int&gt;).
    /// Scans all public static methods, instance methods, and constructors with supported signatures.
    /// </summary>
    public static IReadOnlyList<ScannedBinding> GenerateBindingsForType(Type type, RustWrapperContainer container)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.IsGenericTypeDefinition)
        {
            throw new ArgumentException($"Type '{type}' is an open generic definition. Provide a closed generic type (e.g., typeof(List<int>)).", nameof(type));
        }

        var bindings = new List<ScannedBinding>();

        // Static methods
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName && !m.IsGenericMethodDefinition && !m.ContainsGenericParameters)
            .OrderBy(m => m.Name, StringComparer.Ordinal))
        {
            var parameters = method.GetParameters();
            if (parameters.Any(p => p.ParameterType.IsByRef)) continue;

            var paramTypes = parameters.Select(p => p.ParameterType).ToList();
            if (paramTypes.All(IsScalarBindingType) && IsScalarBindingType(method.ReturnType))
            {
                try
                {
                    var request = new StaticScalarMethodBindingRequest(type, method.Name, paramTypes, container);
                    bindings.Add(CreateStaticScalarMethodBinding(request));
                }
                catch (InvalidOperationException) { }
                catch (NotSupportedException) { }
            }
            else if (paramTypes.All(t => IsObjectHandleBindingType(t) || IsScalarBindingType(t)) && IsObjectHandleBindingType(method.ReturnType))
            {
                try
                {
                    var request = new StaticObjectHandleMethodBindingRequest(type, method.Name, paramTypes, container);
                    bindings.Add(CreateStaticObjectHandleMethodBinding(request));
                }
                catch (InvalidOperationException) { }
                catch (NotSupportedException) { }
            }
        }

        // Instance methods
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName && !m.IsGenericMethodDefinition && !m.ContainsGenericParameters)
            .OrderBy(m => m.Name, StringComparer.Ordinal))
        {
            var parameters = method.GetParameters();
            if (parameters.Any(p => p.ParameterType.IsByRef)) continue;

            var paramTypes = parameters.Select(p => p.ParameterType).ToList();
            if (paramTypes.All(IsScalarBindingType) && IsScalarBindingType(method.ReturnType))
            {
                try
                {
                    var request = new InstanceScalarMethodBindingRequest(type, method.Name, paramTypes, container);
                    bindings.Add(CreateInstanceScalarMethodBinding(request));
                }
                catch (InvalidOperationException) { }
                catch (NotSupportedException) { }
            }
            else if (paramTypes.All(t => IsObjectHandleBindingType(t) || IsScalarBindingType(t)) && IsObjectHandleBindingType(method.ReturnType))
            {
                try
                {
                    var request = new InstanceObjectHandleMethodBindingRequest(type, method.Name, paramTypes, container);
                    bindings.Add(CreateInstanceObjectHandleMethodBinding(request));
                }
                catch (InvalidOperationException) { }
                catch (NotSupportedException) { }
            }
        }

        // Constructors
        foreach (var ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Where(c => !c.ContainsGenericParameters)
            .OrderBy(c => c.GetParameters().Length))
        {
            var parameters = ctor.GetParameters();
            if (parameters.Any(p => p.ParameterType.IsByRef)) continue;

            var paramTypes = parameters.Select(p => p.ParameterType).ToList();
            if (paramTypes.All(t => IsScalarBindingType(t) || IsObjectHandleBindingType(t)))
            {
                try
                {
                    var request = new ConstructorBindingRequest(type, paramTypes, container);
                    bindings.Add(CreateConstructorBinding(request));
                }
                catch (InvalidOperationException) { }
                catch (NotSupportedException) { }
            }
        }

        return bindings;
    }

    private static bool MatchesNamespaceFilter(Type type, string? filter)
    {
        if (string.IsNullOrEmpty(filter)) return true;

        var ns = type.Namespace ?? string.Empty;
        if (filter.EndsWith(".*", StringComparison.Ordinal))
        {
            var prefix = filter[..^2];
            return ns.StartsWith(prefix, StringComparison.Ordinal);
        }

        return string.Equals(ns, filter, StringComparison.Ordinal);
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
            if (!type.IsSZArray)
            {
                return false;
            }

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

    private static string? GetUnsupportedMethodReason(MethodInfo method, ParameterInfo[] parameters)
    {
        if (method.IsGenericMethodDefinition || method.ContainsGenericParameters)
        {
            return "generic or open methods are not supported";
        }

        foreach (var parameter in parameters)
        {
            if (parameter.ParameterType.IsByRef)
            {
                var direction = parameter.IsOut ? "out" : "ref";
                return $"{direction} parameter '{parameter.Name}' is not supported";
            }

            var typeReason = GetUnsupportedTypeReason(parameter.ParameterType);
            if (typeReason is not null)
            {
                return $"parameter '{parameter.Name}' has unsupported type '{FormatDiagnosticTypeName(parameter.ParameterType)}': {typeReason}";
            }
        }

        if (!IsReturnTypeBindable(method.ReturnType))
        {
            return $"return type '{FormatDiagnosticTypeName(method.ReturnType)}' is unsupported: {GetUnsupportedTypeReason(method.ReturnType)}";
        }

        return null;
    }

    private static string? GetUnsupportedPropertyReason(PropertyInfo property)
    {
        if (property.GetMethod is null)
        {
            return "property has no public getter";
        }

        if (property.GetIndexParameters().Length != 0)
        {
            return "indexed properties are not supported";
        }

        var typeReason = GetUnsupportedTypeReason(property.PropertyType);
        if (typeReason is not null)
        {
            return $"property type '{FormatDiagnosticTypeName(property.PropertyType)}' is unsupported: {typeReason}";
        }

        return null;
    }

    private static string? GetUnsupportedTypeReason(Type type)
    {
        if (IsTypeBindable(type))
        {
            return null;
        }

        if (type.IsByRef)
        {
            return "by-reference types are not supported";
        }

        if (type.IsArray)
        {
            if (!type.IsSZArray)
            {
                return $"array rank {type.GetArrayRank().ToString(System.Globalization.CultureInfo.InvariantCulture)} is not supported";
            }

            var elementType = type.GetElementType();
            var elementReason = elementType is null ? "array element type could not be resolved" : GetUnsupportedTypeReason(elementType);
            return elementReason is null
                ? null
                : $"array element type '{FormatDiagnosticTypeName(elementType!)}' is unsupported: {elementReason}";
        }

        if (typeof(Delegate).IsAssignableFrom(type))
        {
            return "delegate types are not supported; use explicit callback ABI metadata";
        }

        if (type.ContainsGenericParameters)
        {
            return "generic or open types are not supported";
        }

        if (type.IsGenericType)
        {
            var genericArguments = string.Join(", ", type.GetGenericArguments().Select(FormatDiagnosticTypeName));
            return $"generic constructed types are not supported; generic arguments: {genericArguments}";
        }

        if (type.IsEnum)
        {
            return $"enum backing type '{FormatDiagnosticTypeName(Enum.GetUnderlyingType(type))}' is not supported";
        }

        return "type is not in the bindable scalar, string, pointer, enum, or array set";
    }

    private static string FormatMethodDisplayName(Type type, string methodName, Type[] paramTypes)
    {
        var typeName = type.FullName ?? type.Name;
        var paramNames = FormatParameterList(paramTypes);
        return $"{typeName}.{methodName}({paramNames})";
    }

    private static string FormatParameterList(IEnumerable<Type> parameterTypes)
        => string.Join(", ", parameterTypes.Select(FormatTypeName));

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

    private static string FormatDiagnosticTypeName(Type type)
        => type.FullName?.Replace('+', '.') ?? type.Name;

    private static bool IsScalarBindingType(Type type)
    {
        return type == typeof(int)
            || type == typeof(long)
            || type == typeof(float)
            || type == typeof(double);
    }

    private static bool IsObjectHandleBindingType(Type type)
    {
        return type == typeof(string)
            || type == typeof(string[])
            || type == typeof(int[])
            || type == typeof(byte[]);
    }

    private static string[] CreateStableParameterNames(int parameterCount)
    {
        return parameterCount switch
        {
            0 => [],
            1 => ["value"],
            2 => ["x", "y"],
            _ => Enumerable.Range(0, parameterCount).Select(static index => $"value{(index + 1).ToString(System.Globalization.CultureInfo.InvariantCulture)}").ToArray()
        };
    }

    private static string CreateSymbol(MethodInfo method, IReadOnlyList<Type> parameterTypes)
    {
        var fullName = method.DeclaringType?.FullName
            ?? throw new InvalidOperationException($"Method '{method.Name}' does not expose a declaring type.");
        var typePart = string.Join("_", fullName.Split('.').Select(static part => part.ToLowerInvariant()));
        var suffix = string.Join("_", parameterTypes.Select(ToSymbolTypeSuffix));
        return $"rustlyn_bindgen_{typePart}_{ToSnakeCase(method.Name)}_{suffix}";
    }

    private static string CreateInstanceSymbol(MethodInfo method, IReadOnlyList<Type> parameterTypes)
    {
        var fullName = method.DeclaringType?.FullName
            ?? throw new InvalidOperationException($"Method '{method.Name}' does not expose a declaring type.");
        var typePart = string.Join("_", fullName.Split('.').Select(static part => part.ToLowerInvariant()));
        var suffix = parameterTypes.Count > 0
            ? string.Join("_", parameterTypes.Select(ToSymbolTypeSuffix))
            : "void";
        return $"rustlyn_bindgen_{typePart}_inst_{ToSnakeCase(method.Name)}_{suffix}";
    }

    private static string CreateHelperMethodName(string symbol)
    {
        const string prefix = "rustlyn_";
        var name = symbol.StartsWith(prefix, StringComparison.Ordinal)
            ? symbol[prefix.Length..]
            : symbol;
        return string.Concat(name.Split('_').Select(static part => char.ToUpperInvariant(part[0]) + part[1..]));
    }

    private static ManagedGlueResult CreateScalarResult(Type returnType, ManagedGlueExpression valueExpression)
    {
        if (returnType == typeof(int))
        {
            return ManagedGlueResult.Int(valueExpression);
        }

        if (returnType == typeof(long))
        {
            return ManagedGlueResult.Long(valueExpression);
        }

        if (returnType == typeof(float))
        {
            return ManagedGlueResult.Float(valueExpression);
        }

        if (returnType == typeof(double))
        {
            return ManagedGlueResult.Double(valueExpression);
        }

        throw new NotSupportedException($"Scalar return type '{returnType}' is not supported.");
    }

    private static string ToManagedGlueTypeName(Type type)
    {
        if (type == typeof(int))
        {
            return "int";
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

        throw new NotSupportedException($"Managed glue scalar type '{type}' is not supported.");
    }

    private static string ToRustTypeName(Type type)
    {
        if (type == typeof(int))
        {
            return "i32";
        }

        if (type == typeof(long))
        {
            return "i64";
        }

        if (type == typeof(float))
        {
            return "f32";
        }

        if (type == typeof(double))
        {
            return "f64";
        }

        throw new NotSupportedException($"Rust scalar type '{type}' is not supported.");
    }

    private static string ToRustObjectHandleTypeName(Type type)
    {
        if (type == typeof(string))
        {
            return "ManagedString";
        }

        if (type == typeof(string[]))
        {
            return "ManagedStringArray";
        }

        if (type == typeof(int[]))
        {
            return "ManagedIntArray";
        }

        if (type == typeof(byte[]))
        {
            return "ManagedByteArray";
        }

        // Generic fallback for arbitrary managed types used as object handles
        return $"Managed{type.Name}";
    }

    private static string ToSymbolTypeSuffix(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(bool)) return "bool";
        if (type.IsArray)
        {
            var elementType = type.GetElementType()
                ?? throw new InvalidOperationException($"Array type '{type}' does not expose an element type.");
            return $"{ToSymbolTypeSuffix(elementType)}_array";
        }

        return ToRustTypeName(type);
    }

    private static string CreateProjectedRustMethodSignatureKey(MethodInfo method)
    {
        var parameterTypes = method.GetParameters().Select(static parameter => ToProjectedRustTypeKey(parameter.ParameterType));
        return $"{ToSnakeCase(method.Name)}({string.Join(", ", parameterTypes)})";
    }

    private static string ToProjectedRustTypeKey(Type type)
    {
        if (type == typeof(string)) return "ManagedString";
        if (type == typeof(int)) return "i32";
        if (type == typeof(long)) return "i64";
        if (type == typeof(float)) return "f32";
        if (type == typeof(double)) return "f64";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(IntPtr)) return "ptr";
        if (type.IsEnum && Enum.GetUnderlyingType(type) == typeof(int)) return "i32";
        if (type.IsSZArray)
        {
            var elementType = type.GetElementType()
                ?? throw new InvalidOperationException($"Array type '{type}' does not expose an element type.");
            return $"{ToProjectedRustTypeKey(elementType)}[]";
        }

        return FormatDiagnosticTypeName(type);
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

    /// <summary>
    /// Analyzes a delegate type to determine if it can be bridged as a callback from Rust.
    /// Returns the callback signature shape, or an unsupported reason if the delegate cannot be bridged.
    /// </summary>
    public static DelegateAnalysisResult AnalyzeDelegateType(Type delegateType)
    {
        ArgumentNullException.ThrowIfNull(delegateType);

        if (!typeof(Delegate).IsAssignableFrom(delegateType))
        {
            throw new ArgumentException($"Type '{delegateType}' is not a delegate type.", nameof(delegateType));
        }

        var invokeMethod = delegateType.GetMethod("Invoke")
            ?? throw new InvalidOperationException($"Delegate type '{delegateType}' does not expose an Invoke method.");

        var parameters = invokeMethod.GetParameters();
        var paramTypes = parameters.Select(p => p.ParameterType).ToList();
        var returnType = invokeMethod.ReturnType;

        // Check if all parameters and return type are bindable
        var unsupportedParams = new List<string>();
        foreach (var param in parameters)
        {
            if (!IsScalarBindingType(param.ParameterType) && !IsObjectHandleBindingType(param.ParameterType))
            {
                if (param.ParameterType != typeof(void))
                {
                    unsupportedParams.Add($"parameter '{param.Name}' has unsupported type '{FormatDiagnosticTypeName(param.ParameterType)}'");
                }
            }
        }

        if (returnType != typeof(void) && !IsScalarBindingType(returnType) && !IsObjectHandleBindingType(returnType))
        {
            unsupportedParams.Add($"return type '{FormatDiagnosticTypeName(returnType)}' is unsupported");
        }

        if (unsupportedParams.Count > 0)
        {
            return new DelegateAnalysisResult(
                delegateType,
                IsBindable: false,
                UnsupportedReason: string.Join("; ", unsupportedParams),
                RustCallbackSignature: null,
                ParameterTypes: paramTypes,
                ReturnType: returnType);
        }

        // Build the Rust callback signature: fn(param_types) -> return_type
        var rustParams = paramTypes.Select(t => IsScalarBindingType(t) ? ToRustTypeName(t) : "i32");
        var rustReturn = returnType == typeof(void)
            ? "()"
            : IsScalarBindingType(returnType) ? ToRustTypeName(returnType) : "i32";
        var rustSignature = $"fn({string.Join(", ", rustParams)}) -> {rustReturn}";

        return new DelegateAnalysisResult(
            delegateType,
            IsBindable: true,
            UnsupportedReason: null,
            RustCallbackSignature: rustSignature,
            ParameterTypes: paramTypes,
            ReturnType: returnType);
    }

    /// <summary>
    /// Analyzes an event for binding compatibility. Returns info about the event's
    /// delegate type and whether add/remove handlers can be generated.
    /// </summary>
    public static EventAnalysisResult AnalyzeEvent(EventInfo eventInfo)
    {
        ArgumentNullException.ThrowIfNull(eventInfo);

        var handlerType = eventInfo.EventHandlerType
            ?? throw new InvalidOperationException($"Event '{eventInfo.Name}' does not expose a handler type.");

        var delegateResult = AnalyzeDelegateType(handlerType);
        var addMethod = eventInfo.GetAddMethod(nonPublic: false);
        var removeMethod = eventInfo.GetRemoveMethod(nonPublic: false);

        return new EventAnalysisResult(
            eventInfo,
            delegateResult,
            HasPublicAdd: addMethod is not null,
            HasPublicRemove: removeMethod is not null);
    }
}

public sealed record StaticScalarMethodBindingRequest(
    Type DeclaringType,
    string MethodName,
    IReadOnlyList<Type> ParameterTypes,
    RustWrapperContainer Container);

public sealed record StaticObjectHandleMethodBindingRequest(
    Type DeclaringType,
    string MethodName,
    IReadOnlyList<Type> ParameterTypes,
    RustWrapperContainer Container);

public sealed record ScannedBinding(
    ManagedApiRequirement Requirement,
    ManagedGlueBinding ManagedGlueBinding,
    RustWrapperMethod RustWrapperMethod);

public sealed record BindingSurfaceScanResult(
    IReadOnlyList<ManagedApiRequirement> Requirements,
    IReadOnlyList<BindingScanUnsupportedShape> UnsupportedShapes);

public sealed record BindingScanUnsupportedShape(
    string DisplayName,
    ManagedApiRequirementKind Kind,
    string? MemberName,
    string Reason);

public sealed record AssemblyScanResult(
    string AssemblyName,
    IReadOnlyList<Type> ScannedTypes,
    IReadOnlyList<ManagedApiRequirement> Requirements,
    IReadOnlyList<BindingScanUnsupportedShape> UnsupportedShapes);

public sealed record InstanceScalarMethodBindingRequest(
    Type DeclaringType,
    string MethodName,
    IReadOnlyList<Type> ParameterTypes,
    RustWrapperContainer Container);

public sealed record InstanceObjectHandleMethodBindingRequest(
    Type DeclaringType,
    string MethodName,
    IReadOnlyList<Type> ParameterTypes,
    RustWrapperContainer Container);

public sealed record ConstructorBindingRequest(
    Type DeclaringType,
    IReadOnlyList<Type> ParameterTypes,
    RustWrapperContainer Container);

public sealed record DelegateAnalysisResult(
    Type DelegateType,
    bool IsBindable,
    string? UnsupportedReason,
    string? RustCallbackSignature,
    IReadOnlyList<Type> ParameterTypes,
    Type ReturnType);

public sealed record EventAnalysisResult(
    EventInfo Event,
    DelegateAnalysisResult DelegateAnalysis,
    bool HasPublicAdd,
    bool HasPublicRemove);
