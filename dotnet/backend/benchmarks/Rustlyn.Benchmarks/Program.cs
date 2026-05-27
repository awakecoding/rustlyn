using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Rustlyn.Backend;

BenchmarkSwitcher.FromAssembly(typeof(LoweringBenchmarks).Assembly).Run(args);

/// <summary>
/// Benchmarks for the IR lowering pipeline (bitcode → LoweredModule).
/// Run with: dotnet run -c Release -- --filter '*'
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 10)]
public class LoweringBenchmarks
{
    private string _addBitcodePath = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Locate the 'add' sample bitcode (must be pre-built)
        var repoRoot = FindRepoRoot();
        _addBitcodePath = Path.Combine(repoRoot, "artifacts", "out", "add", "add.bc");

        if (!File.Exists(_addBitcodePath))
        {
            throw new FileNotFoundException(
                $"Bitcode not found at '{_addBitcodePath}'. Run Build-SampleBitcode.ps1 -Sample add first.");
        }
    }

    [Benchmark(Description = "Lower 'add' bitcode to IR")]
    public LoweredModule LowerAdd()
    {
        return LoweredIrLowerer.LowerBitcode(_addBitcodePath);
    }

    [Benchmark(Description = "Emit 'add' assembly to memory")]
    public void EmitAdd()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"bench-{Guid.NewGuid():N}.dll");
        try
        {
            LoweredAssemblyEmitter.EmitBitcode(_addBitcodePath, outputPath);
        }
        finally
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);
        }
    }

    private static string FindRepoRoot()
    {
        var dir = AppContext.BaseDirectory;
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir, "AGENTS.md")))
                return dir;
            dir = Path.GetDirectoryName(dir);
        }

        throw new InvalidOperationException("Could not locate repository root from benchmark assembly.");
    }
}
