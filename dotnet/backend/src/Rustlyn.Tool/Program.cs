using System.Diagnostics;
using System.Reflection;
using Rustlyn.Backend;
using Rustlyn.Bindings;

if (args.Length == 0 || IsHelpFlag(args[0]) || string.Equals(args[0], "help", StringComparison.OrdinalIgnoreCase))
{
    var topic = args.Length > 1 ? args[1] : null;
    PrintUsage(topic);
    return args.Length == 0 ? 1 : 0;
}

if (IsVersionFlag(args[0]))
{
    PrintVersion();
    return 0;
}

if (HasHelpFlag(args))
{
    PrintUsage(args[0]);
    return 0;
}

if (args.Length >= 1 && string.Equals(args[0], "llvm", StringComparison.OrdinalIgnoreCase))
{
    return RunLlvm(args);
}

if (args.Length >= 1 && string.Equals(args[0], "rustc", StringComparison.OrdinalIgnoreCase))
{
    return RunRustc(args);
}

if (args.Length >= 1 && string.Equals(args[0], "new", StringComparison.OrdinalIgnoreCase))
{
    return RunNew(args);
}

if (args.Length >= 1 && string.Equals(args[0], "run", StringComparison.OrdinalIgnoreCase))
{
    return RunRun(args);
}

