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
    && string.Equals(args[0], "avalonia-managed-glue", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, ManagedGlueGenerator.GenerateRuntimeBridgePartial(ExternalPackageBindingSurfaces.CreateAvaloniaHelloSurface()));
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "powershell-managed-glue", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, ManagedGlueGenerator.GenerateRuntimeBridgePartial(ExternalPackageBindingSurfaces.CreatePowerShellCmdletSurface()));
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
    && string.Equals(args[0], "avalonia-hello-module", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, AvaloniaRustBindingGenerator.GenerateHelloModule());
    return 0;
}

if (args.Length == 3
    && string.Equals(args[0], "avalonia-manifest-json", StringComparison.Ordinal)
    && string.Equals(args[1], "--out", StringComparison.Ordinal))
{
    var outputPath = Path.GetFullPath(args[2]);
    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
    File.WriteAllText(outputPath, BindingManifestGenerator.GenerateJson(ExternalPackageBindingSurfaces.CreateAvaloniaHelloManifest()));
    return 0;
}

if (args.Length == 9
    && string.Equals(args[0], "package-pack", StringComparison.Ordinal)
    && string.Equals(args[1], "--package", StringComparison.Ordinal)
    && string.Equals(args[3], "--version", StringComparison.Ordinal)
    && string.Equals(args[5], "--tfm", StringComparison.Ordinal)
    && string.Equals(args[7], "--out", StringComparison.Ordinal))
{
    if (string.Equals(args[2], "Avalonia", StringComparison.Ordinal)
        && string.Equals(args[4], "12.0.3", StringComparison.Ordinal)
        && string.Equals(args[6], "net10.0", StringComparison.Ordinal))
    {
        ExternalPackageProjectionPackGenerator.WriteAvaloniaHelloPack(args[8]);
        return 0;
    }

    if (string.Equals(args[2], "Microsoft.PowerShell.SDK", StringComparison.Ordinal)
        && string.Equals(args[4], "7.5.0", StringComparison.Ordinal)
        && string.Equals(args[6], "net10.0", StringComparison.Ordinal))
    {
        ExternalPackageProjectionPackGenerator.WritePowerShellCmdletPack(args[8]);
        return 0;
    }

    throw new NotSupportedException("The package-pack prototype currently supports --package Avalonia --version 12.0.3 --tfm net10.0 and --package Microsoft.PowerShell.SDK --version 7.5.0 --tfm net10.0.");
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

if (args.Length >= 3
    && string.Equals(args[0], "validate", StringComparison.Ordinal)
    && string.Equals(args[1], "--type", StringComparison.Ordinal))
{
    var typeName = args[2];
    var reportOnly = args.Length >= 4 && string.Equals(args[3], "--report-only", StringComparison.Ordinal);
    var type = Type.GetType(typeName, throwOnError: false)
        ?? throw new InvalidOperationException($"Type '{typeName}' could not be resolved.");
    var result = BindingSurfaceScanner.ScanTypeWithDiagnostics(type);
    foreach (var shape in result.UnsupportedShapes)
    {
        Console.Error.WriteLine($"{shape.DisplayName}: {shape.Reason}");
    }

    Console.WriteLine($"requirements: {result.Requirements.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    Console.WriteLine($"unsupported: {result.UnsupportedShapes.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    if (reportOnly)
    {
        // Diagnostic mode: succeed as long as the scanner produced at least one
        // projectable requirement. This is used by CI to keep an executable signal
        // on System.Math even while individual overloads remain unsupported.
        return result.Requirements.Count == 0 ? 1 : 0;
    }
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

if (args.Length >= 1
    && string.Equals(args[0], "runtime-scan", StringComparison.Ordinal))
{
    var targetFrameworks = new List<string>();
    string? dotnetRoot = null;
    string? namespaceFilter = null;
    string? outputPath = null;
    var json = false;
    var manifestJson = false;
    var diff = false;
    var diffJson = false;

    for (var index = 1; index < args.Length; index++)
    {
        var arg = args[index];
        if (string.Equals(arg, "--tfm", StringComparison.Ordinal))
        {
            targetFrameworks.Add(ReadRequiredOptionValue(args, ref index, "--tfm"));
        }
        else if (string.Equals(arg, "--dotnet-root", StringComparison.Ordinal))
        {
            dotnetRoot = ReadRequiredOptionValue(args, ref index, "--dotnet-root");
        }
        else if (string.Equals(arg, "--namespace", StringComparison.Ordinal))
        {
            namespaceFilter = ReadRequiredOptionValue(args, ref index, "--namespace");
        }
        else if (string.Equals(arg, "--out", StringComparison.Ordinal))
        {
            outputPath = ReadRequiredOptionValue(args, ref index, "--out");
        }
        else if (string.Equals(arg, "--json", StringComparison.Ordinal))
        {
            json = true;
        }
        else if (string.Equals(arg, "--manifest-json", StringComparison.Ordinal))
        {
            manifestJson = true;
        }
        else if (string.Equals(arg, "--diff", StringComparison.Ordinal))
        {
            diff = true;
        }
        else if (string.Equals(arg, "--diff-json", StringComparison.Ordinal))
        {
            diffJson = true;
        }
        else
        {
            throw new InvalidOperationException($"Unknown runtime-scan option '{arg}'.");
        }
    }

    var scanSet = RuntimeReferenceSurfaceScanner.ScanInstalledPacks(
        targetFrameworks.Count == 0 ? null : targetFrameworks,
        dotnetRoot,
        namespaceFilter);
    var output = json
        ? RuntimeSurfaceReportFormatter.GenerateJson(scanSet)
        : diffJson
            ? RuntimeSurfaceReportFormatter.GenerateDiffJson(scanSet)
        : diff
            ? RuntimeSurfaceReportFormatter.GenerateDiffText(scanSet)
        : manifestJson
            ? RuntimeSurfaceReportFormatter.GenerateManifestJson(scanSet)
        : RuntimeSurfaceReportFormatter.GenerateText(scanSet);

    if (outputPath is null)
    {
        Console.Write(output);
    }
    else
    {
        var fullOutputPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullOutputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
        File.WriteAllText(fullOutputPath, output);
    }

    foreach (var missing in scanSet.MissingTargets)
    {
        Console.Error.WriteLine($"Missing reference pack for {missing.TargetFramework}.");
    }

    return scanSet.Reports.Count > 0 ? 0 : 1;
}

if (args.Length >= 1
    && string.Equals(args[0], "runtime-pack", StringComparison.Ordinal))
{
    var targetFrameworks = new List<string>();
    string? dotnetRoot = null;
    string? namespaceFilter = null;
    string? outputDirectory = null;

    for (var index = 1; index < args.Length; index++)
    {
        var arg = args[index];
        if (string.Equals(arg, "--tfm", StringComparison.Ordinal))
        {
            targetFrameworks.Add(ReadRequiredOptionValue(args, ref index, "--tfm"));
        }
        else if (string.Equals(arg, "--dotnet-root", StringComparison.Ordinal))
        {
            dotnetRoot = ReadRequiredOptionValue(args, ref index, "--dotnet-root");
        }
        else if (string.Equals(arg, "--namespace", StringComparison.Ordinal))
        {
            namespaceFilter = ReadRequiredOptionValue(args, ref index, "--namespace");
        }
        else if (string.Equals(arg, "--out", StringComparison.Ordinal))
        {
            outputDirectory = ReadRequiredOptionValue(args, ref index, "--out");
        }
        else
        {
            throw new InvalidOperationException($"Unknown runtime-pack option '{arg}'.");
        }
    }

    if (outputDirectory is null)
    {
        throw new InvalidOperationException("runtime-pack requires --out <directory>.");
    }

    var scanSet = RuntimeReferenceSurfaceScanner.ScanInstalledPacks(
        targetFrameworks.Count == 0 ? null : targetFrameworks,
        dotnetRoot,
        namespaceFilter);
    RuntimeProjectionPackGenerator.WritePacks(scanSet, outputDirectory);

    foreach (var missing in scanSet.MissingTargets)
    {
        Console.Error.WriteLine($"Missing reference pack for {missing.TargetFramework}.");
    }

    Console.WriteLine($"Runtime projection packs: {scanSet.Reports.Count.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Output: {Path.GetFullPath(outputDirectory)}");
    return scanSet.Reports.Count > 0 ? 0 : 1;
}

if (args.Length >= 1
    && string.Equals(args[0], "runtime-gate", StringComparison.Ordinal))
{
    var targetFrameworks = new List<string>();
    string? dotnetRoot = null;
    string? namespaceFilter = null;
    string? outputPath = null;

    for (var index = 1; index < args.Length; index++)
    {
        var arg = args[index];
        if (string.Equals(arg, "--tfm", StringComparison.Ordinal))
        {
            targetFrameworks.Add(ReadRequiredOptionValue(args, ref index, "--tfm"));
        }
        else if (string.Equals(arg, "--dotnet-root", StringComparison.Ordinal))
        {
            dotnetRoot = ReadRequiredOptionValue(args, ref index, "--dotnet-root");
        }
        else if (string.Equals(arg, "--namespace", StringComparison.Ordinal))
        {
            namespaceFilter = ReadRequiredOptionValue(args, ref index, "--namespace");
        }
        else if (string.Equals(arg, "--out", StringComparison.Ordinal))
        {
            outputPath = ReadRequiredOptionValue(args, ref index, "--out");
        }
        else
        {
            throw new InvalidOperationException($"Unknown runtime-gate option '{arg}'.");
        }
    }

    var scanSet = RuntimeReferenceSurfaceScanner.ScanInstalledPacks(
        targetFrameworks.Count == 0 ? null : targetFrameworks,
        dotnetRoot,
        namespaceFilter);
    var gateResult = RuntimeSurfaceGate.Evaluate(scanSet);
    var output = RuntimeSurfaceGate.GenerateText(gateResult);
    if (outputPath is null)
    {
        Console.Write(output);
    }
    else
    {
        var fullOutputPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullOutputPath) ?? throw new InvalidOperationException("Output directory could not be determined."));
        File.WriteAllText(fullOutputPath, output);
    }

    return gateResult.Passed ? 0 : 1;
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
Console.Error.WriteLine("       Rustlyn.Bindings.Tool package-pack --package Avalonia --version 12.0.3 --tfm net10.0 --out <directory>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool validate --type <assembly-qualified-type>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool scan <assembly.dll> [--namespace <filter>]");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool runtime-scan [--tfm <netX.0>] [--dotnet-root <path>] [--namespace <filter>] [--json|--manifest-json|--diff|--diff-json] [--out <path>]");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool runtime-pack [--tfm <netX.0>] [--dotnet-root <path>] [--namespace <filter>] --out <directory>");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool runtime-gate [--tfm <netX.0>] [--dotnet-root <path>] [--namespace <filter>] [--out <path>]");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool bindgen <assembly.dll> [--namespace <filter>]");
Console.Error.WriteLine("       Rustlyn.Bindings.Tool bindgen-type <assembly-qualified-type>");
return 2;

static string ReadRequiredOptionValue(string[] args, ref int index, string option)
{
    if (index + 1 >= args.Length)
    {
        throw new InvalidOperationException($"Option '{option}' requires a value.");
    }

    index++;
    return args[index];
}
