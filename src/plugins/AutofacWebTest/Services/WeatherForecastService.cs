using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace AutofacWebTest.Services
{
    /// <summary>
    /// Weather Forecast Service
    /// </summary>
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILifetimeScope _lifetimeScope;

        public WeatherForecastService(ILifetimeScope lifetimeScope) => _lifetimeScope = lifetimeScope;

        /// <inheritdoc />
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        /// <summary>
        /// 获取根 Scope
        /// </summary>
        public async Task GetRootScope()
        {
            var rootScope = ((ISharingLifetimeScope)_lifetimeScope).RootLifetimeScope;

            await using var childScope = rootScope.BeginLifetimeScope();
            //childScope.Resolve()
        }
    }
}