if (TryParseCargoArguments(args, out var cargoCratePath, out var cargoBuildOptions, out var cargoLlvmRoot, out var cargoStrict, out var cargoPowerShellCmdletBindings))
{
    try
    {
        var cargoResult = RustBitcodeCompiler.BuildCargoProject(cargoCratePath, cargoBuildOptions);
        var loweredModule = LoweredIrLowerer.LowerBitcode(cargoResult.BitcodePath, cargoLlvmRoot);
        var cargoEmitOptions = CreateEmitOptions(true, cargoStrict, cargoPowerShellCmdletBindings, loweredModule);
        LoweredAssemblyEmitter.EmitModule(loweredModule, cargoResult.AssemblyPath, cargoEmitOptions);

        Console.WriteLine($"Bitcode: {Path.GetFullPath(cargoResult.BitcodePath)}");
        Console.WriteLine($"Assembly: {Path.GetFullPath(cargoResult.AssemblyPath)}");
        Console.WriteLine($"PDB: {Path.GetFullPath(Path.ChangeExtension(cargoResult.AssemblyPath, ".pdb"))}");
        return 0;
    }
    catch (UnsupportedIrException ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 4;
    }
    catch (NotSupportedException ex)
    {
        Console.Error.WriteLine($"Unsupported IR: {ex.Message}");
        return 4;
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("failed with exit code"))
    {
        Console.Error.WriteLine($"Build failed: {ex.Message}");
        return 3;
    }
    catch (FileNotFoundException ex)
    {
        Console.Error.WriteLine($"Tool or artifact missing: {ex.Message}");
        return 2;
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.Error.WriteLine($"Tool or artifact missing: {ex.Message}");
        return 2;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

if (args.Length >= 1 && string.Equals(args[0], "diagnose", StringComparison.OrdinalIgnoreCase))
{
    return RunDiagnose(args);
}

if (TryParseInspectArguments(args, out var inspectArtifactPath, out var inspectLlvmRoot))
{
    try
    {
        var report = BitcodeArtifactInspector.Inspect(inspectArtifactPath, inspectLlvmRoot);

        Console.WriteLine($"Path: {report.FullPath}");
        Console.WriteLine($"Length: {report.Length} bytes");
        Console.WriteLine($"MagicBytes: {report.MagicBytes}");
        Console.WriteLine($"LooksLikeLlvmBitcode: {report.LooksLikeLlvmBitcode}");
        Console.WriteLine($"LastWriteTimeUtc: {report.LastWriteTimeUtc:O}");

        if (report.ModuleSummary is null)
        {
            Console.WriteLine("ModuleSummary: unavailable (configure --llvm-root or RUSTLYN_LLVM_ROOT)");
        }
        else
        {
            Console.WriteLine($"LlvmRoot: {report.ModuleSummary.LlvmRoot}");
            Console.WriteLine($"ReaderKind: {report.ModuleSummary.ReaderKind}");
            Console.WriteLine($"FunctionCount: {report.ModuleSummary.Functions.Count}");
            Console.WriteLine($"AliasCount: {report.ModuleSummary.Aliases.Count}");
            Console.WriteLine($"GlobalCount: {report.ModuleSummary.Globals.Count}");
            Console.WriteLine($"BasicBlockCount: {report.ModuleSummary.BasicBlockCount}");
            Console.WriteLine($"InstructionCount: {report.ModuleSummary.InstructionCount}");

            foreach (var function in report.ModuleSummary.Functions)
            {
                Console.WriteLine($"Function: {function.Name} blocks={function.BasicBlockCount} instructions={function.InstructionCount}");
            }

            foreach (var alias in report.ModuleSummary.Aliases)
            {
                Console.WriteLine($"Alias: {alias.Name} -> {alias.Target} signature={alias.Signature}");
            }

            foreach (var global in report.ModuleSummary.Globals)
            {
                Console.WriteLine($"Global: {global.Name}");
            }
        }

        return report.LooksLikeLlvmBitcode ? 0 : 2;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

if (TryParseLowerArguments(args, out var lowerArtifactPath, out var lowerLlvmRoot))
{
    try
    {
        var loweredModule = LoweredIrLowerer.LowerBitcode(lowerArtifactPath, lowerLlvmRoot);
        Console.WriteLine(LoweredIrLowerer.Dump(loweredModule));
        return 0;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

if (TryParseEmitArguments(args, out var emitArtifactPath, out var emitOutputPath, out var emitLlvmRoot, out var emitPdb, out var emitStrict, out var emitPowerShellCmdletBindings))
{
    try
    {
        var loweredModule = LoweredIrLowerer.LowerBitcode(emitArtifactPath, emitLlvmRoot);
        var emitOptions = CreateEmitOptions(emitPdb, emitStrict, emitPowerShellCmdletBindings, loweredModule);
        LoweredAssemblyEmitter.EmitModule(loweredModule, emitOutputPath, emitOptions);
        Console.WriteLine(Path.GetFullPath(emitOutputPath));
        if (emitPdb)
        {
            Console.WriteLine(Path.GetFullPath(Path.ChangeExtension(emitOutputPath, ".pdb")));
        }
        return 0;
    }
    catch (UnsupportedIrException ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 4;
    }
    catch (NotSupportedException ex)
    {
        Console.Error.WriteLine($"Unsupported IR: {ex.Message}");
        return 4;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

if (TryParseTranslateArguments(args, out var cratePath, out var translateOutputPath, out var translateBuildOptions, out var translateLlvmRoot, out var translateCachePath, out var translateStrict, out var translatePowerShellCmdletBindings))
{
    try
    {
        var bitcodePath = RustBitcodeCompiler.BuildBitcode(cratePath, translateBuildOptions);

        TranslationCache? cache = null;
        if (translateCachePath is not null)
        {
            cache = new TranslationCache(translateCachePath);
        }

        var loweredModule = LoweredIrLowerer.LowerBitcode(bitcodePath, translateLlvmRoot);

        if (cache is not null)
        {
            var stats = cache.GetStats(loweredModule.Functions);
            Console.WriteLine($"Cache: {stats.CachedFunctions}/{stats.TotalFunctions} functions up-to-date, {stats.StaleFunctions} need re-translation");

            // Record all functions in cache after successful emission
            foreach (var fn in loweredModule.Functions)
                cache.RecordTranslation(fn);
            cache.Save();
        }

        var translateEmitOptions = CreateEmitOptions(false, translateStrict, translatePowerShellCmdletBindings, loweredModule);
        LoweredAssemblyEmitter.EmitModule(loweredModule, translateOutputPath, translateEmitOptions);
        Console.WriteLine($"Bitcode: {Path.GetFullPath(bitcodePath)}");
        Console.WriteLine($"Assembly: {Path.GetFullPath(translateOutputPath)}");
        return 0;
    }
    catch (UnsupportedIrException ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 4;
    }
    catch (NotSupportedException ex)
    {
        Console.Error.WriteLine($"Unsupported IR: {ex.Message}");
        return 4;
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("failed with exit code"))
    {
        Console.Error.WriteLine($"Build failed: {ex.Message}");
        return 3;
    }
    catch (FileNotFoundException ex)
    {
        Console.Error.WriteLine($"Tool or artifact missing: {ex.Message}");
        return 2;
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.Error.WriteLine($"Tool or artifact missing: {ex.Message}");
        return 2;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

if (TryParseInvokeArguments(args, out var invokeArtifactPath, out var invokeMethodName, out var invokeArguments, out var invokeLlvmRoot))
{
    try
    {
        var result = LoweredAssemblyInvoker.InvokeBitcode(invokeArtifactPath, invokeMethodName, invokeArguments, invokeLlvmRoot);
        Console.WriteLine(result is null ? "<null>" : result);
        return 0;
    }
    catch (NotSupportedException ex)
    {
        Console.Error.WriteLine($"Unsupported IR: {GetInnermostExceptionMessage(ex)}");
        return 4;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(GetInnermostExceptionMessage(ex));
        return 1;
    }
}

if (TryParsePackArguments(args, out var packCratePath, out var packOutputDir, out var packVersion, out var packBuildOptions, out var packLlvmRoot))
{
    try
    {
        var crateName = Path.GetFileName(packCratePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        var assemblyPath = Path.Combine(packOutputDir, $"{crateName}.dll");

        // Translate
        var bitcodePath = RustBitcodeCompiler.BuildBitcode(packCratePath, packBuildOptions);
        var loweredModule = LoweredIrLowerer.LowerBitcode(bitcodePath, packLlvmRoot);
        var emitOptions = CreateEmitOptions(true, false, false, loweredModule);
        LoweredAssemblyEmitter.EmitModule(loweredModule, assemblyPath, emitOptions);

        // Generate .nuspec
        var spec = NuGetPackager.CreatePackSpec(crateName, packVersion, assemblyPath);
        var nuspecContent = NuGetPackager.GenerateNuspec(spec);
        var nuspecPath = Path.Combine(packOutputDir, $"{spec.PackageId}.nuspec");
        File.WriteAllText(nuspecPath, nuspecContent);

        // Produce a real .nupkg alongside the .nuspec.
        var nupkgPath = Path.Combine(packOutputDir, $"{spec.PackageId}.{spec.Version}.nupkg");
        NuGetPackager.WriteNupkg(spec, nupkgPath);

        Console.WriteLine($"Package ID: {spec.PackageId}");
        Console.WriteLine($"Version: {spec.Version}");
        Console.WriteLine($"Assembly: {Path.GetFullPath(assemblyPath)}");
        Console.WriteLine($"Nuspec: {Path.GetFullPath(nuspecPath)}");
        Console.WriteLine($"Nupkg: {Path.GetFullPath(nupkgPath)}");
        Console.WriteLine($"Files: {spec.Files.Count}");
        return 0;
    }
    catch (NotSupportedException ex)
    {
        Console.Error.WriteLine($"Unsupported IR: {ex.Message}");
        return 4;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 1;
    }
}

PrintUsage(null);
return 1;

static EmitOptions CreateEmitOptions(bool emitPdb, bool strict, bool powerShellCmdletBindings, LoweredModule? loweredModule = null)
{
    var options = new EmitOptions { EmitPdb = emitPdb, StrictUnsupportedIr = strict };
    var manifests = new List<BindingManifestDocument>();
    if (powerShellCmdletBindings)
    {
        manifests.Add(ExternalPackageBindingSurfaces.CreatePowerShellCmdletManifest());
    }

    if (loweredModule is not null && RequiresAvaloniaExternalPackageManifest(loweredModule))
    {
        manifests.Add(ExternalPackageBindingSurfaces.CreateAvaloniaHelloManifest());
    }

    return manifests.Count > 0
        ? options with { BindingManifests = manifests }
        : options;
}

static bool RequiresAvaloniaExternalPackageManifest(LoweredModule loweredModule)
    => loweredModule.Functions
        .SelectMany(static function => function.Blocks)
        .SelectMany(static block => block.Instructions)
        .OfType<LoweredCallInstruction>()
        .Any(static call => call.Callee.StartsWith("rustlyn_avalonia_", StringComparison.Ordinal)
            || call.Callee.StartsWith("rustlyn_bindgen_avalonia_", StringComparison.Ordinal));

static bool IsHelpFlag(string argument) =>
    string.Equals(argument, "--help", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(argument, "-h", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(argument, "/?", StringComparison.Ordinal);

static bool IsVersionFlag(string argument) =>
    string.Equals(argument, "--version", StringComparison.OrdinalIgnoreCase) ||
    string.Equals(argument, "-V", StringComparison.Ordinal);

static bool HasHelpFlag(string[] args)
{
    for (var i = 1; i < args.Length; i++)
    {
        if (IsHelpFlag(args[i])) { return true; }
    }
    return false;
}

static void PrintVersion()
{
    var assembly = Assembly.GetExecutingAssembly();
    var informational = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    var version = informational ?? assembly.GetName().Version?.ToString() ?? "unknown";
    Console.WriteLine($"rustlyn {version}");
    Console.WriteLine($"runtime: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
    var resolvedLlvmRoot = LlvmNativeLibraryLocator.TryResolveToolchainRoot(null);
    Console.WriteLine($"llvm-root: {resolvedLlvmRoot ?? "(unset; configure RUSTLYN_LLVM_ROOT)"}");
}

static void PrintUsage(string? command)
{
    if (!string.IsNullOrWhiteSpace(command))
    {
        var help = GetCommandHelp(command);
        if (help is not null)
        {
            Console.WriteLine(help);
            return;
        }
    }

    Console.WriteLine("rustlyn - Rust -> .NET translation toolchain");
    Console.WriteLine();
    Console.WriteLine("Usage: rustlyn <command> [options]");
    Console.WriteLine();
    Console.WriteLine("Build & run:");
    Console.WriteLine("  new        Scaffold a new sample crate");
    Console.WriteLine("  rustc      Build single-file Rust source into bitcode (+ LLVM IR)");
    Console.WriteLine("  cargo      Build a Cargo crate to bitcode + managed assembly");
    Console.WriteLine("  translate  Build a Cargo crate to a managed assembly (custom outputs)");
    Console.WriteLine("  pack       Translate and emit a .nupkg");
    Console.WriteLine("  run        Build a Cargo crate and invoke a method on the assembly");
    Console.WriteLine();
    Console.WriteLine("Lower-level:");
    Console.WriteLine("  emit       Lower a .bc into a managed .dll");
    Console.WriteLine("  lower      Print the lowered IR for a .bc");
    Console.WriteLine("  inspect    Print metadata for a bitcode artifact");
    Console.WriteLine("  invoke     Invoke a method on an already-built bitcode artifact");
    Console.WriteLine();
    Console.WriteLine("Toolchain:");
    Console.WriteLine("  diagnose   Check toolchain (cargo, rustc, llvm root, rustlyn-llvm)");
    Console.WriteLine("  llvm       Forward a command to the native rustlyn-llvm helper");
    Console.WriteLine("  help       Show this banner or per-command help (rustlyn help <command>)");
    Console.WriteLine();
    Console.WriteLine("Global flags:");
    Console.WriteLine("  --help, -h        Show help (per command or top-level)");
    Console.WriteLine("  --version, -V     Show tool version and toolchain info");
}

#pragma warning disable SA1500
static string? GetCommandHelp(string command) => command.ToLowerInvariant() switch
{
    "new" => "Usage: rustlyn new <name> [--lib|--bin] [--in <dir>] [--edition <year>]\n\nScaffolds <dir>/<name>/Cargo.toml + src/lib.rs (or src/main.rs for --bin).\nDefaults: --lib, edition 2024. --in defaults to ./samples if present, else cwd.",
    "rustc" => "Usage: rustlyn rustc <source.rs> [--out-dir <dir>] [--crate-name <name>] [--crate-type <lib|bin>] [--edition <year>] [--emit <bc|ll|bc,ll>] [--panic <abort|unwind>] [--overflow-checks <on|off>]\n\nWraps rustc with rustlyn's standardized flags (panic=abort, overflow-checks=off, edition=2021) and emits .bc/.ll in a single invocation.",
    "cargo" => "Usage: rustlyn cargo [+<toolchain>] build [--manifest-path <Cargo.toml>] [--release] [--bin <name>] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--strict] [--powershell-cmdlet-bindings] [--llvm-root <path>]\n\nBuilds a Cargo crate to bitcode + managed assembly, writing into Cargo's normal target/<profile>/ directory.",
    "translate" => "Usage: rustlyn translate <crate-path> --out <path-to-dll> [--bitcode-out <path-to-bc>] [--bin <name>] [--debug] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--cache <path>] [--strict] [--powershell-cmdlet-bindings] [--llvm-root <path>]\n\nBuilds a Cargo crate and emits a managed assembly at the specified path.",
    "pack" => "Usage: rustlyn pack <crate-path> --out <dir> [--version <semver>] [--llvm-root <path>]\n\nTranslates a crate and produces a .nuspec + .nupkg in the output directory.",
    "run" => "Usage: rustlyn run [<crate-path>] --method <name> [--arg <type:value>]... [--manifest-path <Cargo.toml>] [--release] [--bin <name>] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--strict] [--llvm-root <path>]\n\nBuilds a Cargo crate (like rustlyn cargo build) then invokes a method on the produced assembly. Arg types: i32, i64, u32, u64.",
    "emit" => "Usage: rustlyn emit <path-to-bc> --out <path-to-dll> [--pdb] [--strict] [--powershell-cmdlet-bindings] [--llvm-root <path>]\n\nLowers an existing .bc into a .NET assembly.",
    "lower" => "Usage: rustlyn lower <path-to-bc> [--llvm-root <path>]\n\nPrints the lowered IR dump for a bitcode artifact.",
    "inspect" => "Usage: rustlyn inspect <path-to-bc> [--llvm-root <path>]\n\nPrints bitcode metadata (magic bytes, functions, globals, aliases).",
    "invoke" => "Usage: rustlyn invoke <path-to-bc> --method <name> [--arg <type:value>]... [--llvm-root <path>]\n\nLoads a .bc, lowers it in memory, and invokes a method. Arg types: i32, i64, u32, u64.",
    "diagnose" => "Usage: rustlyn diagnose [--llvm-root <path>]\n\nChecks cargo, rustc, nightly + rust-src, LLVM toolchain root, and the native rustlyn-llvm helper.",
    "llvm" => "Usage: rustlyn llvm <rustlyn-llvm-command> [--llvm-root <path>]\n\nForwards a command line to the native rustlyn-llvm helper located inside the LLVM toolchain.",
    _ => null
};
#pragma warning restore SA1500

static int RunNew(string[] args)
{
    if (args.Length < 2)
    {
        Console.Error.WriteLine(GetCommandHelp("new"));
        return 1;
    }

    string? name = null;
    string? targetDir = null;
    var edition = "2024";
    var isBinary = false;
    var explicitLib = false;

    for (var i = 1; i < args.Length; i++)
    {
        var argument = args[i];
        if (string.Equals(argument, "--bin", StringComparison.OrdinalIgnoreCase)) { isBinary = true; continue; }
        if (string.Equals(argument, "--lib", StringComparison.OrdinalIgnoreCase)) { explicitLib = true; continue; }
        if (string.Equals(argument, "--in", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length) { targetDir = args[++i]; continue; }
        if (string.Equals(argument, "--edition", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length) { edition = args[++i]; continue; }
        if (name is null && !argument.StartsWith("-", StringComparison.Ordinal)) { name = argument; continue; }
        Console.Error.WriteLine($"Unknown argument: {argument}");
        Console.Error.WriteLine(GetCommandHelp("new"));
        return 1;
    }

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.Error.WriteLine("rustlyn new requires a crate name.");
        return 1;
    }

    if (isBinary && explicitLib)
    {
        Console.Error.WriteLine("--lib and --bin are mutually exclusive.");
        return 1;
    }

    foreach (var c in name)
    {
        if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
        {
            Console.Error.WriteLine($"Invalid crate name '{name}'. Use letters, digits, '_' or '-'.");
            return 1;
        }
    }

    if (targetDir is null)
    {
        var samplesHere = Path.Combine(Directory.GetCurrentDirectory(), "samples");
        targetDir = Directory.Exists(samplesHere) ? samplesHere : Directory.GetCurrentDirectory();
    }

    var resolvedTargetDir = Path.GetFullPath(targetDir);
    var crateDir = Path.Combine(resolvedTargetDir, name);
    if (Directory.Exists(crateDir) && Directory.EnumerateFileSystemEntries(crateDir).Any())
    {
        Console.Error.WriteLine($"Target directory already exists and is not empty: {crateDir}");
        return 2;
    }

    Directory.CreateDirectory(Path.Combine(crateDir, "src"));

    var cargoTomlBuilder = new System.Text.StringBuilder();
    cargoTomlBuilder.AppendLine("[package]");
    cargoTomlBuilder.AppendLine($"name = \"{name}\"");
    cargoTomlBuilder.AppendLine("version = \"0.1.0\"");
    cargoTomlBuilder.AppendLine($"edition = \"{edition}\"");
    cargoTomlBuilder.AppendLine();
    if (isBinary)
    {
        cargoTomlBuilder.AppendLine("[[bin]]");
        cargoTomlBuilder.AppendLine($"name = \"{name}\"");
        cargoTomlBuilder.AppendLine("path = \"src/main.rs\"");
    }
    else
    {
        cargoTomlBuilder.AppendLine("[lib]");
        cargoTomlBuilder.AppendLine("crate-type = [\"lib\"]");
    }

    var cargoTomlPath = Path.Combine(crateDir, "Cargo.toml");
    File.WriteAllText(cargoTomlPath, cargoTomlBuilder.ToString());

    string sourcePath;
    if (isBinary)
    {
        sourcePath = Path.Combine(crateDir, "src", "main.rs");
        File.WriteAllText(sourcePath, $"fn main() {{\n    println!(\"hello from {name}\");\n}}\n");
    }
    else
    {
        sourcePath = Path.Combine(crateDir, "src", "lib.rs");
        var safeName = name.Replace('-', '_');
        var stub = "#[unsafe(no_mangle)]\n" +
                   $"pub extern \"C\" fn {safeName}_answer() -> i32 {{\n" +
                   "    42\n" +
                   "}\n";
        File.WriteAllText(sourcePath, stub);
    }

    Console.WriteLine($"Created {(isBinary ? "binary" : "library")} crate '{name}' at:");
    Console.WriteLine($"  {crateDir}");
    Console.WriteLine($"  {Path.GetRelativePath(crateDir, cargoTomlPath)}");
    Console.WriteLine($"  {Path.GetRelativePath(crateDir, sourcePath)}");
    Console.WriteLine();
    Console.WriteLine("Next: rustlyn cargo build --manifest-path " + cargoTomlPath);
    return 0;
}

static int RunRun(string[] args)
{
    if (args.Length < 2)
    {
        Console.Error.WriteLine(GetCommandHelp("run"));
        return 1;
    }

    var cratePath = Directory.GetCurrentDirectory();
    var buildOptions = new RustBitcodeBuildOptions { Release = false, InferBinaryTarget = true };
    string? llvmRoot = null;
    string? methodName = null;
    var invokeArguments = new List<object?>();
    var strict = false;
    var positionalAssigned = false;

    for (var i = 1; i < args.Length; i++)
    {
        var argument = args[i];

        if (string.Equals(argument, "--release", StringComparison.OrdinalIgnoreCase)) { buildOptions = buildOptions with { Release = true }; continue; }
        if (string.Equals(argument, "--strict", StringComparison.OrdinalIgnoreCase)) { strict = true; continue; }
        if (TryReadOptionValue(args, ref i, "--manifest-path", argument, out var manifestPath)) { cratePath = manifestPath; positionalAssigned = true; continue; }
        if (TryReadOptionValue(args, ref i, "--bin", argument, out var binName)) { buildOptions = buildOptions with { BinaryTargetName = binName }; continue; }
        if (TryReadOptionValue(args, ref i, "--toolchain", argument, out var toolchain)) { buildOptions = buildOptions with { Toolchain = toolchain }; continue; }
        if (TryReadOptionValue(args, ref i, "--target", argument, out var target)) { buildOptions = buildOptions with { Target = target }; continue; }
        if (TryReadOptionValue(args, ref i, "--build-std", argument, out var buildStd)) { buildOptions = buildOptions with { BuildStd = buildStd }; continue; }
        if (TryReadOptionValue(args, ref i, "--build-std-features", argument, out var buildStdFeatures)) { buildOptions = buildOptions with { BuildStdFeatures = buildStdFeatures }; continue; }
        if (TryReadOptionValue(args, ref i, "--llvm-root", argument, out var llvm)) { llvmRoot = llvm; continue; }
        if (TryReadOptionValue(args, ref i, "--method", argument, out var method)) { methodName = method; continue; }
        if (string.Equals(argument, "--arg", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length) { invokeArguments.Add(ParseArgumentValue(args[++i])); continue; }
        if (!positionalAssigned && !argument.StartsWith("-", StringComparison.Ordinal)) { cratePath = argument; positionalAssigned = true; continue; }

        Console.Error.WriteLine($"Unknown argument: {argument}");
        Console.Error.WriteLine(GetCommandHelp("run"));
        return 1;
    }

    if (string.IsNullOrWhiteSpace(methodName))
    {
        Console.Error.WriteLine("rustlyn run requires --method <name>.");
        return 1;
    }

    try
    {
        var cargoResult = RustBitcodeCompiler.BuildCargoProject(cratePath, buildOptions);
        var loweredModule = LoweredIrLowerer.LowerBitcode(cargoResult.BitcodePath, llvmRoot);
        var emitOptions = CreateEmitOptions(true, strict, false, loweredModule);
        LoweredAssemblyEmitter.EmitModule(loweredModule, cargoResult.AssemblyPath, emitOptions);

        var result = LoweredAssemblyInvoker.InvokeBitcode(cargoResult.BitcodePath, methodName, invokeArguments, llvmRoot);
        Console.WriteLine(result is null ? "<null>" : result);
        return 0;
    }
    catch (UnsupportedIrException ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 4;
    }
    catch (NotSupportedException ex)
    {
        Console.Error.WriteLine($"Unsupported IR: {GetInnermostExceptionMessage(ex)}");
        return 4;
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("failed with exit code"))
    {
        Console.Error.WriteLine($"Build failed: {ex.Message}");
        return 3;
    }
    catch (FileNotFoundException ex)
    {
        Console.Error.WriteLine($"Tool or artifact missing: {ex.Message}");
        return 2;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(GetInnermostExceptionMessage(ex));
        return 1;
    }
}

static int RunRustc(string[] args)
{
    if (args.Length < 2)
    {
        Console.Error.WriteLine("Usage: rustlyn rustc <source.rs> [--out-dir <dir>] [--crate-name <name>] [--crate-type <lib|bin>] [--edition <year>] [--emit <bc|ll|bc,ll>] [--panic <abort|unwind>] [--overflow-checks <on|off>]");
        return 1;
    }

    string? sourcePath = null;
    string? outDir = null;
    string? crateName = null;
    var crateType = "lib";
    var edition = "2021";
    var emit = "bc,ll";
    var panic = "abort";
    var overflowChecks = "off";

    for (var index = 1; index < args.Length; index++)
    {
        var argument = args[index];
        if (string.Equals(argument, "--out-dir", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { outDir = args[++index]; continue; }
        if (string.Equals(argument, "--crate-name", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { crateName = args[++index]; continue; }
        if (string.Equals(argument, "--crate-type", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { crateType = args[++index]; continue; }
        if (string.Equals(argument, "--edition", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { edition = args[++index]; continue; }
        if (string.Equals(argument, "--emit", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { emit = args[++index]; continue; }
        if (string.Equals(argument, "--panic", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { panic = args[++index]; continue; }
        if (string.Equals(argument, "--overflow-checks", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length) { overflowChecks = args[++index]; continue; }
        if (sourcePath is null && !argument.StartsWith("-", StringComparison.Ordinal)) { sourcePath = argument; continue; }

        Console.Error.WriteLine($"Unknown or misplaced argument: {argument}");
        return 1;
    }

    if (string.IsNullOrWhiteSpace(sourcePath))
    {
        Console.Error.WriteLine("rustlyn rustc requires a source file path.");
        return 1;
    }

    var resolvedSource = Path.GetFullPath(sourcePath);
    if (!File.Exists(resolvedSource))
    {
        Console.Error.WriteLine($"Source file not found: {resolvedSource}");
        return 2;
    }

    crateName ??= Path.GetFileNameWithoutExtension(resolvedSource);
    var resolvedOutDir = Path.GetFullPath(outDir ?? Path.GetDirectoryName(resolvedSource) ?? Directory.GetCurrentDirectory());
    Directory.CreateDirectory(resolvedOutDir);

    var emitKinds = new List<string>();
    foreach (var part in emit.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    {
        var normalized = part.ToLowerInvariant() switch
        {
            "bc" or "llvm-bc" => "llvm-bc",
            "ll" or "llvm-ir" => "llvm-ir",
            _ => null
        };
        if (normalized is null)
        {
            Console.Error.WriteLine($"Unsupported --emit kind: {part} (expected bc, ll, or bc,ll)");
            return 1;
        }
        if (!emitKinds.Contains(normalized)) { emitKinds.Add(normalized); }
    }

    var bitcodePath = Path.Combine(resolvedOutDir, crateName + ".bc");
    var irPath = Path.Combine(resolvedOutDir, crateName + ".ll");

    var startInfo = new ProcessStartInfo
    {
        FileName = "rustc",
        WorkingDirectory = Directory.GetCurrentDirectory(),
        UseShellExecute = false
    };
    startInfo.ArgumentList.Add(resolvedSource);
    startInfo.ArgumentList.Add("--crate-name"); startInfo.ArgumentList.Add(crateName);
    startInfo.ArgumentList.Add("--crate-type"); startInfo.ArgumentList.Add(crateType);
    startInfo.ArgumentList.Add("--edition"); startInfo.ArgumentList.Add(edition);
    startInfo.ArgumentList.Add("-C"); startInfo.ArgumentList.Add($"overflow-checks={overflowChecks}");
    startInfo.ArgumentList.Add("-C"); startInfo.ArgumentList.Add($"panic={panic}");
    startInfo.ArgumentList.Add("--emit"); startInfo.ArgumentList.Add(string.Join(',', emitKinds));
    startInfo.ArgumentList.Add("--out-dir"); startInfo.ArgumentList.Add(resolvedOutDir);

    Process process;
    try
    {
        process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start rustc.");
    }
    catch (System.ComponentModel.Win32Exception ex)
    {
        Console.Error.WriteLine($"rustc not found on PATH: {ex.Message}");
        return 2;
    }

    process.WaitForExit();
    if (process.ExitCode != 0)
    {
        return process.ExitCode;
    }

    foreach (var kind in emitKinds)
    {
        var produced = kind == "llvm-bc" ? bitcodePath : irPath;
        if (!File.Exists(produced))
        {
            Console.Error.WriteLine($"Expected rustc output was not produced: {produced}");
            return 3;
        }
        Console.WriteLine(produced);
    }

    return 0;
}

static int RunLlvm(string[] args)
{
    if (args.Length < 2)
    {
        Console.Error.WriteLine("Usage: rustlyn llvm <rustlyn-llvm-command> [--llvm-root <path>]");
        return 1;
    }

    string? llvmRoot = null;
    var helperArguments = new List<string>();
    for (var index = 1; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        helperArguments.Add(args[index]);
    }

    if (helperArguments.Count == 0)
    {
        Console.Error.WriteLine("Usage: rustlyn llvm <rustlyn-llvm-command> [--llvm-root <path>]");
        return 1;
    }

    var resolvedLlvmRoot = LlvmNativeLibraryLocator.TryResolveToolchainRoot(llvmRoot);
    if (resolvedLlvmRoot is null)
    {
        Console.Error.WriteLine("Set RUSTLYN_LLVM_ROOT or use --llvm-root to point at an LLVM toolchain containing rustlyn-llvm.");
        return 2;
    }

    var helperPath = LlvmNativeLibraryLocator.TryGetToolPath(resolvedLlvmRoot, "rustlyn-llvm.exe");
    if (helperPath is null)
    {
        Console.Error.WriteLine($"Configured LLVM toolchain does not contain rustlyn-llvm: {LlvmNativeLibraryLocator.GetBinPath(resolvedLlvmRoot)}");
        return 2;
    }

    var startInfo = new ProcessStartInfo
    {
        FileName = helperPath,
        WorkingDirectory = Directory.GetCurrentDirectory(),
        UseShellExecute = false
    };

    foreach (var argument in helperArguments)
    {
        startInfo.ArgumentList.Add(argument);
    }

    using var process = Process.Start(startInfo)
        ?? throw new InvalidOperationException($"Failed to start rustlyn-llvm at '{helperPath}'.");
    process.WaitForExit();
    return process.ExitCode;
}

static bool TryParseCargoArguments(string[] args, out string cratePath, out RustBitcodeBuildOptions buildOptions, out string? llvmRoot, out bool strict, out bool powerShellCmdletBindings)
{
    cratePath = Directory.GetCurrentDirectory();
    buildOptions = new RustBitcodeBuildOptions
    {
        Release = false,
        InferBinaryTarget = true
    };
    llvmRoot = null;
    strict = false;
    powerShellCmdletBindings = false;

    if (args.Length < 2 || !string.Equals(args[0], "cargo", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    var commandIndex = 1;
    if (args[commandIndex].StartsWith("+", StringComparison.Ordinal) && args[commandIndex].Length > 1)
    {
        buildOptions = buildOptions with { Toolchain = args[commandIndex][1..] };
        commandIndex++;
    }

    if (commandIndex >= args.Length || !string.Equals(args[commandIndex], "build", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    for (var index = commandIndex + 1; index < args.Length; index++)
    {
        var argument = args[index];
        if (string.Equals(argument, "--release", StringComparison.OrdinalIgnoreCase))
        {
            buildOptions = buildOptions with { Release = true };
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--manifest-path", argument, out var manifestPath))
        {
            cratePath = manifestPath;
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--bin", argument, out var binaryTargetName))
        {
            buildOptions = buildOptions with { BinaryTargetName = binaryTargetName };
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--target", argument, out var target))
        {
            buildOptions = buildOptions with { Target = target };
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--toolchain", argument, out var toolchain))
        {
            buildOptions = buildOptions with { Toolchain = toolchain };
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--llvm-root", argument, out var parsedLlvmRoot))
        {
            llvmRoot = parsedLlvmRoot;
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--build-std", argument, out var buildStd))
        {
            buildOptions = buildOptions with { BuildStd = buildStd };
            continue;
        }

        if (TryReadOptionValue(args, ref index, "--build-std-features", argument, out var buildStdFeatures))
        {
            buildOptions = buildOptions with { BuildStdFeatures = buildStdFeatures };
            continue;
        }

        if (string.Equals(argument, "-Z", StringComparison.Ordinal) && index + 1 < args.Length)
        {
            if (!TryApplyCargoUnstableOption(args[index + 1], buildOptions, out buildOptions))
            {
                return false;
            }

            index++;
            continue;
        }

        if (argument.StartsWith("-Z", StringComparison.Ordinal) && argument.Length > 2)
        {
            if (!TryApplyCargoUnstableOption(argument[2..], buildOptions, out buildOptions))
            {
                return false;
            }

            continue;
        }

        if (string.Equals(argument, "--strict", StringComparison.OrdinalIgnoreCase))
        {
            strict = true;
            continue;
        }

        if (string.Equals(argument, "--powershell-cmdlet-bindings", StringComparison.OrdinalIgnoreCase))
        {
            powerShellCmdletBindings = true;
            continue;
        }

        return false;
    }

    return true;
}

static bool TryReadOptionValue(string[] args, ref int index, string optionName, string argument, out string value)
{
    value = string.Empty;

    if (argument.StartsWith($"{optionName}=", StringComparison.OrdinalIgnoreCase))
    {
        value = argument[(optionName.Length + 1)..];
        return !string.IsNullOrWhiteSpace(value);
    }

    if (!string.Equals(argument, optionName, StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    if (index + 1 >= args.Length || string.IsNullOrWhiteSpace(args[index + 1]))
    {
        return false;
    }

    value = args[index + 1];
    index++;
    return true;
}

static bool TryApplyCargoUnstableOption(string option, RustBitcodeBuildOptions currentOptions, out RustBitcodeBuildOptions updatedOptions)
{
    updatedOptions = currentOptions;
    const string BuildStdPrefix = "build-std=";
    const string BuildStdFeaturesPrefix = "build-std-features=";

    if (option.StartsWith(BuildStdPrefix, StringComparison.Ordinal))
    {
        updatedOptions = currentOptions with { BuildStd = option[BuildStdPrefix.Length..] };
        return !string.IsNullOrWhiteSpace(updatedOptions.BuildStd);
    }

    if (option.StartsWith(BuildStdFeaturesPrefix, StringComparison.Ordinal))
    {
        updatedOptions = currentOptions with { BuildStdFeatures = option[BuildStdFeaturesPrefix.Length..] };
        return !string.IsNullOrWhiteSpace(updatedOptions.BuildStdFeatures);
    }

    return false;
}

static bool TryParseTranslateArguments(string[] args, out string cratePath, out string outputPath, out RustBitcodeBuildOptions buildOptions, out string? llvmRoot, out string? cachePath, out bool strict, out bool powerShellCmdletBindings)
{
    cratePath = string.Empty;
    outputPath = string.Empty;
    buildOptions = new RustBitcodeBuildOptions();
    llvmRoot = null;
    cachePath = null;
    strict = false;
    powerShellCmdletBindings = false;

    if (args.Length < 4 || !string.Equals(args[0], "translate", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    cratePath = args[1];

    for (var index = 2; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--out", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            outputPath = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--bitcode-out", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            buildOptions = buildOptions with { OutputBitcodePath = args[index + 1] };
            index++;
            continue;
        }

        if (string.Equals(args[index], "--bin", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            buildOptions = buildOptions with { BinaryTargetName = args[index + 1] };
            index++;
            continue;
        }

        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--cache", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            cachePath = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--debug", StringComparison.OrdinalIgnoreCase))
        {
            buildOptions = buildOptions with { Release = false };
            continue;
        }

        if (string.Equals(args[index], "--toolchain", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            buildOptions = buildOptions with { Toolchain = args[index + 1] };
            index++;
            continue;
        }

        if (string.Equals(args[index], "--target", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            buildOptions = buildOptions with { Target = args[index + 1] };
            index++;
            continue;
        }

        if (string.Equals(args[index], "--build-std", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            buildOptions = buildOptions with { BuildStd = args[index + 1] };
            index++;
            continue;
        }

        if (string.Equals(args[index], "--build-std-features", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            buildOptions = buildOptions with { BuildStdFeatures = args[index + 1] };
            index++;
            continue;
        }

        if (string.Equals(args[index], "--strict", StringComparison.OrdinalIgnoreCase))
        {
            strict = true;
            continue;
        }

        if (string.Equals(args[index], "--powershell-cmdlet-bindings", StringComparison.OrdinalIgnoreCase))
        {
            powerShellCmdletBindings = true;
            continue;
        }

        return false;
    }

    return !string.IsNullOrWhiteSpace(outputPath);
}

static bool TryParseInvokeArguments(string[] args, out string artifactPath, out string methodName, out List<object?> arguments, out string? llvmRoot)
{
    artifactPath = string.Empty;
    methodName = string.Empty;
    arguments = [];
    llvmRoot = null;

    if (args.Length < 4 || !string.Equals(args[0], "invoke", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    artifactPath = args[1];

    for (var index = 2; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--method", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            methodName = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--arg", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            arguments.Add(ParseArgumentValue(args[index + 1]));
            index++;
            continue;
        }

        return false;
    }

    return !string.IsNullOrWhiteSpace(methodName);
}

static object ParseArgumentValue(string argument)
{
    var separatorIndex = argument.IndexOf(':', StringComparison.Ordinal);
    if (separatorIndex <= 0 || separatorIndex == argument.Length - 1)
    {
        throw new InvalidOperationException($"Argument '{argument}' must use the form <type:value>.");
    }

    var typeName = argument[..separatorIndex];
    var valueText = argument[(separatorIndex + 1)..];
    return typeName switch
    {
        "i32" => int.Parse(valueText),
        "i64" => long.Parse(valueText),
        "u32" => uint.Parse(valueText),
        "u64" => ulong.Parse(valueText),
        _ => throw new InvalidOperationException($"Argument type '{typeName}' is not supported. Use i32, i64, u32, or u64.")
    };
}

static string GetInnermostExceptionMessage(Exception exception)
{
    var current = exception;
    while (current.InnerException is not null)
    {
        current = current.InnerException;
    }

    return current.Message;
}

static bool TryParseEmitArguments(string[] args, out string artifactPath, out string outputPath, out string? llvmRoot, out bool emitPdb, out bool strict, out bool powerShellCmdletBindings)
{
    artifactPath = string.Empty;
    outputPath = string.Empty;
    llvmRoot = null;
    emitPdb = false;
    strict = false;
    powerShellCmdletBindings = false;

    if (args.Length < 4 || !string.Equals(args[0], "emit", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    artifactPath = args[1];

    for (var index = 2; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--out", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            outputPath = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--pdb", StringComparison.OrdinalIgnoreCase))
        {
            emitPdb = true;
            continue;
        }

        if (string.Equals(args[index], "--strict", StringComparison.OrdinalIgnoreCase))
        {
            strict = true;
            continue;
        }

        if (string.Equals(args[index], "--powershell-cmdlet-bindings", StringComparison.OrdinalIgnoreCase))
        {
            powerShellCmdletBindings = true;
            continue;
        }

        return false;
    }

    return !string.IsNullOrWhiteSpace(outputPath);
}

static bool TryParseLowerArguments(string[] args, out string artifactPath, out string? llvmRoot)
{
    artifactPath = string.Empty;
    llvmRoot = null;

    if (args.Length < 2 || !string.Equals(args[0], "lower", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    artifactPath = args[1];

    for (var index = 2; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        return false;
    }

    return true;
}

static bool TryParseInspectArguments(string[] args, out string artifactPath, out string? llvmRoot)
{
    artifactPath = string.Empty;
    llvmRoot = null;

    if (args.Length < 2 || !string.Equals(args[0], "inspect", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    artifactPath = args[1];

    for (var index = 2; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        return false;
    }

    return true;
}

static bool TryParsePackArguments(string[] args, out string cratePath, out string outputDir, out string version, out RustBitcodeBuildOptions buildOptions, out string? llvmRoot)
{
    cratePath = string.Empty;
    outputDir = string.Empty;
    version = "0.1.0";
    buildOptions = new RustBitcodeBuildOptions();
    llvmRoot = null;

    if (args.Length < 4 || !string.Equals(args[0], "pack", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    cratePath = args[1];

    for (var index = 2; index < args.Length; index++)
    {
        if (string.Equals(args[index], "--out", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            outputDir = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--version", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            version = args[index + 1];
            index++;
            continue;
        }

        if (string.Equals(args[index], "--llvm-root", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
        {
            llvmRoot = args[index + 1];
            index++;
            continue;
        }

        return false;
    }

    return !string.IsNullOrWhiteSpace(outputDir);
}

static int RunDiagnose(string[] args)
{
    string? llvmRoot = null;
    for (var i = 1; i < args.Length; i++)
    {
        if (string.Equals(args[i], "--llvm-root", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
        {
            llvmRoot = args[i + 1];
            i++;
        }
    }

    var allGood = true;

    // Check cargo
    Console.Write("cargo: ");
    var cargoVersion = TryRunTool("cargo", ["--version"]);
    if (cargoVersion is not null)
    {
        Console.WriteLine(cargoVersion.Trim());
    }
    else
    {
        Console.WriteLine("NOT FOUND");
        allGood = false;
    }

    // Check rustc
    Console.Write("rustc: ");
    var rustcVersion = TryRunTool("rustc", ["--version"]);
    if (rustcVersion is not null)
    {
        Console.WriteLine(rustcVersion.Trim());
    }
    else
    {
        Console.WriteLine("NOT FOUND");
        allGood = false;
    }

    // Check nightly toolchain (optional)
    Console.Write("nightly: ");
    var nightlyVersion = TryRunTool("rustc", ["+nightly", "--version"]);
    if (nightlyVersion is not null)
    {
        Console.WriteLine(nightlyVersion.Trim());
    }
    else
    {
        Console.WriteLine("not available (needed for --build-std)");
    }

    // Check rust-src for nightly (optional)
    if (nightlyVersion is not null)
    {
        Console.Write("rust-src: ");
        var sysroot = TryRunTool("rustc", ["+nightly", "--print", "sysroot"]);
        if (sysroot is not null)
        {
            var rustSourcePath = Path.Combine(sysroot.Trim(), "lib", "rustlib", "src", "rust", "library");
            if (Directory.Exists(rustSourcePath))
            {
                Console.WriteLine("installed");
            }
            else
            {
                Console.WriteLine("NOT FOUND (install: rustup component add rust-src --toolchain nightly)");
            }
        }
        else
        {
            Console.WriteLine("could not determine sysroot");
        }
    }

    // Check LLVM tooling
    Console.Write("llvm-root: ");
    var resolvedLlvmRoot = LlvmNativeLibraryLocator.TryResolveToolchainRoot(llvmRoot);
    if (resolvedLlvmRoot is not null)
    {
        Console.WriteLine(resolvedLlvmRoot);

        var binPath = LlvmNativeLibraryLocator.GetBinPath(resolvedLlvmRoot);

        var rustlynLlvmPath = LlvmNativeLibraryLocator.TryGetToolPath(resolvedLlvmRoot, "rustlyn-llvm.exe");
        Console.Write("rustlyn-llvm: ");
        if (rustlynLlvmPath is not null)
        {
            Console.WriteLine("found");
        }
        else
        {
            Console.WriteLine("not found (legacy llvm-opt fallback will be used if available)");
        }

        var optPath = LlvmNativeLibraryLocator.TryGetToolPath(resolvedLlvmRoot, "llvm-opt.exe");
        Console.Write("llvm-opt: ");
        if (optPath is not null)
        {
            Console.WriteLine(rustlynLlvmPath is null
                ? "found (legacy fallback; install rustlyn-llvm for structured JSON)"
                : "found (legacy fallback)");
        }
        else
        {
            Console.WriteLine(rustlynLlvmPath is null ? "NOT FOUND" : "not found");
            if (rustlynLlvmPath is null)
            {
                allGood = false;
            }
        }

        var libLlvmName = OperatingSystem.IsWindows() ? "libLLVM.dll" : "libLLVM.so";
        var libLlvmPath = Path.Combine(binPath, libLlvmName);
        Console.Write("libLLVM: ");
        if (File.Exists(libLlvmPath))
        {
            Console.WriteLine("found");
        }
        else
        {
            Console.WriteLine("not found (optional LLVMSharp direct reader unavailable)");
        }

        Console.Write("llvm-reader: ");
        var readerMode = Environment.GetEnvironmentVariable("RUSTLYN_LLVM_READER");
        Console.WriteLine(string.IsNullOrWhiteSpace(readerMode)
            ? "auto (structured helper when supported, text fallback)"
            : readerMode);

        Console.Write("llvm-opt-passes: ");
        var optPasses = Environment.GetEnvironmentVariable("RUSTLYN_LLVM_OPT_PASSES");
        Console.WriteLine(string.IsNullOrWhiteSpace(optPasses)
            ? "off (set RUSTLYN_LLVM_OPT_PASSES, e.g. 'mem2reg,sroa,simplifycfg')"
            : optPasses);
    }
    else
    {
        Console.WriteLine("NOT CONFIGURED");
        Console.WriteLine("  Set RUSTLYN_LLVM_ROOT or use --llvm-root to point at an LLVM toolchain.");
        allGood = false;
    }

    // Check .NET runtime
    Console.Write("dotnet: ");
    var dotnetVersion = TryRunTool("dotnet", ["--version"]);
    if (dotnetVersion is not null)
    {
        Console.WriteLine(dotnetVersion.Trim());
    }
    else
    {
        Console.WriteLine("NOT FOUND");
        allGood = false;
    }

    Console.WriteLine();
    if (allGood)
    {
        Console.WriteLine("Environment OK: all required tools are available.");
        return 0;
    }
    else
    {
        Console.WriteLine("Environment INCOMPLETE: one or more required tools are missing.");
        return 2;
    }
}

static string? TryRunTool(string fileName, string[] arguments)
{
    try
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = fileName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in arguments)
        {
            startInfo.ArgumentList.Add(arg);
        }

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process is null) return null;

        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return process.ExitCode == 0 ? output : null;
    }
    catch
    {
        return null;
    }
}
