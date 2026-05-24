using Rustlyn.Backend;

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

if (TryParseEmitArguments(args, out var emitArtifactPath, out var emitOutputPath, out var emitLlvmRoot))
{
    try
    {
        LoweredAssemblyEmitter.EmitBitcode(emitArtifactPath, emitOutputPath, emitLlvmRoot);
        Console.WriteLine(Path.GetFullPath(emitOutputPath));
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

if (TryParseTranslateArguments(args, out var cratePath, out var translateOutputPath, out var translateBuildOptions, out var translateLlvmRoot))
{
    try
    {
        var bitcodePath = RustBitcodeCompiler.BuildBitcode(cratePath, translateBuildOptions);
        LoweredAssemblyEmitter.EmitBitcode(bitcodePath, translateOutputPath, translateLlvmRoot);
        Console.WriteLine($"Bitcode: {Path.GetFullPath(bitcodePath)}");
        Console.WriteLine($"Assembly: {Path.GetFullPath(translateOutputPath)}");
        return 0;
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

Console.Error.WriteLine("Usage: Rustlyn.Tool inspect <path-to-bc> [--llvm-root <path>]");
Console.Error.WriteLine("   or: Rustlyn.Tool lower <path-to-bc> [--llvm-root <path>]");
Console.Error.WriteLine("   or: Rustlyn.Tool emit <path-to-bc> --out <path-to-dll> [--llvm-root <path>]");
Console.Error.WriteLine("   or: Rustlyn.Tool translate <crate-path> --out <path-to-dll> [--bitcode-out <path-to-bc>] [--bin <name>] [--debug] [--toolchain <name>] [--target <triple-or-json>] [--build-std <components>] [--build-std-features <features>] [--llvm-root <path>]");
Console.Error.WriteLine("   or: Rustlyn.Tool invoke <path-to-bc> --method <name> [--arg <type:value>]... [--llvm-root <path>]   (types: i32, i64, u32, u64)");
Console.Error.WriteLine("   or: Rustlyn.Tool diagnose [--llvm-root <path>]");
return 1;

static bool TryParseTranslateArguments(string[] args, out string cratePath, out string outputPath, out RustBitcodeBuildOptions buildOptions, out string? llvmRoot)
{
    cratePath = string.Empty;
    outputPath = string.Empty;
    buildOptions = new RustBitcodeBuildOptions();
    llvmRoot = null;

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

static bool TryParseEmitArguments(string[] args, out string artifactPath, out string outputPath, out string? llvmRoot)
{
    artifactPath = string.Empty;
    outputPath = string.Empty;
    llvmRoot = null;

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

        var optPath = Path.Combine(binPath, OperatingSystem.IsWindows() ? "llvm-opt.exe" : "llvm-opt");
        Console.Write("llvm-opt: ");
        if (File.Exists(optPath))
        {
            Console.WriteLine("found");
        }
        else
        {
            Console.WriteLine("NOT FOUND");
            allGood = false;
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
            Console.WriteLine("NOT FOUND (bitcode disassembly unavailable)");
        }
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
