using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace CapTest.Order.Service
{
    public class OrderCreatedEventHandler : ICapSubscribe
    {
        private readonly ILogger _logger;

        public OrderCreatedEventHandler(ILogger logger) => _logger = logger;

        [CapSubscribe(OrderCreatedEventData.Name)]
        public void Handler(OrderCreatedEventData data)
        {
            _logger.LogInformation($"event received: {data.Number}");
        }
    }
}
