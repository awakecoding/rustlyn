namespace RustMcil.Backend;

public static class BitcodeArtifactInspector
{
    private static ReadOnlySpan<byte> LlvmBitcodeMagic => [0x42, 0x43, 0xC0, 0xDE];

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

        return new BitcodeArtifactReport(
            fullPath,
            fileInfo.Length,
            Convert.ToHexString(magicBuffer),
            magicBuffer.AsSpan().SequenceEqual(LlvmBitcodeMagic),
            fileInfo.LastWriteTimeUtc,
            magicBuffer.AsSpan().SequenceEqual(LlvmBitcodeMagic) ? TryReadModuleSummary(fullPath, llvmRoot) : null);
    }

    private static LlvmModuleSummary? TryReadModuleSummary(string artifactPath, string? llvmRoot)
    {
        try
        {
            return LlvmModuleReader.TryReadSummary(artifactPath, llvmRoot);
        }
        catch (InvalidDataException)
        {
            return null;
        }
    }
}

