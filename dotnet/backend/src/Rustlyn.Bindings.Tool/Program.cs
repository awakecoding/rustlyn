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

if (args.Length >= 2
    && string.Equals(args[0], "scan", StringComparison.Ordinal))
{
    var assemblyPath = args[1];
    string? namespaceFilter = null;
    if (args.Length >= 4 && string.Equals(args[2], "--namespace", StringComparison.Ordinal))
    {
        namespaceFilter = args[3];
    }

    var scanResult = BindingSurfaceScanner.ScanAssembly(assemblyPath, namespaceFilter);
    Console.WriteLine($"Assembly: {scanResult.AssemblyName}");
    Console.WriteLine($"Types scanned: {scanResult.ScannedTypes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Requirements: {scanResult.Requirements.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Unsupported shapes: {scanResult.UnsupportedShapes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

    if (scanResult.UnsupportedShapes.Count > 0)
    {
        Console.WriteLine();
        Console.WriteLine("Unsupported:");
        foreach (var shape in scanResult.UnsupportedShapes.Take(20))
        {
            Console.WriteLine($"  {shape.DisplayName}: {shape.Reason}");
        }

        if (scanResult.UnsupportedShapes.Count > 20)
        {
            Console.WriteLine($"  ... and {(scanResult.UnsupportedShapes.Count - 20).ToString(System.Globalization.CultureInfo.InvariantCulture)} more");
        }
    }

    return 0;
}

if (args.Length >= 2
    && string.Equals(args[0], "bindgen", StringComparison.Ordinal))
{
    var assemblyPath = args[1];
    string? namespaceFilter = null;
    for (int i = 2; i < args.Length - 1; i++)
    {
        if (string.Equals(args[i], "--namespace", StringComparison.Ordinal))
        {
            namespaceFilter = args[i + 1];
        }
    }

    var assembly = System.Reflection.Assembly.LoadFrom(Path.GetFullPath(assemblyPath));
    var bindings = BindingSurfaceScanner.AutoGenerateBindings(assembly, namespaceFilter);

    Console.WriteLine($"Assembly: {assembly.GetName().Name}");
    Console.WriteLine($"Bindings generated: {bindings.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

    foreach (var binding in bindings)
    {
        Console.WriteLine($"  {binding.Requirement.DisplayName}");
    }

    return 0;
}

if (args.Length >= 2
    && string.Equals(args[0], "bindgen-type", StringComparison.Ordinal))
{
    var typeName = args[1];
    var type = Type.GetType(typeName, throwOnError: false)
        ?? throw new InvalidOperationException($"Type '{typeName}' could not be resolved. Use assembly-qualified name for types outside mscorlib.");
    var bindings = BindingSurfaceScanner.GenerateBindingsForType(type, RustWrapperContainer.Math);

    Console.WriteLine($"Type: {type.FullName}");
    Console.WriteLine($"Bindings generated: {bindings.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

    foreach (var binding in bindings)
    {
        Console.WriteLine($"  {binding.Requirement.DisplayName}");
    }

    return 0;
}

if (args.Length >= 2
    && string.Equals(args[0], "analyze-delegate", StringComparison.Ordinal))
{
    var typeName = args[1];
    var type = Type.GetType(typeName, throwOnError: false)
        ?? throw new InvalidOperationException($"Type '{typeName}' could not be resolved. Use assembly-qualified name for types outside mscorlib.");
    var result = BindingSurfaceScanner.AnalyzeDelegateType(type);

    Console.WriteLine($"Delegate: {result.DelegateType.FullName}");
    Console.WriteLine($"Bindable: {result.IsBindable}");
    if (result.IsBindable)
    {
        Console.WriteLine($"Rust callback: {result.RustCallbackSignature}");
    }
    else
    {
        Console.WriteLine($"Reason: {result.UnsupportedReason}");
    }

    return 0;
}

if (args.Length >= 2
    && string.Equals(args[0], "analyze-events", StringComparison.Ordinal))
{
    var typeName = args[1];
    var type = Type.GetType(typeName, throwOnError: false)
        ?? throw new InvalidOperationException($"Type '{typeName}' could not be resolved. Use assembly-qualified name for types outside mscorlib.");

    var events = type.GetEvents(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
    Console.WriteLine($"Type: {type.FullName}");
    Console.WriteLine($"Events: {events.Length.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

    foreach (var evt in events)
    {
        var result = BindingSurfaceScanner.AnalyzeEvent(evt);
        var status = result.DelegateAnalysis.IsBindable ? "bindable" : "unsupported";
        Console.WriteLine($"  {evt.Name} ({status})");
        if (result.DelegateAnalysis.IsBindable)
        {
            Console.WriteLine($"    Rust: {result.DelegateAnalysis.RustCallbackSignature}");
        }
        else
        {
            Console.WriteLine($"    Reason: {result.DelegateAnalysis.UnsupportedReason}");
        }
    }

    return 0;
}

Console.Error.WriteLine("Usage: Rustlyn.Bindings.Tool managed-glue --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool rust-system-module --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool manifest --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool manifest-json --out <path>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool pack --out <directory>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool validate --type <assembly-qualified-type>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool scan <assembly.dll> [--namespace <filter>]");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool bindgen <assembly.dll> [--namespace <filter>]");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool bindgen-type <assembly-qualified-type>");
return 2;
