using AutofacWebTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutofacWebTest.Controllers
{
    /// <summary>
    /// WeatherForecastController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase, IWeatherForecastService
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;

            _logger.LogInformation($"create controller {Stopwatch.GetTimestamp()}");
        }

        /// <inheritdoc cref="IWeatherForecastService.Get"/>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation($"exec controller {Stopwatch.GetTimestamp()}");

            return _weatherForecastService.Get();
        }
    }
}
