using System.Text.Json;

namespace DiagnosticTest;

internal sealed class DiagnosticObserver : IObserver<KeyValuePair<string, object?>>
{
    /// <inheritdoc />
    public void OnCompleted()
    {
        Console.WriteLine("OnCompleted");
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        Console.WriteLine("OnError");
    }

    /// <inheritdoc />
    public void OnNext(KeyValuePair<string, object?> value)
    {
        Console.WriteLine($"OnNext with{Environment.CurrentManagedThreadId}: {JsonSerializer.Serialize(value)}");
    }
}
