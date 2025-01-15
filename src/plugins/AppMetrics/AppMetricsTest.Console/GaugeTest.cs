using System.Diagnostics;
using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Scheduling;

namespace AppMetricsTest.Console;

internal sealed class GaugeTest : IDisposable
{
    private static readonly GaugeOptions ProcessPhysicalMemoryGauge = new()
    {
        Name = "Process Physical Memory", MeasurementUnit = Unit.Bytes
    };

    private static readonly GaugeOptions ProcessPhysicalMemoryGaugeMB = new()
    {
        Name = "Process Physical Memory (MB)", MeasurementUnit = Unit.MegaBytes
    };

    private readonly AppMetricsTaskScheduler _scheduler;

    private readonly IMetricsRoot _metrics;

    public GaugeTest()
    {
        _metrics = AppMetrics.CreateDefaultBuilder()
            .Configuration.Configure(options =>
            {
            })
            .Report.ToConsole()
            .Build();

        _scheduler = new AppMetricsTaskScheduler(TimeSpan.FromSeconds(1), async () =>
        {
            await Task.WhenAll(_metrics.ReportRunner.RunAllAsync());
            System.Console.WriteLine("----------------");
        });
        _scheduler.Start();
    }

    public void ProcessPhysicalMemoryGauge_Test()
    {
        var process = Process.GetCurrentProcess();

        _metrics.Measure.Gauge.SetValue(ProcessPhysicalMemoryGauge, process.WorkingSet64);
    }

    /// <summary>
    /// 使用 IMetricValueProvider
    /// </summary>
    public void ProcessPhysicalMemoryGaugeMB_Test()
    {
        var process = Process.GetCurrentProcess();

        var derivedGauge = new DerivedGauge(new FunctionGauge(() => process.WorkingSet64), d => d / 1024.0 / 1024.0);

        _metrics.Measure.Gauge.SetValue(ProcessPhysicalMemoryGaugeMB, () => derivedGauge);
    }

    /// <inheritdoc />
    public void Dispose() => _scheduler.Dispose();
}
