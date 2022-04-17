using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Scheduling;

namespace AppMetricsTest.Console;

internal class CounterTest : IDisposable
{
    private static readonly CounterOptions SentEmailsCounter = new()
    {
        Name = "Sent Emails", MeasurementUnit = Unit.Calls
    };

    private readonly AppMetricsTaskScheduler _scheduler;

    private readonly IMetricsRoot _metrics;

    public CounterTest()
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

    public void SentEmailsCounter_Test()
    {
        _metrics.Measure.Counter.Increment(SentEmailsCounter, 10);
        Thread.Sleep(2000);
        _metrics.Measure.Counter.Decrement(SentEmailsCounter);
        Thread.Sleep(2000);
        _metrics.Provider.Counter.Instance(SentEmailsCounter).Reset(); //重置 Counter
    }

    /// <inheritdoc />
    public void Dispose() => _scheduler.Dispose();
}
