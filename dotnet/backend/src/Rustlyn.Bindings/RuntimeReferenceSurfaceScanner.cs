using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Rustlyn.Bindings;

public static class RuntimeReferenceAssemblyDiscovery
{
    public static readonly IReadOnlyList<string> DefaultTargetFrameworks = ["net8.0", "net9.0", "net10.0"];

    public static RuntimeReferencePackDiscoveryResult Discover(
        IEnumerable<string>? targetFrameworks = null,
        string? dotnetRoot = null)
    {
        var requestedFrameworks = NormalizeTargetFrameworks(targetFrameworks);
        var roots = DiscoverDotNetRoots(dotnetRoot);
        var packs = new List<RuntimeReferencePack>();
        var missingTargets = new List<RuntimeReferencePackMissingTarget>();

        foreach (var targetFramework in requestedFrameworks)
        {
            var candidates = new List<RuntimeReferencePack>();
            foreach (var root in roots)
            {
                var packRoot = Path.Combine(root, "packs", "Microsoft.NETCore.App.Ref");
                if (!Directory.Exists(packRoot))
                {
                    continue;
                }

                foreach (var versionDirectory in Directory.EnumerateDirectories(packRoot).OrderBy(static path => path, StringComparer.Ordinal))
                {
                    var version = Path.GetFileName(versionDirectory);
                    var refDirectory = Path.Combine(versionDirectory, "ref", targetFramework);
                    if (!Directory.Exists(refDirectory))
                    {
                        continue;
                    }

                    var assemblies = Directory.EnumerateFiles(refDirectory, "*.dll", SearchOption.TopDirectoryOnly)
                        .OrderBy(static path => Path.GetFileName(path), StringComparer.Ordinal)
                        .ToArray();
                    candidates.Add(new RuntimeReferencePack(
                        targetFramework,
                        version,
                        versionDirectory,
                        refDirectory,
                        assemblies));
                }
            }

            var selected = candidates
                .OrderBy(static candidate => ParsedPackageVersion.Parse(candidate.PackVersion), ParsedPackageVersion.Comparer)
                .ThenBy(static candidate => candidate.PackRoot, StringComparer.Ordinal)
                .LastOrDefault();
            if (selected is null)
            {
                missingTargets.Add(new RuntimeReferencePackMissingTarget(
                    targetFramework,
                    roots.Count == 0 ? string.Empty : string.Join(Path.PathSeparator, roots)));
                continue;
            }

            packs.Add(selected);
        }

        return new RuntimeReferencePackDiscoveryResult(packs, missingTargets);
    }

    private static IReadOnlyList<string> NormalizeTargetFrameworks(IEnumerable<string>? targetFrameworks)
    {
        var frameworks = (targetFrameworks ?? DefaultTargetFrameworks)
            .Select(static framework => framework.Trim())
            .Where(static framework => framework.Length != 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (frameworks.Length == 0)
        {
            throw new ArgumentException("At least one target framework must be supplied.", nameof(targetFrameworks));
        }

        return frameworks;
    }

    private static IReadOnlyList<string> DiscoverDotNetRoots(string? explicitRoot)
    {
        var roots = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (!string.IsNullOrWhiteSpace(explicitRoot))
        {
            var fullPath = Path.GetFullPath(explicitRoot);
            if (!Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException($"The specified .NET root does not exist: {fullPath}");
            }

            roots.Add(fullPath);
            return roots.OrderBy(static root => root, StringComparer.Ordinal).ToArray();
        }

        AddIfDirectory(roots, Environment.GetEnvironmentVariable("DOTNET_ROOT"));
        AddIfDirectory(roots, Environment.GetEnvironmentVariable("DOTNET_ROOT_X64"));
        AddIfDirectory(roots, Environment.GetEnvironmentVariable("DOTNET_ROOT_X86"));

        var processPath = Environment.ProcessPath;
        if (!string.IsNullOrWhiteSpace(processPath))
        {
            AddIfDirectory(roots, Path.GetDirectoryName(processPath));
        }

        AddIfDirectory(roots, Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "dotnet"));
        AddIfDirectory(roots, Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "dotnet"));

        return roots.OrderBy(static root => root, StringComparer.Ordinal).ToArray();
    }

    private static void AddIfDirectory(ISet<string> roots, string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        var fullPath = Path.GetFullPath(path);
        if (Directory.Exists(fullPath))
        {
            roots.Add(fullPath);
        }
    }

    private sealed record ParsedPackageVersion(int Major, int Minor, int Patch, int Revision, string? Prerelease, string Original)
    {
        public static IComparer<ParsedPackageVersion> Comparer { get; } = new PackageVersionComparer();

        public static ParsedPackageVersion Parse(string value)
        {
            var parts = value.Split('-', 2, StringSplitOptions.TrimEntries);
            var numbers = parts[0]
                .Split('.', StringSplitOptions.TrimEntries)
                .Select(static part => int.TryParse(part, NumberStyles.None, CultureInfo.InvariantCulture, out var number) ? number : 0)
                .Concat(Enumerable.Repeat(0, 4))
                .Take(4)
                .ToArray();

            return new ParsedPackageVersion(
                numbers[0],
                numbers[1],
                numbers[2],
                numbers[3],
                parts.Length == 2 ? parts[1] : null,
                value);
        }

        private sealed class PackageVersionComparer : IComparer<ParsedPackageVersion>
        {
            public int Compare(ParsedPackageVersion? x, ParsedPackageVersion? y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (x is null) return -1;
                if (y is null) return 1;

                var numeric = x.Major.CompareTo(y.Major);
                if (numeric != 0) return numeric;
                numeric = x.Minor.CompareTo(y.Minor);
                if (numeric != 0) return numeric;
                numeric = x.Patch.CompareTo(y.Patch);
                if (numeric != 0) return numeric;
                numeric = x.Revision.CompareTo(y.Revision);
                if (numeric != 0) return numeric;

                if (x.Prerelease is null && y.Prerelease is not null) return 1;
                if (x.Prerelease is not null && y.Prerelease is null) return -1;
                return string.Compare(x.Original, y.Original, StringComparison.Ordinal);
            }
        }
    }
}

