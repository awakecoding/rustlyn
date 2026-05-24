namespace Rustlyn.Backend;

public static class LlvmNativeLibraryLocator
{
    private const string LlvmRootEnvironmentVariable = "RUSTLYN_LLVM_ROOT";
    private static readonly Lock Sync = new();
    private static string? s_configuredRoot;

    public static string? TryResolveToolchainRoot(string? explicitRoot)
    {
        var resolvedRoot = ResolveRoot(explicitRoot);
        if (resolvedRoot is null)
        {
            return null;
        }

        var fullRoot = Path.GetFullPath(resolvedRoot);
        var binPath = GetBinPath(fullRoot);

        if (!Directory.Exists(binPath))
        {
            throw new DirectoryNotFoundException($"Configured LLVM root does not contain a bin directory: {binPath}");
        }

        return fullRoot;
    }

    public static string? TryConfigure(string? explicitRoot)
    {
        var fullRoot = TryResolveToolchainRoot(explicitRoot);
        if (fullRoot is null)
        {
            return null;
        }

        var libraryPath = Path.Combine(GetBinPath(fullRoot), "libLLVM.dll");
        if (!File.Exists(libraryPath))
        {
            return null;
        }

        lock (Sync)
        {
            if (string.Equals(s_configuredRoot, fullRoot, StringComparison.OrdinalIgnoreCase))
            {
                return s_configuredRoot;
            }

            PrependProcessPath(GetBinPath(fullRoot));
            s_configuredRoot = fullRoot;
            return s_configuredRoot;
        }
    }

    public static string GetBinPath(string toolchainRoot)
    {
        return Directory.Exists(Path.Combine(toolchainRoot, "bin"))
            ? Path.Combine(toolchainRoot, "bin")
            : toolchainRoot;
    }

    public static string GetToolPath(string toolchainRoot, string toolName)
    {
        var toolPath = Path.Combine(GetBinPath(toolchainRoot), toolName);
        if (!File.Exists(toolPath))
        {
            throw new FileNotFoundException($"Configured LLVM toolchain does not contain {toolName}.", toolPath);
        }

        return toolPath;
    }

    private static string? ResolveRoot(string? explicitRoot)
    {
        if (!string.IsNullOrWhiteSpace(explicitRoot))
        {
            return explicitRoot;
        }

        var environmentRoot = Environment.GetEnvironmentVariable(LlvmRootEnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(environmentRoot))
        {
            return environmentRoot;
        }

        var workspaceRoot = FindWorkspaceRoot();
        if (workspaceRoot is null)
        {
            return null;
        }

        var defaultRoot = Path.Combine(workspaceRoot, "artifacts", "toolchains", "llvm", "clang+llvm-20.1.8-x86_64-windows");
        return Directory.Exists(defaultRoot) ? defaultRoot : null;
    }

    private static string? FindWorkspaceRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git"))
                || File.Exists(Path.Combine(directory.FullName, "README.md")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }

    private static void PrependProcessPath(string directory)
    {
        var currentPath = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        var separator = Path.PathSeparator.ToString();
        var entries = currentPath.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (entries.Any(entry => string.Equals(entry, directory, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        var updatedPath = string.IsNullOrEmpty(currentPath)
            ? directory
            : string.Concat(directory, separator, currentPath);

        Environment.SetEnvironmentVariable("PATH", updatedPath);
    }
}
