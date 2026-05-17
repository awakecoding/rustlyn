using Mono.Cecil;
using Mono.Cecil.Cil;

namespace RustMcil.Backend;

public static class LoweredAssemblyEmitter
{
    private const string GeneratedTypeName = "RustMcil.GeneratedModule";

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
        var assembly = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(assemblyName, new Version(1, 0, 0, 0)), assemblyName, ModuleKind.Dll);
        var module = assembly.MainModule;

        var generatedType = new TypeDefinition(
            "RustMcil",
            "GeneratedModule",
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class,
            module.TypeSystem.Object);

        module.Types.Add(generatedType);

        var vectorHelpers = EmitVectorHelpers(module, generatedType);

        var methodMap = new Dictionary<string, MethodDefinition>(StringComparer.Ordinal);
        var fieldMap = new Dictionary<string, FieldDefinition>(StringComparer.Ordinal);
        var globalMap = loweredModule.Globals.ToDictionary(global => global.Name, StringComparer.Ordinal);

        foreach (var global in loweredModule.Globals)
        {
            var field = new FieldDefinition(
                global.Name,
                FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly,
                ResolveGlobalFieldType(module, global));

            generatedType.Fields.Add(field);
            fieldMap.Add(global.Name, field);
        }

        if (loweredModule.Globals.Count > 0)
        {
            EmitTypeInitializer(module, generatedType, loweredModule.Globals, fieldMap);
        }

        foreach (var function in loweredModule.Functions)
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

        foreach (var function in loweredModule.Functions)
        {
            EmitFunctionBody(module, methodMap[function.Name], function, methodMap, fieldMap, globalMap, vectorHelpers);
        }

        assembly.Write(outputFullPath);
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

    private static void EmitFunctionBody(ModuleDefinition module, MethodDefinition method, LoweredFunction function, IReadOnlyDictionary<string, MethodDefinition> methodMap, IReadOnlyDictionary<string, FieldDefinition> fieldMap, IReadOnlyDictionary<string, LoweredGlobal> globalMap, VectorHelperMethods vectorHelpers)
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

        method.Body.InitLocals = true;
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
                        if (TryHandleMemoryIntrinsic(call, addressMap, memoryAliases, function.Name))
                        {
                            break;
                        }

                        if (TryEmitKnownPanicCall(module, il, call))
                        {
                            break;
                        }

                        foreach (var argument in call.Arguments)
                        {
                            LoadValue(il, parameters, locals, argument.Type, argument.Value);
                        }

                        if (TryResolveIntrinsicCall(module, call, vectorHelpers, out var intrinsicMethod))
                        {
                            il.Append(il.Create(OpCodes.Call, intrinsicMethod));
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
                            if (!ShouldSkipCallResultWidthNormalization(call))
                            {
                                EmitIntegerWidthNormalization(il, call.ReturnType, null, function.Name);
                            }

                            StoreLocal(method, il, locals, call.Result, call.ReturnType);
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
                        break;

                    case LoweredGetElementPointerInstruction gep:
                        addressMap[gep.Result] = gep;
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
                        LoadValue(il, parameters, locals, store.Type, store.Value);
                        if (addressMap.TryGetValue(store.Destination, out var storeAddress))
                        {
                            if (TryStoreByteOffsetAddress(method, il, locals, storeAddress, store.Type, memoryAliases))
                            {
                                break;
                            }

                            StoreLocal(method, il, locals, GetIndexedSlotName(storeAddress), store.Type);
                            break;
                        }

                        StoreLocal(method, il, locals, store.Destination, store.Type);
                        break;

                    case LoweredLoadInstruction load:
                        if (addressMap.TryGetValue(load.Source, out var loadAddress))
                        {
                            if (TryLoadByteOffsetAddress(method, il, locals, load, loadAddress, memoryAliases))
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
                            if (!TryResolveConstantGlobalElement(load, globalMap, out var constantValue))
                            {
                                throw new NotSupportedException($"Load source '{load.Source}' in function '{function.Name}' is not a local or supported global.");
                            }

                            EmitConstant(il, load.Type, constantValue);
                            StoreLocal(method, il, locals, load.Result, load.Type);
                            break;
                        }

                        il.Append(il.Create(OpCodes.Ldsfld, field));
                        StoreLocal(method, il, locals, load.Result, load.Type);
                        break;

                    case LoweredReturnInstruction ret:
                        LoadValue(il, parameters, locals, ret.Type, ret.Value);
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

    private static void LoadValue(ILProcessor il, IReadOnlyDictionary<string, ParameterDefinition> parameters, IDictionary<string, VariableDefinition> locals, string typeName, string value)
    {
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

        if (TryEmitVectorConstant(il, typeName, value))
        {
            return;
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
        if (TryEmitInsertElementSeed(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        if (TryEmitShuffleBroadcast(method, il, parameters, locals, raw.Text))
        {
            return true;
        }

        return false;
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

    private static bool TryHandleMemoryIntrinsic(LoweredCallInstruction call, IReadOnlyDictionary<string, LoweredGetElementPointerInstruction> addressMap, IDictionary<string, string> memoryAliases, string functionName)
    {
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
        if (!long.TryParse(call.Arguments[2].Value, out var copyLength) || copyLength <= 0)
        {
            throw new NotSupportedException($"Memory intrinsic '{call.Callee}' in function '{functionName}' uses an unsupported copy length '{call.Arguments[2].Value}'.");
        }

        memoryAliases[destinationBase] = sourceBase;
        return true;
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

        il.Append(load.Type switch
        {
            "i32" => il.Create(OpCodes.Ldind_I4),
            "i64" => il.Create(OpCodes.Ldind_I8),
            _ => throw new NotSupportedException($"Pointer-parameter load for lowered type '{load.Type}' is not supported by the current emitter slice.")
        });

        StoreLocal(method, il, locals, load.Result, load.Type);
        return true;
    }

    private static string ResolveAliasedBase(string baseName, IReadOnlyDictionary<string, string> memoryAliases)
    {
        return memoryAliases.TryGetValue(baseName, out var alias)
            ? alias
            : baseName;
    }

    private static bool IsPointerParameter(ParameterDefinition parameter)
    {
        return parameter.ParameterType.MetadataType == MetadataType.IntPtr;
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

    private static string GetIndexedSlotName(LoweredGetElementPointerInstruction address)
    {
        return $"{address.Base}[{address.Index}]";
    }

    private static int GetPrimitiveByteSize(string typeName)
    {
        return typeName switch
        {
            "i32" => 4,
            "i64" => 8,
            _ => throw new NotSupportedException($"Primitive byte size is not defined for lowered type '{typeName}'.")
        };
    }

    private static void EmitConstant(ILProcessor il, string typeName, long constant)
    {
        il.Append(il.Create(OpCodes.Ldc_I8, constant));
        EmitConversion(il, typeName);
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
            LoadValue(il, parameters, locals, edge.Phi.Type, edge.Incoming.Value);
            StoreLocal(method, il, locals, tempLocalName, edge.Phi.Type);
        }

        foreach (var edge in edgeCopies)
        {
            var tempLocalName = GetPhiTempLocalName(targetBlock, edge.Phi.Result, sourceBlock);
            LoadValue(il, parameters, locals, edge.Phi.Type, tempLocalName);
            StoreLocal(method, il, locals, edge.Phi.Result, edge.Phi.Type);
        }
    }

    private static string GetPhiTempLocalName(string targetBlock, string resultName, string sourceBlock)
    {
        return $"$phi.{targetBlock}.{resultName}.{sourceBlock}";
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

    private static TypeReference ResolveGlobalFieldType(ModuleDefinition module, LoweredGlobal global)
    {
        return global.InitializerBytes.Count switch
        {
            4 => module.TypeSystem.Int32,
            8 => module.TypeSystem.Int64,
            _ => throw new NotSupportedException($"Global '{global.Name}' has {global.InitializerBytes.Count} initializer bytes. Only 4-byte and 8-byte integer globals are supported by the current emitter slice.")
        };
    }

    private static void EmitTypeInitializer(ModuleDefinition module, TypeDefinition generatedType, IReadOnlyList<LoweredGlobal> globals, IReadOnlyDictionary<string, FieldDefinition> fieldMap)
    {
        var typeInitializer = new MethodDefinition(
            ".cctor",
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
            module.TypeSystem.Void);

        generatedType.Methods.Add(typeInitializer);
        var il = typeInitializer.Body.GetILProcessor();

        foreach (var global in globals)
        {
            var field = fieldMap[global.Name];
            switch (global.InitializerBytes.Count)
            {
                case 4:
                    il.Append(il.Create(OpCodes.Ldc_I4, BitConverter.ToInt32([.. global.InitializerBytes], 0)));
                    break;
                case 8:
                    il.Append(il.Create(OpCodes.Ldc_I8, BitConverter.ToInt64([.. global.InitializerBytes], 0)));
                    break;
                default:
                    throw new NotSupportedException($"Global '{global.Name}' has an unsupported initializer size of {global.InitializerBytes.Count} bytes.");
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