public static class RuntimeReferenceSurfaceScanner
{
    public static RuntimeSurfaceScanSet ScanInstalledPacks(
        IEnumerable<string>? targetFrameworks = null,
        string? dotnetRoot = null,
        string? namespaceFilter = null)
    {
        var discovery = RuntimeReferenceAssemblyDiscovery.Discover(targetFrameworks, dotnetRoot);
        var reports = discovery.Packs
            .Select(pack => ScanPack(pack, namespaceFilter))
            .ToArray();
        return new RuntimeSurfaceScanSet(reports, discovery.MissingTargets);
    }

    public static RuntimeSurfaceScanReport ScanPack(RuntimeReferencePack pack, string? namespaceFilter = null)
    {
        ArgumentNullException.ThrowIfNull(pack);

        var assemblyReports = new List<RuntimeAssemblyScanReport>();
        var unsupportedShapes = new List<BindingScanUnsupportedShape>();
        var skippedTypesByReason = new SortedDictionary<string, int>(StringComparer.Ordinal);

        using var loadContext = CreateMetadataLoadContext(pack);
        foreach (var assemblyPath in pack.AssemblyPaths.OrderBy(static path => Path.GetFileName(path), StringComparer.Ordinal))
        {
            assemblyReports.Add(ScanAssembly(loadContext, assemblyPath, namespaceFilter, unsupportedShapes, skippedTypesByReason));
        }

        var unsupportedByReason = unsupportedShapes
            .GroupBy(static shape => CategorizeUnsupportedReason(shape.Reason), StringComparer.Ordinal)
            .Select(static group => new RuntimeUnsupportedReasonCount(group.Key, group.Count()))
            .OrderByDescending(static count => count.Count)
            .ThenBy(static count => count.Reason, StringComparer.Ordinal)
            .ToArray();

        var assemblies = assemblyReports
            .OrderBy(static assembly => assembly.AssemblyName, StringComparer.Ordinal)
            .ToArray();
        var runtimeTypes = assemblies
            .SelectMany(static assembly => assembly.Types)
            .OrderBy(static type => type.FullName, StringComparer.Ordinal)
            .ToArray();
        return new RuntimeSurfaceScanReport(
            pack.TargetFramework,
            pack.PackVersion,
            pack.PackRoot,
            pack.RefDirectory,
            assemblies,
            assemblyReports.Sum(static assembly => assembly.AssemblyCount),
            assemblyReports.Sum(static assembly => assembly.ExportedTypeCount),
            assemblyReports.Sum(static assembly => assembly.ScannedTypeCount),
            assemblyReports.Sum(static assembly => assembly.SkippedTypeCount),
            assemblyReports.Sum(static assembly => assembly.PublicMethodCount),
            assemblyReports.Sum(static assembly => assembly.PublicPropertyCount),
            assemblyReports.Sum(static assembly => assembly.PublicEventCount),
            assemblyReports.Sum(static assembly => assembly.PublicConstructorCount),
            assemblyReports.Sum(static assembly => assembly.ProjectedRequirementCount),
            assemblyReports.Sum(static assembly => assembly.ProjectedMemberCount),
            unsupportedShapes.Count,
            new ReadOnlyDictionary<string, int>(skippedTypesByReason),
            unsupportedByReason,
            unsupportedShapes
                .OrderBy(static shape => shape.DisplayName, StringComparer.Ordinal)
                .ThenBy(static shape => shape.Reason, StringComparer.Ordinal)
                .ToArray(),
            runtimeTypes);
    }

