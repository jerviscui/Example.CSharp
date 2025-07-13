using Polly;
using Polly.Fallback;

namespace ConsolePollyTest;

public static class PollyFallbackTest
{

    #region Constants & Statics

    public static async Task Fallback_TestAsync()
    {
        var options = TestBase.GetTelemetry();
        var builder = new ResiliencePipelineBuilder<MyClass>().ConfigureTelemetry(options)
            .AddFallback(
                new FallbackStrategyOptions<MyClass>
                {
                    ShouldHandle = static (args) =>
                            {
                                // Handle all exceptions
                                return ValueTask.FromResult(true);
                            },
                    FallbackAction = static (args) =>
                            {
                                // Return a new instance of MyClass as fallback
                                return Outcome.FromResultAsValueTask(new MyClass("fallback"));
                            },
                    OnFallback = static (args) =>
                            {
                                Console.WriteLine($"Fallback executed for: {args.Outcome.Result?.PropertyName}");
                                return ValueTask.CompletedTask;
                            }
                });

        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        var result = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);

                return new MyClass("return");
            },
            cancellationToken: default);

        Console.WriteLine(result.PropertyName);
    }

    #endregion

    public class MyClass
    {
        public MyClass(string propertyName)
        {
            PropertyName = propertyName;
        }

        #region Properties

        public string PropertyName { get; private set; }

        #endregion
    }
}
