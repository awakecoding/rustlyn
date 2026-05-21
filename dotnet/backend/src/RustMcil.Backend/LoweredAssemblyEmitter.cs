using Mono.Cecil;
using Mono.Cecil.Cil;
using RustMcil.Bindings;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace RustMcil.Backend;

public static class LoweredAssemblyEmitter
{
    private const string GeneratedTypeName = "RustMcil.GeneratedModule";
    private const string GeneratedEntrypointTypeName = "GeneratedEntryPoint";
    private static readonly IReadOnlyDictionary<string, string> GeneratedBindingRuntimeHelpers = BindingSurface
        .CreateTinyBclSurface()
        .ManagedGlueBindings
        .ToDictionary(static binding => binding.Symbol, static binding => binding.RuntimeBridgeHelperMethodName, StringComparer.Ordinal);

    public static void EmitBitcode(string artifactPath, string outputAssemblyPath, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputAssemblyPath);

        var loweredModule = LoweredIrLowerer.LowerBitcode(artifactPath, llvmRoot);
        EmitModule(loweredModule, outputAssemblyPath);
    }

    public static void EmitModule(LoweredModule loweredModule, string outputAssemblyPath)
    {
        ArgumentNullException.ThrowIfNull(loweredModule);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputAssemblyPath);

        var outputFullPath = Path.GetFullPath(outputAssemblyPath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath) ?? throw new InvalidOperationException("Output directory could not be determined."));

        var assemblyName = Path.GetFileNameWithoutExtension(outputFullPath);
        var emittedFunctions = GetReachableFunctions(loweredModule.Functions);
        var consoleEntrypoint = TrySelectConsoleEntrypoint(emittedFunctions);
        var requiresAvaloniaSupport = RequiresAvaloniaSupport(emittedFunctions);
        var assembly = AssemblyDefinition.CreateAssembly(
            new AssemblyNameDefinition(assemblyName, new Version(1, 0, 0, 0)),
            assemblyName,
            consoleEntrypoint is null ? ModuleKind.Dll : ModuleKind.Console);
        var module = assembly.MainModule;

        var generatedType = new TypeDefinition(
            "RustMcil",
            "GeneratedModule",
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class,
            module.TypeSystem.Object);

        module.Types.Add(generatedType);

        var vectorHelpers = EmitVectorHelpers(module, generatedType);
        var memoryHelpers = EmitMemoryHelpers(module, generatedType);
        var pointerBackedGlobals = CollectPointerBackedGlobals(emittedFunctions);

        var methodMap = new Dictionary<string, MethodDefinition>(StringComparer.Ordinal);
        var fieldMap = new Dictionary<string, FieldDefinition>(StringComparer.Ordinal);
        var globalMap = loweredModule.Globals.ToDictionary(global => global.Name, StringComparer.Ordinal);

        foreach (var global in loweredModule.Globals)
        {
            var field = new FieldDefinition(
                global.Name,
                FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly,
                ResolveGlobalFieldType(module, global, pointerBackedGlobals.Contains(global.Name)));

            generatedType.Fields.Add(field);
            fieldMap.Add(global.Name, field);
        }

        if (loweredModule.Globals.Count > 0)
        {
            EmitTypeInitializer(module, generatedType, loweredModule.Globals, fieldMap);
        }

        foreach (var function in emittedFunctions)
        {
            var method = new MethodDefinition(
                function.Name,
                MethodAttributes.Public | MethodAttributes.Static,
                ResolveTypeReference(module, function.ReturnType));

            foreach (var parameter in function.Parameters)
            {
                method.Parameters.Add(new ParameterDefinition(parameter.Name, ParameterAttributes.None, ResolveTypeReference(module, parameter.Type)));
            }

            generatedType.Methods.Add(method);
            methodMap.Add(function.Name, method);
        }

        foreach (var function in emittedFunctions)
        {
            EmitFunctionBody(module, methodMap[function.Name], function, methodMap, fieldMap, globalMap, vectorHelpers, memoryHelpers);
        }

        if (consoleEntrypoint is not null)
        {
            EmitConsoleEntrypoint(module, methodMap, consoleEntrypoint, requiresAvaloniaSupport);
        }

        assembly.Write(outputFullPath);

        if (consoleEntrypoint is not null)
        {
            WriteRuntimeConfig(outputFullPath);
            CopyRuntimeSupportAssemblies(outputFullPath, requiresAvaloniaSupport);
        }
    }

    private static bool RequiresAvaloniaSupport(IReadOnlyList<LoweredFunction> functions)
        => functions.SelectMany(static function => function.Blocks)
            .SelectMany(static block => block.Instructions)
            .OfType<LoweredCallInstruction>()
            .Any(static call => call.Callee.StartsWith("rust_mcil_avalonia_", StringComparison.Ordinal));

    private static LoweredFunction? TrySelectConsoleEntrypoint(IReadOnlyList<LoweredFunction> functions)
    {
        var mainFunction = functions.SingleOrDefault(static function => string.Equals(function.Name, "main", StringComparison.Ordinal));
        if (mainFunction is null)
        {
            return null;
        }

        if (mainFunction.Parameters.Count != 0)
        {
            return null;
        }

        if (!string.Equals(mainFunction.ReturnType, "void", StringComparison.Ordinal)
            && !string.Equals(mainFunction.ReturnType, "i32", StringComparison.Ordinal))
        {
            return null;
        }

        return mainFunction;
    }

    private static void EmitConsoleEntrypoint(ModuleDefinition module, IReadOnlyDictionary<string, MethodDefinition> methodMap, LoweredFunction consoleEntrypoint, bool requiresStaThread)
    {
        var generatedEntrypointType = new TypeDefinition(
            "RustMcil",
            GeneratedEntrypointTypeName,
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class,
            module.TypeSystem.Object);

        module.Types.Add(generatedEntrypointType);

        var wrapperMethod = new MethodDefinition(
            "Main",
            MethodAttributes.Public | MethodAttributes.Static,
            module.TypeSystem.Int32);

        wrapperMethod.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.None, module.ImportReference(typeof(string[]))));
        if (requiresStaThread)
        {
            var staThreadConstructor = typeof(STAThreadAttribute).GetConstructor(Type.EmptyTypes)
                ?? throw new InvalidOperationException("STAThreadAttribute constructor could not be resolved.");
            wrapperMethod.CustomAttributes.Add(new CustomAttribute(module.ImportReference(staThreadConstructor)));
        }

        wrapperMethod.Body.InitLocals = true;
        wrapperMethod.Body.MaxStackSize = 2;
        generatedEntrypointType.Methods.Add(wrapperMethod);

        var il = wrapperMethod.Body.GetILProcessor();
        var targetMethod = methodMap[consoleEntrypoint.Name];
        il.Append(il.Create(OpCodes.Call, targetMethod));

        if (string.Equals(consoleEntrypoint.ReturnType, "void", StringComparison.Ordinal))
        {
            il.Append(il.Create(OpCodes.Ldc_I4_0));
        }

        il.Append(il.Create(OpCodes.Ret));
        module.EntryPoint = wrapperMethod;
    }

    private static void WriteRuntimeConfig(string outputAssemblyPath)
    {
        var runtimeConfigPath = Path.ChangeExtension(outputAssemblyPath, ".runtimeconfig.json");
        var runtimeConfig = new
        {
            runtimeOptions = new
            {
                tfm = $"net{Environment.Version.Major}.{Environment.Version.Minor}",
                framework = new
                {
                    name = "Microsoft.NETCore.App",
                    version = GetCurrentRuntimeVersion()
                },
                rollForward = "LatestPatch"
            }
        };

        File.WriteAllText(
            runtimeConfigPath,
            JsonSerializer.Serialize(runtimeConfig, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static string GetCurrentRuntimeVersion()
    {
        var description = RuntimeInformation.FrameworkDescription;
        var separatorIndex = description.LastIndexOf(' ');
        if (separatorIndex >= 0 && separatorIndex < description.Length - 1)
        {
            return description[(separatorIndex + 1)..];
        }

        return Environment.Version.ToString();
    }

    private static void CopyRuntimeSupportAssemblies(string outputAssemblyPath, bool requiresAvaloniaSupport)
    {
        CopySupportAssembly(typeof(RuntimeBridgeHelpers).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(RustMcil.Interop.ManagedInteropRuntime).Assembly.Location, outputAssemblyPath);

        if (requiresAvaloniaSupport)
        {
            CopyAvaloniaSupportFiles(outputAssemblyPath);
        }
    }

    private static void CopySupportAssembly(string supportAssemblyPath, string outputAssemblyPath)
    {
        if (string.IsNullOrWhiteSpace(supportAssemblyPath) || !File.Exists(supportAssemblyPath))
        {
            return;
        }

        var destinationPath = Path.Combine(
            Path.GetDirectoryName(outputAssemblyPath) ?? throw new InvalidOperationException("Output directory could not be determined."),
            Path.GetFileName(supportAssemblyPath));

        if (string.Equals(Path.GetFullPath(destinationPath), Path.GetFullPath(supportAssemblyPath), StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        File.Copy(supportAssemblyPath, destinationPath, overwrite: true);
    }

    private static void CopyAvaloniaSupportFiles(string outputAssemblyPath)
    {
        var supportAssemblyPath = typeof(RustMcil.AvaloniaSupport.AvaloniaBridge).Assembly.Location;
        if (string.IsNullOrWhiteSpace(supportAssemblyPath) || !File.Exists(supportAssemblyPath))
        {
            return;
        }

        var sourceDirectory = Path.GetDirectoryName(supportAssemblyPath)
            ?? throw new InvalidOperationException("Avalonia support assembly directory could not be determined.");
        var outputDirectory = Path.GetDirectoryName(outputAssemblyPath)
            ?? throw new InvalidOperationException("Output directory could not be determined.");

        foreach (var sourcePath in Directory.GetFiles(sourceDirectory, "*.dll", SearchOption.TopDirectoryOnly))
        {
            CopyFileToDirectory(sourcePath, outputDirectory);
        }

        var runtimesDirectory = Path.Combine(sourceDirectory, "runtimes");
        if (Directory.Exists(runtimesDirectory))
        {
            CopyDirectory(runtimesDirectory, Path.Combine(outputDirectory, "runtimes"));
            CopyCurrentRuntimeNativeAssets(runtimesDirectory, outputDirectory);
        }
    }

    private static void CopyCurrentRuntimeNativeAssets(string runtimesDirectory, string outputDirectory)
    {
        var nativeDirectory = Path.Combine(runtimesDirectory, RuntimeInformation.RuntimeIdentifier, "native");
        if (!Directory.Exists(nativeDirectory))
        {
            return;
        }

        foreach (var sourcePath in Directory.GetFiles(nativeDirectory, "*", SearchOption.TopDirectoryOnly))
        {
            CopyFileToDirectory(sourcePath, outputDirectory);
        }
    }

    private static void CopyFileToDirectory(string sourcePath, string outputDirectory)
    {
        var destinationPath = Path.Combine(outputDirectory, Path.GetFileName(sourcePath));
        if (string.Equals(Path.GetFullPath(destinationPath), Path.GetFullPath(sourcePath), StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        File.Copy(sourcePath, destinationPath, overwrite: true);
    }

    private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        Directory.CreateDirectory(destinationDirectory);
        foreach (var sourcePath in Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceDirectory, sourcePath);
            var destinationPath = Path.Combine(destinationDirectory, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? destinationDirectory);
            File.Copy(sourcePath, destinationPath, overwrite: true);
        }
    }

    private static IReadOnlyList<LoweredFunction> GetReachableFunctions(IReadOnlyList<LoweredFunction> functions)
    {
        var roots = functions.Where(static function => IsExportedRoot(function.Name)).ToArray();
        if (roots.Length == 0)
        {
            return functions;
        }

        var functionMap = functions.ToDictionary(function => function.Name, StringComparer.Ordinal);
        var reachable = new HashSet<string>(StringComparer.Ordinal);
        var pending = new Stack<string>(roots.Select(static function => function.Name).Reverse());

        while (pending.Count > 0)
        {
            var functionName = pending.Pop();
            if (!reachable.Add(functionName)
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
        }

        return functions.Where(function => reachable.Contains(function.Name)).ToArray();
    }

    private static bool IsExportedRoot(string functionName)
    {
        return !functionName.StartsWith("_ZN", StringComparison.Ordinal)
            && !functionName.StartsWith("_R", StringComparison.Ordinal);
    }

    private static bool ShouldSkipCallResultWidthNormalization(LoweredCallInstruction call)
    {
        if (!TryGetIntegerBitWidth(call.ReturnType, out var width) || width >= 32)
        {
            return false;
        }

        return call.Callee switch
        {
            "llvm.smax.i8" => true,
            "llvm.smin.i8" => true,
            "llvm.smax.i16" => true,
            "llvm.smin.i16" => true,
            "llvm.vector.reduce.smax.v16i8" => true,
            "llvm.vector.reduce.smin.v16i8" => true,
            "llvm.vector.reduce.smax.v8i8" => true,
            "llvm.vector.reduce.smin.v8i8" => true,
            "llvm.vector.reduce.smax.v8i16" => true,
            "llvm.vector.reduce.smin.v8i16" => true,
            "llvm.vector.reduce.smax.v4i16" => true,
            "llvm.vector.reduce.smin.v4i16" => true,
            _ => false
        };
    }

    private static bool ShouldStubPreconditionCheck(LoweredFunction function)
    {
        return string.Equals(function.ReturnType, "void", StringComparison.Ordinal)
            && function.Name.Contains("precondition_check", StringComparison.Ordinal);
    }

    private static void EmitFunctionBody(ModuleDefinition module, MethodDefinition method, LoweredFunction function, IReadOnlyDictionary<string, MethodDefinition> methodMap, IReadOnlyDictionary<string, FieldDefinition> fieldMap, IReadOnlyDictionary<string, LoweredGlobal> globalMap, VectorHelperMethods vectorHelpers, MemoryHelperMethods memoryHelpers)
    {
        if (function.Blocks.Count == 0)
        {
            throw new NotSupportedException($"Function '{function.Name}' does not contain any blocks.");
        }

        var entryBlock = function.Blocks[0];
        if (!string.Equals(entryBlock.Name, "start", StringComparison.Ordinal) && !string.Equals(entryBlock.Name, "entry", StringComparison.Ordinal))
        {
            throw new NotSupportedException($"Function '{function.Name}' starts in block '{entryBlock.Name}'. The emitter expects the first block to be 'start' or 'entry'.");
        }

        if (ShouldStubPreconditionCheck(function))
        {
            method.Body.InitLocals = true;
            method.Body.GetILProcessor().Append(method.Body.GetILProcessor().Create(OpCodes.Ret));
            return;
        }

        method.Body.InitLocals = true;
        method.Body.MaxStackSize = 16;
        var il = method.Body.GetILProcessor();
        var locals = new Dictionary<string, VariableDefinition>(StringComparer.Ordinal);
        var parameters = method.Parameters.ToDictionary(parameter => parameter.Name ?? string.Empty, StringComparer.Ordinal);
        var blockLabels = function.Blocks.ToDictionary(block => block.Name, _ => il.Create(OpCodes.Nop), StringComparer.Ordinal);
        var phiByBlock = function.Blocks.ToDictionary(
            block => block.Name,
            block => block.Instructions.OfType<LoweredPhiInstruction>().ToArray(),
            StringComparer.Ordinal);
        var addressMap = new Dictionary<string, LoweredGetElementPointerInstruction>(StringComparer.Ordinal);
        var memoryAliases = new Dictionary<string, string>(StringComparer.Ordinal);
        var rawAddressAllocas = CollectRawAddressAllocas(function);

        PredeclareFunctionLocals(method, locals, function, rawAddressAllocas);

        foreach (var block in function.Blocks)
        {
            il.Append(blockLabels[block.Name]);

            for (var instructionIndex = 0; instructionIndex < block.Instructions.Count; instructionIndex++)
            {
                var instruction = block.Instructions[instructionIndex];
                switch (instruction)
                {
                    case LoweredBinaryInstruction binary:
                        if (IsSupportedVectorType(binary.Type))
                        {
                            EmitVectorBinaryOperation(il, parameters, locals, binary, function.Name, vectorHelpers);
                            break;
                        }

                        LoadValue(il, parameters, locals, binary.Type, binary.Left);
                        LoadValue(il, parameters, locals, binary.Type, binary.Right);
                        if (binary.Operation is "shl" or "lshr" or "ashr")
                        {
                            il.Append(il.Create(OpCodes.Conv_I4));
                        }

                        il.Append(binary.Operation switch
                        {
                            "add" => il.Create(OpCodes.Add),
                            "sub" => il.Create(OpCodes.Sub),
                            "mul" => il.Create(OpCodes.Mul),
                            "sdiv" => il.Create(OpCodes.Div),
                            "udiv" => il.Create(OpCodes.Div_Un),
                            "srem" => il.Create(OpCodes.Rem),
                            "urem" => il.Create(OpCodes.Rem_Un),
                            "and" => il.Create(OpCodes.And),
                            "or" => il.Create(OpCodes.Or),
                            "xor" => il.Create(OpCodes.Xor),
                            "shl" => il.Create(OpCodes.Shl),
                            "lshr" => il.Create(OpCodes.Shr_Un),
                            "ashr" => il.Create(OpCodes.Shr),
                            _ => throw new NotSupportedException($"Unsupported binary operation '{binary.Operation}' in function '{function.Name}'.")
                        });
                        EmitIntegerWidthNormalization(il, binary.Type, binary.Operation, function.Name);
                        StoreLocal(method, il, locals, binary.Result, binary.Type);
                        break;

                    case LoweredCallInstruction call:
                        if (TryHandleIgnoredIntrinsic(call))
                        {
                            break;
                        }

                        if (TryEmitKnownAggregateIntrinsic(method, il, parameters, locals, call, function.Name))
                        {
                            break;
                        }

                        if (TryEmitKnownRuntimeCall(module, il, parameters, locals, fieldMap, call))
                        {
                            if (call.Result is not null)
                            {
                                StoreCallResult(method, il, locals, call.Result, call.ReturnType, function.Name, ShouldSkipCallResultWidthNormalization(call));
                            }
                            else if (!string.Equals(call.ReturnType, "void", StringComparison.Ordinal))
                            {
                                il.Append(il.Create(OpCodes.Pop));
                            }

                            break;
                        }

                        if (TryHandleMemoryIntrinsic(il, parameters, locals, call, addressMap, memoryAliases, function.Name, memoryHelpers, fieldMap))
                        {
                            break;
                        }

                        if (TryEmitKnownPanicCall(module, il, call))
                        {
                            break;
                        }

                        foreach (var argument in call.Arguments)
                        {
                            LoadValue(il, parameters, locals, argument.Type, argument.Value, fieldMap);
                        }

                        if (TryResolveIntrinsicCall(module, call, vectorHelpers, out var intrinsicMethod))
                        {
                            il.Append(il.Create(OpCodes.Call, intrinsicMethod));
                            if (string.Equals(call.Callee, "llvm.ctpop.i64", StringComparison.Ordinal))
                            {
                                il.Append(il.Create(OpCodes.Conv_I8));
                            }
                        }
                        else if (methodMap.TryGetValue(call.Callee, out var callee))
                        {
                            il.Append(il.Create(OpCodes.Call, module.ImportReference(callee)));
                        }
                        else
                        {
                            throw new NotSupportedException($"Call target '{call.Callee}' was not found in the emitted module.");
                        }

                        if (call.Result is not null)
                        {
                            StoreCallResult(method, il, locals, call.Result, call.ReturnType, function.Name, ShouldSkipCallResultWidthNormalization(call));
                        }
                        else if (!string.Equals(call.ReturnType, "void", StringComparison.Ordinal))
                        {
                            il.Append(il.Create(OpCodes.Pop));
                        }
                        break;

                    case LoweredCompareInstruction compare:
                        LoadValue(il, parameters, locals, compare.Type, compare.Left);
                        EmitCompareOperandNormalization(il, compare.Type, compare.Predicate, function.Name);
                        LoadValue(il, parameters, locals, compare.Type, compare.Right);
                        EmitCompareOperandNormalization(il, compare.Type, compare.Predicate, function.Name);
                        EmitComparePredicate(il, compare.Predicate, function.Name);
                        StoreLocal(method, il, locals, compare.Result, module.TypeSystem.Int32);
                        break;

                    case LoweredConditionalBranchInstruction branch:
                        LoadValue(il, parameters, locals, "i32", branch.Condition);
                        var falseEdge = il.Create(OpCodes.Nop);
                        il.Append(il.Create(OpCodes.Brfalse, falseEdge));
                        EmitPhiCopiesForEdge(method, il, parameters, locals, phiByBlock, block.Name, branch.TrueTarget);
                        il.Append(il.Create(OpCodes.Br, ResolveBlockLabel(blockLabels, branch.TrueTarget, function.Name)));
                        il.Append(falseEdge);
                        EmitPhiCopiesForEdge(method, il, parameters, locals, phiByBlock, block.Name, branch.FalseTarget);
                        il.Append(il.Create(OpCodes.Br, ResolveBlockLabel(blockLabels, branch.FalseTarget, function.Name)));
                        break;

                    case LoweredJumpInstruction jump:
                        EmitPhiCopiesForEdge(method, il, parameters, locals, phiByBlock, block.Name, jump.Target);
                        il.Append(il.Create(OpCodes.Br, ResolveBlockLabel(blockLabels, jump.Target, function.Name)));
                        break;

                    case LoweredAllocaInstruction alloca:
                        if (rawAddressAllocas.Contains(alloca.Result)
                            && TryGetByteArrayAllocaSize(alloca.Type, out var allocaSize))
                        {
                            EmitRawByteArrayAlloca(method, il, locals, alloca.Result, allocaSize);
                        }

                        break;

                    case LoweredGetElementPointerInstruction gep:
                        addressMap[gep.Result] = gep;
                        EmitGetElementPointerValue(method, il, parameters, locals, fieldMap, gep);
                        break;

                    case LoweredTruncateInstruction trunc:
                        LoadValue(il, parameters, locals, trunc.FromType, trunc.Value);
                        EmitConversion(il, trunc.ToType);
                        StoreLocal(method, il, locals, trunc.Result, trunc.ToType);
                        break;

                    case LoweredZeroExtendInstruction zext:
                        LoadValue(il, parameters, locals, zext.FromType, zext.Value);
                        EmitZeroExtension(il, zext.FromType, zext.ToType, function.Name);
                        StoreLocal(method, il, locals, zext.Result, zext.ToType);
                        break;

                    case LoweredSignExtendInstruction sext:
                        LoadValue(il, parameters, locals, sext.FromType, sext.Value);
                        EmitSignExtension(il, sext.FromType, sext.ToType, function.Name);
                        StoreLocal(method, il, locals, sext.Result, sext.ToType);
                        break;

                    case LoweredSelectInstruction select:
                        EmitSelect(method, il, parameters, locals, select);
                        break;

                    case LoweredPhiInstruction:
                        break;

                    case LoweredUnreachableInstruction:
                        EmitThrow(il, module, typeof(InvalidOperationException));
                        break;

                    case LoweredStoreInstruction store:
                        if (TryStoreDirectPointerLocal(method, il, parameters, locals, store, function.Name))
                        {
                            break;
                        }

                        if (TryStoreDirectRawAllocaAddress(method, il, parameters, locals, store, function.Name))
                        {
                            break;
                        }

                        if (TryStoreDirectPointerParameter(method, il, parameters, locals, store, function.Name))
                        {
                            break;
                        }

                        if (addressMap.TryGetValue(store.Destination, out var storeAddress))
                        {
                            if (TryStoreByteOffsetAddress(method, il, locals, storeAddress, store.Type, memoryAliases))
                            {
                                break;
                            }

                            if (TryStoreRawAllocaAddress(method, il, parameters, locals, store, storeAddress, memoryAliases, function.Name))
                            {
                                break;
                            }

                            if (TryStorePointerParameterAddress(method, il, parameters, locals, store, storeAddress, memoryAliases, function.Name))
                            {
                                break;
                            }

                            LoadValue(il, parameters, locals, store.Type, store.Value);
                            StoreLocal(method, il, locals, GetIndexedSlotName(storeAddress), store.Type);
                            break;
                        }

                        LoadValue(il, parameters, locals, store.Type, store.Value);
                        StoreLocal(method, il, locals, store.Destination, store.Type);
                        break;

                    case LoweredLoadInstruction load:
                        if (TryLoadDirectPointerLocal(method, il, locals, load, function.Name))
                        {
                            break;
                        }

                        if (TryLoadDirectRawAllocaAddress(method, il, locals, load, function.Name))
                        {
                            break;
                        }

                        if (TryLoadDirectPointerParameter(method, il, locals, parameters, load, function.Name))
                        {
                            break;
                        }

                        if (addressMap.TryGetValue(load.Source, out var loadAddress))
                        {
                            if (TryLoadByteOffsetAddress(method, il, locals, load, loadAddress, memoryAliases))
                            {
                                break;
                            }

                            if (TryLoadRawAllocaAddress(method, il, locals, load, loadAddress, memoryAliases, function.Name))
                            {
                                break;
                            }

                            if (TryLoadPointerParameterAddress(method, il, locals, parameters, load, loadAddress, memoryAliases))
                            {
                                break;
                            }

                            LoadValue(il, parameters, locals, load.Type, GetIndexedSlotName(loadAddress));
                            StoreLocal(method, il, locals, load.Result, load.Type);
                            break;
                        }

                        if (locals.ContainsKey(load.Source))
                        {
                            LoadValue(il, parameters, locals, load.Type, load.Source);
                            StoreLocal(method, il, locals, load.Result, load.Type);
                            break;
                        }

                        if (!fieldMap.TryGetValue(load.Source, out var field))
                        {
                            if (!TryResolveConstantGlobalElement(load, globalMap, out var constantValue)
                                && !TryResolveAnonymousConstantElement(load, out constantValue))
                            {
                                throw new NotSupportedException($"Load source '{load.Source}' in function '{function.Name}' is not a local or supported global.");
                            }

                            if (string.Equals(load.Type, "ptr", StringComparison.Ordinal))
                            {
                                il.Append(il.Create(OpCodes.Ldc_I4_0));
                                il.Append(il.Create(OpCodes.Conv_I));
                            }
                            else
                            {
                                EmitConstant(il, load.Type, constantValue);
                            }

                            StoreLocal(method, il, locals, load.Result, load.Type);
                            break;
                        }

                        il.Append(il.Create(OpCodes.Ldsfld, field));
                        StoreLocal(method, il, locals, load.Result, load.Type);
                        break;

                    case LoweredReturnInstruction ret:
                        if (!string.Equals(ret.Type, "void", StringComparison.Ordinal))
                        {
                            LoadValue(il, parameters, locals, ret.Type, ret.Value);
                        }

                        il.Append(il.Create(OpCodes.Ret));
                        break;

                    case LoweredRawInstruction raw when TryEmitKnownRawInstruction(method, il, parameters, locals, raw):
                        break;

                    case LoweredRawInstruction raw when TryEmitRawSwitch(method, il, parameters, locals, blockLabels, phiByBlock, block.Name, block.Instructions, ref instructionIndex, raw, function.Name):
                        break;

                    default:
                        throw new NotSupportedException($"Instruction '{instruction.GetType().Name}' is not supported by the current emitter slice for function '{function.Name}'.");
                }
            }
        }

        if (!function.Blocks.Any(static block =>
                block.Instructions.Count > 0
                && (block.Instructions[^1] is LoweredReturnInstruction
                    || block.Instructions[^1] is LoweredUnreachableInstruction)))
        {
            throw new NotSupportedException($"Function '{function.Name}' does not end in a supported return or unreachable instruction.");
        }
    }

    private static void PredeclareFunctionLocals(MethodDefinition method, IDictionary<string, VariableDefinition> locals, LoweredFunction function, IReadOnlySet<string> rawAddressAllocas)
    {
        foreach (var block in function.Blocks)
        {
            foreach (var instruction in block.Instructions)
            {
                switch (instruction)
                {
                    case LoweredBinaryInstruction binary:
                        EnsureLocal(method, locals, binary.Result, binary.Type);
                        break;

                    case LoweredCallInstruction { Result: not null } call when TryGetAggregatePairTypes(call.ReturnType, out var firstType, out var secondType):
                        EnsureLocal(method, locals, GetAggregateTupleLocalName(call.Result), ResolveTypeReference(method.Module, call.ReturnType));
                        EnsureLocal(method, locals, GetAggregateComponentLocalName(call.Result, 0), firstType);
                        EnsureLocal(method, locals, GetAggregateComponentLocalName(call.Result, 1), secondType);
                        break;

                    case LoweredCallInstruction { Result: not null } call:
                        EnsureLocal(method, locals, call.Result, call.ReturnType);
                        break;

                    case LoweredCompareInstruction compare:
                        EnsureLocal(method, locals, compare.Result, method.Module.TypeSystem.Int32);
                        break;

                    case LoweredLoadInstruction load:
                        EnsureLocal(method, locals, load.Result, load.Type);
                        break;

                    case LoweredAllocaInstruction alloca
                        when rawAddressAllocas.Contains(alloca.Result)
                             && TryGetByteArrayAllocaSize(alloca.Type, out _):
                        EnsureLocal(method, locals, GetAllocaAddressLocalName(alloca.Result), method.Module.TypeSystem.IntPtr);
                        break;

                    case LoweredGetElementPointerInstruction gep:
                        EnsureLocal(method, locals, gep.Result, "ptr");
                        break;

                    case LoweredTruncateInstruction trunc:
                        EnsureLocal(method, locals, trunc.Result, trunc.ToType);
                        break;

                    case LoweredZeroExtendInstruction zext:
                        EnsureLocal(method, locals, zext.Result, zext.ToType);
                        break;

                    case LoweredSignExtendInstruction sext:
                        EnsureLocal(method, locals, sext.Result, sext.ToType);
                        break;

                    case LoweredSelectInstruction select:
                        EnsureLocal(method, locals, select.Result, select.ValueType);
                        break;

                    case LoweredPhiInstruction phi when TryGetAggregatePairTypes(phi.Type, out var firstType, out var secondType):
                        EnsureLocal(method, locals, GetAggregateComponentLocalName(phi.Result, 0), firstType);
                        EnsureLocal(method, locals, GetAggregateComponentLocalName(phi.Result, 1), secondType);
                        break;

                    case LoweredPhiInstruction phi:
                        EnsureLocal(method, locals, phi.Result, phi.Type);
                        break;

                    case LoweredRawInstruction raw:
                        PredeclareKnownRawInstructionLocals(method, locals, raw.Text);
                        break;
                }
            }
        }
    }

    private static void PredeclareKnownRawInstructionLocals(MethodDefinition method, IDictionary<string, VariableDefinition> locals, string text)
    {
        var intToPtrMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = inttoptr (?<sourceType>i\\d+) (?<value>[^ ]+) to ptr$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (intToPtrMatch.Success)
        {
            EnsureLocal(method, locals, NormalizeRawValue(intToPtrMatch.Groups["result"].Value), "ptr");
            return;
        }

        var ptrToIntMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = ptrtoint ptr (?<value>[^ ]+) to (?<targetType>i\\d+)$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (ptrToIntMatch.Success)
        {
            EnsureLocal(method, locals, NormalizeRawValue(ptrToIntMatch.Groups["result"].Value), ptrToIntMatch.Groups["targetType"].Value);
            return;
        }

        var rawGepMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = getelementptr(?: [A-Za-z0-9_]+)* i8, ptr (?<base>[^,]+), i64 (?<index>.+)$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (rawGepMatch.Success)
        {
            EnsureLocal(method, locals, NormalizeRawValue(rawGepMatch.Groups["result"].Value), "ptr");
            return;
        }

        var insertElementMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = insertelement (?<vectorType><\\d+ x i\\d+>) (?<seed>poison|<.+>), (?<scalarType>i\\d+) (?<value>[^,]+), i64 0$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (insertElementMatch.Success)
        {
            var vectorTypeName = insertElementMatch.Groups["vectorType"].Value;
            if (TryGetSupportedVectorType(vectorTypeName, out _))
            {
                EnsureLocal(method, locals, NormalizeRawValue(insertElementMatch.Groups["result"].Value), vectorTypeName);
            }

            return;
        }

        var shuffleMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = shufflevector (?<vectorType><\\d+ x i\\d+>) (?<source>[^,]+), <\\d+ x i\\d+> poison, <\\d+ x i32> zeroinitializer$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (shuffleMatch.Success)
        {
            var vectorTypeName = shuffleMatch.Groups["vectorType"].Value;
            if (TryGetSupportedVectorType(vectorTypeName, out _))
            {
                EnsureLocal(method, locals, NormalizeRawValue(shuffleMatch.Groups["result"].Value), vectorTypeName);
            }

            return;
        }

        var extractValueMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = extractvalue \\{ (?<firstType>[^,]+), (?<secondType>[^}]+) \\} (?<source>[^,]+), (?<index>[01])$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (extractValueMatch.Success)
        {
            var result = NormalizeRawValue(extractValueMatch.Groups["result"].Value);
            var index = int.Parse(extractValueMatch.Groups["index"].Value);
            var componentType = index == 0
                ? extractValueMatch.Groups["firstType"].Value.Trim()
                : extractValueMatch.Groups["secondType"].Value.Trim();

            if (string.Equals(componentType, "i1", StringComparison.Ordinal))
            {
                EnsureLocal(method, locals, result, method.Module.TypeSystem.Int32);
            }
            else
            {
                EnsureLocal(method, locals, result, componentType);
            }

            return;
        }

        var insertValueMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = insertvalue \\{ (?<firstType>[^,]+), (?<secondType>[^}]+) \\} (?<source>[^,]+), (?<valueType>[^ ]+) (?<value>[^,]+), (?<index>[01])$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (insertValueMatch.Success)
        {
            var result = NormalizeRawValue(insertValueMatch.Groups["result"].Value);
            EnsureLocal(method, locals, GetAggregateComponentLocalName(result, 0), insertValueMatch.Groups["firstType"].Value.Trim());
            EnsureLocal(method, locals, GetAggregateComponentLocalName(result, 1), insertValueMatch.Groups["secondType"].Value.Trim());
            return;
        }

        var freezeMatch = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = freeze (?<type>[^ ]+) (?<value>.+)$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (freezeMatch.Success)
        {
            var result = NormalizeRawValue(freezeMatch.Groups["result"].Value);
            var typeName = freezeMatch.Groups["type"].Value.Trim();
            if (string.Equals(typeName, "i1", StringComparison.Ordinal))
            {
                EnsureLocal(method, locals, result, method.Module.TypeSystem.Int32);
            }
            else
            {
                EnsureLocal(method, locals, result, typeName);
            }
        }
    }

    private static void LoadValue(ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string typeName, string value, IReadOnlyDictionary<string, FieldDefinition>? fieldMap = null)
    {
        if (TryGetAggregatePairTypes(typeName, out var firstType, out var secondType))
        {
            LoadValue(il, parameters, locals, firstType, GetAggregateComponentLocalName(value, 0), fieldMap);
            LoadValue(il, parameters, locals, secondType, GetAggregateComponentLocalName(value, 1), fieldMap);
            il.Append(il.Create(OpCodes.Newobj, CreateAggregatePairConstructor(il.Body.Method.Module, typeName)));
            return;
        }

        if (string.Equals(typeName, "ptr", StringComparison.Ordinal)
            && TryGetRawAllocaAddressLocal(locals, value, out var rawAddressLocal))
        {
            il.Append(il.Create(OpCodes.Ldloc, rawAddressLocal));
            return;
        }

        if (string.Equals(typeName, "ptr", StringComparison.Ordinal)
            && string.Equals(value, "null", StringComparison.Ordinal))
        {
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(OpCodes.Conv_I));
            return;
        }

        if (string.Equals(typeName, "ptr", StringComparison.Ordinal))
        {
            var constantIntToPtrMatch = System.Text.RegularExpressions.Regex.Match(
                value,
                "^\\((?<sourceType>i\\d+) (?<constant>-?\\d+) to ptr\\)$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant);
            if (constantIntToPtrMatch.Success
                && TryParseIntegerConstant(constantIntToPtrMatch.Groups["sourceType"].Value, constantIntToPtrMatch.Groups["constant"].Value, out var pointerConstant))
            {
                EmitConstant(il, constantIntToPtrMatch.Groups["sourceType"].Value, pointerConstant);
                il.Append(il.Create(OpCodes.Conv_I));
                return;
            }
        }

        if (string.Equals(value, "undef", StringComparison.Ordinal)
            || string.Equals(value, "poison", StringComparison.Ordinal))
        {
            if (string.Equals(typeName, "ptr", StringComparison.Ordinal))
            {
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Conv_I));
                return;
            }

            if (TryGetIntegerBitWidth(typeName, out _)
                || string.Equals(typeName, "i1", StringComparison.Ordinal))
            {
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                EmitConversion(il, typeName);
                return;
            }
        }

        if (parameters.TryGetValue(value, out var parameter))
        {
            il.Append(il.Create(OpCodes.Ldarg, parameter));
            return;
        }

        if (locals.TryGetValue(value, out var local))
        {
            il.Append(il.Create(OpCodes.Ldloc, local));
            return;
        }

        if (fieldMap is not null && fieldMap.TryGetValue(value, out var field))
        {
            il.Append(il.Create(OpCodes.Ldsfld, field));
            return;
        }

        var emittedGlobalField = il.Body.Method.Module.Types
            .SelectMany(static type => type.Fields)
            .FirstOrDefault(fieldDefinition => string.Equals(fieldDefinition.Name, value, StringComparison.Ordinal));
        if (emittedGlobalField is not null)
        {
            il.Append(il.Create(OpCodes.Ldsfld, emittedGlobalField));
            return;
        }

        if (TryEmitVectorConstant(il, typeName, value))
        {
            return;
        }

        if (string.Equals(typeName, "i1", StringComparison.Ordinal))
        {
            if (string.Equals(value, "true", StringComparison.Ordinal))
            {
                il.Append(il.Create(OpCodes.Ldc_I4_1));
                return;
            }

            if (string.Equals(value, "false", StringComparison.Ordinal))
            {
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                return;
            }
        }

        if (TryParseIntegerConstant(typeName, value, out var constant))
        {
            EmitConstant(il, typeName, constant);
            return;
        }

        throw new NotSupportedException($"Value '{value}' of type '{typeName}' could not be resolved to a parameter, local, or supported constant.");
    }

    private static bool TryEmitKnownRawInstruction(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredRawInstruction raw)
    {
        if (TryEmitRawGetElementPointer(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitIntToPtr(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitPtrToInt(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitInsertValue(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitExtractValue(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitInsertElementSeed(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitShuffleBroadcast(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitFreeze(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitCleanupControlFlow(il, raw.Text))
        {
            return true;
        }

        return false;
    }

    private static bool TryEmitCleanupControlFlow(ILProcessor il, string text)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(
                text,
            "^(?<result>[^ ]+) = cleanuppad within none \\[\\]$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant))
        {
            return true;
        }

        if (System.Text.RegularExpressions.Regex.IsMatch(
                text,
                "^cleanupret from (?<pad>[^ ]+) unwind to caller$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant))
        {
            il.Append(il.Create(OpCodes.Ret));
            return true;
        }

        return false;
    }

    private static bool TryEmitFreeze(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = freeze (?<type>[^ ]+) (?<value>.+)$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var result = NormalizeRawValue(match.Groups["result"].Value);
        var typeName = match.Groups["type"].Value.Trim();
        var value = NormalizeRawValue(match.Groups["value"].Value.Trim());

        LoadValue(il, parameters, locals, typeName, value);
        if (string.Equals(typeName, "i1", StringComparison.Ordinal))
        {
            StoreLocal(method, il, locals, result, method.Module.TypeSystem.Int32);
        }
        else
        {
            StoreLocal(method, il, locals, result, ResolveTypeReference(method.Module, typeName));
        }

        return true;
    }

    private static bool TryEmitIntToPtr(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = inttoptr (?<sourceType>i\\d+) (?<value>[^ ]+) to ptr$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var result = NormalizeRawValue(match.Groups["result"].Value);
        var sourceType = match.Groups["sourceType"].Value;
        var value = NormalizeRawValue(match.Groups["value"].Value);

        LoadValue(il, parameters, locals, sourceType, value);
        il.Append(il.Create(OpCodes.Conv_I));
        StoreLocal(method, il, locals, result, "ptr");
        return true;
    }

    private static bool TryEmitPtrToInt(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = ptrtoint ptr (?<value>[^ ]+) to (?<targetType>i\\d+)$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var result = NormalizeRawValue(match.Groups["result"].Value);
        var value = NormalizeRawValue(match.Groups["value"].Value);
        var targetType = match.Groups["targetType"].Value;

        LoadValue(il, parameters, locals, "ptr", value);
        EmitConversion(il, targetType);
        StoreLocal(method, il, locals, result, targetType);
        return true;
    }

    private static bool TryEmitRawGetElementPointer(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = getelementptr(?: [A-Za-z0-9_]+)* i8, ptr (?<base>[^,]+), i64 (?<index>.+)$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var result = NormalizeRawValue(match.Groups["result"].Value);
        var baseValue = NormalizeRawValue(match.Groups["base"].Value);
        var indexValue = NormalizeRawValue(match.Groups["index"].Value);

        LoadValue(il, parameters, locals, "ptr", baseValue);
        LoadValue(il, parameters, locals, "i64", indexValue);
        il.Append(il.Create(OpCodes.Conv_I));
        il.Append(il.Create(OpCodes.Add));
        StoreLocal(method, il, locals, result, "ptr");
        return true;
    }

    private static void EmitGetElementPointerValue(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, FieldDefinition> fieldMap, LoweredGetElementPointerInstruction gep)
    {
        LoadValue(il, parameters, locals, "ptr", gep.Base, fieldMap);

        if (gep.Index != 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4, checked(gep.Index * GetGetElementPointerStride(gep.ElementType))));
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Add));
        }

        StoreLocal(method, il, locals, gep.Result, "ptr");
    }

    private static bool TryEmitRawSwitch(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, Instruction> blockLabels, IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock, string sourceBlock, IReadOnlyList<LoweredInstruction> instructions, ref int instructionIndex, LoweredRawInstruction raw, string functionName)
    {
        var switchMatch = System.Text.RegularExpressions.Regex.Match(
            raw.Text,
            "^switch (?<type>i\\d+) (?<value>[^,]+), label %(?<defaultTarget>[^ ]+) \\[$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!switchMatch.Success)
        {
            return false;
        }

        var switchType = switchMatch.Groups["type"].Value;
        var switchValue = NormalizeRawValue(switchMatch.Groups["value"].Value);
        var defaultTarget = switchMatch.Groups["defaultTarget"].Value;
        var caseLabels = new List<(long Value, string Target)>();

        var closingIndex = -1;
        for (var index = instructionIndex + 1; index < instructions.Count; index++)
        {
            if (instructions[index] is not LoweredRawInstruction caseRaw)
            {
                throw new NotSupportedException($"Switch in function '{functionName}' contains a non-raw case entry.");
            }

            if (string.Equals(caseRaw.Text, "]", StringComparison.Ordinal))
            {
                closingIndex = index;
                break;
            }

            var caseMatch = System.Text.RegularExpressions.Regex.Match(
                caseRaw.Text,
                "^(?<type>i\\d+) (?<value>-?\\d+), label %(?<target>[^ ]+)$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant);
            if (!caseMatch.Success)
            {
                throw new NotSupportedException($"Switch case '{caseRaw.Text}' in function '{functionName}' is not supported by the current emitter slice.");
            }

            caseLabels.Add((long.Parse(caseMatch.Groups["value"].Value), caseMatch.Groups["target"].Value));
        }

        if (closingIndex < 0)
        {
            throw new NotSupportedException($"Switch in function '{functionName}' is missing its closing bracket.");
        }

        foreach (var caseLabel in caseLabels)
        {
            LoadValue(il, parameters, locals, switchType, switchValue);
            EmitConstant(il, switchType, caseLabel.Value);
            il.Append(il.Create(OpCodes.Ceq));
            var nextCase = il.Create(OpCodes.Nop);
            il.Append(il.Create(OpCodes.Brfalse, nextCase));
            EmitPhiCopiesForEdge(method, il, parameters, locals, phiByBlock, sourceBlock, caseLabel.Target);
            il.Append(il.Create(OpCodes.Br, ResolveBlockLabel(blockLabels, caseLabel.Target, functionName)));
            il.Append(nextCase);
        }

        EmitPhiCopiesForEdge(method, il, parameters, locals, phiByBlock, sourceBlock, defaultTarget);
        il.Append(il.Create(OpCodes.Br, ResolveBlockLabel(blockLabels, defaultTarget, functionName)));
        instructionIndex = closingIndex;
        return true;
    }

    private static bool TryEmitInsertElementSeed(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = insertelement (?<vectorType><\\d+ x i\\d+>) (?<seed>poison|<.+>), (?<scalarType>i\\d+) (?<value>[^,]+), i64 0$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var vectorTypeName = match.Groups["vectorType"].Value;
        var scalarTypeName = match.Groups["scalarType"].Value;
        if (!TryGetSupportedVectorType(vectorTypeName, out var vectorType)
            || !string.Equals(vectorType.ElementType, scalarTypeName, StringComparison.Ordinal))
        {
            return false;
        }

        var seedValue = match.Groups["seed"].Value;
        if (!TryEmitVectorConstant(il, vectorTypeName, seedValue))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Dup));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        LoadValue(il, parameters, locals, scalarTypeName, NormalizeRawValue(match.Groups["value"].Value));
        il.Append(il.Create(GetVectorElementStoreOpCode(vectorType.ElementWidth)));
        StoreLocal(method, il, locals, NormalizeRawValue(match.Groups["result"].Value), vectorTypeName);
        return true;
    }

    private static bool TryEmitShuffleBroadcast(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = shufflevector (?<vectorType><\\d+ x i\\d+>) (?<source>[^,]+), <\\d+ x i\\d+> poison, <\\d+ x i32> zeroinitializer$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var vectorTypeName = match.Groups["vectorType"].Value;
        if (!TryGetSupportedVectorType(vectorTypeName, out var vectorType))
        {
            return false;
        }

        var sourceName = NormalizeRawValue(match.Groups["source"].Value);
        var sourceLocalName = $"$raw.{NormalizeRawValue(match.Groups["result"].Value)}.source";
        LoadValue(il, parameters, locals, vectorTypeName, sourceName);
        StoreLocal(method, il, locals, sourceLocalName, vectorTypeName);

        il.Append(il.Create(OpCodes.Ldc_I4, vectorType.ElementCount));
        il.Append(il.Create(OpCodes.Newarr, GetVectorElementType(il.Body.Method.Module, vectorType)));

        for (var index = 0; index < vectorType.ElementCount; index++)
        {
            il.Append(il.Create(OpCodes.Dup));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(OpCodes.Ldloc, EnsureLocal(method, locals, sourceLocalName, vectorTypeName)));
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(GetVectorElementLoadOpCode(vectorType.ElementWidth)));
            il.Append(il.Create(GetVectorElementStoreOpCode(vectorType.ElementWidth)));
        }

        StoreLocal(method, il, locals, NormalizeRawValue(match.Groups["result"].Value), vectorTypeName);
        return true;
    }

    private static bool TryEmitExtractValue(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = extractvalue \\{ (?<firstType>[^,]+), (?<secondType>[^}]+) \\} (?<source>[^,]+), (?<index>[01])$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var source = NormalizeRawValue(match.Groups["source"].Value);
        var index = int.Parse(match.Groups["index"].Value);
        var componentType = index == 0
            ? match.Groups["firstType"].Value.Trim()
            : match.Groups["secondType"].Value.Trim();
        var componentLocalName = GetAggregateComponentLocalName(source, index);

        LoadValue(il, parameters, locals, componentType, componentLocalName);
        if (string.Equals(componentType, "i1", StringComparison.Ordinal))
        {
            StoreLocal(method, il, locals, NormalizeRawValue(match.Groups["result"].Value), method.Module.TypeSystem.Int32);
        }
        else
        {
            StoreLocal(method, il, locals, NormalizeRawValue(match.Groups["result"].Value), componentType);
        }

        return true;
    }

    private static bool TryEmitInsertValue(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string text)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            text,
            "^(?<result>[^ ]+) = insertvalue \\{ (?<firstType>[^,]+), (?<secondType>[^}]+) \\} (?<source>[^,]+), (?<valueType>[^ ]+) (?<value>[^,]+), (?<index>[01])$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            return false;
        }

        var result = NormalizeRawValue(match.Groups["result"].Value);
        var source = NormalizeRawValue(match.Groups["source"].Value);
        var firstType = match.Groups["firstType"].Value.Trim();
        var secondType = match.Groups["secondType"].Value.Trim();
        var valueType = match.Groups["valueType"].Value.Trim();
        var value = NormalizeRawValue(match.Groups["value"].Value);
        var index = int.Parse(match.Groups["index"].Value);
        var resultFirst = GetAggregateComponentLocalName(result, 0);
        var resultSecond = GetAggregateComponentLocalName(result, 1);

        if (string.Equals(source, "poison", StringComparison.Ordinal))
        {
            EmitZeroValue(il, firstType);
            StoreLocal(method, il, locals, resultFirst, firstType);
            EmitZeroValue(il, secondType);
            StoreLocal(method, il, locals, resultSecond, secondType);
        }
        else
        {
            LoadValue(il, parameters, locals, firstType, GetAggregateComponentLocalName(source, 0));
            StoreLocal(method, il, locals, resultFirst, firstType);
            LoadValue(il, parameters, locals, secondType, GetAggregateComponentLocalName(source, 1));
            StoreLocal(method, il, locals, resultSecond, secondType);
        }

        LoadValue(il, parameters, locals, valueType, value);
        StoreLocal(method, il, locals, index == 0 ? resultFirst : resultSecond, index == 0 ? firstType : secondType);
        return true;
    }

    private static string NormalizeRawValue(string value)
    {
        var normalized = value.Trim().TrimStart('%');
        return normalized.Length > 0 && normalized.All(char.IsDigit)
            ? $"tmp.{normalized}"
            : normalized;
    }

    private static void StoreLocal(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, string localName, string typeName)
    {
        var local = EnsureLocal(method, locals, localName, typeName);

        il.Append(il.Create(OpCodes.Stloc, local));
    }

    private static void StoreLocal(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, string localName, TypeReference typeReference)
    {
        var local = EnsureLocal(method, locals, localName, typeReference);

        il.Append(il.Create(OpCodes.Stloc, local));
    }

    private static VariableDefinition EnsureLocal(MethodDefinition method, IDictionary<string, VariableDefinition> locals, string localName, string typeName)
    {
        return EnsureLocal(method, locals, localName, ResolveTypeReference(method.Module, typeName));
    }

    private static VariableDefinition EnsureLocal(MethodDefinition method, IDictionary<string, VariableDefinition> locals, string localName, TypeReference typeReference)
    {
        if (!locals.TryGetValue(localName, out var local))
        {
            local = new VariableDefinition(typeReference);
            method.Body.Variables.Add(local);
            locals.Add(localName, local);
        }
        else if (!string.Equals(local.VariableType.FullName, typeReference.FullName, StringComparison.Ordinal))
        {
            throw new NotSupportedException($"Local '{localName}' was previously emitted as '{local.VariableType.FullName}' and cannot be reused as '{typeReference.FullName}'.");
        }

        return local;
    }

    private static Instruction ResolveBlockLabel(IReadOnlyDictionary<string, Instruction> blockLabels, string blockName, string functionName)
    {
        if (!blockLabels.TryGetValue(blockName, out var label))
        {
            throw new NotSupportedException($"Branch target '{blockName}' was not found in function '{functionName}'.");
        }

        return label;
    }

    private static void EmitComparePredicate(ILProcessor il, string predicate, string functionName)
    {
        switch (predicate)
        {
            case "sgt":
                il.Append(il.Create(OpCodes.Cgt));
                return;

            case "slt":
                il.Append(il.Create(OpCodes.Clt));
                return;

            case "eq":
                il.Append(il.Create(OpCodes.Ceq));
                return;

            case "ne":
                il.Append(il.Create(OpCodes.Ceq));
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Ceq));
                return;

            case "ugt":
                il.Append(il.Create(OpCodes.Cgt_Un));
                return;

            case "ult":
                il.Append(il.Create(OpCodes.Clt_Un));
                return;

            case "sle":
                il.Append(il.Create(OpCodes.Cgt));
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Ceq));
                return;

            case "sge":
                il.Append(il.Create(OpCodes.Clt));
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Ceq));
                return;

            case "ule":
                il.Append(il.Create(OpCodes.Cgt_Un));
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Ceq));
                return;

            case "uge":
                il.Append(il.Create(OpCodes.Clt_Un));
                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Ceq));
                return;

            default:
                throw new NotSupportedException($"Unsupported compare predicate '{predicate}' in function '{functionName}'.");
        }
    }

    private static void EmitCompareOperandNormalization(ILProcessor il, string typeName, string predicate, string functionName)
    {
        if (!TryGetIntegerBitWidth(typeName, out var width) || width >= 32)
        {
            return;
        }

        if (width <= 0)
        {
            throw new NotSupportedException($"Integer width '{typeName}' is not supported by the current emitter slice in function '{functionName}'.");
        }

        switch (predicate)
        {
            case "sgt":
            case "slt":
            case "sle":
            case "sge":
                EmitSignedNarrowIntegerNormalization(il, width);
                return;

            case "eq":
            case "ne":
            case "ugt":
            case "ult":
            case "ule":
            case "uge":
                EmitUnsignedNarrowIntegerNormalization(il, width);
                return;

            default:
                throw new NotSupportedException($"Unsupported compare predicate '{predicate}' in function '{functionName}'.");
        }
    }

    private static void EmitSignedNarrowIntegerNormalization(ILProcessor il, int width)
    {
        switch (width)
        {
            case 8:
                il.Append(il.Create(OpCodes.Conv_I1));
                return;
            case 16:
                il.Append(il.Create(OpCodes.Conv_I2));
                return;
            default:
                var shiftAmount = 32 - width;
                il.Append(il.Create(OpCodes.Ldc_I4, shiftAmount));
                il.Append(il.Create(OpCodes.Shl));
                il.Append(il.Create(OpCodes.Ldc_I4, shiftAmount));
                il.Append(il.Create(OpCodes.Shr));
                return;
        }
    }

    private static void EmitUnsignedNarrowIntegerNormalization(ILProcessor il, int width)
    {
        switch (width)
        {
            case 8:
                il.Append(il.Create(OpCodes.Conv_U1));
                il.Append(il.Create(OpCodes.Conv_U4));
                return;
            case 16:
                il.Append(il.Create(OpCodes.Conv_U2));
                il.Append(il.Create(OpCodes.Conv_U4));
                return;
            default:
                EmitIntegerMask(il, width);
                il.Append(il.Create(OpCodes.Conv_U4));
                return;
        }
    }

    private static bool TryResolveIntrinsicCall(ModuleDefinition module, LoweredCallInstruction call, VectorHelperMethods vectorHelpers, out MethodReference methodReference)
    {
        methodReference = null!;

        if (string.Equals(call.Callee, "llvm.fshl.i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.FunnelShiftLeftI32;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXor;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXorI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXorI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXorI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXorI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXorI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAdd;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAddI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAddI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAddI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAddI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAddI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOrI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOrI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOrI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOrI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOrI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smax.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smin.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smax.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smin.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.and.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAndI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.and.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAndI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.and.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAndI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOr;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.and.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAnd;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smin.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smax.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smin.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smax.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umax.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxUnsignedI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umin.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinUnsignedI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umax.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxUnsignedI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umax.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxUnsignedI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umin.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinUnsignedI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umax.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxUnsignedI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umin.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinUnsignedI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umin.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinUnsignedI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smax.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMax;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smin.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMin;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umax.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxUnsigned;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umin.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinUnsigned;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.Max;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.Min;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsigned;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.v4i32", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsigned;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsignedI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.v8i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsignedI16x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsignedI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.v4i16", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsignedI16x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsignedI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsignedI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsignedI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.v4i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsignedI8x4;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.v16i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinI8x16;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.v8i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinI8x8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxScalarI8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinScalarI8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsignedScalarI8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.i8", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsignedScalarI8;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.add.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAddI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.xor.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceXorI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.or.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceOrI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smax.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.smin.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umax.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMaxUnsignedI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.umin.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceMinUnsignedI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.vector.reduce.and.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.ReduceAndI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smax.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.smin.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umax.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MaxUnsignedI64;
            return true;
        }

        if (string.Equals(call.Callee, "llvm.umin.v2i64", StringComparison.Ordinal))
        {
            methodReference = vectorHelpers.MinUnsignedI64;
            return true;
        }

        var methodInfo = call.Callee switch
        {
            "llvm.smin.i16" => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(short), typeof(short)]),
            "llvm.smax.i16" => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(short), typeof(short)]),
            "llvm.umax.i16" => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ushort), typeof(ushort)]),
            "llvm.umin.i16" => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ushort), typeof(ushort)]),
            "llvm.smax.i32" => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)]),
            "llvm.smax.i64" => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(long), typeof(long)]),
            "llvm.umax.i32" => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint), typeof(uint)]),
            "llvm.umax.i64" => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong), typeof(ulong)]),
            "llvm.smin.i32" => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)]),
            "llvm.smin.i64" => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(long), typeof(long)]),
            "llvm.umin.i32" => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint), typeof(uint)]),
            "llvm.umin.i64" => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong), typeof(ulong)]),
            "llvm.ctpop.i64" => typeof(System.Numerics.BitOperations).GetMethod(nameof(System.Numerics.BitOperations.PopCount), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong)]),
            _ => null
        };

        if (methodInfo is null)
        {
            return false;
        }

        methodReference = module.ImportReference(methodInfo);
        return true;
    }

    private static bool TryEmitKnownPanicCall(ModuleDefinition module, ILProcessor il, LoweredCallInstruction call)
    {
        if (call.Result is not null)
        {
            return false;
        }

        if (call.Callee.Contains("panic_const_div_by_zero", StringComparison.Ordinal))
        {
            EmitThrow(il, module, typeof(DivideByZeroException));
            return true;
        }

        if (call.Callee.Contains("panic_const_rem_by_zero", StringComparison.Ordinal))
        {
            EmitThrow(il, module, typeof(DivideByZeroException));
            return true;
        }

        if (call.Callee.Contains("panic_const_div_overflow", StringComparison.Ordinal))
        {
            EmitThrow(il, module, typeof(OverflowException));
            return true;
        }

        if (call.Callee.Contains("panic_const_rem_overflow", StringComparison.Ordinal))
        {
            EmitThrow(il, module, typeof(OverflowException));
            return true;
        }

        return false;
    }

    private static bool TryEmitKnownRuntimeCall(ModuleDefinition module, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, FieldDefinition> fieldMap, LoweredCallInstruction call)
    {
        if (string.Equals(call.ReturnType, "void", StringComparison.Ordinal)
            && call.Callee.Contains("precondition_check", StringComparison.Ordinal))
        {
            return true;
        }

        if (call.Callee.Contains("rem_euclid", StringComparison.Ordinal))
        {
            if (call.Arguments.Count < 2)
            {
                throw new NotSupportedException($"Numeric runtime call '{call.Callee}' did not provide the expected rem_euclid argument list.");
            }

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value, fieldMap);
            LoadValue(il, parameters, locals, call.Arguments[1].Type, call.Arguments[1].Value, fieldMap);

            var helperName = call.ReturnType switch
            {
                "i32" => nameof(RuntimeBridgeHelpers.RemEuclidI32),
                "i64" => nameof(RuntimeBridgeHelpers.RemEuclidI64),
                _ => throw new NotSupportedException($"Numeric runtime call '{call.Callee}' with return type '{call.ReturnType}' is not supported.")
            };

            var helperMethod = typeof(RuntimeBridgeHelpers).GetMethod(helperName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                ?? throw new InvalidOperationException($"Runtime bridge helper '{helperName}' could not be resolved.");
            il.Append(il.Create(OpCodes.Call, module.ImportReference(helperMethod)));
            return true;
        }

        if (call.Callee.Contains("raw_vec12handle_error", StringComparison.Ordinal))
        {
            EmitThrow(il, module, typeof(OutOfMemoryException));
            return true;
        }

        if (call.Callee.Contains("panic_fmt", StringComparison.Ordinal)
            || call.Callee.Contains("panic_nounwind_fmt", StringComparison.Ordinal)
            || call.Callee.Contains("expect_failed", StringComparison.Ordinal)
            || call.Callee.Contains("unwrap_failed", StringComparison.Ordinal))
        {
            EmitThrow(il, module, typeof(InvalidOperationException));
            return true;
        }

        if (TryEmitManagedRuntimeApiCall(module, il, parameters, locals, fieldMap, call))
        {
            return true;
        }

        if (call.Callee.Contains("Wtf8", StringComparison.Ordinal)
            && call.Callee.Contains("to_owned", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.InitializeWtf8PathBuffer));
            return true;
        }

        if (call.Callee.Contains("PathBuf5_push", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.AppendPathSegment));
            return true;
        }

        if (call.Callee.Contains("read_to_string5inner", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ReadFileToRustString));
            return true;
        }

        if (call.Callee.Contains("___rust_no_alloc_shim_is_unstable_v2", StringComparison.Ordinal))
        {
            return true;
        }

        if (call.Callee.Contains("___rust_alloc", StringComparison.Ordinal))
        {
            if (call.Arguments.Count < 1)
            {
                throw new NotSupportedException($"Allocator runtime call '{call.Callee}' did not provide a size argument.");
            }

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value);
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.AllocHGlobal), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)])
                ?? throw new InvalidOperationException("Marshal.AllocHGlobal(IntPtr) could not be resolved."))));
            return true;
        }

        if (call.Callee.Contains("___rust_realloc", StringComparison.Ordinal))
        {
            if (call.Arguments.Count < 4)
            {
                throw new NotSupportedException($"Allocator runtime call '{call.Callee}' did not provide the expected realloc argument list.");
            }

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value);
            LoadValue(il, parameters, locals, call.Arguments[3].Type, call.Arguments[3].Value);
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReAllocHGlobal), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(IntPtr)])
                ?? throw new InvalidOperationException("Marshal.ReAllocHGlobal(IntPtr, IntPtr) could not be resolved."))));
            return true;
        }

        if (call.Callee.Contains("___rust_dealloc", StringComparison.Ordinal))
        {
            if (call.Arguments.Count < 1)
            {
                throw new NotSupportedException($"Allocator runtime call '{call.Callee}' did not provide the expected dealloc argument list.");
            }

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value);
            il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.FreeHGlobal), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)])
                ?? throw new InvalidOperationException("Marshal.FreeHGlobal(IntPtr) could not be resolved."))));
            return true;
        }

        return false;
    }

    private static bool TryEmitManagedRuntimeApiCall(ModuleDefinition module, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, FieldDefinition> fieldMap, LoweredCallInstruction call)
    {
        MethodReference? methodReference = null;
        FieldReference? fieldReference = null;

        if (TryEmitAvaloniaBridgeCall(module, il, parameters, locals, fieldMap, call))
        {
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_is_windows", StringComparison.Ordinal))
        {
            EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 0);

            var isWindowsMethod = typeof(OperatingSystem).GetMethod(nameof(OperatingSystem.IsWindows), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                ?? throw new InvalidOperationException("OperatingSystem.IsWindows() could not be resolved.");
            methodReference = module.ImportReference(isWindowsMethod);
        }
        else if (string.Equals(call.Callee, "rust_mcil_dotnet_directory_separator_char", StringComparison.Ordinal))
        {
            EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 0);

            var separatorField = typeof(System.IO.Path).GetField(nameof(System.IO.Path.DirectorySeparatorChar), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                ?? throw new InvalidOperationException("Path.DirectorySeparatorChar field could not be resolved.");
            fieldReference = module.ImportReference(separatorField);
        }
        else if (string.Equals(call.Callee, "rust_mcil_dotnet_path_separator_char", StringComparison.Ordinal))
        {
            EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 0);

            var separatorField = typeof(System.IO.Path).GetField(nameof(System.IO.Path.PathSeparator), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                ?? throw new InvalidOperationException("Path.PathSeparator field could not be resolved.");
            fieldReference = module.ImportReference(separatorField);
        }
        else if (string.Equals(call.Callee, "rust_mcil_dotnet_math_max_i32", StringComparison.Ordinal))
        {
            EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 2);

            var maxMethod = typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)])
                ?? throw new InvalidOperationException("Math.Max(int, int) could not be resolved.");
            methodReference = module.ImportReference(maxMethod);
        }
        else if (string.Equals(call.Callee, "rust_mcil_dotnet_math_min_i32", StringComparison.Ordinal))
        {
            EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 2);

            var minMethod = typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)])
                ?? throw new InvalidOperationException("Math.Min(int, int) could not be resolved.");
            methodReference = module.ImportReference(minMethod);
        }
        else if (string.Equals(call.Callee, "rust_mcil_dotnet_bitops_popcount_u32", StringComparison.Ordinal))
        {
            EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 1);

            var popCountMethod = typeof(System.Numerics.BitOperations).GetMethod(nameof(System.Numerics.BitOperations.PopCount), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint)])
                ?? throw new InvalidOperationException("BitOperations.PopCount(uint) could not be resolved.");
            methodReference = module.ImportReference(popCountMethod);
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_command_line_arg_count", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CommandLineArgCount));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_command_line_arg_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8CommandLineArgLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_copy_command_line_arg_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8CommandLineArg));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_console_write_line_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ConsoleWriteLineUtf8));
            return true;
        }

        if (GeneratedBindingRuntimeHelpers.TryGetValue(call.Callee, out var generatedBindingHelperName))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, generatedBindingHelperName);
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_console_write_prefixed_line_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ConsoleWritePrefixedLineUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_console_write_path_line_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ConsoleWritePathLineUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_console_write_numbered_line_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ConsoleWriteNumberedLineUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_console_write_i32", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ConsoleWriteI32));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_console_write_path_count_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.ConsoleWritePathCountUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_file_read_all_lines_count", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8ReadAllLinesCount));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_file_read_all_lines_line_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8ReadAllLinesLineLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_file_copy_read_all_lines_line_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8ReadAllLinesLine));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_string_contains", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8StringContains));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_string_replace_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8StringReplaceLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_string_copy_replace_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8StringReplace));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_combine_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathCombineLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_current_directory_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8CurrentDirectoryLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_user_profile_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8UserProfileLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_temp_path_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8TempPathLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_documents_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8DocumentsLength));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_relative_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetRelativeLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_directory_name_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetDirectoryNameLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_full_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetFullPathLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_root_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetRootLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_combine3_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathCombine3LengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_combine_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathCombine));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_copy_current_directory_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8CurrentDirectory));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_copy_user_profile_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8UserProfile));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_copy_temp_path_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8TempPath));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_copy_documents_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8Documents));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_relative_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathGetRelative));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_directory_name_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathGetDirectoryName));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_full_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathGetFullPath));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_root_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathGetRoot));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_combine3_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathCombine3));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_change_extension_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathChangeExtensionLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_change_extension_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathChangeExtension));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_file_name_without_extension_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetFileNameWithoutExtensionLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_file_name_without_extension_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathGetFileNameWithoutExtension));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_file_name_utf8_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetFileNameLengthUtf8));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_copy_file_name_utf8", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.CopyUtf8PathGetFileName));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_string_index_of", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8StringIndexOf));
            return true;
        }

        if (string.Equals(call.Callee, "rust_mcil_dotnet_path_get_file_name_len", StringComparison.Ordinal))
        {
            EmitRuntimeBridgeCall(module, il, parameters, locals, fieldMap, call, nameof(RuntimeBridgeHelpers.Utf8PathGetFileNameLength));
            return true;
        }

        if (methodReference is not null)
        {
            foreach (var argument in call.Arguments)
            {
                LoadValue(il, parameters, locals, argument.Type, argument.Value, fieldMap);
            }

            il.Append(il.Create(OpCodes.Call, methodReference));
            return true;
        }

        if (fieldReference is not null)
        {
            il.Append(il.Create(OpCodes.Ldsfld, fieldReference));
            return true;
        }

        if (!string.Equals(call.Callee, "rust_mcil_dotnet_newline_len", StringComparison.Ordinal))
        {
            return false;
        }

        EnsureRuntimeBridgeArgumentCount(call, expectedArgumentCount: 0);

        var newLineGetter = typeof(Environment).GetProperty(nameof(Environment.NewLine), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetMethod
            ?? throw new InvalidOperationException("Environment.NewLine getter could not be resolved.");
        var stringLengthGetter = typeof(string).GetProperty(nameof(string.Length), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetMethod
            ?? throw new InvalidOperationException("string.Length getter could not be resolved.");

        il.Append(il.Create(OpCodes.Call, module.ImportReference(newLineGetter)));
        il.Append(il.Create(OpCodes.Callvirt, module.ImportReference(stringLengthGetter)));
        return true;
    }

    private static bool TryEmitAvaloniaBridgeCall(ModuleDefinition module, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, FieldDefinition> fieldMap, LoweredCallInstruction call)
    {
        var bridgeCall = call.Callee switch
        {
            "rust_mcil_avalonia_run_app" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.RunApp), ArgumentCount: 0),
            "rust_mcil_avalonia_window_new" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.CreateWindow), ArgumentCount: 0),
            "rust_mcil_avalonia_stack_panel_new" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.CreateStackPanel), ArgumentCount: 0),
            "rust_mcil_avalonia_text_block_new" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.CreateTextBlock), ArgumentCount: 0),
            "rust_mcil_avalonia_button_new" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.CreateButton), ArgumentCount: 0),
            "rust_mcil_avalonia_window_set_title_utf8" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetWindowTitleUtf8), ArgumentCount: 3),
            "rust_mcil_avalonia_window_set_size" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetWindowSize), ArgumentCount: 3),
            "rust_mcil_avalonia_window_set_content" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetWindowContent), ArgumentCount: 2),
            "rust_mcil_avalonia_stack_panel_set_spacing" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetStackPanelSpacing), ArgumentCount: 2),
            "rust_mcil_avalonia_stack_panel_set_margin" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetStackPanelMargin), ArgumentCount: 2),
            "rust_mcil_avalonia_stack_panel_add_child" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.AddStackPanelChild), ArgumentCount: 2),
            "rust_mcil_avalonia_text_block_set_text_utf8" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetTextBlockTextUtf8), ArgumentCount: 3),
            "rust_mcil_avalonia_button_set_content_utf8" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetButtonContentUtf8), ArgumentCount: 3),
            "rust_mcil_avalonia_button_set_on_click" => (MethodName: nameof(RustMcil.AvaloniaSupport.AvaloniaBridge.SetButtonOnClick), ArgumentCount: 3),
            _ => default
        };

        if (bridgeCall.MethodName is null)
        {
            return false;
        }

        EnsureRuntimeBridgeArgumentCount(call, bridgeCall.ArgumentCount);
        foreach (var argument in call.Arguments)
        {
            LoadValue(il, parameters, locals, argument.Type, argument.Value, fieldMap);
        }

        var bridgeMethod = typeof(RustMcil.AvaloniaSupport.AvaloniaBridge).GetMethod(bridgeCall.MethodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            ?? throw new InvalidOperationException($"Avalonia bridge method '{bridgeCall.MethodName}' could not be resolved.");
        il.Append(il.Create(OpCodes.Call, module.ImportReference(bridgeMethod)));
        return true;
    }

    private static void EnsureRuntimeBridgeArgumentCount(LoweredCallInstruction call, int expectedArgumentCount)
    {
        if (call.Arguments.Count != expectedArgumentCount)
        {
            throw new NotSupportedException($"Managed runtime bridge call '{call.Callee}' expected {expectedArgumentCount} argument(s), but received {call.Arguments.Count}.");
        }
    }

    private static void EmitRuntimeBridgeCall(ModuleDefinition module, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, FieldDefinition> fieldMap, LoweredCallInstruction call, string helperName)
    {
        foreach (var argument in call.Arguments)
        {
            LoadValue(il, parameters, locals, argument.Type, argument.Value, fieldMap);
        }

        var helperMethod = typeof(RuntimeBridgeHelpers).GetMethod(helperName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            ?? throw new InvalidOperationException($"Runtime bridge helper '{helperName}' could not be resolved.");
        il.Append(il.Create(OpCodes.Call, module.ImportReference(helperMethod)));
    }

    private static bool TryHandleMemoryIntrinsic(ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredCallInstruction call, IReadOnlyDictionary<string, LoweredGetElementPointerInstruction> addressMap, IDictionary<string, string> memoryAliases, string functionName, MemoryHelperMethods memoryHelpers, IReadOnlyDictionary<string, FieldDefinition> fieldMap)
    {
        if (string.Equals(call.Callee, "llvm.memset.p0.i64", StringComparison.Ordinal))
        {
            if (call.Arguments.Count < 4)
            {
                throw new NotSupportedException($"Memory intrinsic '{call.Callee}' in function '{functionName}' did not provide the expected argument list.");
            }

            if (long.TryParse(call.Arguments[2].Value, out var fillLength) && fillLength <= 0)
            {
                return true;
            }

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value, fieldMap);
            LoadValue(il, parameters, locals, call.Arguments[1].Type, call.Arguments[1].Value, fieldMap);
            LoadValue(il, parameters, locals, call.Arguments[2].Type, call.Arguments[2].Value, fieldMap);
            il.Append(il.Create(OpCodes.Call, memoryHelpers.SetBytesI64));
            return true;
        }

        if (!string.Equals(call.Callee, "llvm.memcpy.p0.p0.i64", StringComparison.Ordinal))
        {
            return false;
        }

        if (call.Arguments.Count < 4)
        {
            throw new NotSupportedException($"Memory intrinsic '{call.Callee}' in function '{functionName}' did not provide the expected argument list.");
        }

        var destinationBase = ResolveMemoryBase(call.Arguments[0].Value, addressMap);
        var sourceBase = ResolveMemoryBase(call.Arguments[1].Value, addressMap);
        if (long.TryParse(call.Arguments[2].Value, out var copyLength))
        {
            if (copyLength <= 0)
            {
                return true;
            }

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value, fieldMap);
            LoadValue(il, parameters, locals, call.Arguments[1].Type, call.Arguments[1].Value, fieldMap);
            il.Append(il.Create(OpCodes.Ldc_I8, copyLength));
            il.Append(il.Create(OpCodes.Call, memoryHelpers.CopyBytesI64));
            return true;
        }

        LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value, fieldMap);
        LoadValue(il, parameters, locals, call.Arguments[1].Type, call.Arguments[1].Value, fieldMap);
        LoadValue(il, parameters, locals, call.Arguments[2].Type, call.Arguments[2].Value, fieldMap);
        il.Append(il.Create(OpCodes.Call, memoryHelpers.CopyBytesI64));
        return true;
    }

    private static MemoryHelperMethods EmitMemoryHelpers(ModuleDefinition module, TypeDefinition generatedType)
    {
        var copyBytesI64 = new MethodDefinition(
            "__memory_copy_i64",
            MethodAttributes.Private | MethodAttributes.Static,
            module.TypeSystem.Void);

        copyBytesI64.Parameters.Add(new ParameterDefinition("destination", ParameterAttributes.None, module.TypeSystem.IntPtr));
        copyBytesI64.Parameters.Add(new ParameterDefinition("source", ParameterAttributes.None, module.TypeSystem.IntPtr));
        copyBytesI64.Parameters.Add(new ParameterDefinition("length", ParameterAttributes.None, module.TypeSystem.Int64));
        copyBytesI64.Body.InitLocals = true;

        var indexLocal = new VariableDefinition(module.TypeSystem.Int64);
        copyBytesI64.Body.Variables.Add(indexLocal);

        var il = copyBytesI64.Body.GetILProcessor();
        var loopStart = il.Create(OpCodes.Nop);
        var loopBody = il.Create(OpCodes.Nop);
        var done = il.Create(OpCodes.Ret);

        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(OpCodes.Conv_I8));
        il.Append(il.Create(OpCodes.Stloc, indexLocal));
        il.Append(loopStart);
        il.Append(il.Create(OpCodes.Ldloc, indexLocal));
        il.Append(il.Create(OpCodes.Ldarg, copyBytesI64.Parameters[2]));
        il.Append(il.Create(OpCodes.Clt));
        il.Append(il.Create(OpCodes.Brtrue, loopBody));
        il.Append(il.Create(OpCodes.Br, done));
        il.Append(loopBody);
        il.Append(il.Create(OpCodes.Ldarg, copyBytesI64.Parameters[0]));
        il.Append(il.Create(OpCodes.Ldloc, indexLocal));
        il.Append(il.Create(OpCodes.Conv_I4));
        il.Append(il.Create(OpCodes.Ldarg, copyBytesI64.Parameters[1]));
        il.Append(il.Create(OpCodes.Ldloc, indexLocal));
        il.Append(il.Create(OpCodes.Conv_I4));
        il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReadByte), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(int)])
            ?? throw new InvalidOperationException("Marshal.ReadByte(IntPtr, int) could not be resolved."))));
        il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.WriteByte), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(int), typeof(byte)])
            ?? throw new InvalidOperationException("Marshal.WriteByte(IntPtr, int, byte) could not be resolved."))));
        il.Append(il.Create(OpCodes.Ldloc, indexLocal));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(OpCodes.Conv_I8));
        il.Append(il.Create(OpCodes.Add));
        il.Append(il.Create(OpCodes.Stloc, indexLocal));
        il.Append(il.Create(OpCodes.Br, loopStart));
        il.Append(done);

        var setBytesI64 = new MethodDefinition(
            "__memory_set_i64",
            MethodAttributes.Private | MethodAttributes.Static,
            module.TypeSystem.Void);

        setBytesI64.Parameters.Add(new ParameterDefinition("destination", ParameterAttributes.None, module.TypeSystem.IntPtr));
        setBytesI64.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, module.TypeSystem.Byte));
        setBytesI64.Parameters.Add(new ParameterDefinition("length", ParameterAttributes.None, module.TypeSystem.Int64));
        setBytesI64.Body.InitLocals = true;

        var setIndexLocal = new VariableDefinition(module.TypeSystem.Int64);
        setBytesI64.Body.Variables.Add(setIndexLocal);

        var setIl = setBytesI64.Body.GetILProcessor();
        var setLoopStart = setIl.Create(OpCodes.Nop);
        var setLoopBody = setIl.Create(OpCodes.Nop);
        var setDone = setIl.Create(OpCodes.Ret);

        setIl.Append(setIl.Create(OpCodes.Ldc_I4_0));
        setIl.Append(setIl.Create(OpCodes.Conv_I8));
        setIl.Append(setIl.Create(OpCodes.Stloc, setIndexLocal));
        setIl.Append(setLoopStart);
        setIl.Append(setIl.Create(OpCodes.Ldloc, setIndexLocal));
        setIl.Append(setIl.Create(OpCodes.Ldarg, setBytesI64.Parameters[2]));
        setIl.Append(setIl.Create(OpCodes.Clt));
        setIl.Append(setIl.Create(OpCodes.Brtrue, setLoopBody));
        setIl.Append(setIl.Create(OpCodes.Br, setDone));
        setIl.Append(setLoopBody);
        setIl.Append(setIl.Create(OpCodes.Ldarg, setBytesI64.Parameters[0]));
        setIl.Append(setIl.Create(OpCodes.Ldloc, setIndexLocal));
        setIl.Append(setIl.Create(OpCodes.Conv_I4));
        setIl.Append(setIl.Create(OpCodes.Ldarg, setBytesI64.Parameters[1]));
        setIl.Append(setIl.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.WriteByte), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(int), typeof(byte)])
            ?? throw new InvalidOperationException("Marshal.WriteByte(IntPtr, int, byte) could not be resolved."))));
        setIl.Append(setIl.Create(OpCodes.Ldloc, setIndexLocal));
        setIl.Append(setIl.Create(OpCodes.Ldc_I4_1));
        setIl.Append(setIl.Create(OpCodes.Conv_I8));
        setIl.Append(setIl.Create(OpCodes.Add));
        setIl.Append(setIl.Create(OpCodes.Stloc, setIndexLocal));
        setIl.Append(setIl.Create(OpCodes.Br, setLoopStart));
        setIl.Append(setDone);

        generatedType.Methods.Add(copyBytesI64);
        generatedType.Methods.Add(setBytesI64);
        return new MemoryHelperMethods(copyBytesI64, setBytesI64);
    }

    private static bool TryHandleIgnoredIntrinsic(LoweredCallInstruction call)
    {
        return string.Equals(call.Callee, "llvm.experimental.noalias.scope.decl", StringComparison.Ordinal)
            || call.Callee.StartsWith("llvm.lifetime.start", StringComparison.Ordinal)
            || call.Callee.StartsWith("llvm.lifetime.end", StringComparison.Ordinal)
            || string.Equals(call.Callee, "llvm.assume", StringComparison.Ordinal);
    }

    private static string ResolveMemoryBase(string value, IReadOnlyDictionary<string, LoweredGetElementPointerInstruction> addressMap)
    {
        return addressMap.TryGetValue(value, out var address)
            ? address.Base
            : value;
    }

    private static bool TryStoreByteOffsetAddress(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, LoweredGetElementPointerInstruction address, string valueType, IReadOnlyDictionary<string, string> memoryAliases)
    {
        if (!string.Equals(address.ElementType, "i8", StringComparison.Ordinal))
        {
            return false;
        }

        var aliasedBase = ResolveAliasedBase(address.Base, memoryAliases);
        if (!TryGetLocalPrimitiveWidth(locals, aliasedBase, out var baseWidth))
        {
            return false;
        }

        var bitOffset = checked(address.Index * 8);
        if (!CanProjectPackedValue(baseWidth, bitOffset, valueType))
        {
            throw new NotSupportedException($"Byte-offset store into '{address.Base}' at byte index {address.Index} with type '{valueType}' is not supported by the current emitter slice.");
        }

        StoreLocal(method, il, locals, GetPackedByteOffsetSlotName(aliasedBase, address.Index), valueType);
        return true;
    }

    private static bool TryLoadByteOffsetAddress(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, LoweredLoadInstruction load, LoweredGetElementPointerInstruction address, IReadOnlyDictionary<string, string> memoryAliases)
    {
        if (!string.Equals(address.ElementType, "i8", StringComparison.Ordinal))
        {
            return false;
        }

        var aliasedBase = ResolveAliasedBase(address.Base, memoryAliases);
        if (locals.ContainsKey(GetPackedByteOffsetSlotName(aliasedBase, address.Index)))
        {
            LoadValue(il, new Dictionary<string, ParameterDefinition>(StringComparer.Ordinal), locals, load.Type, GetPackedByteOffsetSlotName(aliasedBase, address.Index));
            StoreLocal(method, il, locals, load.Result, load.Type);
            return true;
        }

        if (!TryGetLocalPrimitiveWidth(locals, aliasedBase, out var baseWidth))
        {
            return false;
        }

        var bitOffset = checked(address.Index * 8);
        if (!CanProjectPackedValue(baseWidth, bitOffset, load.Type))
        {
            throw new NotSupportedException($"Byte-offset load from '{address.Base}' at byte index {address.Index} with type '{load.Type}' is not supported by the current emitter slice.");
        }

        var baseType = baseWidth == 64 ? "i64" : "i32";
        LoadValue(il, new Dictionary<string, ParameterDefinition>(StringComparer.Ordinal), locals, baseType, aliasedBase);
        if (bitOffset > 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4, bitOffset));
            il.Append(il.Create(OpCodes.Shr_Un));
        }

        EmitConversion(il, load.Type);
        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static bool TryLoadPointerParameterAddress(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, ParameterDefinition> parameters, LoweredLoadInstruction load, LoweredGetElementPointerInstruction address, IReadOnlyDictionary<string, string> memoryAliases)
    {
        if (!string.Equals(address.ElementType, "i8", StringComparison.Ordinal))
        {
            return false;
        }

        var aliasedBase = ResolveAliasedBase(address.Base, memoryAliases);
        if (!parameters.TryGetValue(aliasedBase, out var parameter) || !IsPointerParameter(parameter))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldarg, parameter));
        if (address.Index != 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4, address.Index));
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Add));
        }

        EmitIndirectLoad(method, il, load.Type, $"Pointer-parameter load for lowered type '{load.Type}' is not supported by the current emitter slice.");

        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static bool TryLoadDirectPointerParameter(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, ParameterDefinition> parameters, LoweredLoadInstruction load, string functionName)
    {
        if (!parameters.TryGetValue(load.Source, out var parameter) || !IsPointerParameter(parameter))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldarg, parameter));
        EmitIndirectLoad(method, il, load.Type, $"Direct pointer-parameter load for lowered type '{load.Type}' is not supported in function '{functionName}'.");

        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static bool TryStoreDirectPointerParameter(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredStoreInstruction store, string functionName)
    {
        if (!parameters.TryGetValue(store.Destination, out var parameter) || !IsPointerParameter(parameter))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldarg, parameter));
        LoadValue(il, parameters, locals, store.Type, store.Value);
        EmitIndirectStore(method, il, store.Type, $"Direct pointer-parameter store for lowered type '{store.Type}' is not supported in function '{functionName}'.");

        return true;
    }

    private static bool TryStoreDirectPointerLocal(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredStoreInstruction store, string functionName)
    {
        if (!locals.TryGetValue(store.Destination, out var destinationLocal) || !IsPointerLocal(destinationLocal))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldloc, destinationLocal));
        LoadValue(il, parameters, locals, store.Type, store.Value);
        EmitIndirectStore(method, il, store.Type, $"Direct pointer-local store for lowered type '{store.Type}' is not supported in function '{functionName}'.");
        return true;
    }

    private static bool TryStorePointerParameterAddress(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredStoreInstruction store, LoweredGetElementPointerInstruction address, IReadOnlyDictionary<string, string> memoryAliases, string functionName)
    {
        if (!string.Equals(address.ElementType, "i8", StringComparison.Ordinal))
        {
            return false;
        }

        var aliasedBase = ResolveAliasedBase(address.Base, memoryAliases);
        if (!parameters.TryGetValue(aliasedBase, out var parameter) || !IsPointerParameter(parameter))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldarg, parameter));
        if (address.Index != 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4, address.Index));
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Add));
        }

        LoadValue(il, parameters, locals, store.Type, store.Value);
        EmitIndirectStore(method, il, store.Type, $"Pointer-parameter store for lowered type '{store.Type}' is not supported by the current emitter slice in function '{functionName}'.");
        return true;
    }

    private static bool TryLoadDirectRawAllocaAddress(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, LoweredLoadInstruction load, string functionName)
    {
        if (!TryGetRawAllocaAddressLocal(locals, load.Source, out var rawAddressLocal))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldloc, rawAddressLocal));
        EmitIndirectLoad(method, il, load.Type, $"Direct raw alloca load for lowered type '{load.Type}' is not supported in function '{functionName}'.");
        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static bool TryLoadDirectPointerLocal(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, LoweredLoadInstruction load, string functionName)
    {
        if (!locals.TryGetValue(load.Source, out var sourceLocal) || !IsPointerLocal(sourceLocal))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldloc, sourceLocal));
        EmitIndirectLoad(method, il, load.Type, $"Direct pointer-local load for lowered type '{load.Type}' is not supported in function '{functionName}'.");
        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static bool TryStoreDirectRawAllocaAddress(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredStoreInstruction store, string functionName)
    {
        if (!TryGetRawAllocaAddressLocal(locals, store.Destination, out var rawAddressLocal))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldloc, rawAddressLocal));
        LoadValue(il, parameters, locals, store.Type, store.Value);
        EmitIndirectStore(method, il, store.Type, $"Direct raw alloca store for lowered type '{store.Type}' is not supported in function '{functionName}'.");
        return true;
    }

    private static bool TryLoadRawAllocaAddress(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, LoweredLoadInstruction load, LoweredGetElementPointerInstruction address, IReadOnlyDictionary<string, string> memoryAliases, string functionName)
    {
        if (!string.Equals(address.ElementType, "i8", StringComparison.Ordinal))
        {
            return false;
        }

        var aliasedBase = ResolveAliasedBase(address.Base, memoryAliases);
        if (!TryGetRawAllocaAddressLocal(locals, aliasedBase, out var rawAddressLocal))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldloc, rawAddressLocal));
        if (address.Index != 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4, address.Index));
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Add));
        }

        EmitIndirectLoad(method, il, load.Type, $"Raw alloca load for lowered type '{load.Type}' is not supported in function '{functionName}'.");
        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static bool TryStoreRawAllocaAddress(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredStoreInstruction store, LoweredGetElementPointerInstruction address, IReadOnlyDictionary<string, string> memoryAliases, string functionName)
    {
        if (!string.Equals(address.ElementType, "i8", StringComparison.Ordinal))
        {
            return false;
        }

        var aliasedBase = ResolveAliasedBase(address.Base, memoryAliases);
        if (!TryGetRawAllocaAddressLocal(locals, aliasedBase, out var rawAddressLocal))
        {
            return false;
        }

        il.Append(il.Create(OpCodes.Ldloc, rawAddressLocal));
        if (address.Index != 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4, address.Index));
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Add));
        }

        LoadValue(il, parameters, locals, store.Type, store.Value);
        EmitIndirectStore(method, il, store.Type, $"Raw alloca store for lowered type '{store.Type}' is not supported in function '{functionName}'.");
        return true;
    }

    private static string ResolveAliasedBase(string baseName, IReadOnlyDictionary<string, string> memoryAliases)
    {
        return memoryAliases.TryGetValue(baseName, out var alias)
            ? alias
            : baseName;
    }

    private static HashSet<string> CollectRawAddressAllocas(LoweredFunction function)
    {
        var byteArrayAllocas = function.Blocks
            .SelectMany(static block => block.Instructions)
            .OfType<LoweredAllocaInstruction>()
            .Where(static alloca => TryGetByteArrayAllocaSize(alloca.Type, out _))
            .Select(static alloca => alloca.Result)
            .ToHashSet(StringComparer.Ordinal);

        var rawAddressAllocas = new HashSet<string>(StringComparer.Ordinal);
        foreach (var instruction in function.Blocks.SelectMany(static block => block.Instructions))
        {
            switch (instruction)
            {
                case LoweredCallInstruction call:
                    foreach (var argument in call.Arguments)
                    {
                        if (string.Equals(argument.Type, "ptr", StringComparison.Ordinal)
                            && byteArrayAllocas.Contains(argument.Value))
                        {
                            rawAddressAllocas.Add(argument.Value);
                        }
                    }

                    break;

                case LoweredLoadInstruction load when byteArrayAllocas.Contains(load.Source):
                    rawAddressAllocas.Add(load.Source);
                    break;

                case LoweredStoreInstruction store when byteArrayAllocas.Contains(store.Destination):
                    rawAddressAllocas.Add(store.Destination);
                    break;
            }
        }

        return rawAddressAllocas;
    }

    private static bool TryGetByteArrayAllocaSize(string typeName, out int byteCount)
    {
        byteCount = 0;
        if (!typeName.StartsWith("[", StringComparison.Ordinal)
            || !typeName.EndsWith("]", StringComparison.Ordinal)
            || !typeName.Contains(" x i8", StringComparison.Ordinal))
        {
            return false;
        }

        var separatorIndex = typeName.IndexOf(" x i8", StringComparison.Ordinal);
        return int.TryParse(typeName[1..separatorIndex], out byteCount) && byteCount >= 0;
    }

    private static void EmitRawByteArrayAlloca(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, string localName, int byteCount)
    {
        if (byteCount == 0)
        {
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(OpCodes.Conv_I));
        }
        else
        {
            il.Append(il.Create(OpCodes.Ldc_I4, byteCount));
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.AllocHGlobal), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)])
                ?? throw new InvalidOperationException("Marshal.AllocHGlobal(IntPtr) could not be resolved."))));
        }

        StoreLocal(method, il, locals, GetAllocaAddressLocalName(localName), method.Module.TypeSystem.IntPtr);
    }

    private static bool TryGetRawAllocaAddressLocal(IDictionary<string, VariableDefinition> locals, string baseName, out VariableDefinition local)
    {
        return locals.TryGetValue(GetAllocaAddressLocalName(baseName), out local!);
    }

    private static string GetAllocaAddressLocalName(string baseName)
    {
        return $"$addr.{baseName}";
    }

    private static void EmitIndirectLoad(MethodDefinition method, ILProcessor il, string typeName, string errorMessage)
    {
        il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeName switch
        {
            "i8" => typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReadByte), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)]),
            "i32" => typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReadInt32), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)]),
            "i64" => typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReadInt64), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)]),
            "ptr" => typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReadIntPtr), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)]),
            _ => throw new NotSupportedException(errorMessage)
        } ?? throw new InvalidOperationException($"Marshal read helper for '{typeName}' could not be resolved."))));
    }

    private static void EmitIndirectStore(MethodDefinition method, ILProcessor il, string typeName, string errorMessage)
    {
        switch (typeName)
        {
            case "i8":
                il.Append(il.Create(OpCodes.Conv_U1));
                il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.WriteByte), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(byte)])
                    ?? throw new InvalidOperationException("Marshal.WriteByte(IntPtr, byte) could not be resolved."))));
                return;

            case "i32":
                il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.WriteInt32), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(int)])
                    ?? throw new InvalidOperationException("Marshal.WriteInt32(IntPtr, int) could not be resolved."))));
                return;

            case "i64":
                il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.WriteInt64), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(long)])
                    ?? throw new InvalidOperationException("Marshal.WriteInt64(IntPtr, long) could not be resolved."))));
                return;

            case "ptr":
                il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.WriteIntPtr), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr), typeof(IntPtr)])
                    ?? throw new InvalidOperationException("Marshal.WriteIntPtr(IntPtr, IntPtr) could not be resolved."))));
                return;

            default:
                throw new NotSupportedException(errorMessage);
        }
    }

    private static bool IsPointerParameter(ParameterDefinition parameter)
    {
        return parameter.ParameterType.MetadataType == MetadataType.IntPtr;
    }

    private static bool IsPointerLocal(VariableDefinition local)
    {
        return local.VariableType.MetadataType == MetadataType.IntPtr;
    }

    private static bool TryGetLocalPrimitiveWidth(IDictionary<string, VariableDefinition> locals, string localName, out int width)
    {
        width = 0;
        if (!locals.TryGetValue(localName, out var local))
        {
            return false;
        }

        if (local.VariableType.MetadataType == MetadataType.Int64)
        {
            width = 64;
            return true;
        }

        if (local.VariableType.MetadataType == MetadataType.Int32)
        {
            width = 32;
            return true;
        }

        return false;
    }

    private static bool CanProjectPackedValue(int baseWidth, int bitOffset, string valueType)
    {
        var targetWidth = valueType switch
        {
            "i32" => 32,
            "i64" => 64,
            _ => 0
        };

        return targetWidth > 0 && bitOffset >= 0 && bitOffset + targetWidth <= baseWidth;
    }

    private static bool TryEmitKnownAggregateIntrinsic(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredCallInstruction call, string functionName)
    {
        if (call.Result is null)
        {
            return false;
        }

        if (call.Callee.Contains("memchr_aligned", StringComparison.Ordinal))
        {
            if (call.Arguments.Count != 3)
            {
                throw new NotSupportedException($"Aggregate memchr helper '{call.Callee}' in function '{functionName}' did not provide the expected argument list.");
            }

            var needleLocalName = GetAggregateComponentLocalName(call.Result, 20);
            var baseLocalName = GetAggregateComponentLocalName(call.Result, 21);
            var lengthLocalName = GetAggregateComponentLocalName(call.Result, 22);
            var indexLocalName = GetAggregateComponentLocalName(call.Result, 23);
            var foundLocalName = GetAggregateComponentLocalName(call.Result, 0);
            var resultIndexLocalName = GetAggregateComponentLocalName(call.Result, 1);
            var loopCheck = il.Create(OpCodes.Nop);
            var loopBody = il.Create(OpCodes.Nop);
            var foundLabel = il.Create(OpCodes.Nop);
            var doneLabel = il.Create(OpCodes.Nop);

            LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value);
            StoreLocal(method, il, locals, needleLocalName, call.Arguments[0].Type);
            LoadValue(il, parameters, locals, call.Arguments[1].Type, call.Arguments[1].Value);
            StoreLocal(method, il, locals, baseLocalName, "ptr");
            LoadValue(il, parameters, locals, call.Arguments[2].Type, call.Arguments[2].Value);
            StoreLocal(method, il, locals, lengthLocalName, "i64");

            EmitConstant(il, "i64", 0);
            StoreLocal(method, il, locals, indexLocalName, "i64");
            EmitConstant(il, "i64", 0);
            StoreLocal(method, il, locals, foundLocalName, "i64");
            EmitConstant(il, "i64", 0);
            StoreLocal(method, il, locals, resultIndexLocalName, "i64");
            il.Append(il.Create(OpCodes.Br, loopCheck));

            il.Append(loopBody);
            LoadValue(il, parameters, locals, "ptr", baseLocalName);
            LoadValue(il, parameters, locals, "i64", indexLocalName);
            il.Append(il.Create(OpCodes.Conv_I));
            il.Append(il.Create(OpCodes.Add));
            il.Append(il.Create(OpCodes.Call, method.Module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.ReadByte), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)])
                ?? throw new InvalidOperationException("Marshal.ReadByte(IntPtr) could not be resolved."))));
            LoadValue(il, parameters, locals, call.Arguments[0].Type, needleLocalName);
            il.Append(il.Create(OpCodes.Ceq));
            il.Append(il.Create(OpCodes.Brtrue, foundLabel));

            LoadValue(il, parameters, locals, "i64", indexLocalName);
            EmitConstant(il, "i64", 1);
            il.Append(il.Create(OpCodes.Add));
            StoreLocal(method, il, locals, indexLocalName, "i64");

            il.Append(loopCheck);
            LoadValue(il, parameters, locals, "i64", indexLocalName);
            LoadValue(il, parameters, locals, "i64", lengthLocalName);
            il.Append(il.Create(OpCodes.Clt));
            il.Append(il.Create(OpCodes.Brtrue, loopBody));
            il.Append(il.Create(OpCodes.Br, doneLabel));

            il.Append(foundLabel);
            EmitConstant(il, "i64", 1);
            StoreLocal(method, il, locals, foundLocalName, "i64");
            LoadValue(il, parameters, locals, "i64", indexLocalName);
            StoreLocal(method, il, locals, resultIndexLocalName, "i64");
            il.Append(il.Create(OpCodes.Br, doneLabel));

            il.Append(doneLabel);
            return true;
        }

        if (!string.Equals(call.Callee, "llvm.umul.with.overflow.i64", StringComparison.Ordinal))
        {
            return false;
        }

        if (call.Arguments.Count != 2)
        {
            throw new NotSupportedException($"Aggregate intrinsic '{call.Callee}' in function '{functionName}' did not provide the expected argument list.");
        }

        var leftLocalName = GetAggregateComponentLocalName(call.Result, 10);
        var rightLocalName = GetAggregateComponentLocalName(call.Result, 11);
        var resultLocalName = GetAggregateComponentLocalName(call.Result, 0);
        var overflowLocalName = GetAggregateComponentLocalName(call.Result, 1);
        var zeroLabel = il.Create(OpCodes.Nop);
        var endLabel = il.Create(OpCodes.Nop);

        LoadValue(il, parameters, locals, call.Arguments[0].Type, call.Arguments[0].Value);
        StoreLocal(method, il, locals, leftLocalName, "i64");
        LoadValue(il, parameters, locals, call.Arguments[1].Type, call.Arguments[1].Value);
        StoreLocal(method, il, locals, rightLocalName, "i64");

        LoadValue(il, parameters, locals, "i64", leftLocalName);
        LoadValue(il, parameters, locals, "i64", rightLocalName);
        il.Append(il.Create(OpCodes.Mul));
        StoreLocal(method, il, locals, resultLocalName, "i64");

        LoadValue(il, parameters, locals, "i64", leftLocalName);
        il.Append(il.Create(OpCodes.Ldc_I8, 0L));
        il.Append(il.Create(OpCodes.Ceq));
        il.Append(il.Create(OpCodes.Brtrue, zeroLabel));

        LoadValue(il, parameters, locals, "i64", rightLocalName);
        il.Append(il.Create(OpCodes.Ldc_I8, -1L));
        LoadValue(il, parameters, locals, "i64", leftLocalName);
        il.Append(il.Create(OpCodes.Div_Un));
        il.Append(il.Create(OpCodes.Cgt_Un));
        StoreLocal(method, il, locals, overflowLocalName, method.Module.TypeSystem.Int32);
        il.Append(il.Create(OpCodes.Br, endLabel));

        il.Append(zeroLabel);
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        StoreLocal(method, il, locals, overflowLocalName, method.Module.TypeSystem.Int32);
        il.Append(endLabel);
        return true;
    }

    private static string GetAggregateComponentLocalName(string baseName, int index)
    {
        return $"$agg.{baseName}.{index}";
    }

    private static string GetPackedByteOffsetSlotName(string baseName, int byteOffset)
    {
        return $"{baseName}@{byteOffset}";
    }

    private static bool TryResolveConstantGlobalElement(LoweredLoadInstruction load, IReadOnlyDictionary<string, LoweredGlobal> globalMap, out long value)
    {
        value = 0;

        var bracketIndex = load.Source.IndexOf('[', StringComparison.Ordinal);
        if (bracketIndex <= 0 || !load.Source.EndsWith(']'))
        {
            return false;
        }

        var globalName = load.Source[..bracketIndex];
        var indexText = load.Source[(bracketIndex + 1)..^1];
        if (!globalMap.TryGetValue(globalName, out var global) || !int.TryParse(indexText, out var elementIndex) || elementIndex < 0)
        {
            return false;
        }

        var elementSize = GetPrimitiveByteSize(load.Type);
        var byteOffset = elementIndex * elementSize;
        if (byteOffset + elementSize > global.InitializerBytes.Count)
        {
            throw new NotSupportedException($"Global element access '{load.Source}' exceeds the constant initializer size for '{globalName}'.");
        }

        var bytes = global.InitializerBytes.Skip(byteOffset).Take(elementSize).ToArray();
        value = load.Type switch
        {
            "i32" => BitConverter.ToInt32(bytes, 0),
            "i64" => BitConverter.ToInt64(bytes, 0),
            _ => throw new NotSupportedException($"Global element access for element type '{load.Type}' is not supported by the current emitter slice.")
        };

        return true;
    }

    private static bool TryResolveAnonymousConstantElement(LoweredLoadInstruction load, out long value)
    {
        value = 0;

        if (!load.Source.StartsWith("anon.", StringComparison.Ordinal))
        {
            return false;
        }

        return string.Equals(load.Type, "ptr", StringComparison.Ordinal)
            || string.Equals(load.Type, "i32", StringComparison.Ordinal)
            || string.Equals(load.Type, "i64", StringComparison.Ordinal);
    }

    private static string GetIndexedSlotName(LoweredGetElementPointerInstruction address)
    {
        return $"{address.Base}[{address.Index}]";
    }

    private static int GetPrimitiveByteSize(string typeName)
    {
        return typeName switch
        {
            "i8" => 1,
            "i32" => 4,
            "i64" => 8,
            _ => throw new NotSupportedException($"Primitive byte size is not defined for lowered type '{typeName}'.")
        };
    }

    private static int GetGetElementPointerStride(string elementType)
    {
        return elementType switch
        {
            "ptr" => IntPtr.Size,
            _ => GetPrimitiveByteSize(elementType)
        };
    }

    private static void EmitConstant(ILProcessor il, string typeName, long constant)
    {
        il.Append(il.Create(OpCodes.Ldc_I8, constant));
        EmitConversion(il, typeName);
    }

    private static void EmitZeroValue(ILProcessor il, string typeName)
    {
        if (TryGetIntegerBitWidth(typeName, out _))
        {
            EmitConstant(il, typeName, 0);
            return;
        }

        if (string.Equals(typeName, "ptr", StringComparison.Ordinal))
        {
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(OpCodes.Conv_I));
            return;
        }

        throw new NotSupportedException($"Zero initialization for lowered type '{typeName}' is not supported by the current emitter slice.");
    }

    private static void EmitThrow(ILProcessor il, ModuleDefinition module, Type exceptionType)
    {
        var constructor = exceptionType.GetConstructor(Type.EmptyTypes)
            ?? throw new InvalidOperationException($"Exception type '{exceptionType.FullName}' does not expose a public parameterless constructor.");

        il.Append(il.Create(OpCodes.Newobj, module.ImportReference(constructor)));
        il.Append(il.Create(OpCodes.Throw));
    }

    private static void EmitZeroExtension(ILProcessor il, string fromType, string toType, string functionName)
    {
        if (string.Equals(fromType, toType, StringComparison.Ordinal))
        {
            return;
        }

        if (TryGetIntegerBitWidth(fromType, out var fromWidth)
            && TryGetIntegerBitWidth(toType, out var toWidth)
            && fromWidth < toWidth
            && toWidth <= 64)
        {
            if (fromWidth < 32)
            {
                EmitIntegerMask(il, fromWidth);
            }

            if (toWidth <= 32)
            {
                il.Append(il.Create(OpCodes.Conv_U4));
            }
            else
            {
                il.Append(il.Create(OpCodes.Conv_U8));
            }

            EmitIntegerWidthNormalization(il, toType, null, functionName);
            return;
        }

        if (string.Equals(fromType, "i1", StringComparison.Ordinal))
        {
            switch (toType)
            {
                case "i32":
                    il.Append(il.Create(OpCodes.Conv_U4));
                    return;
                case "i64":
                    il.Append(il.Create(OpCodes.Conv_U8));
                    return;
            }
        }

        throw new NotSupportedException($"Zero extension from '{fromType}' to '{toType}' is not supported by the current emitter slice in function '{functionName}'.");
    }

    private static void EmitSignExtension(ILProcessor il, string fromType, string toType, string functionName)
    {
        if (string.Equals(fromType, toType, StringComparison.Ordinal))
        {
            return;
        }

        if (string.Equals(fromType, "i1", StringComparison.Ordinal))
        {
            il.Append(il.Create(OpCodes.Neg));

            switch (toType)
            {
                case "i32":
                    return;
                case "i64":
                    il.Append(il.Create(OpCodes.Conv_I8));
                    return;
            }
        }

        if (TryGetIntegerBitWidth(fromType, out var fromWidth)
            && TryGetIntegerBitWidth(toType, out var toWidth)
            && fromWidth < toWidth
            && toWidth <= 64)
        {
            if (fromWidth <= 32 && toWidth <= 32)
            {
                il.Append(il.Create(OpCodes.Conv_I4));
                return;
            }

            if (fromWidth <= 32 && toWidth <= 64)
            {
                il.Append(il.Create(OpCodes.Conv_I8));
                EmitIntegerWidthNormalization(il, toType, null, functionName);
                return;
            }
        }

        if (string.Equals(fromType, "i32", StringComparison.Ordinal)
            && string.Equals(toType, "i64", StringComparison.Ordinal))
        {
            il.Append(il.Create(OpCodes.Conv_I8));
            return;
        }

        throw new NotSupportedException($"Sign extension from '{fromType}' to '{toType}' is not supported by the current emitter slice in function '{functionName}'.");
    }

    private static void EmitSelect(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredSelectInstruction select)
    {
        var trueLabel = il.Create(OpCodes.Nop);
        var endLabel = il.Create(OpCodes.Nop);

        LoadValue(il, parameters, locals, "i32", select.Condition);
        il.Append(il.Create(OpCodes.Brtrue, trueLabel));
        LoadValue(il, parameters, locals, select.ValueType, select.FalseValue);
        il.Append(il.Create(OpCodes.Br, endLabel));
        il.Append(trueLabel);
        LoadValue(il, parameters, locals, select.ValueType, select.TrueValue);
        il.Append(endLabel);
        StoreLocal(method, il, locals, select.Result, select.ValueType);
    }

    private static void EmitPhiCopiesForEdge(MethodDefinition method, ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock, string sourceBlock, string targetBlock)
    {
        if (!phiByBlock.TryGetValue(targetBlock, out var phiInstructions) || phiInstructions.Length == 0)
        {
            return;
        }

        var edgeCopies = new List<(LoweredPhiInstruction Phi, LoweredPhiIncoming Incoming)>();

        foreach (var phi in phiInstructions)
        {
            var matchingIncoming = phi.Incoming
                .Where(incoming => string.Equals(incoming.SourceBlock, sourceBlock, StringComparison.Ordinal))
                .ToArray();

            if (matchingIncoming.Length == 0)
            {
                continue;
            }

            if (matchingIncoming.Length > 1)
            {
                var distinctValues = matchingIncoming
                    .Select(static incoming => incoming.Value)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                if (distinctValues.Length > 1)
                {
                    throw new NotSupportedException($"Phi '{phi.Result}' in block '{targetBlock}' has multiple distinct incoming values from source block '{sourceBlock}', which is not supported by the current emitter slice.");
                }
            }

            edgeCopies.Add((phi, matchingIncoming[0]));
        }

        foreach (var edge in edgeCopies)
        {
            var tempLocalName = GetPhiTempLocalName(targetBlock, edge.Phi.Result, sourceBlock);

            if (TryGetAggregatePairTypes(edge.Phi.Type, out var firstType, out var secondType))
            {
                var incomingFirst = GetAggregateComponentLocalName(edge.Incoming.Value, 0);
                var incomingSecond = GetAggregateComponentLocalName(edge.Incoming.Value, 1);
                var tempFirst = GetAggregateComponentLocalName(tempLocalName, 0);
                var tempSecond = GetAggregateComponentLocalName(tempLocalName, 1);

                LoadValue(il, parameters, locals, firstType, incomingFirst);
                StoreLocal(method, il, locals, tempFirst, firstType);
                LoadValue(il, parameters, locals, secondType, incomingSecond);
                StoreLocal(method, il, locals, tempSecond, secondType);
                continue;
            }

            LoadValue(il, parameters, locals, edge.Phi.Type, edge.Incoming.Value);
            StoreLocal(method, il, locals, tempLocalName, edge.Phi.Type);
        }

        foreach (var edge in edgeCopies)
        {
            var tempLocalName = GetPhiTempLocalName(targetBlock, edge.Phi.Result, sourceBlock);

            if (TryGetAggregatePairTypes(edge.Phi.Type, out var firstType, out var secondType))
            {
                var tempFirst = GetAggregateComponentLocalName(tempLocalName, 0);
                var tempSecond = GetAggregateComponentLocalName(tempLocalName, 1);
                var resultFirst = GetAggregateComponentLocalName(edge.Phi.Result, 0);
                var resultSecond = GetAggregateComponentLocalName(edge.Phi.Result, 1);

                LoadValue(il, parameters, locals, firstType, tempFirst);
                StoreLocal(method, il, locals, resultFirst, firstType);
                LoadValue(il, parameters, locals, secondType, tempSecond);
                StoreLocal(method, il, locals, resultSecond, secondType);
                continue;
            }

            LoadValue(il, parameters, locals, edge.Phi.Type, tempLocalName);
            StoreLocal(method, il, locals, edge.Phi.Result, edge.Phi.Type);
        }
    }

    private static string GetPhiTempLocalName(string targetBlock, string resultName, string sourceBlock)
    {
        return $"$phi.{targetBlock}.{resultName}.{sourceBlock}";
    }

    private static bool TryGetAggregatePairTypes(string typeName, out string firstType, out string secondType)
    {
        var match = System.Text.RegularExpressions.Regex.Match(
            typeName,
            "^\\{ (?<first>[^,]+), (?<second>[^}]+) \\}$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            firstType = string.Empty;
            secondType = string.Empty;
            return false;
        }

        firstType = match.Groups["first"].Value.Trim();
        secondType = match.Groups["second"].Value.Trim();
        return true;
    }

    private static string GetAggregateTupleLocalName(string baseName)
    {
        return $"$agg.{baseName}";
    }

    private static void StoreCallResult(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, string resultName, string returnType, string functionName, bool skipWidthNormalization)
    {
        if (TryGetAggregatePairTypes(returnType, out var firstType, out var secondType))
        {
            StoreAggregatePairResult(method, il, locals, resultName, firstType, secondType);
            return;
        }

        if (!skipWidthNormalization)
        {
            EmitIntegerWidthNormalization(il, returnType, null, functionName);
        }

        StoreLocal(method, il, locals, resultName, returnType);
    }

    private static void StoreAggregatePairResult(MethodDefinition method, ILProcessor il, IDictionary<string, VariableDefinition> locals, string resultName, string firstType, string secondType)
    {
        var aggregateType = $"{{ {firstType}, {secondType} }}";
        var tupleLocal = EnsureLocal(method, locals, GetAggregateTupleLocalName(resultName), ResolveTypeReference(method.Module, aggregateType));
        il.Append(il.Create(OpCodes.Stloc, tupleLocal));

        il.Append(il.Create(OpCodes.Ldloc, tupleLocal));
        il.Append(il.Create(OpCodes.Callvirt, CreateAggregatePairItemGetter(method.Module, aggregateType, firstType, secondType, itemIndex: 0)));
        StoreLocal(method, il, locals, GetAggregateComponentLocalName(resultName, 0), firstType);

        il.Append(il.Create(OpCodes.Ldloc, tupleLocal));
        il.Append(il.Create(OpCodes.Callvirt, CreateAggregatePairItemGetter(method.Module, aggregateType, firstType, secondType, itemIndex: 1)));
        StoreLocal(method, il, locals, GetAggregateComponentLocalName(resultName, 1), secondType);
    }

    private static MethodReference CreateAggregatePairItemGetter(ModuleDefinition module, string aggregateType, string firstType, string secondType, int itemIndex)
    {
        var runtimeTupleType = ResolveAggregateRuntimeType(firstType, secondType);
        var propertyName = itemIndex == 0 ? "Item1" : "Item2";
        var getter = runtimeTupleType.GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.GetMethod
            ?? throw new InvalidOperationException($"Tuple getter '{propertyName}' could not be resolved for aggregate type '{aggregateType}'.");
        return module.ImportReference(getter);
    }

    private static MethodReference CreateAggregatePairConstructor(ModuleDefinition module, string aggregateType)
    {
        if (!TryGetAggregatePairTypes(aggregateType, out var firstType, out var secondType))
        {
            throw new InvalidOperationException($"Aggregate type '{aggregateType}' could not be parsed.");
        }

        var runtimeTupleType = ResolveAggregateRuntimeType(firstType, secondType);
        var constructor = runtimeTupleType.GetConstructor([ResolveRuntimeType(firstType), ResolveRuntimeType(secondType)])
            ?? throw new InvalidOperationException($"Tuple constructor could not be resolved for aggregate type '{aggregateType}'.");
        return module.ImportReference(constructor);
    }

    private static void EmitConversion(ILProcessor il, string typeName)
    {
        if (TryGetIntegerBitWidth(typeName, out var width))
        {
            il.Append(width <= 32
                ? il.Create(OpCodes.Conv_I4)
                : il.Create(OpCodes.Conv_I8));
        }
    }

    private static TypeReference ResolveTypeReference(ModuleDefinition module, string typeName)
    {
        if (TryGetAggregatePairTypes(typeName, out var firstType, out var secondType))
        {
            return module.ImportReference(ResolveAggregateRuntimeType(firstType, secondType));
        }

        if (TryGetIntegerBitWidth(typeName, out var width))
        {
            return width <= 32
                ? module.TypeSystem.Int32
                : module.TypeSystem.Int64;
        }

        if (TryGetSupportedVectorType(typeName, out var vectorType))
        {
            return new ArrayType(GetVectorElementType(module, vectorType));
        }

        return typeName switch
        {
            "ptr" => module.TypeSystem.IntPtr,
            "void" => module.TypeSystem.Void,
            _ => throw new NotSupportedException($"Lowered type '{typeName}' is not supported by the first emitter slice.")
        };
    }

    private static Type ResolveAggregateRuntimeType(string firstType, string secondType)
    {
        return typeof(Tuple<,>).MakeGenericType(ResolveRuntimeType(firstType), ResolveRuntimeType(secondType));
    }

    private static Type ResolveRuntimeType(string typeName)
    {
        if (TryGetIntegerBitWidth(typeName, out var width))
        {
            return width <= 32 ? typeof(int) : typeof(long);
        }

        return typeName switch
        {
            "ptr" => typeof(IntPtr),
            _ => throw new NotSupportedException($"Lowered runtime type '{typeName}' is not supported for aggregate imports.")
        };
    }

    private static TypeReference ResolveGlobalFieldType(ModuleDefinition module, LoweredGlobal global)
    {
        return ResolveGlobalFieldType(module, global, false);
    }

    private static TypeReference ResolveGlobalFieldType(ModuleDefinition module, LoweredGlobal global, bool preferPointerStorage)
    {
        if (preferPointerStorage)
        {
            return module.TypeSystem.IntPtr;
        }

        return global.InitializerBytes.Count switch
        {
            4 => module.TypeSystem.Int32,
            8 => module.TypeSystem.Int64,
            _ => module.TypeSystem.IntPtr
        };
    }

    private static HashSet<string> CollectPointerBackedGlobals(IReadOnlyList<LoweredFunction> functions)
    {
        var pointerBackedGlobals = new HashSet<string>(StringComparer.Ordinal);

        foreach (var function in functions)
        {
            foreach (var call in function.Blocks.SelectMany(static block => block.Instructions).OfType<LoweredCallInstruction>())
            {
                foreach (var argument in call.Arguments)
                {
                    if (string.Equals(argument.Type, "ptr", StringComparison.Ordinal)
                        && !string.IsNullOrEmpty(argument.Value)
                        && !argument.Value.StartsWith('%')
                        && !argument.Value.StartsWith("tmp.", StringComparison.Ordinal)
                        && !argument.Value.StartsWith("vtable.", StringComparison.Ordinal))
                    {
                        pointerBackedGlobals.Add(argument.Value);
                    }
                }
            }
        }

        return pointerBackedGlobals;
    }

    private static void EmitTypeInitializer(ModuleDefinition module, TypeDefinition generatedType, IReadOnlyList<LoweredGlobal> globals, IReadOnlyDictionary<string, FieldDefinition> fieldMap)
    {
        var typeInitializer = new MethodDefinition(
            ".cctor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
            module.TypeSystem.Void);

        generatedType.Methods.Add(typeInitializer);
        typeInitializer.Body.InitLocals = true;
        var globalBufferLocal = new VariableDefinition(module.TypeSystem.IntPtr);
        typeInitializer.Body.Variables.Add(globalBufferLocal);
        var il = typeInitializer.Body.GetILProcessor();

        foreach (var global in globals)
        {
            var field = fieldMap[global.Name];
            if (string.Equals(field.FieldType.FullName, module.TypeSystem.IntPtr.FullName, StringComparison.Ordinal))
            {
                il.Append(il.Create(OpCodes.Ldc_I4, global.InitializerBytes.Count));
                il.Append(il.Create(OpCodes.Conv_I));
                il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.AllocHGlobal), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(IntPtr)])
                    ?? throw new InvalidOperationException("Marshal.AllocHGlobal(IntPtr) could not be resolved."))));
                il.Append(il.Create(OpCodes.Stloc, globalBufferLocal));
                il.Append(il.Create(OpCodes.Ldc_I4, global.InitializerBytes.Count));
                il.Append(il.Create(OpCodes.Newarr, module.TypeSystem.Byte));

                for (var i = 0; i < global.InitializerBytes.Count; i++)
                {
                    il.Append(il.Create(OpCodes.Dup));
                    il.Append(il.Create(OpCodes.Ldc_I4, i));
                    il.Append(il.Create(OpCodes.Ldc_I4, (int)global.InitializerBytes[i]));
                    il.Append(il.Create(OpCodes.Stelem_I1));
                }

                il.Append(il.Create(OpCodes.Ldc_I4_0));
                il.Append(il.Create(OpCodes.Ldloc, globalBufferLocal));
                il.Append(il.Create(OpCodes.Ldc_I4, global.InitializerBytes.Count));
                il.Append(il.Create(OpCodes.Call, module.ImportReference(typeof(System.Runtime.InteropServices.Marshal).GetMethod(nameof(System.Runtime.InteropServices.Marshal.Copy), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(byte[]), typeof(int), typeof(IntPtr), typeof(int)])
                    ?? throw new InvalidOperationException("Marshal.Copy(byte[], int, IntPtr, int) could not be resolved."))));
                il.Append(il.Create(OpCodes.Ldloc, globalBufferLocal));
            }
            else
            {
                switch (global.InitializerBytes.Count)
                {
                    case 4:
                        il.Append(il.Create(OpCodes.Ldc_I4, BitConverter.ToInt32([.. global.InitializerBytes], 0)));
                        break;
                    case 8:
                        il.Append(il.Create(OpCodes.Ldc_I8, BitConverter.ToInt64([.. global.InitializerBytes], 0)));
                        break;
                    default:
                        throw new NotSupportedException($"Scalar global '{global.Name}' with initializer size {global.InitializerBytes.Count} is not supported by the current emitter slice.");
                }
            }

            il.Append(il.Create(OpCodes.Stsfld, field));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static bool TryParseIntegerConstant(string typeName, string value, out long constant)
    {
        if (string.Equals(value, "poison", StringComparison.Ordinal))
        {
            constant = 0;
            return TryGetIntegerBitWidth(typeName, out var poisonWidth)
                && poisonWidth <= 64;
        }

        if (TryGetIntegerBitWidth(typeName, out var width) && width <= 64)
        {
            if (width == 1 && int.TryParse(value, out var bitValue) && (bitValue == 0 || bitValue == 1))
            {
                constant = bitValue;
                return true;
            }

            if (long.TryParse(value, out var parsedValue))
            {
                constant = parsedValue;
                return true;
            }
        }

        constant = 0;
        return false;
    }

    private static void EmitIntegerWidthNormalization(ILProcessor il, string typeName, string? operation, string functionName)
    {
        if (!TryGetIntegerBitWidth(typeName, out var width) || width == 1 || width == 32 || width == 64)
        {
            return;
        }

        if (width > 64)
        {
            throw new NotSupportedException($"Integer width '{typeName}' is not supported by the current emitter slice in function '{functionName}'.");
        }

        if (string.Equals(operation, "ashr", StringComparison.Ordinal))
        {
            throw new NotSupportedException($"Arithmetic right shift for non-native integer width '{typeName}' is not supported by the current emitter slice in function '{functionName}'.");
        }

        EmitIntegerMask(il, width);
        il.Append(il.Create(OpCodes.Conv_I8));
    }

    private static void EmitIntegerMask(ILProcessor il, int width)
    {
        if (width <= 0 || width >= 64)
        {
            return;
        }

        var mask = (1L << width) - 1;
        il.Append(il.Create(OpCodes.Ldc_I8, mask));
        il.Append(il.Create(OpCodes.And));
    }

    private static bool TryGetIntegerBitWidth(string typeName, out int width)
    {
        width = 0;

        if (string.IsNullOrWhiteSpace(typeName)
            || typeName.Length < 2
            || typeName[0] != 'i'
            || !int.TryParse(typeName[1..], out width)
            || width <= 0)
        {
            return false;
        }

        return true;
    }

    private static void EmitVectorBinaryOperation(ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, LoweredBinaryInstruction binary, string functionName, VectorHelperMethods vectorHelpers)
    {
        LoadValue(il, parameters, locals, binary.Type, binary.Left);
        LoadValue(il, parameters, locals, binary.Type, binary.Right);

        var helperMethod = binary.Type switch
        {
            "<16 x i8>" => binary.Operation switch
            {
                "add" => vectorHelpers.AddI8x16,
                "or" => vectorHelpers.OrI8x16,
                "xor" => vectorHelpers.XorI8x16,
                _ => null
            },
            "<8 x i8>" => binary.Operation switch
            {
                "add" => vectorHelpers.AddI8x8,
                "or" => vectorHelpers.OrI8x8,
                "xor" => vectorHelpers.XorI8x8,
                _ => null
            },
            "<4 x i8>" => binary.Operation switch
            {
                "add" => vectorHelpers.AddI8x4,
                "and" => vectorHelpers.AndI8x4,
                "or" => vectorHelpers.OrI8x4,
                "xor" => vectorHelpers.XorI8x4,
                _ => null
            },
            "<8 x i16>" => binary.Operation switch
            {
                "add" => vectorHelpers.AddI16x8,
                "and" => vectorHelpers.AndI16x8,
                "or" => vectorHelpers.OrI16x8,
                "xor" => vectorHelpers.XorI16x8,
                _ => null
            },
            "<4 x i16>" => binary.Operation switch
            {
                "add" => vectorHelpers.AddI16x4,
                "and" => vectorHelpers.AndI16x4,
                "or" => vectorHelpers.OrI16x4,
                "xor" => vectorHelpers.XorI16x4,
                _ => null
            },
            "<4 x i32>" => binary.Operation switch
            {
                "add" => vectorHelpers.Add,
                "and" => vectorHelpers.And,
                "or" => vectorHelpers.Or,
                "xor" => vectorHelpers.Xor,
                _ => null
            },
            "<2 x i64>" => binary.Operation switch
            {
                "add" => vectorHelpers.AddI64,
                "and" => vectorHelpers.AndI64,
                "or" => vectorHelpers.OrI64,
                "xor" => vectorHelpers.XorI64,
                _ => null
            },
            _ => null
        };

        if (helperMethod is null)
        {
            throw new NotSupportedException($"Vector binary operation '{binary.Operation}' in function '{functionName}' is not supported by the current emitter slice.");
        }

        il.Append(il.Create(OpCodes.Call, helperMethod));
        StoreLocal(il.Body.Method, il, locals, binary.Result, binary.Type);
    }

    private static bool TryEmitVectorConstant(ILProcessor il, string typeName, string value)
    {
        if (!TryGetSupportedVectorType(typeName, out var vectorType))
        {
            return false;
        }

        if (string.Equals(value, "poison", StringComparison.Ordinal)
            || string.Equals(value, "zeroinitializer", StringComparison.Ordinal))
        {
            EmitVectorConstant(il, vectorType, Enumerable.Repeat(0L, vectorType.ElementCount).ToArray());
            return true;
        }

        if (TryParseVectorSplat(value, vectorType, out var splatValue))
        {
            EmitVectorConstant(il, vectorType, Enumerable.Repeat(splatValue, vectorType.ElementCount).ToArray());
            return true;
        }

        if (TryParseVectorLiteral(value, vectorType, out var literalValues))
        {
            EmitVectorConstant(il, vectorType, literalValues);
            return true;
        }

        return false;
    }

    private static bool TryParseVectorSplat(string value, SupportedVectorType vectorType, out long splatValue)
    {
        splatValue = 0;
        var match = System.Text.RegularExpressions.Regex.Match(value, $"^splat \\({System.Text.RegularExpressions.Regex.Escape(vectorType.ElementType)} (?<value>-?\\d+)\\)$", System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        if (!match.Success)
        {
            return false;
        }

        return long.TryParse(match.Groups["value"].Value, out splatValue);
    }

    private static bool TryParseVectorLiteral(string value, SupportedVectorType vectorType, out long[] elements)
    {
        elements = [];

        if (!value.StartsWith("<", StringComparison.Ordinal) || !value.EndsWith(">", StringComparison.Ordinal))
        {
            return false;
        }

        var parts = value[1..^1]
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != vectorType.ElementCount)
        {
            return false;
        }

        var parsed = new long[vectorType.ElementCount];
        for (var index = 0; index < parts.Length; index++)
        {
            var part = parts[index];
            if (!part.StartsWith($"{vectorType.ElementType} ", StringComparison.Ordinal))
            {
                return false;
            }

            var literalValue = part[(vectorType.ElementType.Length + 1)..];
            if (string.Equals(literalValue, "poison", StringComparison.Ordinal))
            {
                parsed[index] = 0;
                continue;
            }

            if (!long.TryParse(literalValue, out parsed[index]))
            {
                return false;
            }
        }

        elements = parsed;
        return true;
    }

    private static void EmitVectorConstant(ILProcessor il, SupportedVectorType vectorType, IReadOnlyList<long> values)
    {
        il.Append(il.Create(OpCodes.Ldc_I4, values.Count));
        il.Append(il.Create(OpCodes.Newarr, GetVectorElementType(il.Body.Method.Module, vectorType)));

        for (var index = 0; index < values.Count; index++)
        {
            if (values[index] == 0)
            {
                continue;
            }

            il.Append(il.Create(OpCodes.Dup));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            if (vectorType.ElementWidth == 8)
            {
                il.Append(il.Create(OpCodes.Ldc_I4, checked((int)values[index])));
                il.Append(il.Create(OpCodes.Stelem_I1));
            }
            else if (vectorType.ElementWidth == 16)
            {
                il.Append(il.Create(OpCodes.Ldc_I4, checked((short)values[index])));
                il.Append(il.Create(OpCodes.Stelem_I2));
            }
            else if (vectorType.ElementWidth == 32)
            {
                il.Append(il.Create(OpCodes.Ldc_I4, checked((int)values[index])));
                il.Append(il.Create(OpCodes.Stelem_I4));
            }
            else
            {
                il.Append(il.Create(OpCodes.Ldc_I8, values[index]));
                il.Append(il.Create(OpCodes.Stelem_I8));
            }
        }
    }

    private static TypeReference GetVectorElementType(ModuleDefinition module, SupportedVectorType vectorType)
    {
        return vectorType.ElementWidth switch
        {
            8 => module.TypeSystem.SByte,
            16 => module.TypeSystem.Int16,
            32 => module.TypeSystem.Int32,
            64 => module.TypeSystem.Int64,
            _ => throw new NotSupportedException($"Vector element width '{vectorType.ElementWidth}' is not supported by the current emitter slice.")
        };
    }

    private static OpCode GetVectorElementLoadOpCode(int elementWidth)
    {
        return elementWidth switch
        {
            8 => OpCodes.Ldelem_I1,
            16 => OpCodes.Ldelem_I2,
            32 => OpCodes.Ldelem_I4,
            64 => OpCodes.Ldelem_I8,
            _ => throw new NotSupportedException($"Vector element width '{elementWidth}' is not supported by the current emitter slice.")
        };
    }

    private static OpCode GetVectorElementStoreOpCode(int elementWidth)
    {
        return elementWidth switch
        {
            8 => OpCodes.Stelem_I1,
            16 => OpCodes.Stelem_I2,
            32 => OpCodes.Stelem_I4,
            64 => OpCodes.Stelem_I8,
            _ => throw new NotSupportedException($"Vector element width '{elementWidth}' is not supported by the current emitter slice.")
        };
    }

    private static VectorHelperMethods EmitVectorHelpers(ModuleDefinition module, TypeDefinition generatedType)
    {
        var maxScalarI8Method = new MethodDefinition(
            "__scalar_i8_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        maxScalarI8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, module.TypeSystem.SByte));
        maxScalarI8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, module.TypeSystem.SByte));
        generatedType.Methods.Add(maxScalarI8Method);
        EmitScalarSignedMaxHelperBody(maxScalarI8Method);

        var maxUnsignedScalarI8Method = new MethodDefinition(
            "__scalar_i8_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        maxUnsignedScalarI8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, module.TypeSystem.SByte));
        maxUnsignedScalarI8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, module.TypeSystem.SByte));
        generatedType.Methods.Add(maxUnsignedScalarI8Method);
        EmitScalarUnsignedMaxHelperBody(maxUnsignedScalarI8Method);

        var minScalarI8Method = new MethodDefinition(
            "__scalar_i8_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        minScalarI8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, module.TypeSystem.SByte));
        minScalarI8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, module.TypeSystem.SByte));
        generatedType.Methods.Add(minScalarI8Method);
        EmitScalarSignedMinHelperBody(minScalarI8Method);

        var minUnsignedScalarI8Method = new MethodDefinition(
            "__scalar_i8_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        minUnsignedScalarI8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, module.TypeSystem.SByte));
        minUnsignedScalarI8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, module.TypeSystem.SByte));
        generatedType.Methods.Add(minUnsignedScalarI8Method);
        EmitScalarUnsignedMinHelperBody(minUnsignedScalarI8Method);

        var funnelShiftLeftI32Method = new MethodDefinition(
            "__intrinsic_fshl_i32",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        funnelShiftLeftI32Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, module.TypeSystem.Int32));
        funnelShiftLeftI32Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, module.TypeSystem.Int32));
        funnelShiftLeftI32Method.Parameters.Add(new ParameterDefinition("amount", ParameterAttributes.None, module.TypeSystem.Int32));
        generatedType.Methods.Add(funnelShiftLeftI32Method);
        EmitFunnelShiftLeftI32HelperBody(funnelShiftLeftI32Method);

        var vectorI8x16Type = new ArrayType(module.TypeSystem.SByte);
        var addI8x16Method = new MethodDefinition(
            "__vector_i8x16_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        addI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        addI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(addI8x16Method);
        EmitVectorBinaryHelperBody(addI8x16Method, OpCodes.Add, 16, 8);

        var orI8x16Method = new MethodDefinition(
            "__vector_i8x16_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        orI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        orI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(orI8x16Method);
        EmitVectorBinaryHelperBody(orI8x16Method, OpCodes.Or, 16, 8);

        var xorI8x16Method = new MethodDefinition(
            "__vector_i8x16_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        xorI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        xorI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(xorI8x16Method);
        EmitVectorBinaryHelperBody(xorI8x16Method, OpCodes.Xor, 16, 8);

        var reduceXorI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceXorI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceXorI8x16Method);
        EmitVectorReduceXorHelperBody(reduceXorI8x16Method, 8, 16);

        var reduceAddI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceAddI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceAddI8x16Method);
        EmitVectorReduceAddHelperBody(reduceAddI8x16Method, 16, 8);

        var reduceOrI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceOrI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceOrI8x16Method);
        EmitVectorReduceOrHelperBody(reduceOrI8x16Method, 16, 8);

        var maxI8x16Method = new MethodDefinition(
            "__vector_i8x16_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        maxI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        maxI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(maxI8x16Method);
        EmitVectorSignedMaxHelperBody(maxI8x16Method, 16, 8);

        var maxUnsignedI8x16Method = new MethodDefinition(
            "__vector_i8x16_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        maxUnsignedI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        maxUnsignedI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(maxUnsignedI8x16Method);
        EmitVectorUnsignedMaxHelperBody(maxUnsignedI8x16Method, 16, 8);

        var minUnsignedI8x16Method = new MethodDefinition(
            "__vector_i8x16_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        minUnsignedI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        minUnsignedI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(minUnsignedI8x16Method);
        EmitVectorUnsignedMinHelperBody(minUnsignedI8x16Method, 16, 8);

        var minI8x16Method = new MethodDefinition(
            "__vector_i8x16_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x16Type);
        minI8x16Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x16Type));
        minI8x16Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(minI8x16Method);
        EmitVectorSignedMinHelperBody(minI8x16Method, 16, 8);

        var reduceMaxI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMaxI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceMaxI8x16Method);
        EmitVectorReduceSignedMaxHelperBody(reduceMaxI8x16Method, 16, 8);

        var reduceMinI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMinI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceMinI8x16Method);
        EmitVectorReduceSignedMinHelperBody(reduceMinI8x16Method, 16, 8);

        var reduceMaxUnsignedI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMaxUnsignedI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceMaxUnsignedI8x16Method);
        EmitVectorReduceUnsignedMaxHelperBody(reduceMaxUnsignedI8x16Method, 16, 8);

        var reduceMinUnsignedI8x16Method = new MethodDefinition(
            "__vector_i8x16_reduce_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMinUnsignedI8x16Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x16Type));
        generatedType.Methods.Add(reduceMinUnsignedI8x16Method);
        EmitVectorReduceUnsignedMinHelperBody(reduceMinUnsignedI8x16Method, 16, 8);

        var vectorI8x8Type = new ArrayType(module.TypeSystem.SByte);
        var addI8x8Method = new MethodDefinition(
            "__vector_i8x8_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x8Type);
        addI8x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x8Type));
        addI8x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(addI8x8Method);
        EmitVectorBinaryHelperBody(addI8x8Method, OpCodes.Add, 8, 8);

        var orI8x8Method = new MethodDefinition(
            "__vector_i8x8_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x8Type);
        orI8x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x8Type));
        orI8x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(orI8x8Method);
        EmitVectorBinaryHelperBody(orI8x8Method, OpCodes.Or, 8, 8);

        var xorI8x8Method = new MethodDefinition(
            "__vector_i8x8_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x8Type);
        xorI8x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x8Type));
        xorI8x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(xorI8x8Method);
        EmitVectorBinaryHelperBody(xorI8x8Method, OpCodes.Xor, 8, 8);

        var reduceXorI8x8Method = new MethodDefinition(
            "__vector_i8x8_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceXorI8x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(reduceXorI8x8Method);
        EmitVectorReduceXorHelperBody(reduceXorI8x8Method, 8, 8);

        var reduceAddI8x8Method = new MethodDefinition(
            "__vector_i8x8_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceAddI8x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(reduceAddI8x8Method);
        EmitVectorReduceAddHelperBody(reduceAddI8x8Method, 8, 8);

        var reduceOrI8x8Method = new MethodDefinition(
            "__vector_i8x8_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceOrI8x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(reduceOrI8x8Method);
        EmitVectorReduceOrHelperBody(reduceOrI8x8Method, 8, 8);

        var maxI8x8Method = new MethodDefinition(
            "__vector_i8x8_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x8Type);
        maxI8x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x8Type));
        maxI8x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(maxI8x8Method);
        EmitVectorSignedMaxHelperBody(maxI8x8Method, 8, 8);

        var reduceMaxI8x8Method = new MethodDefinition(
            "__vector_i8x8_reduce_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMaxI8x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(reduceMaxI8x8Method);
        EmitVectorReduceSignedMaxHelperBody(reduceMaxI8x8Method, 8, 8);

        var minI8x8Method = new MethodDefinition(
            "__vector_i8x8_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x8Type);
        minI8x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x8Type));
        minI8x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(minI8x8Method);
        EmitVectorSignedMinHelperBody(minI8x8Method, 8, 8);

        var reduceMinI8x8Method = new MethodDefinition(
            "__vector_i8x8_reduce_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMinI8x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x8Type));
        generatedType.Methods.Add(reduceMinI8x8Method);
        EmitVectorReduceSignedMinHelperBody(reduceMinI8x8Method, 8, 8);

        var vectorI8x4Type = new ArrayType(module.TypeSystem.SByte);
        var addI8x4Method = new MethodDefinition(
            "__vector_i8x4_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x4Type);
        addI8x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x4Type));
        addI8x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(addI8x4Method);
        EmitVectorBinaryHelperBody(addI8x4Method, OpCodes.Add, 4, 8);

        var andI8x4Method = new MethodDefinition(
            "__vector_i8x4_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x4Type);
        andI8x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x4Type));
        andI8x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(andI8x4Method);
        EmitVectorBinaryHelperBody(andI8x4Method, OpCodes.And, 4, 8);

        var orI8x4Method = new MethodDefinition(
            "__vector_i8x4_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x4Type);
        orI8x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x4Type));
        orI8x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(orI8x4Method);
        EmitVectorBinaryHelperBody(orI8x4Method, OpCodes.Or, 4, 8);

        var xorI8x4Method = new MethodDefinition(
            "__vector_i8x4_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x4Type);
        xorI8x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x4Type));
        xorI8x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(xorI8x4Method);
        EmitVectorBinaryHelperBody(xorI8x4Method, OpCodes.Xor, 4, 8);

        var maxUnsignedI8x4Method = new MethodDefinition(
            "__vector_i8x4_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x4Type);
        maxUnsignedI8x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x4Type));
        maxUnsignedI8x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(maxUnsignedI8x4Method);
        EmitVectorUnsignedMaxHelperBody(maxUnsignedI8x4Method, 4, 8);

        var minUnsignedI8x4Method = new MethodDefinition(
            "__vector_i8x4_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI8x4Type);
        minUnsignedI8x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI8x4Type));
        minUnsignedI8x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(minUnsignedI8x4Method);
        EmitVectorUnsignedMinHelperBody(minUnsignedI8x4Method, 4, 8);

        var reduceXorI8x4Method = new MethodDefinition(
            "__vector_i8x4_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceXorI8x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(reduceXorI8x4Method);
        EmitVectorReduceXorHelperBody(reduceXorI8x4Method, 8, 4);

        var reduceAddI8x4Method = new MethodDefinition(
            "__vector_i8x4_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceAddI8x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(reduceAddI8x4Method);
        EmitVectorReduceAddHelperBody(reduceAddI8x4Method, 4, 8);

        var reduceOrI8x4Method = new MethodDefinition(
            "__vector_i8x4_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceOrI8x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(reduceOrI8x4Method);
        EmitVectorReduceOrHelperBody(reduceOrI8x4Method, 4, 8);

        var reduceAndI8x4Method = new MethodDefinition(
            "__vector_i8x4_reduce_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceAndI8x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(reduceAndI8x4Method);
        EmitVectorReduceAndHelperBody(reduceAndI8x4Method, 4, 8);

        var reduceMaxUnsignedI8x4Method = new MethodDefinition(
            "__vector_i8x4_reduce_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMaxUnsignedI8x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(reduceMaxUnsignedI8x4Method);
        EmitVectorReduceUnsignedMaxHelperBody(reduceMaxUnsignedI8x4Method, 4, 8);

        var reduceMinUnsignedI8x4Method = new MethodDefinition(
            "__vector_i8x4_reduce_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.SByte);
        reduceMinUnsignedI8x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI8x4Type));
        generatedType.Methods.Add(reduceMinUnsignedI8x4Method);
        EmitVectorReduceUnsignedMinHelperBody(reduceMinUnsignedI8x4Method, 4, 8);

        var vectorI16x8Type = new ArrayType(module.TypeSystem.Int16);
        var addI16x8Method = new MethodDefinition(
            "__vector_i16x8_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        addI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        addI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(addI16x8Method);
        EmitVectorBinaryHelperBody(addI16x8Method, OpCodes.Add, 8, 16);

        var xorI16x8Method = new MethodDefinition(
            "__vector_i16x8_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        xorI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        xorI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(xorI16x8Method);
        EmitVectorBinaryHelperBody(xorI16x8Method, OpCodes.Xor, 8, 16);

        var andI16x8Method = new MethodDefinition(
            "__vector_i16x8_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        andI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        andI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(andI16x8Method);
        EmitVectorBinaryHelperBody(andI16x8Method, OpCodes.And, 8, 16);

        var orI16x8Method = new MethodDefinition(
            "__vector_i16x8_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        orI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        orI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(orI16x8Method);
        EmitVectorBinaryHelperBody(orI16x8Method, OpCodes.Or, 8, 16);

        var minI16x8Method = new MethodDefinition(
            "__vector_i16x8_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        minI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        minI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(minI16x8Method);
        EmitVectorSignedMinHelperBody(minI16x8Method, 8, 16);

        var maxI16x8Method = new MethodDefinition(
            "__vector_i16x8_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        maxI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        maxI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(maxI16x8Method);
        EmitVectorSignedMaxHelperBody(maxI16x8Method, 8, 16);

        var maxUnsignedI16x8Method = new MethodDefinition(
            "__vector_i16x8_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        maxUnsignedI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        maxUnsignedI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(maxUnsignedI16x8Method);
        EmitVectorUnsignedMaxHelperBody(maxUnsignedI16x8Method, 8, 16);

        var minUnsignedI16x8Method = new MethodDefinition(
            "__vector_i16x8_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x8Type);
        minUnsignedI16x8Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x8Type));
        minUnsignedI16x8Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(minUnsignedI16x8Method);
        EmitVectorUnsignedMinHelperBody(minUnsignedI16x8Method, 8, 16);

        var reduceMinI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMinI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceMinI16x8Method);
        EmitVectorReduceSignedMinHelperBody(reduceMinI16x8Method, 8, 16);

        var reduceMaxI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMaxI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceMaxI16x8Method);
        EmitVectorReduceSignedMaxHelperBody(reduceMaxI16x8Method, 8, 16);

        var reduceMaxUnsignedI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMaxUnsignedI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceMaxUnsignedI16x8Method);
        EmitVectorReduceUnsignedMaxHelperBody(reduceMaxUnsignedI16x8Method, 8, 16);

        var reduceMinUnsignedI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMinUnsignedI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceMinUnsignedI16x8Method);
        EmitVectorReduceUnsignedMinHelperBody(reduceMinUnsignedI16x8Method, 8, 16);

        var reduceXorI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceXorI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceXorI16x8Method);
        EmitVectorReduceXorHelperBody(reduceXorI16x8Method, 16, 8);

        var reduceAddI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceAddI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceAddI16x8Method);
        EmitVectorReduceAddHelperBody(reduceAddI16x8Method, 8, 16);

        var reduceOrI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceOrI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceOrI16x8Method);
        EmitVectorReduceOrHelperBody(reduceOrI16x8Method, 8, 16);

        var reduceAndI16x8Method = new MethodDefinition(
            "__vector_i16x8_reduce_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceAndI16x8Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x8Type));
        generatedType.Methods.Add(reduceAndI16x8Method);
        EmitVectorReduceAndHelperBody(reduceAndI16x8Method, 8, 16);

        var vectorI16x4Type = new ArrayType(module.TypeSystem.Int16);
        var addI16x4Method = new MethodDefinition(
            "__vector_i16x4_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        addI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        addI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(addI16x4Method);
        EmitVectorBinaryHelperBody(addI16x4Method, OpCodes.Add, 4, 16);

        var xorI16x4Method = new MethodDefinition(
            "__vector_i16x4_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        xorI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        xorI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(xorI16x4Method);
        EmitVectorBinaryHelperBody(xorI16x4Method, OpCodes.Xor, 4, 16);

        var andI16x4Method = new MethodDefinition(
            "__vector_i16x4_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        andI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        andI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(andI16x4Method);
        EmitVectorBinaryHelperBody(andI16x4Method, OpCodes.And, 4, 16);

        var orI16x4Method = new MethodDefinition(
            "__vector_i16x4_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        orI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        orI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(orI16x4Method);
        EmitVectorBinaryHelperBody(orI16x4Method, OpCodes.Or, 4, 16);

        var minI16x4Method = new MethodDefinition(
            "__vector_i16x4_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        minI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        minI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(minI16x4Method);
        EmitVectorSignedMinHelperBody(minI16x4Method, 4, 16);

        var maxI16x4Method = new MethodDefinition(
            "__vector_i16x4_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        maxI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        maxI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(maxI16x4Method);
        EmitVectorSignedMaxHelperBody(maxI16x4Method, 4, 16);

        var maxUnsignedI16x4Method = new MethodDefinition(
            "__vector_i16x4_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        maxUnsignedI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        maxUnsignedI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(maxUnsignedI16x4Method);
        EmitVectorUnsignedMaxHelperBody(maxUnsignedI16x4Method, 4, 16);

        var minUnsignedI16x4Method = new MethodDefinition(
            "__vector_i16x4_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI16x4Type);
        minUnsignedI16x4Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI16x4Type));
        minUnsignedI16x4Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(minUnsignedI16x4Method);
        EmitVectorUnsignedMinHelperBody(minUnsignedI16x4Method, 4, 16);

        var reduceMinI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMinI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceMinI16x4Method);
        EmitVectorReduceSignedMinHelperBody(reduceMinI16x4Method, 4, 16);

        var reduceMaxI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMaxI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceMaxI16x4Method);
        EmitVectorReduceSignedMaxHelperBody(reduceMaxI16x4Method, 4, 16);

        var reduceMaxUnsignedI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMaxUnsignedI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceMaxUnsignedI16x4Method);
        EmitVectorReduceUnsignedMaxHelperBody(reduceMaxUnsignedI16x4Method, 4, 16);

        var reduceMinUnsignedI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceMinUnsignedI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceMinUnsignedI16x4Method);
        EmitVectorReduceUnsignedMinHelperBody(reduceMinUnsignedI16x4Method, 4, 16);

        var reduceXorI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceXorI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceXorI16x4Method);
        EmitVectorReduceXorHelperBody(reduceXorI16x4Method, 16, 4);

        var reduceAddI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceAddI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceAddI16x4Method);
        EmitVectorReduceAddHelperBody(reduceAddI16x4Method, 4, 16);

        var reduceOrI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceOrI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceOrI16x4Method);
        EmitVectorReduceOrHelperBody(reduceOrI16x4Method, 4, 16);

        var reduceAndI16x4Method = new MethodDefinition(
            "__vector_i16x4_reduce_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int16);
        reduceAndI16x4Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI16x4Type));
        generatedType.Methods.Add(reduceAndI16x4Method);
        EmitVectorReduceAndHelperBody(reduceAndI16x4Method, 4, 16);

        var vectorI32Type = new ArrayType(module.TypeSystem.Int32);
        var addMethod = new MethodDefinition(
            "__vector_i32_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        addMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        addMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(addMethod);
        EmitVectorBinaryHelperBody(addMethod, OpCodes.Add, 4, 32);

        var xorMethod = new MethodDefinition(
            "__vector_i32_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        xorMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        xorMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(xorMethod);
        EmitVectorBinaryHelperBody(xorMethod, OpCodes.Xor, 4, 32);

        var andMethod = new MethodDefinition(
            "__vector_i32_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        andMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        andMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(andMethod);
        EmitVectorBinaryHelperBody(andMethod, OpCodes.And, 4, 32);

        var orMethod = new MethodDefinition(
            "__vector_i32_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        orMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        orMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(orMethod);
        EmitVectorBinaryHelperBody(orMethod, OpCodes.Or, 4, 32);

        var maxMethod = new MethodDefinition(
            "__vector_i32_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        maxMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        maxMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(maxMethod);
        EmitVectorSignedMaxHelperBody(maxMethod, 4, 32);

        var minMethod = new MethodDefinition(
            "__vector_i32_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        minMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        minMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(minMethod);
        EmitVectorSignedMinHelperBody(minMethod, 4, 32);

        var maxUnsignedMethod = new MethodDefinition(
            "__vector_i32_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        maxUnsignedMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        maxUnsignedMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(maxUnsignedMethod);
        EmitVectorUnsignedMaxHelperBody(maxUnsignedMethod, 4, 32);

        var minUnsignedMethod = new MethodDefinition(
            "__vector_i32_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI32Type);
        minUnsignedMethod.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI32Type));
        minUnsignedMethod.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(minUnsignedMethod);
        EmitVectorUnsignedMinHelperBody(minUnsignedMethod, 4, 32);

        var reduceXorMethod = new MethodDefinition(
            "__vector_i32_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceXorMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceXorMethod);
        EmitVectorReduceXorHelperBody(reduceXorMethod, 32);

        var reduceAddMethod = new MethodDefinition(
            "__vector_i32_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceAddMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceAddMethod);
        EmitVectorReduceAddHelperBody(reduceAddMethod, 4, 32);

        var reduceOrMethod = new MethodDefinition(
            "__vector_i32_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceOrMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceOrMethod);
        EmitVectorReduceOrHelperBody(reduceOrMethod, 4, 32);

        var reduceAndMethod = new MethodDefinition(
            "__vector_i32_reduce_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceAndMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceAndMethod);
        EmitVectorReduceAndHelperBody(reduceAndMethod, 4, 32);

        var reduceMaxMethod = new MethodDefinition(
            "__vector_i32_reduce_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceMaxMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceMaxMethod);
        EmitVectorReduceSignedMaxHelperBody(reduceMaxMethod, 4, 32);

        var reduceMinMethod = new MethodDefinition(
            "__vector_i32_reduce_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceMinMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceMinMethod);
        EmitVectorReduceSignedMinHelperBody(reduceMinMethod, 4, 32);

        var reduceMaxUnsignedMethod = new MethodDefinition(
            "__vector_i32_reduce_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceMaxUnsignedMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceMaxUnsignedMethod);
        EmitVectorReduceUnsignedMaxHelperBody(reduceMaxUnsignedMethod, 4, 32);

        var reduceMinUnsignedMethod = new MethodDefinition(
            "__vector_i32_reduce_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int32);
        reduceMinUnsignedMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI32Type));
        generatedType.Methods.Add(reduceMinUnsignedMethod);
        EmitVectorReduceUnsignedMinHelperBody(reduceMinUnsignedMethod, 4, 32);

        var vectorI64Type = new ArrayType(module.TypeSystem.Int64);
        var addI64Method = new MethodDefinition(
            "__vector_i64_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        addI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        addI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(addI64Method);
        EmitVectorBinaryHelperBody(addI64Method, OpCodes.Add, 2, 64);

        var xorI64Method = new MethodDefinition(
            "__vector_i64_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        xorI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        xorI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(xorI64Method);
        EmitVectorBinaryHelperBody(xorI64Method, OpCodes.Xor, 2, 64);

        var andI64Method = new MethodDefinition(
            "__vector_i64_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        andI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        andI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(andI64Method);
        EmitVectorBinaryHelperBody(andI64Method, OpCodes.And, 2, 64);

        var orI64Method = new MethodDefinition(
            "__vector_i64_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        orI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        orI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(orI64Method);
        EmitVectorBinaryHelperBody(orI64Method, OpCodes.Or, 2, 64);

        var maxI64Method = new MethodDefinition(
            "__vector_i64_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        maxI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        maxI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(maxI64Method);
        EmitVectorSignedMaxHelperBody(maxI64Method, 2, 64);

        var minI64Method = new MethodDefinition(
            "__vector_i64_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        minI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        minI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(minI64Method);
        EmitVectorSignedMinHelperBody(minI64Method, 2, 64);

        var maxUnsignedI64Method = new MethodDefinition(
            "__vector_i64_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        maxUnsignedI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        maxUnsignedI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(maxUnsignedI64Method);
        EmitVectorUnsignedMaxHelperBody(maxUnsignedI64Method, 2, 64);

        var minUnsignedI64Method = new MethodDefinition(
            "__vector_i64_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            vectorI64Type);
        minUnsignedI64Method.Parameters.Add(new ParameterDefinition("left", ParameterAttributes.None, vectorI64Type));
        minUnsignedI64Method.Parameters.Add(new ParameterDefinition("right", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(minUnsignedI64Method);
        EmitVectorUnsignedMinHelperBody(minUnsignedI64Method, 2, 64);

        var reduceAddI64Method = new MethodDefinition(
            "__vector_i64_reduce_add",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceAddI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceAddI64Method);
        EmitVectorReduceAddHelperBody(reduceAddI64Method, 2, 64);

        var reduceXorI64Method = new MethodDefinition(
            "__vector_i64_reduce_xor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceXorI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceXorI64Method);
        EmitVectorReduceXorHelperBody(reduceXorI64Method, 64, 2);

        var reduceOrI64Method = new MethodDefinition(
            "__vector_i64_reduce_or",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceOrI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceOrI64Method);
        EmitVectorReduceOrHelperBody(reduceOrI64Method, 2, 64);

        var reduceMaxI64Method = new MethodDefinition(
            "__vector_i64_reduce_smax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceMaxI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceMaxI64Method);
        EmitVectorReduceSignedMaxHelperBody(reduceMaxI64Method, 2, 64);

        var reduceMinI64Method = new MethodDefinition(
            "__vector_i64_reduce_smin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceMinI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceMinI64Method);
        EmitVectorReduceSignedMinHelperBody(reduceMinI64Method, 2, 64);

        var reduceMaxUnsignedI64Method = new MethodDefinition(
            "__vector_i64_reduce_umax",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceMaxUnsignedI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceMaxUnsignedI64Method);
        EmitVectorReduceUnsignedMaxHelperBody(reduceMaxUnsignedI64Method, 2, 64);

        var reduceMinUnsignedI64Method = new MethodDefinition(
            "__vector_i64_reduce_umin",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceMinUnsignedI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceMinUnsignedI64Method);
        EmitVectorReduceUnsignedMinHelperBody(reduceMinUnsignedI64Method, 2, 64);

        var reduceAndI64Method = new MethodDefinition(
            "__vector_i64_reduce_and",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig,
            module.TypeSystem.Int64);
        reduceAndI64Method.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, vectorI64Type));
        generatedType.Methods.Add(reduceAndI64Method);
        EmitVectorReduceAndHelperBody(reduceAndI64Method, 2, 64);

        return new VectorHelperMethods(addMethod, xorMethod, andMethod, orMethod, maxMethod, minMethod, maxUnsignedMethod, minUnsignedMethod, reduceXorMethod, reduceAddMethod, reduceOrMethod, reduceAndMethod, reduceMaxMethod, reduceMinMethod, reduceMaxUnsignedMethod, reduceMinUnsignedMethod, addI64Method, xorI64Method, andI64Method, orI64Method, maxI64Method, minI64Method, maxUnsignedI64Method, minUnsignedI64Method, reduceAddI64Method, reduceXorI64Method, reduceOrI64Method, reduceAndI64Method, reduceMaxI64Method, reduceMinI64Method, reduceMaxUnsignedI64Method, reduceMinUnsignedI64Method, addI16x8Method, andI16x8Method, orI16x8Method, xorI16x8Method, maxI16x8Method, minI16x8Method, maxUnsignedI16x8Method, minUnsignedI16x8Method, reduceXorI16x8Method, reduceAddI16x8Method, reduceOrI16x8Method, reduceAndI16x8Method, reduceMaxI16x8Method, reduceMinI16x8Method, reduceMaxUnsignedI16x8Method, reduceMinUnsignedI16x8Method, addI16x4Method, andI16x4Method, orI16x4Method, xorI16x4Method, maxI16x4Method, minI16x4Method, maxUnsignedI16x4Method, minUnsignedI16x4Method, reduceXorI16x4Method, reduceAddI16x4Method, reduceOrI16x4Method, reduceAndI16x4Method, reduceMaxI16x4Method, reduceMinI16x4Method, reduceMaxUnsignedI16x4Method, reduceMinUnsignedI16x4Method, maxScalarI8Method, maxUnsignedScalarI8Method, minScalarI8Method, minUnsignedScalarI8Method, funnelShiftLeftI32Method, addI8x16Method, orI8x16Method, xorI8x16Method, maxI8x16Method, minI8x16Method, maxUnsignedI8x16Method, minUnsignedI8x16Method, reduceXorI8x16Method, reduceAddI8x16Method, reduceOrI8x16Method, reduceMaxI8x16Method, reduceMinI8x16Method, reduceMaxUnsignedI8x16Method, reduceMinUnsignedI8x16Method, addI8x8Method, orI8x8Method, xorI8x8Method, maxI8x8Method, minI8x8Method, reduceXorI8x8Method, reduceAddI8x8Method, reduceOrI8x8Method, reduceMaxI8x8Method, reduceMinI8x8Method, addI8x4Method, andI8x4Method, orI8x4Method, xorI8x4Method, maxUnsignedI8x4Method, minUnsignedI8x4Method, reduceXorI8x4Method, reduceAddI8x4Method, reduceOrI8x4Method, reduceAndI8x4Method, reduceMaxUnsignedI8x4Method, reduceMinUnsignedI8x4Method);
    }

    private static void EmitScalarSignedMaxHelperBody(MethodDefinition method)
    {
        var il = method.Body.GetILProcessor();
        var useLeft = il.Create(OpCodes.Nop);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Clt));
        il.Append(il.Create(OpCodes.Brfalse_S, useLeft));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ret));
        il.Append(useLeft);
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitScalarUnsignedMaxHelperBody(MethodDefinition method)
    {
        var il = method.Body.GetILProcessor();
        var useLeft = il.Create(OpCodes.Nop);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4, 255));
        il.Append(il.Create(OpCodes.And));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ldc_I4, 255));
        il.Append(il.Create(OpCodes.And));
        il.Append(il.Create(OpCodes.Bge_Un_S, useLeft));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ret));
        il.Append(useLeft);
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitScalarSignedMinHelperBody(MethodDefinition method)
    {
        var il = method.Body.GetILProcessor();
        var useLeft = il.Create(OpCodes.Nop);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Cgt));
        il.Append(il.Create(OpCodes.Brfalse_S, useLeft));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ret));
        il.Append(useLeft);
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitScalarUnsignedMinHelperBody(MethodDefinition method)
    {
        var il = method.Body.GetILProcessor();
        var useLeft = il.Create(OpCodes.Nop);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4, 255));
        il.Append(il.Create(OpCodes.And));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ldc_I4, 255));
        il.Append(il.Create(OpCodes.And));
        il.Append(il.Create(OpCodes.Ble_Un_S, useLeft));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ret));
        il.Append(useLeft);
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitFunnelShiftLeftI32HelperBody(MethodDefinition method)
    {
        method.Body.InitLocals = true;
        var il = method.Body.GetILProcessor();
        var shiftLocal = new VariableDefinition(method.Module.TypeSystem.Int32);
        var inverseShiftLocal = new VariableDefinition(method.Module.TypeSystem.Int32);
        method.Body.Variables.Add(shiftLocal);
        method.Body.Variables.Add(inverseShiftLocal);

        il.Append(il.Create(OpCodes.Ldarg_2));
        il.Append(il.Create(OpCodes.Ldc_I4, 31));
        il.Append(il.Create(OpCodes.And));
        il.Append(il.Create(OpCodes.Stloc, shiftLocal));

        il.Append(il.Create(OpCodes.Ldc_I4, 32));
        il.Append(il.Create(OpCodes.Ldloc, shiftLocal));
        il.Append(il.Create(OpCodes.Sub));
        il.Append(il.Create(OpCodes.Ldc_I4, 31));
        il.Append(il.Create(OpCodes.And));
        il.Append(il.Create(OpCodes.Stloc, inverseShiftLocal));

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldloc, shiftLocal));
        il.Append(il.Create(OpCodes.Shl));
        il.Append(il.Create(OpCodes.Ldarg_1));
        il.Append(il.Create(OpCodes.Ldloc, inverseShiftLocal));
        il.Append(il.Create(OpCodes.Shr_Un));
        il.Append(il.Create(OpCodes.Or));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorBinaryHelperBody(MethodDefinition method, OpCode opCode, int elementCount, int elementWidth)
    {
        method.Body.InitLocals = true;
        var il = method.Body.GetILProcessor();
        var vectorType = new SupportedVectorType($"i{elementWidth}", elementWidth, elementCount);
        var elementType = GetVectorElementType(method.Module, vectorType);
        var resultLocal = new VariableDefinition(new ArrayType(elementType));
        method.Body.Variables.Add(resultLocal);

        il.Append(il.Create(OpCodes.Ldc_I4, elementCount));
        il.Append(il.Create(OpCodes.Newarr, elementType));
        il.Append(il.Create(OpCodes.Stloc, resultLocal));

        var loadElement = GetVectorElementLoadOpCode(elementWidth);
        var storeElement = GetVectorElementStoreOpCode(elementWidth);

        for (var index = 0; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldloc, resultLocal));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Ldarg_1));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(opCode));
            il.Append(il.Create(storeElement));
        }

        il.Append(il.Create(OpCodes.Ldloc, resultLocal));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceXorHelperBody(MethodDefinition method, int elementWidth, int elementCount = 4)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Xor));

        for (var index = 2; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Xor));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceAddHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Add));

        for (var index = 2; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Add));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceOrHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Or));

        for (var index = 2; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Or));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceAndHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(loadElement));
        il.Append(il.Create(OpCodes.And));

        for (var index = 2; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.And));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorSignedMaxHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        method.Body.InitLocals = true;
        var il = method.Body.GetILProcessor();
        var vectorType = new SupportedVectorType($"i{elementWidth}", elementWidth, elementCount);
        var elementType = GetVectorElementType(method.Module, vectorType);
        var resultLocal = new VariableDefinition(new ArrayType(elementType));
        method.Body.Variables.Add(resultLocal);

        var loadElement = GetVectorElementLoadOpCode(elementWidth);
        var storeElement = GetVectorElementStoreOpCode(elementWidth);
        MethodReference? maxMethodReference = null;
        if (elementWidth != 8)
        {
            var maxMethod = elementWidth switch
            {
                16 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(short), typeof(short)]),
                32 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)]),
                64 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(long), typeof(long)]),
                _ => null
            };
            maxMethodReference = method.Module.ImportReference(maxMethod!);
        }

        il.Append(il.Create(OpCodes.Ldc_I4, elementCount));
        il.Append(il.Create(OpCodes.Newarr, elementType));
        il.Append(il.Create(OpCodes.Stloc, resultLocal));

        for (var index = 0; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldloc, resultLocal));
            il.Append(il.Create(OpCodes.Ldc_I4, index));

            if (elementWidth == 8)
            {
                var useLeft = il.Create(OpCodes.Nop);
                var storeValue = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Bge_S, useLeft));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Br_S, storeValue));
                il.Append(useLeft);
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(storeValue);
            }
            else
            {
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Call, maxMethodReference!));
            }

            il.Append(il.Create(storeElement));
        }

        il.Append(il.Create(OpCodes.Ldloc, resultLocal));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceSignedMaxHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        if (elementWidth == 8)
        {
            method.Body.InitLocals = true;
            var currentLocal = new VariableDefinition(method.Module.TypeSystem.SByte);
            method.Body.Variables.Add(currentLocal);

            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Stloc, currentLocal));

            for (var index = 1; index < elementCount; index++)
            {
                var keepCurrent = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldloc, currentLocal));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Bge_S, keepCurrent));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Stloc, currentLocal));
                il.Append(keepCurrent);
            }

            il.Append(il.Create(OpCodes.Ldloc, currentLocal));
            il.Append(il.Create(OpCodes.Ret));
            return;
        }

        var maxMethod = elementWidth switch
        {
            16 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(short), typeof(short)]),
            32 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)]),
            64 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(long), typeof(long)]),
            _ => null
        };
        var maxMethodReference = method.Module.ImportReference(maxMethod!);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));

        for (var index = 1; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Call, maxMethodReference));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorSignedMinHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        method.Body.InitLocals = true;
        var il = method.Body.GetILProcessor();
        var vectorType = new SupportedVectorType($"i{elementWidth}", elementWidth, elementCount);
        var elementType = GetVectorElementType(method.Module, vectorType);
        var resultLocal = new VariableDefinition(new ArrayType(elementType));
        method.Body.Variables.Add(resultLocal);

        var loadElement = GetVectorElementLoadOpCode(elementWidth);
        var storeElement = GetVectorElementStoreOpCode(elementWidth);
        MethodReference? minMethodReference = null;
        if (elementWidth != 8)
        {
            var minMethod = elementWidth switch
            {
                16 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(short), typeof(short)]),
                32 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)]),
                64 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(long), typeof(long)]),
                _ => null
            };
            minMethodReference = method.Module.ImportReference(minMethod!);
        }

        il.Append(il.Create(OpCodes.Ldc_I4, elementCount));
        il.Append(il.Create(OpCodes.Newarr, elementType));
        il.Append(il.Create(OpCodes.Stloc, resultLocal));

        for (var index = 0; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldloc, resultLocal));
            il.Append(il.Create(OpCodes.Ldc_I4, index));

            if (elementWidth == 8)
            {
                var useLeft = il.Create(OpCodes.Nop);
                var storeValue = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ble_S, useLeft));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Br_S, storeValue));
                il.Append(useLeft);
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(storeValue);
            }
            else
            {
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Call, minMethodReference!));
            }

            il.Append(il.Create(storeElement));
        }

        il.Append(il.Create(OpCodes.Ldloc, resultLocal));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceSignedMinHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        if (elementWidth == 8)
        {
            method.Body.InitLocals = true;
            var currentLocal = new VariableDefinition(method.Module.TypeSystem.SByte);
            method.Body.Variables.Add(currentLocal);

            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Stloc, currentLocal));

            for (var index = 1; index < elementCount; index++)
            {
                var keepCurrent = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldloc, currentLocal));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ble_S, keepCurrent));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Stloc, currentLocal));
                il.Append(keepCurrent);
            }

            il.Append(il.Create(OpCodes.Ldloc, currentLocal));
            il.Append(il.Create(OpCodes.Ret));
            return;
        }

        var minMethod = elementWidth switch
        {
            16 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(short), typeof(short)]),
            32 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(int), typeof(int)]),
            64 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(long), typeof(long)]),
            _ => null
        };
        var minMethodReference = method.Module.ImportReference(minMethod!);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));

        for (var index = 1; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Call, minMethodReference));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorUnsignedMaxHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        method.Body.InitLocals = true;
        var il = method.Body.GetILProcessor();
        var vectorType = new SupportedVectorType($"i{elementWidth}", elementWidth, elementCount);
        var elementType = GetVectorElementType(method.Module, vectorType);
        var resultLocal = new VariableDefinition(new ArrayType(elementType));
        method.Body.Variables.Add(resultLocal);
        var loadElement = GetVectorElementLoadOpCode(elementWidth);
        var storeElement = GetVectorElementStoreOpCode(elementWidth);
        MethodReference? maxMethodReference = null;
        if (elementWidth != 8)
        {
            var maxMethod = elementWidth switch
            {
                16 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ushort), typeof(ushort)]),
                32 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint), typeof(uint)]),
                64 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong), typeof(ulong)]),
                _ => null
            };
            maxMethodReference = method.Module.ImportReference(maxMethod!);
        }

        il.Append(il.Create(OpCodes.Ldc_I4, elementCount));
        il.Append(il.Create(OpCodes.Newarr, elementType));
        il.Append(il.Create(OpCodes.Stloc, resultLocal));

        for (var index = 0; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldloc, resultLocal));
            il.Append(il.Create(OpCodes.Ldc_I4, index));

            if (elementWidth == 8)
            {
                var useLeft = il.Create(OpCodes.Nop);
                var storeValue = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Bge_Un_S, useLeft));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Br_S, storeValue));
                il.Append(useLeft);
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(storeValue);
            }
            else
            {
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Call, maxMethodReference!));
            }

            il.Append(il.Create(storeElement));
        }

        il.Append(il.Create(OpCodes.Ldloc, resultLocal));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceUnsignedMaxHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        if (elementWidth == 8)
        {
            method.Body.InitLocals = true;
            var currentLocal = new VariableDefinition(method.Module.TypeSystem.SByte);
            method.Body.Variables.Add(currentLocal);

            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Stloc, currentLocal));

            for (var index = 1; index < elementCount; index++)
            {
                var keepCurrent = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldloc, currentLocal));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Bge_Un_S, keepCurrent));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Stloc, currentLocal));
                il.Append(keepCurrent);
            }

            il.Append(il.Create(OpCodes.Ldloc, currentLocal));
            il.Append(il.Create(OpCodes.Ret));
            return;
        }

        var maxMethod = elementWidth switch
        {
            16 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ushort), typeof(ushort)]),
            32 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint), typeof(uint)]),
            64 => typeof(Math).GetMethod(nameof(Math.Max), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong), typeof(ulong)]),
            _ => null
        };
        var maxMethodReference = method.Module.ImportReference(maxMethod!);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));

        for (var index = 1; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Call, maxMethodReference));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorUnsignedMinHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        method.Body.InitLocals = true;
        var il = method.Body.GetILProcessor();
        var vectorType = new SupportedVectorType($"i{elementWidth}", elementWidth, elementCount);
        var elementType = GetVectorElementType(method.Module, vectorType);
        var resultLocal = new VariableDefinition(new ArrayType(elementType));
        method.Body.Variables.Add(resultLocal);
        var loadElement = GetVectorElementLoadOpCode(elementWidth);
        var storeElement = GetVectorElementStoreOpCode(elementWidth);
        MethodReference? minMethodReference = null;
        if (elementWidth != 8)
        {
            var minMethod = elementWidth switch
            {
                16 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ushort), typeof(ushort)]),
                32 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint), typeof(uint)]),
                64 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong), typeof(ulong)]),
                _ => null
            };
            minMethodReference = method.Module.ImportReference(minMethod!);
        }

        il.Append(il.Create(OpCodes.Ldc_I4, elementCount));
        il.Append(il.Create(OpCodes.Newarr, elementType));
        il.Append(il.Create(OpCodes.Stloc, resultLocal));

        for (var index = 0; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldloc, resultLocal));
            il.Append(il.Create(OpCodes.Ldc_I4, index));

            if (elementWidth == 8)
            {
                var useLeft = il.Create(OpCodes.Nop);
                var storeValue = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Ble_Un_S, useLeft));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Br_S, storeValue));
                il.Append(useLeft);
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(storeValue);
            }
            else
            {
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldarg_1));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Call, minMethodReference!));
            }

            il.Append(il.Create(storeElement));
        }

        il.Append(il.Create(OpCodes.Ldloc, resultLocal));
        il.Append(il.Create(OpCodes.Ret));
    }

    private static void EmitVectorReduceUnsignedMinHelperBody(MethodDefinition method, int elementCount, int elementWidth)
    {
        var il = method.Body.GetILProcessor();
        var loadElement = GetVectorElementLoadOpCode(elementWidth);

        if (elementWidth == 8)
        {
            method.Body.InitLocals = true;
            var currentLocal = new VariableDefinition(method.Module.TypeSystem.SByte);
            method.Body.Variables.Add(currentLocal);

            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4_0));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Stloc, currentLocal));

            for (var index = 1; index < elementCount; index++)
            {
                var keepCurrent = il.Create(OpCodes.Nop);

                il.Append(il.Create(OpCodes.Ldloc, currentLocal));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Ldc_I4, 255));
                il.Append(il.Create(OpCodes.And));
                il.Append(il.Create(OpCodes.Ble_Un_S, keepCurrent));
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldc_I4, index));
                il.Append(il.Create(loadElement));
                il.Append(il.Create(OpCodes.Stloc, currentLocal));
                il.Append(keepCurrent);
            }

            il.Append(il.Create(OpCodes.Ldloc, currentLocal));
            il.Append(il.Create(OpCodes.Ret));
            return;
        }

        var minMethod = elementWidth switch
        {
            16 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ushort), typeof(ushort)]),
            32 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(uint), typeof(uint)]),
            64 => typeof(Math).GetMethod(nameof(Math.Min), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, [typeof(ulong), typeof(ulong)]),
            _ => null
        };
        var minMethodReference = method.Module.ImportReference(minMethod!);

        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldc_I4_0));
        il.Append(il.Create(loadElement));

        for (var index = 1; index < elementCount; index++)
        {
            il.Append(il.Create(OpCodes.Ldarg_0));
            il.Append(il.Create(OpCodes.Ldc_I4, index));
            il.Append(il.Create(loadElement));
            il.Append(il.Create(OpCodes.Call, minMethodReference));
        }

        il.Append(il.Create(OpCodes.Ret));
    }

    private static bool IsSupportedVectorType(string typeName)
    {
        return TryGetSupportedVectorType(typeName, out _);
    }

    private static bool TryGetSupportedVectorType(string typeName, out SupportedVectorType vectorType)
    {
        vectorType = typeName switch
        {
            "<16 x i8>" => new SupportedVectorType("i8", 8, 16),
            "<8 x i8>" => new SupportedVectorType("i8", 8, 8),
            "<4 x i8>" => new SupportedVectorType("i8", 8, 4),
            "<8 x i16>" => new SupportedVectorType("i16", 16, 8),
            "<4 x i16>" => new SupportedVectorType("i16", 16, 4),
            "<4 x i32>" => new SupportedVectorType("i32", 32, 4),
            "<2 x i64>" => new SupportedVectorType("i64", 64, 2),
            _ => default
        };

        return vectorType.ElementType is not null;
    }

    private readonly record struct SupportedVectorType(
        string ElementType,
        int ElementWidth,
        int ElementCount);

    private sealed record MemoryHelperMethods(
        MethodDefinition CopyBytesI64,
        MethodDefinition SetBytesI64);

    private sealed record VectorHelperMethods(
        MethodDefinition Add,
        MethodDefinition Xor,
        MethodDefinition And,
        MethodDefinition Or,
        MethodDefinition Max,
        MethodDefinition Min,
        MethodDefinition MaxUnsigned,
        MethodDefinition MinUnsigned,
        MethodDefinition ReduceXor,
        MethodDefinition ReduceAdd,
        MethodDefinition ReduceOr,
        MethodDefinition ReduceAnd,
        MethodDefinition ReduceMax,
        MethodDefinition ReduceMin,
        MethodDefinition ReduceMaxUnsigned,
        MethodDefinition ReduceMinUnsigned,
        MethodDefinition AddI64,
        MethodDefinition XorI64,
        MethodDefinition AndI64,
        MethodDefinition OrI64,
        MethodDefinition MaxI64,
        MethodDefinition MinI64,
        MethodDefinition MaxUnsignedI64,
        MethodDefinition MinUnsignedI64,
        MethodDefinition ReduceAddI64,
        MethodDefinition ReduceXorI64,
        MethodDefinition ReduceOrI64,
        MethodDefinition ReduceAndI64,
        MethodDefinition ReduceMaxI64,
        MethodDefinition ReduceMinI64,
        MethodDefinition ReduceMaxUnsignedI64,
        MethodDefinition ReduceMinUnsignedI64,
        MethodDefinition AddI16x8,
        MethodDefinition AndI16x8,
        MethodDefinition OrI16x8,
        MethodDefinition XorI16x8,
        MethodDefinition MaxI16x8,
        MethodDefinition MinI16x8,
        MethodDefinition MaxUnsignedI16x8,
        MethodDefinition MinUnsignedI16x8,
        MethodDefinition ReduceXorI16x8,
        MethodDefinition ReduceAddI16x8,
        MethodDefinition ReduceOrI16x8,
        MethodDefinition ReduceAndI16x8,
        MethodDefinition ReduceMaxI16x8,
        MethodDefinition ReduceMinI16x8,
        MethodDefinition ReduceMaxUnsignedI16x8,
        MethodDefinition ReduceMinUnsignedI16x8,
        MethodDefinition AddI16x4,
        MethodDefinition AndI16x4,
        MethodDefinition OrI16x4,
        MethodDefinition XorI16x4,
        MethodDefinition MaxI16x4,
        MethodDefinition MinI16x4,
        MethodDefinition MaxUnsignedI16x4,
        MethodDefinition MinUnsignedI16x4,
        MethodDefinition ReduceXorI16x4,
        MethodDefinition ReduceAddI16x4,
        MethodDefinition ReduceOrI16x4,
        MethodDefinition ReduceAndI16x4,
        MethodDefinition ReduceMaxI16x4,
        MethodDefinition ReduceMinI16x4,
        MethodDefinition ReduceMaxUnsignedI16x4,
        MethodDefinition ReduceMinUnsignedI16x4,
        MethodDefinition MaxScalarI8,
        MethodDefinition MaxUnsignedScalarI8,
        MethodDefinition MinScalarI8,
        MethodDefinition MinUnsignedScalarI8,
        MethodDefinition FunnelShiftLeftI32,
        MethodDefinition AddI8x16,
        MethodDefinition OrI8x16,
        MethodDefinition XorI8x16,
        MethodDefinition MaxI8x16,
        MethodDefinition MinI8x16,
        MethodDefinition MaxUnsignedI8x16,
        MethodDefinition MinUnsignedI8x16,
        MethodDefinition ReduceXorI8x16,
        MethodDefinition ReduceAddI8x16,
        MethodDefinition ReduceOrI8x16,
        MethodDefinition ReduceMaxI8x16,
        MethodDefinition ReduceMinI8x16,
        MethodDefinition ReduceMaxUnsignedI8x16,
        MethodDefinition ReduceMinUnsignedI8x16,
        MethodDefinition AddI8x8,
        MethodDefinition OrI8x8,
        MethodDefinition XorI8x8,
        MethodDefinition MaxI8x8,
        MethodDefinition MinI8x8,
        MethodDefinition ReduceXorI8x8,
        MethodDefinition ReduceAddI8x8,
        MethodDefinition ReduceOrI8x8,
        MethodDefinition ReduceMaxI8x8,
        MethodDefinition ReduceMinI8x8,
        MethodDefinition AddI8x4,
        MethodDefinition AndI8x4,
        MethodDefinition OrI8x4,
        MethodDefinition XorI8x4,
        MethodDefinition MaxUnsignedI8x4,
        MethodDefinition MinUnsignedI8x4,
        MethodDefinition ReduceXorI8x4,
        MethodDefinition ReduceAddI8x4,
        MethodDefinition ReduceOrI8x4,
        MethodDefinition ReduceAndI8x4,
        MethodDefinition ReduceMaxUnsignedI8x4,
        MethodDefinition ReduceMinUnsignedI8x4);
}