    private static RuntimeAssemblyScanReport ScanAssembly(
        MetadataLoadContext loadContext,
        string assemblyPath,
        string? namespaceFilter,
        ICollection<BindingScanUnsupportedShape> unsupportedShapes,
        IDictionary<string, int> skippedTypesByReason)
    {
        try
        {
            var assembly = loadContext.LoadFromAssemblyPath(Path.GetFullPath(assemblyPath));
            var exportedTypes = GetExportedTypes(assembly, assemblyPath, unsupportedShapes)
                .Where(type => MatchesNamespaceFilter(type, namespaceFilter))
                .OrderBy(static type => type.FullName, StringComparer.Ordinal)
                .ToArray();

            var scannedTypeCount = 0;
            var skippedTypeCount = 0;
            var projectedRequirementCount = 0;
            var projectedMemberCount = 0;
            var unsupportedBefore = unsupportedShapes.Count;

            var publicMethodCount = 0;
            var publicPropertyCount = 0;
            var publicEventCount = 0;
            var publicConstructorCount = 0;
            var runtimeTypes = new List<BindingManifestRuntimeType>();
            var assemblyIdentity = BindingManifestAssemblyIdentity.From(assembly.GetName());
            var assemblyName = assembly.GetName().Name ?? Path.GetFileNameWithoutExtension(assemblyPath);

            foreach (var type in exportedTypes)
            {
                publicMethodCount += CountPublicDeclaredMethods(type);
                publicPropertyCount += CountPublicDeclaredProperties(type);
                publicEventCount += CountPublicDeclaredEvents(type);
                publicConstructorCount += CountPublicConstructors(type);

                var skippedReason = GetSkippedTypeReason(type);
                if (skippedReason is not null)
                {
                    skippedTypeCount++;
                    Increment(skippedTypesByReason, skippedReason);
                    runtimeTypes.Add(CreateRuntimeType(type, assemblyName, "deferred", skippedReason, []));
                    continue;
                }

                try
                {
                    var result = BindingSurfaceScanner.ScanTypeWithDiagnostics(type);
                    scannedTypeCount++;
                    projectedRequirementCount += result.Requirements.Count;
                    projectedMemberCount += result.Requirements.Count(static requirement => requirement.Kind != ManagedApiRequirementKind.Type);
                    runtimeTypes.Add(CreateRuntimeType(type, assemblyName, "projected", null, CreateRuntimeMembers(type, result)));
                    foreach (var shape in result.UnsupportedShapes)
                    {
                        unsupportedShapes.Add(shape);
                    }
                }
                catch (Exception ex) when (ex is TypeLoadException or FileLoadException or FileNotFoundException or NotSupportedException or InvalidOperationException)
                {
                    var reason = $"type scan failed: {ex.GetType().Name}: {ex.Message}";
                    runtimeTypes.Add(CreateRuntimeType(type, assemblyName, "rejected", reason, []));
                    unsupportedShapes.Add(new BindingScanUnsupportedShape(
                        type.FullName ?? type.Name,
                        ManagedApiRequirementKind.Type,
                        MemberName: null,
                        reason));
                }
            }

            return new RuntimeAssemblyScanReport(
                assemblyName,
                assemblyIdentity,
                assemblyPath,
                AssemblyCount: 1,
                exportedTypes.Length,
                scannedTypeCount,
                skippedTypeCount,
                publicMethodCount,
                publicPropertyCount,
                publicEventCount,
                publicConstructorCount,
                projectedRequirementCount,
                projectedMemberCount,
                unsupportedShapes.Count - unsupportedBefore,
                LoadDiagnostic: null,
                runtimeTypes
                    .OrderBy(static type => type.FullName, StringComparer.Ordinal)
                    .ToArray());
        }
        catch (Exception ex) when (ex is BadImageFormatException or FileLoadException or FileNotFoundException or TypeLoadException)
        {
            unsupportedShapes.Add(new BindingScanUnsupportedShape(
                Path.GetFileName(assemblyPath),
                ManagedApiRequirementKind.Type,
                MemberName: null,
                $"assembly load failed: {ex.GetType().Name}: {ex.Message}"));

            return new RuntimeAssemblyScanReport(
                Path.GetFileNameWithoutExtension(assemblyPath),
                new BindingManifestAssemblyIdentity(Path.GetFileNameWithoutExtension(assemblyPath), Version: null, CultureName: null, PublicKeyToken: string.Empty),
                assemblyPath,
                AssemblyCount: 1,
                ExportedTypeCount: 0,
                ScannedTypeCount: 0,
                SkippedTypeCount: 0,
                PublicMethodCount: 0,
                PublicPropertyCount: 0,
                PublicEventCount: 0,
                PublicConstructorCount: 0,
                ProjectedRequirementCount: 0,
                ProjectedMemberCount: 0,
                UnsupportedShapeCount: 1,
                LoadDiagnostic: $"{ex.GetType().Name}: {ex.Message}",
                Types: []);
        }
    }

    private static Type[] GetExportedTypes(Assembly assembly, string assemblyPath, ICollection<BindingScanUnsupportedShape> unsupportedShapes)
    {
        try
        {
            return assembly.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            foreach (var loaderException in ex.LoaderExceptions.Where(static exception => exception is not null))
            {
                unsupportedShapes.Add(new BindingScanUnsupportedShape(
                    Path.GetFileName(assemblyPath),
                    ManagedApiRequirementKind.Type,
                    MemberName: null,
                    $"exported type load failed: {loaderException!.GetType().Name}: {loaderException.Message}"));
            }

            return ex.Types.Where(static type => type is not null).Cast<Type>().ToArray();
        }
    }

    private static BindingManifestRuntimeType CreateRuntimeType(
        Type type,
        string assemblyName,
        string projectionStatus,
        string? unsupportedReason,
        IReadOnlyList<BindingManifestRuntimeMember> members)
    {
        var fullName = BindingManifestFormatting.FormatTypeName(type);
        return new BindingManifestRuntimeType(
            new BindingManifestRuntimeTypeIdentity(
                assemblyName,
                fullName,
                GetMetadataToken(type),
                GetGenericArity(type)),
            type.Namespace ?? string.Empty,
            type.Name,
            fullName,
            GetRuntimeTypeKind(type),
            GetGenericArity(type),
            type.IsNested,
            projectionStatus,
            "current-binding-policy-v1",
            unsupportedReason,
            GetAttributeTypeNames(type),
            members);
    }

