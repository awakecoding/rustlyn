namespace Rustlyn.Bindings;

public static class RuntimeCallableCoverageGate
{
    public static RuntimeCallableCoverageReport Measure(BindingManifestDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var namespaces = document.Bindings
            .Select(static binding => ExtractNamespace(binding.ManagedTarget))
            .Where(static ns => ns.Length != 0)
            .GroupBy(static ns => ns, StringComparer.Ordinal)
            .Select(static group => new RuntimeCallableNamespaceCoverage(group.Key, group.Count()))
            .OrderBy(static coverage => coverage.Namespace, StringComparer.Ordinal)
            .ToArray();

        return new RuntimeCallableCoverageReport(
            document.TargetFramework ?? document.RuntimeSurface?.TargetFramework ?? string.Empty,
            document.Bindings.Count,
            namespaces);
    }

    public static RuntimeCallableCoverageGateResult Evaluate(
        BindingManifestDocument document,
        IReadOnlyList<RuntimeCallableCoverageRequirement> requirements)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(requirements);

        var report = Measure(document);
        var counts = report.Namespaces.ToDictionary(static item => item.Namespace, static item => item.CallableBindingCount, StringComparer.Ordinal);
        var diagnostics = new List<RuntimeCallableCoverageDiagnostic>();
        foreach (var requirement in requirements)
        {
            counts.TryGetValue(requirement.Namespace, out var actual);
            if (actual < requirement.MinimumCallableBindings)
            {
                diagnostics.Add(new RuntimeCallableCoverageDiagnostic(
                    requirement.Namespace,
                    requirement.MinimumCallableBindings,
                    actual,
                    $"namespace '{requirement.Namespace}' has {actual.ToString(System.Globalization.CultureInfo.InvariantCulture)} callable bindings; expected at least {requirement.MinimumCallableBindings.ToString(System.Globalization.CultureInfo.InvariantCulture)}"));
            }
        }

        return new RuntimeCallableCoverageGateResult(diagnostics.Count == 0, report, diagnostics);
    }

    private static string ExtractNamespace(string managedTarget)
    {
        const string prefix = "global::";
        if (!managedTarget.StartsWith(prefix, StringComparison.Ordinal))
        {
            return string.Empty;
        }

        var openParen = managedTarget.IndexOf('(', StringComparison.Ordinal);
        var memberTarget = openParen < 0
            ? managedTarget[prefix.Length..]
            : managedTarget[prefix.Length..openParen];
        var memberSeparator = memberTarget.LastIndexOf('.');
        if (memberSeparator <= 0)
        {
            return string.Empty;
        }

        var declaringType = memberTarget[..memberSeparator];
        var typeSeparator = declaringType.LastIndexOf('.');
        return typeSeparator <= 0
            ? string.Empty
            : declaringType[..typeSeparator];
    }
}

public sealed record RuntimeCallableCoverageReport(
    string TargetFramework,
    int CallableBindingCount,
    IReadOnlyList<RuntimeCallableNamespaceCoverage> Namespaces);

public sealed record RuntimeCallableNamespaceCoverage(
    string Namespace,
    int CallableBindingCount);

public sealed record RuntimeCallableCoverageRequirement(
    string Namespace,
    int MinimumCallableBindings);

public sealed record RuntimeCallableCoverageGateResult(
    bool Passed,
    RuntimeCallableCoverageReport Coverage,
    IReadOnlyList<RuntimeCallableCoverageDiagnostic> Diagnostics);

public sealed record RuntimeCallableCoverageDiagnostic(
    string Namespace,
    int MinimumCallableBindings,
    int ActualCallableBindings,
    string Message);
