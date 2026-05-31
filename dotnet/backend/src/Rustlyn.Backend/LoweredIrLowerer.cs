using System.Text;
using System.Text.RegularExpressions;

namespace Rustlyn.Backend;

public static partial class LoweredIrLowerer
{
    public static LoweredModule LowerBitcode(string artifactPath, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);

        var fullArtifactPath = Path.GetFullPath(artifactPath);

#if RUSTLYN_BACKEND_IN_PROCESS_LLVM
        if (string.IsNullOrWhiteSpace(llvmRoot))
        {
            return LowerLlvmIr(InProcessLlvmModuleReader.ReadLlvmIr(fullArtifactPath));
        }
#endif

        var toolchainRoot = LlvmNativeLibraryLocator.TryResolveToolchainRoot(llvmRoot)
            ?? throw new InvalidOperationException("An LLVM toolchain root is required for lowering. Configure --llvm-root or RUSTLYN_LLVM_ROOT.");

        var optimizedPath = RustlynLlvmOptimizer.MaybeOptimize(fullArtifactPath, toolchainRoot);
        var readerMode = GetLlvmReaderMode();
        if (readerMode != LlvmReaderMode.Text)
        {
            var structuredModule = TryLowerBitcodeWithStructuredJson(optimizedPath, toolchainRoot, readerMode);
            if (structuredModule is not null)
            {
                return structuredModule;
            }
        }

