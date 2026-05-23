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
                EmitInstruction(encoder, block.Instructions, ref instrIdx, paramIndices, localIndices, labelMap, methodHandles, phiByBlock, block.Name, typeContext, fieldHandles, globalMap);
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
        IReadOnlyDictionary<string, LoweredGlobal> globalMap)
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
                encoder.OpCode(ILOpCode.Conv_i4);
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

            case LoweredCallInstruction call:
                {
                    var callArgs = call.Arguments;
                    var isCtlzCttz = call.Callee.StartsWith("llvm.ctlz.", StringComparison.Ordinal)
                                  || call.Callee.StartsWith("llvm.cttz.", StringComparison.Ordinal);
                    var effectiveArgCount = isCtlzCttz ? Math.Min(callArgs.Count, 1) : callArgs.Count;

                    for (var argIdx = 0; argIdx < effectiveArgCount; argIdx++)
                    {
                        EmitLoadValue(encoder, callArgs[argIdx].Value, paramIndices, localIndices, fieldHandles);
                    }

                    if (methodHandles.TryGetValue(call.Callee, out var calleeHandle))
                    {
                        encoder.Call(calleeHandle);
                    }
                    else if (typeContext.TryResolveIntrinsic(call.Callee, out var intrinsicHandle))
                    {
                        encoder.Call(intrinsicHandle);
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

            case LoweredAllocaInstruction:
                // Alloca maps to a local variable slot — no IL needed
                break;

            case LoweredStoreInstruction store:
                if (localIndices.TryGetValue(store.Destination, out var storeLocalIdx))
                {
                    EmitLoadValue(encoder, store.Value, paramIndices, localIndices, fieldHandles);
                    encoder.StoreLocal(storeLocalIdx);
                }
                break;

            case LoweredLoadInstruction load:
                {
                    if (localIndices.TryGetValue(load.Source, out var loadLocalIdx))
                    {
                        encoder.LoadLocal(loadLocalIdx);
                        if (IsGepResult(load.Source, instructions, instrIdx))
                        {
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
                    else
                    {
                        encoder.LoadConstantI4(0);
                        encoder.StoreLocal(localIndices[load.Result]);
                    }
                }
                break;

            case LoweredGetElementPointerInstruction gep:
                {
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

    private static void EmitLoadValue(InstructionEncoder encoder, string value, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, FieldDefinitionHandle> fieldHandles)
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

        // Global field reference
        if (fieldHandles.TryGetValue(value, out var fieldHandle))
        {
            encoder.OpCode(ILOpCode.Ldsfld);
            encoder.Token(fieldHandle);
            return;
        }

        // Fallback: push 0
        encoder.LoadConstantI4(0);
    }

    private static void EmitCompare(InstructionEncoder encoder, string predicate)
    {
        switch (predicate)
        {
            case "eq":
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "ne":
                encoder.OpCode(ILOpCode.Ceq);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "slt":
                encoder.OpCode(ILOpCode.Clt);
                break;
            case "ult":
                encoder.OpCode(ILOpCode.Clt_un);
                break;
            case "sgt":
                encoder.OpCode(ILOpCode.Cgt);
                break;
            case "ugt":
                encoder.OpCode(ILOpCode.Cgt_un);
                break;
            case "sle":
                encoder.OpCode(ILOpCode.Cgt);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "ule":
                encoder.OpCode(ILOpCode.Cgt_un);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "sge":
                encoder.OpCode(ILOpCode.Clt);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            case "uge":
                encoder.OpCode(ILOpCode.Clt_un);
                encoder.LoadConstantI4(0);
                encoder.OpCode(ILOpCode.Ceq);
                break;
            default:
                encoder.OpCode(ILOpCode.Ceq);
                break;
        }
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
        LoweredLoadInstruction l => l.Result,
        LoweredAllocaInstruction a => a.Result,
        LoweredGetElementPointerInstruction g => g.Result,
        _ => null
    };

    private static string GetLocalType(LoweredInstruction instruction) => instruction switch
    {
        LoweredGetElementPointerInstruction => "ptr",
        LoweredZeroExtendInstruction z => z.ToType,
        LoweredSignExtendInstruction s => s.ToType,
        LoweredLoadInstruction l => l.Type,
        LoweredAllocaInstruction => "ptr",
        LoweredPhiInstruction p => InferPhiType(p.Type),
        LoweredBinaryInstruction b => b.Type,
        LoweredCompareInstruction => "i32",
        LoweredSelectInstruction s => s.ValueType,
        LoweredTruncateInstruction t => t.ToType,
        LoweredCallInstruction c => c.ReturnType,
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
        foreach (var caseLabel in caseLabels)
        {
            EmitLoadValue(encoder, switchValue, paramIndices, localIndices, fieldHandles);
            encoder.LoadConstantI4((int)caseLabel.Value);
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
                encoder.LoadConstantI8(BitConverter.ToInt64([.. global.InitializerBytes], 0));
            }
            else
            {
                encoder.LoadConstantI4(BitConverter.ToInt32([.. global.InitializerBytes], 0));
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
    }
}
