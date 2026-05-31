using System.Management.Automation;

namespace Rustlyn.PowerShellSupport;

public sealed class PowerShellErrorRecord
{
    public PowerShellErrorRecord(
        string message,
        string fullyQualifiedErrorId,
        ErrorCategory category,
        object? targetObject = null,
        string? recommendedAction = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullyQualifiedErrorId);

        Message = message;
        FullyQualifiedErrorId = fullyQualifiedErrorId;
        Category = category;
        TargetObject = targetObject;
        RecommendedAction = recommendedAction;
    }

    public string Message { get; }

    public string FullyQualifiedErrorId { get; }

    public ErrorCategory Category { get; }

    public object? TargetObject { get; }

    public string? RecommendedAction { get; }

    public ErrorRecord ToErrorRecord()
    {
        var record = new ErrorRecord(
            new RuntimeException(Message),
            FullyQualifiedErrorId,
            Category,
            TargetObject);

        if (!string.IsNullOrWhiteSpace(RecommendedAction))
        {
            record.ErrorDetails = new ErrorDetails(Message)
            {
                RecommendedAction = RecommendedAction
            };
        }

        return record;
    }
}
