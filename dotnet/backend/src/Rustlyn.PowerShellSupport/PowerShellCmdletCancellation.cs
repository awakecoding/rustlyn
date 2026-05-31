using System.Threading;
using System.Management.Automation;

namespace Rustlyn.PowerShellSupport;

public sealed class PowerShellCmdletCancellation
{
    private int _isCancellationRequested;

    public bool IsCancellationRequested
        => Volatile.Read(ref _isCancellationRequested) != 0;

    public void RequestCancellation()
        => Interlocked.Exchange(ref _isCancellationRequested, 1);

    public void ThrowIfCancellationRequested()
    {
        if (IsCancellationRequested)
        {
            throw new PipelineStoppedException();
        }
    }
}
