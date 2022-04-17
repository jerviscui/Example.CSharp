using App.Metrics;
using App.Metrics.Counter;
using Microsoft.AspNetCore.Mvc;

namespace AppMetricsTest.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IMetricsRoot _metricsRoot;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMetricsRoot metricsRoot)
        {
            _logger = logger;
            _metricsRoot = metricsRoot;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            _metricsRoot.Measure.Counter.Increment(new CounterOptions { Name = "test" },
                new MetricTags("key1", "value1"), 2);

            return Ok();
        }
    }
}
