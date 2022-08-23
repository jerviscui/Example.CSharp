using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using StackExchange.Redis;
using Timer = System.Timers.Timer;

namespace StackExchangeRedisTest.Lock;

public interface IRedisLock : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 是否获取锁
    /// </summary>
    /// <value>
    ///   <c>true</c> if the lock has taken; otherwise, <c>false</c>.
    /// </value>
    public bool HasTaken { get; }

    /// <summary>
    /// 获取锁
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    public ValueTask<bool> LockAsync(TimeSpan timeout);
}

public sealed class RedisLock : IRedisLock
{
    private static readonly TimeSpan Expiry = TimeSpan.FromSeconds(30);

    private static readonly long Period = ((long)Expiry.TotalMilliseconds - 1) / 3;

    private static readonly Dictionary<string, int> Counts = new();

    private static readonly Dictionary<string, ManualResetEventSlim> ResetEvents = new();

    private static readonly object LockObj = new();

    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly IDatabase _database;

    private readonly string _host;

    private readonly string _key;

    private Timer? _renewalTimer;

    private string? _value;

    public RedisLock(IDatabase database, string key)
    {
        _database = database;
        _key = key;

        _cancellationTokenSource = new CancellationTokenSource();
        _host = Environment.GetEnvironmentVariable("HOSTNAME") ?? string.Empty;

        Init(_key, _database);
    }

    private CancellationToken Token => _cancellationTokenSource.Token;

    private static long GetMillionsec(long duration)
    {
        return duration / (Stopwatch.Frequency / 1000);
    }

    private static void Init(string key, IDatabase database)
    {
        if (!ResetEvents.ContainsKey(key))
        {
            lock (LockObj)
            {
                if (!ResetEvents.ContainsKey(key))
                {
                    var resetEvent = new ManualResetEventSlim(true);
                    ResetEvents.Add(key, resetEvent);
                    Counts.Add(key, 0);

                    //subscribe
                    var subscriber = database.Multiplexer.GetSubscriber();
                    subscriber.Subscribe($"__keyspace@{database.Database}__:{key}", (_, value) =>
                    {
                        if (value == "del")
                        {
                            resetEvent.Set();
                        }
                    });
                }
            }
        }
    }

    public bool HasTaken { get; private set; }

    public async ValueTask<bool> LockAsync(TimeSpan timeout)
    {
        do
        {
            var timestamp = Stopwatch.GetTimestamp();
            var resetEvent = GetResetEvent();

            try
            {
                //todo: don't use async lock?
                //有歧义 使用错误容易死锁
                if (!resetEvent.Wait(timeout, Token))
                {
                    //timeout
                    HasTaken = false;
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                //token canceled
                HasTaken = false;
                break;
            }

            resetEvent.Reset();
            _value = GetValue();

            HasTaken = await _database.LockTakeAsync(_key, _value, Expiry);
            if (HasTaken)
            {
                var timer = new Timer();
                timer.Interval = Period;
                timer.AutoReset = true;
                timer.Elapsed += ExtendLock;
                timer.Start();
                _renewalTimer = timer;

                break;
            }

            timeout -= TimeSpan.FromMilliseconds(Math.Min(GetMillionsec(Stopwatch.GetTimestamp() - timestamp),
                timeout.TotalMilliseconds));
        } while (!HasTaken);

        return HasTaken;
    }

    private string GetValue()
    {
        var value = Counts[_key];
        value += 1;
        Counts[_key] = value;

        return $"{_host}:{_key}:{value.ToString()}";
    }

    private ManualResetEventSlim GetResetEvent()
    {
        var value = ResetEvents.GetValueOrDefault(_key);

        Debug.Assert(value != null, nameof(value) + " != null");
        return value;
    }

    private void ExtendLock(object? sender, ElapsedEventArgs e)
    {
        _database.LockExtend(_key, _value, Expiry);
    }

    //todo:
    //public void GetReentryLock(string key, string transactionId, TimeSpan timeout)
    //{
    //}

    #region Dispose

    private async ValueTask<bool> ReleaseLock()
    {
        try
        {
            return await _database.LockReleaseAsync(_key, _value);
        }
        catch
        {
            //todo: which exception
        }

        return false;
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            _renewalTimer?.Stop();
            _renewalTimer?.Dispose();

            ReleaseLock().AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    private async Task DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            _renewalTimer?.Stop();
            _renewalTimer?.Dispose();

            await ReleaseLock();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~RedisLock()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
