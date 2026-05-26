namespace Rustlyn.Bindings;

public static class TaskFutureProjectionPolicy
{
    private static readonly string[] TaskLikeGenericPrefixes =
    [
        "System.Threading.Tasks.Task`1",
        "System.Threading.Tasks.ValueTask`1"
    ];

    public static TaskFutureProjectionAnalysis AnalyzeReturnType(Type returnType)
    {
        ArgumentNullException.ThrowIfNull(returnType);

        if (string.Equals(returnType.FullName, "System.Threading.Tasks.Task", StringComparison.Ordinal)
            || string.Equals(returnType.FullName, "System.Threading.Tasks.ValueTask", StringComparison.Ordinal))
        {
            return new TaskFutureProjectionAnalysis(
                IsTaskLike: true,
                IsGeneric: false,
                ResultType: null,
                ResultTypeIsBindable: true,
                FutureKind: "future-void");
        }

        if (returnType.IsGenericType
            && returnType.GetGenericTypeDefinition() is { } genericDefinition
            && (string.Equals(genericDefinition.FullName, "System.Threading.Tasks.Task`1", StringComparison.Ordinal)
                || string.Equals(genericDefinition.FullName, "System.Threading.Tasks.ValueTask`1", StringComparison.Ordinal)))
        {
            var resultType = returnType.GetGenericArguments()[0];
            return new TaskFutureProjectionAnalysis(
                IsTaskLike: true,
                IsGeneric: true,
                ResultType: BindingManifestFormatting.FormatTypeName(resultType),
                ResultTypeIsBindable: BindingSurfaceScanner.IsTypeBindable(resultType),
                FutureKind: "future-result");
        }

        return new TaskFutureProjectionAnalysis(
            IsTaskLike: false,
            IsGeneric: false,
            ResultType: null,
            ResultTypeIsBindable: false,
            FutureKind: "not-task");
    }

    public static TaskFutureProjectionAnalysis AnalyzeReturnType(string returnType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(returnType);

        if (string.Equals(returnType, "System.Threading.Tasks.Task", StringComparison.Ordinal)
            || string.Equals(returnType, "System.Threading.Tasks.ValueTask", StringComparison.Ordinal))
        {
            return new TaskFutureProjectionAnalysis(
                IsTaskLike: true,
                IsGeneric: false,
                ResultType: null,
                ResultTypeIsBindable: true,
                FutureKind: "future-void");
        }

        if (TaskLikeGenericPrefixes.Any(prefix => returnType.StartsWith(prefix, StringComparison.Ordinal))
            || returnType.StartsWith("System.Threading.Tasks.Task<", StringComparison.Ordinal)
            || returnType.StartsWith("System.Threading.Tasks.ValueTask<", StringComparison.Ordinal))
        {
            var resultType = ExtractGenericResultType(returnType);
            return new TaskFutureProjectionAnalysis(
                IsTaskLike: true,
                IsGeneric: true,
                ResultType: resultType,
                ResultTypeIsBindable: resultType is not null && IsBindableResultTypeName(resultType),
                FutureKind: "future-result");
        }

        return new TaskFutureProjectionAnalysis(
            IsTaskLike: false,
            IsGeneric: false,
            ResultType: null,
            ResultTypeIsBindable: false,
            FutureKind: "not-task");
    }

    public static string CreateDeferredReason(TaskFutureProjectionAnalysis analysis)
    {
        ArgumentNullException.ThrowIfNull(analysis);
        if (!analysis.IsTaskLike)
        {
            throw new ArgumentException("Only task-like return analyses can be converted to a deferred future reason.", nameof(analysis));
        }

        return analysis.IsGeneric
            ? analysis.ResultTypeIsBindable
                ? $"task return type awaits Rust Future bridge prototype for result '{analysis.ResultType}'"
                : $"task return type has unsupported future result '{analysis.ResultType}'"
            : "task return type awaits Rust Future bridge prototype for completion";
    }

    private static string? ExtractGenericResultType(string returnType)
    {
        var doubleBracketStart = returnType.IndexOf("[[", StringComparison.Ordinal);
        if (doubleBracketStart >= 0)
        {
            var resultStart = doubleBracketStart + 2;
            var resultEnd = returnType.IndexOf(',', resultStart);
            if (resultEnd < 0)
            {
                resultEnd = returnType.IndexOf("]]", resultStart, StringComparison.Ordinal);
            }

            return resultEnd > resultStart ? returnType[resultStart..resultEnd] : null;
        }

        var angleStart = returnType.IndexOf('<');
        if (angleStart >= 0)
        {
            var angleEnd = returnType.LastIndexOf('>');
            return angleEnd > angleStart + 1 ? returnType[(angleStart + 1)..angleEnd] : null;
        }

        var bracketStart = returnType.IndexOf('[');
        if (bracketStart >= 0)
        {
            var bracketEnd = returnType.LastIndexOf(']');
            return bracketEnd > bracketStart + 1 ? returnType[(bracketStart + 1)..bracketEnd] : null;
        }

        return null;
    }

    private static bool IsBindableResultTypeName(string typeName)
        => typeName is "System.Int32"
            or "System.Int64"
            or "System.Single"
            or "System.Double"
            or "System.Boolean"
            or "System.String"
            or "System.Int32[]"
            or "System.Byte[]"
            or "System.String[]";
}

public sealed record TaskFutureProjectionAnalysis(
    bool IsTaskLike,
    bool IsGeneric,
    string? ResultType,
    bool ResultTypeIsBindable,
    string FutureKind);
