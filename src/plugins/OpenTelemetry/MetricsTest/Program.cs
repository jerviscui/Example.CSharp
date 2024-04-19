using System.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;

Console.WriteLine("Hello, World!");

var meter = new Meter("Sample.Meter", "1.0.0");
var counter = meter.CreateCounter<int>("sales-counter");

var builder = Sdk.CreateMeterProviderBuilder();
builder.AddMeter(meter.Name);
//builder.AddPrometheusHttpListener(options =>
//{
//    // test http://localhost:9464/metrics
//    options.UriPrefixes = new[] { "http://localhost:9464/" };
//});
builder.AddOtlpExporter((exporterOptions, metricReaderOptions) =>
{
    exporterOptions.Endpoint =
        new Uri(" http://localhost:9090/api/v1/otlp/v1/metrics"); // Prometheus otlp-write-receiver url
    exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
    metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5_000;
});

using var meterProvider = builder.Build();

var random = Random.Shared;
while (!Console.KeyAvailable)
{
    await Task.Delay(500);
    counter.Add(random.Next(10));
}
