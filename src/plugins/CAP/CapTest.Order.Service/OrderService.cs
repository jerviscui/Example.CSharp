using CapTest.Shared;
using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.DependencyInjection;

namespace CapTest.Order.Service
{
    public class OrderService
    {
        private readonly ICapPublisher _capPublisher;

        private readonly IServiceProvider _serviceProvider;

        public OrderService(ICapPublisher capPublisher, IServiceProvider serviceProvider)
        {
            _capPublisher = capPublisher;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> Create(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            await using var transaction = dbContext.Database.BeginTransaction(_capPublisher);

            var order = new Order(id, DateTime.Now.ToString("s"));
            await dbContext.Orders.AddAsync(order);
            await _capPublisher.PublishAsync(OrderCreatedEventData.Name, new OrderCreatedEventData(order.Number));

            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return order.Number;
        }

        public async Task<string> CreateWithoutCapPgsql(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var order = new Order(id, DateTime.Now.ToString("s"));
            await dbContext.Orders.AddAsync(order);

            var outter = _capPublisher.Transaction.Value;

            try
            {
                var capTransaction =
                    new PushMessageTransaction(scope.ServiceProvider.GetRequiredService<IDispatcher>());
                capTransaction.DbTransaction = transaction;
                capTransaction.AutoCommit = false;
                _capPublisher.Transaction.Value = capTransaction;

                await _capPublisher.PublishAsync(OrderCreatedEventData.Name, new OrderCreatedEventData(order.Number));

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync(); //此时 message 没有 EnqueueToPublish

                await capTransaction.CommitAsync();
            }
            finally
            {
                _capPublisher.Transaction.Value = outter!;
            }

            return order.Number;
        }

        // ReSharper disable once InconsistentNaming
        public async Task CreateMessageWithHeaders()
        {
            var headers = new Dictionary<string, string?> { { "msg-by-header", "0" } };
            await _capPublisher.PublishAsync("test.header", DateTime.Now.ToString("s"), headers);
        }
    }
}
