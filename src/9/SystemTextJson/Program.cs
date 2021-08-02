using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemTextJsonTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //SerializerTimeTest();
            SerializeStructTest();
        }

        private static void SerializerTimeTest()
        {
            var options = new JsonSerializerOptions();

            var t1 = DateTime.UtcNow;

            //"2021-01-28T04:51:18.7164123Z"
            Console.WriteLine(JsonSerializer.Serialize(t1, options));

            //"2021-01-28T04:51:18.7164123+08:00"
            Console.WriteLine(JsonSerializer.Serialize(DateTime.SpecifyKind(t1, DateTimeKind.Local), options));
            // DateTime.SpecifyKind 只改时区不改时间

            //"2021-01-28T12:51:18.7164123+08:00"
            Console.WriteLine(JsonSerializer.Serialize(t1.ToLocalTime(), options));
            // ToLocalTime 按照时区规则调整时间
            Console.WriteLine();

            var s = "\"2020-09-06T11:31:01.9233950-07:00\"";
            var t2 = JsonSerializer.Deserialize<DateTime>(s);
            //"2020-09-07T02:31:01.923395+08:00"
            Console.WriteLine(JsonSerializer.Serialize(t2, options)); //默认转换为本地时区时间
        }

        private static void SerializeStructTest()
        {
            var json =
                "{\"date\":\"2020-09-06T11:31:01.9233950-07:00\",\"temperatureC\":-1,\"temperatureF\":0,\"summary\":\"Scorching\"}";
            var options = new JsonSerializerOptions
            {
                IncludeFields = true, //支持字段
                WriteIndented = true, //格式缩进
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,                              //反序列化名称大小写区分
                IgnoreNullValues = false,                                        //忽略空值
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault, //空值处理策略，
                //WhenWritingDefault 序列化时忽略默认值
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                    JsonNumberHandling.WriteAsString,        //数字处理策略，将数字处理为字符串
                ReferenceHandler = ReferenceHandler.Preserve //循环引用处理
            };

            var forecast = JsonSerializer.Deserialize<Forecast>(json, options);

            //2020-09-07T02:31:01.9233950+08:00
            Console.WriteLine(forecast.Date.ToString("o")); //反序列化时转为本地时区时间
            Console.WriteLine(forecast.Date.Kind);

            Console.WriteLine();

            Console.WriteLine(forecast.TemperatureC); //0 JsonIgnore
            Console.WriteLine(forecast.TemperatureF); //1 JsonIgnoreCondition.WhenWritingDefault
            Console.WriteLine(forecast.Summary);      //""

            //{
            //    "temperatureF": "1",
            //    "summary": "",
            //    "date": "2020-09-07T02:31:01.923395+08:00"
            //}
            var roundTrippedJson = JsonSerializer.Serialize(forecast, options);
            Console.WriteLine(roundTrippedJson);
        }

        private readonly struct Forecast
        {
            public readonly DateTime Date;

            [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
            public int TemperatureC { get; }

            public int TemperatureF { get; }

            public string? Summary { get; }

            [JsonConstructor]
            public Forecast(DateTime date, int temperatureC, int temperatureF, string summary) =>
                (Date, TemperatureC, TemperatureF, Summary) = (date, temperatureC, 1, string.Empty);
        }
    }
}
