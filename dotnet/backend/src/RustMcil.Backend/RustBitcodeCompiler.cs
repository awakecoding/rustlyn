using System.Diagnostics;
using System.Text.Json;

namespace RustMcil.Backend;

public static class RustBitcodeCompiler
{
    public static string BuildLibraryBitcode(string cratePath, bool release = true, string? outputBitcodePath = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cratePath);

        var manifestPath = ResolveManifestPath(cratePath);
        var cargoMetadata = ReadCargoMetadata(manifestPath);

        var buildArguments = new List<string>
        {
            "rustc",
            "--manifest-path",
            manifestPath,
            "--lib"
        };

        if (release)
        {
            buildArguments.Add("--release");
        }

        buildArguments.AddRange(
        [
            "--",
            "--emit",
            "llvm-bc",
            "-C",
            "overflow-checks=off",
            "-C",
            "panic=abort"
        ]);

        RunProcess(
            fileName: "cargo",
            arguments: buildArguments,
            workingDirectory: Path.GetDirectoryName(manifestPath) ?? Directory.GetCurrentDirectory());

        var artifactPath = FindBitcodeArtifact(cargoMetadata.TargetDirectory, release, cargoMetadata.LibraryTargetName);
        if (string.IsNullOrWhiteSpace(outputBitcodePath))
        {
            return artifactPath;
        }

        var outputFullPath = Path.GetFullPath(outputBitcodePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFullPath) ?? throw new InvalidOperationException("The bitcode output directory could not be determined."));
        File.Copy(artifactPath, outputFullPath, overwrite: true);
        return outputFullPath;
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

    private static CargoMetadata ReadCargoMetadata(string manifestPath)
    {
        var metadataJson = RunProcess(
            fileName: "cargo",
            arguments: ["metadata", "--format-version", "1", "--no-deps", "--manifest-path", manifestPath],
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

    private static string FindBitcodeArtifact(string targetDirectory, bool release, string libraryTargetName)
    {
        var profileDirectory = release ? "release" : "debug";
        var depsDirectory = Path.Combine(targetDirectory, profileDirectory, "deps");
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

        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Command '{fileName} {string.Join(' ', arguments)}' failed with exit code {process.ExitCode}.{Environment.NewLine}{standardError.Trim()}".TrimEnd());
        }

        return standardOutput;
    }

    private sealed record CargoMetadata(string TargetDirectory, string LibraryTargetName);
}