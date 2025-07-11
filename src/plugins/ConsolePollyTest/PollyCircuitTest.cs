using Polly;
using Polly.CircuitBreaker;

namespace ConsolePollyTest;

public static class PollyCircuitTest
{

    #region Constants & Statics

    public static async Task Circuit_3Per2sec_TestAsync()
    {
        var options = TestBase.GetTelemetry();
        var builder = new ResiliencePipelineBuilder().ConfigureTelemetry(options)
            .AddCircuitBreaker(
                new CircuitBreakerStrategyOptions
                {
                    MinimumThroughput = 3,
                    SamplingDuration = TimeSpan.FromSeconds(2),
                    FailureRatio = 0.5,
                    BreakDuration = TimeSpan.FromSeconds(1),
                    ShouldHandle = static (args) =>
                            {
                                // Handle all exceptions
                                return ValueTask.FromResult(true);
                            },
                });
        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        var i = 0;
        while (i++ < 10)
        {
            try
            {
                var result = await pipeline.ExecuteAsync(
                    async (token) =>
                    {
                        var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);
                        return r;
                    },
                    cancellationToken: default);

                Console.WriteLine(i);
                Console.WriteLine(result);
                Console.WriteLine();
            }
            catch (BrokenCircuitException e)
            {
                Console.WriteLine(i);
                Console.WriteLine(e);
                Console.WriteLine();
                await Task.Delay(e.RetryAfter ?? TimeSpan.FromSeconds(5));
            }
        }
    }

    #endregion

}
