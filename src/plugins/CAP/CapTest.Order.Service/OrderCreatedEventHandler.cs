using CapTest.Shared;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CapTest.Order.Service;

public class OrderCreatedEventHandler : ICapSubscribe
{

    #region Constants & Statics

    [CapSubscribe(OrderConsts.HeaderMessageName)]
    public static void MessageWithHeaders(string data, [FromCap] CapHeader header)
    {
        var value = header["msg-by-header"];
    }

    #endregion

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

    #endregion

}
