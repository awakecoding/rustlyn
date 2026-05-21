using RustMcil.Bindings;

if (args.Length == 3
    && string.Equals(args[0], "managed-glue", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, ManagedGlueGenerator.GenerateRuntimeBridgePartial(BindingSurface.CreateTinyBclSurface()));
    return 0;
}

Console.Error.WriteLine("Usage: RustMcil.Bindings.Tool managed-glue --out <path>");
return 2;