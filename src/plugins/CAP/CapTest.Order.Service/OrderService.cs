using CapTest.Shared;
using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.DependencyInjection;

namespace CapTest.Order.Service;

public class OrderService
{
    private readonly ICapPublisher _capPublisher;

    private readonly IServiceProvider _serviceProvider;

    public OrderService(ICapPublisher capPublisher, IServiceProvider serviceProvider)
    {
        _capPublisher = capPublisher;
        _serviceProvider = serviceProvider;
    }

    #region Methods

    public async Task<string> CreateAsync(int id, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        await using var transaction = await dbContext.Database
            .BeginTransactionAsync(_capPublisher, cancellationToken: cancellationToken);

        var order = new Order(id, DateTime.Now.ToString("s"));
        _ = await dbContext.Orders.AddAsync(order, cancellationToken);

        await _capPublisher.PublishAsync(
            OrderCreatedEventData.Name,
            new OrderCreatedEventData(order.Number),
            cancellationToken: cancellationToken);

        _ = await dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return order.Number;
    }

    public async Task<string> CreateDelayAsync(int id, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        await using var transaction = await dbContext.Database
            .BeginTransactionAsync(_capPublisher, cancellationToken: cancellationToken);

        var order = new Order(id, DateTime.Now.ToString("s"));
        _ = await dbContext.Orders.AddAsync(order, cancellationToken);

        await _capPublisher.PublishDelayAsync(
            TimeSpan.FromMinutes(1),
            OrderCreatedEventData.Name,
            new OrderCreatedEventData(order.Number),
            cancellationToken: cancellationToken);
        _ = await dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return order.Number;
    }

    // ReSharper disable once InconsistentNaming
#pragma warning disable CRR0034 // An asynchronous method's name is missing an 'Async' suffix
    public async Task CreateMessageWithHeaders(CancellationToken cancellationToken = default)
#pragma warning restore CRR0034 // An asynchronous method's name is missing an 'Async' suffix
    {
        var headers = new Dictionary<string, string?> { { "msg-by-header", "0" } };
        await _capPublisher.PublishAsync(
            OrderConsts.HeaderMessageName,
            DateTime.Now.ToString("s"),
            headers,
            cancellationToken);
    }

    public async Task<string> CreateWithoutCapPgsqlAsync(int id, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var order = new Order(id, DateTime.Now.ToString("s"));
        _ = await dbContext.Orders.AddAsync(order, cancellationToken);

        var outter = _capPublisher.Transaction;

        try
        {
            var capTransaction =
                new PushMessageTransaction(scope.ServiceProvider.GetRequiredService<IDispatcher>())
                {
                    DbTransaction = transaction,
                    AutoCommit = false
                };
            _capPublisher.Transaction = capTransaction;

            await _capPublisher.PublishAsync(
                OrderCreatedEventData.Name,
                new OrderCreatedEventData(order.Number),
                cancellationToken: cancellationToken);

            _ = await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken); //此时 message 没有 EnqueueToPublish

            await capTransaction.CommitAsync(cancellationToken);
        }
        finally
        {
            _capPublisher.Transaction = outter!;
        }

        return order.Number;
    }

    #endregion

}
