using Polly;
using Polly.Retry;

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
        var builder = new ResiliencePipelineBuilder().AddRetry(
            new RetryStrategyOptions
            {
                Delay = TimeSpan.FromSeconds(1),
                MaxRetryAttempts = 2,
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

    #endregion

}
