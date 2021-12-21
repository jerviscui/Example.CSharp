namespace DiagnosticTest;

public class Subscriber : IObserver<Data>
{
    private IDisposable? _unsubscriber;

    public long Id { get; private set; }

    public Subscriber() => Id = DateTime.Now.Ticks;

    public void Register(Publisher publisher)
    {
        _unsubscriber = publisher.Subscribe(this);
    }

    /// <inheritdoc />
    public void OnCompleted()
    {
        Console.WriteLine($"OnCompleted:{Id}");

        _unsubscriber?.Dispose();
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        Console.WriteLine($"OnError:{Id}");
        Console.WriteLine(error.Message);
    }

    /// <inheritdoc />
    public void OnNext(Data value)
    {
        Console.WriteLine($"OnNext:{Id} {value.Name}");
    }
}
