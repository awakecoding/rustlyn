namespace RustMcil.Backend;

public sealed record LoweredModule(
    IReadOnlyList<LoweredFunction> Functions,
    IReadOnlyList<LoweredGlobal> Globals);

public sealed record LoweredGlobal(
    string Name,
    IReadOnlyList<byte> InitializerBytes);

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

public sealed record LoweredRawInstruction(
    string Text) : LoweredInstruction;

public sealed record LoweredArgument(
    string Type,
    string Value);

public sealed record LoweredPhiIncoming(
    string Value,
    string SourceBlock);
