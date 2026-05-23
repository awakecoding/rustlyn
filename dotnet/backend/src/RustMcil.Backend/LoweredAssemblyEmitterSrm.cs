using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace RustMcil.Backend;

/// <summary>
/// Assembly emitter using System.Reflection.Metadata (SRM) — no Mono.Cecil dependency.
/// Currently a parallel implementation for validation; will replace LoweredAssemblyEmitter.cs.
/// </summary>
public static class LoweredAssemblyEmitterSrm
{
    public static void EmitBitcodeSrm(string artifactPath, string outputAssemblyPath, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputAssemblyPath);

        var outputFullPath = Path.GetFullPath(outputAssemblyPath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath) ?? throw new InvalidOperationException("Output directory could not be determined."));

        var assemblyName = Path.GetFileNameWithoutExtension(outputFullPath);
        var loweredModule = LoweredIrLowerer.LowerBitcode(artifactPath, llvmRoot);
        var emittedFunctions = LoweredAssemblyEmitter.GetReachableFunctions(loweredModule.Functions);

        EmitAssembly(outputFullPath, assemblyName, loweredModule, emittedFunctions);
    }

    private static void EmitAssembly(string outputPath, string assemblyName, LoweredModule loweredModule, IReadOnlyList<LoweredFunction> functions)
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
        metadataBuilder.AddModule(
            generation: 0,
            moduleName: metadataBuilder.GetOrAddString(assemblyName + ".dll"),
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

        // Type reference: System.Object (base type for our generated class)
        var systemObjectRef = metadataBuilder.AddTypeReference(
            systemRuntimeRef,
            metadataBuilder.GetOrAddString("System"),
            metadataBuilder.GetOrAddString("Object"));

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
        var methodHandles = new Dictionary<string, MethodDefinitionHandle>(StringComparer.Ordinal);
        for (var i = 0; i < functions.Count; i++)
        {
            methodHandles[functions[i].Name] = MetadataTokens.MethodDefinitionHandle(i + 1);
        }

        // Emit method bodies first (need all handle mappings available for intra-module calls)
        var bodyOffsets = new int[functions.Count];
        for (var i = 0; i < functions.Count; i++)
        {
            bodyOffsets[i] = EmitFunctionBody(metadataBuilder, methodBodyStream, functions[i], methodHandles);
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

        // Write PE
        WritePE(outputPath, metadataBuilder, ilBuilder, entryPoint: default);
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

    private static int EmitFunctionBody(MetadataBuilder metadataBuilder, MethodBodyStreamEncoder methodBodyStream, LoweredFunction function, IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles)
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
        foreach (var block in function.Blocks)
        {
            foreach (var instruction in block.Instructions)
            {
                var resultName = GetInstructionResult(instruction);
                if (resultName is not null && !localIndices.ContainsKey(resultName))
                {
                    localIndices[resultName] = localCount++;
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
                localEncoder.AddVariable().Type().Int32();
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

            foreach (var instruction in block.Instructions)
            {
                EmitInstruction(encoder, instruction, paramIndices, localIndices, labelMap, methodHandles, phiByBlock, block.Name);
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
        LoweredInstruction instruction,
        IReadOnlyDictionary<string, int> paramIndices,
        IReadOnlyDictionary<string, int> localIndices,
        IReadOnlyDictionary<string, LabelHandle> labelMap,
        IReadOnlyDictionary<string, MethodDefinitionHandle> methodHandles,
        IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock,
        string sourceBlock)
    {
        switch (instruction)
        {
            case LoweredBinaryInstruction binary:
                EmitLoadValue(encoder, binary.Left, paramIndices, localIndices);
                EmitLoadValue(encoder, binary.Right, paramIndices, localIndices);
                encoder.OpCode(MapBinaryOp(binary.Operation));
                encoder.StoreLocal(localIndices[binary.Result]);
                break;

            case LoweredReturnInstruction ret:
                if (!string.Equals(ret.Type, "void", StringComparison.Ordinal))
                {
                    EmitLoadValue(encoder, ret.Value, paramIndices, localIndices);
                }
                encoder.OpCode(ILOpCode.Ret);
                break;

            case LoweredConditionalBranchInstruction condBr:
                // For conditional branches with PHI targets: emit condition check,
                // then separate PHI copies on each path
                {
                    var trueHasPhis = phiByBlock.TryGetValue(condBr.TrueTarget, out var truePhis) && truePhis.Length > 0;
                    var falseHasPhis = phiByBlock.TryGetValue(condBr.FalseTarget, out var falsePhis) && falsePhis.Length > 0;

                    if (!trueHasPhis && !falseHasPhis)
                    {
                        // Simple case: no PHIs on either target
                        EmitLoadValue(encoder, condBr.Condition, paramIndices, localIndices);
                        encoder.Branch(ILOpCode.Brtrue, labelMap[condBr.TrueTarget]);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.FalseTarget]);
                    }
                    else
                    {
                        // PHI case: use brfalse to skip true-path PHI copies
                        var falsePathLabel = encoder.DefineLabel();
                        EmitLoadValue(encoder, condBr.Condition, paramIndices, localIndices);
                        encoder.Branch(ILOpCode.Brfalse, falsePathLabel);
                        // True path: emit PHI copies then jump to true target
                        EmitPhiCopies(encoder, paramIndices, localIndices, phiByBlock, sourceBlock, condBr.TrueTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.TrueTarget]);
                        // False path: emit PHI copies then jump to false target
                        encoder.MarkLabel(falsePathLabel);
                        EmitPhiCopies(encoder, paramIndices, localIndices, phiByBlock, sourceBlock, condBr.FalseTarget);
                        encoder.Branch(ILOpCode.Br, labelMap[condBr.FalseTarget]);
                    }
                }
                break;

            case LoweredJumpInstruction jump:
                EmitPhiCopies(encoder, paramIndices, localIndices, phiByBlock, sourceBlock, jump.Target);
                encoder.Branch(ILOpCode.Br, labelMap[jump.Target]);
                break;

            case LoweredCompareInstruction cmp:
                EmitLoadValue(encoder, cmp.Left, paramIndices, localIndices);
                EmitLoadValue(encoder, cmp.Right, paramIndices, localIndices);
                EmitCompare(encoder, cmp.Predicate);
                encoder.StoreLocal(localIndices[cmp.Result]);
                break;

            case LoweredPhiInstruction:
                // PHI values are stored before branches — nothing to emit here
                break;

            case LoweredSelectInstruction sel:
                // Simplified: just load true value for now
                EmitLoadValue(encoder, sel.TrueValue, paramIndices, localIndices);
                encoder.StoreLocal(localIndices[sel.Result]);
                break;

            case LoweredTruncateInstruction trunc:
                EmitLoadValue(encoder, trunc.Value, paramIndices, localIndices);
                encoder.OpCode(ILOpCode.Conv_i4);
                encoder.StoreLocal(localIndices[trunc.Result]);
                break;

            case LoweredZeroExtendInstruction zext:
                EmitLoadValue(encoder, zext.Value, paramIndices, localIndices);
                encoder.OpCode(ILOpCode.Conv_u8);
                encoder.StoreLocal(localIndices[zext.Result]);
                break;

            case LoweredSignExtendInstruction sext:
                EmitLoadValue(encoder, sext.Value, paramIndices, localIndices);
                encoder.OpCode(ILOpCode.Conv_i8);
                encoder.StoreLocal(localIndices[sext.Result]);
                break;

            case LoweredCallInstruction call:
                // Emit arguments
                foreach (var arg in call.Arguments)
                {
                    EmitLoadValue(encoder, arg.Value, paramIndices, localIndices);
                }
                // Call the function if it's in our module
                if (methodHandles.TryGetValue(call.Callee, out var calleeHandle))
                {
                    encoder.Call(calleeHandle);
                }
                else
                {
                    // External call — pop args and push zero for now
                    foreach (var _ in call.Arguments)
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
                break;

            case LoweredAllocaInstruction:
            case LoweredStoreInstruction:
            case LoweredLoadInstruction:
            case LoweredGetElementPointerInstruction:
            case LoweredRawInstruction:
            case LoweredUnreachableInstruction:
                // Not yet implemented in spike
                encoder.OpCode(ILOpCode.Nop);
                break;

            default:
                encoder.OpCode(ILOpCode.Nop);
                break;
        }
    }

    private static void EmitPhiCopies(InstructionEncoder encoder, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices, IReadOnlyDictionary<string, LoweredPhiInstruction[]> phiByBlock, string sourceBlock, string targetBlock)
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

            EmitLoadValue(encoder, incoming.Value, paramIndices, localIndices);
            encoder.StoreLocal(localIndices[phi.Result]);
        }
    }

    private static void EmitLoadValue(InstructionEncoder encoder, string value, IReadOnlyDictionary<string, int> paramIndices, IReadOnlyDictionary<string, int> localIndices)
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

        if (long.TryParse(value, out var longConst))
        {
            if (longConst is >= int.MinValue and <= int.MaxValue)
                encoder.LoadConstantI4((int)longConst);
            else
                encoder.LoadConstantI8(longConst);
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

    private static bool TryGetIntegerBitWidth(string typeName, out int width)
    {
        width = 0;
        if (typeName.Length < 2 || typeName[0] != 'i')
            return false;
        return int.TryParse(typeName.AsSpan(1), out width) && width > 0;
    }
}
