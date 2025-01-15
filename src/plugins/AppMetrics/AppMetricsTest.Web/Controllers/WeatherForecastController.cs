using App.Metrics;
using App.Metrics.Counter;
using Microsoft.AspNetCore.Mvc;

namespace AppMetricsTest.Web.Controllers;

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

    #region Methods

    [HttpGet]
    [Route("Counter1")]
    public IActionResult Counter1()
    {
        //使用了不同的 tag，counter1 和 counter2 是两个计数器
        _metricsRoot.Measure.Counter.Increment(new CounterOptions
        {
            Name =
                                                                        "test"
        }, new MetricTags("key1", "counter1"));

        return Ok();
    }

    [HttpGet]
    [Route("Counter2")]
    public IActionResult Counter2()
    {
        //使用了不同的 tag，counter1 和 counter2 是两个计数器
        _metricsRoot.Measure.Counter.Increment(new CounterOptions
        {
            Name =
                                                                        "test"
        }, new MetricTags("key1", "counter2"));

        return Ok();
    }

    [HttpGet]
    [Route("Test")]
    public IActionResult Test()
    {
        _metricsRoot.Measure.Counter
            .Increment(new CounterOptions { Name = "test" }, new MetricTags("key1", "value1"), 2);

        return Ok();
    }

    #endregion

}
