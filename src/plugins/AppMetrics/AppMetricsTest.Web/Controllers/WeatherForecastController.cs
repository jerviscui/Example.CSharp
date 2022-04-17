using System.Text;
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            var builder = new MetricsBuilder();
            var metricsRoot = builder.Build();

            metricsRoot.Measure.Counter.Increment(new CounterOptions { Name = "test" },
                new MetricTags("key1", "value1"), 2);

            var source = metricsRoot.Snapshot.Get();

            var stream = new MemoryStream();
            await metricsRoot.DefaultOutputMetricsFormatter.WriteAsync(stream, source);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            return Ok(content);
        }
    }
}
