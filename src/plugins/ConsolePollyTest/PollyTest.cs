using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.Telemetry;

namespace ConsolePollyTest;

public static class PollyTest
{

    #region Constants & Statics

    public static Uri Host { get; set; } = new Uri("http://localhost:5222");

    private static HttpClient GetClient()
    {
        var client = new HttpClient { BaseAddress = Host };
        return client;
    }

    private static Uri GetUri(string path)
    {
        return new Uri(Host, path);
    }

    private static TelemetryOptions GetTelemetry()
    {
        var telemetryOptions = new TelemetryOptions
        {
            // Configure logging
            LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole()),
        };
        telemetryOptions.TelemetryListeners.Add(new MyTelemetryListener());
        telemetryOptions.MeteringEnrichers.Add(new MyMeteringEnricher());

        return telemetryOptions;
    }

    public static async Task NoStrategy_TestAsync()
    {
        var builder = new ResiliencePipelineBuilder();
        var pipeline = builder.Build();

        using var client = GetClient();

        var result = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(GetUri("/weatherforecast/"), token);
                return r;
            },
            cancellationToken: default);

        Console.WriteLine(result);
    }

    public static async Task Retry_ExecThree_TestAsync()
    {
        var options = GetTelemetry();
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

        using var client = GetClient();

        var result = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(GetUri("/weatherforecast/"), token);
                return r;
            },
            cancellationToken: default);

        Console.WriteLine(result);
    }

    public static async Task Retry_UseOnePipeline_TestAsync()
    {
        var builder = new ResiliencePipelineBuilder().ConfigureTelemetry(GetTelemetry().LoggerFactory)
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

        using var client = GetClient();

        // retry three times
        var t1 = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(GetUri("/weatherforecast/"), token);
                return r;
            },
            cancellationToken: default);

        // retry three times
        var t2 = await pipeline.ExecuteAsync(
            async (token) =>
            {
                var r = await client.GetAsync(GetUri("/weatherforecast/"), token);
                return r;
            },
            cancellationToken: default);

        //var result = await Task.WhenAll(t1.AsTask(), t2.AsTask());
        //Console.WriteLine(result[0]);
        Console.WriteLine(t1);
    }

    #endregion

}