    private static IReadOnlyList<BindingManifestRuntimeMember> CreateRuntimeMembers(Type type, BindingSurfaceScanResult scanResult)
    {
        var projected = scanResult.Requirements
            .Where(static requirement => requirement.Kind != ManagedApiRequirementKind.Type)
            .GroupBy(static requirement => CreateRequirementKey(requirement.Kind, requirement.DisplayName), StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal);
        var unsupported = scanResult.UnsupportedShapes
            .GroupBy(static shape => CreateRequirementKey(shape.Kind, shape.DisplayName), StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal);
        var members = new List<BindingManifestRuntimeMember>();

        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(static method => !method.IsSpecialName)
            .OrderBy(static method => method.Name, StringComparer.Ordinal)
            .ThenBy(static method => FormatParameterList(method.GetParameters().Select(static parameter => parameter.ParameterType)), StringComparer.Ordinal))
        {
            var displayName = FormatMethodDisplayName(type, method.Name, method.GetParameters().Select(static parameter => parameter.ParameterType));
            members.Add(CreateRuntimeMember(
                type,
                ManagedApiRequirementKind.Method,
                method.Name,
                displayName,
                method,
                method.ReturnType,
                method.GetParameters(),
                method.IsStatic,
                GetGenericArity(method),
                projected,
                unsupported));
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(static property => property.Name, StringComparer.Ordinal))
        {
            var displayName = $"{BindingManifestFormatting.FormatTypeName(type)}.{property.Name}";
            members.Add(CreateRuntimeMember(
                type,
                ManagedApiRequirementKind.Property,
                property.Name,
                displayName,
                property,
                property.PropertyType,
                property.GetIndexParameters(),
                IsPropertyStatic(property),
                genericArity: 0,
                projected,
                unsupported));
        }

        foreach (var eventInfo in type.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(static eventInfo => eventInfo.Name, StringComparer.Ordinal))
        {
            var displayName = $"{BindingManifestFormatting.FormatTypeName(type)}.{eventInfo.Name}";
            members.Add(CreateRuntimeMember(
                type,
                ManagedApiRequirementKind.Event,
                eventInfo.Name,
                displayName,
                eventInfo,
                eventInfo.EventHandlerType,
                parameters: [],
                IsEventStatic(eventInfo),
                genericArity: 0,
                projected,
                unsupported));
        }

        foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(static constructor => constructor.GetParameters().Length)
            .ThenBy(static constructor => FormatParameterList(constructor.GetParameters().Select(static parameter => parameter.ParameterType)), StringComparer.Ordinal))
        {
            members.Add(CreateConstructorRuntimeMember(type, constructor));
        }

        return members
            .OrderBy(static member => member.DisplayName, StringComparer.Ordinal)
            .ThenBy(static member => member.Signature, StringComparer.Ordinal)
            .ToArray();
    }

    private static BindingManifestRuntimeMember CreateRuntimeMember(
        Type declaringType,
        ManagedApiRequirementKind kind,
        string name,
        string displayName,
        MemberInfo member,
        Type? returnType,
        IReadOnlyList<ParameterInfo> parameters,
        bool isStatic,
        int genericArity,
        IReadOnlyDictionary<string, ManagedApiRequirement> projected,
        IReadOnlyDictionary<string, BindingScanUnsupportedShape> unsupported)
    {
        var key = CreateRequirementKey(kind, displayName);
        var projectionStatus = projected.ContainsKey(key) ? "projected" : "deferred";
        string? unsupportedReason = null;
        string? unsupportedReasonCode = null;
        if (unsupported.TryGetValue(key, out var unsupportedShape))
        {
            projectionStatus = "rejected";
            unsupportedReason = unsupportedShape.Reason;
            unsupportedReasonCode = CategorizeUnsupportedReason(unsupportedShape.Reason);
        }

        if (returnType is not null
            && TaskFutureProjectionPolicy.AnalyzeReturnType(returnType) is { IsTaskLike: true } taskAnalysis)
        {
            projectionStatus = "deferred";
            unsupportedReason = TaskFutureProjectionPolicy.CreateDeferredReason(taskAnalysis);
            unsupportedReasonCode = "task-future";
        }

        if (string.Equals(projectionStatus, "deferred", StringComparison.Ordinal)
            && unsupportedReason is null)
        {
            unsupportedReason = "projection policy has not classified this member yet";
            unsupportedReasonCode = "unclassified-member";
        }

        return new BindingManifestRuntimeMember(
            new BindingManifestRuntimeMemberIdentity(
                BindingManifestFormatting.FormatTypeName(declaringType),
                name,
                kind.ToString(),
                GetMetadataToken(member),
                CreateSignatureKey(kind, name, parameters.Select(static parameter => parameter.ParameterType))),
            kind.ToString(),
            name,
            displayName,
            CreateSignature(name, parameters.Select(static parameter => parameter.ParameterType), returnType),
            projectionStatus,
            "current-binding-policy-v1",
            unsupportedReason,
            unsupportedReasonCode,
            isStatic,
            genericArity,
            returnType is null ? null : BindingManifestFormatting.FormatTypeName(returnType),
            "unknown",
            parameters.Select(CreateRuntimeParameter).ToArray(),
            GetAttributeTypeNames(member));
    }

    private static BindingManifestRuntimeMember CreateConstructorRuntimeMember(Type declaringType, ConstructorInfo constructor)
    {
        var parameters = constructor.GetParameters();
        var unsupportedReason = GetUnsupportedConstructorReason(constructor, parameters);
        var projectionStatus = unsupportedReason is null ? "projected" : "rejected";
        var displayName = $"{BindingManifestFormatting.FormatTypeName(declaringType)}..ctor({FormatProjectedParameterList(parameters.Select(static parameter => parameter.ParameterType))})";
        return new BindingManifestRuntimeMember(
            new BindingManifestRuntimeMemberIdentity(
                BindingManifestFormatting.FormatTypeName(declaringType),
                ".ctor",
                ManagedApiRequirementKind.Constructor.ToString(),
                GetMetadataToken(constructor),
                CreateSignatureKey(ManagedApiRequirementKind.Constructor, ".ctor", parameters.Select(static parameter => parameter.ParameterType))),
            ManagedApiRequirementKind.Constructor.ToString(),
            ".ctor",
            displayName,
            CreateSignature(".ctor", parameters.Select(static parameter => parameter.ParameterType), ReturnType: null),
            projectionStatus,
            "current-binding-policy-v1",
            unsupportedReason,
            unsupportedReason is null ? null : CategorizeUnsupportedReason(unsupportedReason),
            IsStatic: false,
            GenericArity: 0,
            ReturnType: null,
            ReturnNullability: "unknown",
            parameters.Select(CreateRuntimeParameter).ToArray(),
            GetAttributeTypeNames(constructor));
    }

    private static BindingManifestRuntimeParameter CreateRuntimeParameter(ParameterInfo parameter)
    {
        return new BindingManifestRuntimeParameter(
            parameter.Name ?? $"arg{parameter.Position.ToString(CultureInfo.InvariantCulture)}",
            parameter.Position,
            BindingManifestFormatting.FormatTypeName(parameter.ParameterType),
            "unknown",
            parameter.IsOut,
            parameter.IsOptional,
            GetAttributeTypeNames(parameter));
    }

    private static string? GetUnsupportedConstructorReason(ConstructorInfo constructor, ParameterInfo[] parameters)
    {
        if (constructor.ContainsGenericParameters)
        {
            return "generic or open constructors are not supported";
        }

        foreach (var parameter in parameters)
        {
            if (parameter.ParameterType.IsByRef)
            {
                var direction = parameter.IsOut ? "out" : "ref";
                return $"{direction} parameter '{parameter.Name}' is not supported";
            }

            if (!BindingSurfaceScanner.IsTypeBindable(parameter.ParameterType))
            {
                return $"parameter '{parameter.Name}' has unsupported type '{BindingManifestFormatting.FormatTypeName(parameter.ParameterType)}'";
            }
        }

        return null;
    }

    private static string CreateRequirementKey(ManagedApiRequirementKind kind, string displayName)
        => $"{kind}:{displayName}";

    private static string CreateSignatureKey(ManagedApiRequirementKind kind, string name, IEnumerable<Type> parameterTypes)
        => $"{kind}:{name}({FormatParameterList(parameterTypes)})";

    private static string CreateSignature(string name, IEnumerable<Type> parameterTypes, Type? ReturnType)
    {
        var signature = $"{name}({FormatParameterList(parameterTypes)})";
        return ReturnType is null
            ? signature
            : $"{signature} -> {BindingManifestFormatting.FormatTypeName(ReturnType)}";
    }

    private static string FormatMethodDisplayName(Type type, string methodName, IEnumerable<Type> parameterTypes)
        => $"{BindingManifestFormatting.FormatTypeName(type)}.{methodName}({FormatProjectedParameterList(parameterTypes)})";

    private static string FormatParameterList(IEnumerable<Type> parameterTypes)
        => string.Join(", ", parameterTypes.Select(BindingManifestFormatting.FormatTypeName));

    private static string FormatProjectedParameterList(IEnumerable<Type> parameterTypes)
        => string.Join(", ", parameterTypes.Select(FormatProjectedTypeName));

    private static string FormatProjectedTypeName(Type type)
    {
        return type.FullName switch
        {
            "System.String" => "string",
            "System.Int32" => "int",
            "System.Int64" => "long",
            "System.Single" => "float",
            "System.Double" => "double",
            "System.Boolean" => "bool",
            "System.Void" => "void",
            "System.IntPtr" => "IntPtr",
            _ when type.IsArray && type.GetElementType() is { } elementType => $"{FormatProjectedTypeName(elementType)}[]",
            _ => type.Name
        };
    }

    private static int GetMetadataToken(MemberInfo member)
    {
        try
        {
            return member.MetadataToken;
        }
        catch (InvalidOperationException)
        {
            return 0;
        }
    }

    private static int GetGenericArity(Type type)
        => type.IsGenericType ? type.GetGenericArguments().Length : 0;

    private static int GetGenericArity(MethodInfo method)
        => method.IsGenericMethod ? method.GetGenericArguments().Length : 0;

    private static string GetRuntimeTypeKind(Type type)
    {
        if (type.IsInterface) return "interface";
        if (type.IsEnum) return "enum";
        if (IsDelegateType(type)) return "delegate";
        if (type.IsValueType) return "struct";
        if (type.IsAbstract && type.IsSealed) return "static-class";
        if (type.IsAbstract) return "abstract-class";
        return "class";
    }

    private static bool IsDelegateType(Type type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (string.Equals(current.FullName, "System.Delegate", StringComparison.Ordinal)
                || string.Equals(current.FullName, "System.MulticastDelegate", StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsPropertyStatic(PropertyInfo property)
    {
        var accessor = property.GetMethod ?? property.SetMethod;
        return accessor?.IsStatic == true;
    }

    private static bool IsEventStatic(EventInfo eventInfo)
    {
        var accessor = eventInfo.AddMethod ?? eventInfo.RemoveMethod;
        return accessor?.IsStatic == true;
    }

    private static IReadOnlyList<string> GetAttributeTypeNames(MemberInfo member)
    {
        try
        {
            return member.GetCustomAttributesData()
                .Select(static attribute => BindingManifestFormatting.FormatTypeName(attribute.AttributeType))
                .OrderBy(static name => name, StringComparer.Ordinal)
                .ToArray();
        }
        catch (Exception ex) when (ex is TypeLoadException or FileLoadException or FileNotFoundException or InvalidOperationException)
        {
            return [];
        }
    }

    private static IReadOnlyList<string> GetAttributeTypeNames(ParameterInfo parameter)
    {
        try
        {
            return parameter.GetCustomAttributesData()
                .Select(static attribute => BindingManifestFormatting.FormatTypeName(attribute.AttributeType))
                .OrderBy(static name => name, StringComparer.Ordinal)
                .ToArray();
        }
        catch (Exception ex) when (ex is TypeLoadException or FileLoadException or FileNotFoundException or InvalidOperationException)
        {
            return [];
        }
    }

    private static bool MatchesNamespaceFilter(Type type, string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return true;
        }

        var ns = type.Namespace ?? string.Empty;
        if (filter.EndsWith(".*", StringComparison.Ordinal))
        {
            return ns.StartsWith(filter[..^2], StringComparison.Ordinal);
        }

        return string.Equals(ns, filter, StringComparison.Ordinal);
    }

    private static string? GetSkippedTypeReason(Type type)
    {
        if (type.IsGenericTypeDefinition) return "generic type definition";
        if (type.IsInterface) return "interface";
        if (type.IsAbstract && !type.IsSealed) return "abstract type";
        return null;
    }

    private static int CountPublicDeclaredMethods(Type type)
        => TryCount(() => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Count(static method => !method.IsSpecialName));

    private static int CountPublicDeclaredProperties(Type type)
        => TryCount(() => type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length);

    private static int CountPublicDeclaredEvents(Type type)
        => TryCount(() => type.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length);

    private static int CountPublicConstructors(Type type)
        => TryCount(() => type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length);

    private static int TryCount(Func<int> count)
    {
        try
        {
            return count();
        }
        catch (Exception ex) when (ex is TypeLoadException or FileLoadException or FileNotFoundException)
        {
            return 0;
        }
    }

    private static void Increment(IDictionary<string, int> counts, string key)
        => counts[key] = counts.TryGetValue(key, out var count) ? count + 1 : 1;

    private static string CategorizeUnsupportedReason(string reason)
    {
        if (reason.StartsWith("events are not supported", StringComparison.Ordinal)) return "event";
        if (reason.StartsWith("generic or open methods", StringComparison.Ordinal)) return "generic-method";
        if (reason.StartsWith("return type", StringComparison.Ordinal)) return "unsupported-return-type";
        if (reason.StartsWith("parameter", StringComparison.Ordinal)) return "unsupported-parameter-type";
        if (reason.StartsWith("ref parameter", StringComparison.Ordinal)) return "ref-parameter";
        if (reason.StartsWith("out parameter", StringComparison.Ordinal)) return "out-parameter";
        if (reason.StartsWith("property type", StringComparison.Ordinal)) return "unsupported-property-type";
        if (reason.StartsWith("indexed properties", StringComparison.Ordinal)) return "indexed-property";
        if (reason.StartsWith("overload projects", StringComparison.Ordinal)) return "overload-collision";
        if (reason.StartsWith("task return type", StringComparison.Ordinal)) return "task-future";
        if (reason.StartsWith("projection policy has not classified", StringComparison.Ordinal)) return "unclassified-member";
        if (reason.StartsWith("type scan failed", StringComparison.Ordinal)) return "type-scan-failure";
        if (reason.StartsWith("assembly load failed", StringComparison.Ordinal)) return "assembly-load-failure";
        if (reason.StartsWith("exported type load failed", StringComparison.Ordinal)) return "exported-type-load-failure";
        return "other";
    }

    private static MetadataLoadContext CreateMetadataLoadContext(RuntimeReferencePack pack)
    {
        var pathsByFileName = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var assemblyPath in pack.AssemblyPaths)
        {
            pathsByFileName[Path.GetFileName(assemblyPath)] = assemblyPath;
        }

        var trustedPlatformAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;
        if (!string.IsNullOrWhiteSpace(trustedPlatformAssemblies))
        {
            foreach (var assemblyPath in trustedPlatformAssemblies.Split(Path.PathSeparator))
            {
                if (!string.IsNullOrWhiteSpace(assemblyPath) && File.Exists(assemblyPath))
                {
                    pathsByFileName.TryAdd(Path.GetFileName(assemblyPath), assemblyPath);
                }
            }
        }

        var resolver = new PathAssemblyResolver(pathsByFileName.Values);
        return new MetadataLoadContext(resolver, "System.Private.CoreLib");
    }
}

