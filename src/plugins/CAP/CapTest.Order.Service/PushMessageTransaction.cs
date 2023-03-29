using DotNetCore.CAP;
using DotNetCore.CAP.Transport;

namespace CapTest.Order.Service;

internal class PushMessageTransaction : CapTransactionBase
{
    /// <inheritdoc />
    public PushMessageTransaction(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    /// <inheritdoc />
    public override void Commit()
    {
        Flush();
    }

    /// <inheritdoc />
    public override Task CommitAsync(CancellationToken cancellationToken = new())
    {
        Flush();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Rollback()
    {
    }

    /// <inheritdoc />
    public override Task RollbackAsync(CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
    }
}
