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
    public Task<string> CreateAsync(int id)
    {
        return _orderService.Create(id);
    }

    [HttpPost]
    [Route("CreateMessageWithHeaders")]
    // ReSharper disable once InconsistentNaming
    public async Task<IActionResult> CreateTTLMessage()
    {
        await _orderService.CreateMessageWithHeaders();

        return Ok();
    }

    [HttpPost]
    [Route("CreateWithTransaction/{id}")]
    public Task<string> CreateWithTransactionAsync(int id)
    {
        return _orderService.CreateWithoutCapPgsql(id);
    }

    #endregion

}
