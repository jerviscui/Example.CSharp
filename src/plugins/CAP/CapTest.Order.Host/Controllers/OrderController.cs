using CapTest.Order.Service;
using Microsoft.AspNetCore.Mvc;

namespace CapTest.Order.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    #region Methods

    [HttpPost]
    [Route("{id}")]
    public Task<string> CreateAsync(int id, CancellationToken cancellationToken)
    {
        return _orderService.CreateAsync(id, cancellationToken);
    }

    [HttpPost]
    [Route("CreateDelay/{id}")]
    public Task<string> CreateDelayAsync(int id, CancellationToken cancellationToken)
    {
        return _orderService.CreateDelayAsync(id, cancellationToken);
    }

    [HttpPost]
    [Route("CreateMessageWithHeaders")]
    // ReSharper disable once InconsistentNaming
    public async Task<IActionResult> CreateMessageWithHeadersAsync(CancellationToken cancellationToken)
    {
        await _orderService.CreateMessageWithHeaders(cancellationToken);

        return Ok();
    }

    [HttpPost]
    [Route("CreateWithTransaction/{id}")]
    public Task<string> CreateWithTransactionAsync(int id, CancellationToken cancellationToken)
    {
        return _orderService.CreateWithoutCapPgsqlAsync(id, cancellationToken);
    }

    #endregion

}
