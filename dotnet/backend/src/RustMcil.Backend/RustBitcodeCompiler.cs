using System.Diagnostics;
using System.Text.Json;

namespace RustMcil.Backend;

public static class RustBitcodeCompiler
{
    public static string BuildLibraryBitcode(string cratePath, bool release = true, string? outputBitcodePath = null)
        => BuildLibraryBitcode(
            cratePath,
            new RustBitcodeBuildOptions
            {
                Release = release,
                OutputBitcodePath = outputBitcodePath
            });

    public static string BuildLibraryBitcode(string cratePath, RustBitcodeBuildOptions? options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cratePath);

        options ??= new RustBitcodeBuildOptions();
        ValidateOptions(options);

        var manifestPath = ResolveManifestPath(cratePath);
        var cargoMetadata = ReadCargoMetadata(manifestPath, options.Toolchain);
        var buildArguments = CreateBuildArguments(manifestPath, options);

        RunProcess(
            fileName: "cargo",
            arguments: buildArguments,
            workingDirectory: Path.GetDirectoryName(manifestPath) ?? Directory.GetCurrentDirectory());

        var artifactPath = FindBitcodeArtifact(cargoMetadata.TargetDirectory, options.Release, cargoMetadata.LibraryTargetName, options.Target);
        if (string.IsNullOrWhiteSpace(options.OutputBitcodePath))
        {
            return artifactPath;
        }

        var outputFullPath = Path.GetFullPath(options.OutputBitcodePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath) ?? throw new InvalidOperationException("The bitcode output directory could not be determined."));
        File.Copy(artifactPath, outputFullPath, overwrite: true);
        return outputFullPath;
    }

    private static void ValidateOptions(RustBitcodeBuildOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.BuildStd) && string.IsNullOrWhiteSpace(options.Toolchain))
        {
            throw new InvalidOperationException("build-std requires an explicit cargo toolchain such as 'nightly'. Configure --toolchain when using --build-std.");
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
            manifestPath,
            "--lib"
        ]);

        if (options.Release)
        {
            buildArguments.Add("--release");
        }

        if (!string.IsNullOrWhiteSpace(options.BuildStd))
        {
            buildArguments.Add("-Z");
            buildArguments.Add($"build-std={options.BuildStd}");
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
            "llvm-bc",
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

    private static CargoMetadata ReadCargoMetadata(string manifestPath, string? toolchain)
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

        var libraryTarget = package.GetProperty("targets")
            .EnumerateArray()
            .FirstOrDefault(targetElement =>
                targetElement.GetProperty("kind")
                    .EnumerateArray()
                    .Any(kindElement => string.Equals(kindElement.GetString(), "lib", StringComparison.Ordinal)));

        if (libraryTarget.ValueKind == JsonValueKind.Undefined)
        {
            throw new InvalidOperationException($"Cargo package '{manifestPath}' does not expose a library target yet. The current translate command only supports crates with a [lib] target.");
        }

        var targetDirectory = root.GetProperty("target_directory").GetString();
        var targetName = libraryTarget.GetProperty("name").GetString();
        if (string.IsNullOrWhiteSpace(targetDirectory) || string.IsNullOrWhiteSpace(targetName))
        {
            throw new InvalidOperationException($"Cargo metadata for '{manifestPath}' was missing target directory or library target name.");
        }

        return new CargoMetadata(Path.GetFullPath(targetDirectory), targetName);
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
        var candidates = Directory.GetFiles(depsDirectory, $"{normalizedTargetName}-*.bc", SearchOption.TopDirectoryOnly);
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

    private sealed record CargoMetadata(string TargetDirectory, string LibraryTargetName);
}