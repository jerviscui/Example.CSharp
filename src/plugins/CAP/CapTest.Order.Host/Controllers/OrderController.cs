using System.Threading.Tasks;
using CapTest.Order.Service;
using Microsoft.AspNetCore.Mvc;

namespace CapTest.Order.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<string> CreateAsync(int id)
        {
            return await _orderService.Create(id);
        }
    }
}
