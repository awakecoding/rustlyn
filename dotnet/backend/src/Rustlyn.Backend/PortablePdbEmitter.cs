using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace Rustlyn.Backend;

/// <summary>
/// Emits Portable PDB debug information for translated assemblies.
/// Maps emitted IL methods back to their original .rs source locations
/// when debug metadata is available from LLVM bitcode.
/// </summary>
public sealed class PortablePdbEmitter
{
    private readonly MetadataBuilder _debugMetadata = new();
    private readonly Dictionary<string, DocumentHandle> _documents = new(StringComparer.Ordinal);
    private readonly List<MethodDebugInfo> _methodDebugInfos = [];
    private readonly Guid _pdbId = Guid.NewGuid();

    /// <summary>
    /// Registers a source document (e.g., a .rs file) for use in sequence points.
    /// </summary>
    public DocumentHandle GetOrAddDocument(string path)
    {
        if (_documents.TryGetValue(path, out var existing))
            return existing;

        var handle = _debugMetadata.AddDocument(
            name: _debugMetadata.GetOrAddDocumentName(path),
            hashAlgorithm: _debugMetadata.GetOrAddGuid(HashAlgorithmGuids.Sha256),
            hash: default,
            language: _debugMetadata.GetOrAddGuid(LanguageGuids.Rust));

        _documents[path] = handle;
        return handle;
    }

    /// <summary>
    /// Records debug information for a method: the document it came from and
    /// IL-to-source line mappings.
    /// </summary>
    public void AddMethodDebugInfo(MethodDefinitionHandle method, DocumentHandle document, IReadOnlyList<SequencePointEntry> sequencePoints)
    {
        _methodDebugInfos.Add(new MethodDebugInfo(method, document, sequencePoints));
    }

    /// <summary>
    /// Records a method with a single entry-point sequence point (function start line).
    /// Use when full line mapping is not yet available.
    /// </summary>
    public void AddMethodEntryPoint(MethodDefinitionHandle method, DocumentHandle document, int startLine)
    {
        var points = new[] { new SequencePointEntry(IlOffset: 0, StartLine: startLine, StartColumn: 0, EndLine: startLine, EndColumn: 0) };
        AddMethodDebugInfo(method, document, points);
    }

    /// <summary>
    /// Serializes the PDB and writes it to disk. Returns the PDB content ID
    /// that must be embedded in the PE's debug directory.
    /// </summary>
    public PortablePdbResult Serialize(string pdbPath)
    {
        // Write method debug information rows
        foreach (var info in _methodDebugInfos.OrderBy(m => MetadataTokens.GetRowNumber(m.Method)))
        {
            var sequencePointsBlob = SerializeSequencePoints(info.Document, info.SequencePoints);

            _debugMetadata.AddMethodDebugInformation(
                document: info.Document,
                sequencePoints: _debugMetadata.GetOrAddBlob(sequencePointsBlob));
        }

        // PortablePdbBuilder requires row counts for cross-reference tables from the PE metadata.
        // When emitting a standalone PDB without synchronized metadata, pass empty row counts.
        var rowCounts = ImmutableArray.Create(new int[MetadataTokens.TableCount]);
        var pdbBuilder = new PortablePdbBuilder(_debugMetadata, rowCounts, MetadataTokens.MethodDefinitionHandle(1));
        var pdbBlob = new BlobBuilder();
        var pdbContentId = pdbBuilder.Serialize(pdbBlob);

        using var fs = new FileStream(pdbPath, FileMode.Create, FileAccess.Write);
        pdbBlob.WriteContentTo(fs);

        return new PortablePdbResult(_pdbId, pdbContentId, pdbPath);
    }

    private static BlobBuilder SerializeSequencePoints(DocumentHandle document, IReadOnlyList<SequencePointEntry> points)
    {
        var blob = new BlobBuilder();
        var encoder = new BlobEncoder(blob);

        // Sequence points blob format (ECMA-335 Portable PDB spec):
        // - LocalSignature row (0 = none)
        // - InitialDocument (if needed)
        // - Then records: IL offset delta, start line delta, start column delta, etc.
        blob.WriteCompressedInteger(0); // LocalSignature = none

        if (points.Count == 0)
            return blob;

        // First record: absolute IL offset, absolute line/column
        var first = points[0];
        blob.WriteCompressedInteger(first.IlOffset); // IL offset (absolute for first)
        var deltaLines = first.EndLine - first.StartLine;
        var deltaColumns = first.EndColumn - first.StartColumn;

        if (deltaLines == 0 && deltaColumns == 0)
        {
            // Hidden sequence point
            blob.WriteCompressedInteger(0);
            blob.WriteCompressedInteger(0);
        }
        else
        {
            blob.WriteCompressedInteger(deltaLines);
            if (deltaLines == 0)
            {
                blob.WriteCompressedInteger(deltaColumns);
            }
            else
            {
                blob.WriteCompressedSignedInteger(deltaColumns);
            }
        }

        blob.WriteCompressedInteger(first.StartLine); // absolute start line
        blob.WriteCompressedSignedInteger(first.StartColumn); // absolute start column

        // Subsequent records: deltas from previous
        for (int i = 1; i < points.Count; i++)
        {
            var prev = points[i - 1];
            var curr = points[i];

            var ilDelta = curr.IlOffset - prev.IlOffset;
            blob.WriteCompressedInteger(ilDelta);

            deltaLines = curr.EndLine - curr.StartLine;
            deltaColumns = curr.EndColumn - curr.StartColumn;

            if (deltaLines == 0 && deltaColumns == 0)
            {
                blob.WriteCompressedInteger(0);
                blob.WriteCompressedInteger(0);
            }
            else
            {
                blob.WriteCompressedInteger(deltaLines);
                if (deltaLines == 0)
                {
                    blob.WriteCompressedInteger(deltaColumns);
                }
                else
                {
                    blob.WriteCompressedSignedInteger(deltaColumns);
                }
            }

            blob.WriteCompressedSignedInteger(curr.StartLine - prev.StartLine);
            blob.WriteCompressedSignedInteger(curr.StartColumn - prev.StartColumn);
        }

        return blob;
    }

    private sealed record MethodDebugInfo(
        MethodDefinitionHandle Method,
        DocumentHandle Document,
        IReadOnlyList<SequencePointEntry> SequencePoints);

    /// <summary>
    /// Well-known language GUIDs for Portable PDB.
    /// </summary>
    private static class LanguageGuids
    {
        /// <summary>Rust language GUID (custom — no official GUID exists yet).</summary>
        public static readonly Guid Rust = new("E4B8D57A-7C4E-4E14-B5F6-8A1A8B5C3D2E");
    }

    /// <summary>
    /// Well-known hash algorithm GUIDs for Portable PDB.
    /// </summary>
    private static class HashAlgorithmGuids
    {
        public static readonly Guid Sha256 = new("8829d00f-11b8-4213-878b-770e8597ac16");
    }
}

/// <summary>
/// A single IL offset → source location mapping.
/// </summary>
public sealed record SequencePointEntry(
    int IlOffset,
    int StartLine,
    int StartColumn,
    int EndLine,
    int EndColumn);

/// <summary>
/// Result of PDB serialization — provides the content ID needed by the PE builder.
/// </summary>
public sealed record PortablePdbResult(
    Guid PdbId,
    BlobContentId ContentId,
    string PdbPath);
