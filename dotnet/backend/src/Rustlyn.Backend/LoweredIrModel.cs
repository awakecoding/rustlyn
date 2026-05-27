namespace Rustlyn.Backend;

public sealed record LoweredModule(
    IReadOnlyList<LoweredFunction> Functions,
    IReadOnlyList<LoweredGlobal> Globals,
    string? SourcePath = null,
    LoweredModuleMetadata? Metadata = null);

/// <summary>
/// Module-level metadata captured from LLVM IR/bitcode. Not all back-ends populate every field; consumers
/// must treat unset values as "unknown" and fall back to platform defaults only with explicit policy.
/// </summary>
public sealed record LoweredModuleMetadata(
    string? DataLayout = null,
    string? TargetTriple = null,
    IReadOnlyList<string>? SourceDocuments = null);

public sealed record LoweredGlobal(
    string Name,
    IReadOnlyList<byte> InitializerBytes,
    IReadOnlyList<LoweredGlobalPointerRelocation> PointerRelocations)
{
    public LoweredGlobal(string name, IReadOnlyList<byte> initializerBytes)
        : this(name, initializerBytes, [])
    {
    }
}

public sealed record LoweredGlobalPointerRelocation(
    int Offset,
    string Target);

public sealed record LoweredFunction(
    string Name,
    string ReturnType,
    IReadOnlyList<LoweredParameter> Parameters,
    IReadOnlyList<LoweredBlock> Blocks,
    string? ReturnExtension = null);

public sealed record LoweredParameter(
    string Name,
    string Type,
    string? Extension = null);

public sealed record LoweredBlock(
    string Name,
    IReadOnlyList<LoweredInstruction> Instructions);

public abstract record LoweredInstruction;

public sealed record LoweredBinaryInstruction(
    string Result,
    string Operation,
    string Type,
    string Left,
    string Right) : LoweredInstruction;

public sealed record LoweredCallInstruction(
    string? Result,
    string ReturnType,
    string Callee,
    IReadOnlyList<LoweredArgument> Arguments) : LoweredInstruction;

public sealed record LoweredReturnInstruction(
    string Type,
    string Value) : LoweredInstruction;

public sealed record LoweredCompareInstruction(
    string Result,
    string Predicate,
    string Type,
    string Left,
    string Right) : LoweredInstruction;

public sealed record LoweredConditionalBranchInstruction(
    string Condition,
    string TrueTarget,
    string FalseTarget) : LoweredInstruction;

public sealed record LoweredJumpInstruction(
    string Target) : LoweredInstruction;

public sealed record LoweredLoadInstruction(
    string Result,
    string Type,
    string Source) : LoweredInstruction;

public sealed record LoweredAllocaInstruction(
    string Result,
    string Type) : LoweredInstruction;

public sealed record LoweredGetElementPointerInstruction(
    string Result,
    string ElementType,
    string Base,
    int Index,
    string? IndexVariable = null) : LoweredInstruction;

public sealed record LoweredTruncateInstruction(
    string Result,
    string FromType,
    string ToType,
    string Value) : LoweredInstruction;

public sealed record LoweredZeroExtendInstruction(
    string Result,
    string FromType,
    string ToType,
    string Value) : LoweredInstruction;

public sealed record LoweredSignExtendInstruction(
    string Result,
    string FromType,
    string ToType,
    string Value) : LoweredInstruction;

public sealed record LoweredPtrToIntInstruction(
    string Result,
    string ToType,
    string Value) : LoweredInstruction;

public sealed record LoweredIntToPtrInstruction(
    string Result,
    string Value) : LoweredInstruction;

public sealed record LoweredFreezeInstruction(
    string Result,
    string Type,
    string Value) : LoweredInstruction;

public sealed record LoweredSelectInstruction(
    string Result,
    string Condition,
    string ValueType,
    string TrueValue,
    string FalseValue) : LoweredInstruction;

public sealed record LoweredPhiInstruction(
    string Result,
    string Type,
    IReadOnlyList<LoweredPhiIncoming> Incoming) : LoweredInstruction;

public sealed record LoweredUnreachableInstruction() : LoweredInstruction;

public sealed record LoweredStoreInstruction(
    string Type,
    string Value,
    string Destination) : LoweredInstruction;

public sealed record LoweredExtractValueInstruction(
    string Result,
    string AggregateType,
    string Source,
    int Index) : LoweredInstruction;

public sealed record LoweredInsertValueInstruction(
    string Result,
    string AggregateType,
    string Base,
    string Value,
    int Index) : LoweredInstruction;

public sealed record LoweredAtomicRmwInstruction(
    string Result,
    string Operation,
    string Pointer,
    string ValueType,
    string Value) : LoweredInstruction;

public sealed record LoweredCmpxchgInstruction(
    string Result,
    string Pointer,
    string ValueType,
    string CompareValue,
    string NewValue) : LoweredInstruction;

public sealed record LoweredRawInstruction(
    string Text) : LoweredInstruction;

/// <summary>
/// Typed marker for LLVM <c>invoke</c> control-flow. Carries the call shape plus
/// the normal/exceptional successor labels so that emission can either lower it
/// (once exception regions are implemented) or fail fast with structured detail.
/// </summary>
public sealed record LoweredInvokeInstruction(
    string? Result,
    string ReturnType,
    string Callee,
    IReadOnlyList<LoweredArgument> Arguments,
    string NormalLabel,
    string UnwindLabel) : LoweredInstruction;

/// <summary>
/// Typed marker for a landing pad / cleanup / catch shape. The clauses are kept
/// verbatim from the IR text so diagnostics can echo the user's source intent.
/// </summary>
public sealed record LoweredLandingPadInstruction(
    string? Result,
    string ResultType,
    string ClausesText,
    bool IsCleanup) : LoweredInstruction;

/// <summary>Typed marker for LLVM <c>fence</c>. Preserves ordering text verbatim.</summary>
public sealed record LoweredFenceInstruction(
    string Ordering,
    string? SyncScope) : LoweredInstruction;

/// <summary>Typed marker for a volatile load. Pointer/type captured for diagnostics.</summary>
public sealed record LoweredVolatileLoadInstruction(
    string Result,
    string ValueType,
    string Pointer) : LoweredInstruction;

/// <summary>Typed marker for a volatile store. Pointer/type captured for diagnostics.</summary>
public sealed record LoweredVolatileStoreInstruction(
    string ValueType,
    string Value,
    string Pointer) : LoweredInstruction;

/// <summary>
/// Typed representation of LLVM <c>switch</c>. Replaces the previous multi-raw-instruction state
/// machine in the emitter so the model carries every case explicitly and downstream tools (strict
/// diagnostics, JIT-style codegen) can reason about switch shape without re-parsing IR text.
/// </summary>
public sealed record LoweredSwitchInstruction(
    string ValueType,
    string Value,
    string DefaultLabel,
    System.Collections.Generic.IReadOnlyList<LoweredSwitchCase> Cases) : LoweredInstruction;

public sealed record LoweredSwitchCase(long Value, string Target);

public sealed record LoweredArgument(
    string Type,
    string Value);

public sealed record LoweredPhiIncoming(
    string Value,
    string SourceBlock);
