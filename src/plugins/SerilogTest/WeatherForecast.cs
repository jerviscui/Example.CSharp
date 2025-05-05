namespace SerilogTest;

public class WeatherForecast
{

    #region Properties

    public DateOnly Date { get; set; }

    public DateTime DateTime { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public string? Summary { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    #endregion

}
