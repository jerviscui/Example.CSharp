using System.Collections.Concurrent;
using EasyNetQ.Consumer;
using EasyNetQ.Logging;

namespace EasyNetQTest;

public class RequeueConsumerErrorStrategy : IConsumerErrorStrategy
{
    private readonly ConcurrentDictionary<string, DateTime> _logDic = new();

    private readonly ILogger<DefaultConsumerErrorStrategy> _logger;

    public RequeueConsumerErrorStrategy(ILogger<DefaultConsumerErrorStrategy> logger) => _logger = logger;

    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public Task<AckStrategy> HandleConsumerErrorAsync(ConsumerExecutionContext context, Exception exception,
        CancellationToken cancellationToken = default)
    {
        //选择性记录异常日志，相同key 1分钟内只记录一次

        var receivedInfo = context.ReceivedInfo;
        var key = $"{receivedInfo.Exchange}-{receivedInfo.Queue}-{receivedInfo.RoutingKey}";
        if (_logDic.TryGetValue(key, out var last))
        {
            if (last.AddMinutes(1) < DateTime.Now)
            {
                Log();
            }
        }
        else
        {
            Log();
        }

        void Log()
        {
            _logDic.AddOrUpdate(key, _ => DateTime.Now, (_, _) => DateTime.Now);
            _logger.Error(
                exception,
                "Exception thrown by subscription callback, receivedInfo={receivedInfo}",
                receivedInfo);
        }

        return Task.FromResult(AckStrategies.NackWithRequeue);
    }

    /// <inheritdoc />
    public Task<AckStrategy> HandleConsumerCancelledAsync(ConsumerExecutionContext context,
        CancellationToken cancellationToken = default) => Task.FromResult(AckStrategies.NackWithRequeue);
}