public static class RuntimeSurfaceReportFormatter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string GenerateJson(RuntimeSurfaceScanSet scanSet)
        => JsonSerializer.Serialize(scanSet, JsonOptions);

    public static string GenerateManifestJson(RuntimeSurfaceScanSet scanSet)
        => JsonSerializer.Serialize(scanSet.Reports.Select(BindingManifestDocument.FromRuntimeSurface).ToArray(), JsonOptions);

    public static string GenerateDiffJson(RuntimeSurfaceScanSet scanSet)
        => JsonSerializer.Serialize(RuntimeSurfaceDiffReporter.CreateDiffs(scanSet), JsonOptions);

    public static string GenerateDiffText(RuntimeSurfaceScanSet scanSet)
    {
        var diffSet = RuntimeSurfaceDiffReporter.CreateDiffs(scanSet);
        var builder = new StringBuilder();
        foreach (var diff in diffSet.Diffs)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(CultureInfo.InvariantCulture, $"Diff: {diff.FromTargetFramework} -> {diff.ToTargetFramework}");
            builder.AppendLine(CultureInfo.InvariantCulture, $"Added types: {diff.AddedTypes.Count}");
            AppendSampleList(builder, diff.AddedTypes);
            builder.AppendLine(CultureInfo.InvariantCulture, $"Removed types: {diff.RemovedTypes.Count}");
            AppendSampleList(builder, diff.RemovedTypes);
            builder.AppendLine(CultureInfo.InvariantCulture, $"Added members: {diff.AddedMembers.Count}");
            AppendSampleList(builder, diff.AddedMembers);
            builder.AppendLine(CultureInfo.InvariantCulture, $"Removed members: {diff.RemovedMembers.Count}");
            AppendSampleList(builder, diff.RemovedMembers);
            builder.AppendLine(CultureInfo.InvariantCulture, $"Projection status changes: {diff.StatusChanges.Count}");
            AppendSampleList(builder, diff.StatusChanges.Select(static change =>
                $"{change.DisplayName}: {change.FromStatus} -> {change.ToStatus}").ToArray());
        }

        return builder.ToString();
    }

    public static string GenerateText(RuntimeSurfaceScanSet scanSet)
    {
        var builder = new StringBuilder();
        foreach (var missing in scanSet.MissingTargets)
        {
            builder.Append("Missing reference pack: ");
            builder.Append(missing.TargetFramework);
            if (!string.IsNullOrEmpty(missing.SearchRoots))
            {
                builder.Append(" searched ");
                builder.Append(missing.SearchRoots);
            }

            builder.AppendLine();
        }

        foreach (var report in scanSet.Reports)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            AppendReport(builder, report);
        }

        return builder.ToString();
    }

    private static void AppendReport(StringBuilder builder, RuntimeSurfaceScanReport report)
    {
        builder.AppendLine(CultureInfo.InvariantCulture, $"Target framework: {report.TargetFramework}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Reference pack: Microsoft.NETCore.App.Ref {report.PackVersion}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Reference path: {report.RefDirectory}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Assemblies: {report.AssemblyCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Exported types: {report.ExportedTypeCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Scanned types: {report.ScannedTypeCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Skipped types: {report.SkippedTypeCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Public methods: {report.PublicMethodCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Public properties: {report.PublicPropertyCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Public events: {report.PublicEventCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Public constructors: {report.PublicConstructorCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Projected requirements: {report.ProjectedRequirementCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Projected members: {report.ProjectedMemberCount}");
        builder.AppendLine(CultureInfo.InvariantCulture, $"Unsupported shapes: {report.UnsupportedShapeCount}");

        if (report.SkippedTypesByReason.Count > 0)
        {
            builder.AppendLine("Skipped type reasons:");
            foreach (var reason in report.SkippedTypesByReason.OrderBy(static pair => pair.Key, StringComparer.Ordinal))
            {
                builder.AppendLine(CultureInfo.InvariantCulture, $"  {reason.Key}: {reason.Value}");
            }
        }

        if (report.UnsupportedShapesByReason.Count > 0)
        {
            builder.AppendLine("Unsupported shape reasons:");
            foreach (var reason in report.UnsupportedShapesByReason)
            {
                builder.AppendLine(CultureInfo.InvariantCulture, $"  {reason.Reason}: {reason.Count}");
            }
        }

        builder.AppendLine("Assemblies:");
        foreach (var assembly in report.Assemblies)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"  {assembly.AssemblyName}: types={assembly.ExportedTypeCount} scanned={assembly.ScannedTypeCount} projected={assembly.ProjectedMemberCount} unsupported={assembly.UnsupportedShapeCount}");
            if (assembly.LoadDiagnostic is not null)
            {
                builder.AppendLine(CultureInfo.InvariantCulture, $"    load: {assembly.LoadDiagnostic}");
            }
        }
    }

    private static void AppendSampleList(StringBuilder builder, IReadOnlyList<string> values)
    {
        const int limit = 50;
        foreach (var value in values.Take(limit))
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"  {value}");
        }

        if (values.Count > limit)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"  ... and {values.Count - limit} more");
        }
    }
}

