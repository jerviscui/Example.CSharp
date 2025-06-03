using CapTest.Shared;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace CapTest.Depot.Service;

public class OrderCreatedEventHandler : ICapSubscribe
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    #region Methods

    [CapSubscribe(OrderCreatedEventData.Name, Group = OrderConsts.MessageGroupName)]
    public void Handler(OrderCreatedEventData data)
    {
        _logger.LogInformation("event received: node2 {Number}", data.Number);
    }

    [CapSubscribe(OrderCreatedEventData.Name, Group = OrderConsts.MessageGroupName + "2")]
    public void Handler2(OrderCreatedEventData data)
    {
        _logger.LogInformation("event received group2: node2 {Number}", data.Number);
    }

    #endregion

}
