using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using Rustlyn.Bindings;

namespace Rustlyn.Backend;

/// <summary>
/// Assembly emitter using System.Reflection.Metadata (SRM) — no external dependencies.
/// </summary>
public static class LoweredAssemblyEmitter
{
    [ThreadStatic]
    private static bool _strictUnsupportedIr;

    /// <summary>True when emission is running under <see cref="EmitOptions.StrictUnsupportedIr"/>.</summary>
    private static bool StrictUnsupportedIr => _strictUnsupportedIr;
    public static void EmitBitcode(string artifactPath, string outputAssemblyPath, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputAssemblyPath);

        var loweredModule = LoweredIrLowerer.LowerBitcode(artifactPath, llvmRoot);
        EmitModule(loweredModule, outputAssemblyPath);
    }

    public static void EmitBitcode(string artifactPath, string outputAssemblyPath, EmitOptions options, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputAssemblyPath);

        var loweredModule = LoweredIrLowerer.LowerBitcode(artifactPath, llvmRoot);
        EmitModule(loweredModule, outputAssemblyPath, options);
    }

    public static void EmitModule(LoweredModule loweredModule, string outputAssemblyPath)
    {
        EmitModule(loweredModule, outputAssemblyPath, EmitOptions.Default);
    }

    public static void EmitModule(LoweredModule loweredModule, string outputAssemblyPath, EmitOptions options)
    {
        ArgumentNullException.ThrowIfNull(loweredModule);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputAssemblyPath);

        var outputFullPath = Path.GetFullPath(outputAssemblyPath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath) ?? throw new InvalidOperationException("Output directory could not be determined."));

        var assemblyName = Path.GetFileNameWithoutExtension(outputFullPath);
        var emittedFunctions = GetReachableFunctions(loweredModule.Functions, loweredModule.Globals);
        var consoleEntrypoint = TrySelectConsoleEntrypoint(emittedFunctions);
        var requiredPackageManifests = GetRequiredExternalPackageManifests(emittedFunctions, options);
        var requiresAvaloniaSupport = RequiresAvaloniaSupport(emittedFunctions)
            || requiredPackageManifests.Any(static manifest => string.Equals(manifest.PackageSurface?.PackageId, "Avalonia", StringComparison.Ordinal));

        // Emit PDB if requested
        PortablePdbResult? pdbResult = null;
        if (options.EmitPdb)
        {
            var pdbPath = Path.ChangeExtension(outputFullPath, ".pdb");
            var pdbEmitter = new PortablePdbEmitter();

            // Register a synthetic source document based on the module source path or assembly name
            var sourceDoc = pdbEmitter.GetOrAddDocument(
                loweredModule.SourcePath ?? $"{assemblyName}.rs");

            // Add entry-point sequence points for each function (line = function index + 1)
            for (var i = 0; i < emittedFunctions.Count; i++)
            {
                var methodHandle = MetadataTokens.MethodDefinitionHandle(i + 1);
                pdbEmitter.AddMethodEntryPoint(methodHandle, sourceDoc, startLine: i + 1);
            }

            pdbResult = pdbEmitter.Serialize(pdbPath);
        }

        EmitAssembly(outputFullPath, assemblyName, loweredModule, emittedFunctions, consoleEntrypoint, requiresAvaloniaSupport, pdbResult, options);

        if (consoleEntrypoint is not null)
        {
            WriteRuntimeConfig(outputFullPath);
            CopyRuntimeSupportAssemblies(outputFullPath, requiredPackageManifests);
        }
    }

    internal static IReadOnlyList<LoweredFunction> GetReachableFunctions(IReadOnlyList<LoweredFunction> functions, IReadOnlyList<LoweredGlobal>? globals = null)
    {
        var roots = functions.Where(static function => IsExportedRoot(function.Name)).ToArray();
        if (roots.Length == 0)
        {
            return functions;
        }

        var functionMap = functions.ToDictionary(function => function.Name, StringComparer.Ordinal);
        var reachable = new HashSet<string>(StringComparer.Ordinal);
        var relocationRoots = globals is null
            ? Enumerable.Empty<string>()
            : globals.SelectMany(static global => global.PointerRelocations)
                .Select(static relocation => relocation.Target)
                .Where(functionMap.ContainsKey);
        var pendingRoots = roots.Select(static function => function.Name)
            .Concat(relocationRoots)
            .Distinct(StringComparer.Ordinal);
        var pending = new Stack<string>(pendingRoots.Reverse());

        while (pending.Count > 0)
        {
            var functionName = pending.Pop();
            if (!reachable.Add(functionName)
                || IsExcludedRuntimeStub(functionName)
                || !functionMap.TryGetValue(functionName, out var function))
            {
                continue;
            }

            foreach (var callee in function.Blocks
                         .SelectMany(static block => block.Instructions)
                         .OfType<LoweredCallInstruction>()
                         .Select(static call => call.Callee)
                         .Where(functionMap.ContainsKey))
            {
                pending.Push(callee);
            }

            foreach (var select in function.Blocks
                         .SelectMany(static block => block.Instructions)
                         .OfType<LoweredSelectInstruction>()
                         .Where(static s => string.Equals(s.ValueType, "ptr", StringComparison.Ordinal)))
            {
                var trueValue = NormalizeSymbolReference(select.TrueValue);
                var falseValue = NormalizeSymbolReference(select.FalseValue);
                if (functionMap.ContainsKey(trueValue))
                    pending.Push(trueValue);
                if (functionMap.ContainsKey(falseValue))
                    pending.Push(falseValue);
            }

            // Function pointers stored into locals (e.g., store ptr @func -> local)
            foreach (var storeValue in function.Blocks
                         .SelectMany(static block => block.Instructions)
                         .OfType<LoweredStoreInstruction>()
                         .Select(static store => NormalizeSymbolReference(store.Value))
                         .Where(functionMap.ContainsKey))
            {
                pending.Push(storeValue);
            }

            // Function pointers passed as call arguments (e.g., call apply(ptr @double, i32 5))
            foreach (var argValue in function.Blocks
                         .SelectMany(static block => block.Instructions)
                         .OfType<LoweredCallInstruction>()
                         .SelectMany(static call => call.Arguments)
                         .Select(static arg => NormalizeSymbolReference(arg.Value))
                         .Where(functionMap.ContainsKey))
            {
                pending.Push(argValue);
            }
        }

        return functions.Where(function => reachable.Contains(function.Name) && !IsExcludedRuntimeStub(function.Name)).ToArray();
    }

    private static bool IsExportedRoot(string functionName)
    {
        return !functionName.StartsWith("_ZN", StringComparison.Ordinal)
            && !functionName.StartsWith("_R", StringComparison.Ordinal);
    }

    private static bool IsExcludedRuntimeStub(string functionName)
    {
        return string.Equals(functionName, "rust_eh_personality", StringComparison.Ordinal)
            || string.Equals(functionName, "__rust_eh_personality", StringComparison.Ordinal);
    }

    private static LoweredFunction? TrySelectConsoleEntrypoint(IReadOnlyList<LoweredFunction> functions)
    {
        var mainFunction = functions.SingleOrDefault(static function => string.Equals(function.Name, "main", StringComparison.Ordinal));
        if (mainFunction is null)
            return null;
        if (mainFunction.Parameters.Count != 0)
            return null;
        if (!string.Equals(mainFunction.ReturnType, "void", StringComparison.Ordinal)
            && !string.Equals(mainFunction.ReturnType, "i32", StringComparison.Ordinal))
            return null;
        return mainFunction;
    }

    private static bool RequiresAvaloniaSupport(IReadOnlyList<LoweredFunction> functions)
        => functions.SelectMany(static function => function.Blocks)
            .SelectMany(static block => block.Instructions)
            .OfType<LoweredCallInstruction>()
            .Any(static call => call.Callee.StartsWith("rustlyn_avalonia_", StringComparison.Ordinal)
                || call.Callee.StartsWith("rustlyn_bindgen_avalonia_", StringComparison.Ordinal));

    private static IReadOnlyList<BindingManifestDocument> GetRequiredExternalPackageManifests(IReadOnlyList<LoweredFunction> functions, EmitOptions options)
    {
        var callees = functions.SelectMany(static function => function.Blocks)
            .SelectMany(static block => block.Instructions)
            .OfType<LoweredCallInstruction>()
            .Select(static call => call.Callee)
            .ToHashSet(StringComparer.Ordinal);
        var manifests = new List<BindingManifestDocument>();
        foreach (var manifest in options.BindingManifests.Where(static manifest => manifest.PackageSurface is not null))
        {
            if (manifest.Bindings.Any(binding => callees.Contains(binding.Symbol)))
            {
                AddExternalPackageManifest(manifests, manifest);
            }
        }

        if (callees.Any(static callee => callee.StartsWith("rustlyn_avalonia_", StringComparison.Ordinal)
                || callee.StartsWith("rustlyn_bindgen_avalonia_", StringComparison.Ordinal)))
        {
            AddExternalPackageManifest(manifests, ExternalPackageBindingSurfaces.CreateAvaloniaHelloManifest());
        }

        return manifests;
    }

    private static void AddExternalPackageManifest(List<BindingManifestDocument> manifests, BindingManifestDocument manifest)
    {
        var package = manifest.PackageSurface
            ?? throw new InvalidOperationException("External package manifest is missing package metadata.");
        if (manifests.Any(existing => string.Equals(existing.PackageSurface?.PackageId, package.PackageId, StringComparison.Ordinal)
                && string.Equals(existing.PackageSurface?.PackageVersion, package.PackageVersion, StringComparison.Ordinal)
                && string.Equals(existing.PackageSurface?.TargetFramework, package.TargetFramework, StringComparison.Ordinal)))
        {
            return;
        }

        manifests.Add(manifest);
    }

    private static void WriteRuntimeConfig(string outputAssemblyPath)
    {
        var runtimeConfigPath = Path.ChangeExtension(outputAssemblyPath, ".runtimeconfig.json");
        var runtimeConfig = $$"""
            {
              "runtimeOptions": {
                "tfm": "net{{Environment.Version.Major}}.{{Environment.Version.Minor}}",
                "framework": {
                  "name": "Microsoft.NETCore.App",
                  "version": "{{GetCurrentRuntimeVersion()}}"
                },
                "rollForward": "LatestPatch"
              }
            }
            """;
        File.WriteAllText(runtimeConfigPath, runtimeConfig);
    }

    private static string GetCurrentRuntimeVersion()
    {
        var description = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        var separatorIndex = description.LastIndexOf(' ');
        if (separatorIndex >= 0 && separatorIndex < description.Length - 1)
            return description[(separatorIndex + 1)..];
        return Environment.Version.ToString();
    }

    private static void CopyRuntimeSupportAssemblies(string outputAssemblyPath, IReadOnlyList<BindingManifestDocument> packageManifests)
    {
        CopySupportAssembly(typeof(RuntimeBridgeHelpers).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(Rustlyn.Runtime.NumericRuntime).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(Rustlyn.Os.HostEnvironment).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(Rustlyn.Interop.ManagedInteropRuntime).Assembly.Location, outputAssemblyPath);
        foreach (var manifest in packageManifests)
        {
            CopyExternalPackageRuntimeAssets(manifest, outputAssemblyPath);
        }
    }

    private static void CopySupportAssembly(string supportAssemblyPath, string outputAssemblyPath)
    {
        if (string.IsNullOrWhiteSpace(supportAssemblyPath) || !File.Exists(supportAssemblyPath))
            return;

        var destinationPath = Path.Combine(
            Path.GetDirectoryName(outputAssemblyPath) ?? throw new InvalidOperationException("Output directory could not be determined."),
            Path.GetFileName(supportAssemblyPath));

        if (string.Equals(Path.GetFullPath(destinationPath), Path.GetFullPath(supportAssemblyPath), StringComparison.OrdinalIgnoreCase))
            return;

        File.Copy(supportAssemblyPath, destinationPath, overwrite: true);
    }

    private static void CopyExternalPackageRuntimeAssets(BindingManifestDocument manifest, string outputAssemblyPath)
    {
        var package = manifest.PackageSurface
            ?? throw new InvalidOperationException("External package manifest is missing package metadata.");
        var sourceDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var assembly in package.Assemblies)
        {
            if (string.IsNullOrWhiteSpace(assembly.Path) || !File.Exists(assembly.Path))
            {
                continue;
            }

            CopySupportAssembly(assembly.Path, outputAssemblyPath);
            var assemblyDirectory = Path.GetDirectoryName(assembly.Path);
            if (!string.IsNullOrWhiteSpace(assemblyDirectory) && Directory.Exists(assemblyDirectory))
            {
                sourceDirectories.Add(assemblyDirectory);
            }
        }

        foreach (var sourceDirectory in sourceDirectories)
        {
            CopyExternalPackageRuntimeAssets(sourceDirectory, outputAssemblyPath, package.RuntimeAssetPatterns);
        }
    }

    private static void CopyExternalPackageRuntimeAssets(string sourceDirectory, string outputAssemblyPath, IReadOnlyList<string> runtimeAssetPatterns)
    {
        var destinationDirectory = Path.GetDirectoryName(outputAssemblyPath)
            ?? throw new InvalidOperationException("Output directory could not be determined.");
        if (!Directory.Exists(sourceDirectory))
            return;

        if (runtimeAssetPatterns.Contains("*.dll", StringComparer.Ordinal))
        {
            foreach (var filePath in Directory.GetFiles(sourceDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                CopySupportAssembly(filePath, outputAssemblyPath);
            }
        }

        if (runtimeAssetPatterns.Contains("*.json", StringComparer.Ordinal))
        {
            foreach (var filePath in Directory.GetFiles(sourceDirectory, "*.json", SearchOption.TopDirectoryOnly))
            {
                CopySupportAssembly(filePath, outputAssemblyPath);
            }
        }

        if (runtimeAssetPatterns.Any(static pattern => pattern.StartsWith("runtimes/", StringComparison.Ordinal)
                || pattern.StartsWith("runtimes\\", StringComparison.Ordinal)))
        {
            var runtimeAssetsDirectory = Path.Combine(sourceDirectory, "runtimes");
            if (Directory.Exists(runtimeAssetsDirectory))
            {
                CopyDirectory(runtimeAssetsDirectory, Path.Combine(destinationDirectory, "runtimes"));
                CopyCurrentRuntimeNativeAssets(runtimeAssetsDirectory, destinationDirectory);
            }
        }
    }

    private static void CopyCurrentRuntimeNativeAssets(string runtimeAssetsDirectory, string destinationDirectory)
    {
        var rid = GetCurrentRuntimeIdentifier();
        if (rid is null)
            return;

        var nativeDirectory = Path.Combine(runtimeAssetsDirectory, rid, "native");
        if (!Directory.Exists(nativeDirectory))
            return;

        foreach (var filePath in Directory.GetFiles(nativeDirectory, "*", SearchOption.TopDirectoryOnly))
        {
            File.Copy(filePath, Path.Combine(destinationDirectory, Path.GetFileName(filePath)), overwrite: true);
        }
    }

    private static string? GetCurrentRuntimeIdentifier()
    {
        var architecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture switch
        {
            System.Runtime.InteropServices.Architecture.X64 => "x64",
            System.Runtime.InteropServices.Architecture.X86 => "x86",
            System.Runtime.InteropServices.Architecture.Arm64 => "arm64",
            _ => null
        };
        if (architecture is null)
            return null;

        if (OperatingSystem.IsWindows())
            return $"win-{architecture}";
        if (OperatingSystem.IsLinux())
            return $"linux-{architecture}";
        if (OperatingSystem.IsMacOS())
            return "osx";
        return null;
    }

    private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        Directory.CreateDirectory(destinationDirectory);
        foreach (var filePath in Directory.GetFiles(sourceDirectory, "*", SearchOption.TopDirectoryOnly))
        {
            File.Copy(filePath, Path.Combine(destinationDirectory, Path.GetFileName(filePath)), overwrite: true);
        }

        foreach (var childDirectory in Directory.GetDirectories(sourceDirectory, "*", SearchOption.TopDirectoryOnly))
        {
            CopyDirectory(childDirectory, Path.Combine(destinationDirectory, Path.GetFileName(childDirectory)));
        }
    }

    private static void EmitAssembly(string outputPath, string assemblyName, LoweredModule loweredModule, IReadOnlyList<LoweredFunction> functions, LoweredFunction? consoleEntrypoint, bool requiresStaThread, PortablePdbResult? pdbResult = null, EmitOptions? emitOptions = null)
    {
        var metadataBuilder = new MetadataBuilder();
        var ilBuilder = new BlobBuilder();
        var methodBodyStream = new MethodBodyStreamEncoder(ilBuilder);

        // Assembly row
        metadataBuilder.AddAssembly(
            metadataBuilder.GetOrAddString(assemblyName),
            version: new Version(1, 0, 0, 0),
            culture: default,
            publicKey: default,
            flags: default,
            hashAlgorithm: System.Reflection.AssemblyHashAlgorithm.Sha1);

        // Module row
        var isExe = consoleEntrypoint is not null;
        metadataBuilder.AddModule(
            generation: 0,
            moduleName: metadataBuilder.GetOrAddString(assemblyName + (isExe ? ".exe" : ".dll")),
            mvid: metadataBuilder.GetOrAddGuid(Guid.NewGuid()),
            encId: default,
            encBaseId: default);

        // Assembly reference: System.Runtime
        var systemRuntimeRef = metadataBuilder.AddAssemblyReference(
            name: metadataBuilder.GetOrAddString("System.Runtime"),
            version: new Version(10, 0, 0, 0),
            culture: default,
            publicKeyOrToken: metadataBuilder.GetOrAddBlob(
                new byte[] { 0xB0, 0x3F, 0x5F, 0x7F, 0x11, 0xD5, 0x0A, 0x3A }),
            flags: default,
            hashValue: default);

        // Assembly reference: System.Runtime.InteropServices (for Marshal)
        var systemInteropRef = metadataBuilder.AddAssemblyReference(
            name: metadataBuilder.GetOrAddString("System.Runtime.InteropServices"),
            version: new Version(10, 0, 0, 0),
            culture: default,
            publicKeyOrToken: metadataBuilder.GetOrAddBlob(
                new byte[] { 0xB0, 0x3F, 0x5F, 0x7F, 0x11, 0xD5, 0x0A, 0x3A }),
            flags: default,
            hashValue: default);

        // Pre-compute BCL type/member references
        var typeContext = new SrmTypeContext(metadataBuilder, systemRuntimeRef, systemInteropRef, emitOptions?.BindingManifests);

        // Type reference: System.Object (base type for our generated class)
        var systemObjectRef = metadataBuilder.AddTypeReference(
            systemRuntimeRef,
            metadataBuilder.GetOrAddString("System"),
            metadataBuilder.GetOrAddString("Object"));

        // Emit global fields
        var fieldHandles = new Dictionary<string, FieldDefinitionHandle>(StringComparer.Ordinal);
        var globalMap = loweredModule.Globals.ToDictionary(g => g.Name, StringComparer.Ordinal);
        var fieldIndex = 1;
        foreach (var global in loweredModule.Globals)
        {
            var fieldSigBlob = EncodeFieldSignature(metadataBuilder, global);
            var fieldHandle = metadataBuilder.AddFieldDefinition(
                attributes: FieldAttributes.Public | FieldAttributes.Static,
                name: metadataBuilder.GetOrAddString(global.Name),
                signature: fieldSigBlob);
            fieldHandles[global.Name] = fieldHandle;
            fieldIndex++;
        }

        // <Module> implicit type (required by ECMA-335, must be first TypeDef)
        metadataBuilder.AddTypeDefinition(
            attributes: default,
            @namespace: default,
            name: metadataBuilder.GetOrAddString("<Module>"),
            baseType: default,
            fieldList: MetadataTokens.FieldDefinitionHandle(1),
            methodList: MetadataTokens.MethodDefinitionHandle(1));

        // Rustlyn.GeneratedModule static class
        metadataBuilder.AddTypeDefinition(
            attributes: TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class,
            @namespace: metadataBuilder.GetOrAddString("Rustlyn"),
            name: metadataBuilder.GetOrAddString("GeneratedModule"),
            baseType: systemObjectRef,
            fieldList: MetadataTokens.FieldDefinitionHandle(1),
            methodList: MetadataTokens.MethodDefinitionHandle(1));

        // Pre-compute method handles (SRM assigns handles sequentially)
        // .cctor goes last if globals exist
        var hasCctor = loweredModule.Globals.Count > 0;
        var totalMethods = functions.Count + (hasCctor ? 1 : 0);
        var methodHandles = new Dictionary<string, MethodDefinitionHandle>(StringComparer.Ordinal);
        for (var i = 0; i < functions.Count; i++)
        {
            methodHandles[functions[i].Name] = MetadataTokens.MethodDefinitionHandle(i + 1);
        }

        // Track functions that return aggregates too large for i64 packing (need sret buffer)
        var sretFunctions = new HashSet<string>(StringComparer.Ordinal);
        foreach (var fn in functions)
        {
            if (IsAggregateType(fn.ReturnType) && AggregateNeedsSret(fn.ReturnType))
                sretFunctions.Add(fn.Name);
        }

        // Pre-scan for globals mutated by store/atomicrmw/cmpxchg (skip constant folding for these)
        var mutatedGlobals = new HashSet<string>(StringComparer.Ordinal);
        foreach (var fn in functions)
        {
            foreach (var block in fn.Blocks)
            {
                foreach (var instr in block.Instructions)
                {
                    if (instr is LoweredStoreInstruction st && fieldHandles.ContainsKey(st.Destination))
                        mutatedGlobals.Add(st.Destination);
                    else if (instr is LoweredAtomicRmwInstruction rmw && fieldHandles.ContainsKey(rmw.Pointer))
                        mutatedGlobals.Add(rmw.Pointer);
                    else if (instr is LoweredCmpxchgInstruction cx && fieldHandles.ContainsKey(cx.Pointer))
                        mutatedGlobals.Add(cx.Pointer);
                }
            }
        }

        // Emit method bodies first (need all handle mappings available for intra-module calls)
        var bodyOffsets = new int[totalMethods];
        var skippedFunctions = new List<(string Name, string Reason)>();
        var strict = emitOptions?.StrictUnsupportedIr == true;
        _strictUnsupportedIr = strict;
        try
        {
            for (var i = 0; i < functions.Count; i++)
            {
                try
                {
                    bodyOffsets[i] = EmitFunctionBody(metadataBuilder, methodBodyStream, functions[i], methodHandles, typeContext, fieldHandles, globalMap, sretFunctions, mutatedGlobals);
                }
                catch (NotSupportedException ex)
                {
                    skippedFunctions.Add((functions[i].Name, ex.Message));
                    if (!strict)
                    {
                        bodyOffsets[i] = EmitStubBody(metadataBuilder, methodBodyStream, typeContext, functions[i].Name, ex.Message);
                    }
                }
                catch (Exception ex) when (ex is not OutOfMemoryException)
                {
                    skippedFunctions.Add((functions[i].Name, ex.Message));
                    if (!strict)
                    {
                        bodyOffsets[i] = EmitStubBody(metadataBuilder, methodBodyStream, typeContext, functions[i].Name, ex.Message);
                    }
                }
            }

            if (skippedFunctions.Count > 0)
            {
                if (strict)
                {
                    throw new UnsupportedIrException(skippedFunctions.ConvertAll(static f =>
                        new UnsupportedIrFunction(
                            f.Name,
                            f.Reason,
                            UnsupportedIrDiagnosticClassifier.Classify(f.Reason))));
                }

                Console.Error.WriteLine($"Warning: {skippedFunctions.Count} function(s) stubbed due to unsupported IR:");
                foreach (var (name, reason) in skippedFunctions)
                {
                    Console.Error.WriteLine($"  {name}: {reason}");
                }
            }
        }
        finally
        {
            _strictUnsupportedIr = false;
        }

        // Emit .cctor body if we have globals
        if (hasCctor)
        {
            bodyOffsets[functions.Count] = EmitCctorBody(metadataBuilder, methodBodyStream, loweredModule.Globals, fieldHandles, methodHandles, globalMap, typeContext);
        }

        // Add method definition rows + parameters in matching order
        var paramRowIndex = 1;
        for (var i = 0; i < functions.Count; i++)
        {
            var function = functions[i];
            var signatureBlob = BuildMethodSignature(metadataBuilder, function);

            metadataBuilder.AddMethodDefinition(
                attributes: MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig,
                implAttributes: MethodImplAttributes.IL | MethodImplAttributes.Managed,
                name: metadataBuilder.GetOrAddString(function.Name),
                signature: signatureBlob,
                bodyOffset: bodyOffsets[i],
                parameterList: MetadataTokens.ParameterHandle(paramRowIndex));

            var fnIsSret = sretFunctions.Contains(function.Name);
            if (fnIsSret)
            {
                metadataBuilder.AddParameter(
                    attributes: ParameterAttributes.None,
                    name: metadataBuilder.GetOrAddString("__retbuf"),
                    sequenceNumber: 1);
                paramRowIndex++;
            }
            var seqOffset = fnIsSret ? 1 : 0;
            for (var j = 0; j < function.Parameters.Count; j++)
            {
                metadataBuilder.AddParameter(
                    attributes: ParameterAttributes.None,
                    name: metadataBuilder.GetOrAddString(function.Parameters[j].Name),
                    sequenceNumber: j + 1 + seqOffset);
                paramRowIndex++;
            }
        }

        // Add .cctor method definition if needed
        if (hasCctor)
        {
            var cctorSigBlob = new BlobBuilder();
            new BlobEncoder(cctorSigBlob).MethodSignature().Parameters(0, out var cctorRet, out _);
            cctorRet.Void();

            metadataBuilder.AddMethodDefinition(
                attributes: MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                implAttributes: MethodImplAttributes.IL | MethodImplAttributes.Managed,
                name: metadataBuilder.GetOrAddString(".cctor"),
                signature: metadataBuilder.GetOrAddBlob(cctorSigBlob),
                bodyOffset: bodyOffsets[functions.Count],
                parameterList: MetadataTokens.ParameterHandle(paramRowIndex));
            paramRowIndex++; // .cctor has no user params but occupies a row
        }

        // Emit console entry point wrapper if needed
        MethodDefinitionHandle entryPointHandle = default;
        if (consoleEntrypoint is not null)
        {
            entryPointHandle = EmitConsoleEntrypoint(metadataBuilder, methodBodyStream, ilBuilder, functions, consoleEntrypoint, methodHandles, typeContext, systemObjectRef, requiresStaThread, ref paramRowIndex);
        }

        // Write PE
        WritePE(outputPath, metadataBuilder, ilBuilder, entryPoint: entryPointHandle, pdbResult: pdbResult);
    }

    private static BlobHandle BuildMethodSignature(MetadataBuilder metadataBuilder, LoweredFunction function)
    {
        var isSret = IsAggregateType(function.ReturnType) && AggregateNeedsSret(function.ReturnType);
        var blob = new BlobBuilder();
        var paramCount = function.Parameters.Count + (isSret ? 1 : 0);
        new BlobEncoder(blob)
            .MethodSignature()
            .Parameters(paramCount, out var returnTypeEncoder, out var parametersEncoder);

        if (isSret)
        {
            returnTypeEncoder.Void();
            parametersEncoder.AddParameter().Type().IntPtr(); // hidden retbuf
        }
        else
        {
            EncodeReturnType(returnTypeEncoder, function.ReturnType);
        }
        foreach (var param in function.Parameters)
        {
            EncodeParameterType(parametersEncoder, param.Type);
        }

        return metadataBuilder.GetOrAddBlob(blob);
    }

    private static void EncodeReturnType(ReturnTypeEncoder encoder, string typeName)
    {
        if (string.Equals(typeName, "void", StringComparison.Ordinal))
        {
            encoder.Void();
            return;
        }
        // Aggregate return types like { i32, i32 } are packed into i64
        if (IsAggregateType(typeName))
        {
            encoder.Type().Int64();
            return;
        }
        EncodeSignatureType(encoder.Type(), typeName);
    }

    private static void EncodeParameterType(ParametersEncoder encoder, string typeName)
    {
        EncodeSignatureType(encoder.AddParameter().Type(), typeName);
    }

    private static void EncodeSignatureType(SignatureTypeEncoder encoder, string typeName)
    {
        if (TryParseVectorType(typeName, out _, out var elementType))
        {
            EncodeVectorArrayType(encoder, elementType);
            return;
        }

        switch (typeName)
        {
            case "ptr":
                encoder.IntPtr();
                break;
            case "float":
                encoder.Single();
                break;
            case "double":
                encoder.Double();
                break;
            default:
                if (TryGetIntegerBitWidth(typeName, out var width))
                {
                    if (width <= 32)
                        encoder.Int32();
                    else
                        encoder.Int64();
                }
                else
                {
                    encoder.Int32(); // fallback
                }
                break;
        }
    }

    private static void EncodeVectorArrayType(SignatureTypeEncoder encoder, string elementType)
    {
        var elementEncoder = encoder.SZArray();
        switch (elementType)
        {
            case "i8":
                elementEncoder.SByte();
                break;
            case "i16":
                elementEncoder.Int16();
                break;
            case "i64":
                elementEncoder.Int64();
                break;
            default:
                elementEncoder.Int32();
                break;
        }
    }

    private static bool IsScalarLocalType(string typeName)
    {
        return string.Equals(typeName, "ptr", StringComparison.Ordinal)
            || string.Equals(typeName, "float", StringComparison.Ordinal)
            || string.Equals(typeName, "double", StringComparison.Ordinal)
            || TryGetIntegerBitWidth(typeName, out _);
    }

    private static int EmitFunctionBody(MetadataBuilder metadataBuilder, MethodBodyStreamEncoder methodBodyStream, LoweredFunction function, IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles, SrmTypeContext typeContext, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, LoweredGlobal> globalMap, IReadOnlySet<string> sretFunctions, IReadOnlySet<string> mutatedGlobals)
    {
        var isSret = sretFunctions.Contains(function.Name);
        var codeBuilder = new BlobBuilder();
        var controlFlowBuilder = new ControlFlowBuilder();
        var encoder = new InstructionEncoder(codeBuilder, controlFlowBuilder);

        // Pre-allocate labels for all basic blocks
        var labelMap = new Dictionary<string, LabelHandle>(StringComparer.Ordinal);
        foreach (var block in function.Blocks)
        {
            labelMap[block.Name] = encoder.DefineLabel();
        }

        // Build PHI map: target block → PHI instructions in that block
        var phiByBlock = function.Blocks.ToDictionary(
            block => block.Name,
            block => block.Instructions.OfType<LoweredPhiInstruction>().ToArray(),
            StringComparer.Ordinal);

        // Pre-scan for locals (any instruction with a named result gets a local slot)
        var localCount = 0;
        var localIndices = new Dictionary<string, int>(StringComparer.Ordinal);
        var localTypes = new List<string>(); // track type for each local
        var aggregatePointerLocals = new HashSet<string>(StringComparer.Ordinal);
        foreach (var block in function.Blocks)
        {
            foreach (var instruction in block.Instructions)
            {
                var resultName = GetInstructionResult(instruction);
                if (resultName is not null && !localIndices.ContainsKey(resultName))
                {
                    localIndices[resultName] = localCount++;
                    var localType = GetLocalType(instruction);
                    // Sret calls and their insertvalues produce buffer pointers, not packed i64
                    if (instruction is LoweredCallInstruction sc
                        && IsAggregateType(sc.ReturnType) && sretFunctions.Contains(sc.Callee))
                    {
                        localType = "ptr";
                        aggregatePointerLocals.Add(sc.Result!);
                    }
                    else if (instruction is LoweredCallInstruction rbc
                        && IsAggregateType(rbc.ReturnType)
                        && AggregateNeedsSret(rbc.ReturnType)
                        && typeContext.RuntimeBridgeReturnsViaSret(rbc.Callee))
                    {
                        localType = "ptr";
                        aggregatePointerLocals.Add(rbc.Result!);
                    }
                    else if (instruction is LoweredInsertValueInstruction siv
                        && isSret && IsAggregateType(siv.AggregateType) && AggregateNeedsSret(siv.AggregateType))
                    {
                        localType = "ptr";
                        aggregatePointerLocals.Add(siv.Result);
                    }
                    localTypes.Add(localType);
                }
            }
        }

        // Identify allocas that have variable-index GEP access — these need localloc
        // (can't be scalar-replaced because they need real addressable memory)
        var locallocAllocas = new HashSet<string>(StringComparer.Ordinal);
        // Also identify allocas whose address is passed to function calls (ptr-passing)
        var allocaNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredAllocaInstruction a)
                    allocaNames.Add(a.Result);
                if (instr is LoweredGetElementPointerInstruction gep && gep.IndexVariable is not null)
                    locallocAllocas.Add(gep.Base);
            }
        }
        // Detect allocas passed as ptr arguments to calls
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredCallInstruction call)
                {
                    foreach (var arg in call.Arguments)
                    {
                        if (allocaNames.Contains(arg.Value))
                            locallocAllocas.Add(arg.Value);
                    }
                }
                else if (instr is LoweredStoreInstruction store
                    && string.Equals(store.Type, "ptr", StringComparison.Ordinal)
                    && allocaNames.Contains(NormalizeSymbolReference(store.Value)))
                {
                    locallocAllocas.Add(NormalizeSymbolReference(store.Value));
                }
            }
        }

        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredStoreInstruction store
                    && allocaNames.Contains(store.Destination)
                    && !locallocAllocas.Contains(store.Destination)
                    && localIndices.TryGetValue(store.Destination, out var scalarAllocaLocal)
                    && IsScalarLocalType(store.Type))
                {
                    localTypes[scalarAllocaLocal] = store.Type;
                }
            }
        }

        // Track which parameters are ptr type (receive addresses, need ldind/stind)
        var ptrParams = new HashSet<string>(StringComparer.Ordinal);
        foreach (var p in function.Parameters)
        {
            if (string.Equals(p.Type, "ptr", StringComparison.Ordinal))
                ptrParams.Add(p.Name);
        }

        // SROA: for allocas accessed via constant-index GEPs, create element locals
        // gepElementLocal maps a GEP result name -> the local index of the element slot
        var gepElementLocal = new Dictionary<string, int>(StringComparer.Ordinal);
        var allocaElementLocals = new Dictionary<(string alloca, int index), int>();
        // gepBaseOffset tracks each GEP result -> (root alloca, combined byte offset)
        var gepBaseOffset = new Dictionary<string, (string root, int offset)>(StringComparer.Ordinal);
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredGetElementPointerInstruction gep && gep.IndexVariable is null)
                {
                    // Resolve GEP-on-GEP chains: if base is itself a GEP result, combine offsets
                    string rootBase = gep.Base;
                    int combinedOffset = gep.Index;
                    if (gepBaseOffset.TryGetValue(gep.Base, out var parentGep))
                    {
                        rootBase = parentGep.root;
                        combinedOffset = parentGep.offset + gep.Index;
                    }
                    gepBaseOffset[gep.Result] = (rootBase, combinedOffset);

                    // Skip SROA for allocas that need localloc (variable-index access)
                    if (locallocAllocas.Contains(rootBase))
                        continue;

                    // Only apply SROA to GEPs whose root is an alloca (not params or other locals)
                    if (!allocaNames.Contains(rootBase))
                        continue;

                    var key = (rootBase, combinedOffset);
                    if (!allocaElementLocals.TryGetValue(key, out var elemLocalIdx))
                    {
                        elemLocalIdx = localCount++;
                        allocaElementLocals[key] = elemLocalIdx;
                        localTypes.Add(gep.ElementType);
                    }
                    gepElementLocal[gep.Result] = elemLocalIdx;
                }
            }
        }

        // Multi-value locals: for extractvalue on overflow-intrinsic calls and cmpxchg
        // multiValueLocals maps a call/instruction result name -> array of local indices per field
        var multiValueLocals = new Dictionary<string, int[]>(StringComparer.Ordinal);
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredExtractValueInstruction ev && !multiValueLocals.ContainsKey(ev.Source))
                {
                    // Allocate multi-value locals for overflow intrinsic results and cmpxchg results
                    var sourceCall = FindCallByResult(function, ev.Source);
                    var sourceCmpxchg = sourceCall is null ? FindCmpxchgByResult(function, ev.Source) : null;
                    if ((sourceCall is not null && sourceCall.Callee.Contains(".with.overflow.", StringComparison.Ordinal))
                        || sourceCmpxchg is not null)
                    {
                        var fieldTypes = ParseAggregateFieldTypes(ev.AggregateType);
                        var fieldLocals = new int[fieldTypes.Length];
                        for (var fi = 0; fi < fieldTypes.Length; fi++)
                        {
                            fieldLocals[fi] = localCount++;
                            localTypes.Add(fieldTypes[fi]);
                        }
                        multiValueLocals[ev.Source] = fieldLocals;
                    }
                }
            }
        }

        // Build local variables signature
        StandaloneSignatureHandle localSigHandle = default;
        if (localCount > 0)
        {
            var localSigBlob = new BlobBuilder();
            var localEncoder = new BlobEncoder(localSigBlob).LocalVariableSignature(localCount);
            for (var i = 0; i < localCount; i++)
            {
                EncodeSignatureType(localEncoder.AddVariable().Type(), localTypes[i]);
            }
            localSigHandle = metadataBuilder.AddStandaloneSignature(metadataBuilder.GetOrAddBlob(localSigBlob));
        }

        // Parameter name -> argument index
        var paramIndices = new Dictionary<string, int>(StringComparer.Ordinal);
        var paramShift = isSret ? 1 : 0; // arg 0 is hidden retbuf for sret functions
        for (var i = 0; i < function.Parameters.Count; i++)
        {
            paramIndices[function.Parameters[i].Name] = i + paramShift;
        }

        // Emit IL per basic block
        foreach (var block in function.Blocks)
        {
            encoder.MarkLabel(labelMap[block.Name]);

            for (var instrIdx = 0; instrIdx < block.Instructions.Count; instrIdx++)
            {
                EmitInstruction(encoder, block.Instructions, ref instrIdx, paramIndices, localIndices, labelMap, methodHandles, phiByBlock, block.Name, typeContext, fieldHandles, globalMap, metadataBuilder, gepElementLocal, multiValueLocals, locallocAllocas, ptrParams, allocaNames, aggregatePointerLocals, sretFunctions, isSret, function.ReturnExtension, mutatedGlobals);
            }
        }

        // Encode method body into stream
        return methodBodyStream.AddMethodBody(
            encoder,
            maxStack: 8,
            localVariablesSignature: localSigHandle,
            attributes: localCount > 0 ? MethodBodyAttributes.InitLocals : MethodBodyAttributes.None);
    }

    private static void EmitInstruction(
        InstructionEncoder encoder,
        IReadOnlyList<LoweredInstruction> instructions,
        ref int instrIdx,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, LabelHandle> labelMap,
        IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles,
        IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock,
        string sourceBlock,
        SrmTypeContext typeContext,
        IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles,
        IReadOnlyDictionary<string, LoweredGlobal> globalMap,
        MetadataBuilder metadataBuilder,
        IReadOnlyDictionary<string, int> gepElementLocal,
        IReadOnlyDictionary<string, int[]> multiValueLocals,
        IReadOnlySet<string> locallocAllocas,
        IReadOnlySet<string> ptrParams,
        IReadOnlySet<string> allocaNames,
        IReadOnlySet<string> aggregatePointerLocals,
        IReadOnlySet<string> sretFunctions,
        bool isSret,
        string? returnExtension,
        IReadOnlySet<string> mutatedGlobals)
    {
        var instruction = instructions[instrIdx];
        switch (instruction)
        {
            case LoweredBinaryInstruction binary:
                if (TryEmitVectorBinaryInstruction(encoder, binary, typeContext, paramIndices, localIndices, fieldHandles))
                {
                    break;
                }

                EmitLoadValue(encoder, binary.Left, paramIndices, localIndices, fieldHandles);
                EmitLoadValue(encoder, binary.Right, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(MapBinaryOp(binary.Operation));
                encoder.StoreLocal(localIndices[binary.Result]);
                break;

            case LoweredReturnInstruction ret:
                if (isSret && IsAggregateType(ret.Type))
                {
                    // Sret: fields already written to retbuf via insertvalue. Just ret void.
                    encoder.OpCode(ILOpCode.Ret);
                }
                else if (!string.Equals(ret.Type, "void", StringComparison.Ordinal))
                {
                    EmitLoadValue(encoder, ret.Value, paramIndices, localIndices, fieldHandles);
                    EmitIntegerExtension(encoder, ret.Type, returnExtension);
                    encoder.OpCode(ILOpCode.Ret);
                }
                else
                {
                    encoder.OpCode(ILOpCode.Ret);
                }
                break;

            case LoweredConditionalBranchInstruction condBr:
                {
                    if (IsVectorLoopGuard(condBr))
                    {
                        EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, condBr.TrueTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.TrueTarget]);
                        break;
                    }

                    var trueHasPhis = phiByBlock.TryGetValue(condBr.TrueTarget, out var truePhis) && truePhis.Length > 0;
                    var falseHasPhis = phiByBlock.TryGetValue(condBr.FalseTarget, out var falsePhis) && falsePhis.Length > 0;

                    if (!trueHasPhis && !falseHasPhis)
                    {
                        EmitLoadValue(encoder, condBr.Condition, paramIndices, localIndices, fieldHandles);
                        encoder.Branch(ILOpCode.Brtrue, labelMap[condBr.TrueTarget]);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.FalseTarget]);
                    }
                    else
                    {
                        var falsePathLabel = encoder.DefineLabel();
                        EmitLoadValue(encoder, condBr.Condition, paramIndices, localIndices, fieldHandles);
                        encoder.Branch(ILOpCode.Brfalse, falsePathLabel);
                        EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, condBr.TrueTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.TrueTarget]);
                        encoder.MarkLabel(falsePathLabel);
                        EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, condBr.FalseTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.FalseTarget]);
                    }
                }
                break;

            case LoweredJumpInstruction jump:
                EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, jump.Target);
                encoder.Branch(ILOpCode.Br, labelMap[jump.Target]);
                break;

            case LoweredCompareInstruction cmp:
                if (TryEmitVectorCompareInstruction(encoder, cmp, typeContext, paramIndices, localIndices, fieldHandles))
                {
                    break;
                }

                EmitLoadValue(encoder, cmp.Left, paramIndices, localIndices, fieldHandles);
                EmitSignedNarrowNormalization(encoder, cmp.Type, cmp.Predicate);
                EmitLoadValue(encoder, cmp.Right, paramIndices, localIndices, fieldHandles);
                EmitSignedNarrowNormalization(encoder, cmp.Type, cmp.Predicate);
                EmitCompare(encoder, cmp.Predicate);
                encoder.StoreLocal(localIndices[cmp.Result]);
                break;

            case LoweredPhiInstruction:
                break;

            case LoweredSelectInstruction sel:
                {
                    var falseLabel = encoder.DefineLabel();
                    var endLabel = encoder.DefineLabel();
                    EmitLoadValue(encoder, sel.Condition, paramIndices, localIndices, fieldHandles);
                    encoder.Branch(ILOpCode.Brfalse, falseLabel);
                    EmitLoadValue(encoder, sel.TrueValue, paramIndices, localIndices, fieldHandles, methodHandles);
                    encoder.Branch(ILOpCode.Br, endLabel);
                    encoder.MarkLabel(falseLabel);
                    EmitLoadValue(encoder, sel.FalseValue, paramIndices, localIndices, fieldHandles, methodHandles);
                    encoder.MarkLabel(endLabel);
                    encoder.StoreLocal(localIndices[sel.Result]);
                }
                break;

            case LoweredTruncateInstruction trunc:
                EmitLoadValue(encoder, trunc.Value, paramIndices, localIndices, fieldHandles);
                if (string.Equals(trunc.ToType, "i1", StringComparison.Ordinal))
                {
                    encoder.LoadConstantI4(1);
                    encoder.OpCode(ILOpCode.And);
                }
                else
                {
                    encoder.OpCode(trunc.ToType switch
                    {
                        "float" => ILOpCode.Conv_r4,
                        "double" => ILOpCode.Conv_r8,
                        "i8" => ILOpCode.Conv_i1,
                        "i16" => ILOpCode.Conv_i2,
                        "i64" => ILOpCode.Conv_i8,
                        _ => ILOpCode.Conv_i4
                    });
                }
                encoder.StoreLocal(localIndices[trunc.Result]);
                break;

            case LoweredZeroExtendInstruction zext:
                EmitLoadValue(encoder, zext.Value, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(ILOpCode.Conv_u8);
                encoder.StoreLocal(localIndices[zext.Result]);
                break;

            case LoweredSignExtendInstruction sext:
                EmitLoadValue(encoder, sext.Value, paramIndices, localIndices, fieldHandles);
                if (sext.FromType == "i1")
                {
                    // sext i1 1 → -1: negate the boolean (0→0, 1→-1)
                    encoder.OpCode(ILOpCode.Neg);
                }
                if (sext.ToType is "i64" or "i128")
                    encoder.OpCode(ILOpCode.Conv_i8);
                else if (sext.ToType == "i32")
                    encoder.OpCode(ILOpCode.Conv_i4);
                else if (sext.ToType == "i16")
                    encoder.OpCode(ILOpCode.Conv_i2);
                encoder.StoreLocal(localIndices[sext.Result]);
                break;

            case LoweredPtrToIntInstruction p2i:
                EmitLoadValue(encoder, p2i.Value, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(p2i.ToType switch
                {
                    "i64" => ILOpCode.Conv_u8,
                    _ => ILOpCode.Conv_u
                });
                encoder.StoreLocal(localIndices[p2i.Result]);
                break;

            case LoweredIntToPtrInstruction i2p:
                EmitLoadValue(encoder, i2p.Value, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(ILOpCode.Conv_u);
                encoder.StoreLocal(localIndices[i2p.Result]);
                break;

            case LoweredFreezeInstruction freeze:
                EmitLoadValue(encoder, freeze.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                encoder.StoreLocal(localIndices[freeze.Result]);
                break;

            case LoweredBitcastInstruction bitcast:
                if (!TryEmitBitcastInstruction(encoder, bitcast, typeContext, paramIndices, localIndices, fieldHandles))
                {
                    throw new NotSupportedException($"LLVM bitcast from {bitcast.FromType} to {bitcast.ToType} is not yet supported.");
                }
                break;

            case LoweredInsertElementInstruction insertElement:
                EmitInsertElementInstruction(encoder, insertElement, typeContext, paramIndices, localIndices, fieldHandles);
                break;

            case LoweredShuffleVectorInstruction shuffleVector:
                if (!TryEmitShuffleVectorInstruction(encoder, shuffleVector, typeContext, paramIndices, localIndices, fieldHandles))
                {
                    throw new NotSupportedException($"LLVM shufflevector with mask '{shuffleVector.Mask}' is not yet supported.");
                }
                break;

            case LoweredCallInstruction call:
                {
                    var callArgs = call.Arguments;
                    var isCtlzCttz = call.Callee.StartsWith("llvm.ctlz.", StringComparison.Ordinal)
                                  || call.Callee.StartsWith("llvm.cttz.", StringComparison.Ordinal);
                    var effectiveArgCount = isCtlzCttz ? Math.Min(callArgs.Count, 1) : callArgs.Count;

                    // Check if this call produces a multi-value result (e.g., overflow intrinsics)
                    if (call.Result is not null && multiValueLocals.TryGetValue(call.Result, out var mvLocals))
                    {
                        if (TryEmitOverflowIntrinsic(encoder, call, paramIndices, localIndices, fieldHandles, mvLocals))
                        {
                            // Overflow intrinsic handled — results stored in mvLocals
                            break;
                        }
                    }

                    // Saturating arithmetic: intercept before arg push for proper clamping
                    if (TryEmitSaturatingIntrinsic(encoder, call, paramIndices, localIndices, fieldHandles))
                    {
                        if (call.Result is not null)
                            encoder.StoreLocal(localIndices[call.Result]);
                        break;
                    }

                    if (TryEmitNarrowMinMaxIntrinsic(encoder, typeContext, call, paramIndices, localIndices, fieldHandles))
                    {
                        if (call.Result is not null)
                            encoder.StoreLocal(localIndices[call.Result]);
                        break;
                    }

                    // Memory intrinsics: memcpy/memset/memmove → cpblk/initblk
                    if (call.Callee.StartsWith("llvm.memcpy.", StringComparison.Ordinal)
                        || call.Callee.StartsWith("llvm.memmove.", StringComparison.Ordinal))
                    {
                        // cpblk: dest, src, size (drop isvolatile arg)
                        EmitLoadPtrValue(encoder, call.Arguments[0], paramIndices, localIndices, fieldHandles, globalMap, methodHandles);
                        EmitLoadPtrValue(encoder, call.Arguments[1], paramIndices, localIndices, fieldHandles, globalMap, methodHandles);
                        EmitLoadValue(encoder, call.Arguments[2].Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.OpCode(ILOpCode.Conv_u);
                        encoder.OpCode(ILOpCode.Cpblk);
                        break;
                    }
                    if (call.Callee.StartsWith("llvm.memset.", StringComparison.Ordinal))
                    {
                        // initblk: dest, val, size (drop isvolatile arg)
                        EmitLoadPtrValue(encoder, call.Arguments[0], paramIndices, localIndices, fieldHandles, globalMap, methodHandles);
                        EmitLoadValue(encoder, call.Arguments[1].Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadValue(encoder, call.Arguments[2].Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.OpCode(ILOpCode.Conv_u);
                        encoder.OpCode(ILOpCode.Initblk);
                        break;
                    }

                    if (TryEmitVectorReduceCall(encoder, call, paramIndices, localIndices, fieldHandles))
                    {
                        if (call.Result is not null)
                            encoder.StoreLocal(localIndices[call.Result]);
                        break;
                    }

                    // Indirect call: callee is a local variable or parameter (function pointer)
                    var indirectCalleeName = NormalizeIndirectCalleeName(call.Callee);
                    var isIndirectCall = localIndices.ContainsKey(indirectCalleeName)
                        || paramIndices.ContainsKey(indirectCalleeName);
                    var hasRuntimeBridge = typeContext.TryResolveRuntimeBridge(call.Callee, out var runtimeBridgeHandle);

                    // Sret call: callee returns aggregate via hidden retbuf parameter
                    var isRuntimeBridgeSretCall = hasRuntimeBridge
                        && IsAggregateType(call.ReturnType)
                        && AggregateNeedsSret(call.ReturnType)
                        && typeContext.RuntimeBridgeReturnsViaSret(call.Callee);
                    var isSretCall = !isIndirectCall && (sretFunctions.Contains(call.Callee) || isRuntimeBridgeSretCall);
                    if (isSretCall && call.Result is not null)
                    {
                        // Allocate buffer for aggregate result and store pointer in result local
                        var aggSize = GetAggregateSize(call.ReturnType);
                        encoder.LoadConstantI4(aggSize);
                        encoder.OpCode(ILOpCode.Conv_u);
                        encoder.OpCode(ILOpCode.Localloc);
                        encoder.StoreLocal(localIndices[call.Result]);
                        // Push buffer as hidden first arg
                        encoder.LoadLocal(localIndices[call.Result]);
                    }

                    for (var argIdx = 0; argIdx < effectiveArgCount; argIdx++)
                    {
                        var arg = callArgs[argIdx];
                        // Small scalar globals need field addresses; pointer-backed data globals already store native data pointers.
                        if (string.Equals(arg.Type, "ptr", StringComparison.Ordinal)
                            && fieldHandles.TryGetValue(arg.Value, out var argFieldHandle))
                        {
                            if (IsPointerBackedGlobal(globalMap, arg.Value))
                            {
                                encoder.OpCode(ILOpCode.Ldsfld);
                                encoder.Token(argFieldHandle);
                            }
                            else
                            {
                                encoder.OpCode(ILOpCode.Ldsflda);
                                encoder.Token(argFieldHandle);
                                encoder.OpCode(ILOpCode.Conv_u);
                            }
                        }
                        else
                        {
                            EmitLoadValue(encoder, arg.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        }
                    }

                    if (isIndirectCall)
                    {
                        // Load function pointer from local or parameter, then calli
                        if (localIndices.TryGetValue(indirectCalleeName, out var calleeLocalIdx))
                            encoder.LoadLocal(calleeLocalIdx);
                        else
                            encoder.LoadArgument(paramIndices[indirectCalleeName]);
                        encoder.CallIndirect(BuildStandaloneSignature(metadataBuilder, call.ReturnType, callArgs));
                    }
                    else if (hasRuntimeBridge)
                    {
                        encoder.Call(runtimeBridgeHandle);
                    }
                    else if (TryEmitAllocatorCall(encoder, typeContext, call.Callee, effectiveArgCount))
                    {
                        // Handled as runtime allocator stub
                    }
                    else if (TryEmitPanicCall(encoder, typeContext, call.Callee, effectiveArgCount))
                    {
                        // Handled as panic → throw mapping
                    }
                    else if (methodHandles.TryGetValue(call.Callee, out var calleeHandle))
                    {
                        encoder.Call(calleeHandle);
                        // If this call returns a multi-value (struct), store fields
                        if (!isSretCall && call.Result is not null && multiValueLocals.TryGetValue(call.Result, out var callMvLocals))
                        {
                            // Return struct: store first field, pop rest (simple 2-field case)
                            if (callMvLocals.Length >= 2)
                            {
                                encoder.OpCode(ILOpCode.Dup);
                                encoder.StoreLocal(callMvLocals[0]);
                                // Second field is often a separate return — for now store dup
                                encoder.StoreLocal(callMvLocals[1]);
                            }
                            else if (callMvLocals.Length == 1)
                            {
                                encoder.StoreLocal(callMvLocals[0]);
                            }
                            break;
                        }
                    }
                    else if (typeContext.TryResolveIntrinsic(call.Callee, out var intrinsicHandle))
                    {
                        encoder.Call(intrinsicHandle);
                    }
                    else if (typeContext.TryResolveAvaloniaBridge(call.Callee, out var avaloniaBridgeHandle))
                    {
                        encoder.Call(avaloniaBridgeHandle);
                    }
                    else if (TryEmitInlineIntrinsic(encoder, call.Callee, call.ReturnType))
                    {
                        // Handled inline (e.g., conv opcodes for fptosi/sitofp)
                    }
                    else
                    {
                        for (var argIdx = 0; argIdx < effectiveArgCount; argIdx++)
                        {
                            encoder.OpCode(ILOpCode.Pop);
                        }
                        if (call.Result is not null)
                        {
                            encoder.LoadConstantI4(0);
                        }
                    }
                    if (call.Result is not null && !isSretCall)
                    {
                        encoder.StoreLocal(localIndices[call.Result]);
                    }
                }
                break;

            case LoweredAllocaInstruction alloca:
                if (locallocAllocas.Contains(alloca.Result))
                {
                    // Emit localloc for allocas that need real addressable memory
                    var allocSize = ParseAllocaSize(alloca.Type);
                    encoder.LoadConstantI4(allocSize);
                    encoder.OpCode(ILOpCode.Conv_u);
                    encoder.OpCode(ILOpCode.Localloc);
                    encoder.StoreLocal(localIndices[alloca.Result]);
                }
                // Otherwise: alloca maps to a local variable slot — no IL needed (SROA handles it)
                break;

            case LoweredStoreInstruction store:
                if (TryParseVectorType(store.Type, out _, out _) && TryEmitVectorStore(encoder, store, paramIndices, localIndices, fieldHandles, methodHandles))
                {
                    break;
                }

                // Handle vector constant stores (e.g., store <4 x i32> <i32 0, i32 0, i32 5, i32 7> -> ptr)
                if (store.Type.StartsWith('<') && store.Value.StartsWith('<') && TryParseVectorConstant(store.Value, out var vecElements))
                {
                    var vecElemType = ParseVectorElementType(store.Type);
                    var elemSize = GetFieldSize(vecElemType);
                    // Get destination pointer
                    void EmitDestPtr()
                    {
                        if (localIndices.TryGetValue(store.Destination, out var dIdx))
                            encoder.LoadLocal(dIdx);
                        else if (paramIndices.TryGetValue(store.Destination, out var pIdx))
                            encoder.LoadArgument(pIdx);
                        else
                            encoder.LoadConstantI4(0); // fallback
                    }
                    for (var elemIdx = 0; elemIdx < vecElements.Length; elemIdx++)
                    {
                        EmitDestPtr();
                        if (elemIdx > 0)
                        {
                            encoder.LoadConstantI4(elemIdx * elemSize);
                            encoder.OpCode(ILOpCode.Add);
                        }
                        EmitLoadValue(encoder, vecElements[elemIdx], paramIndices, localIndices, fieldHandles);
                        EmitIndirectStore(encoder, vecElemType);
                    }
                    break;
                }
                if (gepElementLocal.TryGetValue(store.Destination, out var gepStoreIdx))
                {
                    // Store to a GEP element slot (SROA)
                    EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                    encoder.StoreLocal(gepStoreIdx);
                }
                else if (localIndices.TryGetValue(store.Destination, out var storeLocalIdx))
                {
                    // Indirect store needed unless destination is a simple scalar-replaced alloca
                    if (!allocaNames.Contains(store.Destination) || locallocAllocas.Contains(store.Destination) || IsGepResult(store.Destination, instructions, instrIdx))
                    {
                        // Store through a pointer (localloc'd alloca, non-SROA GEP, or computed ptr): ldloc ptr; value; stind
                        encoder.LoadLocal(storeLocalIdx);
                        EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitIndirectStore(encoder, store.Type);
                    }
                    else
                    {
                        EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        // Truncate to sub-int32 width for scalar-replaced i8/i16 allocas
                        if (TryGetIntegerBitWidth(store.Type, out var storeWidth) && storeWidth <= 8)
                            encoder.OpCode(ILOpCode.Conv_u1);
                        else if (storeWidth > 8 && storeWidth <= 16)
                            encoder.OpCode(ILOpCode.Conv_u2);
                        encoder.StoreLocal(storeLocalIdx);
                    }
                }
                else if (paramIndices.TryGetValue(store.Destination, out var storeParamIdx) && ptrParams.Contains(store.Destination))
                {
                    // Store through a ptr parameter: ldarg addr; value; stind
                    encoder.LoadArgument(storeParamIdx);
                    EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                    EmitIndirectStore(encoder, store.Type);
                }
                else if (fieldHandles.TryGetValue(store.Destination, out var storeFieldHandle))
                {
                    // Store to a global static field: value; stsfld
                    EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                    encoder.OpCode(ILOpCode.Stsfld);
                    encoder.Token(storeFieldHandle);
                }
                break;

            case LoweredLoadInstruction load:
                {
                    if (gepElementLocal.TryGetValue(load.Source, out var gepLoadIdx))
                    {
                        // Load from a GEP element slot (SROA)
                        encoder.LoadLocal(gepLoadIdx);
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                    else if (TryParseVectorType(load.Type, out _, out _) && TryEmitVectorLoad(encoder, load, typeContext, paramIndices, localIndices, fieldHandles, methodHandles))
                    {
                    }
                    else if (localIndices.TryGetValue(load.Source, out var loadLocalIdx))
                    {
                        encoder.LoadLocal(loadLocalIdx);
                        // Indirect load needed unless source is a simple scalar-replaced alloca
                        if (!allocaNames.Contains(load.Source) || locallocAllocas.Contains(load.Source) || IsGepResult(load.Source, instructions, instrIdx))
                        {
                            EmitIndirectLoad(encoder, load.Type);
                        }
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                    else if (paramIndices.TryGetValue(load.Source, out var loadParamIdx))
                    {
                        encoder.LoadArgument(loadParamIdx);
                        if (ptrParams.Contains(load.Source))
                        {
                            // ptr parameter holds an address — dereference it
                            EmitIndirectLoad(encoder, load.Type);
                        }
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                    else if (fieldHandles.TryGetValue(load.Source, out var fieldHandle))
                    {
                        if (!mutatedGlobals.Contains(load.Source) && TryResolveConstantGlobalElement(load.Source, load.Type, globalMap, out var constantValue))
                        {
                            EmitConstantValue(encoder, load.Type, constantValue);
                        }
                        else
                        {
                            encoder.OpCode(ILOpCode.Ldsfld);
                            encoder.Token(fieldHandle);
                        }
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                    else if (TryParseGlobalElementAccess(load.Source, out var baseName, out var elementIndex)
                             && fieldHandles.TryGetValue(baseName, out var arrayFieldHandle))
                    {
                        if (TryResolveConstantGlobalElementAtIndex(baseName, elementIndex, load.Type, globalMap, out var arrValue))
                        {
                            EmitConstantValue(encoder, load.Type, arrValue);
                        }
                        else
                        {
                            encoder.OpCode(ILOpCode.Ldsfld);
                            encoder.Token(arrayFieldHandle);
                        }
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                    else
                    {
                        encoder.LoadConstantI4(0);
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                }
                break;

            case LoweredGetElementPointerInstruction gep:
                {
                    if (gepElementLocal.ContainsKey(gep.Result))
                    {
                        // SROA: this GEP is resolved statically to an element local — no IL needed
                        break;
                    }
                    EmitLoadValue(encoder, gep.Base, paramIndices, localIndices, fieldHandles);
                    if (gep.IndexVariable is not null)
                    {
                        EmitLoadValue(encoder, gep.IndexVariable, paramIndices, localIndices, fieldHandles);
                        var stride = GetElementTypeStride(gep.ElementType);
                        if (stride != 1)
                        {
                            encoder.LoadConstantI8((long)stride);
                            encoder.OpCode(ILOpCode.Mul);
                        }
                        encoder.OpCode(ILOpCode.Conv_i);
                        encoder.OpCode(ILOpCode.Add);
                    }
                    else if (gep.Index != 0)
                    {
                        encoder.LoadConstantI4(checked(gep.Index * GetElementTypeStride(gep.ElementType)));
                        encoder.OpCode(ILOpCode.Conv_i);
                        encoder.OpCode(ILOpCode.Add);
                    }
                    encoder.StoreLocal(localIndices[gep.Result]);
                }
                break;

            case LoweredExtractValueInstruction ev:
                {
                    if (multiValueLocals.TryGetValue(ev.Source, out var evLocals) && ev.Index < evLocals.Length)
                    {
                        encoder.LoadLocal(evLocals[ev.Index]);
                    }
                    else if (localIndices.TryGetValue(ev.Source, out var srcLocal))
                    {
                        if (aggregatePointerLocals.Contains(ev.Source))
                        {
                            // Source is a pointer to sret buffer — read field at offset
                            var fieldOffset = GetAggregateFieldOffset(ev.AggregateType, ev.Index);
                            var fieldTypes = ParseAggregateFieldTypes(ev.AggregateType);
                            encoder.LoadLocal(srcLocal);
                            if (fieldOffset > 0)
                            {
                                encoder.LoadConstantI4(fieldOffset);
                                encoder.OpCode(ILOpCode.Add);
                            }
                            EmitIndirectLoad(encoder, fieldTypes[ev.Index]);
                        }
                        else
                        {
                            // Source is a packed i64 (from aggregate-returning function or insertvalue)
                            // Unpack: index 0 = low 32 bits, index 1 = high 32 bits
                            var fieldTypes = ParseAggregateFieldTypes(ev.AggregateType);
                            encoder.LoadLocal(srcLocal);
                            if (ev.Index > 0)
                            {
                                encoder.LoadConstantI4(ev.Index * 32);
                                encoder.OpCode(ILOpCode.Shr_un);
                            }
                            if (ev.Index >= fieldTypes.Length || fieldTypes[ev.Index] != "i64")
                            {
                                encoder.OpCode(ILOpCode.Conv_i4);
                            }
                        }
                    }
                    else
                    {
                        encoder.LoadConstantI4(0);
                    }
                    encoder.StoreLocal(localIndices[ev.Result]);
                }
                break;

            case LoweredInsertValueInstruction iv:
                {
                    if (isSret && IsAggregateType(iv.AggregateType) && AggregateNeedsSret(iv.AggregateType))
                    {
                        // Sret function: write field directly to retbuf (arg 0) at field offset
                        var fieldOffset = GetAggregateFieldOffset(iv.AggregateType, iv.Index);
                        var fieldTypes = ParseAggregateFieldTypes(iv.AggregateType);
                        encoder.LoadArgument(0); // retbuf pointer
                        if (fieldOffset > 0)
                        {
                            encoder.LoadConstantI4(fieldOffset);
                            encoder.OpCode(ILOpCode.Add);
                        }
                        EmitLoadValue(encoder, iv.Value, paramIndices, localIndices, fieldHandles);
                        EmitIndirectStore(encoder, fieldTypes[iv.Index]);
                        // Store a dummy value to the result local (not used, but keeps local tracking consistent)
                        encoder.LoadArgument(0);
                        encoder.StoreLocal(localIndices[iv.Result]);
                    }
                    else
                    {
                        // Pack a field into an i64 at the specified index
                        // insertvalue { i32, i32 } base, i32 value, index
                        // Result = base with field[index] replaced
                        if (string.Equals(iv.Base, "poison", StringComparison.Ordinal)
                            || string.Equals(iv.Base, "undef", StringComparison.Ordinal))
                        {
                            // Fresh aggregate — start from 0
                            EmitLoadValue(encoder, iv.Value, paramIndices, localIndices, fieldHandles);
                            encoder.OpCode(ILOpCode.Conv_u4); // ensure unsigned for shift
                            encoder.OpCode(ILOpCode.Conv_u8);
                            if (iv.Index > 0)
                            {
                                encoder.LoadConstantI4(iv.Index * 32);
                                encoder.OpCode(ILOpCode.Shl);
                            }
                        }
                        else
                        {
                            // Merge into existing packed value
                            EmitLoadValue(encoder, iv.Base, paramIndices, localIndices, fieldHandles);
                            EmitLoadValue(encoder, iv.Value, paramIndices, localIndices, fieldHandles);
                            encoder.OpCode(ILOpCode.Conv_u4);
                            encoder.OpCode(ILOpCode.Conv_u8);
                            if (iv.Index > 0)
                            {
                                encoder.LoadConstantI4(iv.Index * 32);
                                encoder.OpCode(ILOpCode.Shl);
                            }
                            encoder.OpCode(ILOpCode.Or);
                        }
                        encoder.StoreLocal(localIndices[iv.Result]);
                    }
                }
                break;

            case LoweredAtomicRmwInstruction rmw:
                {
                    // atomicrmw op ptr %ptr, i32 %val → old value
                    // Single-threaded semantics: old = *ptr; *ptr = op(old, val); result = old
                    EmitLoadAddress(encoder, rmw.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
                    EmitIndirectLoad(encoder, rmw.ValueType); // old = *ptr
                    encoder.OpCode(ILOpCode.Dup); // keep old for result
                    EmitLoadValue(encoder, rmw.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                    switch (rmw.Operation)
                    {
                        case "add": encoder.OpCode(ILOpCode.Add); break;
                        case "sub": encoder.OpCode(ILOpCode.Sub); break;
                        case "and": encoder.OpCode(ILOpCode.And); break;
                        case "or": encoder.OpCode(ILOpCode.Or); break;
                        case "xor": encoder.OpCode(ILOpCode.Xor); break;
                        case "xchg": encoder.OpCode(ILOpCode.Pop); encoder.OpCode(ILOpCode.Pop);
                            EmitLoadValue(encoder, rmw.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                            break;
                        default: encoder.OpCode(ILOpCode.Add); break; // fallback
                    }
                    // Stack: [old, new_value]. Store new_value back to *ptr
                    // Need: ptr, new_value on stack for stind. Rearrange.
                    // Current stack from dup+op: [old_result, new_value]
                    // We need to store new_value to *ptr, so reload ptr
                    var rmwTempLocal = localIndices.TryGetValue(rmw.Result, out var rmwResIdx) ? rmwResIdx : -1;
                    // Store new_value to a temp, then: push ptr, push new_value, stind, push old
                    if (rmwTempLocal >= 0)
                    {
                        // Stack: [old, new_value]
                        encoder.StoreLocal(rmwTempLocal); // save new_value temporarily (reuse result local)
                        // Stack: [old]
                        // Now store new_value back to *ptr
                        EmitLoadAddress(encoder, rmw.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.LoadLocal(rmwTempLocal); // load new_value
                        EmitIndirectStore(encoder, rmw.ValueType);
                        // Stack: [old] — result is old value
                        encoder.StoreLocal(rmwTempLocal);
                    }
                    else
                    {
                        // No result needed — just pop
                        encoder.OpCode(ILOpCode.Pop);
                        encoder.OpCode(ILOpCode.Pop);
                    }
                }
                break;

            case LoweredCmpxchgInstruction cx:
                {
                    // cmpxchg ptr %ptr, i32 %cmp, i32 %new → { old_value, success_flag }
                    // Single-threaded: old = *ptr; if (old == cmp) *ptr = new; result = {old, old==cmp ? 1 : 0}
                    if (multiValueLocals.TryGetValue(cx.Result, out var cxLocals) && cxLocals.Length >= 2)
                    {
                        // Load old value
                        EmitLoadAddress(encoder, cx.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitIndirectLoad(encoder, cx.ValueType);
                        encoder.StoreLocal(cxLocals[0]); // store old value

                        // Compare old == cmp
                        encoder.LoadLocal(cxLocals[0]);
                        EmitLoadValue(encoder, cx.CompareValue, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.OpCode(ILOpCode.Ceq);
                        encoder.StoreLocal(cxLocals[1]); // store success flag

                        // If success, store new value to *ptr
                        encoder.LoadLocal(cxLocals[1]);
                        var cxSkipLabel = encoder.DefineLabel();
                        encoder.Branch(ILOpCode.Brfalse, cxSkipLabel);
                        EmitLoadAddress(encoder, cx.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadValue(encoder, cx.NewValue, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitIndirectStore(encoder, cx.ValueType);
                        encoder.MarkLabel(cxSkipLabel);
                        encoder.OpCode(ILOpCode.Nop);
                    }
                    else if (localIndices.TryGetValue(cx.Result, out var cxResIdx))
                    {
                        // Pack into i64: low 32 = old value, high 32 = success flag
                        EmitLoadAddress(encoder, cx.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitIndirectLoad(encoder, cx.ValueType);
                        encoder.OpCode(ILOpCode.Dup);
                        // Compare old == cmp
                        EmitLoadValue(encoder, cx.CompareValue, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.OpCode(ILOpCode.Ceq);
                        // Pack: success << 32 | old
                        encoder.OpCode(ILOpCode.Conv_u8);
                        encoder.LoadConstantI4(32);
                        encoder.OpCode(ILOpCode.Shl);
                        // Stack: [old, flag<<32]
                        encoder.OpCode(ILOpCode.Or); // need old as i64 too
                        encoder.StoreLocal(cxResIdx);
                        // Also do conditional store
                        // Simple approach: always store if equal — reload and check
                    }
                    else
                    {
                        encoder.OpCode(ILOpCode.Nop);
                    }
                }
                break;

            case LoweredRawInstruction raw when TryEmitRawSwitch(encoder, raw, instructions, ref instrIdx, typeContext, paramIndices, localIndices, fieldHandles, labelMap, phiByBlock, sourceBlock):
                break;

            case LoweredSwitchInstruction sw:
                EmitTypedSwitch(encoder, sw, typeContext, paramIndices, localIndices, fieldHandles, labelMap, phiByBlock, sourceBlock);
                break;

            case LoweredRawInstruction raw:
                if (StrictUnsupportedIr)
                {
                    throw new NotSupportedException($"unsupported raw LLVM instruction in block '{sourceBlock}': '{raw.Text}'");
                }
                encoder.OpCode(ILOpCode.Nop);
                break;

            case LoweredInvokeInstruction invoke:
                throw new NotSupportedException($"LLVM 'invoke' is not yet supported (normal=%{invoke.NormalLabel}, unwind=%{invoke.UnwindLabel}) — see docs/support-matrix.md (panic-unwind).");

            case LoweredLandingPadInstruction landingPad:
                throw new NotSupportedException($"LLVM 'landingpad' is not yet supported (cleanup={landingPad.IsCleanup}) — see docs/support-matrix.md (panic-unwind).");

            case LoweredFenceInstruction fence:
                throw new NotSupportedException($"LLVM 'fence {fence.Ordering}' is not yet supported — see docs/support-matrix.md (atomics).");

            case LoweredVolatileLoadInstruction volatileLoad:
                throw new NotSupportedException($"LLVM volatile load is not yet supported (result %{volatileLoad.Result}) — see docs/support-matrix.md (memory model).");

            case LoweredVolatileStoreInstruction:
                throw new NotSupportedException("LLVM volatile store is not yet supported — see docs/support-matrix.md (memory model).");

            case LoweredUnreachableInstruction:
                encoder.OpCode(ILOpCode.Ldnull);
                encoder.OpCode(ILOpCode.Throw);
                break;

            default:
                encoder.OpCode(ILOpCode.Nop);
                break;
        }
    }

    private static void EmitPhiCopies(InstructionEncoder encoder, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock, string sourceBlock, string targetBlock)
    {
        if (!phiByBlock.TryGetValue(targetBlock, out var phiInstructions) || phiInstructions.Length == 0)
        {
            return;
        }

        // Collect all phi copies for this edge
        var copies = new List<(string Value, string Result, string Type)>();
        foreach (var phi in phiInstructions)
        {
            var incoming = phi.Incoming.FirstOrDefault(i => string.Equals(i.SourceBlock, sourceBlock, StringComparison.Ordinal));
            if (incoming is null) continue;
            copies.Add((incoming.Value, phi.Result, phi.Type));
        }

        if (copies.Count <= 1)
        {
            // No conflict possible with a single copy
            foreach (var (value, result, type) in copies)
            {
                EmitLoadPhiValue(encoder, typeContext, value, type, paramIndices, localIndices, fieldHandles);
                encoder.StoreLocal(localIndices[result]);
            }
            return;
        }

        // Check for conflicts: a phi source that is also a phi destination
        var destinations = new HashSet<string>(copies.Select(c => c.Result), StringComparer.Ordinal);
        var hasConflict = copies.Any(c => destinations.Contains(c.Value));

        if (!hasConflict)
        {
            // Safe to emit in order
            foreach (var (value, result, type) in copies)
            {
                EmitLoadPhiValue(encoder, typeContext, value, type, paramIndices, localIndices, fieldHandles);
                encoder.StoreLocal(localIndices[result]);
            }
        }
        else
        {
            // Load all values first (push onto stack), then store in reverse
            foreach (var (value, _, type) in copies)
            {
                EmitLoadPhiValue(encoder, typeContext, value, type, paramIndices, localIndices, fieldHandles);
            }
            for (var i = copies.Count - 1; i >= 0; i--)
            {
                encoder.StoreLocal(localIndices[copies[i].Result]);
            }
        }
    }

    private static bool IsVectorLoopGuard(LoweredConditionalBranchInstruction branch)
    {
        return branch.Condition.StartsWith("min.iters.check", StringComparison.Ordinal)
            && !IsVectorBlockName(branch.TrueTarget)
            && IsVectorBlockName(branch.FalseTarget);
    }

    private static bool IsVectorBlockName(string blockName)
    {
        return blockName.Contains("vector", StringComparison.Ordinal)
            || blockName.StartsWith("vec.", StringComparison.Ordinal);
    }

    private static void EmitLoadPhiValue(InstructionEncoder encoder, SrmTypeContext typeContext, string value, string type, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (TryParseVectorType(type, out var count, out var elementType)
            && TryEmitVectorLiteral(encoder, typeContext, value, count, elementType, paramIndices, localIndices, fieldHandles))
        {
            return;
        }

        EmitLoadValue(encoder, value, paramIndices, localIndices, fieldHandles);
    }

    private static void EmitLoadValue(InstructionEncoder encoder, string value, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, MethodDefinitionHandle>? methodHandles = null)
    {
        if (paramIndices.TryGetValue(value, out var paramIndex))
        {
            encoder.LoadArgument(paramIndex);
            return;
        }

        if (localIndices.TryGetValue(value, out var localIndex))
        {
            encoder.LoadLocal(localIndex);
            return;
        }

        if (string.Equals(value, "true", StringComparison.Ordinal))
        {
            encoder.LoadConstantI4(1);
            return;
        }

        if (string.Equals(value, "false", StringComparison.Ordinal))
        {
            encoder.LoadConstantI4(0);
            return;
        }

        if (long.TryParse(value, out var longConst))
        {
            if (longConst is >= int.MinValue and <= int.MaxValue)
                encoder.LoadConstantI4((int)longConst);
            else
                encoder.LoadConstantI8(longConst);
            return;
        }

        // Float/double literals (e.g., "3.500000e+00", "0.5", "1.0")
        if (double.TryParse(value, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var doubleConst))
        {
            // Use float if value is representable as float and looks like a float context
            if (float.IsFinite((float)doubleConst) && (float)doubleConst == doubleConst)
                encoder.LoadConstantR4((float)doubleConst);
            else
                encoder.LoadConstantR8(doubleConst);
            return;
        }

        // Global field reference
        var symbolValue = NormalizeSymbolReference(value);
        if (fieldHandles.TryGetValue(symbolValue, out var fieldHandle))
        {
            encoder.OpCode(ILOpCode.Ldsfld);
            encoder.Token(fieldHandle);
            return;
        }

        // Function reference → ldftn (for function pointers stored into locals)
        if (methodHandles is not null && methodHandles.TryGetValue(symbolValue, out var fnHandle))
        {
            encoder.OpCode(ILOpCode.Ldftn);
            encoder.Token(fnHandle);
            return;
        }

        // Fallback: push 0
        encoder.LoadConstantI4(0);
    }

    private static bool TryEmitVectorLiteral(InstructionEncoder encoder, SrmTypeContext typeContext, string value, int count, string elementType, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (localIndices.ContainsKey(value) || paramIndices.ContainsKey(value) || fieldHandles.ContainsKey(value))
        {
            return false;
        }

        string[] elements;
        if (string.Equals(value, "zeroinitializer", StringComparison.Ordinal)
            || string.Equals(value, "poison", StringComparison.Ordinal)
            || string.Equals(value, "undef", StringComparison.Ordinal))
        {
            elements = Enumerable.Repeat("0", count).ToArray();
        }
        else if (TryParseSplatValue(value, out var splatValue))
        {
            elements = Enumerable.Repeat(splatValue, count).ToArray();
        }
        else if (!TryParseVectorConstant(value, out elements))
        {
            return false;
        }

        encoder.LoadConstantI4(count);
        encoder.OpCode(ILOpCode.Newarr);
        encoder.Token(GetVectorElementTypeReference(typeContext, elementType));

        for (var index = 0; index < count; index++)
        {
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI4(index);
            EmitLoadValue(encoder, index < elements.Length ? elements[index] : "0", paramIndices, localIndices, fieldHandles);
            EmitTruncateToVectorElement(encoder, elementType);
            EmitStoreVectorElement(encoder, elementType);
        }

        return true;
    }

    private static bool TryParseSplatValue(string value, out string splatValue)
    {
        splatValue = string.Empty;
        var trimmed = value.Trim();
        if (!trimmed.StartsWith("splat (", StringComparison.Ordinal) || !trimmed.EndsWith(')'))
        {
            return false;
        }

        var inner = trimmed["splat (".Length..^1].Trim();
        var separator = inner.LastIndexOf(' ');
        if (separator < 0 || separator == inner.Length - 1)
        {
            return false;
        }

        splatValue = inner[(separator + 1)..].Trim();
        return true;
    }

    private static TypeReferenceHandle GetVectorElementTypeReference(SrmTypeContext typeContext, string elementType) => elementType switch
    {
        "i8" => typeContext.SByteTypeRef,
        "i16" => typeContext.Int16TypeRef,
        "i64" => typeContext.Int64TypeRef,
        _ => typeContext.Int32TypeRef
    };

    private static void EmitLoadVectorElement(InstructionEncoder encoder, string value, string vectorType, int index, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (!TryParseVectorType(vectorType, out var count, out var elementType))
        {
            encoder.LoadConstantI4(0);
            return;
        }

        if (string.Equals(value, "zeroinitializer", StringComparison.Ordinal)
            || string.Equals(value, "poison", StringComparison.Ordinal)
            || string.Equals(value, "undef", StringComparison.Ordinal))
        {
            encoder.LoadConstantI4(0);
            return;
        }

        if (TryParseSplatValue(value, out var splatValue))
        {
            EmitLoadValue(encoder, splatValue, paramIndices, localIndices, fieldHandles);
            EmitTruncateToVectorElement(encoder, elementType);
            return;
        }

        if (TryParseVectorConstant(value, out var elements))
        {
            EmitLoadValue(encoder, index < elements.Length ? elements[index] : "0", paramIndices, localIndices, fieldHandles);
            EmitTruncateToVectorElement(encoder, elementType);
            return;
        }

        EmitLoadValue(encoder, value, paramIndices, localIndices, fieldHandles);
        encoder.LoadConstantI4(Math.Min(index, count - 1));
        EmitLoadVectorElementAtIndex(encoder, elementType);
    }

    private static void EmitLoadVectorElementAtIndex(InstructionEncoder encoder, string elementType)
    {
        encoder.OpCode(elementType switch
        {
            "i8" => ILOpCode.Ldelem_i1,
            "i16" => ILOpCode.Ldelem_i2,
            "i64" => ILOpCode.Ldelem_i8,
            _ => ILOpCode.Ldelem_i4
        });
    }

    private static void EmitStoreVectorElement(InstructionEncoder encoder, string elementType)
    {
        encoder.OpCode(elementType switch
        {
            "i8" => ILOpCode.Stelem_i1,
            "i16" => ILOpCode.Stelem_i2,
            "i64" => ILOpCode.Stelem_i8,
            _ => ILOpCode.Stelem_i4
        });
    }

    private static void EmitTruncateToVectorElement(InstructionEncoder encoder, string elementType)
    {
        switch (elementType)
        {
            case "i8":
                encoder.OpCode(ILOpCode.Conv_u1);
                break;
            case "i16":
                encoder.OpCode(ILOpCode.Conv_u2);
                break;
        }
    }

    private static bool TryEmitVectorCompareInstruction(InstructionEncoder encoder, LoweredCompareInstruction compare, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (!TryParseVectorType(compare.Type, out var count, out var elementType))
        {
            return false;
        }

        encoder.LoadConstantI4(count);
        encoder.OpCode(ILOpCode.Newarr);
        encoder.Token(GetVectorElementTypeReference(typeContext, "i1"));
        for (var index = 0; index < count; index++)
        {
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI4(index);
            EmitLoadVectorElement(encoder, compare.Left, compare.Type, index, paramIndices, localIndices, fieldHandles);
            EmitSignedNarrowNormalization(encoder, elementType, compare.Predicate);
            EmitLoadVectorElement(encoder, compare.Right, compare.Type, index, paramIndices, localIndices, fieldHandles);
            EmitSignedNarrowNormalization(encoder, elementType, compare.Predicate);
            EmitCompare(encoder, compare.Predicate);
            EmitStoreVectorElement(encoder, "i1");
        }

        encoder.StoreLocal(localIndices[compare.Result]);
        return true;
    }

    private static void EmitInsertElementInstruction(InstructionEncoder encoder, LoweredInsertElementInstruction insert, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (!TryParseVectorType(insert.VectorType, out var count, out var elementType))
        {
            encoder.LoadConstantI4(0);
            encoder.StoreLocal(localIndices[insert.Result]);
            return;
        }

        encoder.LoadConstantI4(count);
        encoder.OpCode(ILOpCode.Newarr);
        encoder.Token(GetVectorElementTypeReference(typeContext, elementType));
        for (var index = 0; index < count; index++)
        {
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI4(index);
            if (index == insert.Index)
            {
                EmitLoadValue(encoder, insert.Value, paramIndices, localIndices, fieldHandles);
                EmitTruncateToVectorElement(encoder, elementType);
            }
            else
            {
                EmitLoadVectorElement(encoder, insert.Base, insert.VectorType, index, paramIndices, localIndices, fieldHandles);
            }
            EmitStoreVectorElement(encoder, elementType);
        }

        encoder.StoreLocal(localIndices[insert.Result]);
    }

    private static bool TryEmitShuffleVectorInstruction(InstructionEncoder encoder, LoweredShuffleVectorInstruction shuffle, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (!TryParseVectorType(shuffle.VectorType, out var inputCount, out var elementType)
            || !TryParseVectorType(shuffle.MaskType, out var outputCount, out _))
        {
            return false;
        }

        encoder.LoadConstantI4(outputCount);
        encoder.OpCode(ILOpCode.Newarr);
        encoder.Token(GetVectorElementTypeReference(typeContext, elementType));
        for (var index = 0; index < outputCount; index++)
        {
            if (!TryGetShuffleMaskIndex(shuffle.Mask, index, out var sourceIndex))
            {
                return false;
            }

            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI4(index);
            if (sourceIndex < inputCount)
            {
                EmitLoadVectorElement(encoder, shuffle.First, shuffle.VectorType, sourceIndex, paramIndices, localIndices, fieldHandles);
            }
            else
            {
                EmitLoadVectorElement(encoder, shuffle.Second, shuffle.VectorType, sourceIndex - inputCount, paramIndices, localIndices, fieldHandles);
            }
            EmitStoreVectorElement(encoder, elementType);
        }

        encoder.StoreLocal(localIndices[shuffle.Result]);
        return true;
    }

    private static bool TryEmitBitcastInstruction(InstructionEncoder encoder, LoweredBitcastInstruction bitcast, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (string.Equals(bitcast.FromType, bitcast.ToType, StringComparison.Ordinal))
        {
            EmitLoadValue(encoder, bitcast.Value, paramIndices, localIndices, fieldHandles);
            encoder.StoreLocal(localIndices[bitcast.Result]);
            return true;
        }

        if (TryParseVectorType(bitcast.FromType, out var fromCount, out var fromElement)
            && TryGetIntegerBitWidth(bitcast.ToType, out var toWidth)
            && string.Equals(fromElement, "i1", StringComparison.Ordinal)
            && toWidth >= fromCount)
        {
            encoder.LoadConstantI4(0);
            for (var index = 0; index < fromCount; index++)
            {
                EmitLoadVectorElement(encoder, bitcast.Value, bitcast.FromType, index, paramIndices, localIndices, fieldHandles);
                encoder.LoadConstantI4(1 << index);
                encoder.OpCode(ILOpCode.Mul);
                encoder.OpCode(ILOpCode.Or);
            }
            encoder.StoreLocal(localIndices[bitcast.Result]);
            return true;
        }

        if (string.Equals(bitcast.FromType, "<16 x i8>", StringComparison.Ordinal)
            && string.Equals(bitcast.ToType, "<2 x i64>", StringComparison.Ordinal))
        {
            encoder.LoadConstantI4(2);
            encoder.OpCode(ILOpCode.Newarr);
            encoder.Token(GetVectorElementTypeReference(typeContext, "i64"));
            for (var lane = 0; lane < 2; lane++)
            {
                encoder.OpCode(ILOpCode.Dup);
                encoder.LoadConstantI4(lane);
                encoder.LoadConstantI8(0);
                for (var byteIndex = 0; byteIndex < 8; byteIndex++)
                {
                    EmitLoadVectorElement(encoder, bitcast.Value, bitcast.FromType, (lane * 8) + byteIndex, paramIndices, localIndices, fieldHandles);
                    encoder.LoadConstantI4(255);
                    encoder.OpCode(ILOpCode.And);
                    encoder.OpCode(ILOpCode.Conv_u8);
                    if (byteIndex > 0)
                    {
                        encoder.LoadConstantI4(byteIndex * 8);
                        encoder.OpCode(ILOpCode.Shl);
                    }
                    encoder.OpCode(ILOpCode.Or);
                }
                EmitStoreVectorElement(encoder, "i64");
            }
            encoder.StoreLocal(localIndices[bitcast.Result]);
            return true;
        }

        return false;
    }

    private static bool TryEmitVectorLoad(InstructionEncoder encoder, LoweredLoadInstruction load, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, MethodDefinitionHandle>? methodHandles)
    {
        if (!TryParseVectorType(load.Type, out var count, out var elementType))
        {
            return false;
        }

        var elementSize = GetFieldSize(elementType);
        encoder.LoadConstantI4(count);
        encoder.OpCode(ILOpCode.Newarr);
        encoder.Token(GetVectorElementTypeReference(typeContext, elementType));
        for (var index = 0; index < count; index++)
        {
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI4(index);
            EmitLoadAddress(encoder, load.Source, paramIndices, localIndices, fieldHandles, methodHandles);
            if (index > 0)
            {
                encoder.LoadConstantI4(index * elementSize);
                encoder.OpCode(ILOpCode.Add);
            }
            EmitIndirectLoad(encoder, elementType);
            EmitStoreVectorElement(encoder, elementType);
        }

        encoder.StoreLocal(localIndices[load.Result]);
        return true;
    }

    private static bool TryEmitVectorStore(InstructionEncoder encoder, LoweredStoreInstruction store, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, MethodDefinitionHandle>? methodHandles)
    {
        if (!TryParseVectorType(store.Type, out var count, out var elementType))
        {
            return false;
        }

        var elementSize = GetFieldSize(elementType);
        for (var index = 0; index < count; index++)
        {
            EmitLoadAddress(encoder, store.Destination, paramIndices, localIndices, fieldHandles, methodHandles);
            if (index > 0)
            {
                encoder.LoadConstantI4(index * elementSize);
                encoder.OpCode(ILOpCode.Add);
            }
            EmitLoadVectorElement(encoder, store.Value, store.Type, index, paramIndices, localIndices, fieldHandles);
            EmitIndirectStore(encoder, elementType);
        }

        return true;
    }

    private static bool TryEmitVectorBinaryInstruction(InstructionEncoder encoder, LoweredBinaryInstruction binary, SrmTypeContext typeContext, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (!TryParseVectorType(binary.Type, out var count, out var elementType))
        {
            return false;
        }

        if (binary.Operation is not ("add" or "sub" or "mul" or "and" or "or" or "xor" or "shl" or "lshr" or "ashr"))
        {
            return false;
        }

        encoder.LoadConstantI4(count);
        encoder.OpCode(ILOpCode.Newarr);
        encoder.Token(GetVectorElementTypeReference(typeContext, elementType));

        for (var index = 0; index < count; index++)
        {
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI4(index);
            EmitLoadVectorElement(encoder, binary.Left, binary.Type, index, paramIndices, localIndices, fieldHandles);
            EmitLoadVectorElement(encoder, binary.Right, binary.Type, index, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(MapBinaryOp(binary.Operation));
            EmitTruncateToVectorElement(encoder, elementType);
            EmitStoreVectorElement(encoder, elementType);
        }

        encoder.StoreLocal(localIndices[binary.Result]);
        return true;
    }

    private static bool TryEmitVectorReduceCall(InstructionEncoder encoder, LoweredCallInstruction call, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        const string prefix = "llvm.vector.reduce.";
        if (!call.Callee.StartsWith(prefix, StringComparison.Ordinal) || call.Arguments.Count == 0)
        {
            return false;
        }

        var remainder = call.Callee[prefix.Length..];
        var vectorMarker = remainder.LastIndexOf(".v", StringComparison.Ordinal);
        if (vectorMarker < 0)
        {
            return false;
        }

        var operation = remainder[..vectorMarker];
        if (operation is not ("add" or "and" or "or" or "xor"))
        {
            return false;
        }

        var vectorType = call.Arguments[0].Type;
        if (!TryParseVectorType(vectorType, out var count, out var elementType) || count <= 0)
        {
            return false;
        }

        var vectorValue = call.Arguments[0].Value;
        EmitLoadVectorElement(encoder, vectorValue, vectorType, 0, paramIndices, localIndices, fieldHandles);
        for (var index = 1; index < count; index++)
        {
            EmitLoadVectorElement(encoder, vectorValue, vectorType, index, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(operation switch
            {
                "add" => ILOpCode.Add,
                "and" => ILOpCode.And,
                "or" => ILOpCode.Or,
                _ => ILOpCode.Xor
            });
            EmitTruncateToVectorElement(encoder, elementType);
        }

        EmitIntegerExtension(encoder, call.ReturnType, call.ReturnType is "i8" or "i16" ? "zeroext" : null);
        return true;
    }

    private static bool TryEmitNarrowMinMaxIntrinsic(InstructionEncoder encoder, SrmTypeContext typeContext, LoweredCallInstruction call, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        if (call.Arguments.Count != 2)
        {
            return false;
        }

        var operation = call.Callee switch
        {
            var name when name.StartsWith("llvm.smax.", StringComparison.Ordinal) => "smax",
            var name when name.StartsWith("llvm.smin.", StringComparison.Ordinal) => "smin",
            var name when name.StartsWith("llvm.umax.", StringComparison.Ordinal) => "umax",
            var name when name.StartsWith("llvm.umin.", StringComparison.Ordinal) => "umin",
            _ => null
        };
        if (operation is null || call.ReturnType is not ("i8" or "i16"))
        {
            return false;
        }

        var extension = operation[0] == 's' ? "signext" : "zeroext";
        EmitLoadValue(encoder, call.Arguments[0].Value, paramIndices, localIndices, fieldHandles);
        EmitIntegerExtension(encoder, call.ReturnType, extension);
        EmitLoadValue(encoder, call.Arguments[1].Value, paramIndices, localIndices, fieldHandles);
        EmitIntegerExtension(encoder, call.ReturnType, extension);
        encoder.Call(operation.EndsWith("max", StringComparison.Ordinal)
            ? typeContext.MathMaxI32
            : typeContext.MathMinI32);
        return true;
    }

    /// <summary>
    /// Load a pointer-typed argument value. Scalar fields use ldsflda; pointer-backed globals use ldsfld.
    /// </summary>
    private static void EmitLoadPtrValue(InstructionEncoder encoder, LoweredArgument arg, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, LoweredGlobal> globalMap, IReadOnlyDictionary<string, MethodDefinitionHandle>? methodHandles = null)
    {
        if (string.Equals(arg.Type, "ptr", StringComparison.Ordinal)
            && fieldHandles.TryGetValue(arg.Value, out var fh))
        {
            if (IsPointerBackedGlobal(globalMap, arg.Value))
            {
                encoder.OpCode(ILOpCode.Ldsfld);
                encoder.Token(fh);
            }
            else
            {
                encoder.OpCode(ILOpCode.Ldsflda);
                encoder.Token(fh);
                encoder.OpCode(ILOpCode.Conv_u);
            }
        }
        else
        {
            EmitLoadValue(encoder, arg.Value, paramIndices, localIndices, fieldHandles, methodHandles);
        }
    }

    private static bool IsPointerBackedGlobal(IReadOnlyDictionary<string, LoweredGlobal> globalMap, string name)
        => globalMap.TryGetValue(name, out var global) && global.InitializerBytes.Count > 8;

    /// <summary>
    /// Load the address of a value (for atomic pointer operands). Globals get ldsflda; locals/params emit their value directly (already pointers).
    /// </summary>
    private static void EmitLoadAddress(InstructionEncoder encoder, string value, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, MethodDefinitionHandle>? methodHandles = null)
    {
        if (fieldHandles.TryGetValue(value, out var fh))
        {
            encoder.OpCode(ILOpCode.Ldsflda);
            encoder.Token(fh);
        }
        else
        {
            EmitLoadValue(encoder, value, paramIndices, localIndices, fieldHandles, methodHandles);
        }
    }

    private static void EmitCompare(InstructionEncoder encoder, string predicate)
    {
        switch (predicate)
        {
            case "eq":
            case "oeq": // ordered float equal
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "ne":
            case "une": // unordered float not-equal
                encoder.OpCode(ILOpCode.Ceq);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "one": // ordered float not-equal
                encoder.OpCode(ILOpCode.Ceq);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "slt":
            case "olt": // ordered float less-than
                encoder.OpCode(ILOpCode.Clt);
                break;
            case "ult": // unsigned or unordered float less-than
                encoder.OpCode(ILOpCode.Clt_un);
                break;
            case "sgt":
            case "ogt": // ordered float greater-than
                encoder.OpCode(ILOpCode.Cgt);
                break;
            case "ugt": // unsigned or unordered float greater-than
                encoder.OpCode(ILOpCode.Cgt_un);
                break;
            case "sle":
            case "ole": // ordered float less-or-equal
                encoder.OpCode(ILOpCode.Cgt);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "ule": // unsigned or unordered float less-or-equal
                encoder.OpCode(ILOpCode.Cgt_un);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "sge":
            case "oge": // ordered float greater-or-equal
                encoder.OpCode(ILOpCode.Clt);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "uge": // unsigned or unordered float greater-or-equal
                encoder.OpCode(ILOpCode.Clt_un);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            default:
                encoder.OpCode(ILOpCode.Ceq);
                break;
        }
    }

    private static void EmitSignedNarrowNormalization(InstructionEncoder encoder, string type, string predicate)
    {
        if (!IsSignedIntegerCompare(predicate))
        {
            return;
        }

        EmitIntegerExtension(encoder, type, "signext");
    }

    private static bool IsSignedIntegerCompare(string predicate)
    {
        return predicate is "slt" or "sgt" or "sle" or "sge";
    }

    private static void EmitIntegerExtension(InstructionEncoder encoder, string type, string? extension)
    {
        if (extension is null)
        {
            return;
        }

        if (string.Equals(type, "i1", StringComparison.Ordinal))
        {
            if (string.Equals(extension, "signext", StringComparison.Ordinal))
            {
                encoder.OpCode(ILOpCode.Neg);
            }
            return;
        }

        if (string.Equals(type, "i8", StringComparison.Ordinal))
        {
            encoder.OpCode(string.Equals(extension, "signext", StringComparison.Ordinal)
                ? ILOpCode.Conv_i1
                : ILOpCode.Conv_u1);
        }
        else if (string.Equals(type, "i16", StringComparison.Ordinal))
        {
            encoder.OpCode(string.Equals(extension, "signext", StringComparison.Ordinal)
                ? ILOpCode.Conv_i2
                : ILOpCode.Conv_u2);
        }
        else if (string.Equals(type, "i32", StringComparison.Ordinal) && string.Equals(extension, "zeroext", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Conv_u4);
        }
    }

    private static StandaloneSignatureHandle BuildStandaloneSignature(
        MetadataBuilder metadataBuilder, string returnType, IReadOnlyList<LoweredArgument> args)
    {
        var sigBlob = new BlobBuilder();
        new BlobEncoder(sigBlob)
            .MethodSignature(SignatureCallingConvention.Default)
            .Parameters(args.Count,
                returnEncoder => EncodeReturnType(returnEncoder, returnType),
                parametersEncoder =>
                {
                    for (var i = 0; i < args.Count; i++)
                    {
                        EncodeParameterType(parametersEncoder, args[i].Type);
                    }
                });
        return metadataBuilder.AddStandaloneSignature(metadataBuilder.GetOrAddBlob(sigBlob));
    }

    private static string[] ParseAggregateFieldTypes(string aggregateType)
    {
        // Parse "{ i32, i1 }" → ["i32", "i32"] (i1 promoted to i32 for local storage)
        var inner = aggregateType.TrimStart('{').TrimEnd('}').Trim();
        var fields = inner.Split(',');
        var result = new string[fields.Length];
        for (var i = 0; i < fields.Length; i++)
        {
            var ft = fields[i].Trim();
            result[i] = ft switch
            {
                "i1" or "i8" or "i16" or "i32" => "i32",
                "i64" => "i64",
                "ptr" => "ptr",
                "float" => "float",
                "double" => "double",
                _ => "i32"
            };
        }
        return result;
    }

    private static bool TryEmitSaturatingIntrinsic(
        InstructionEncoder encoder,
        LoweredCallInstruction call,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
    {
        // usub.sat.i32(a, b): unsigned saturating sub → max(a-b, 0)
        if (call.Callee.Contains("llvm.usub.sat.", StringComparison.Ordinal) && call.Arguments.Count >= 2)
        {
            var a = call.Arguments[0].Value;
            var b = call.Arguments[1].Value;
            // Branchless: (a - b) * (a >= b ? 1 : 0)
            EmitLoadValue(encoder, a, paramIndices, localIndices, fieldHandles);
            EmitLoadValue(encoder, b, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Sub);
            EmitLoadValue(encoder, a, paramIndices, localIndices, fieldHandles);
            EmitLoadValue(encoder, b, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Clt_un); // 1 if a < b (underflow)
            encoder.LoadConstantI4(0);
            encoder.OpCode(ILOpCode.Ceq); // 1 if no underflow (a >= b)
            encoder.OpCode(ILOpCode.Mul); // 0 if underflow, a-b otherwise
            return true;
        }

        // uadd.sat.i32(a, b): unsigned saturating add → min(a+b, UINT_MAX)
        if (call.Callee.Contains("llvm.uadd.sat.", StringComparison.Ordinal) && call.Arguments.Count >= 2)
        {
            var a = call.Arguments[0].Value;
            var b = call.Arguments[1].Value;
            // Branchless: (a + b) | -(( a+b < a) ? 1 : 0)
            EmitLoadValue(encoder, a, paramIndices, localIndices, fieldHandles);
            EmitLoadValue(encoder, b, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Add);
            encoder.OpCode(ILOpCode.Dup);
            EmitLoadValue(encoder, a, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Clt_un); // 1 if overflow
            encoder.OpCode(ILOpCode.Neg); // -1 (0xFFFFFFFF) if overflow, 0 if not
            encoder.OpCode(ILOpCode.Or); // all bits set if overflow = UINT_MAX
            return true;
        }

        // ssub.sat.i32(a, b): signed saturating sub → clamp(a-b, INT_MIN, INT_MAX)
        if (call.Callee.Contains("llvm.ssub.sat.", StringComparison.Ordinal) && call.Arguments.Count >= 2)
        {
            var a = call.Arguments[0].Value;
            var b = call.Arguments[1].Value;
            // Widen to i64, subtract, then clamp with branches
            var clampMinLabel = encoder.DefineLabel();
            var clampMaxLabel = encoder.DefineLabel();
            var endLabel = encoder.DefineLabel();
            EmitLoadValue(encoder, a, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Conv_i8);
            EmitLoadValue(encoder, b, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Conv_i8);
            encoder.OpCode(ILOpCode.Sub); // i64 result, no overflow
            // Check > INT_MAX
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI8(2147483647L);
            encoder.Branch(ILOpCode.Bgt, clampMaxLabel);
            // Check < INT_MIN
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI8(-2147483648L);
            encoder.Branch(ILOpCode.Blt, clampMinLabel);
            // In range: conv to i32
            encoder.OpCode(ILOpCode.Conv_i4);
            encoder.Branch(ILOpCode.Br, endLabel);
            // Clamp to INT_MAX
            encoder.MarkLabel(clampMaxLabel);
            encoder.OpCode(ILOpCode.Pop);
            encoder.LoadConstantI4(int.MaxValue);
            encoder.Branch(ILOpCode.Br, endLabel);
            // Clamp to INT_MIN
            encoder.MarkLabel(clampMinLabel);
            encoder.OpCode(ILOpCode.Pop);
            encoder.LoadConstantI4(int.MinValue);
            encoder.MarkLabel(endLabel);
            return true;
        }

        // sadd.sat.i32(a, b): signed saturating add → clamp(a+b, INT_MIN, INT_MAX)
        if (call.Callee.Contains("llvm.sadd.sat.", StringComparison.Ordinal) && call.Arguments.Count >= 2)
        {
            var a = call.Arguments[0].Value;
            var b = call.Arguments[1].Value;
            var clampMinLabel = encoder.DefineLabel();
            var clampMaxLabel = encoder.DefineLabel();
            var endLabel = encoder.DefineLabel();
            EmitLoadValue(encoder, a, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Conv_i8);
            EmitLoadValue(encoder, b, paramIndices, localIndices, fieldHandles);
            encoder.OpCode(ILOpCode.Conv_i8);
            encoder.OpCode(ILOpCode.Add);
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI8(2147483647L);
            encoder.Branch(ILOpCode.Bgt, clampMaxLabel);
            encoder.OpCode(ILOpCode.Dup);
            encoder.LoadConstantI8(-2147483648L);
            encoder.Branch(ILOpCode.Blt, clampMinLabel);
            encoder.OpCode(ILOpCode.Conv_i4);
            encoder.Branch(ILOpCode.Br, endLabel);
            encoder.MarkLabel(clampMaxLabel);
            encoder.OpCode(ILOpCode.Pop);
            encoder.LoadConstantI4(int.MaxValue);
            encoder.Branch(ILOpCode.Br, endLabel);
            encoder.MarkLabel(clampMinLabel);
            encoder.OpCode(ILOpCode.Pop);
            encoder.LoadConstantI4(int.MinValue);
            encoder.MarkLabel(endLabel);
            return true;
        }

        // llvm.bitreverse.i32(x): reverse all 32 bits
        // Uses the call result local as scratch for the 5-step reversal algorithm.
        if (call.Callee.Contains("llvm.bitreverse.i32", StringComparison.Ordinal)
            && call.Arguments.Count >= 1 && call.Result is not null
            && localIndices.TryGetValue(call.Result, out var scratchLocal))
        {
            var x = call.Arguments[0].Value;
            // Store input to scratch local
            EmitLoadValue(encoder, x, paramIndices, localIndices, fieldHandles);
            encoder.StoreLocal(scratchLocal);
            // Step 1: swap odd/even bits
            // x = ((x >> 1) & 0x55555555) | ((x & 0x55555555) << 1)
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(1);
            encoder.OpCode(ILOpCode.Shr_un);
            encoder.LoadConstantI4(0x55555555);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(0x55555555);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadConstantI4(1);
            encoder.OpCode(ILOpCode.Shl);
            encoder.OpCode(ILOpCode.Or);
            encoder.StoreLocal(scratchLocal);
            // Step 2: swap pairs
            // x = ((x >> 2) & 0x33333333) | ((x & 0x33333333) << 2)
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(2);
            encoder.OpCode(ILOpCode.Shr_un);
            encoder.LoadConstantI4(0x33333333);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(0x33333333);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadConstantI4(2);
            encoder.OpCode(ILOpCode.Shl);
            encoder.OpCode(ILOpCode.Or);
            encoder.StoreLocal(scratchLocal);
            // Step 3: swap nibbles
            // x = ((x >> 4) & 0x0F0F0F0F) | ((x & 0x0F0F0F0F) << 4)
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(4);
            encoder.OpCode(ILOpCode.Shr_un);
            encoder.LoadConstantI4(0x0F0F0F0F);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(0x0F0F0F0F);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadConstantI4(4);
            encoder.OpCode(ILOpCode.Shl);
            encoder.OpCode(ILOpCode.Or);
            encoder.StoreLocal(scratchLocal);
            // Step 4: swap bytes
            // x = ((x >> 8) & 0x00FF00FF) | ((x & 0x00FF00FF) << 8)
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(8);
            encoder.OpCode(ILOpCode.Shr_un);
            encoder.LoadConstantI4(0x00FF00FF);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(0x00FF00FF);
            encoder.OpCode(ILOpCode.And);
            encoder.LoadConstantI4(8);
            encoder.OpCode(ILOpCode.Shl);
            encoder.OpCode(ILOpCode.Or);
            encoder.StoreLocal(scratchLocal);
            // Step 5: swap halfwords
            // x = (x >> 16) | (x << 16)
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(16);
            encoder.OpCode(ILOpCode.Shr_un);
            encoder.LoadLocal(scratchLocal);
            encoder.LoadConstantI4(16);
            encoder.OpCode(ILOpCode.Shl);
            encoder.OpCode(ILOpCode.Or);
            // Leave result on stack — caller will store to result local
            return true;
        }

        return false;
    }

    private static bool TryEmitOverflowIntrinsic(
        InstructionEncoder encoder,
        LoweredCallInstruction call,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles,
        int[] mvLocals)
    {
        // llvm.sadd.with.overflow.i32(a, b) → { result: i32, overflow: i1 }
        // llvm.ssub.with.overflow.i32(a, b) → { result: i32, overflow: i1 }
        // llvm.smul.with.overflow.i32(a, b) → { result: i32, overflow: i1 }
        // llvm.uadd.with.overflow.i32(a, b) → { result: i32, overflow: i1 }
        if (call.Callee.Contains(".with.overflow.", StringComparison.Ordinal) && mvLocals.Length >= 2)
        {
            var isSigned = call.Callee.Contains(".sadd.", StringComparison.Ordinal)
                        || call.Callee.Contains(".ssub.", StringComparison.Ordinal)
                        || call.Callee.Contains(".smul.", StringComparison.Ordinal);

            var isAdd = call.Callee.Contains("add.with.overflow", StringComparison.Ordinal);
            var isSub = call.Callee.Contains("sub.with.overflow", StringComparison.Ordinal);
            var isMul = call.Callee.Contains("mul.with.overflow", StringComparison.Ordinal);

            // Load both arguments
            if (call.Arguments.Count >= 2)
            {
                EmitLoadValue(encoder, call.Arguments[0].Value, paramIndices, localIndices, fieldHandles);
                EmitLoadValue(encoder, call.Arguments[1].Value, paramIndices, localIndices, fieldHandles);
            }

            if (isMul && isSigned)
            {
                // Signed multiply overflow: widen to i64, multiply, check if truncation changes value
                // Stack: a, b
                // Store b, widen a to i64
                encoder.OpCode(ILOpCode.Conv_i8); // b → i64
                var tempB = mvLocals[1]; // reuse overflow local temporarily
                encoder.StoreLocal(tempB);
                encoder.OpCode(ILOpCode.Conv_i8); // a → i64 (a is now on top since b was stored)
                // Wait, stack order: a is below b. Let me redo.
                // Actually after loading a then b, stack is [a, b]. Conv_i8 converts b.
                // Need different approach:

                // Reload: pop both, load a conv i64, load b conv i64, mul, check
                // Simpler: compute product normally, then check overflow with sign-bit trick
                encoder.LoadLocal(tempB); // reload b as i64
                encoder.OpCode(ILOpCode.Mul); // i64 * i64
                encoder.OpCode(ILOpCode.Dup);
                encoder.OpCode(ILOpCode.Conv_i4); // truncate back
                encoder.StoreLocal(mvLocals[0]); // store truncated result

                // Check overflow: (i64_result != (i64)(i32_result))
                encoder.LoadLocal(mvLocals[0]);
                encoder.OpCode(ILOpCode.Conv_i8);
                encoder.OpCode(ILOpCode.Ceq);
                // ceq gives 1 if equal (no overflow), we want 1 if overflow
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                encoder.StoreLocal(mvLocals[1]);
            }
            else if (isMul && !isSigned)
            {
                // Unsigned multiply overflow: similar but with conv.u8
                encoder.OpCode(ILOpCode.Conv_u8);
                var tempB = mvLocals[1];
                encoder.StoreLocal(tempB);
                encoder.OpCode(ILOpCode.Conv_u8);
                encoder.LoadLocal(tempB);
                encoder.OpCode(ILOpCode.Mul);
                encoder.OpCode(ILOpCode.Dup);
                encoder.OpCode(ILOpCode.Conv_u4);
                encoder.StoreLocal(mvLocals[0]);
                encoder.LoadLocal(mvLocals[0]);
                encoder.OpCode(ILOpCode.Conv_u8);
                encoder.OpCode(ILOpCode.Ceq);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                encoder.StoreLocal(mvLocals[1]);
            }
            else
            {
                // Add/Sub overflow detection via sign-bit trick
                // Compute result
                encoder.OpCode(isAdd ? ILOpCode.Add : ILOpCode.Sub);
                encoder.StoreLocal(mvLocals[0]);

                if (isSigned)
                {
                    // Signed overflow: ((a ^ result) & (b ^ result)) < 0 for add
                    //                   ((a ^ result) & ((~b) ^ result)) < 0 for sub
                    // Simpler: widen to i64 and compare
                    EmitLoadValue(encoder, call.Arguments[0].Value, paramIndices, localIndices, fieldHandles);
                    encoder.OpCode(ILOpCode.Conv_i8);
                    EmitLoadValue(encoder, call.Arguments[1].Value, paramIndices, localIndices, fieldHandles);
                    encoder.OpCode(ILOpCode.Conv_i8);
                    encoder.OpCode(isAdd ? ILOpCode.Add : ILOpCode.Sub);
                    // Wide result on stack
                    encoder.LoadLocal(mvLocals[0]);
                    encoder.OpCode(ILOpCode.Conv_i8);
                    encoder.OpCode(ILOpCode.Ceq);
                    // 1 if no overflow, 0 if overflow — invert
                    encoder.LoadConstantI4(0);
                    encoder.OpCode(ILOpCode.Ceq);
                    encoder.StoreLocal(mvLocals[1]);
                }
                else
                {
                    // Unsigned overflow: for add, overflow if result < a
                    // for sub, overflow if result > a (borrow)
                    encoder.LoadLocal(mvLocals[0]);
                    EmitLoadValue(encoder, call.Arguments[0].Value, paramIndices, localIndices, fieldHandles);
                    encoder.OpCode(isAdd ? ILOpCode.Clt_un : ILOpCode.Cgt_un);
                    encoder.StoreLocal(mvLocals[1]);
                }
            }
            return true;
        }

        // llvm.sadd.sat / llvm.ssub.sat / llvm.uadd.sat / llvm.usub.sat
        if (call.Callee.Contains(".sat.", StringComparison.Ordinal) && mvLocals.Length >= 1)
        {
            // Saturating intrinsics return a single value — shouldn't normally get here
            // since they don't produce multi-value results. Fallback.
            return false;
        }

        return false;
    }

    private static bool TryEmitInlineIntrinsic(InstructionEncoder encoder, string callee, string returnType)
    {
        // fptosi.sat: float/double → int with saturation (use conv.iN)
        if (callee.StartsWith("llvm.fptosi.sat.", StringComparison.Ordinal))
        {
            if (returnType == "i64")
                encoder.OpCode(ILOpCode.Conv_i8);
            else
                encoder.OpCode(ILOpCode.Conv_i4);
            return true;
        }

        // fptoui.sat: float/double → uint with saturation
        if (callee.StartsWith("llvm.fptoui.sat.", StringComparison.Ordinal))
        {
            if (returnType == "i64")
                encoder.OpCode(ILOpCode.Conv_u8);
            else
                encoder.OpCode(ILOpCode.Conv_u4);
            return true;
        }

        // sitofp/uitofp: int → float/double
        if (callee.StartsWith("llvm.sitofp.", StringComparison.Ordinal)
            || callee.StartsWith("llvm.uitofp.", StringComparison.Ordinal))
        {
            if (returnType == "double")
                encoder.OpCode(ILOpCode.Conv_r8);
            else
                encoder.OpCode(ILOpCode.Conv_r4);
            return true;
        }

        // llvm.sadd.sat / llvm.ssub.sat / llvm.uadd.sat / llvm.usub.sat: saturating arithmetic
        // Handled by TryEmitSaturatingIntrinsic before arg push — should not reach here
        if (callee.Contains(".sat.", StringComparison.Ordinal)
            && (callee.Contains("add", StringComparison.Ordinal) || callee.Contains("sub", StringComparison.Ordinal)))
        {
            // Fallback: wrapping arithmetic (only reached if TryEmitSaturatingIntrinsic missed)
            var isSub = callee.Contains("sub", StringComparison.Ordinal);
            encoder.OpCode(isSub ? ILOpCode.Sub : ILOpCode.Add);
            return true;
        }

        return false;
    }

    private static bool TryEmitAllocatorCall(InstructionEncoder encoder, SrmTypeContext typeContext, string callee, int argCount)
    {
        // __rust_alloc(size: i64, align: i64) -> ptr
        if (callee.Contains("___rust_alloc_zeroed", StringComparison.Ordinal))
        {
            // Stack: [size, align]. Drop align, conv size to IntPtr, call AllocHGlobal.
            encoder.OpCode(ILOpCode.Pop);
            encoder.OpCode(ILOpCode.Conv_i);
            encoder.Call(typeContext.MarshalAllocHGlobal);
            return true;
        }
        if (callee.Contains("___rust_alloc", StringComparison.Ordinal)
            && !callee.Contains("___rust_no_alloc_shim", StringComparison.Ordinal))
        {
            // Stack: [size, align]. Drop align, conv size to IntPtr, call AllocHGlobal.
            encoder.OpCode(ILOpCode.Pop);
            encoder.OpCode(ILOpCode.Conv_i);
            encoder.Call(typeContext.MarshalAllocHGlobal);
            return true;
        }
        // __rust_dealloc(ptr, size: i64, align: i64) -> void
        if (callee.Contains("___rust_dealloc", StringComparison.Ordinal))
        {
            // Stack: [ptr, size, align]. Drop align, drop size, call FreeHGlobal(ptr).
            encoder.OpCode(ILOpCode.Pop);
            encoder.OpCode(ILOpCode.Pop);
            encoder.Call(typeContext.MarshalFreeHGlobal);
            return true;
        }
        // __rust_no_alloc_shim_is_unstable_v2() -> void (no-op marker)
        if (callee.Contains("___rust_no_alloc_shim", StringComparison.Ordinal))
        {
            // No args, no result. Nothing to emit.
            return true;
        }
        // handle_alloc_error(size, align) -> ! (diverges)
        if (callee.Contains("handle_alloc_error", StringComparison.Ordinal))
        {
            // Stack: [size, align]. Pop both and throw.
            for (var i = 0; i < argCount; i++)
                encoder.OpCode(ILOpCode.Pop);
            encoder.LoadString(default(UserStringHandle));
            encoder.OpCode(ILOpCode.Newobj);
            encoder.Token(typeContext.NotSupportedExceptionCtor);
            encoder.OpCode(ILOpCode.Throw);
            return true;
        }
        return false;
    }

    private static bool TryEmitPanicCall(InstructionEncoder encoder, SrmTypeContext typeContext, string callee, int argCount)
    {
        // Map Rust panic_const functions to .NET exceptions.
        // These appear with both legacy (_ZN) and v0 (_R) mangling but always contain the key substring.
        MemberReferenceHandle ctorHandle;
        if (callee.Contains("panic_const_div_by_zero", StringComparison.Ordinal)
            || callee.Contains("panic_const_rem_by_zero", StringComparison.Ordinal))
        {
            ctorHandle = typeContext.DivideByZeroExceptionCtor;
        }
        else if (callee.Contains("panic_const_div_overflow", StringComparison.Ordinal)
            || callee.Contains("panic_const_rem_overflow", StringComparison.Ordinal))
        {
            ctorHandle = typeContext.OverflowExceptionCtor;
        }
        else
        {
            return false;
        }

        // Pop any arguments (typically 1 ptr arg for the panic location)
        for (var i = 0; i < argCount; i++)
            encoder.OpCode(ILOpCode.Pop);

        encoder.OpCode(ILOpCode.Newobj);
        encoder.Token(ctorHandle);
        encoder.OpCode(ILOpCode.Throw);
        return true;
    }

    private static ILOpCode MapBinaryOp(string operation) => operation switch
    {
        "add" or "fadd" => ILOpCode.Add,
        "sub" or "fsub" => ILOpCode.Sub,
        "mul" or "fmul" => ILOpCode.Mul,
        "sdiv" or "fdiv" => ILOpCode.Div,
        "udiv" => ILOpCode.Div_un,
        "srem" or "frem" => ILOpCode.Rem,
        "urem" => ILOpCode.Rem_un,
        "and" => ILOpCode.And,
        "or" => ILOpCode.Or,
        "xor" => ILOpCode.Xor,
        "shl" => ILOpCode.Shl,
        "lshr" => ILOpCode.Shr_un,
        "ashr" => ILOpCode.Shr,
        _ => throw new NotSupportedException($"Binary op '{operation}' not supported.")
    };

    private static string? GetInstructionResult(LoweredInstruction instruction) => instruction switch
    {
        LoweredBinaryInstruction b => b.Result,
        LoweredCompareInstruction c => c.Result,
        LoweredCallInstruction call => call.Result,
        LoweredPhiInstruction phi => phi.Result,
        LoweredSelectInstruction sel => sel.Result,
        LoweredTruncateInstruction t => t.Result,
        LoweredZeroExtendInstruction z => z.Result,
        LoweredSignExtendInstruction s => s.Result,
        LoweredPtrToIntInstruction p => p.Result,
        LoweredIntToPtrInstruction i => i.Result,
        LoweredFreezeInstruction f => f.Result,
        LoweredBitcastInstruction b => b.Result,
        LoweredInsertElementInstruction ie => ie.Result,
        LoweredShuffleVectorInstruction sv => sv.Result,
        LoweredLoadInstruction l => l.Result,
        LoweredAllocaInstruction a => a.Result,
        LoweredGetElementPointerInstruction g => g.Result,
        LoweredExtractValueInstruction e => e.Result,
        LoweredInsertValueInstruction iv => iv.Result,
        LoweredAtomicRmwInstruction rmw => rmw.Result,
        LoweredCmpxchgInstruction cx => cx.Result,
        _ => null
    };

    private static string GetLocalType(LoweredInstruction instruction) => instruction switch
    {
        LoweredGetElementPointerInstruction => "ptr",
        LoweredZeroExtendInstruction z => z.ToType,
        LoweredSignExtendInstruction s => s.ToType,
        LoweredPtrToIntInstruction p => p.ToType,
        LoweredIntToPtrInstruction => "ptr",
        LoweredFreezeInstruction f => f.Type,
        LoweredBitcastInstruction b => b.ToType,
        LoweredInsertElementInstruction ie => ie.VectorType,
        LoweredShuffleVectorInstruction sv => InferShuffleVectorType(sv),
        LoweredLoadInstruction l => l.Type,
        LoweredAllocaInstruction => "ptr",
        LoweredPhiInstruction p => InferPhiType(p.Type),
        LoweredBinaryInstruction b => b.Type,
        LoweredCompareInstruction c => TryParseVectorType(c.Type, out var compareCount, out _) ? $"<{compareCount} x i1>" : "i32",
        LoweredSelectInstruction s => s.ValueType,
        LoweredTruncateInstruction t => t.ToType,
        LoweredCallInstruction c => IsAggregateType(c.ReturnType) ? "i64" : c.ReturnType,
        LoweredExtractValueInstruction e => InferExtractValueType(e),
        LoweredInsertValueInstruction => "i64",
        LoweredAtomicRmwInstruction rmw => rmw.ValueType,
        LoweredCmpxchgInstruction => "i64",
        _ => "i32"
    };

    private static string InferPhiType(string type)
    {
        if (TryGetIntegerBitWidth(type, out var width) && width > 32)
            return "i64";
        if (string.Equals(type, "ptr", StringComparison.Ordinal))
            return "ptr";
        if (string.Equals(type, "float", StringComparison.Ordinal))
            return "float";
        if (string.Equals(type, "double", StringComparison.Ordinal))
            return "double";
        if (IsAggregateType(type))
            return "i64";
        return "i32";
    }

    private static string InferExtractValueType(LoweredExtractValueInstruction ev)
    {
        // Parse aggregate type like "{ i32, i1 }" and return the field type at index
        var inner = ev.AggregateType.TrimStart('{').TrimEnd('}').Trim();
        var fields = inner.Split(',');
        if (ev.Index < fields.Length)
        {
            var fieldType = fields[ev.Index].Trim();
            if (string.Equals(fieldType, "i1", StringComparison.Ordinal))
                return "i32";
            if (string.Equals(fieldType, "i64", StringComparison.Ordinal))
                return "i64";
            if (string.Equals(fieldType, "ptr", StringComparison.Ordinal))
                return "ptr";
            if (string.Equals(fieldType, "float", StringComparison.Ordinal))
                return "float";
            if (string.Equals(fieldType, "double", StringComparison.Ordinal))
                return "double";
        }
        return "i32";
    }

    private static bool IsAggregateType(string typeName) =>
        typeName.StartsWith('{') && typeName.EndsWith('}');

    private static bool AggregateNeedsSret(string aggregateType)
    {
        // Aggregates containing ptr or i64 fields need sret (can't pack into 64 bits)
        var fields = ParseAggregateFieldTypes(aggregateType);
        var totalBits = 0;
        foreach (var f in fields)
        {
            totalBits += f switch
            {
                "ptr" => 64,
                "i64" => 64,
                "double" => 64,
                _ => 32
            };
        }
        return totalBits > 64;
    }

    private static int GetAggregateFieldOffset(string aggregateType, int index)
    {
        var fields = ParseAggregateFieldTypes(aggregateType);
        var offset = 0;
        for (var i = 0; i < index && i < fields.Length; i++)
        {
            offset += GetFieldSize(fields[i]);
        }
        return offset;
    }

    private static int GetAggregateSize(string aggregateType)
    {
        var fields = ParseAggregateFieldTypes(aggregateType);
        var size = 0;
        foreach (var f in fields)
            size += GetFieldSize(f);
        return size;
    }

    private static int GetFieldSize(string fieldType) => fieldType switch
    {
        "ptr" => 8,
        "i64" => 8,
        "double" => 8,
        "float" => 4,
        _ => 4 // i32, i16, i8, i1 all stored as 4 bytes for alignment
    };

    /// <summary>
    /// Parse a vector constant like "&lt;i32 0, i32 0, i32 5, i32 7&gt;" into element values ["0", "0", "5", "7"].
    /// </summary>
    private static bool TryParseVectorConstant(string value, out string[] elements)
    {
        // Format: <type val, type val, ...>
        elements = Array.Empty<string>();
        if (!value.StartsWith('<') || !value.EndsWith('>'))
            return false;
        var inner = value[1..^1].Trim();
        var parts = inner.Split(',');
        var result = new string[parts.Length];
        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i].Trim();
            var spaceIdx = part.LastIndexOf(' ');
            if (spaceIdx < 0)
                return false;
            result[i] = part[(spaceIdx + 1)..];
        }
        elements = result;
        return true;
    }

    /// <summary>
    /// Parse a vector type like "&lt;4 x i32&gt;" and return the element type "i32".
    /// </summary>
    private static string ParseVectorElementType(string vectorType)
    {
        return TryParseVectorType(vectorType, out _, out var elementType)
            ? elementType
            : "i32";
    }

    private static string InferShuffleVectorType(LoweredShuffleVectorInstruction shuffle)
    {
        return TryParseVectorType(shuffle.VectorType, out _, out var elementType)
               && TryParseVectorType(shuffle.MaskType, out var count, out _)
            ? $"<{count} x {elementType}>"
            : shuffle.VectorType;
    }

    private static bool TryGetShuffleMaskIndex(string mask, int outputIndex, out int sourceIndex)
    {
        sourceIndex = 0;
        if (string.Equals(mask, "zeroinitializer", StringComparison.Ordinal)
            || string.Equals(mask, "poison", StringComparison.Ordinal)
            || string.Equals(mask, "undef", StringComparison.Ordinal))
        {
            return true;
        }

        if (!TryParseVectorConstant(mask, out var elements) || outputIndex >= elements.Length)
        {
            return false;
        }

        return int.TryParse(elements[outputIndex], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out sourceIndex);
    }

    private static bool TryParseVectorType(string vectorType, out int count, out string elementType)
    {
        count = 0;
        elementType = string.Empty;
        var trimmed = vectorType.Trim();
        if (trimmed.Length < 5 || trimmed[0] != '<' || trimmed[^1] != '>')
        {
            return false;
        }

        var inner = trimmed[1..^1].Trim();
        var xIdx = inner.IndexOf(" x ", StringComparison.Ordinal);
        if (xIdx < 0 || !int.TryParse(inner[..xIdx].Trim(), out count) || count <= 0)
        {
            return false;
        }

        elementType = inner[(xIdx + 3)..].Trim();
        return !string.IsNullOrWhiteSpace(elementType);
    }

    private static LoweredCallInstruction? FindCallByResult(LoweredFunction function, string resultName)
    {
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredCallInstruction call && string.Equals(call.Result, resultName, StringComparison.Ordinal))
                    return call;
            }
        }
        return null;
    }

    private static LoweredCmpxchgInstruction? FindCmpxchgByResult(LoweredFunction function, string resultName)
    {
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredCmpxchgInstruction cx && string.Equals(cx.Result, resultName, StringComparison.Ordinal))
                    return cx;
            }
        }
        return null;
    }

    private static void EmitTypedSwitch(
        InstructionEncoder encoder,
        LoweredSwitchInstruction sw,
        SrmTypeContext typeContext,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles,
        IReadOnlyDictionary<string, LabelHandle> labelMap,
        IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock,
        string sourceBlock)
    {
        var switchValue = NormalizeRawValue(sw.Value);
        var isWideSwitch = string.Equals(sw.ValueType, "i64", StringComparison.Ordinal);

        foreach (var c in sw.Cases)
        {
            EmitLoadValue(encoder, switchValue, paramIndices, localIndices, fieldHandles);
            if (isWideSwitch)
            {
                encoder.LoadConstantI8(c.Value);
            }
            else
            {
                encoder.LoadConstantI4((int)c.Value);
            }
            encoder.OpCode(ILOpCode.Ceq);
            var nextCase = encoder.DefineLabel();
            encoder.Branch(ILOpCode.Brfalse, nextCase);
            EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, c.Target);
            encoder.Branch(ILOpCode.Br, labelMap[c.Target]);
            encoder.MarkLabel(nextCase);
        }

        EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, sw.DefaultLabel);
        encoder.Branch(ILOpCode.Br, labelMap[sw.DefaultLabel]);
    }

    private static bool TryEmitRawSwitch(
        InstructionEncoder encoder,
        LoweredRawInstruction raw,
        IReadOnlyList<LoweredInstruction> instructions,
        ref int instrIdx,
        SrmTypeContext typeContext,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles,
        IReadOnlyDictionary<string, LabelHandle> labelMap,
        IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock,
        string sourceBlock)
    {
        var switchMatch = System.Text.RegularExpressions.Regex.Match(
            raw.Text,
            "^switch (?<type>i\\d+) (?<value>[^,]+), label %(?<defaultTarget>\"[^\"]+\"|[^\\s]+) \\[$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!switchMatch.Success)
        {
            return false;
        }

        var switchValue = NormalizeRawValue(switchMatch.Groups["value"].Value);
        var defaultTarget = NormalizeRawLabel(switchMatch.Groups["defaultTarget"].Value);
        var caseLabels = new List<(long Value, string Target)>();

        var closingIndex = -1;
        for (var index = instrIdx + 1; index < instructions.Count; index++)
        {
            if (instructions[index] is not LoweredRawInstruction caseRaw)
            {
                break;
            }

            if (string.Equals(caseRaw.Text, "]", StringComparison.Ordinal))
            {
                closingIndex = index;
                break;
            }

            var caseMatch = System.Text.RegularExpressions.Regex.Match(
                caseRaw.Text,
                "^(?<type>i\\d+) (?<value>-?\\d+), label %(?<target>\"[^\"]+\"|[^\\s]+)$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant);
            if (caseMatch.Success)
            {
                caseLabels.Add((long.Parse(caseMatch.Groups["value"].Value), NormalizeRawLabel(caseMatch.Groups["target"].Value)));
            }
        }

        if (closingIndex < 0)
        {
            return false;
        }

        // Emit compare-and-branch chain for each case
        var switchType = switchMatch.Groups["type"].Value;
        var isWideSwitch = string.Equals(switchType, "i64", StringComparison.Ordinal);
        foreach (var caseLabel in caseLabels)
        {
            EmitLoadValue(encoder, switchValue, paramIndices, localIndices, fieldHandles);
            if (isWideSwitch)
            {
                encoder.LoadConstantI8(caseLabel.Value);
            }
            else
            {
                encoder.LoadConstantI4((int)caseLabel.Value);
            }
            encoder.OpCode(ILOpCode.Ceq);
            var nextCase = encoder.DefineLabel();
            encoder.Branch(ILOpCode.Brfalse, nextCase);
            EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, caseLabel.Target);
            encoder.Branch(ILOpCode.Br, labelMap[caseLabel.Target]);
            encoder.MarkLabel(nextCase);
        }

        // Default case
        EmitPhiCopies(encoder, typeContext, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, defaultTarget);
        encoder.Branch(ILOpCode.Br, labelMap[defaultTarget]);
        instrIdx = closingIndex;
        return true;
    }

    private static string NormalizeRawLabel(string rawLabel)
    {
        var label = rawLabel.Trim().TrimStart('%');
        return label.Length >= 2 && label[0] == '"' && label[^1] == '"'
            ? label[1..^1]
            : label;
    }

    private static string NormalizeRawValue(string rawValue)
    {
        // Strip type prefix: "%x" -> "x", "i32 %x" -> "x"
        var value = rawValue.Trim();
        var percentIdx = value.IndexOf('%');
        if (percentIdx >= 0)
        {
            var name = value[(percentIdx + 1)..];
            // LLVM unnamed registers %0, %1, etc. correspond to lowered "tmp.N" names
            if (int.TryParse(name, out _))
                return "tmp." + name;
            return NormalizeNamedValue(name);
        }
        return value;
    }

    private static string NormalizeIndirectCalleeName(string callee)
    {
        var normalized = callee.Trim();
        if (normalized.StartsWith('%'))
        {
            normalized = normalized[1..];
        }

        if (normalized.Length > 0 && normalized.All(char.IsDigit))
        {
            return $"tmp.{normalized}";
        }

        return NormalizeNamedValue(normalized);
    }

    private static string NormalizeNamedValue(string value)
    {
        const string prefix = "tmp.";
        return value.StartsWith(prefix, StringComparison.Ordinal)
            && value.Length > prefix.Length
            && value[prefix.Length..].All(char.IsDigit)
            ? $"named_{value}"
            : value;
    }

    private static string NormalizeSymbolReference(string value)
    {
        var normalized = value.Trim();
        return normalized.Length >= 2 && normalized[0] == '"' && normalized[^1] == '"'
            ? normalized[1..^1]
            : normalized;
    }

    private static bool IsGepResult(string localName, IReadOnlyList<LoweredInstruction> instructions, int currentIdx)
    {
        // Check if localName was defined by a GEP instruction earlier in the function
        for (var i = 0; i < currentIdx; i++)
        {
            if (instructions[i] is LoweredGetElementPointerInstruction gep &&
                string.Equals(gep.Result, localName, StringComparison.Ordinal))
            {
                return true;
            }
        }
        return false;
    }

    private static void EmitIndirectLoad(InstructionEncoder encoder, string type)
    {
        if (TryGetIntegerBitWidth(type, out var width))
        {
            if (width <= 8)
                encoder.OpCode(ILOpCode.Ldind_u1);
            else if (width <= 16)
                encoder.OpCode(ILOpCode.Ldind_u2);
            else if (width <= 32)
                encoder.OpCode(ILOpCode.Ldind_i4);
            else
                encoder.OpCode(ILOpCode.Ldind_i8);
        }
        else if (string.Equals(type, "ptr", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Ldind_i);
        }
        else if (string.Equals(type, "float", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Ldind_r4);
        }
        else if (string.Equals(type, "double", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Ldind_r8);
        }
        else
        {
            encoder.OpCode(ILOpCode.Ldind_i4);
        }
    }

    private static void EmitIndirectStore(InstructionEncoder encoder, string type)
    {
        if (TryGetIntegerBitWidth(type, out var width))
        {
            if (width <= 8)
                encoder.OpCode(ILOpCode.Stind_i1);
            else if (width <= 16)
                encoder.OpCode(ILOpCode.Stind_i2);
            else if (width <= 32)
                encoder.OpCode(ILOpCode.Stind_i4);
            else
                encoder.OpCode(ILOpCode.Stind_i8);
        }
        else if (string.Equals(type, "ptr", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Stind_i);
        }
        else if (string.Equals(type, "float", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Stind_r4);
        }
        else if (string.Equals(type, "double", StringComparison.Ordinal))
        {
            encoder.OpCode(ILOpCode.Stind_r8);
        }
        else
        {
            encoder.OpCode(ILOpCode.Stind_i4);
        }
    }

    private static int ParseAllocaSize(string allocaType)
    {
        // Parse "[N x i8]" → N, or type size for simple types
        var match = System.Text.RegularExpressions.Regex.Match(allocaType, @"\[(\d+)\s+x\s+i8\]");
        if (match.Success)
            return int.Parse(match.Groups[1].Value);
        // Fallback: estimate from type
        if (TryGetIntegerBitWidth(allocaType, out var bits))
            return (bits + 7) / 8;
        return 8; // default
    }

    private static void EmitConstantValue(InstructionEncoder encoder, string type, long value)
    {
        if (TryGetIntegerBitWidth(type, out var width) && width > 32)
        {
            encoder.LoadConstantI8(value);
        }
        else
        {
            encoder.LoadConstantI4((int)value);
        }
    }

    private static bool TryResolveConstantGlobalElement(string source, string type, IReadOnlyDictionary<string, LoweredGlobal> globalMap, out long value)
    {
        value = 0;
        // For scalar globals with small initializers, resolve the constant directly
        if (!globalMap.TryGetValue(source, out var global))
        {
            return false;
        }
        if (TryGetIntegerBitWidth(type, out var width) && width <= 32 && global.InitializerBytes.Count == 4)
        {
            value = BitConverter.ToInt32([.. global.InitializerBytes], 0);
            return true;
        }
        if (TryGetIntegerBitWidth(type, out width) && width <= 64 && global.InitializerBytes.Count == 8)
        {
            value = BitConverter.ToInt64([.. global.InitializerBytes], 0);
            return true;
        }
        return false;
    }

    private static bool TryParseGlobalElementAccess(string source, out string baseName, out int index)
    {
        baseName = "";
        index = 0;
        var bracketIdx = source.IndexOf('[');
        if (bracketIdx < 0) return false;
        var closeBracket = source.IndexOf(']', bracketIdx);
        if (closeBracket < 0) return false;
        baseName = source[..bracketIdx];
        return int.TryParse(source[(bracketIdx + 1)..closeBracket], out index);
    }

    private static bool TryResolveConstantGlobalElementAtIndex(string baseName, int elementIndex, string type, IReadOnlyDictionary<string, LoweredGlobal> globalMap, out long value)
    {
        value = 0;
        if (!globalMap.TryGetValue(baseName, out var global))
        {
            return false;
        }
        var stride = GetElementTypeStride(type);
        var offset = elementIndex * stride;
        if (offset + stride > global.InitializerBytes.Count)
        {
            return false;
        }
        var bytes = global.InitializerBytes.Skip(offset).Take(stride).ToArray();
        if (stride == 4)
        {
            value = BitConverter.ToInt32(bytes, 0);
            return true;
        }
        if (stride == 8)
        {
            value = BitConverter.ToInt64(bytes, 0);
            return true;
        }
        if (stride == 1)
        {
            value = bytes[0];
            return true;
        }
        if (stride == 2)
        {
            value = BitConverter.ToInt16(bytes, 0);
            return true;
        }
        return false;
    }

    private static int GetElementTypeStride(string elementType)
    {
        if (TryGetIntegerBitWidth(elementType, out var width))
        {
            return Math.Max(1, width / 8);
        }
        return elementType switch
        {
            "float" => 4,
            "double" => 8,
            "ptr" => 8,
            _ => 1
        };
    }

    private static BlobHandle EncodeFieldSignature(MetadataBuilder metadataBuilder, LoweredGlobal global)
    {
        var blob = new BlobBuilder();
        var encoder = new BlobEncoder(blob).Field().Type();
        if (global.InitializerBytes.Count > 8)
        {
            encoder.IntPtr();
        }
        else if (global.InitializerBytes.Count > 4)
        {
            encoder.Int64();
        }
        else
        {
            encoder.Int32();
        }
        return metadataBuilder.GetOrAddBlob(blob);
    }

    private static int EmitCctorBody(MetadataBuilder metadataBuilder, MethodBodyStreamEncoder methodBodyStream, IReadOnlyList<LoweredGlobal> globals, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles, IReadOnlyDictionary<string, LoweredGlobal> globalMap, SrmTypeContext typeContext)
    {
        var codeBuilder = new BlobBuilder();
        var encoder = new InstructionEncoder(codeBuilder);

        // For pointer-backed globals, we need a local to hold the IntPtr
        var hasPointerGlobal = globals.Any(g => g.InitializerBytes.Count > 8);
        var localCount = hasPointerGlobal ? 1 : 0;

        StandaloneSignatureHandle localSigHandle = default;
        if (localCount > 0)
        {
            var localSigBlob = new BlobBuilder();
            new BlobEncoder(localSigBlob).LocalVariableSignature(localCount)
                .AddVariable().Type().IntPtr();
            localSigHandle = metadataBuilder.AddStandaloneSignature(metadataBuilder.GetOrAddBlob(localSigBlob));
        }

        foreach (var global in globals)
        {
            var fieldHandle = fieldHandles[global.Name];

            if (global.InitializerBytes.Count > 8)
            {
                // Pointer-backed global: allocate native memory, copy bytes, store pointer
                encoder.LoadConstantI4(global.InitializerBytes.Count);
                encoder.OpCode(ILOpCode.Conv_i);
                encoder.Call(typeContext.MarshalAllocHGlobal);
                encoder.StoreLocal(0);

                // Create byte array and populate
                encoder.LoadConstantI4(global.InitializerBytes.Count);
                encoder.OpCode(ILOpCode.Newarr);
                encoder.Token(typeContext.ByteTypeRef);

                for (var i = 0; i < global.InitializerBytes.Count; i++)
                {
                    encoder.OpCode(ILOpCode.Dup);
                    encoder.LoadConstantI4(i);
                    encoder.LoadConstantI4((int)global.InitializerBytes[i]);
                    encoder.OpCode(ILOpCode.Stelem_i1);
                }

                // Marshal.Copy(byte[], 0, IntPtr, count)
                encoder.LoadConstantI4(0);
                encoder.LoadLocal(0);
                encoder.LoadConstantI4(global.InitializerBytes.Count);
                encoder.Call(typeContext.MarshalCopy);

                foreach (var relocation in global.PointerRelocations)
                {
                    encoder.LoadLocal(0);
                    if (relocation.Offset != 0)
                    {
                        encoder.LoadConstantI4(relocation.Offset);
                        encoder.OpCode(ILOpCode.Conv_i);
                        encoder.OpCode(ILOpCode.Add);
                    }

                    if (methodHandles.TryGetValue(relocation.Target, out var targetHandle))
                    {
                        encoder.OpCode(ILOpCode.Ldftn);
                        encoder.Token(targetHandle);
                        encoder.OpCode(ILOpCode.Conv_i);
                    }
                    else if (fieldHandles.TryGetValue(relocation.Target, out var targetFieldHandle))
                    {
                        if (IsPointerBackedGlobal(globalMap, relocation.Target))
                        {
                            encoder.OpCode(ILOpCode.Ldsfld);
                            encoder.Token(targetFieldHandle);
                        }
                        else
                        {
                            encoder.OpCode(ILOpCode.Ldsflda);
                            encoder.Token(targetFieldHandle);
                            encoder.OpCode(ILOpCode.Conv_u);
                        }
                    }
                    else
                    {
                        encoder.LoadConstantI4(0);
                        encoder.OpCode(ILOpCode.Conv_i);
                    }
                    encoder.OpCode(ILOpCode.Stind_i);
                }

                encoder.LoadLocal(0);
            }
            else if (global.InitializerBytes.Count > 4)
            {
                var padded = new byte[8];
                for (var i = 0; i < global.InitializerBytes.Count; i++)
                    padded[i] = global.InitializerBytes[i];
                encoder.LoadConstantI8(BitConverter.ToInt64(padded, 0));
            }
            else
            {
                var padded = new byte[4];
                for (var i = 0; i < global.InitializerBytes.Count; i++)
                    padded[i] = global.InitializerBytes[i];
                encoder.LoadConstantI4(BitConverter.ToInt32(padded, 0));
            }

            encoder.OpCode(ILOpCode.Stsfld);
            encoder.Token(fieldHandle);
        }

        encoder.OpCode(ILOpCode.Ret);

        return methodBodyStream.AddMethodBody(
            encoder,
            maxStack: 8,
            localVariablesSignature: localSigHandle,
            attributes: localCount > 0 ? MethodBodyAttributes.InitLocals : MethodBodyAttributes.None);
    }

    private static void WritePE(string outputPath, MetadataBuilder metadataBuilder, BlobBuilder ilBuilder, MethodDefinitionHandle entryPoint, PortablePdbResult? pdbResult = null)
    {
        var metadataRootBuilder = new MetadataRootBuilder(metadataBuilder);

        var peHeaderBuilder = entryPoint.IsNil
            ? PEHeaderBuilder.CreateLibraryHeader()
            : PEHeaderBuilder.CreateExecutableHeader();

        // Build debug directory entries if PDB was emitted
        var debugDirectoryBuilder = pdbResult is not null ? new DebugDirectoryBuilder() : null;
        if (pdbResult is not null && debugDirectoryBuilder is not null)
        {
            var pdbFileName = Path.GetFileName(pdbResult.PdbPath);
            debugDirectoryBuilder.AddCodeViewEntry(pdbFileName, pdbResult.ContentId, portablePdbVersion: 0x0100);
        }

        var peBuilder = new ManagedPEBuilder(
            header: peHeaderBuilder,
            metadataRootBuilder: metadataRootBuilder,
            ilStream: ilBuilder,
            entryPoint: entryPoint,
            debugDirectoryBuilder: debugDirectoryBuilder);

        var peBlob = new BlobBuilder();
        peBuilder.Serialize(peBlob);

        using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        peBlob.WriteContentTo(fs);
    }

    private static MethodDefinitionHandle EmitConsoleEntrypoint(
        MetadataBuilder metadataBuilder,
        MethodBodyStreamEncoder methodBodyStream,
        BlobBuilder ilBuilder,
        IReadOnlyList<LoweredFunction> functions,
        LoweredFunction consoleEntrypoint,
        IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles,
        SrmTypeContext typeContext,
        EntityHandle systemObjectRef,
        bool requiresStaThread,
        ref int paramRowIndex)
    {
        // Emit GeneratedEntryPoint type definition
        metadataBuilder.AddTypeDefinition(
            attributes: TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class,
            @namespace: metadataBuilder.GetOrAddString("Rustlyn"),
            name: metadataBuilder.GetOrAddString("GeneratedEntryPoint"),
            baseType: systemObjectRef,
            fieldList: MetadataTokens.FieldDefinitionHandle(metadataBuilder.GetRowCount(TableIndex.Field) + 1),
            methodList: MetadataTokens.MethodDefinitionHandle(metadataBuilder.GetRowCount(TableIndex.MethodDef) + 1));

        // Build Main(string[] args) -> int signature
        var mainSigBlob = new BlobBuilder();
        new BlobEncoder(mainSigBlob).MethodSignature().Parameters(1, out var mainRet, out var mainParms);
        mainRet.Type().Int32();
        mainParms.AddParameter().Type().SZArray().String();

        // Emit the Main wrapper body: call main(), handle void return
        var codeBuilder = new BlobBuilder();
        var encoder = new InstructionEncoder(codeBuilder);

        encoder.Call(methodHandles[consoleEntrypoint.Name]);
        if (string.Equals(consoleEntrypoint.ReturnType, "void", StringComparison.Ordinal))
        {
            encoder.LoadConstantI4(0);
        }
        encoder.OpCode(ILOpCode.Ret);

        var bodyOffset = methodBodyStream.AddMethodBody(encoder, maxStack: 2);

        // Add the Main method definition
        var mainMethodHandle = metadataBuilder.AddMethodDefinition(
            attributes: MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig,
            implAttributes: MethodImplAttributes.IL | MethodImplAttributes.Managed,
            name: metadataBuilder.GetOrAddString("Main"),
            signature: metadataBuilder.GetOrAddBlob(mainSigBlob),
            bodyOffset: bodyOffset,
            parameterList: MetadataTokens.ParameterHandle(paramRowIndex));

        metadataBuilder.AddParameter(
            attributes: ParameterAttributes.None,
            name: metadataBuilder.GetOrAddString("args"),
            sequenceNumber: 1);
        paramRowIndex++;

        // Add [STAThread] attribute if required
        if (requiresStaThread)
        {
            var staThreadType = metadataBuilder.AddTypeReference(
                typeContext.SystemRuntime,
                metadataBuilder.GetOrAddString("System"),
                metadataBuilder.GetOrAddString("STAThreadAttribute"));

            // .ctor() signature: instance void ()
            var ctorSigBlob = new BlobBuilder();
            new BlobEncoder(ctorSigBlob).MethodSignature(SignatureCallingConvention.Default, 0, true)
                .Parameters(0, out var ctorRet, out _);
            ctorRet.Void();

            var staThreadCtor = metadataBuilder.AddMemberReference(
                staThreadType,
                metadataBuilder.GetOrAddString(".ctor"),
                metadataBuilder.GetOrAddBlob(ctorSigBlob));

            // Custom attribute value: just prolog (0x0001) + no named args
            var attrValueBlob = new BlobBuilder();
            attrValueBlob.WriteUInt16(0x0001); // prolog
            attrValueBlob.WriteUInt16(0x0000); // no named args

            metadataBuilder.AddCustomAttribute(
                mainMethodHandle,
                staThreadCtor,
                metadataBuilder.GetOrAddBlob(attrValueBlob));
        }

        return mainMethodHandle;
    }

    private static int EmitStubBody(MetadataBuilder metadataBuilder, MethodBodyStreamEncoder methodBodyStream, SrmTypeContext typeContext, string functionName, string reason)
    {
        var codeBuilder = new BlobBuilder();
        var encoder = new InstructionEncoder(codeBuilder);

        // ldstr "Unsupported: <functionName>: <reason>"
        var message = $"Unsupported: {functionName}: {reason}";
        encoder.LoadString(metadataBuilder.GetOrAddUserString(message));
        // newobj NotSupportedException(string)
        encoder.OpCode(ILOpCode.Newobj);
        encoder.Token(typeContext.NotSupportedExceptionCtor);
        encoder.OpCode(ILOpCode.Throw);

        return methodBodyStream.AddMethodBody(encoder, maxStack: 2);
    }

    private static bool TryGetIntegerBitWidth(string typeName, out int width)
    {
        width = 0;
        if (typeName.Length < 2 || typeName[0] != 'i')
            return false;
        return int.TryParse(typeName.AsSpan(1), out width) && width > 0;
    }

    /// <summary>
    /// Pre-computed type and member reference handles for BCL and intrinsic calls.
    /// All handles must be added before any method body references them.
    /// </summary>
    private sealed class SrmTypeContext
    {
        public AssemblyReferenceHandle SystemRuntime { get; }
        public TypeReferenceHandle BitOperations { get; }
        public TypeReferenceHandle ByteTypeRef { get; }
        public TypeReferenceHandle SByteTypeRef { get; }
        public TypeReferenceHandle Int16TypeRef { get; }
        public TypeReferenceHandle Int32TypeRef { get; }
        public TypeReferenceHandle Int64TypeRef { get; }
        public MemberReferenceHandle PopCountU32 { get; }
        public MemberReferenceHandle PopCountU64 { get; }
        public MemberReferenceHandle LeadingZeroCountU32 { get; }
        public MemberReferenceHandle LeadingZeroCountU64 { get; }
        public MemberReferenceHandle TrailingZeroCountU32 { get; }
        public MemberReferenceHandle TrailingZeroCountU64 { get; }
        public MemberReferenceHandle ReverseEndianness32 { get; }
        public MemberReferenceHandle ReverseEndianness64 { get; }
        public MemberReferenceHandle MarshalAllocHGlobal { get; }
        public MemberReferenceHandle MarshalFreeHGlobal { get; }
        public MemberReferenceHandle MarshalCopy { get; }
        public MemberReferenceHandle NotSupportedExceptionCtor { get; }
        public MemberReferenceHandle DivideByZeroExceptionCtor { get; }
        public MemberReferenceHandle OverflowExceptionCtor { get; }
        public MemberReferenceHandle MathSqrt { get; }
        public MemberReferenceHandle MathFloor { get; }
        public MemberReferenceHandle MathCeiling { get; }
        public MemberReferenceHandle MathAbs { get; }
        public MemberReferenceHandle MathMin { get; }
        public MemberReferenceHandle MathMax { get; }
        public MemberReferenceHandle MathPow { get; }
        public MemberReferenceHandle MathMaxI32 { get; }
        public MemberReferenceHandle MathMinI32 { get; }
        public MemberReferenceHandle MathMaxI64 { get; }
        public MemberReferenceHandle MathMinI64 { get; }
        public MemberReferenceHandle MathMaxU32 { get; }
        public MemberReferenceHandle MathMinU32 { get; }
        public MemberReferenceHandle MathMaxU64 { get; }
        public MemberReferenceHandle MathMinU64 { get; }
        public MemberReferenceHandle MathFSqrt { get; }
        public MemberReferenceHandle MathFFloor { get; }
        public MemberReferenceHandle MathFCeiling { get; }
        public MemberReferenceHandle MathFAbs { get; }

        private readonly Dictionary<string, MemberReferenceHandle> _intrinsicMap;
        private readonly Dictionary<string, MemberReferenceHandle> _avaloniaBridgeMap;
        private readonly Dictionary<string, MemberReferenceHandle> _runtimeBridgeMap;
        private readonly HashSet<MemberReferenceHandle> _runtimeBridgeSretHandles;

        public SrmTypeContext(
            MetadataBuilder mb,
            AssemblyReferenceHandle systemRuntime,
            AssemblyReferenceHandle systemInterop,
            IReadOnlyList<BindingManifestDocument>? bindingManifests = null)
        {
            SystemRuntime = systemRuntime;

            // Additional assembly reference for types not in System.Runtime
            var systemMemory = mb.AddAssemblyReference(
                name: mb.GetOrAddString("System.Memory"),
                version: new Version(10, 0, 0, 0),
                culture: default,
                publicKeyOrToken: mb.GetOrAddBlob(
                    new byte[] { 0xCC, 0x7B, 0x13, 0xFF, 0xCD, 0x2D, 0xDD, 0x51 }),
                flags: default,
                hashValue: default);

            var avaloniaSupportAssemblyName = typeof(Rustlyn.AvaloniaSupport.AvaloniaBridge).Assembly.GetName();
            var avaloniaSupport = mb.AddAssemblyReference(
                name: mb.GetOrAddString(avaloniaSupportAssemblyName.Name ?? "Rustlyn.AvaloniaSupport"),
                version: avaloniaSupportAssemblyName.Version ?? new Version(1, 0, 0, 0),
                culture: default,
                publicKeyOrToken: default,
                flags: default,
                hashValue: default);

            var backendAssemblyName = typeof(RuntimeBridgeHelpers).Assembly.GetName();
            var backendAssembly = mb.AddAssemblyReference(
                name: mb.GetOrAddString(backendAssemblyName.Name ?? "Rustlyn.Backend"),
                version: backendAssemblyName.Version ?? new Version(1, 0, 0, 0),
                culture: default,
                publicKeyOrToken: default,
                flags: default,
                hashValue: default);

            // System.Numerics.BitOperations (in System.Runtime)
            BitOperations = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System.Numerics"),
                mb.GetOrAddString("BitOperations"));

            // System.Buffers.Binary.BinaryPrimitives (in System.Memory)
            var binaryPrimitives = mb.AddTypeReference(
                systemMemory,
                mb.GetOrAddString("System.Buffers.Binary"),
                mb.GetOrAddString("BinaryPrimitives"));

            var avaloniaBridge = mb.AddTypeReference(
                avaloniaSupport,
                mb.GetOrAddString("Rustlyn.AvaloniaSupport"),
                mb.GetOrAddString("AvaloniaBridge"));

            var runtimeBridgeHelpers = mb.AddTypeReference(
                backendAssembly,
                mb.GetOrAddString("Rustlyn.Backend"),
                mb.GetOrAddString("RuntimeBridgeHelpers"));

            // Method signatures
            PopCountU32 = AddStaticMethod(mb, BitOperations, "PopCount", EncodeU32ToI32Sig(mb));
            PopCountU64 = AddStaticMethod(mb, BitOperations, "PopCount", EncodeU64ToI32Sig(mb));
            LeadingZeroCountU32 = AddStaticMethod(mb, BitOperations, "LeadingZeroCount", EncodeU32ToI32Sig(mb));
            LeadingZeroCountU64 = AddStaticMethod(mb, BitOperations, "LeadingZeroCount", EncodeU64ToI32Sig(mb));
            TrailingZeroCountU32 = AddStaticMethod(mb, BitOperations, "TrailingZeroCount", EncodeU32ToI32Sig(mb));
            TrailingZeroCountU64 = AddStaticMethod(mb, BitOperations, "TrailingZeroCount", EncodeU64ToI32Sig(mb));
            ReverseEndianness32 = AddStaticMethod(mb, binaryPrimitives, "ReverseEndianness", EncodeI32ToI32Sig(mb));
            ReverseEndianness64 = AddStaticMethod(mb, binaryPrimitives, "ReverseEndianness", EncodeI64ToI64Sig(mb));

            // System.Math (for libm intrinsics: sqrt, floor, ceil, etc.)
            var mathType = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("Math"));

            var f64ToF64Sig = EncodeF64ToF64Sig(mb);
            MathSqrt = AddStaticMethod(mb, mathType, "Sqrt", f64ToF64Sig);
            MathFloor = AddStaticMethod(mb, mathType, "Floor", f64ToF64Sig);
            MathCeiling = AddStaticMethod(mb, mathType, "Ceiling", f64ToF64Sig);
            MathAbs = AddStaticMethod(mb, mathType, "Abs", f64ToF64Sig);

            var f64f64ToF64Sig = EncodeF64F64ToF64Sig(mb);
            MathMin = AddStaticMethod(mb, mathType, "Min", f64f64ToF64Sig);
            MathMax = AddStaticMethod(mb, mathType, "Max", f64f64ToF64Sig);
            MathPow = AddStaticMethod(mb, mathType, "Pow", f64f64ToF64Sig);

            // Integer Math.Max/Min overloads (for llvm.smax/smin/umax/umin)
            var i32i32ToI32Sig = EncodeI32I32ToI32Sig(mb);
            MathMaxI32 = AddStaticMethod(mb, mathType, "Max", i32i32ToI32Sig);
            MathMinI32 = AddStaticMethod(mb, mathType, "Min", i32i32ToI32Sig);
            var i64i64ToI64Sig = EncodeI64I64ToI64Sig(mb);
            MathMaxI64 = AddStaticMethod(mb, mathType, "Max", i64i64ToI64Sig);
            MathMinI64 = AddStaticMethod(mb, mathType, "Min", i64i64ToI64Sig);
            var u32u32ToU32Sig = EncodeU32U32ToU32Sig(mb);
            MathMaxU32 = AddStaticMethod(mb, mathType, "Max", u32u32ToU32Sig);
            MathMinU32 = AddStaticMethod(mb, mathType, "Min", u32u32ToU32Sig);
            var u64u64ToU64Sig = EncodeU64U64ToU64Sig(mb);
            MathMaxU64 = AddStaticMethod(mb, mathType, "Max", u64u64ToU64Sig);
            MathMinU64 = AddStaticMethod(mb, mathType, "Min", u64u64ToU64Sig);

            // System.MathF (for float-specific operations)
            var mathFType = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("MathF"));

            var f32ToF32Sig = EncodeF32ToF32Sig(mb);
            MathFSqrt = AddStaticMethod(mb, mathFType, "Sqrt", f32ToF32Sig);
            MathFFloor = AddStaticMethod(mb, mathFType, "Floor", f32ToF32Sig);
            MathFCeiling = AddStaticMethod(mb, mathFType, "Ceiling", f32ToF32Sig);
            MathFAbs = AddStaticMethod(mb, mathFType, "Abs", f32ToF32Sig);

            // System.Byte type reference (for newarr in .cctor)
            ByteTypeRef = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("Byte"));
            SByteTypeRef = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("SByte"));
            Int16TypeRef = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("Int16"));
            Int32TypeRef = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("Int32"));
            Int64TypeRef = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("Int64"));

            // System.Runtime.InteropServices.Marshal references (for pointer-backed globals)
            var marshalType = mb.AddTypeReference(
                systemInterop,
                mb.GetOrAddString("System.Runtime.InteropServices"),
                mb.GetOrAddString("Marshal"));

            // Marshal.AllocHGlobal(IntPtr) -> IntPtr
            var allocHGlobalSig = new BlobBuilder();
            new BlobEncoder(allocHGlobalSig).MethodSignature().Parameters(1, out var allocRet, out var allocParms);
            allocRet.Type().IntPtr();
            allocParms.AddParameter().Type().IntPtr();
            MarshalAllocHGlobal = AddStaticMethod(mb, marshalType, "AllocHGlobal", mb.GetOrAddBlob(allocHGlobalSig));

            // Marshal.FreeHGlobal(IntPtr) -> void
            var freeHGlobalSig = new BlobBuilder();
            new BlobEncoder(freeHGlobalSig).MethodSignature().Parameters(1, out var freeRet, out var freeParms);
            freeRet.Void();
            freeParms.AddParameter().Type().IntPtr();
            MarshalFreeHGlobal = AddStaticMethod(mb, marshalType, "FreeHGlobal", mb.GetOrAddBlob(freeHGlobalSig));

            // Marshal.Copy(byte[], int, IntPtr, int) -> void
            var copySig = new BlobBuilder();
            new BlobEncoder(copySig).MethodSignature().Parameters(4, out var copyRet, out var copyParms);
            copyRet.Void();
            copyParms.AddParameter().Type().SZArray().Byte();
            copyParms.AddParameter().Type().Int32();
            copyParms.AddParameter().Type().IntPtr();
            copyParms.AddParameter().Type().Int32();
            MarshalCopy = AddStaticMethod(mb, marshalType, "Copy", mb.GetOrAddBlob(copySig));

            // System.NotSupportedException..ctor(string)
            var notSupportedExceptionType = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("NotSupportedException"));

            var nseCtorSig = new BlobBuilder();
            new BlobEncoder(nseCtorSig).MethodSignature(SignatureCallingConvention.Default, 0, true)
                .Parameters(1, out var nseCtorRet, out var nseCtorParms);
            nseCtorRet.Void();
            nseCtorParms.AddParameter().Type().String();
            NotSupportedExceptionCtor = mb.AddMemberReference(
                notSupportedExceptionType,
                mb.GetOrAddString(".ctor"),
                mb.GetOrAddBlob(nseCtorSig));

            // System.DivideByZeroException..ctor()
            var divideByZeroExceptionType = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("DivideByZeroException"));

            var dbzCtorSig = new BlobBuilder();
            new BlobEncoder(dbzCtorSig).MethodSignature(SignatureCallingConvention.Default, 0, true)
                .Parameters(0, out var dbzCtorRet, out _);
            dbzCtorRet.Void();
            DivideByZeroExceptionCtor = mb.AddMemberReference(
                divideByZeroExceptionType,
                mb.GetOrAddString(".ctor"),
                mb.GetOrAddBlob(dbzCtorSig));

            // System.OverflowException..ctor()
            var overflowExceptionType = mb.AddTypeReference(
                systemRuntime,
                mb.GetOrAddString("System"),
                mb.GetOrAddString("OverflowException"));

            var ofeCtorSig = new BlobBuilder();
            new BlobEncoder(ofeCtorSig).MethodSignature(SignatureCallingConvention.Default, 0, true)
                .Parameters(0, out var ofeCtorRet, out _);
            ofeCtorRet.Void();
            OverflowExceptionCtor = mb.AddMemberReference(
                overflowExceptionType,
                mb.GetOrAddString(".ctor"),
                mb.GetOrAddBlob(ofeCtorSig));

            _avaloniaBridgeMap = new Dictionary<string, MemberReferenceHandle>(StringComparer.Ordinal)
            {
                ["rustlyn_avalonia_run_app"] = AddStaticMethod(mb, avaloniaBridge, "RunApp", EncodeBridgeMethodSig(mb, "i32"))
            };

            _runtimeBridgeMap = BuildRuntimeBridgeMap(mb, runtimeBridgeHelpers, bindingManifests ?? [], out _runtimeBridgeSretHandles);

            // Build intrinsic lookup
            _intrinsicMap = new Dictionary<string, MemberReferenceHandle>(StringComparer.Ordinal)
            {
                ["llvm.ctpop.i32"] = PopCountU32,
                ["llvm.ctpop.i64"] = PopCountU64,
                ["llvm.ctlz.i32"] = LeadingZeroCountU32,
                ["llvm.ctlz.i64"] = LeadingZeroCountU64,
                ["llvm.cttz.i32"] = TrailingZeroCountU32,
                ["llvm.cttz.i64"] = TrailingZeroCountU64,
                ["llvm.bswap.i32"] = ReverseEndianness32,
                ["llvm.bswap.i64"] = ReverseEndianness64,
                // libm double functions
                ["sqrt"] = MathSqrt,
                ["llvm.sqrt.f64"] = MathSqrt,
                ["floor"] = MathFloor,
                ["llvm.floor.f64"] = MathFloor,
                ["ceil"] = MathCeiling,
                ["llvm.ceil.f64"] = MathCeiling,
                ["fabs"] = MathAbs,
                ["llvm.fabs.f64"] = MathAbs,
                ["fmin"] = MathMin,
                ["llvm.minnum.f64"] = MathMin,
                ["fmax"] = MathMax,
                ["llvm.maxnum.f64"] = MathMax,
                ["pow"] = MathPow,
                ["llvm.pow.f64"] = MathPow,
                // libm float functions
                ["sqrtf"] = MathFSqrt,
                ["llvm.sqrt.f32"] = MathFSqrt,
                ["floorf"] = MathFFloor,
                ["llvm.floor.f32"] = MathFFloor,
                ["ceilf"] = MathFCeiling,
                ["llvm.ceil.f32"] = MathFCeiling,
                ["fabsf"] = MathFAbs,
                ["llvm.fabs.f32"] = MathFAbs,
                // Integer max/min intrinsics
                ["llvm.smax.i32"] = MathMaxI32,
                ["llvm.smax.i64"] = MathMaxI64,
                ["llvm.smin.i32"] = MathMinI32,
                ["llvm.smin.i64"] = MathMinI64,
                ["llvm.umax.i32"] = MathMaxU32,
                ["llvm.umax.i64"] = MathMaxU64,
                ["llvm.umin.i32"] = MathMinU32,
                ["llvm.umin.i64"] = MathMinU64,
            };
        }

        public bool TryResolveIntrinsic(string callee, out MemberReferenceHandle handle)
        {
            return _intrinsicMap.TryGetValue(callee, out handle);
        }

        public bool TryResolveAvaloniaBridge(string callee, out MemberReferenceHandle handle)
        {
            return _avaloniaBridgeMap.TryGetValue(callee, out handle);
        }

        public bool TryResolveRuntimeBridge(string callee, out MemberReferenceHandle handle)
        {
            if (callee.Contains("core4wtf84Wtf88to_owned", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_pathbuf_to_owned", out handle))
            {
                return true;
            }

            if (callee.Contains("RawVecInner", StringComparison.Ordinal)
                && callee.Contains("grow_amortized", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_alloc_raw_vec_grow_amortized", out handle))
            {
                return true;
            }

            if (callee.Contains("RawVecInner", StringComparison.Ordinal)
                && callee.Contains("try_allocate_in", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_alloc_raw_vec_try_allocate_in", out handle))
            {
                return true;
            }

            if (callee.Contains("Vec$LT$T$C$A$GT$", StringComparison.Ordinal)
                && callee.Contains("7reserve", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_alloc_vec_reserve_bytes", out handle))
            {
                return true;
            }

            // str::starts_with<char> monomorphization
            if (callee.Contains("impl$u20$str$GT$11starts_with", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_core_str_starts_with_char", out handle))
            {
                return true;
            }

            // str::ends_with<char> monomorphization
            if (callee.Contains("impl$u20$str$GT$9ends_with", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_core_str_ends_with_char", out handle))
            {
                return true;
            }

            // swap_nonoverlapping_chunks
            if (callee.Contains("swap_nonoverlapping_chunks", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_core_ptr_swap_nonoverlapping_chunks", out handle))
            {
                return true;
            }

            // core::str::count::do_count_chars
            if (callee.Contains("3str5count14do_count_chars", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_core_str_do_count_chars", out handle))
            {
                return true;
            }

            // core::str::count::char_count_general_case
            if (callee.Contains("3str5count23char_count_general_case", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_core_str_char_count_general_case", out handle))
            {
                return true;
            }

            // Cow<str>::deref returning {ptr, i64} (the 24-byte Cow variant)
            if (callee.Contains("Cow$LT$B$GT$", StringComparison.Ordinal)
                && callee.Contains("Deref$GT$5deref", StringComparison.Ordinal)
                && callee.Contains("hc3dd6b6198c46a26", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_cow_str_deref", out handle))
            {
                return true;
            }

            // Cow<B>::deref returning ptr (the 48-byte Cow variant)
            if (callee.Contains("Cow$LT$B$GT$", StringComparison.Ordinal)
                && callee.Contains("Deref$GT$5deref", StringComparison.Ordinal)
                && callee.Contains("haf21d018b912faa4", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_cow_bytes_deref", out handle))
            {
                return true;
            }

            // Cow<B>::default
            if (callee.Contains("Cow$LT$B$GT$", StringComparison.Ordinal)
                && callee.Contains("Default$GT$7default", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_cow_default", out handle))
            {
                return true;
            }

            // Cow<B>::clone
            if (callee.Contains("Cow$LT$B$GT$", StringComparison.Ordinal)
                && callee.Contains("Clone$GT$5clone", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_cow_clone", out handle))
            {
                return true;
            }

            // ScanError::new_str
            if (callee.Contains("ScanError7new_str", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_saphyr_scan_error_new_str", out handle))
            {
                return true;
            }

            // Event::empty_scalar (no params other than sret)
            if (callee.Contains("Event12empty_scalar", StringComparison.Ordinal)
                && !callee.Contains("with_anchor", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_saphyr_event_empty_scalar", out handle))
            {
                return true;
            }

            // Event::empty_scalar_with_anchor
            if (callee.Contains("Event24empty_scalar_with_anchor", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_saphyr_event_empty_scalar_with_anchor", out handle))
            {
                return true;
            }

            // Yaml::value_from_cow_and_metadata
            if (callee.Contains("Yaml27value_from_cow_and_metadata", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_saphyr_yaml_value_from_cow_and_metadata", out handle))
            {
                return true;
            }

            // Tag::is_yaml_core_schema
            if (callee.Contains("Tag19is_yaml_core_schema", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_saphyr_tag_is_yaml_core_schema", out handle))
            {
                return true;
            }

            // Vec<T,A>::drop
            if (callee.Contains("Vec$LT$T$C$A$GT$", StringComparison.Ordinal)
                && callee.Contains("Drop$GT$4drop", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_vec_drop", out handle))
            {
                return true;
            }

            // RawVec<T,A>::drop
            if (callee.Contains("RawVec$LT$T$C$A$GT$", StringComparison.Ordinal)
                && callee.Contains("Drop$GT$4drop", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_raw_vec_drop", out handle))
            {
                return true;
            }

            // SlicePartialEq::equal_same_length
            if (callee.Contains("SlicePartialEq", StringComparison.Ordinal)
                && callee.Contains("equal_same_length", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_slice_equal_same_length", out handle))
            {
                return true;
            }

            // std::thread::local::LocalKey<T>::with (hash random seed)
            if (callee.Contains("LocalKey$LT$T$GT$4with", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_thread_local_hash_keys", out handle))
            {
                return true;
            }

            // Hash random seed TLS initialization — returns ptr, need a different bridge
            // RUST_STD_INTERNAL_VAL3tls() -> ptr: returns TLS storage pointer
            if (callee.Contains("RUST_STD_INTERNAL_VAL3tls", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_tls_storage_slot", out handle))
            {
                return true;
            }

            // thread_local native lazy Storage get_or_init_slow(ptr, ptr) -> ptr
            if (callee.Contains("Storage$LT$T$C$D$GT$16get_or_init_slow", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_tls_get_or_init_slow", out handle))
            {
                return true;
            }

            // alloc::fmt::format::format_inner
            if (callee.Contains("alloc3fmt6format12format_inner", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_alloc_fmt_format_inner", out handle))
            {
                return true;
            }
            if (callee.Contains("5alloc3fmt6format12format_inner", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_alloc_fmt_format_inner", out handle))
            {
                return true;
            }

            // Zip iterator new
            if (callee.Contains("ZipImpl$LT$A$C$B$GT$$GT$3new", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_zip_iter_new", out handle))
            {
                return true;
            }

            // Yaml::into_tagged — takes (sret_ptr, node_ptr, tag_ptr), returns void
            // For now, let this fall through to unresolved (fills with zeros = Yaml::Null)
            // TODO: proper bridge that wraps node with tag

            // Box<T>::hash / OrderedFloat::hash
            if ((callee.Contains("Box$LT$T$C$A$GT$", StringComparison.Ordinal) || callee.Contains("OrderedFloat", StringComparison.Ordinal))
                && callee.Contains("Hash$GT$4hash", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_hash_no_op", out handle))
            {
                return true;
            }

            if (callee.Contains("core..wtf8..Wtf8", StringComparison.Ordinal)
                && callee.Contains("8to_owned", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_pathbuf_to_owned", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path9file_name", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_file_name", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path9file_stem", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_file_stem", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path9extension", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_extension", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path6parent", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_parent", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path10components", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_components", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path11is_absolute", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_is_absolute", out handle))
            {
                return true;
            }

            if (callee.Contains("std..path..Components", StringComparison.Ordinal)
                && callee.Contains("core..cmp..PartialEq", StringComparison.Ordinal)
                && callee.Contains("2eq", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_components_eq", out handle))
            {
                return true;
            }

            if (callee.Contains("std..path..PathBuf", StringComparison.Ordinal)
                && callee.Contains("core..cmp..PartialEq", StringComparison.Ordinal)
                && callee.Contains("2eq", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_eq", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path12_starts_with", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_starts_with", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path10_ends_with", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_ends_with", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path5_join", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_join", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path4join", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_join", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path15_with_extension", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_with_extension", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path4Path15_with_file_name", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_path_with_file_name", out handle))
            {
                return true;
            }

            if (callee.Contains("std4path", StringComparison.Ordinal)
                && (callee.Contains("PathBuf5__push", StringComparison.Ordinal)
                    || callee.Contains("PathBuf5_push", StringComparison.Ordinal))
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_pathbuf_push", out handle))
            {
                return true;
            }

            if (callee.Contains("std2fs14read_to_string5inner", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_fs_read_to_string_inner", out handle))
            {
                return true;
            }

            if (callee.Contains("core5slice6memchr14memchr_aligned", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_core_slice_memchr_aligned", out handle))
            {
                return true;
            }

            if (callee.Contains("std2io5stdio6_print", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_io_print", out handle))
            {
                return true;
            }

            if (callee.Contains("std2io5stdio7_eprint", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_io_eprint", out handle))
            {
                return true;
            }

            if (callee.Contains("std2io5stdio6stdout", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_io_stdout", out handle))
            {
                return true;
            }

            if (callee.Contains("Stdout", StringComparison.Ordinal)
                && callee.Contains("write_all", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_io_stdout_write_all", out handle))
            {
                return true;
            }

            if (callee.Contains("Stderr", StringComparison.Ordinal)
                && callee.Contains("write_all", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_io_stderr_write_all", out handle))
            {
                return true;
            }

            if (callee.Contains("Stdout", StringComparison.Ordinal)
                && callee.Contains("5flush", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_io_stdout_flush", out handle))
            {
                return true;
            }

            if (callee.Contains("std4time7Instant3now", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_time_instant_now", out handle))
            {
                return true;
            }

            if (callee.Contains("std4time7Instant7elapsed", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_time_instant_elapsed", out handle))
            {
                return true;
            }

            if (callee.Contains("std3env11current_dir", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_env_current_dir", out handle))
            {
                return true;
            }

            if (callee.Contains("std3env8temp_dir", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_env_temp_dir", out handle))
            {
                return true;
            }

            if (callee.Contains("std3env3var", StringComparison.Ordinal)
                && _runtimeBridgeMap.TryGetValue("rustlyn_std_env_var", out handle))
            {
                return true;
            }

            return _runtimeBridgeMap.TryGetValue(callee, out handle);
        }

        public bool RuntimeBridgeReturnsViaSret(string callee)
        {
            return TryResolveRuntimeBridge(callee, out var handle)
                && _runtimeBridgeSretHandles.Contains(handle);
        }

        private static MemberReferenceHandle AddStaticMethod(MetadataBuilder mb, TypeReferenceHandle parent, string name, BlobHandle signature)
        {
            return mb.AddMemberReference(parent, mb.GetOrAddString(name), signature);
        }

        private static Dictionary<string, MemberReferenceHandle> BuildRuntimeBridgeMap(
            MetadataBuilder mb,
            TypeReferenceHandle runtimeBridgeHelpers,
            IReadOnlyList<BindingManifestDocument> bindingManifests,
            out HashSet<MemberReferenceHandle> sretHandles)
        {
            var methods = typeof(RuntimeBridgeHelpers)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(static method => method.Name, StringComparer.Ordinal);
            var handles = new Dictionary<string, MemberReferenceHandle>(StringComparer.Ordinal);
            foreach (var method in methods.Values)
            {
                handles[method.Name] = AddStaticMethod(
                    mb,
                    runtimeBridgeHelpers,
                    method.Name,
                    EncodeReflectedMethodSig(mb, method));
            }

            var bridgeMap = new Dictionary<string, MemberReferenceHandle>(StringComparer.Ordinal);
            sretHandles = [];
            var bindingManifest = BindingManifestDocument.FromSurface(BindingSurface.CreateTinyBclSurface());
            foreach (var binding in bindingManifest.Bindings)
            {
                if (!handles.TryGetValue(binding.Helper, out var handle))
                {
                    throw new InvalidOperationException($"Generated binding symbol '{binding.Symbol}' targets missing runtime bridge helper '{binding.Helper}'.");
                }

                bridgeMap[binding.Symbol] = handle;
                if (RuntimeBridgeUsesSret(methods[binding.Helper]))
                {
                    sretHandles.Add(handle);
                }
            }

            var avaloniaManifest = ExternalPackageBindingSurfaces.CreateAvaloniaHelloManifest();
            foreach (var binding in avaloniaManifest.Bindings)
            {
                if (!handles.TryGetValue(binding.Helper, out var handle))
                {
                    throw new InvalidOperationException($"Generated Avalonia binding symbol '{binding.Symbol}' targets missing runtime bridge helper '{binding.Helper}'.");
                }

                bridgeMap[binding.Symbol] = handle;
                if (RuntimeBridgeUsesSret(methods[binding.Helper]))
                {
                    sretHandles.Add(handle);
                }
            }

            foreach (var (symbol, methodName) in RuntimeBridgeAliases)
            {
                if (handles.TryGetValue(methodName, out var handle))
                {
                    bridgeMap[symbol] = handle;
                    if (RuntimeBridgeUsesSret(methods[methodName]))
                    {
                        sretHandles.Add(handle);
                    }
                }
            }

            foreach (var alias in RustStdShimManifest.Aliases)
            {
                if (handles.TryGetValue(alias.HelperMethodName, out var handle))
                {
                    bridgeMap[alias.Symbol] = handle;
                    if (RuntimeBridgeUsesSret(methods[alias.HelperMethodName]))
                    {
                        sretHandles.Add(handle);
                    }
                }
            }

            foreach (var manifest in bindingManifests)
            {
                foreach (var binding in manifest.Bindings)
                {
                    if (!handles.TryGetValue(binding.Helper, out var handle))
                    {
                        handle = AddStaticMethod(
                            mb,
                            runtimeBridgeHelpers,
                            binding.Helper,
                            EncodeManifestBindingSig(mb, binding));
                        handles[binding.Helper] = handle;
                    }

                    bridgeMap[binding.Symbol] = handle;
                    if (ManifestBindingUsesSret(binding))
                    {
                        sretHandles.Add(handle);
                    }
                }
            }

            foreach (var method in methods.Values.Where(static method => method.Name.StartsWith("Bindgen", StringComparison.Ordinal)))
            {
                var handle = handles[method.Name];
                bridgeMap["rustlyn_" + ToSnakeCase(method.Name)] = handle;
                if (RuntimeBridgeUsesSret(method))
                {
                    sretHandles.Add(handle);
                }
            }

            return bridgeMap;
        }

        private static BlobHandle EncodeManifestBindingSig(MetadataBuilder mb, BindingManifestBinding binding)
            => EncodeBridgeMethodSig(
                mb,
                ToBridgeSignatureType(binding.ReturnType),
                binding.Parameters.Select(static parameter => ToBridgeSignatureType(parameter.Type)).ToArray());

        private static string ToBridgeSignatureType(string managedGlueType)
        {
            return managedGlueType switch
            {
                "int" => "i32",
                "long" => "i64",
                "float" => "float",
                "double" => "double",
                "IntPtr" => "ptr",
                "void" => "void",
                _ => throw new NotSupportedException($"Managed glue type '{managedGlueType}' is not supported in runtime binding manifests.")
            };
        }

        private static bool ManifestBindingUsesSret(BindingManifestBinding binding)
            => string.Equals(binding.ReturnType, "void", StringComparison.Ordinal)
                && binding.Parameters.Count > 0
                && string.Equals(binding.Parameters[0].Type, "IntPtr", StringComparison.Ordinal);

        private static bool RuntimeBridgeUsesSret(MethodInfo method)
        {
            var parameters = method.GetParameters();
            return method.ReturnType == typeof(void)
                && parameters.Length > 0
                && parameters[0].ParameterType == typeof(IntPtr);
        }

        private static readonly (string Symbol, string MethodName)[] RuntimeBridgeAliases =
        [
            ("rustlyn_dotnet_is_windows", nameof(RuntimeBridgeHelpers.IsWindows)),
            ("rustlyn_dotnet_directory_separator_char", nameof(RuntimeBridgeHelpers.DirectorySeparatorChar)),
            ("rustlyn_dotnet_path_separator_char", nameof(RuntimeBridgeHelpers.PathSeparatorChar)),
            ("rustlyn_dotnet_newline_len", nameof(RuntimeBridgeHelpers.NewlineLength)),
            ("rustlyn_dotnet_bitops_popcount_u32", nameof(RuntimeBridgeHelpers.PopCountU32)),
            ("rustlyn_dotnet_math_max_i32", nameof(RuntimeBridgeHelpers.MathMaxI32)),
            ("rustlyn_dotnet_math_min_i32", nameof(RuntimeBridgeHelpers.MathMinI32)),
            ("rustlyn_alloc_raw_vec_grow_amortized", nameof(RuntimeBridgeHelpers.RustRawVecGrowAmortized)),
            ("rustlyn_alloc_raw_vec_try_allocate_in", nameof(RuntimeBridgeHelpers.RustRawVecTryAllocateIn)),
            ("rustlyn_alloc_vec_reserve_bytes", nameof(RuntimeBridgeHelpers.RustVecReserveBytes)),
            ("memcmp", nameof(RuntimeBridgeHelpers.Memcmp)),
            ("llvm.ucmp.i8.i64", nameof(RuntimeBridgeHelpers.LlvmUnsignedCompareI8I64)),
            ("rustlyn_core_str_starts_with_char", nameof(RuntimeBridgeHelpers.CoreStrStartsWithChar)),
            ("rustlyn_core_str_ends_with_char", nameof(RuntimeBridgeHelpers.CoreStrEndsWithChar)),
            ("rustlyn_core_ptr_swap_nonoverlapping_chunks", nameof(RuntimeBridgeHelpers.SwapNonoverlappingChunks)),
            ("rustlyn_core_slice_memchr_aligned", nameof(RuntimeBridgeHelpers.CoreSliceMemchrAligned)),
            ("rustlyn_core_str_do_count_chars", nameof(RuntimeBridgeHelpers.CoreStrDoCountChars)),
            ("rustlyn_core_str_char_count_general_case", nameof(RuntimeBridgeHelpers.CoreStrCharCountGeneralCase)),
            ("rustlyn_cow_str_deref", nameof(RuntimeBridgeHelpers.CowStrDeref)),
            ("rustlyn_cow_default", nameof(RuntimeBridgeHelpers.CowDefault)),
            ("rustlyn_cow_bytes_deref", nameof(RuntimeBridgeHelpers.CowBytesDeref)),
            ("rustlyn_cow_clone", nameof(RuntimeBridgeHelpers.CowClone)),
            ("rustlyn_saphyr_scan_error_new_str", nameof(RuntimeBridgeHelpers.ScanErrorNewStr)),
            ("rustlyn_saphyr_event_empty_scalar", nameof(RuntimeBridgeHelpers.EventEmptyScalar)),
            ("rustlyn_saphyr_event_empty_scalar_with_anchor", nameof(RuntimeBridgeHelpers.EventEmptyScalarWithAnchor)),
            ("rustlyn_saphyr_yaml_value_from_cow_and_metadata", nameof(RuntimeBridgeHelpers.YamlValueFromCowAndMetadata)),
            ("rustlyn_saphyr_tag_is_yaml_core_schema", nameof(RuntimeBridgeHelpers.TagIsYamlCoreSchema)),
            ("rustlyn_vec_drop", nameof(RuntimeBridgeHelpers.VecDrop)),
            ("rustlyn_raw_vec_drop", nameof(RuntimeBridgeHelpers.RawVecDrop)),
            ("rustlyn_slice_equal_same_length", nameof(RuntimeBridgeHelpers.SliceEqualSameLength)),
            ("rustlyn_thread_local_hash_keys", nameof(RuntimeBridgeHelpers.ThreadLocalHashKeys)),
            ("rustlyn_tls_storage_slot", nameof(RuntimeBridgeHelpers.TlsStorageSlot)),
            ("rustlyn_tls_get_or_init_slow", nameof(RuntimeBridgeHelpers.TlsGetOrInitSlow)),
            ("rustlyn_alloc_fmt_format_inner", nameof(RuntimeBridgeHelpers.FormatInner)),
            ("rustlyn_zip_iter_new", nameof(RuntimeBridgeHelpers.ZipIterNew)),
            ("rustlyn_hash_no_op", nameof(RuntimeBridgeHelpers.HashNoOp)),
            ("rustlyn_dotnet_command_line_arg_count", nameof(RuntimeBridgeHelpers.CommandLineArgCount)),
            ("rustlyn_dotnet_command_line_arg_utf8_len", nameof(RuntimeBridgeHelpers.Utf8CommandLineArgLength)),
            ("rustlyn_dotnet_copy_command_line_arg_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8CommandLineArg)),
            ("rustlyn_dotnet_console_write_line_utf8", nameof(RuntimeBridgeHelpers.ConsoleWriteLineUtf8)),
            ("rustlyn_dotnet_console_write_prefixed_line_utf8", nameof(RuntimeBridgeHelpers.ConsoleWritePrefixedLineUtf8)),
            ("rustlyn_dotnet_console_write_path_line_utf8", nameof(RuntimeBridgeHelpers.ConsoleWritePathLineUtf8)),
            ("rustlyn_dotnet_console_write_numbered_line_utf8", nameof(RuntimeBridgeHelpers.ConsoleWriteNumberedLineUtf8)),
            ("rustlyn_dotnet_console_write_i32", nameof(RuntimeBridgeHelpers.ConsoleWriteI32)),
            ("rustlyn_dotnet_console_write_path_count_utf8", nameof(RuntimeBridgeHelpers.ConsoleWritePathCountUtf8)),
            ("rustlyn_dotnet_file_read_all_lines_count", nameof(RuntimeBridgeHelpers.Utf8ReadAllLinesCount)),
            ("rustlyn_dotnet_file_read_all_lines_line_utf8_len", nameof(RuntimeBridgeHelpers.Utf8ReadAllLinesLineLength)),
            ("rustlyn_dotnet_file_copy_read_all_lines_line_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8ReadAllLinesLine)),
            ("rustlyn_dotnet_path_get_root_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathGetRootLengthUtf8)),
            ("rustlyn_dotnet_path_copy_root_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathGetRoot)),
            ("rustlyn_dotnet_path_get_full_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathGetFullPathLengthUtf8)),
            ("rustlyn_dotnet_path_copy_full_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathGetFullPath)),
            ("rustlyn_dotnet_path_get_directory_name_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathGetDirectoryNameLengthUtf8)),
            ("rustlyn_dotnet_path_copy_directory_name_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathGetDirectoryName)),
            ("rustlyn_dotnet_path_get_relative_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathGetRelativeLengthUtf8)),
            ("rustlyn_dotnet_path_copy_relative_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathGetRelative)),
            ("rustlyn_dotnet_documents_utf8_len", nameof(RuntimeBridgeHelpers.Utf8DocumentsLength)),
            ("rustlyn_dotnet_copy_documents_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8Documents)),
            ("rustlyn_dotnet_temp_path_utf8_len", nameof(RuntimeBridgeHelpers.Utf8TempPathLength)),
            ("rustlyn_dotnet_copy_temp_path_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8TempPath)),
            ("rustlyn_dotnet_user_profile_utf8_len", nameof(RuntimeBridgeHelpers.Utf8UserProfileLength)),
            ("rustlyn_dotnet_copy_user_profile_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8UserProfile)),
            ("rustlyn_dotnet_current_directory_utf8_len", nameof(RuntimeBridgeHelpers.Utf8CurrentDirectoryLength)),
            ("rustlyn_dotnet_copy_current_directory_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8CurrentDirectory)),
            ("rustlyn_dotnet_path_combine3_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathCombine3LengthUtf8)),
            ("rustlyn_dotnet_path_copy_combine3_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathCombine3)),
            ("rustlyn_dotnet_path_combine_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathCombineLengthUtf8)),
            ("rustlyn_dotnet_path_copy_combine_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathCombine)),
            ("rustlyn_dotnet_path_change_extension_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathChangeExtensionLengthUtf8)),
            ("rustlyn_dotnet_path_copy_change_extension_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathChangeExtension)),
            ("rustlyn_dotnet_path_get_file_name_without_extension_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathGetFileNameWithoutExtensionLengthUtf8)),
            ("rustlyn_dotnet_path_copy_file_name_without_extension_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathGetFileNameWithoutExtension)),
            ("rustlyn_dotnet_path_get_file_name_utf8_len", nameof(RuntimeBridgeHelpers.Utf8PathGetFileNameLengthUtf8)),
            ("rustlyn_dotnet_path_copy_file_name_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8PathGetFileName)),
            ("rustlyn_dotnet_path_get_file_name_len", nameof(RuntimeBridgeHelpers.Utf8PathGetFileNameLength)),
            ("rustlyn_dotnet_string_replace_utf8_len", nameof(RuntimeBridgeHelpers.Utf8StringReplaceLength)),
            ("rustlyn_dotnet_string_copy_replace_utf8", nameof(RuntimeBridgeHelpers.CopyUtf8StringReplace)),
            ("rustlyn_dotnet_string_contains", nameof(RuntimeBridgeHelpers.Utf8StringContains)),
            ("rustlyn_dotnet_string_index_of", nameof(RuntimeBridgeHelpers.Utf8StringIndexOf))
        ];

        private static BlobHandle EncodeReflectedMethodSig(MetadataBuilder mb, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(parameters.Length, out var ret, out var parms);
            EncodeReflectedSignatureType(ret, method.ReturnType);
            foreach (var parameter in parameters)
            {
                EncodeReflectedSignatureType(parms.AddParameter().Type(), parameter.ParameterType);
            }

            return mb.GetOrAddBlob(blob);
        }

        private static void EncodeReflectedSignatureType(ReturnTypeEncoder encoder, Type type)
        {
            if (type == typeof(void))
            {
                encoder.Void();
                return;
            }

            EncodeReflectedSignatureType(encoder.Type(), type);
        }

        private static void EncodeReflectedSignatureType(SignatureTypeEncoder encoder, Type type)
        {
            if (type == typeof(int))
                encoder.Int32();
            else if (type == typeof(uint))
                encoder.UInt32();
            else if (type == typeof(byte))
                encoder.Byte();
            else if (type == typeof(sbyte))
                encoder.SByte();
            else if (type == typeof(long))
                encoder.Int64();
            else if (type == typeof(ulong))
                encoder.UInt64();
            else if (type == typeof(float))
                encoder.Single();
            else if (type == typeof(double))
                encoder.Double();
            else if (type == typeof(bool))
                encoder.Boolean();
            else if (type == typeof(nint) || type == typeof(IntPtr))
                encoder.IntPtr();
            else if (type == typeof(string))
                encoder.String();
            else
                throw new NotSupportedException($"Unsupported runtime bridge signature type '{type.FullName}'.");
        }

        private static string ToSnakeCase(string value)
        {
            var builder = new System.Text.StringBuilder(value.Length + 16);
            for (var i = 0; i < value.Length; i++)
            {
                var ch = value[i];
                if (char.IsUpper(ch))
                {
                    if (i > 0 && (char.IsLower(value[i - 1]) || char.IsDigit(value[i - 1]) || (i + 1 < value.Length && char.IsLower(value[i + 1]))))
                        builder.Append('_');
                    builder.Append(char.ToLowerInvariant(ch));
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString();
        }

        private static BlobHandle EncodeBridgeMethodSig(MetadataBuilder mb, string returnType, params string[] parameterTypes)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(parameterTypes.Length, out var ret, out var parms);
            if (string.Equals(returnType, "void", StringComparison.Ordinal))
                ret.Void();
            else
                EncodeSignatureType(ret.Type(), returnType);

            foreach (var parameterType in parameterTypes)
            {
                EncodeSignatureType(parms.AddParameter().Type(), parameterType);
            }

            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeU32ToI32Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(1, out var ret, out var parms);
            ret.Type().Int32();
            parms.AddParameter().Type().UInt32();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeU64ToI32Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(1, out var ret, out var parms);
            ret.Type().Int32();
            parms.AddParameter().Type().UInt64();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeI32ToI32Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(1, out var ret, out var parms);
            ret.Type().Int32();
            parms.AddParameter().Type().Int32();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeI64ToI64Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(1, out var ret, out var parms);
            ret.Type().Int64();
            parms.AddParameter().Type().Int64();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeF64ToF64Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(1, out var ret, out var parms);
            ret.Type().Double();
            parms.AddParameter().Type().Double();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeF64F64ToF64Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(2, out var ret, out var parms);
            ret.Type().Double();
            parms.AddParameter().Type().Double();
            parms.AddParameter().Type().Double();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeF32ToF32Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(1, out var ret, out var parms);
            ret.Type().Single();
            parms.AddParameter().Type().Single();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeI32I32ToI32Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(2, out var ret, out var parms);
            ret.Type().Int32();
            parms.AddParameter().Type().Int32();
            parms.AddParameter().Type().Int32();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeI64I64ToI64Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(2, out var ret, out var parms);
            ret.Type().Int64();
            parms.AddParameter().Type().Int64();
            parms.AddParameter().Type().Int64();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeU32U32ToU32Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(2, out var ret, out var parms);
            ret.Type().UInt32();
            parms.AddParameter().Type().UInt32();
            parms.AddParameter().Type().UInt32();
            return mb.GetOrAddBlob(blob);
        }

        private static BlobHandle EncodeU64U64ToU64Sig(MetadataBuilder mb)
        {
            var blob = new BlobBuilder();
            new BlobEncoder(blob).MethodSignature().Parameters(2, out var ret, out var parms);
            ret.Type().UInt64();
            parms.AddParameter().Type().UInt64();
            parms.AddParameter().Type().UInt64();
            return mb.GetOrAddBlob(blob);
        }
    }
}
