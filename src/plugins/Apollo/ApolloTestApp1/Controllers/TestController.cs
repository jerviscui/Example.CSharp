using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApolloTestApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        private readonly IServiceProvider _serviceProvider;

        public TestController(ILogger<TestController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpGet(Name = "GetMyOption")]
        public IActionResult GetMyOption()
        {
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            var options = _serviceProvider.GetRequiredService<IOptions<MyOption>>();

            _logger.LogInformation($"IConfiguration type: {configuration.GetType().FullName}");

            return Ok(options.Value);
        }
    }
}
