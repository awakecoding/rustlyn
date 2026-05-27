namespace Rustlyn.Backend;

public static class BitcodeArtifactInspector
{
    private static ReadOnlySpan<byte> LlvmBitcodeMagic => [0x42, 0x43, 0xC0, 0xDE];
    // LLVM bitcode wrapper format (used by rustc on macOS/Linux): magic is 0x0B17C0DE stored little-endian.
    // See llvm/Bitcode/BitcodeAnalyzer: prepends a 16-byte header before the raw bitcode payload.
    private static ReadOnlySpan<byte> LlvmBitcodeWrapperMagic => [0xDE, 0xC0, 0x17, 0x0B];

    public static BitcodeArtifactReport Inspect(string artifactPath, string? llvmRoot = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);

        var fullPath = Path.GetFullPath(artifactPath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Bitcode artifact was not found.", fullPath);
        }

        var fileInfo = new FileInfo(fullPath);
        var magicBuffer = new byte[4];

        using var stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        var bytesRead = stream.Read(magicBuffer, 0, magicBuffer.Length);

        if (bytesRead < magicBuffer.Length)
        {
            throw new InvalidDataException($"Expected at least {magicBuffer.Length} bytes in bitcode artifact '{fullPath}', but only read {bytesRead}.");
        }

        var magicSpan = magicBuffer.AsSpan();
        var looksLikeBitcode = magicSpan.SequenceEqual(LlvmBitcodeMagic)
            || magicSpan.SequenceEqual(LlvmBitcodeWrapperMagic);
        LlvmModuleSummary? summary = null;
        string? summaryError = null;
        if (looksLikeBitcode)
        {
            (summary, summaryError) = TryReadModuleSummary(fullPath, llvmRoot);
        }

        return new BitcodeArtifactReport(
            fullPath,
            fileInfo.Length,
            Convert.ToHexString(magicBuffer),
            looksLikeBitcode,
            fileInfo.LastWriteTimeUtc,
            summary,
            summaryError);
    }

    private static (LlvmModuleSummary? Summary, string? Error) TryReadModuleSummary(string artifactPath, string? llvmRoot)
    {
        try
        {
            return (LlvmModuleReader.TryReadSummary(artifactPath, llvmRoot), null);
        }
        catch (InvalidDataException ex)
        {
            return (null, ex.Message);
        }
    }
}

