using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

Console.WriteLine("Hello, World!");

var builder = Sdk.CreateTracerProviderBuilder();

using var tracerProvider = builder.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService("TraceTest"))
    .AddSource("Sample.DistributedTracing")
    .AddConsoleExporter()
    .AddZipkinExporter(options => { })
    .Build();

var activitySource = new ActivitySource("Sample.DistributedTracing", "1.0.0");

using (var activity = activitySource.StartActivity("Do", ActivityKind.Server))
{
    activity?.SetTag("tag1", "foo");
    activity?.SetTag("tag2", "bar");

    await StepOne();
    activity?.AddEvent(new ActivityEvent("StepOne end."));

    await StepTwo();
    activity?.AddEvent(new ActivityEvent("StepTwo end."));

    activity?.SetTag("otel.status_code", DateTime.Now.Microsecond % 2 == 0 ? "OK" : "ERROR");
    activity?.SetTag("otel.status_description", "Use this text give more information about the error");
}

Console.ReadLine();
activitySource.Dispose();

async Task StepOne()
{
    using (var activity = activitySource.StartActivity(ActivityKind.Client))
    {
        Console.WriteLine("One");
        await Task.Delay(50);
        await StepOneOne();
    }
}

async Task StepOneOne()
{
    using (var activity = activitySource.StartActivity(ActivityKind.Server))
    {
        Console.WriteLine("OneOne");
        await Task.Delay(50);
    }
}

async Task StepTwo()
{
    using (var activity = activitySource.StartActivity(ActivityKind.Producer))
    {
        Console.WriteLine("Two");
        await Task.Delay(500);
    }
}
