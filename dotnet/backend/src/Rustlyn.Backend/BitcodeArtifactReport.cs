namespace Rustlyn.Backend;

public sealed record BitcodeArtifactReport(
    string FullPath,
    long Length,
    string MagicBytes,
    bool LooksLikeLlvmBitcode,
    DateTimeOffset LastWriteTimeUtc,
    LlvmModuleSummary? ModuleSummary);

public sealed record LlvmModuleSummary(
    string LlvmRoot,
    string ReaderKind,
    IReadOnlyList<LlvmFunctionSummary> Functions,
    IReadOnlyList<LlvmAliasSummary> Aliases,
    IReadOnlyList<LlvmGlobalSummary> Globals)
{
    public int BasicBlockCount => Functions.Sum(static function => function.BasicBlockCount);

    public int InstructionCount => Functions.Sum(static function => function.InstructionCount);
}

public sealed record LlvmFunctionSummary(
    string Name,
    int BasicBlockCount,
    int InstructionCount);

public sealed record LlvmAliasSummary(
    string Name,
    string Target,
    string Signature);

public sealed record LlvmGlobalSummary(
    string Name);