        var llvmIr = LlvmToolingDisassembler.ReadLlvmIr(optimizedPath, toolchainRoot);
        return LowerLlvmIr(llvmIr);
    }

    public static LoweredModule LowerLlvmIr(string llvmIr)
    {
        var functions = new List<LoweredFunction>();
        var globals = new List<LoweredGlobal>();
        var namedTypes = new Dictionary<string, string>(StringComparer.Ordinal);
        string? currentFunctionName = null;
        string? currentReturnType = null;
        string? currentReturnExtension = null;
        List<LoweredParameter>? currentParameters = null;
        List<LoweredBlock>? currentBlocks = null;
        LoweredBlockBuilder? currentBlock = null;

        foreach (var rawLine in ReadLines(llvmIr))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';'))
            {
                continue;
            }

            if (currentFunctionName is null)
            {
                var namedTypeMatch = NamedTypeDefinitionRegex().Match(line);
                if (namedTypeMatch.Success)
                {
                    namedTypes[namedTypeMatch.Groups["name"].Value] = namedTypeMatch.Groups["type"].Value.Trim();
                    continue;
                }

                var aliasMatch = FunctionAliasRegex().Match(line);
                if (aliasMatch.Success)
                {
                    functions.Add(CreateAliasFunction(aliasMatch));
                    continue;
                }

                if (TryParsePointerRelocationGlobal(line, out var relocationGlobal))
                {
                    globals.Add(relocationGlobal);
                    continue;
                }

                if (TryParseScalarPointerRelocationGlobal(line, out var scalarRelocationGlobal))
                {
                    globals.Add(scalarRelocationGlobal);
                    continue;
                }

                var globalMatch = ConstantByteArrayGlobalRegex().Match(line);
                if (globalMatch.Success)
                {
                    globals.Add(new LoweredGlobal(
                        globalMatch.Groups["name"].Value,
                        ParseConstantByteArray(globalMatch.Groups["bytes"].Value)));
                    continue;
                }

                var intArrayGlobalMatch = ConstantIntegerArrayGlobalRegex().Match(line);
                if (intArrayGlobalMatch.Success)
                {
                    var intArrayBytes = ParseConstantIntegerArray(
                        intArrayGlobalMatch.Groups["elementType"].Value,
                        intArrayGlobalMatch.Groups["values"].Value);
                    if (intArrayBytes is not null)
                    {
                        globals.Add(new LoweredGlobal(
                            intArrayGlobalMatch.Groups["name"].Value,
                            intArrayBytes));
                        continue;
                    }
                }

                var zeroInitArrayMatch = ZeroInitByteArrayGlobalRegex().Match(line);
                if (zeroInitArrayMatch.Success)
                {
                    var size = int.Parse(zeroInitArrayMatch.Groups["size"].Value);
                    globals.Add(new LoweredGlobal(
                        zeroInitArrayMatch.Groups["name"].Value,
                        new byte[size]));
                    continue;
                }

                var zeroInitScalarMatch = ZeroInitScalarGlobalRegex().Match(line);
                if (zeroInitScalarMatch.Success)
                {
                    var bits = int.Parse(zeroInitScalarMatch.Groups["bits"].Value);
                    var byteSize = (bits + 7) / 8;
                    globals.Add(new LoweredGlobal(
                        zeroInitScalarMatch.Groups["name"].Value,
                        new byte[byteSize]));
                    continue;
                }

                var scalarGlobalMatch = ScalarIntegerGlobalRegex().Match(line);
                if (scalarGlobalMatch.Success)
                {
                    var bits = int.Parse(scalarGlobalMatch.Groups["bits"].Value);
                    var scalarBytes = ParseScalarIntegerBytes(bits, scalarGlobalMatch.Groups["value"].Value);
                    if (scalarBytes is not null)
                    {
                        globals.Add(new LoweredGlobal(
                            scalarGlobalMatch.Groups["name"].Value,
                            scalarBytes));
                        continue;
                    }
                }

                if (TryParseZeroableAggregateGlobal(line, out var zeroableAggregateGlobal))
                {
                    globals.Add(zeroableAggregateGlobal);
                    continue;
                }

                if (!TryParseFunctionHeader(line, out var functionName, out var returnType, out var returnExtension, out var parameters))
                {
                    continue;
                }

                currentFunctionName = functionName;
                currentReturnType = returnType;
                currentReturnExtension = returnExtension;
                currentParameters = parameters;
                currentBlocks = [];
                currentBlock = null;
                continue;
            }

            if (line == "}")
            {
                if (currentBlock is not null)
                {
                    currentBlocks!.Add(currentBlock.ToBlock());
                }

                functions.Add(new LoweredFunction(currentFunctionName, currentReturnType!, currentParameters!, currentBlocks!, currentReturnExtension));
                currentFunctionName = null;
                currentReturnType = null;
                currentReturnExtension = null;
                currentParameters = null;
                currentBlocks = null;
                currentBlock = null;
                continue;
            }

            var blockMatch = BasicBlockRegex().Match(line);
            if (blockMatch.Success)
            {
                if (currentBlock is not null)
                {
                    currentBlocks!.Add(currentBlock.ToBlock());
                }

                currentBlock = new LoweredBlockBuilder(NormalizeBlockName(blockMatch.Groups["name"].Value));
                continue;
            }

            currentBlock ??= new LoweredBlockBuilder(currentParameters!.Count.ToString());
            currentBlock.Add(ParseInstruction(line));
        }

        return new LoweredModule(functions, globals, namedTypes);
    }

    public static string Dump(LoweredModule module)
    {
        var builder = new StringBuilder();

        foreach (var function in module.Functions)
        {
            builder.Append(function.ReturnType);
            builder.Append(' ');
            builder.Append(function.Name);
            builder.Append('(');
            builder.Append(string.Join(", ", function.Parameters.Select(parameter => $"{parameter.Type} {parameter.Name}")));
            builder.AppendLine(")");

            foreach (var block in function.Blocks)
            {
                builder.Append("  block ");
                builder.AppendLine(block.Name);

                foreach (var instruction in block.Instructions)
                {
                    builder.Append("    ");
                    builder.AppendLine(FormatInstruction(instruction));
                }
            }
        }

        foreach (var global in module.Globals)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append("global ");
            builder.Append(global.Name);
            builder.Append(" = ");
            builder.Append(string.Join(" ", global.InitializerBytes.Select(static value => value.ToString("X2"))));
            foreach (var relocation in global.PointerRelocations)
            {
                builder.Append(" ; reloc +");
                builder.Append(relocation.Offset.ToString(System.Globalization.CultureInfo.InvariantCulture));
                builder.Append(" -> ");
                builder.Append(relocation.Target);
            }
        }

        return builder.ToString().TrimEnd();
    }

    private static List<LoweredParameter> ParseParameters(string parameterText)
    {
        if (string.IsNullOrWhiteSpace(parameterText))
        {
            return [];
        }

        var parameters = new List<LoweredParameter>();
        var remaining = parameterText.Trim();

        while (!string.IsNullOrEmpty(remaining))
        {
            var separatorIndex = FindTopLevelComma(remaining);
            string segment;
            if (separatorIndex < 0)
            {
                segment = remaining;
                remaining = string.Empty;
            }
            else
            {
                segment = remaining[..separatorIndex];
                remaining = remaining[(separatorIndex + 1)..].TrimStart();
            }

            var parameter = ParseParameter(segment);
            if (parameter is not null)
            {
                parameters.Add(parameter);
            }
        }

        return parameters;
    }

    private static LoweredInstruction ParseInstruction(string line)
    {
        line = StripTrailingInstructionMetadata(line);

        var earlyTyped = TryParseUnsupportedTyped(line);
        if (earlyTyped is not null)
        {
            return earlyTyped;
        }

        if (TryParseBinaryInstruction(line, out var binaryInstruction))
        {
            return binaryInstruction;
        }

            var truncateMatch = TruncateInstructionRegex().Match(line);
            if (truncateMatch.Success)
            {
                return new LoweredTruncateInstruction(
                NormalizeResultName(truncateMatch.Groups["result"].Value),
                NormalizeType(truncateMatch.Groups["fromType"].Value),
                NormalizeType(truncateMatch.Groups["toType"].Value),
                NormalizeValue(truncateMatch.Groups["value"].Value));
            }

        var zeroExtendMatch = ZeroExtendInstructionRegex().Match(line);
        if (zeroExtendMatch.Success)
        {
            return new LoweredZeroExtendInstruction(
                NormalizeResultName(zeroExtendMatch.Groups["result"].Value),
                NormalizeType(zeroExtendMatch.Groups["fromType"].Value),
                NormalizeType(zeroExtendMatch.Groups["toType"].Value),
                NormalizeValue(zeroExtendMatch.Groups["value"].Value));
        }

        var signExtendMatch = SignExtendInstructionRegex().Match(line);
        if (signExtendMatch.Success)
        {
            return new LoweredSignExtendInstruction(
                NormalizeResultName(signExtendMatch.Groups["result"].Value),
                NormalizeType(signExtendMatch.Groups["fromType"].Value),
                NormalizeType(signExtendMatch.Groups["toType"].Value),
                NormalizeValue(signExtendMatch.Groups["value"].Value));
        }

        var bitcastMatch = BitcastInstructionRegex().Match(line);
        if (bitcastMatch.Success)
        {
            return new LoweredBitcastInstruction(
                NormalizeResultName(bitcastMatch.Groups["result"].Value),
                NormalizeType(bitcastMatch.Groups["fromType"].Value),
                NormalizeType(bitcastMatch.Groups["toType"].Value),
                NormalizeValue(bitcastMatch.Groups["value"].Value));
        }

        var ptrToIntMatch = PtrToIntInstructionRegex().Match(line);
        if (ptrToIntMatch.Success)
        {
            return new LoweredPtrToIntInstruction(
                NormalizeResultName(ptrToIntMatch.Groups["result"].Value),
                NormalizeType(ptrToIntMatch.Groups["toType"].Value),
                NormalizeValue(ptrToIntMatch.Groups["value"].Value));
        }

        var intToPtrMatch = IntToPtrInstructionRegex().Match(line);
        if (intToPtrMatch.Success)
        {
            return new LoweredIntToPtrInstruction(
                NormalizeResultName(intToPtrMatch.Groups["result"].Value),
                NormalizeValue(intToPtrMatch.Groups["value"].Value));
        }

        var freezeMatch = FreezeInstructionRegex().Match(line);
        if (freezeMatch.Success)
        {
            return new LoweredFreezeInstruction(
                NormalizeResultName(freezeMatch.Groups["result"].Value),
                NormalizeType(freezeMatch.Groups["type"].Value),
                NormalizeValue(freezeMatch.Groups["value"].Value));
        }

        var floatConvertMatch = FloatConvertInstructionRegex().Match(line);
        if (floatConvertMatch.Success)
        {
            return new LoweredTruncateInstruction(
                NormalizeResultName(floatConvertMatch.Groups["result"].Value),
                NormalizeType(floatConvertMatch.Groups["fromType"].Value),
                NormalizeType(floatConvertMatch.Groups["toType"].Value),
                NormalizeValue(floatConvertMatch.Groups["value"].Value));
        }

        if (TryParseSelectInstruction(line, out var selectInstruction))
        {
            return selectInstruction;
        }

        var selectMatch = SelectInstructionRegex().Match(line);
        if (selectMatch.Success)
        {
            return new LoweredSelectInstruction(
                NormalizeResultName(selectMatch.Groups["result"].Value),
                NormalizeValue(selectMatch.Groups["condition"].Value),
                NormalizeType(selectMatch.Groups["valueType"].Value),
                NormalizeValue(selectMatch.Groups["trueValue"].Value),
                NormalizeValue(selectMatch.Groups["falseValue"].Value));
        }

        var phiMatch = PhiInstructionRegex().Match(line);
        if (phiMatch.Success)
        {
            return new LoweredPhiInstruction(
                NormalizeResultName(phiMatch.Groups["result"].Value),
                NormalizeType(phiMatch.Groups["type"].Value),
                ParsePhiIncoming(phiMatch.Groups["incoming"].Value));
        }

        if (UnreachableInstructionRegex().IsMatch(line))
        {
            return new LoweredUnreachableInstruction();
        }

        if (TryParseCallInstruction(line, out var callInstruction))
        {
            return callInstruction;
        }

        if (string.Equals(line, "ret void", StringComparison.Ordinal))
        {
            return new LoweredReturnInstruction("void", "void");
        }

        var returnMatch = ReturnInstructionRegex().Match(line);
        if (returnMatch.Success)
        {
            return new LoweredReturnInstruction(
                NormalizeType(returnMatch.Groups["type"].Value),
                NormalizeValue(StripTrailingInstructionMetadata(returnMatch.Groups["value"].Value)));
        }

        var compareMatch = CompareInstructionRegex().Match(line);
        if (compareMatch.Success)
        {
            return new LoweredCompareInstruction(
                NormalizeResultName(compareMatch.Groups["result"].Value),
                compareMatch.Groups["predicate"].Value,
                NormalizeType(compareMatch.Groups["type"].Value),
                NormalizeValue(compareMatch.Groups["left"].Value),
                NormalizeValue(compareMatch.Groups["right"].Value));
        }

        var branchMatch = ConditionalBranchInstructionRegex().Match(line);
        if (branchMatch.Success)
        {
            return new LoweredConditionalBranchInstruction(
                NormalizeValue(branchMatch.Groups["condition"].Value),
                NormalizeBlockName(branchMatch.Groups["trueLabel"].Value),
                NormalizeBlockName(branchMatch.Groups["falseLabel"].Value));
        }

        var jumpMatch = UnconditionalBranchInstructionRegex().Match(line);
        if (jumpMatch.Success)
        {
            return new LoweredJumpInstruction(NormalizeBlockName(jumpMatch.Groups["target"].Value));
        }

        var getElementPointerMatch = GetElementPointerInstructionRegex().Match(line);
        if (getElementPointerMatch.Success)
        {
            return new LoweredGetElementPointerInstruction(
                NormalizeResultName(getElementPointerMatch.Groups["result"].Value),
                getElementPointerMatch.Groups["elementType"].Value.Trim(),
                NormalizeValue(getElementPointerMatch.Groups["base"].Value),
                int.Parse(getElementPointerMatch.Groups["index"].Value));
        }

        var dynamicGepMatch = DynamicGetElementPointerInstructionRegex().Match(line);
        if (dynamicGepMatch.Success)
        {
            return new LoweredGetElementPointerInstruction(
                NormalizeResultName(dynamicGepMatch.Groups["result"].Value),
                dynamicGepMatch.Groups["elementType"].Value.Trim(),
                NormalizeValue(dynamicGepMatch.Groups["base"].Value),
                0,
                NormalizeValue(dynamicGepMatch.Groups["indexVar"].Value));
        }

        var globalElementLoadMatch = GlobalElementLoadInstructionRegex().Match(line);
        if (globalElementLoadMatch.Success)
        {
            return new LoweredLoadInstruction(
                NormalizeResultName(globalElementLoadMatch.Groups["result"].Value),
                NormalizeType(globalElementLoadMatch.Groups["type"].Value),
                $"{NormalizeValue(globalElementLoadMatch.Groups["source"].Value)}[{globalElementLoadMatch.Groups["index"].Value}]"
            );
        }

        var loadMatch = LoadInstructionRegex().Match(line);
        if (loadMatch.Success)
        {
            return new LoweredLoadInstruction(
                NormalizeResultName(loadMatch.Groups["result"].Value),
                NormalizeType(loadMatch.Groups["type"].Value),
                NormalizeValue(loadMatch.Groups["source"].Value));
        }

        var allocaMatch = AllocaInstructionRegex().Match(line);
        if (allocaMatch.Success)
        {
            return new LoweredAllocaInstruction(
                NormalizeResultName(allocaMatch.Groups["result"].Value),
                NormalizeType(allocaMatch.Groups["type"].Value));
        }

        var globalElementStoreMatch = GlobalElementStoreInstructionRegex().Match(line);
        if (globalElementStoreMatch.Success)
        {
            return new LoweredStoreInstruction(
                NormalizeType(globalElementStoreMatch.Groups["type"].Value),
                NormalizeValue(globalElementStoreMatch.Groups["value"].Value),
                $"{NormalizeValue(globalElementStoreMatch.Groups["destination"].Value)}[{globalElementStoreMatch.Groups["index"].Value}]");
        }

        if (TryParseStoreInstruction(line, out var storeInstruction))
        {
            return storeInstruction;
        }

        var storeMatch = StoreInstructionRegex().Match(line);
        if (storeMatch.Success)
        {
            return new LoweredStoreInstruction(
                NormalizeType(storeMatch.Groups["type"].Value),
                NormalizeValue(storeMatch.Groups["value"].Value),
                NormalizeValue(storeMatch.Groups["destination"].Value));
        }

        var extractValueMatch = ExtractValueInstructionRegex().Match(line);
        if (extractValueMatch.Success)
        {
            return new LoweredExtractValueInstruction(
                NormalizeResultName(extractValueMatch.Groups["result"].Value),
                extractValueMatch.Groups["aggType"].Value.Trim(),
                NormalizeValue(extractValueMatch.Groups["source"].Value),
                int.Parse(extractValueMatch.Groups["index"].Value));
        }

        var insertValueMatch = InsertValueInstructionRegex().Match(line);
        if (insertValueMatch.Success)
        {
            return new LoweredInsertValueInstruction(
                NormalizeResultName(insertValueMatch.Groups["result"].Value),
                insertValueMatch.Groups["aggType"].Value.Trim(),
                NormalizeValue(insertValueMatch.Groups["base"].Value),
                NormalizeValue(insertValueMatch.Groups["value"].Value),
                int.Parse(insertValueMatch.Groups["index"].Value));
        }

        var atomicRmwMatch = AtomicRmwInstructionRegex().Match(line);
        if (atomicRmwMatch.Success)
        {
            return new LoweredAtomicRmwInstruction(
                NormalizeResultName(atomicRmwMatch.Groups["result"].Value),
                atomicRmwMatch.Groups["op"].Value,
                NormalizeValue(atomicRmwMatch.Groups["ptr"].Value),
                atomicRmwMatch.Groups["valType"].Value.Trim(),
                NormalizeValue(atomicRmwMatch.Groups["val"].Value));
        }

        var globalElementCmpxchgMatch = GlobalElementCmpxchgInstructionRegex().Match(line);
        if (globalElementCmpxchgMatch.Success)
        {
            return new LoweredCmpxchgInstruction(
                NormalizeResultName(globalElementCmpxchgMatch.Groups["result"].Value),
                $"{NormalizeValue(globalElementCmpxchgMatch.Groups["ptr"].Value)}[{globalElementCmpxchgMatch.Groups["index"].Value}]",
                globalElementCmpxchgMatch.Groups["valType"].Value.Trim(),
                NormalizeValue(globalElementCmpxchgMatch.Groups["cmpVal"].Value),
                NormalizeValue(globalElementCmpxchgMatch.Groups["newVal"].Value));
        }

        var cmpxchgMatch = CmpxchgInstructionRegex().Match(line);
        if (cmpxchgMatch.Success)
        {
            return new LoweredCmpxchgInstruction(
                NormalizeResultName(cmpxchgMatch.Groups["result"].Value),
                NormalizeValue(cmpxchgMatch.Groups["ptr"].Value),
                cmpxchgMatch.Groups["valType"].Value.Trim(),
                NormalizeValue(cmpxchgMatch.Groups["cmpVal"].Value),
                NormalizeValue(cmpxchgMatch.Groups["newVal"].Value));
        }

        // Typed detectors for known-unsupported constructs. We keep them as semantic records so
        // the emitter can fail with structured diagnostics in strict mode (or stub bodies in
        // permissive mode) instead of treating every unknown line as opaque raw text.
        var typed = TryParseUnsupportedTyped(line);
        if (typed is not null)
        {
            return typed;
        }

        return new LoweredRawInstruction(line);
    }

    private static LoweredInstruction? TryParseUnsupportedTyped(string line)
    {
        var trimmed = line.TrimStart();

        // Strip "<result> = " prefix if present, capturing the result name.
        string? result = null;
        var eqIndex = trimmed.IndexOf('=');
        if (eqIndex > 0 && trimmed.StartsWith('%'))
        {
            result = NormalizeResultName(trimmed.Substring(0, eqIndex).Trim());
            trimmed = trimmed.Substring(eqIndex + 1).TrimStart();
        }

        if (trimmed.StartsWith("invoke ", StringComparison.Ordinal))
        {
            // Capture normal/unwind labels for diagnostics, callee text in body.
            string normalLabel = "?", unwindLabel = "?";
            var toIdx = trimmed.IndexOf(" to label %", StringComparison.Ordinal);
            if (toIdx >= 0)
            {
                var rest = trimmed.Substring(toIdx + " to label %".Length);
                var space = rest.IndexOf(' ');
                normalLabel = space > 0 ? rest.Substring(0, space) : rest;
                var unwindIdx = rest.IndexOf("unwind label %", StringComparison.Ordinal);
                if (unwindIdx >= 0)
                {
                    var u = rest.Substring(unwindIdx + "unwind label %".Length);
                    var us = u.IndexOf(' ');
                    unwindLabel = us > 0 ? u.Substring(0, us) : u;
                }
            }
            return new LoweredInvokeInstruction(result, "void", trimmed, [], normalLabel, unwindLabel);
        }

        if (trimmed.StartsWith("landingpad ", StringComparison.Ordinal))
        {
            var isCleanup = trimmed.Contains("cleanup", StringComparison.Ordinal);
            return new LoweredLandingPadInstruction(result, "i8*", trimmed, isCleanup);
        }

        if (trimmed.StartsWith("fence ", StringComparison.Ordinal))
        {
            var orderingText = trimmed.Substring("fence ".Length).Trim();
            string? scope = null;
            const string scopeMarker = "syncscope(";
            var scopeIdx = orderingText.IndexOf(scopeMarker, StringComparison.Ordinal);
            if (scopeIdx >= 0)
            {
                var afterScope = orderingText.Substring(scopeIdx + scopeMarker.Length);
                var close = afterScope.IndexOf(')');
                if (close > 0) { scope = afterScope.Substring(0, close); }
            }
            return new LoweredFenceInstruction(orderingText, scope);
        }

        // Volatile load: "%r = load volatile <ty>, ptr <ptr>"
        if (result is not null && trimmed.StartsWith("load volatile ", StringComparison.Ordinal))
        {
            var volatileLoadMatch = VolatileLoadInstructionRegex().Match(trimmed);
            if (volatileLoadMatch.Success)
            {
                return new LoweredVolatileLoadInstruction(
                    result,
                    NormalizeType(volatileLoadMatch.Groups["type"].Value),
                    NormalizeValue(volatileLoadMatch.Groups["source"].Value));
            }

            return new LoweredVolatileLoadInstruction(result, "?", trimmed);
        }

        // Volatile store: "store volatile <ty> <val>, <ty>* <ptr>"
        if (trimmed.StartsWith("store volatile ", StringComparison.Ordinal))
        {
            return new LoweredVolatileStoreInstruction("?", "?", trimmed);
        }

        return null;
    }

    private static List<LoweredArgument> ParseArguments(string argumentText)
    {
        if (string.IsNullOrWhiteSpace(argumentText))
        {
            return [];
        }

        var arguments = new List<LoweredArgument>();
        var remaining = argumentText.Trim();

        while (!string.IsNullOrEmpty(remaining))
        {
            var separatorIndex = FindTopLevelComma(remaining);
            string segment;
            if (separatorIndex < 0)
            {
                segment = remaining;
                remaining = string.Empty;
            }
            else
            {
                segment = remaining[..separatorIndex];
                remaining = remaining[(separatorIndex + 1)..].TrimStart();
            }

            var argument = ParseArgument(segment);
            if (argument is not null)
            {
                arguments.Add(argument);
            }
        }

        return arguments;
    }

    private static bool TryParseBinaryInstruction(string line, out LoweredBinaryInstruction instruction)
    {
        instruction = null!;

        var equalsIndex = line.IndexOf('=');
        if (equalsIndex <= 1)
        {
            return false;
        }

        var resultText = line[..equalsIndex].Trim();
        if (!resultText.StartsWith("%", StringComparison.Ordinal))
        {
            return false;
        }

        var remainder = line[(equalsIndex + 1)..].TrimStart();
        var operationSeparator = remainder.IndexOf(' ');
        if (operationSeparator <= 0)
        {
            return false;
        }

        var operation = remainder[..operationSeparator];
        if (!IsSupportedBinaryOperation(operation))
        {
            return false;
        }

        remainder = remainder[(operationSeparator + 1)..].TrimStart();
        while (!string.IsNullOrEmpty(remainder))
        {
            if (TryReadTypePrefix(remainder, out var type, out var operandsText))
            {
                operandsText = StripTrailingInstructionMetadata(operandsText);
                var commaIndex = FindTopLevelComma(operandsText);
                if (commaIndex <= 0 || commaIndex == operandsText.Length - 1)
                {
                    return false;
                }

                instruction = new LoweredBinaryInstruction(
                    NormalizeResultName(resultText),
                    operation,
                    NormalizeType(type),
                    NormalizeValue(operandsText[..commaIndex]),
                    NormalizeValue(operandsText[(commaIndex + 1)..]));
                return true;
            }

            var nextTokenSeparator = remainder.IndexOf(' ');
            if (nextTokenSeparator < 0)
            {
                return false;
            }

            remainder = remainder[(nextTokenSeparator + 1)..].TrimStart();
        }

        return false;
    }

    private static bool TryParseSelectInstruction(string line, out LoweredSelectInstruction instruction)
    {
        instruction = null!;

        const string marker = "= select i1 ";
        var markerIndex = line.IndexOf(marker, StringComparison.Ordinal);
        if (markerIndex <= 1)
        {
            return false;
        }

        var resultText = line[..markerIndex].Trim();
        if (resultText.Length == 0 || resultText[0] != '%')
        {
            return false;
        }

        var remainder = line[(markerIndex + marker.Length)..].TrimStart();
        var conditionSeparator = FindTopLevelComma(remainder);
        if (conditionSeparator <= 0 || conditionSeparator == remainder.Length - 1)
        {
            return false;
        }

        var condition = remainder[..conditionSeparator];
        remainder = remainder[(conditionSeparator + 1)..].TrimStart();

        if (!TryReadTypePrefix(remainder, out var valueType, out var valueRemainder)
            || string.IsNullOrWhiteSpace(valueRemainder))
        {
            return false;
        }

        var trueValueSeparator = FindTopLevelComma(valueRemainder);
        if (trueValueSeparator <= 0 || trueValueSeparator == valueRemainder.Length - 1)
        {
            return false;
        }

        var trueValue = valueRemainder[..trueValueSeparator];
        var falseSegment = valueRemainder[(trueValueSeparator + 1)..].TrimStart();
        if (!TryReadTypePrefix(falseSegment, out _, out var falseValue)
            || string.IsNullOrWhiteSpace(falseValue))
        {
            return false;
        }

        instruction = new LoweredSelectInstruction(
            NormalizeResultName(resultText),
            NormalizeValue(StripTrailingInstructionMetadata(condition)),
            NormalizeType(valueType),
            NormalizeValue(StripTrailingInstructionMetadata(trueValue)),
            NormalizeValue(StripTrailingInstructionMetadata(falseValue)));
        return true;
    }

    private static bool IsSupportedBinaryOperation(string operation)
    {
        return operation is "add" or "sub" or "mul" or "sdiv" or "udiv" or "srem" or "urem" or "fadd" or "fsub" or "fmul" or "fdiv" or "frem" or "and" or "or" or "xor" or "shl" or "lshr" or "ashr";
    }

    private static bool TryParseStoreInstruction(string line, out LoweredStoreInstruction instruction)
    {
        instruction = default!;
        var trimmed = line.Trim();
        if (!trimmed.StartsWith("store ", StringComparison.Ordinal))
        {
            return false;
        }

        var remainder = trimmed["store ".Length..].TrimStart();
        if (remainder.StartsWith("atomic ", StringComparison.Ordinal))
        {
            remainder = remainder["atomic ".Length..].TrimStart();
        }

        if (!TryReadTypePrefix(remainder, out var type, out var valueAndDestination))
        {
            return false;
        }

        var valueSeparator = FindTopLevelComma(valueAndDestination);
        if (valueSeparator < 0)
        {
            return false;
        }

        var value = valueAndDestination[..valueSeparator].Trim();
        var destinationSegment = valueAndDestination[(valueSeparator + 1)..].TrimStart();
        if (!destinationSegment.StartsWith("ptr ", StringComparison.Ordinal))
        {
            return false;
        }

        var destination = ReadPointerOperand(destinationSegment["ptr ".Length..]);
        if (string.IsNullOrWhiteSpace(destination))
        {
            return false;
        }

        instruction = new LoweredStoreInstruction(
            NormalizeType(type),
            NormalizeValue(StripTrailingInstructionMetadata(value)),
            NormalizeValue(StripTrailingInstructionMetadata(destination)));
        return true;
    }

    private static string ReadPointerOperand(string text)
    {
        var trimmed = text.TrimStart();
        var commaIndex = FindTopLevelComma(trimmed);
        var operand = commaIndex >= 0 ? trimmed[..commaIndex].TrimEnd() : trimmed;

        if (operand.StartsWith("getelementptr", StringComparison.Ordinal)
            || operand.StartsWith("inttoptr", StringComparison.Ordinal)
            || operand.StartsWith("bitcast", StringComparison.Ordinal)
            || operand.StartsWith("addrspacecast", StringComparison.Ordinal))
        {
            var openParenIndex = operand.IndexOf('(');
            if (openParenIndex >= 0 && TryFindMatchingParenthesis(operand, openParenIndex, out var closeParenIndex))
            {
                return operand[..(closeParenIndex + 1)].TrimEnd();
            }
        }

        return ReadTopLevelToken(operand).Trim();
    }

    private static bool TryParseCallInstruction(string line, out LoweredCallInstruction instruction)
    {
        instruction = null!;

        var remaining = line.Trim();
        string? result = null;

        var equalsIndex = remaining.IndexOf('=');
        if (equalsIndex > 0)
        {
            result = NormalizeResultName(remaining[..equalsIndex].Trim());
            remaining = remaining[(equalsIndex + 1)..].TrimStart();
        }

        if (!TrySkipToCallKeyword(remaining, out remaining))
        {
            return false;
        }

        while (!string.IsNullOrEmpty(remaining))
        {
            if (TryReadTypePrefix(remaining, out var returnType, out var callRemainder))
            {
                if (!TryReadCallTarget(callRemainder, out var callee, out var argumentsText))
                {
                    return false;
                }

                instruction = new LoweredCallInstruction(
                    result,
                    NormalizeType(returnType),
                    NormalizeFunctionName(callee),
                    ParseArguments(argumentsText));
                return true;
            }

            var token = ReadTopLevelToken(remaining);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            remaining = remaining[token.Length..].TrimStart();
        }

        return false;
    }

    private static bool TrySkipToCallKeyword(string text, out string remainder)
    {
        remainder = text.TrimStart();

        while (!string.IsNullOrEmpty(remainder))
        {
            var token = ReadTopLevelToken(remainder);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            remainder = remainder[token.Length..].TrimStart();
            if (string.Equals(token, "call", StringComparison.Ordinal)
                || string.Equals(token, "invoke", StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryReadCallTarget(string text, out string callee, out string argumentsText)
    {
        callee = string.Empty;
        argumentsText = string.Empty;

        var trimmed = text.TrimStart();
        var openParenIndex = trimmed.IndexOf('(');
        if (openParenIndex <= 0 || !TryFindMatchingParenthesis(trimmed, openParenIndex, out var closeParenIndex))
        {
            return false;
        }

        callee = trimmed[..openParenIndex].Trim();
        argumentsText = trimmed[(openParenIndex + 1)..closeParenIndex];
        return !string.IsNullOrWhiteSpace(callee);
    }

    private static bool TryParseFunctionHeader(string line, out string functionName, out string returnType, out string? returnExtension, out List<LoweredParameter> parameters)
    {
        functionName = string.Empty;
        returnType = string.Empty;
        returnExtension = null;
        parameters = [];

        const string marker = "define ";
        if (!line.StartsWith(marker, StringComparison.Ordinal))
        {
            return false;
        }

        var nameMarker = line.IndexOf('@');
        if (nameMarker < 0)
        {
            return false;
        }

        var openParenIndex = line.IndexOf('(', nameMarker);
        if (openParenIndex < 0 || !TryFindMatchingParenthesis(line, openParenIndex, out var closeParenIndex))
        {
            return false;
        }

        var signaturePrefix = line[marker.Length..nameMarker].Trim();
        var rawName = line[nameMarker..openParenIndex].Trim();
        if (string.IsNullOrWhiteSpace(signaturePrefix) || string.IsNullOrWhiteSpace(rawName))
        {
            return false;
        }

        functionName = NormalizeFunctionName(rawName);
        returnType = NormalizeType(signaturePrefix);
        returnExtension = FindIntegerExtensionAttribute(signaturePrefix);
        parameters = ParseParameters(line[(openParenIndex + 1)..closeParenIndex]);
        return true;
    }

    private static bool TryReadTypePrefix(string text, out string type, out string remainder)
    {
        var trimmed = text.TrimStart();

        if (TryReadDelimitedType(trimmed, '<', '>', out type, out remainder)
            || TryReadDelimitedType(trimmed, '{', '}', out type, out remainder)
            || TryReadDelimitedType(trimmed, '[', ']', out type, out remainder)
            || TryReadKeywordType(trimmed, "ptr", out type, out remainder)
            || TryReadKeywordType(trimmed, "float", out type, out remainder)
            || TryReadKeywordType(trimmed, "double", out type, out remainder)
            || TryReadIntegerType(trimmed, out type, out remainder)
            || TryReadKeywordType(trimmed, "void", out type, out remainder))
        {
            return true;
        }

        type = string.Empty;
        remainder = string.Empty;
        return false;
    }

    private static bool TryReadDelimitedType(string text, char open, char close, out string type, out string remainder)
    {
        type = string.Empty;
        remainder = string.Empty;

        if (string.IsNullOrEmpty(text) || text[0] != open)
        {
            return false;
        }

        var depth = 0;
        for (var index = 0; index < text.Length; index++)
        {
            if (text[index] == open)
            {
                depth++;
            }
            else if (text[index] == close)
            {
                depth--;
                if (depth == 0)
                {
                    type = text[..(index + 1)];
                    remainder = text[(index + 1)..].TrimStart();
                    return true;
                }
            }
        }

        return false;
    }

    private static bool TryReadKeywordType(string text, string keyword, out string type, out string remainder)
    {
        type = string.Empty;
        remainder = string.Empty;

        if (!text.StartsWith(keyword, StringComparison.Ordinal))
        {
            return false;
        }

        var length = keyword.Length;
        if (string.Equals(keyword, "ptr", StringComparison.Ordinal)
            && text.Length > length
            && char.IsWhiteSpace(text[length])
            && text[(length + 1)..].StartsWith("addrspace(", StringComparison.Ordinal))
        {
            var closingIndex = text.IndexOf(')', length + 1);
            if (closingIndex < 0)
            {
                return false;
            }

            length = closingIndex + 1;
        }

        if (text.Length > length && !char.IsWhiteSpace(text[length]))
        {
            return false;
        }

        type = text[..length];
        remainder = text[length..].TrimStart();
        return true;
    }

    private static bool TryReadIntegerType(string text, out string type, out string remainder)
    {
        type = string.Empty;
        remainder = string.Empty;

        if (text.Length < 2 || text[0] != 'i' || !char.IsDigit(text[1]))
        {
            return false;
        }

        var length = 2;
        while (length < text.Length && char.IsDigit(text[length]))
        {
            length++;
        }

        if (text.Length > length && !char.IsWhiteSpace(text[length]))
        {
            return false;
        }

        type = text[..length];
        remainder = text[length..].TrimStart();
        return true;
    }

    private static int FindTopLevelComma(string text)
    {
        var angleDepth = 0;
        var bracketDepth = 0;
        var parenthesisDepth = 0;

        for (var index = 0; index < text.Length; index++)
        {
            switch (text[index])
            {
                case '<':
                    angleDepth++;
                    break;
                case '>':
                    angleDepth--;
                    break;
                case '[':
                    bracketDepth++;
                    break;
                case ']':
                    bracketDepth--;
                    break;
                case '(':
                    parenthesisDepth++;
                    break;
                case ')':
                    parenthesisDepth--;
                    break;
                case ',':
                    if (angleDepth == 0 && bracketDepth == 0 && parenthesisDepth == 0)
                    {
                        return index;
                    }

                    break;
            }
        }

        return -1;
    }

    private static bool TryFindMatchingParenthesis(string text, int openParenIndex, out int closeParenIndex)
    {
        closeParenIndex = -1;
        if (openParenIndex < 0 || openParenIndex >= text.Length || text[openParenIndex] != '(')
        {
            return false;
        }

        var depth = 1;
        for (var index = openParenIndex + 1; index < text.Length; index++)
        {
            if (text[index] == '(')
            {
                depth++;
            }
            else if (text[index] == ')')
            {
                depth--;
                if (depth == 0)
                {
                    closeParenIndex = index;
                    return true;
                }
            }
        }

        return false;
    }

    private static bool TryReadBracketedSegment(string text, int startIndex, out int segmentStart, out int segmentEnd)
    {
        segmentStart = -1;
        segmentEnd = -1;

        for (var index = startIndex; index < text.Length; index++)
        {
            if (text[index] != '[')
            {
                continue;
            }

            segmentStart = index;
            var depth = 1;
            for (var innerIndex = index + 1; innerIndex < text.Length; innerIndex++)
            {
                if (text[innerIndex] == '[')
                {
                    depth++;
                }
                else if (text[innerIndex] == ']')
                {
                    depth--;
                    if (depth == 0)
                    {
                        segmentEnd = innerIndex;
                        return true;
                    }
                }
            }

            return false;
        }

        return false;
    }

    private static string FormatInstruction(LoweredInstruction instruction)
    {
        return instruction switch
        {
            LoweredBinaryInstruction binary => $"{binary.Result} = {binary.Operation} {binary.Type} {binary.Left}, {binary.Right}",
            LoweredCallInstruction call when call.Result is null => $"call {call.ReturnType} {call.Callee}({FormatArguments(call.Arguments)})",
            LoweredCallInstruction call => $"{call.Result} = call {call.ReturnType} {call.Callee}({FormatArguments(call.Arguments)})",
            LoweredReturnInstruction ret => $"ret {ret.Type} {ret.Value}",
            LoweredCompareInstruction compare => $"{compare.Result} = icmp {compare.Predicate} {compare.Type} {compare.Left}, {compare.Right}",
            LoweredBitcastInstruction bitcast => $"{bitcast.Result} = bitcast {bitcast.FromType} {bitcast.Value} to {bitcast.ToType}",
            LoweredConditionalBranchInstruction branch => $"br {branch.Condition} -> {branch.TrueTarget}, {branch.FalseTarget}",
            LoweredJumpInstruction jump => $"br -> {jump.Target}",
            LoweredLoadInstruction load => $"{load.Result} = load {load.Type} {load.Source}",
            LoweredAllocaInstruction alloca => $"{alloca.Result} = alloca {alloca.Type}",
            LoweredGetElementPointerInstruction gep => $"{gep.Result} = gep {gep.ElementType} {gep.Base}[{gep.Index}]",
            LoweredTruncateInstruction trunc => $"{trunc.Result} = trunc {trunc.FromType} {trunc.Value} to {trunc.ToType}",
            LoweredZeroExtendInstruction zext => $"{zext.Result} = zext {zext.FromType} {zext.Value} to {zext.ToType}",
            LoweredSignExtendInstruction sext => $"{sext.Result} = sext {sext.FromType} {sext.Value} to {sext.ToType}",
            LoweredPtrToIntInstruction p2i => $"{p2i.Result} = ptrtoint ptr {p2i.Value} to {p2i.ToType}",
            LoweredIntToPtrInstruction i2p => $"{i2p.Result} = inttoptr to ptr {i2p.Value}",
            LoweredFreezeInstruction freeze => $"{freeze.Result} = freeze {freeze.Type} {freeze.Value}",
            LoweredSelectInstruction select => $"{select.Result} = select i1 {select.Condition}, {select.ValueType} {select.TrueValue}, {select.ValueType} {select.FalseValue}",
            LoweredPhiInstruction phi => $"{phi.Result} = phi {phi.Type} {FormatPhiIncoming(phi.Incoming)}",
            LoweredUnreachableInstruction => "unreachable",
            LoweredStoreInstruction store => $"store {store.Type} {store.Value} -> {store.Destination}",
            LoweredExtractValueInstruction ev => $"{ev.Result} = extractvalue {ev.AggregateType} {ev.Source}, {ev.Index}",
            LoweredInsertValueInstruction iv => $"{iv.Result} = insertvalue {iv.AggregateType} {iv.Base}, {iv.Value}, {iv.Index}",
            LoweredAtomicRmwInstruction rmw => $"{rmw.Result} = atomicrmw {rmw.Operation} ptr {rmw.Pointer}, {rmw.ValueType} {rmw.Value}",
            LoweredCmpxchgInstruction cx => $"{cx.Result} = cmpxchg ptr {cx.Pointer}, {cx.ValueType} {cx.CompareValue}, {cx.ValueType} {cx.NewValue}",
            LoweredInvokeInstruction invoke => FormatInvoke(invoke),
            LoweredLandingPadInstruction landingPad => FormatLandingPad(landingPad),
            LoweredFenceInstruction fence => $"fence {fence.Ordering}",
            LoweredVolatileLoadInstruction load => $"{load.Result} = load volatile {load.ValueType} {load.Pointer}",
            LoweredVolatileStoreInstruction store => $"store volatile {store.ValueType} {store.Value} -> {store.Pointer}",
            LoweredSwitchInstruction sw => $"switch {sw.ValueType} {sw.Value} -> {sw.DefaultLabel} [{FormatSwitchCases(sw.Cases)}]",
            LoweredRawInstruction raw => raw.Text,
            _ => throw new InvalidOperationException($"Unsupported lowered instruction type: {instruction.GetType().Name}")
        };
    }

    private static string FormatInvoke(LoweredInvokeInstruction invoke)
    {
        if (invoke.ReturnType == "void"
            && invoke.Arguments.Count == 0
            && invoke.Callee.StartsWith("invoke ", StringComparison.Ordinal))
        {
            return invoke.Result is null ? invoke.Callee : $"{invoke.Result} = {invoke.Callee}";
        }

        var prefix = invoke.Result is null ? "invoke" : $"{invoke.Result} = invoke";
        return $"{prefix} {invoke.ReturnType} {invoke.Callee}({FormatArguments(invoke.Arguments)}) to {invoke.NormalLabel} unwind {invoke.UnwindLabel}";
    }

    private static string FormatLandingPad(LoweredLandingPadInstruction landingPad)
    {
        var prefix = landingPad.Result is null ? "landingpad" : $"{landingPad.Result} = landingpad";
        var cleanup = landingPad.IsCleanup ? " cleanup" : string.Empty;
        return $"{prefix} {landingPad.ResultType}{cleanup} {landingPad.ClausesText}".TrimEnd();
    }

    private static string FormatArguments(IReadOnlyList<LoweredArgument> arguments)
    {
        return string.Join(", ", arguments.Select(argument => $"{argument.Type} {argument.Value}"));
    }

    private static string FormatSwitchCases(IReadOnlyList<LoweredSwitchCase> cases)
    {
        return string.Join(", ", cases.Select(@case => $"{@case.Value}: {@case.Target}"));
    }

    private static long ParseSwitchCaseValue(string valueText, string typeText)
    {
        if (long.TryParse(valueText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var signedValue))
        {
            return signedValue;
        }

        if (!typeText.StartsWith('i') || !int.TryParse(typeText[1..], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var bitWidth))
        {
            throw new FormatException($"Switch case type '{typeText}' is not an integer type.");
        }

        if (bitWidth is <= 0 or > 64
            || !ulong.TryParse(valueText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var unsignedValue))
        {
            throw new OverflowException($"Switch case value '{valueText}' cannot be represented as {typeText}.");
        }

        if (bitWidth == 64)
        {
            return unchecked((long)unsignedValue);
        }

        var signBit = 1UL << (bitWidth - 1);
        var mask = (1UL << bitWidth) - 1UL;
        var truncated = unsignedValue & mask;
        return (truncated & signBit) == 0
            ? (long)truncated
            : unchecked((long)(truncated | ~mask));
    }

    private static bool CanCoalesceSwitchType(string typeText)
    {
        return typeText.StartsWith('i')
            && int.TryParse(typeText[1..], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var bitWidth)
            && bitWidth is > 0 and <= 64;
    }

    private static string FormatPhiIncoming(IReadOnlyList<LoweredPhiIncoming> incoming)
    {
        return string.Join(", ", incoming.Select(edge => $"[ {edge.Value}, %{edge.SourceBlock} ]"));
    }

    private static List<LoweredPhiIncoming> ParsePhiIncoming(string incomingText)
    {
        var incoming = new List<LoweredPhiIncoming>();
        var index = 0;

        while (TryReadBracketedSegment(incomingText, index, out var segmentStart, out var segmentEnd))
        {
            var segment = incomingText[(segmentStart + 1)..segmentEnd];
            var separatorIndex = FindTopLevelComma(segment);
            if (separatorIndex > 0)
            {
                var value = segment[..separatorIndex];
                var source = segment[(separatorIndex + 1)..].Trim();
                if (source.StartsWith("%", StringComparison.Ordinal))
                {
                    source = source[1..];
                }

                incoming.Add(new LoweredPhiIncoming(
                    NormalizeValue(value),
                    NormalizeBlockName(source)));
            }

            index = segmentEnd + 1;
        }

        return incoming;
    }

    private static string NormalizeValue(string value)
    {
        var trimmed = value.Trim();

        if (trimmed.StartsWith('%') || trimmed.StartsWith('@'))
        {
            return NormalizeIdentifier(trimmed[1..]);
        }

        return trimmed;
    }

    private static string StripTrailingInstructionMetadata(string value)
    {
        var remaining = value.Trim();

        while (true)
        {
            var commaIndex = FindLastTopLevelComma(remaining);
            if (commaIndex < 0)
            {
                return remaining;
            }

            var suffix = remaining[(commaIndex + 1)..].TrimStart();
            if (!suffix.StartsWith('!'))
            {
                return remaining;
            }

            remaining = remaining[..commaIndex].TrimEnd();
        }
    }

    private static int FindLastTopLevelComma(string text)
    {
        var angleDepth = 0;
        var bracketDepth = 0;
        var parenthesisDepth = 0;
        var lastIndex = -1;

        for (var index = 0; index < text.Length; index++)
        {
            switch (text[index])
            {
                case '<': angleDepth++; break;
                case '>': angleDepth--; break;
                case '[': bracketDepth++; break;
                case ']': bracketDepth--; break;
                case '(': parenthesisDepth++; break;
                case ')': parenthesisDepth--; break;
                case ',':
                    if (angleDepth == 0 && bracketDepth == 0 && parenthesisDepth == 0)
                    {
                        lastIndex = index;
                    }
                    break;
            }
        }

        return lastIndex;
    }

    private static string NormalizeResultName(string value)
    {
        return NormalizeIdentifier(value.Trim().TrimStart('%').TrimStart('@'));
    }

    private static string NormalizeBlockName(string value)
    {
        var trimmed = value.Trim().TrimStart('%');
        if (trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"')
        {
            trimmed = trimmed[1..^1];
        }

        return trimmed;
    }

    private static string NormalizeIdentifier(string value)
    {
        if (value.Length > 0 && value.All(char.IsDigit))
        {
            return $"tmp.{value}";
        }

        return IsGeneratedTemporaryName(value)
            ? $"named_{value}"
            : value;
    }

    private static bool IsGeneratedTemporaryName(string value)
    {
        const string prefix = "tmp.";
        return value.StartsWith(prefix, StringComparison.Ordinal)
            && value.Length > prefix.Length
            && value[prefix.Length..].All(char.IsDigit);
    }

    private static string NormalizeFunctionName(string value)
    {
        var trimmed = value.Trim().TrimStart('@');
        if (trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"')
        {
            trimmed = trimmed[1..^1];
        }

        return NormalizeIdentifier(trimmed);
    }

    private static string NormalizeType(string typeText)
    {
        var match = CoreTypeRegex().Match(typeText.Trim());
        return match.Success
            ? match.Value
            : typeText.Trim();
    }

    private static LoweredParameter? ParseParameter(string parameter)
    {
        var trimmed = parameter.Trim();
        if (!TryReadTypePrefix(trimmed, out var type, out var remainder)
            || string.IsNullOrWhiteSpace(remainder))
        {
            return null;
        }

        var extension = FindIntegerExtensionAttribute(remainder);
        remainder = StripLeadingArgumentAttributes(remainder);
        if (string.IsNullOrWhiteSpace(remainder))
        {
            return null;
        }

        var parameterName = ReadTopLevelToken(remainder);
        if (string.IsNullOrWhiteSpace(parameterName))
        {
            return null;
        }

        return new LoweredParameter(
            NormalizeValue(parameterName),
            NormalizeType(type),
            extension);
    }

    private static LoweredArgument? ParseArgument(string argument)
    {
        var trimmed = argument.Trim();
        if (!TryReadTypePrefix(trimmed, out var type, out var remainder)
            || string.IsNullOrWhiteSpace(remainder))
        {
            return null;
        }

        remainder = StripLeadingArgumentAttributes(remainder);
        if (string.IsNullOrWhiteSpace(remainder))
        {
            return null;
        }

        return new LoweredArgument(
            NormalizeType(type),
            NormalizeValue(remainder));
    }

    private static string? FindIntegerExtensionAttribute(string text)
    {
        var remaining = text.TrimStart();

        while (!string.IsNullOrEmpty(remaining))
        {
            var token = ReadTopLevelToken(remaining);
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            if (string.Equals(token, "signext", StringComparison.Ordinal)
                || string.Equals(token, "zeroext", StringComparison.Ordinal))
            {
                return token;
            }

            remaining = remaining[token.Length..].TrimStart();
        }

        return null;
    }

    private static string StripLeadingArgumentAttributes(string text)
    {
        var remaining = text.TrimStart();

        while (!string.IsNullOrEmpty(remaining) && !LooksLikeArgumentValueStart(remaining))
        {
            var token = ReadTopLevelToken(remaining);
            if (string.IsNullOrEmpty(token))
            {
                break;
            }

            remaining = remaining[token.Length..].TrimStart();

            if (string.Equals(token, "align", StringComparison.Ordinal))
            {
                var alignment = ReadTopLevelToken(remaining);
                if (string.IsNullOrEmpty(alignment))
                {
                    return string.Empty;
                }

                remaining = remaining[alignment.Length..].TrimStart();
                continue;
            }

            if (!IsLeadingArgumentAttribute(token))
            {
                break;
            }
        }

        return remaining;
    }

    private static bool LooksLikeArgumentValueStart(string text)
    {
        var trimmed = text.TrimStart();
        if (string.IsNullOrEmpty(trimmed))
        {
            return false;
        }

        var first = trimmed[0];
        if (first is '%' or '@' or '-' or '<' or '[' || char.IsDigit(first))
        {
            return true;
        }

        return trimmed.StartsWith("null", StringComparison.Ordinal)
            || trimmed.StartsWith("undef", StringComparison.Ordinal)
            || trimmed.StartsWith("poison", StringComparison.Ordinal)
            || trimmed.StartsWith("zeroinitializer", StringComparison.Ordinal)
            || trimmed.StartsWith("true", StringComparison.Ordinal)
            || trimmed.StartsWith("false", StringComparison.Ordinal)
            || trimmed.StartsWith("none", StringComparison.Ordinal)
            || trimmed.StartsWith("blockaddress(", StringComparison.Ordinal)
            || trimmed.StartsWith("splat (", StringComparison.Ordinal)
            || trimmed.StartsWith("c\"", StringComparison.Ordinal);
    }

    private static bool IsLeadingArgumentAttribute(string token)
    {
        return token is "align"
            or "allocptr"
            or "allocalign"
            or "byref"
            or "captures"
            or "dead_on_return"
            or "dead_on_unwind"
            or "immarg"
            or "inalloca"
            or "inreg"
            or "nest"
            or "noalias"
            or "nocapture"
            or "nofree"
            or "nonnull"
            or "noundef"
            or "preallocated"
            or "readnone"
            or "readonly"
            or "returned"
            or "signext"
            or "swiftasync"
            or "swiftself"
            or "swifterror"
            or "sret"
            or "writable"
            or "writeonly"
            or "zeroext"
            || token.StartsWith("alignstack(", StringComparison.Ordinal)
            || token.StartsWith("byref(", StringComparison.Ordinal)
            || token.StartsWith("byval(", StringComparison.Ordinal)
            || token.StartsWith("captures(", StringComparison.Ordinal)
            || token.StartsWith("dereferenceable(", StringComparison.Ordinal)
            || token.StartsWith("dereferenceable_or_null(", StringComparison.Ordinal)
            || token.StartsWith("elementtype(", StringComparison.Ordinal)
            || token.StartsWith("inalloca(", StringComparison.Ordinal)
            || token.StartsWith("initializes(", StringComparison.Ordinal)
            || token.StartsWith("preallocated(", StringComparison.Ordinal)
            || token.StartsWith("range(", StringComparison.Ordinal)
            || token.StartsWith("sret(", StringComparison.Ordinal);
    }

    private static string ReadTopLevelToken(string text)
    {
        var angleDepth = 0;
        var bracketDepth = 0;
        var parenDepth = 0;

        for (var index = 0; index < text.Length; index++)
        {
            switch (text[index])
            {
                case '<':
                    angleDepth++;
                    break;
                case '>':
                    angleDepth = Math.Max(0, angleDepth - 1);
                    break;
                case '[':
                    bracketDepth++;
                    break;
                case ']':
                    bracketDepth = Math.Max(0, bracketDepth - 1);
                    break;
                case '(':
                    parenDepth++;
                    break;
                case ')':
                    parenDepth = Math.Max(0, parenDepth - 1);
                    break;
                default:
                    if (char.IsWhiteSpace(text[index]) && angleDepth == 0 && bracketDepth == 0 && parenDepth == 0)
                    {
                        return text[..index];
                    }

                    break;
            }
        }

        return text;
    }

    private static LoweredFunction CreateAliasFunction(Match aliasMatch)
    {
        var functionName = aliasMatch.Groups["name"].Value;
        var targetName = aliasMatch.Groups["target"].Value;
        var returnType = NormalizeType(aliasMatch.Groups["returnType"].Value);
        var parameterTypes = ParseAliasParameterTypes(aliasMatch.Groups["parameters"].Value);
        var parameters = parameterTypes
            .Select((parameterType, index) => new LoweredParameter($"arg{index}", parameterType))
            .ToList();
        var arguments = parameters
            .Select(parameter => new LoweredArgument(parameter.Type, parameter.Name))
            .ToList();

        var instructions = new List<LoweredInstruction>();
        if (string.Equals(returnType, "void", StringComparison.Ordinal))
        {
            instructions.Add(new LoweredCallInstruction(null, returnType, targetName, arguments));
            instructions.Add(new LoweredReturnInstruction("void", "void"));
        }
        else
        {
            instructions.Add(new LoweredCallInstruction("_0", returnType, targetName, arguments));
            instructions.Add(new LoweredReturnInstruction(returnType, "_0"));
        }

        return new LoweredFunction(functionName, returnType, parameters, [new LoweredBlock("entry", instructions)]);
    }

    private static List<string> ParseAliasParameterTypes(string parameterText)
    {
        if (string.IsNullOrWhiteSpace(parameterText))
        {
            return [];
        }

        return parameterText
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(NormalizeType)
            .ToList();
    }

    private static IEnumerable<string> ReadLines(string text)
    {
        using var reader = new StringReader(text);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }

    private static List<byte> ParseConstantByteArray(string escapedBytes)
    {
        var bytes = new List<byte>();
        for (var index = 0; index < escapedBytes.Length;)
        {
            if (escapedBytes[index] == '\\' && index + 2 < escapedBytes.Length)
            {
                if (Uri.IsHexDigit(escapedBytes[index + 1]) && Uri.IsHexDigit(escapedBytes[index + 2]))
                {
                    bytes.Add(Convert.ToByte(escapedBytes.Substring(index + 1, 2), 16));
                    index += 3;
                    continue;
                }

                if (escapedBytes[index + 1] is '\\' or '"')
                {
                    bytes.Add((byte)escapedBytes[index + 1]);
                    index += 2;
                    continue;
                }
            }

            bytes.Add((byte)escapedBytes[index]);
            index++;
        }

        return bytes;
    }

    private static List<byte>? ParseConstantIntegerArray(string elementType, string valuesText)
    {
        var bitWidth = elementType.StartsWith('i') ? int.Parse(elementType[1..]) : 0;
        if (bitWidth is not (8 or 16 or 32 or 64))
        {
            return null;
        }

        var byteWidth = bitWidth / 8;
        var bytes = new List<byte>();
        var valuePattern = new System.Text.RegularExpressions.Regex($@"i{bitWidth}\s+(-?\d+)");
        foreach (System.Text.RegularExpressions.Match match in valuePattern.Matches(valuesText))
        {
            var value = long.Parse(match.Groups[1].Value);
            for (var i = 0; i < byteWidth; i++)
            {
                bytes.Add((byte)(value & 0xFF));
                value >>= 8;
            }
        }

        return bytes.Count > 0 ? bytes : null;
    }

    private static List<byte>? ParseScalarIntegerBytes(int bitWidth, string valueText)
    {
        if (bitWidth is not (8 or 16 or 32 or 64))
        {
            return null;
        }

        var byteWidth = bitWidth / 8;
        var value = long.Parse(valueText);
        var bytes = new List<byte>(byteWidth);
        for (var i = 0; i < byteWidth; i++)
        {
            bytes.Add((byte)(value & 0xFF));
            value >>= 8;
        }
        return bytes;
    }

    private static bool TryParseZeroableAggregateGlobal(string line, out LoweredGlobal global)
    {
        global = null!;

        var match = ZeroableAggregateGlobalRegex().Match(line);
        if (!match.Success)
        {
            return false;
        }

        var initializer = match.Groups["initializer"].Value;
        if (initializer.Contains("ptr", StringComparison.Ordinal))
        {
            return false;
        }

        var bytes = new List<byte>();
        foreach (Match fieldMatch in ZeroableAggregateFieldRegex().Matches(initializer))
        {
            if (fieldMatch.Groups["bytes"].Success)
            {
                bytes.AddRange(ParseConstantByteArray(fieldMatch.Groups["bytes"].Value));
                continue;
            }

            var size = int.Parse(fieldMatch.Groups["size"].Value);
            bytes.AddRange(new byte[size]);
        }

        if (bytes.Count == 0)
        {
            return false;
        }

        global = new LoweredGlobal(match.Groups["name"].Value, bytes);
        return true;
    }

    private static bool TryParsePointerRelocationGlobal(string line, out LoweredGlobal global)
    {
        global = null!;

        var match = PointerRelocationGlobalRegex().Match(line);
        if (!match.Success)
        {
            return false;
        }

        var bytes = ParseConstantByteArray(match.Groups["bytes"].Value).ToList();
        var relocations = new List<LoweredGlobalPointerRelocation>();
        var initializer = match.Groups["initializer"].Value;
        foreach (Match fieldMatch in PointerRelocationFieldRegex().Matches(initializer))
        {
            if (fieldMatch.Groups["bytes"].Success)
            {
                bytes.AddRange(ParseConstantByteArray(fieldMatch.Groups["bytes"].Value));
                continue;
            }

            if (fieldMatch.Groups["zero"].Success)
            {
                bytes.AddRange(new byte[int.Parse(fieldMatch.Groups["size"].Value)]);
                continue;
            }

            if (fieldMatch.Groups["target"].Success)
            {
                relocations.Add(new LoweredGlobalPointerRelocation(
                    bytes.Count,
                    NormalizeFunctionName(fieldMatch.Groups["target"].Value)));
                bytes.AddRange(new byte[IntPtr.Size]);
            }
        }

        if (relocations.Count == 0)
        {
            return false;
        }

        global = new LoweredGlobal(
            match.Groups["name"].Value,
            bytes,
            relocations);
        return true;
    }

    private static bool TryParseScalarPointerRelocationGlobal(string line, out LoweredGlobal global)
    {
        global = null!;

        var match = ScalarPointerRelocationGlobalRegex().Match(line);
        if (!match.Success)
        {
            return false;
        }

        global = new LoweredGlobal(
            match.Groups["name"].Value,
            new byte[IntPtr.Size],
            [new LoweredGlobalPointerRelocation(0, NormalizeFunctionName(match.Groups["target"].Value))]);
        return true;
    }

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:[^=]+\\s+)?alias\\s+(?<returnType>[^\\s(]+)\\s*\\((?<parameters>[^)]*)\\),\\s+ptr\\s+@(?<target>[^\\s,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex FunctionAliasRegex();

    [GeneratedRegex("^(?<name>%[^\\s=]+)\\s*=\\s*type\\s+(?<type>.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex NamedTypeDefinitionRegex();

    [GeneratedRegex("<[^>]+>|\\{[^}]+\\}|\\[[^\\]]+\\]|ptr(?:\\s+addrspace\\(\\d+\\))?|i\\d+|float|double|void", RegexOptions.CultureInvariant)]
    private static partial Regex CoreTypeRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?constant\\s+<\\{.*\\}>\\s+<\\{\\s*(?<initializer>.*)\\s*\\}>(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex PointerRelocationGlobalRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?(?:constant|global)\\s+ptr\\s+@(?<target>\"[^\"]+\"|[^\\s,]+)(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ScalarPointerRelocationGlobalRegex();

    [GeneratedRegex("ptr\\s+@(?<target>\"[^\"]+\"|[^\\s,}]+)|\\[(?<size>\\d+)\\s+x\\s+i8\\]\\s+(?:(?<zero>zeroinitializer|undef)|c\"(?<bytes>[^\"]*)\")", RegexOptions.CultureInvariant)]
    private static partial Regex PointerRelocationFieldRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?(?:constant|global)\\s+\\[(?<size>\\d+)\\s+x\\s+i8\\]\\s+c\"(?<bytes>[^\"]*)\"(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ConstantByteArrayGlobalRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?constant\\s+\\[\\d+\\s+x\\s+(?<elementType>i\\d+)\\]\\s+\\[(?<values>[^\\]]+)\\](?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ConstantIntegerArrayGlobalRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?(?:constant|global)\\s+\\[(?<size>\\d+)\\s+x\\s+i8\\]\\s+zeroinitializer(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ZeroInitByteArrayGlobalRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?(?:constant|global)\\s+(?<type>i(?<bits>\\d+))\\s+(?:0|zeroinitializer)(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ZeroInitScalarGlobalRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?(?:constant|global)\\s+i(?<bits>\\d+)\\s+(?<value>-?\\d+)(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ScalarIntegerGlobalRegex();

    [GeneratedRegex("^@(?<name>[^\\s=]+)\\s*=\\s*(?:.+?\\s+)?(?:constant|global)\\s+<\\{[^>]+\\}>\\s+<\\{\\s*(?<initializer>.*)\\s*\\}>(?:,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ZeroableAggregateGlobalRegex();

    [GeneratedRegex("\\[(?<size>\\d+)\\s+x\\s+i8\\]\\s+(?:(?<zero>zeroinitializer|undef)|c\"(?<bytes>[^\"]*)\")", RegexOptions.CultureInvariant)]
    private static partial Regex ZeroableAggregateFieldRegex();

    [GeneratedRegex("^(?<name>\"[^\"]+\"|[0-9]+|[A-Za-z$._][-A-Za-z$._0-9]*):", RegexOptions.CultureInvariant)]
    private static partial Regex BasicBlockRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*(?<op>add|sub|mul|sdiv|udiv|srem|urem|fadd|fsub|fmul|fdiv|frem|and|or|xor|shl|lshr|ashr)(?:\\s+[^\\s]+)*\\s+(?<type><[^>]+>|[^\\s]+)\\s+(?<left>[^,]+),\\s*(?<right>.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex BinaryInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*trunc(?:\\s+[^\\s]+)*\\s+(?<fromType><[^>]+>|[^\\s]+)\\s+(?<value>[^\\s]+)\\s+to\\s+(?<toType><[^>]+>|[^\\s,]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex TruncateInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*zext(?:\\s+[^\\s]+)*\\s+(?<fromType><[^>]+>|[^\\s]+)\\s+(?<value>[^\\s]+)\\s+to\\s+(?<toType><[^>]+>|[^\\s,]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ZeroExtendInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*sext(?:\\s+[^\\s]+)*\\s+(?<fromType><[^>]+>|[^\\s]+)\\s+(?<value>[^\\s]+)\\s+to\\s+(?<toType><[^>]+>|[^\\s,]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex SignExtendInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*bitcast\\s+(?<fromType><[^>]+>|[^\\s]+)\\s+(?<value>[^\\s]+)\\s+to\\s+(?<toType><[^>]+>|[^\\s,]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex BitcastInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*ptrtoint\\s+ptr\\s+(?<value>[^\\s]+)\\s+to\\s+(?<toType>[^\\s,]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex PtrToIntInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*inttoptr\\s+(?:[^\\s]+)\\s+(?<value>[^\\s]+)\\s+to\\s+ptr(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex IntToPtrInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*freeze\\s+(?<type><[^>]+>|[^\\s]+)\\s+(?<value>[^\\s]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex FreezeInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*(?:fptosi|fptoui|sitofp|uitofp|fpext|fptrunc)(?:\\s+[^\\s]+)*\\s+(?<fromType><[^>]+>|[^\\s]+)\\s+(?<value>[^\\s]+)\\s+to\\s+(?<toType><[^>]+>|[^\\s,]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex FloatConvertInstructionRegex();

    [GeneratedRegex("^(?<result>%?[^\\s=]+)\\s*=\\s*select\\s+i1\\s+(?<condition>[^,]+),\\s+(?<valueType><[^>]+>|[^\\s]+)\\s+(?<trueValue>[^,]+),\\s+(?:<[^>]+>|[^\\s]+)\\s+(?<falseValue>.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex SelectInstructionRegex();

    [GeneratedRegex("^(?<result>%?[^\\s=]+)\\s*=\\s*phi\\s+(?<type>\\{[^}]+\\}|<[^>]+>|[^\\s]+)\\s+(?<incoming>.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex PhiInstructionRegex();

    [GeneratedRegex("^unreachable$", RegexOptions.CultureInvariant)]
    private static partial Regex UnreachableInstructionRegex();

    [GeneratedRegex("^ret\\s+(?<type>\\{[^}]+\\}|<[^>]+>|[^\\s]+)\\s+(?<value>.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex ReturnInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*(?:icmp(?:\\s+samesign)?|fcmp)\\s+(?<predicate>[^\\s]+)\\s+(?<type><[^>]+>|[^\\s]+)\\s+(?<left>[^,]+),\\s*(?<right>.+)$", RegexOptions.CultureInvariant)]
    private static partial Regex CompareInstructionRegex();

    [GeneratedRegex("^br\\s+i1\\s+(?<condition>[^,]+),\\s+label\\s+%(?<trueLabel>[^,\\s]+),\\s+label\\s+%(?<falseLabel>[^,\\s]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ConditionalBranchInstructionRegex();

    [GeneratedRegex("^br\\s+label\\s+%(?<target>[^,\\s]+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex UnconditionalBranchInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*getelementptr(?:\\s+[A-Za-z0-9_]+)*\\s+(?<elementType>.+),\\s+ptr\\s+(?<base>[^,]+),\\s+i64\\s+(?<index>-?\\d+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex GetElementPointerInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*getelementptr(?:\\s+[A-Za-z0-9_]+)*\\s+(?<elementType>.+),\\s+ptr\\s+(?<base>[^,]+),\\s+i(?:32|64)\\s+(?<indexVar>%[^\\s,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex DynamicGetElementPointerInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*load\\s+(?:atomic\\s+)?(?<type>[^,]+),\\s+ptr\\s+getelementptr(?:\\s+[A-Za-z0-9_]+)*\\s*\\((?<elementType>.+),\\s+ptr\\s+@(?<source>[^,]+),\\s+i64\\s+(?<index>-?\\d+)\\).*$", RegexOptions.CultureInvariant)]
    private static partial Regex GlobalElementLoadInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*load\\s+(?:atomic\\s+)?(?<type>[^,]+),\\s+ptr\\s+(?<source>[^\\s,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex LoadInstructionRegex();

    [GeneratedRegex("^load\\s+volatile\\s+(?<type>[^,]+),\\s+ptr\\s+(?<source>[^\\s,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex VolatileLoadInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*alloca\\s+(?<type>[^,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex AllocaInstructionRegex();

    [GeneratedRegex("^store\\s+(?:atomic\\s+)?(?<type><[^>]+>|[^\\s]+)\\s+(?<value><[^>]+>|[^,]+),\\s+ptr\\s+(?<destination>[^\\s,]+).*$", RegexOptions.CultureInvariant)]
    private static partial Regex StoreInstructionRegex();

    [GeneratedRegex("^store\\s+(?:atomic\\s+)?(?<type><[^>]+>|[^\\s]+)\\s+(?<value><[^>]+>|[^,]+),\\s+ptr\\s+getelementptr(?:\\s+[A-Za-z0-9_]+)*\\s*\\((?<elementType>.+),\\s+ptr\\s+@(?<destination>[^,]+),\\s+i64\\s+(?<index>-?\\d+)\\).*$", RegexOptions.CultureInvariant)]
    private static partial Regex GlobalElementStoreInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*extractvalue\\s+(?<aggType>\\{[^}]+\\})\\s+(?<source>[^,]+),\\s*(?<index>\\d+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex ExtractValueInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*insertvalue\\s+(?<aggType>\\{[^}]+\\})\\s+(?<base>[^,]+),\\s+(?:[^\\s]+)\\s+(?<value>[^,]+),\\s*(?<index>\\d+)(?:\\s*,.*)?$", RegexOptions.CultureInvariant)]
    private static partial Regex InsertValueInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*atomicrmw\\s+(?<op>\\w+)\\s+ptr\\s+(?<ptr>[^,]+),\\s*(?<valType>\\w+)\\s+(?<val>[^\\s,]+)", RegexOptions.CultureInvariant)]
    private static partial Regex AtomicRmwInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*cmpxchg(?:\\s+weak)?\\s+ptr\\s+(?<ptr>[^,]+),\\s*(?<valType>\\w+)\\s+(?<cmpVal>[^,]+),\\s*\\w+\\s+(?<newVal>[^\\s,]+)", RegexOptions.CultureInvariant)]
    private static partial Regex CmpxchgInstructionRegex();

    [GeneratedRegex("^%(?<result>[^\\s=]+)\\s*=\\s*cmpxchg(?:\\s+weak)?\\s+ptr\\s+getelementptr(?:\\s+[A-Za-z0-9_]+)*\\s*\\((?<elementType>.+),\\s+ptr\\s+@(?<ptr>[^,]+),\\s+i64\\s+(?<index>-?\\d+)\\),\\s*(?<valType>\\w+)\\s+(?<cmpVal>[^,]+),\\s*\\w+\\s+(?<newVal>[^\\s,]+)", RegexOptions.CultureInvariant)]
    private static partial Regex GlobalElementCmpxchgInstructionRegex();

    private sealed class LoweredBlockBuilder(string name)
    {
        private readonly List<LoweredInstruction> _instructions = [];

        public string Name { get; } = name;

        public void Add(LoweredInstruction instruction)
        {
            _instructions.Add(instruction);
        }

        public LoweredBlock ToBlock()
        {
            return new LoweredBlock(Name, CoalesceSwitches(_instructions));
        }

        private static IReadOnlyList<LoweredInstruction> CoalesceSwitches(IReadOnlyList<LoweredInstruction> instructions)
        {
            // Detect the multi-line LLVM switch pattern that the text lowerer emits as raw lines:
            //   switch <type> <value>, label %<default> [
            //     <type> <case-value>, label %<target>
            //     ...
            //   ], !prof !metadata
            // and replace it with a single LoweredSwitchInstruction. This frees downstream tools
            // from re-parsing the IR text and gives strict diagnostics structured switch metadata.
            var headerRegex = new System.Text.RegularExpressions.Regex(
                "^switch (?<type>i\\d+) (?<value>[^,]+), label %(?<defaultTarget>\"[^\"]+\"|[^\\s]+) \\[$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant);
            var caseRegex = new System.Text.RegularExpressions.Regex(
                "^(?<type>i\\d+) (?<value>-?\\d+), label %(?<target>\"[^\"]+\"|[^\\s]+)$",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant);

            List<LoweredInstruction>? rewritten = null;
            for (var i = 0; i < instructions.Count; i++)
            {
                if (instructions[i] is LoweredRawInstruction raw)
                {
                    var hdr = headerRegex.Match(raw.Text);
                    if (hdr.Success)
                    {
                        if (!CanCoalesceSwitchType(hdr.Groups["type"].Value))
                        {
                            rewritten?.Add(instructions[i]);
                            continue;
                        }

                        // Scan forward for cases until "]".
                        var cases = new List<LoweredSwitchCase>();
                        var j = i + 1;
                        var closing = -1;
                        while (j < instructions.Count && instructions[j] is LoweredRawInstruction caseRaw)
                        {
                            if (IsSwitchClosingLine(caseRaw.Text))
                            {
                                closing = j;
                                break;
                            }
                            var cm = caseRegex.Match(caseRaw.Text);
                            if (cm.Success)
                            {
                                cases.Add(new LoweredSwitchCase(
                                    ParseSwitchCaseValue(cm.Groups["value"].Value, cm.Groups["type"].Value),
                                    StripLabelQuotes(cm.Groups["target"].Value)));
                                j++;
                                continue;
                            }
                            break;
                        }

                        if (closing >= 0)
                        {
                            rewritten ??= new List<LoweredInstruction>(instructions.Take(i));
                            rewritten.Add(new LoweredSwitchInstruction(
                                hdr.Groups["type"].Value,
                                hdr.Groups["value"].Value.Trim(),
                                StripLabelQuotes(hdr.Groups["defaultTarget"].Value),
                                cases));
                            i = closing;
                            continue;
                        }
                    }
                }

                rewritten?.Add(instructions[i]);
            }

            return rewritten ?? instructions;
        }

        private static bool IsSwitchClosingLine(string text)
        {
            var trimmed = text.Trim();
            return string.Equals(trimmed, "]", StringComparison.Ordinal)
                || trimmed.StartsWith("],", StringComparison.Ordinal);
        }

        private static string StripLabelQuotes(string raw)
        {
            var t = raw.Trim();
            return t.Length >= 2 && t[0] == '"' && t[^1] == '"' ? t.Substring(1, t.Length - 2) : t;
        }
    }
}
