using CapTest.Shared;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CapTest.Order.Service
{
    public class OrderCreatedEventHandler : ICapSubscribe
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        private readonly IConfiguration _configuration;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [CapSubscribe(OrderCreatedEventData.Name)]
        public void Handler(OrderCreatedEventData data)
        {
            var host = _configuration.GetSection("ASPNETCORE_HOSTNAME").Value;
            var port = _configuration.GetSection("ASPNETCORE_PORT").Value;

            _logger.LogInformation($"event received: node1 {data.Number}");
        }
    }
}
