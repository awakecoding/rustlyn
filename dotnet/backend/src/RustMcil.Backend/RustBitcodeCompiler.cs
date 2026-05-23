using System.Diagnostics;
using System.Text.Json;

namespace RustMcil.Backend;

public static class RustBitcodeCompiler
{
    public static string BuildLibraryBitcode(string cratePath, bool release = true, string? outputBitcodePath = null)
        => BuildBitcode(
            cratePath,
            new RustBitcodeBuildOptions
            {
                Release = release,
                OutputBitcodePath = outputBitcodePath
            });

    public static string BuildLibraryBitcode(string cratePath, RustBitcodeBuildOptions? options)
        => BuildBitcode(cratePath, options);

    public static string BuildBinaryBitcode(string cratePath, string binaryTargetName, bool release = true, string? outputBitcodePath = null)
        => BuildBitcode(
            cratePath,
            new RustBitcodeBuildOptions
            {
                Release = release,
                OutputBitcodePath = outputBitcodePath,
                BinaryTargetName = binaryTargetName
            });

    public static string BuildBitcode(string cratePath, RustBitcodeBuildOptions? options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cratePath);

        options ??= new RustBitcodeBuildOptions();
        options = NormalizeOptions(options);
        ValidateOptions(options);

        var manifestPath = ResolveManifestPath(cratePath);
        var crateDirectory = Path.GetDirectoryName(manifestPath) ?? Directory.GetCurrentDirectory();
        ValidateTargetPath(options, crateDirectory);
        PreflightBuildStd(options, crateDirectory);
        var cargoMetadata = ReadCargoMetadata(manifestPath, options.Toolchain, options.BinaryTargetName);
        var buildArguments = CreateCargoRustcArguments(manifestPath, options);

        string? buildFailure = null;

        try
        {
            RunProcess(
                fileName: "cargo",
                arguments: buildArguments,
                workingDirectory: Path.GetDirectoryName(manifestPath) ?? Directory.GetCurrentDirectory());
        }
        catch (InvalidOperationException ex) when (!string.IsNullOrWhiteSpace(options.BinaryTargetName))
        {
            buildFailure = ex.Message;
        }

        string artifactPath;
        try
        {
            artifactPath = FindBitcodeArtifact(cargoMetadata.TargetDirectory, options.Release, cargoMetadata.TargetName, options.Target);
        }
        catch when (buildFailure is not null)
        {
            throw new InvalidOperationException(buildFailure);
        }

        if (string.IsNullOrWhiteSpace(options.OutputBitcodePath))
        {
            return artifactPath;
        }

