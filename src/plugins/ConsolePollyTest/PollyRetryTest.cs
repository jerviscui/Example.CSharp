using Polly;
using Polly.Retry;

namespace ConsolePollyTest;

public static class PollyRetryTest
{

    #region Constants & Statics

    public static async Task NoStrategy_TestAsync()
    {
        var builder = new ResiliencePipelineBuilder();
        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        var result = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);
                return r;
            },
            cancellationToken: default);

        Console.WriteLine(result);
    }

    public static async Task Retry_ExecThree_TestAsync()
    {
        var options = TestBase.GetTelemetry();
        var builder = new ResiliencePipelineBuilder().ConfigureTelemetry(options)
            .AddRetry(
                new RetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(1),
                    //MaxDelay = 
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    MaxRetryAttempts = 2,
                    //DelayGenerator = (args) =>
                    //        {
                    //            return ValueTask.FromResult(
                    //                (TimeSpan?)TimeSpan.FromSeconds(Math.Pow(2, args.AttemptNumber)));
                    //        },
                    ShouldHandle = (args) =>
                            {
                                Console.WriteLine(args.AttemptNumber);
                                return ValueTask.FromResult(args.AttemptNumber >= 0);
                            }
                });
        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        var result = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);
                return r;
            },
            cancellationToken: default);

        Console.WriteLine(result);
    }

    public static async Task Retry_UseOnePipeline_TestAsync()
    {
        var builder = new ResiliencePipelineBuilder().ConfigureTelemetry(TestBase.GetTelemetry().LoggerFactory)
            .AddRetry(
                new RetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 3,
                    ShouldHandle = (args) =>
                            {
                                Console.WriteLine(args.AttemptNumber);
                                return ValueTask.FromResult(args.AttemptNumber >= 0);
                            }
                });

        var pipeline = builder.Build();

        using var client = TestBase.GetClient();

        // retry three times
        var t1 = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);
                return r;
            },
            cancellationToken: default);

        // retry three times
        var t2 = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(TestBase.GetUri("/weatherforecast/GetWeatherForecast"), token);
                return r;
            },
            cancellationToken: default);

        //var result = await Task.WhenAll(t1.AsTask(), t2.AsTask());
        //Console.WriteLine(result[0]);
        Console.WriteLine(t1);
    }

    #endregion

}