public static class RuntimeSurfaceDiffReporter
{
    public static RuntimeSurfaceDiffSet CreateDiffs(RuntimeSurfaceScanSet scanSet)
    {
        ArgumentNullException.ThrowIfNull(scanSet);

        var diffs = new List<RuntimeSurfaceDiffReport>();
        for (var index = 0; index < scanSet.Reports.Count - 1; index++)
        {
            diffs.Add(CreateDiff(scanSet.Reports[index], scanSet.Reports[index + 1]));
        }

        return new RuntimeSurfaceDiffSet(diffs);
    }

    private static RuntimeSurfaceDiffReport CreateDiff(RuntimeSurfaceScanReport from, RuntimeSurfaceScanReport to)
    {
        var fromTypes = from.Types.ToDictionary(CreateTypeKey, static type => type, StringComparer.Ordinal);
        var toTypes = to.Types.ToDictionary(CreateTypeKey, static type => type, StringComparer.Ordinal);
        var fromMembers = FlattenMembers(from.Types);
        var toMembers = FlattenMembers(to.Types);

        var addedTypes = toTypes.Keys.Except(fromTypes.Keys, StringComparer.Ordinal)
            .Select(key => toTypes[key].FullName)
            .OrderBy(static name => name, StringComparer.Ordinal)
            .ToArray();
        var removedTypes = fromTypes.Keys.Except(toTypes.Keys, StringComparer.Ordinal)
            .Select(key => fromTypes[key].FullName)
            .OrderBy(static name => name, StringComparer.Ordinal)
            .ToArray();
        var addedMembers = toMembers.Keys.Except(fromMembers.Keys, StringComparer.Ordinal)
            .Select(key => toMembers[key].DisplayName)
            .OrderBy(static name => name, StringComparer.Ordinal)
            .ToArray();
        var removedMembers = fromMembers.Keys.Except(toMembers.Keys, StringComparer.Ordinal)
            .Select(key => fromMembers[key].DisplayName)
            .OrderBy(static name => name, StringComparer.Ordinal)
            .ToArray();
        var statusChanges = fromMembers.Keys.Intersect(toMembers.Keys, StringComparer.Ordinal)
            .Select(key => new { From = fromMembers[key], To = toMembers[key] })
            .Where(static pair =>
                !string.Equals(pair.From.ProjectionStatus, pair.To.ProjectionStatus, StringComparison.Ordinal) ||
                !string.Equals(pair.From.UnsupportedReasonCode, pair.To.UnsupportedReasonCode, StringComparison.Ordinal))
            .Select(static pair => new RuntimeSurfaceMemberStatusChange(
                pair.To.DisplayName,
                pair.From.ProjectionStatus,
                pair.To.ProjectionStatus,
                pair.From.UnsupportedReasonCode,
                pair.To.UnsupportedReasonCode))
            .OrderBy(static change => change.DisplayName, StringComparer.Ordinal)
            .ToArray();

        return new RuntimeSurfaceDiffReport(
            from.TargetFramework,
            to.TargetFramework,
            addedTypes,
            removedTypes,
            addedMembers,
            removedMembers,
            statusChanges);
    }

