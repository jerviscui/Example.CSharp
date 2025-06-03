using CapTest.Shared;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CapTest.Order.Service;

public class OrderCreatedEventHandler : ICapSubscribe
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    #region Methods

    [CapSubscribe(OrderCreatedEventData.Name)]
    public void Handler(OrderCreatedEventData data)
    {
        var host = _configuration.GetSection("ASPNETCORE_HOSTNAME").Value;
        var port = _configuration.GetSection("ASPNETCORE_PORT").Value;

        _logger.LogInformation($"event received: node1 {data.Number}");
    }

    [CapSubscribe(OrderConsts.HeaderMessageName)]
#pragma warning disable CA1822 // Mark members as static
    public void MessageWithHeaders(string data, [FromCap] CapHeader header)
#pragma warning restore CA1822 // Mark members as static
    {
        var value = header["msg-by-header"];
    }

    #endregion

}
