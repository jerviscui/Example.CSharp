using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
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
            await _capPublisher.PublishAsync(OrderCreatedEventData.Name, new OrderCreatedEventData(order.Number), "");

            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return order.Number;

            //var trans = database.BeginTransaction();
            //publisher.Transaction.Value =
            //    ActivatorUtilities.CreateInstance<PostgreSqlCapTransaction>(publisher.ServiceProvider);
            //var capTrans = publisher.Transaction.Value.Begin(trans, autoCommit);
            //return new CapEFDbTransaction(capTrans);
        }
    }
}
