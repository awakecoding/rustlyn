using Rustlyn.Bindings;

if (args.Length == 3
    && string.Equals(args[0], "managed-glue", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, ManagedGlueGenerator.GenerateRuntimeBridgePartial(BindingSurface.CreateTinyBclSurface()));
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "rust-system-module", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, RustBindingGenerator.GenerateSystemModule(BindingSurface.CreateTinyBclSurface()));
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "manifest", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, BindingManifestGenerator.GenerateText(BindingSurface.CreateTinyBclSurface()));
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "manifest-json", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, BindingManifestGenerator.GenerateJson(BindingSurface.CreateTinyBclSurface()));
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "pack", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    BindingArtifactPackGenerator.WritePack(BindingSurface.CreateTinyBclSurface(), args[2]);
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "validate", StringComparison.Ordinal)
    && string.Equals(args[1], "--type", StringComparison.Ordinal))
{
    var type = Type.GetType(args[2], throwOnError: false)
        ?? throw new InvalidOperationException($"Type '{args[2]}' could not be resolved.");
    var result = BindingSurfaceScanner.ScanTypeWithDiagnostics(type);
    foreach (var shape in result.UnsupportedShapes)
    {
        Console.Error.WriteLine($"{shape.DisplayName}: {shape.Reason}");
    }

    Console.WriteLine($"requirements: {result.Requirements.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    Console.WriteLine($"unsupported: {result.UnsupportedShapes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    return result.UnsupportedShapes.Count == 0 ? 0 : 1;
}

Console.Error.WriteLine("Usage: Rustlyn.Bindings.Tool managed-glue --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool rust-system-module --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool manifest --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool manifest-json --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool pack --out <directory>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool validate --type <assembly-qualified-type>");
return 2;
