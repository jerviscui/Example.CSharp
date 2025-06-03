using DotNetCore.CAP;
using DotNetCore.CAP.Transport;

namespace CapTest.Order.Service;

internal sealed class PushMessageTransaction : CapTransactionBase
{
    /// <inheritdoc/>
    public PushMessageTransaction(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    #region Methods

    /// <inheritdoc/>
    public override void Commit()
    {
        Flush();
    }

    /// <inheritdoc/>
    public override Task CommitAsync(CancellationToken cancellationToken = new())
    {
        Flush();

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
    }

    /// <inheritdoc/>
    public override void Rollback()
    {
    }

    /// <inheritdoc/>
    public override Task RollbackAsync(CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    #endregion

}
