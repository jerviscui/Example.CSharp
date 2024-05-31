using System.Collections.Generic;
using System.Diagnostics;
using AutofacWebTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _logger.LogInformation($"create controller {Stopwatch.GetTimestamp()}");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        /// <inheritdoc cref="IWeatherForecastService.Get"/>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _logger.LogInformation($"exec controller {Stopwatch.GetTimestamp()}");
#pragma warning restore CA1848 // Use the LoggerMessage delegates

            return _weatherForecastService.Get();
        }
    }
}
