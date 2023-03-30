using EasyNetQ;
using EasyNetQ.Internals;

public static class SubTopicExtensions
{
    private static Action<ISubscriptionConfiguration> Configure<T>(Action<ISubscriptionConfiguration>? configure)
    {
        return configuration =>
        {
            configuration.WithTopic(typeof(T).Name);
            configure?.Invoke(configuration);
        };
    }

    public static AwaitableDisposable<SubscriptionResult> SubscribeAsync<T>(this IPubSub pubSub,
        Action<T> onMessage,
        CancellationToken cancellationToken = default)
    {
        return SubscribeAsync(pubSub,
            onMessage,
            null,
            cancellationToken
        );
    }

    public static AwaitableDisposable<SubscriptionResult> SubscribeAsync<T>(this IPubSub pubSub,
        Action<T> onMessage,
        Action<ISubscriptionConfiguration>? configure,
        CancellationToken cancellationToken = default)
    {
        var onMessageAsync = TaskHelpers.FromAction<T>((m, _) => onMessage(m));

        return pubSub.SubscribeAsync(
            "",
            onMessageAsync,
            Configure<T>(configure),
            cancellationToken
        );
    }

    public static AwaitableDisposable<SubscriptionResult> SubscribeAsync<T>(this IPubSub pubSub,
        Func<T, Task> onMessage,
        CancellationToken cancellationToken = default)
    {
        return pubSub.SubscribeAsync<T>(
            "",
            (m, _) => onMessage(m),
            Configure<T>(null),
            cancellationToken
        );
    }

    public static SubscriptionResult Subscribe<T>(this IPubSub pubSub,
        Action<T> onMessage,
        CancellationToken cancellationToken = default)
    {
        return Subscribe(pubSub,
            onMessage,
            null,
            cancellationToken
        );
    }

    public static SubscriptionResult Subscribe<T>(this IPubSub pubSub,
        Action<T> onMessage,
        Action<ISubscriptionConfiguration>? configure,
        CancellationToken cancellationToken = default)
    {
        var onMessageAsync = TaskHelpers.FromAction<T>((m, _) => onMessage(m));

        return pubSub.SubscribeAsync("", onMessageAsync, Configure<T>(configure), cancellationToken)
            .GetAwaiter()
            .GetResult();
    }

    public static SubscriptionResult Subscribe<T>(this IPubSub pubSub,
        Func<T, Task> onMessage,
        CancellationToken cancellationToken = default)
    {
        return Subscribe<T>(pubSub,
            (m, _) => onMessage(m),
            null,
            cancellationToken
        );
    }

    public static SubscriptionResult Subscribe<T>(this IPubSub pubSub,
        Func<T, CancellationToken, Task> onMessage,
        Action<ISubscriptionConfiguration>? configure,
        CancellationToken cancellationToken = default)
    {
        return pubSub.SubscribeAsync("", onMessage, Configure<T>(configure), cancellationToken)
            .GetAwaiter()
            .GetResult();
    }
}
