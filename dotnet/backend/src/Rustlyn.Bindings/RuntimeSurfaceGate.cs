using System.Globalization;
using System.Text;

namespace Rustlyn.Bindings;

public static class RuntimeSurfaceGate
{
    private static readonly HashSet<string> ValidMemberStatuses = new(StringComparer.Ordinal)
    {
        "projected",
        "rejected",
        "deferred"
    };

    public static RuntimeSurfaceGateResult Evaluate(RuntimeSurfaceScanSet scanSet)
    {
        ArgumentNullException.ThrowIfNull(scanSet);

        var diagnostics = new List<RuntimeSurfaceGateDiagnostic>();
        if (scanSet.Reports.Count == 0)
        {
            diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                "no-runtime-reports",
                "No runtime reference packs were scanned."));
        }

        foreach (var missing in scanSet.MissingTargets)
        {
            diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                "missing-reference-pack",
                $"Missing reference pack for {missing.TargetFramework}."));
        }

        foreach (var report in scanSet.Reports)
        {
            if (report.AssemblyCount == 0)
            {
                diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                    "empty-runtime-report",
                    $"{report.TargetFramework} did not scan any assemblies."));
            }

            foreach (var assembly in report.Assemblies)
            {
                if (!string.IsNullOrWhiteSpace(assembly.LoadDiagnostic))
                {
                    diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                        "assembly-load-diagnostic",
                        $"{report.TargetFramework} {assembly.AssemblyName}: {assembly.LoadDiagnostic}"));
                }
            }

            foreach (var type in report.Types)
            {
                if (string.IsNullOrWhiteSpace(type.ProjectionStatus))
                {
                    diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                        "missing-type-status",
                        $"{report.TargetFramework} {type.FullName} has no projection status."));
                }

                foreach (var member in type.Members)
                {
                    if (!ValidMemberStatuses.Contains(member.ProjectionStatus))
                    {
                        diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                            "invalid-member-status",
                            $"{report.TargetFramework} {member.DisplayName} has invalid projection status '{member.ProjectionStatus}'."));
                        continue;
                    }

                    if (!string.Equals(member.ProjectionStatus, "projected", StringComparison.Ordinal)
                        && string.IsNullOrWhiteSpace(member.UnsupportedReasonCode))
                    {
                        diagnostics.Add(new RuntimeSurfaceGateDiagnostic(
                            "missing-member-diagnostic",
                            $"{report.TargetFramework} {member.DisplayName} is {member.ProjectionStatus} without a reason code."));
                    }
                }
            }
        }

        return new RuntimeSurfaceGateResult(
            Passed: diagnostics.Count == 0,
            ReportsChecked: scanSet.Reports.Count,
            Diagnostics: diagnostics);
    }

    public static string GenerateText(RuntimeSurfaceGateResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var builder = new StringBuilder();
        builder.AppendLine(result.Passed ? "Runtime surface gate: passed" : "Runtime surface gate: failed");
        builder.AppendLine($"Reports checked: {result.ReportsChecked.ToString(CultureInfo.InvariantCulture)}");
        builder.AppendLine($"Diagnostics: {result.Diagnostics.Count.ToString(CultureInfo.InvariantCulture)}");
        foreach (var diagnostic in result.Diagnostics)
        {
            builder.AppendLine($"{diagnostic.Code}: {diagnostic.Message}");
        }

        return builder.ToString();
    }
}

public sealed record RuntimeSurfaceGateResult(
    bool Passed,
    int ReportsChecked,
    IReadOnlyList<RuntimeSurfaceGateDiagnostic> Diagnostics);

public sealed record RuntimeSurfaceGateDiagnostic(string Code, string Message);
