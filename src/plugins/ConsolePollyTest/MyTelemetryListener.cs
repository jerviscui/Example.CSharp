using Polly.Telemetry;

namespace ConsolePollyTest;

internal sealed class MyTelemetryListener : TelemetryListener
{
    public override void Write<TResult, TArgs>(in TelemetryEventArguments<TResult, TArgs> args)
    {
        Console.WriteLine($"Telemetry event occurred: {args.Event.EventName}");
    }
}
