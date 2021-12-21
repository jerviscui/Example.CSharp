using System.Collections.Concurrent;

namespace DiagnosticTest;

public class Publisher : IObservable<Data>, IDisposable
{
    private readonly ConcurrentQueue<Data>
        _datas = new(new[] { new() { Name = "test1" }, new Data { Name = "test2" } });

    private readonly ConcurrentDictionary<IObserver<Data>, IDisposable> _dic = new();

    /// <inheritdoc />
    public IDisposable Subscribe(IObserver<Data> observer)
    {
        if (_dic.ContainsKey(observer))
        {
            return _dic[observer];
        }

        var unsubscriber = new Unsubscriber<Data>(_dic, observer);
        _dic.TryAdd(observer, unsubscriber);

        foreach (var data in _datas)
        {
            observer.OnNext(data);
        }

        return unsubscriber;
    }

    public void Publish(Data data)
    {
        _datas.Enqueue(data);
        foreach (var (key, value) in _dic)
        {
            key.OnNext(data);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var (key, value) in _dic)
        {
            key.OnCompleted();
        }

        _dic.Clear();
    }
}

internal class Unsubscriber<T> : IDisposable
{
    private readonly IObserver<T> _observer;

    private readonly ConcurrentDictionary<IObserver<T>, IDisposable> _observers;

    internal Unsubscriber(ConcurrentDictionary<IObserver<T>, IDisposable> observers, IObserver<T> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        if (_observers.ContainsKey(_observer))
        {
            _observers.Remove(_observer, out _);
        }
    }
}