    private static Dictionary<string, BindingManifestRuntimeMember> FlattenMembers(IEnumerable<BindingManifestRuntimeType> types)
    {
        var members = new Dictionary<string, BindingManifestRuntimeMember>(StringComparer.Ordinal);
        foreach (var type in types)
        {
            foreach (var member in type.Members)
            {
                members[CreateMemberKey(member)] = member;
            }
        }

        return members;
    }

    private static string CreateTypeKey(BindingManifestRuntimeType type)
        => $"{type.Identity.AssemblyName}:{type.FullName}";

    private static string CreateMemberKey(BindingManifestRuntimeMember member)
        => $"{member.Identity.DeclaringType}:{member.Identity.Kind}:{member.Identity.SignatureKey}";
}

public sealed record RuntimeReferencePack(
    string TargetFramework,
    string PackVersion,
    string PackRoot,
    string RefDirectory,
    IReadOnlyList<string> AssemblyPaths);

public sealed record RuntimeReferencePackMissingTarget(string TargetFramework, string SearchRoots);

public sealed record RuntimeReferencePackDiscoveryResult(
    IReadOnlyList<RuntimeReferencePack> Packs,
    IReadOnlyList<RuntimeReferencePackMissingTarget> MissingTargets);

public sealed record RuntimeSurfaceScanSet(
    IReadOnlyList<RuntimeSurfaceScanReport> Reports,
    IReadOnlyList<RuntimeReferencePackMissingTarget> MissingTargets);

public sealed record RuntimeSurfaceDiffSet(IReadOnlyList<RuntimeSurfaceDiffReport> Diffs);

public sealed record RuntimeSurfaceDiffReport(
    string FromTargetFramework,
    string ToTargetFramework,
    IReadOnlyList<string> AddedTypes,
    IReadOnlyList<string> RemovedTypes,
    IReadOnlyList<string> AddedMembers,
    IReadOnlyList<string> RemovedMembers,
    IReadOnlyList<RuntimeSurfaceMemberStatusChange> StatusChanges);

public sealed record RuntimeSurfaceMemberStatusChange(
    string DisplayName,
    string FromStatus,
    string ToStatus,
    string? FromReasonCode,
    string? ToReasonCode);

public sealed record RuntimeSurfaceScanReport(
    string TargetFramework,
    string PackVersion,
    string PackRoot,
    string RefDirectory,
    IReadOnlyList<RuntimeAssemblyScanReport> Assemblies,
    int AssemblyCount,
    int ExportedTypeCount,
    int ScannedTypeCount,
    int SkippedTypeCount,
    int PublicMethodCount,
    int PublicPropertyCount,
    int PublicEventCount,
    int PublicConstructorCount,
    int ProjectedRequirementCount,
    int ProjectedMemberCount,
    int UnsupportedShapeCount,
    IReadOnlyDictionary<string, int> SkippedTypesByReason,
    IReadOnlyList<RuntimeUnsupportedReasonCount> UnsupportedShapesByReason,
    IReadOnlyList<BindingScanUnsupportedShape> UnsupportedShapes,
    IReadOnlyList<BindingManifestRuntimeType> Types);

public sealed record RuntimeAssemblyScanReport(
    string AssemblyName,
    BindingManifestAssemblyIdentity Identity,
    string AssemblyPath,
    int AssemblyCount,
    int ExportedTypeCount,
    int ScannedTypeCount,
    int SkippedTypeCount,
    int PublicMethodCount,
    int PublicPropertyCount,
    int PublicEventCount,
    int PublicConstructorCount,
    int ProjectedRequirementCount,
    int ProjectedMemberCount,
    int UnsupportedShapeCount,
    string? LoadDiagnostic,
    IReadOnlyList<BindingManifestRuntimeType> Types);

public sealed record RuntimeUnsupportedReasonCount(string Reason, int Count);
