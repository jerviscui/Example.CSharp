using Microsoft.Extensions.Logging;
using Polly.Telemetry;

namespace ConsolePollyTest;

public static class TestBase
{
    public static Uri Host { get; set; } = new Uri("http://localhost:5222");

    public static HttpClient GetClient()
    {
        var client = new HttpClient { BaseAddress = Host };
        return client;
    }

    public static Uri GetUri(string path)
    {
        return new Uri(Host, path);
    }

    public static TelemetryOptions GetTelemetry()
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
}
