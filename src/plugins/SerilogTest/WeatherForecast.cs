namespace SerilogTest;

public class WeatherForecast : A
{

    #region Properties

    public DateOnly Date { get; set; }

    public DateTime DateTime { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public string? Summary { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public override string TypeName { get; } = nameof(WeatherForecast);

    #endregion

}

public abstract class A
{

    #region Properties

    public long Id { get; protected set; }

    public abstract string TypeName { get; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{TypeName} Id:{Id}";
    }

    #endregion

}