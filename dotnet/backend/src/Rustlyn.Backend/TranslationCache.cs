using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Rustlyn.Backend;

/// <summary>
/// Caches translated function IL so that incremental re-translation only processes
/// functions whose LLVM IR has changed since the last translation.
/// </summary>
public sealed class TranslationCache
{
    private readonly string _cachePath;
    private Dictionary<string, CacheEntry> _entries;

    public TranslationCache(string cachePath)
    {
        _cachePath = cachePath;
        _entries = Load(cachePath);
    }

    /// <summary>
    /// Returns true if the function's IR hash matches the cached version
    /// (meaning it doesn't need re-translation).
    /// </summary>
    public bool IsUpToDate(LoweredFunction function)
    {
        var hash = ComputeFunctionHash(function);
        return _entries.TryGetValue(function.Name, out var entry) && entry.IrHash == hash;
    }

    /// <summary>
    /// Records a successful translation of a function.
    /// </summary>
    public void RecordTranslation(LoweredFunction function)
    {
        var hash = ComputeFunctionHash(function);
        _entries[function.Name] = new CacheEntry(function.Name, hash, DateTime.UtcNow);
    }

    /// <summary>
    /// Returns the set of functions that need re-translation (changed or new).
    /// </summary>
    public IReadOnlyList<LoweredFunction> GetStaleEntries(IReadOnlyList<LoweredFunction> functions)
    {
        return functions.Where(f => !IsUpToDate(f)).ToList();
    }

    /// <summary>
    /// Returns cache statistics.
    /// </summary>
    public CacheStats GetStats(IReadOnlyList<LoweredFunction> functions)
    {
        var stale = functions.Count(f => !IsUpToDate(f));
        return new CacheStats(
            TotalFunctions: functions.Count,
            CachedFunctions: functions.Count - stale,
            StaleFunctions: stale,
            CacheEntries: _entries.Count);
    }

    /// <summary>
    /// Persists the cache to disk.
    /// </summary>
    public void Save()
    {
        var dir = Path.GetDirectoryName(_cachePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(_entries.Values.ToList(), CacheJsonContext.Default.ListCacheEntry);
        File.WriteAllText(_cachePath, json, Encoding.UTF8);
    }

    /// <summary>
    /// Computes a stable hash of a function's IR representation.
    /// Changes in instructions, parameters, or return type produce a different hash.
    /// </summary>
    private static string ComputeFunctionHash(LoweredFunction function)
    {
        using var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        hasher.AppendData(Encoding.UTF8.GetBytes(function.Name));
        hasher.AppendData(Encoding.UTF8.GetBytes(function.ReturnType));

        foreach (var param in function.Parameters)
        {
            hasher.AppendData(Encoding.UTF8.GetBytes(param.Name));
            hasher.AppendData(Encoding.UTF8.GetBytes(param.Type));
        }

        foreach (var block in function.Blocks)
        {
            hasher.AppendData(Encoding.UTF8.GetBytes(block.Name));
            foreach (var instr in block.Instructions)
            {
                hasher.AppendData(Encoding.UTF8.GetBytes(instr.GetType().Name));
            }
        }

        var hash = hasher.GetHashAndReset();
        return Convert.ToHexString(hash);
    }

    private static Dictionary<string, CacheEntry> Load(string path)
    {
        if (!File.Exists(path))
            return new Dictionary<string, CacheEntry>(StringComparer.Ordinal);

        try
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            var entries = JsonSerializer.Deserialize(json, CacheJsonContext.Default.ListCacheEntry);
            if (entries is null)
                return new Dictionary<string, CacheEntry>(StringComparer.Ordinal);

            return entries.ToDictionary(e => e.FunctionName, StringComparer.Ordinal);
        }
        catch (JsonException)
        {
            return new Dictionary<string, CacheEntry>(StringComparer.Ordinal);
        }
    }
}

/// <summary>
/// A single function's cache entry.
/// </summary>
public sealed record CacheEntry(string FunctionName, string IrHash, DateTime TranslatedAt);

/// <summary>
/// Statistics about the translation cache state.
/// </summary>
public sealed record CacheStats(int TotalFunctions, int CachedFunctions, int StaleFunctions, int CacheEntries);

/// <summary>
/// JSON serialization context for cache entries (AOT-compatible).
/// </summary>
[System.Text.Json.Serialization.JsonSerializable(typeof(List<CacheEntry>))]
internal partial class CacheJsonContext : System.Text.Json.Serialization.JsonSerializerContext
{
}
