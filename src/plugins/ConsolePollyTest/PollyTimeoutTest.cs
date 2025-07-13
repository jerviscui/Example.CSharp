using Polly;
using Polly.Timeout;

namespace ConsolePollyTest;

public static class PollyTimeoutTest
{

    #region Constants & Statics

    public static async Task Timeout_OuterCancel_TestAsync()
    {
        var options = TestBase.GetTelemetry();
        var builder = new ResiliencePipelineBuilder().ConfigureTelemetry(options)
            .AddTimeout(
                new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(1),
                    OnTimeout = (args) =>
                            {
                                Console.WriteLine(args.Context);

                                return ValueTask.CompletedTask;
                            }
                });

        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        using var cts = new CancellationTokenSource();
        var outerToken = cts.Token;

        try
        {
            var result = await pipeline.ExecuteAsync(
                async (token) =>
                {
                    //使用 outerToken 而不是 token，不会抛出 TimeoutRejectedException 异常
                    await Task.Delay(2_000, outerToken);
                    var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), outerToken);

                    return r;
                },
                cancellationToken: outerToken);

            Console.WriteLine(result);
        }
        catch (TimeoutRejectedException ex)
        {
            Console.WriteLine(ex);
        }
    }

    public static async Task Timeout_TestAsync()
    {
        var options = TestBase.GetTelemetry();
        var builder = new ResiliencePipelineBuilder().ConfigureTelemetry(options)
            .AddTimeout(
                new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(1),
                    OnTimeout = (args) =>
                            {
                                Console.WriteLine(args.Context);

                                return ValueTask.CompletedTask;
                            }
                });

        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        try
        {
            var result = await pipeline.ExecuteAsync(
                async (token) =>
                {
                    await Task.Delay(2_000, token);

                    var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);

                    return r;
                },
                cancellationToken: default);

            Console.WriteLine(result);
        }
        catch (TimeoutRejectedException ex)
        {
            Console.WriteLine(ex);
        }
    }

    #endregion

}
