using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewtonsoftJsonTest;

public class WeatherForecast
{

    #region Properties

    public object? Data { get; set; }

    public List<object>? DataList { get; set; }

    #endregion

}

[JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(WeatherForecast))]
[JsonSerializable(typeof(List<WeatherForecast>))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
internal sealed partial class WeatherForecastContext : JsonSerializerContext
{
}

public static class SystemTextJsonTestGeneratorTest
{

    #region Constants & Statics

    public static void DeserializeTest()
    {
        const string? jsonString = """
            {
              "Data": "Sunny",
              "DataList": [
                true,
                1
              ]
            }
            """;

        var data = JsonSerializer.Deserialize(jsonString, WeatherForecastContext.Default.WeatherForecast);
    }

    public static void SerializeTest()
    {
        var data = new WeatherForecast { Data = "Sunny", DataList = [true, 1] };

        var jsonString = JsonSerializer.Serialize(data, WeatherForecastContext.Default.WeatherForecast);
        //output:
        //{
        //  "Data": "Sunny",
        //  "DataList": [
        //    true,
        //    1
        //  ]
        //}
    }

    #endregion

}
