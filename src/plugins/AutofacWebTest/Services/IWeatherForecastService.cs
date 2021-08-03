using System.Collections.Generic;

namespace AutofacWebTest.Services
{
    public interface IWeatherForecastService
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        IEnumerable<WeatherForecast> Get();
    }
}