        var outputFullPath = Path.GetFullPath(options.OutputBitcodePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath) ?? throw new InvalidOperationException("The bitcode output directory could not be determined."));
        File.Copy(artifactPath, outputFullPath, overwrite: true);
        CopySiblingLlvmIrArtifact(artifactPath, outputFullPath);
        return outputFullPath;
    }

    public static IReadOnlyList<string> CreateCargoRustcArguments(string manifestPath, RustBitcodeBuildOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(manifestPath);
        options = NormalizeOptions(options);
        ValidateOptions(options);

        return CreateBuildArguments(manifestPath, options);
    }

    private static void ValidateOptions(RustBitcodeBuildOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.BuildStd) && string.IsNullOrWhiteSpace(options.Toolchain))
        {
            throw new InvalidOperationException("build-std requires an explicit cargo toolchain such as 'nightly'. Configure --toolchain when using --build-std.");
        }

        if (!string.IsNullOrWhiteSpace(options.BuildStdFeatures) && string.IsNullOrWhiteSpace(options.BuildStd))
        {
            throw new InvalidOperationException("build-std-features requires --build-std. Configure --build-std when using --build-std-features.");
        }

        if (options.BinaryTargetName is not null && string.IsNullOrWhiteSpace(options.BinaryTargetName))
        {
            throw new InvalidOperationException("Binary target names must be non-empty when --bin is specified.");
        }
    }

    private static RustBitcodeBuildOptions NormalizeOptions(RustBitcodeBuildOptions options)
    {
        var normalizedToolchain = NormalizeToolchainName(options.Toolchain);
        return string.Equals(normalizedToolchain, options.Toolchain, StringComparison.Ordinal)
            ? options
            : options with { Toolchain = normalizedToolchain };
    }

    private static string? NormalizeToolchainName(string? toolchain)
    {
        if (string.IsNullOrWhiteSpace(toolchain))
        {
            return null;
        }

        var normalized = toolchain.Trim();
        while (normalized.StartsWith("+", StringComparison.Ordinal))
        {
            normalized = normalized[1..];
        }

        return normalized;
    }

    private static void ValidateTargetPath(RustBitcodeBuildOptions options, string crateDirectory)
    {
        if (string.IsNullOrWhiteSpace(options.Target)
            || !Path.GetExtension(options.Target).Equals(".json", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var targetPath = Path.IsPathFullyQualified(options.Target)
            ? options.Target
            : Path.Combine(crateDirectory, options.Target);

        if (!File.Exists(targetPath))
        {
            throw new FileNotFoundException($"Rust target JSON '{options.Target}' was not found.", targetPath);
        }
    }

    private static void PreflightBuildStd(RustBitcodeBuildOptions options, string workingDirectory)
    {
        if (string.IsNullOrWhiteSpace(options.BuildStd))
        {
            return;
        }

        var sysroot = RunProcess(
            fileName: "rustc",
            arguments: [$"+{options.Toolchain}", "--print", "sysroot"],
            workingDirectory: workingDirectory).Trim();

        if (string.IsNullOrWhiteSpace(sysroot))
        {
            throw new InvalidOperationException($"rustc +{options.Toolchain} --print sysroot did not return a sysroot path.");
        }

        var rustSourcePath = Path.Combine(sysroot, "lib", "rustlib", "src", "rust", "library");
        if (!Directory.Exists(rustSourcePath))
        {
            throw new InvalidOperationException($"build-std requires the rust-src component for toolchain '{options.Toolchain}'. Install it with: rustup component add rust-src --toolchain {options.Toolchain}.");
        }
    }

    private static List<string> CreateBuildArguments(string manifestPath, RustBitcodeBuildOptions options)
    {
        var buildArguments = new List<string>();

        if (!string.IsNullOrWhiteSpace(options.Toolchain))
        {
            buildArguments.Add($"+{options.Toolchain}");
        }

        buildArguments.AddRange(
        [
            "rustc",
            "--manifest-path",
            manifestPath
        ]);

        if (string.IsNullOrWhiteSpace(options.BinaryTargetName))
        {
            buildArguments.Add("--lib");
        }
        else
        {
            buildArguments.Add("--bin");
            buildArguments.Add(options.BinaryTargetName);
        }

        if (options.Release)
        {
            buildArguments.Add("--release");
        }

        if (!string.IsNullOrWhiteSpace(options.BuildStd))
        {
            buildArguments.Add("-Z");
            buildArguments.Add($"build-std={options.BuildStd}");
        }

        if (!string.IsNullOrWhiteSpace(options.BuildStdFeatures))
        {
            buildArguments.Add("-Z");
            buildArguments.Add($"build-std-features={options.BuildStdFeatures}");
        }

        if (!string.IsNullOrWhiteSpace(options.Target))
        {
            buildArguments.Add("--target");
            buildArguments.Add(options.Target);
        }

        buildArguments.AddRange(
        [
            "--",
            "--emit",
            "llvm-bc,llvm-ir",
            "-C",
            "overflow-checks=off"
        ]);

        if (options.PanicAbort)
        {
            buildArguments.Add("-C");
            buildArguments.Add("panic=abort");
        }

        return buildArguments;
    }

    private static string ResolveManifestPath(string cratePath)
    {
        var candidatePath = Path.GetFullPath(cratePath);
        if (File.Exists(candidatePath))
        {
            if (!string.Equals(Path.GetFileName(candidatePath), "Cargo.toml", StringComparison.OrdinalIgnoreCase))
            {
                throw new FileNotFoundException($"Expected a Cargo.toml manifest or a crate directory, but got '{cratePath}'.", candidatePath);
            }

            return candidatePath;
        }

        if (Directory.Exists(candidatePath))
        {
            var manifestPath = Path.Combine(candidatePath, "Cargo.toml");
            if (File.Exists(manifestPath))
            {
                return manifestPath;
            }
        }

        throw new FileNotFoundException($"Cargo manifest not found for crate path '{cratePath}'.", candidatePath);
    }

    private static CargoMetadata ReadCargoMetadata(string manifestPath, string? toolchain, string? binaryTargetName)
    {
        var metadataArguments = new List<string>();
        if (!string.IsNullOrWhiteSpace(toolchain))
        {
            metadataArguments.Add($"+{toolchain}");
        }

        metadataArguments.AddRange(["metadata", "--format-version", "1", "--no-deps", "--manifest-path", manifestPath]);

        var metadataJson = RunProcess(
            fileName: "cargo",
            arguments: metadataArguments,
            workingDirectory: Path.GetDirectoryName(manifestPath) ?? Directory.GetCurrentDirectory());

        using var document = JsonDocument.Parse(metadataJson);
        var root = document.RootElement;
        var manifestFullPath = Path.GetFullPath(manifestPath);

        var package = root.GetProperty("packages")
            .EnumerateArray()
            .FirstOrDefault(packageElement =>
                string.Equals(
                    Path.GetFullPath(packageElement.GetProperty("manifest_path").GetString() ?? string.Empty),
                    manifestFullPath,
                    StringComparison.OrdinalIgnoreCase));

        if (package.ValueKind == JsonValueKind.Undefined)
        {
            throw new InvalidOperationException($"Cargo metadata did not contain a package entry for manifest '{manifestPath}'.");
        }

        var selectedTarget = SelectTarget(package, manifestPath, binaryTargetName);

        var targetDirectory = root.GetProperty("target_directory").GetString();
        var targetName = selectedTarget.GetProperty("name").GetString();
        if (string.IsNullOrWhiteSpace(targetDirectory) || string.IsNullOrWhiteSpace(targetName))
        {
            throw new InvalidOperationException($"Cargo metadata for '{manifestPath}' was missing target directory or target name.");
        }

        return new CargoMetadata(Path.GetFullPath(targetDirectory), targetName);
    }

    private static JsonElement SelectTarget(JsonElement package, string manifestPath, string? binaryTargetName)
    {
        var targets = package.GetProperty("targets").EnumerateArray();

        if (string.IsNullOrWhiteSpace(binaryTargetName))
        {
            var libraryTarget = targets.FirstOrDefault(targetElement =>
                targetElement.GetProperty("kind")
                    .EnumerateArray()
                    .Any(static kindElement =>
                    {
                        var kind = kindElement.GetString();
                        return string.Equals(kind, "lib", StringComparison.Ordinal)
                            || string.Equals(kind, "staticlib", StringComparison.Ordinal)
                            || string.Equals(kind, "cdylib", StringComparison.Ordinal);
                    }));

            if (libraryTarget.ValueKind != JsonValueKind.Undefined)
            {
                return libraryTarget;
            }

            throw new InvalidOperationException($"Cargo package '{manifestPath}' does not expose a library target yet. The current translate command only supports crates with a [lib] target unless --bin <name> is specified.");
        }

        var binaryTarget = targets.FirstOrDefault(targetElement =>
            string.Equals(targetElement.GetProperty("name").GetString(), binaryTargetName, StringComparison.Ordinal)
            && targetElement.GetProperty("kind")
                .EnumerateArray()
                .Any(kindElement => string.Equals(kindElement.GetString(), "bin", StringComparison.Ordinal)));

        if (binaryTarget.ValueKind != JsonValueKind.Undefined)
        {
            return binaryTarget;
        }

        throw new InvalidOperationException($"Cargo package '{manifestPath}' does not expose a binary target named '{binaryTargetName}'.");
    }

    private static string FindBitcodeArtifact(string targetDirectory, bool release, string libraryTargetName, string? target)
    {
        var profileDirectory = release ? "release" : "debug";
        var depsDirectory = string.IsNullOrWhiteSpace(target)
            ? Path.Combine(targetDirectory, profileDirectory, "deps")
            : Path.Combine(targetDirectory, ResolveTargetDirectoryName(target), profileDirectory, "deps");
        if (!Directory.Exists(depsDirectory))
        {
            throw new DirectoryNotFoundException($"Cargo target directory '{depsDirectory}' was not produced.");
        }

        var normalizedTargetName = libraryTargetName.Replace('-', '_');
        var exactCandidate = Path.Combine(depsDirectory, $"{normalizedTargetName}.bc");
        var candidates = Directory.GetFiles(depsDirectory, $"{normalizedTargetName}-*.bc", SearchOption.TopDirectoryOnly)
            .Concat(File.Exists(exactCandidate) ? [exactCandidate] : [])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (candidates.Length == 0)
        {
            throw new FileNotFoundException($"No LLVM bitcode artifact matching '{normalizedTargetName}-*.bc' was found in '{depsDirectory}'.");
        }

        return candidates
            .Select(path => new FileInfo(path))
            .OrderByDescending(fileInfo => fileInfo.LastWriteTimeUtc)
            .Select(fileInfo => fileInfo.FullName)
            .First();
    }

    private static void CopySiblingLlvmIrArtifact(string artifactPath, string outputBitcodePath)
    {
        var sourceLlvmIrPath = Path.ChangeExtension(artifactPath, ".ll");
        if (!File.Exists(sourceLlvmIrPath))
        {
            return;
        }

        File.Copy(sourceLlvmIrPath, Path.ChangeExtension(outputBitcodePath, ".ll"), overwrite: true);
    }

    private static string ResolveTargetDirectoryName(string target)
    {
        if (Path.GetExtension(target).Equals(".json", StringComparison.OrdinalIgnoreCase))
        {
            return Path.GetFileNameWithoutExtension(target);
        }

        return target;
    }

    private static string RunProcess(string fileName, IReadOnlyList<string> arguments, string workingDirectory)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Failed to start process '{fileName}'.");

        var standardOutputTask = process.StandardOutput.ReadToEndAsync();
        var standardErrorTask = process.StandardError.ReadToEndAsync();
        process.WaitForExit();
        Task.WaitAll(standardOutputTask, standardErrorTask);

        var standardOutput = standardOutputTask.GetAwaiter().GetResult();
        var standardError = standardErrorTask.GetAwaiter().GetResult();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Command '{fileName} {string.Join(' ', arguments)}' failed with exit code {process.ExitCode}.{Environment.NewLine}{standardError.Trim()}".TrimEnd());
        }

        return standardOutput;
    }

    private sealed record CargoMetadata(string TargetDirectory, string TargetName);
}
