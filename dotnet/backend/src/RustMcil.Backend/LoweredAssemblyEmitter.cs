using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace RustMcil.Backend;

/// <summary>
/// Assembly emitter using System.Reflection.Metadata (SRM) — no external dependencies.
/// </summary>
public static class LoweredAssemblyEmitter
{
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

        EmitAssembly(outputFullPath, assemblyName, loweredModule, emittedFunctions, consoleEntrypoint, requiresAvaloniaSupport);

        if (consoleEntrypoint is not null)
        {
            WriteRuntimeConfig(outputFullPath);
            CopyRuntimeSupportAssemblies(outputFullPath, requiresAvaloniaSupport);
        }
    }

    internal static IReadOnlyList<LoweredFunction> GetReachableFunctions(IReadOnlyList<LoweredFunction> functions)
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
                if (functionMap.ContainsKey(select.TrueValue))
                    pending.Push(select.TrueValue);
                if (functionMap.ContainsKey(select.FalseValue))
                    pending.Push(select.FalseValue);
            }

            // Function pointers stored into locals (e.g., store ptr @func -> local)
            foreach (var storeValue in function.Blocks
                         .SelectMany(static block => block.Instructions)
                         .OfType<LoweredStoreInstruction>()
                         .Select(static store => store.Value)
                         .Where(functionMap.ContainsKey))
            {
                pending.Push(storeValue);
            }

            // Function pointers passed as call arguments (e.g., call apply(ptr @double, i32 5))
            foreach (var argValue in function.Blocks
                         .SelectMany(static block => block.Instructions)
                         .OfType<LoweredCallInstruction>()
                         .SelectMany(static call => call.Arguments)
                         .Select(static arg => arg.Value)
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
            .Any(static call => call.Callee.StartsWith("rust_mcil_avalonia_", StringComparison.Ordinal));

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

    private static void CopyRuntimeSupportAssemblies(string outputAssemblyPath, bool requiresAvaloniaSupport)
    {
        CopySupportAssembly(typeof(RuntimeBridgeHelpers).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(RustMcil.Runtime.NumericRuntime).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(RustMcil.Os.HostEnvironment).Assembly.Location, outputAssemblyPath);
        CopySupportAssembly(typeof(RustMcil.Interop.ManagedInteropRuntime).Assembly.Location, outputAssemblyPath);
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

    private static void EmitAssembly(string outputPath, string assemblyName, LoweredModule loweredModule, IReadOnlyList<LoweredFunction> functions, LoweredFunction? consoleEntrypoint, bool requiresStaThread)
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
        var typeContext = new SrmTypeContext(metadataBuilder, systemRuntimeRef, systemInteropRef);

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
                attributes: FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly,
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

        // RustMcil.GeneratedModule static class
        metadataBuilder.AddTypeDefinition(
            attributes: TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class,
            @namespace: metadataBuilder.GetOrAddString("RustMcil"),
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

        // Emit method bodies first (need all handle mappings available for intra-module calls)
        var bodyOffsets = new int[totalMethods];
        var skippedFunctions = new List<(string Name, string Reason)>();
        for (var i = 0; i < functions.Count; i++)
        {
            try
            {
                bodyOffsets[i] = EmitFunctionBody(metadataBuilder, methodBodyStream, functions[i], methodHandles, typeContext, fieldHandles, globalMap);
            }
            catch (NotSupportedException ex)
            {
                skippedFunctions.Add((functions[i].Name, ex.Message));
                bodyOffsets[i] = EmitStubBody(metadataBuilder, methodBodyStream, typeContext, functions[i].Name, ex.Message);
            }
            catch (Exception ex) when (ex is not OutOfMemoryException)
            {
                skippedFunctions.Add((functions[i].Name, ex.Message));
                bodyOffsets[i] = EmitStubBody(metadataBuilder, methodBodyStream, typeContext, functions[i].Name, ex.Message);
            }
        }

        if (skippedFunctions.Count > 0)
        {
            Console.Error.WriteLine($"Warning: {skippedFunctions.Count} function(s) stubbed due to unsupported IR:");
            foreach (var (name, reason) in skippedFunctions)
            {
                Console.Error.WriteLine($"  {name}: {reason}");
            }
        }

        // Emit .cctor body if we have globals
        if (hasCctor)
        {
            bodyOffsets[functions.Count] = EmitCctorBody(metadataBuilder, methodBodyStream, loweredModule.Globals, fieldHandles, typeContext);
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

            for (var j = 0; j < function.Parameters.Count; j++)
            {
                metadataBuilder.AddParameter(
                    attributes: ParameterAttributes.None,
                    name: metadataBuilder.GetOrAddString(function.Parameters[j].Name),
                    sequenceNumber: j + 1);
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
        WritePE(outputPath, metadataBuilder, ilBuilder, entryPoint: entryPointHandle);
    }

    private static BlobHandle BuildMethodSignature(MetadataBuilder metadataBuilder, LoweredFunction function)
    {
        var blob = new BlobBuilder();
        new BlobEncoder(blob)
            .MethodSignature()
            .Parameters(function.Parameters.Count, out var returnTypeEncoder, out var parametersEncoder);

        EncodeReturnType(returnTypeEncoder, function.ReturnType);
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

    private static int EmitFunctionBody(MetadataBuilder metadataBuilder, MethodBodyStreamEncoder methodBodyStream, LoweredFunction function, IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles, SrmTypeContext typeContext, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, LoweredGlobal> globalMap)
    {
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
        foreach (var block in function.Blocks)
        {
            foreach (var instruction in block.Instructions)
            {
                var resultName = GetInstructionResult(instruction);
                if (resultName is not null && !localIndices.ContainsKey(resultName))
                {
                    localIndices[resultName] = localCount++;
                    localTypes.Add(GetLocalType(instruction));
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

        // Multi-value locals: for extractvalue on overflow-intrinsic calls
        // multiValueLocals maps a call result name -> array of local indices per field
        var multiValueLocals = new Dictionary<string, int[]>(StringComparer.Ordinal);
        foreach (var block in function.Blocks)
        {
            foreach (var instr in block.Instructions)
            {
                if (instr is LoweredExtractValueInstruction ev && !multiValueLocals.ContainsKey(ev.Source))
                {
                    // Only allocate multi-value locals for overflow intrinsic results
                    var sourceCall = FindCallByResult(function, ev.Source);
                    if (sourceCall is not null && sourceCall.Callee.Contains(".with.overflow.", StringComparison.Ordinal))
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
                switch (localTypes[i])
                {
                    case "ptr":
                        localEncoder.AddVariable().Type().IntPtr();
                        break;
                    case "i64":
                        localEncoder.AddVariable().Type().Int64();
                        break;
                    case "float":
                        localEncoder.AddVariable().Type().Single();
                        break;
                    case "double":
                        localEncoder.AddVariable().Type().Double();
                        break;
                    default:
                        localEncoder.AddVariable().Type().Int32();
                        break;
                }
            }
            localSigHandle = metadataBuilder.AddStandaloneSignature(metadataBuilder.GetOrAddBlob(localSigBlob));
        }

        // Parameter name -> argument index
        var paramIndices = new Dictionary<string, int>(StringComparer.Ordinal);
        for (var i = 0; i < function.Parameters.Count; i++)
        {
            paramIndices[function.Parameters[i].Name] = i;
        }

        // Emit IL per basic block
        foreach (var block in function.Blocks)
        {
            encoder.MarkLabel(labelMap[block.Name]);

            for (var instrIdx = 0; instrIdx < block.Instructions.Count; instrIdx++)
            {
                EmitInstruction(encoder, block.Instructions, ref instrIdx, paramIndices, localIndices, labelMap, methodHandles, phiByBlock, block.Name, typeContext, fieldHandles, globalMap, metadataBuilder, gepElementLocal, multiValueLocals, locallocAllocas, ptrParams);
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
        IReadOnlySet<string> ptrParams)
    {
        var instruction = instructions[instrIdx];
        switch (instruction)
        {
            case LoweredBinaryInstruction binary:
                EmitLoadValue(encoder, binary.Left, paramIndices, localIndices, fieldHandles);
                EmitLoadValue(encoder, binary.Right, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(MapBinaryOp(binary.Operation));
                encoder.StoreLocal(localIndices[binary.Result]);
                break;

            case LoweredReturnInstruction ret:
                if (!string.Equals(ret.Type, "void", StringComparison.Ordinal))
                {
                    EmitLoadValue(encoder, ret.Value, paramIndices, localIndices, fieldHandles);
                }
                encoder.OpCode(ILOpCode.Ret);
                break;

            case LoweredConditionalBranchInstruction condBr:
                {
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
                        EmitPhiCopies(encoder, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, condBr.TrueTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.TrueTarget]);
                        encoder.MarkLabel(falsePathLabel);
                        EmitPhiCopies(encoder, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, condBr.FalseTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.FalseTarget]);
                    }
                }
                break;

            case LoweredJumpInstruction jump:
                EmitPhiCopies(encoder, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, jump.Target);
                encoder.Branch(ILOpCode.Br, labelMap[jump.Target]);
                break;

            case LoweredCompareInstruction cmp:
                EmitLoadValue(encoder, cmp.Left, paramIndices, localIndices, fieldHandles);
                EmitLoadValue(encoder, cmp.Right, paramIndices, localIndices, fieldHandles);
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
                    EmitLoadValue(encoder, sel.TrueValue, paramIndices, localIndices, fieldHandles);
                    encoder.Branch(ILOpCode.Br, endLabel);
                    encoder.MarkLabel(falseLabel);
                    EmitLoadValue(encoder, sel.FalseValue, paramIndices, localIndices, fieldHandles);
                    encoder.MarkLabel(endLabel);
                    encoder.StoreLocal(localIndices[sel.Result]);
                }
                break;

            case LoweredTruncateInstruction trunc:
                EmitLoadValue(encoder, trunc.Value, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(trunc.ToType switch
                {
                    "float" => ILOpCode.Conv_r4,
                    "double" => ILOpCode.Conv_r8,
                    "i8" => ILOpCode.Conv_i1,
                    "i16" => ILOpCode.Conv_i2,
                    "i64" => ILOpCode.Conv_i8,
                    _ => ILOpCode.Conv_i4
                });
                encoder.StoreLocal(localIndices[trunc.Result]);
                break;

            case LoweredZeroExtendInstruction zext:
                EmitLoadValue(encoder, zext.Value, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(ILOpCode.Conv_u8);
                encoder.StoreLocal(localIndices[zext.Result]);
                break;

            case LoweredSignExtendInstruction sext:
                EmitLoadValue(encoder, sext.Value, paramIndices, localIndices, fieldHandles);
                encoder.OpCode(ILOpCode.Conv_i8);
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

                    // Memory intrinsics: memcpy/memset/memmove → cpblk/initblk
                    if (call.Callee.StartsWith("llvm.memcpy.", StringComparison.Ordinal)
                        || call.Callee.StartsWith("llvm.memmove.", StringComparison.Ordinal))
                    {
                        // cpblk: dest, src, size (drop isvolatile arg)
                        EmitLoadPtrValue(encoder, call.Arguments[0], paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadPtrValue(encoder, call.Arguments[1], paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadValue(encoder, call.Arguments[2].Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.OpCode(ILOpCode.Conv_u);
                        encoder.OpCode(ILOpCode.Cpblk);
                        break;
                    }
                    if (call.Callee.StartsWith("llvm.memset.", StringComparison.Ordinal))
                    {
                        // initblk: dest, val, size (drop isvolatile arg)
                        EmitLoadPtrValue(encoder, call.Arguments[0], paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadValue(encoder, call.Arguments[1].Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadValue(encoder, call.Arguments[2].Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        encoder.OpCode(ILOpCode.Conv_u);
                        encoder.OpCode(ILOpCode.Initblk);
                        break;
                    }

                    // Indirect call: callee is a local variable or parameter (function pointer)
                    var isIndirectCall = call.Callee.StartsWith('%')
                        && (localIndices.ContainsKey(call.Callee[1..]) || paramIndices.ContainsKey(call.Callee[1..]));

                    for (var argIdx = 0; argIdx < effectiveArgCount; argIdx++)
                    {
                        var arg = callArgs[argIdx];
                        // When a global field is passed as ptr, we need its address (ldsflda), not its value
                        if (string.Equals(arg.Type, "ptr", StringComparison.Ordinal)
                            && fieldHandles.TryGetValue(arg.Value, out var argFieldHandle))
                        {
                            encoder.OpCode(ILOpCode.Ldsflda);
                            encoder.Token(argFieldHandle);
                            encoder.OpCode(ILOpCode.Conv_u); // managed ptr → native int (IntPtr)
                        }
                        else
                        {
                            EmitLoadValue(encoder, arg.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        }
                    }

                    if (isIndirectCall)
                    {
                        // Load function pointer from local or parameter, then calli
                        var calleeName = call.Callee[1..];
                        if (localIndices.TryGetValue(calleeName, out var calleeLocalIdx))
                            encoder.LoadLocal(calleeLocalIdx);
                        else
                            encoder.LoadArgument(paramIndices[calleeName]);
                        encoder.CallIndirect(BuildStandaloneSignature(metadataBuilder, call.ReturnType, callArgs));
                    }
                    else if (methodHandles.TryGetValue(call.Callee, out var calleeHandle))
                    {
                        encoder.Call(calleeHandle);
                        // If this call returns a multi-value (struct), store fields
                        if (call.Result is not null && multiValueLocals.TryGetValue(call.Result, out var callMvLocals))
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
                    if (call.Result is not null)
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
                if (gepElementLocal.TryGetValue(store.Destination, out var gepStoreIdx))
                {
                    // Store to a GEP element slot (SROA)
                    EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                    encoder.StoreLocal(gepStoreIdx);
                }
                else if (localIndices.TryGetValue(store.Destination, out var storeLocalIdx))
                {
                    if (locallocAllocas.Contains(store.Destination) || IsGepResult(store.Destination, instructions, instrIdx))
                    {
                        // Store through a pointer (localloc'd alloca or non-SROA GEP result): ldloc ptr; value; stind
                        encoder.LoadLocal(storeLocalIdx);
                        EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitIndirectStore(encoder, store.Type);
                    }
                    else
                    {
                        EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles, methodHandles);
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
                break;

            case LoweredLoadInstruction load:
                {
                    if (gepElementLocal.TryGetValue(load.Source, out var gepLoadIdx))
                    {
                        // Load from a GEP element slot (SROA)
                        encoder.LoadLocal(gepLoadIdx);
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                    else if (localIndices.TryGetValue(load.Source, out var loadLocalIdx))
                    {
                        encoder.LoadLocal(loadLocalIdx);
                        if (locallocAllocas.Contains(load.Source) || IsGepResult(load.Source, instructions, instrIdx))
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
                        if (TryResolveConstantGlobalElement(load.Source, load.Type, globalMap, out var constantValue))
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
                        // Source is a packed i64 (from aggregate-returning function or insertvalue)
                        // Unpack: index 0 = low 32 bits, index 1 = high 32 bits
                        encoder.LoadLocal(srcLocal);
                        if (ev.Index > 0)
                        {
                            encoder.LoadConstantI4(ev.Index * 32);
                            encoder.OpCode(ILOpCode.Shr_un);
                        }
                        encoder.OpCode(ILOpCode.Conv_i4);
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
                break;

            case LoweredAtomicRmwInstruction rmw:
                {
                    // atomicrmw op ptr %ptr, i32 %val → old value
                    // Single-threaded semantics: old = *ptr; *ptr = op(old, val); result = old
                    EmitLoadValue(encoder, rmw.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
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
                        EmitLoadValue(encoder, rmw.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
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
                        EmitLoadValue(encoder, cx.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
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
                        EmitLoadValue(encoder, cx.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitLoadValue(encoder, cx.NewValue, paramIndices, localIndices, fieldHandles, methodHandles);
                        EmitIndirectStore(encoder, cx.ValueType);
                        encoder.MarkLabel(cxSkipLabel);
                        encoder.OpCode(ILOpCode.Nop);
                    }
                    else if (localIndices.TryGetValue(cx.Result, out var cxResIdx))
                    {
                        // Pack into i64: low 32 = old value, high 32 = success flag
                        EmitLoadValue(encoder, cx.Pointer, paramIndices, localIndices, fieldHandles, methodHandles);
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

            case LoweredRawInstruction raw when TryEmitRawSwitch(encoder, raw, instructions, ref instrIdx, paramIndices, localIndices, fieldHandles, labelMap, phiByBlock, sourceBlock):
                break;

            case LoweredRawInstruction:
                encoder.OpCode(ILOpCode.Nop);
                break;

            case LoweredUnreachableInstruction:
                encoder.OpCode(ILOpCode.Ldnull);
                encoder.OpCode(ILOpCode.Throw);
                break;

            default:
                encoder.OpCode(ILOpCode.Nop);
                break;
        }
    }

    private static void EmitPhiCopies(InstructionEncoder encoder, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock, string sourceBlock, string targetBlock)
    {
        if (!phiByBlock.TryGetValue(targetBlock, out var phiInstructions) || phiInstructions.Length == 0)
        {
            return;
        }

        foreach (var phi in phiInstructions)
        {
            var incoming = phi.Incoming.FirstOrDefault(i => string.Equals(i.SourceBlock, sourceBlock, StringComparison.Ordinal));
            if (incoming is null)
            {
                continue;
            }

            EmitLoadValue(encoder, incoming.Value, paramIndices, localIndices, fieldHandles);
            encoder.StoreLocal(localIndices[phi.Result]);
        }
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
        if (fieldHandles.TryGetValue(value, out var fieldHandle))
        {
            encoder.OpCode(ILOpCode.Ldsfld);
            encoder.Token(fieldHandle);
            return;
        }

        // Function reference → ldftn (for function pointers stored into locals)
        if (methodHandles is not null && methodHandles.TryGetValue(value, out var fnHandle))
        {
            encoder.OpCode(ILOpCode.Ldftn);
            encoder.Token(fnHandle);
            return;
        }

        // Fallback: push 0
        encoder.LoadConstantI4(0);
    }

    /// <summary>
    /// Load a pointer-typed argument value. For fields, emits ldsflda; for others, delegates to EmitLoadValue.
    /// </summary>
    private static void EmitLoadPtrValue(InstructionEncoder encoder, LoweredArgument arg, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, IReadOnlyDictionary<string, MethodDefinitionHandle>? methodHandles = null)
    {
        if (string.Equals(arg.Type, "ptr", StringComparison.Ordinal)
            && fieldHandles.TryGetValue(arg.Value, out var fh))
        {
            encoder.OpCode(ILOpCode.Ldsflda);
            encoder.Token(fh);
            encoder.OpCode(ILOpCode.Conv_u); // managed ptr → native int (IntPtr)
        }
        else
        {
            EmitLoadValue(encoder, arg.Value, paramIndices, localIndices, fieldHandles, methodHandles);
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

    private static StandaloneSignatureHandle BuildStandaloneSignature(
        MetadataBuilder metadataBuilder, string returnType, IReadOnlyList<LoweredArgument> args)
    {
        var sigBlob = new BlobBuilder();
        new BlobEncoder(sigBlob)
            .MethodSignature(SignatureCallingConvention.Default)
            .Parameters(args.Count,
                returnEncoder =>
                {
                    if (returnType == "void")
                        returnEncoder.Void();
                    else
                        returnEncoder.Type().Int32();
                },
                parametersEncoder =>
                {
                    for (var i = 0; i < args.Count; i++)
                    {
                        parametersEncoder.AddParameter().Type().Int32();
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
        LoweredLoadInstruction l => l.Type,
        LoweredAllocaInstruction => "ptr",
        LoweredPhiInstruction p => InferPhiType(p.Type),
        LoweredBinaryInstruction b => b.Type,
        LoweredCompareInstruction => "i32",
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

    private static bool TryEmitRawSwitch(
        InstructionEncoder encoder,
        LoweredRawInstruction raw,
        IReadOnlyList<LoweredInstruction> instructions,
        ref int instrIdx,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles,
        IReadOnlyDictionary<string, LabelHandle> labelMap,
        IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock,
        string sourceBlock)
    {
        var switchMatch = System.Text.RegularExpressions.Regex.Match(
            raw.Text,
            "^switch (?<type>i\\d+) (?<value>[^,]+), label %(?<defaultTarget>[^ ]+) \\[$",
            System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        if (!switchMatch.Success)
        {
            return false;
        }

        var switchValue = NormalizeRawValue(switchMatch.Groups["value"].Value);
        var defaultTarget = switchMatch.Groups["defaultTarget"].Value;
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
                "^(?<type>i\\d+) (?<value>-?\\d+), label %(?<target>[^ ]+)$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant);
            if (caseMatch.Success)
            {
                caseLabels.Add((long.Parse(caseMatch.Groups["value"].Value), caseMatch.Groups["target"].Value));
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
            EmitPhiCopies(encoder, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, caseLabel.Target);
            encoder.Branch(ILOpCode.Br, labelMap[caseLabel.Target]);
            encoder.MarkLabel(nextCase);
        }

        // Default case
        EmitPhiCopies(encoder, paramIndices, localIndices, fieldHandles, phiByBlock, sourceBlock, defaultTarget);
        encoder.Branch(ILOpCode.Br, labelMap[defaultTarget]);
        instrIdx = closingIndex;
        return true;
    }

    private static string NormalizeRawValue(string rawValue)
    {
        // Strip type prefix: "%x" -> "x", "i32 %x" -> "x"
        var value = rawValue.Trim();
        var percentIdx = value.IndexOf('%');
        if (percentIdx >= 0)
        {
            return value[(percentIdx + 1)..];
        }
        return value;
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
                encoder.OpCode(ILOpCode.Ldind_i1);
            else if (width <= 16)
                encoder.OpCode(ILOpCode.Ldind_i2);
            else if (width <= 32)
                encoder.OpCode(ILOpCode.Ldind_i4);
            else
                encoder.OpCode(ILOpCode.Ldind_i8);
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

    private static int EmitCctorBody(MetadataBuilder metadataBuilder, MethodBodyStreamEncoder methodBodyStream, IReadOnlyList<LoweredGlobal> globals, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles, SrmTypeContext typeContext)
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

    private static void WritePE(string outputPath, MetadataBuilder metadataBuilder, BlobBuilder ilBuilder, MethodDefinitionHandle entryPoint)
    {
        var metadataRootBuilder = new MetadataRootBuilder(metadataBuilder);

        var peHeaderBuilder = entryPoint.IsNil
            ? PEHeaderBuilder.CreateLibraryHeader()
            : PEHeaderBuilder.CreateExecutableHeader();

        var peBuilder = new ManagedPEBuilder(
            header: peHeaderBuilder,
            metadataRootBuilder: metadataRootBuilder,
            ilStream: ilBuilder,
            entryPoint: entryPoint);

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
            @namespace: metadataBuilder.GetOrAddString("RustMcil"),
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
        public MemberReferenceHandle PopCountU32 { get; }
        public MemberReferenceHandle PopCountU64 { get; }
        public MemberReferenceHandle LeadingZeroCountU32 { get; }
        public MemberReferenceHandle LeadingZeroCountU64 { get; }
        public MemberReferenceHandle TrailingZeroCountU32 { get; }
        public MemberReferenceHandle TrailingZeroCountU64 { get; }
        public MemberReferenceHandle ReverseEndianness32 { get; }
        public MemberReferenceHandle ReverseEndianness64 { get; }
        public MemberReferenceHandle MarshalAllocHGlobal { get; }
        public MemberReferenceHandle MarshalCopy { get; }
        public MemberReferenceHandle NotSupportedExceptionCtor { get; }
        public MemberReferenceHandle MathSqrt { get; }
        public MemberReferenceHandle MathFloor { get; }
        public MemberReferenceHandle MathCeiling { get; }
        public MemberReferenceHandle MathAbs { get; }
        public MemberReferenceHandle MathMin { get; }
        public MemberReferenceHandle MathMax { get; }
        public MemberReferenceHandle MathPow { get; }
        public MemberReferenceHandle MathFSqrt { get; }
        public MemberReferenceHandle MathFFloor { get; }
        public MemberReferenceHandle MathFCeiling { get; }
        public MemberReferenceHandle MathFAbs { get; }

        private readonly Dictionary<string, MemberReferenceHandle> _intrinsicMap;

        public SrmTypeContext(MetadataBuilder mb, AssemblyReferenceHandle systemRuntime, AssemblyReferenceHandle systemInterop)
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
            };
        }

        public bool TryResolveIntrinsic(string callee, out MemberReferenceHandle handle)
        {
            return _intrinsicMap.TryGetValue(callee, out handle);
        }

        private static MemberReferenceHandle AddStaticMethod(MetadataBuilder mb, TypeReferenceHandle parent, string name, BlobHandle signature)
        {
            return mb.AddMemberReference(parent, mb.GetOrAddString(name), signature);
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
    }
}
