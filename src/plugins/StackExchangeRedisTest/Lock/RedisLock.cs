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
    public bool Lock(TimeSpan timeout);
}

public sealed class RedisLock : IRedisLock
{
    private static readonly TimeSpan Expiry = TimeSpan.FromSeconds(30);

    private static readonly long Period = ((long)Expiry.TotalMilliseconds - 1) / 3;

    private static readonly Dictionary<string, int> Counts = new();

    private static readonly Dictionary<string, SemaphoreSlim> ResetEvents = new();

    private static readonly object LockObj = new();

    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly IDatabase _database;

    private readonly string _host;

    private readonly string _key;

    private Timer? _renewalTimer;

    private string _value;

    private readonly SemaphoreSlim _dispose;

    public RedisLock(IDatabase database, string key)
    {
        _database = database;
        _key = key;

        _cancellationTokenSource = new CancellationTokenSource();
        _host = Environment.GetEnvironmentVariable("HOSTNAME") ?? string.Empty;
        _dispose = new SemaphoreSlim(1);
        _value = string.Empty;

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
                    var resetEvent = new SemaphoreSlim(1);
                    ResetEvents.Add(key, resetEvent);
                    Counts.Add(key, 0);

                    //subscribe
                    var subscriber = database.Multiplexer.GetSubscriber();
                    subscriber.Subscribe($"__keyspace@{database.Database}__:{key}", (_, value) =>
                    {
                        if (value == "del")
                        {
                            resetEvent.Release();
                        }
                    });
                }
            }
        }
    }

    public bool HasTaken { get; private set; }

    public bool Lock(TimeSpan timeout)
    {
        //todo: just once
        do
        {
            var timestamp = Stopwatch.GetTimestamp();
            var resetEvent = GetResetEvent();

            try
            {
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

            _value = GetValue();

            // ReSharper disable once MethodSupportsCancellation
            _dispose.Wait();

            try
            {
                if (Token.IsCancellationRequested)
                {
                    HasTaken = false;
                    break;
                }

                HasTaken = _database.LockTake(_key, _value, Expiry);
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
            }
            finally
            {
                _dispose.Release();
            }

            if ((long)timeout.TotalMilliseconds != Timeout.Infinite)
            {
                timeout -= TimeSpan.FromMilliseconds(GetMillionsec(Stopwatch.GetTimestamp() - timestamp));

                if (timeout.TotalMilliseconds <= 0)
                {
                    HasTaken = false;
                    break;
                }
            }
        } while (!Token.IsCancellationRequested);

        return HasTaken;
    }

    //public async Task<bool> LockAsync(TimeSpan timeout)
    //{
    //    do
    //    {
    //        var timestamp = Stopwatch.GetTimestamp();
    //        var resetEvent = GetResetEvent();

    //        try
    //        {
    //            //有歧义 使用错误容易死锁
    //            if (!resetEvent.Wait(timeout, Token))
    //            {
    //                //timeout
    //                HasTaken = false;
    //                break;
    //            }
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            //token canceled
    //            HasTaken = false;
    //            break;
    //        }

    //        resetEvent.Reset();
    //        _value = GetValue();

    //        if (Token.IsCancellationRequested)
    //        {
    //            HasTaken = false;
    //            break;
    //        }

    //        HasTaken = await _database.LockTakeAsync(_key, _value, Expiry);
    //        if (HasTaken)
    //        {
    //            var timer = new Timer();
    //            timer.Interval = Period;
    //            timer.AutoReset = true;
    //            timer.Elapsed += ExtendLock;
    //            timer.Start();
    //            _renewalTimer = timer;

    //            break;
    //        }

    //        timeout -= TimeSpan.FromMilliseconds(Math.Min(GetMillionsec(Stopwatch.GetTimestamp() - timestamp),
    //            timeout.TotalMilliseconds));
    //    } while (!HasTaken);

    //    return HasTaken;
    //}

    private string GetValue()
    {
        var value = Counts[_key];
        value += 1;
        Counts[_key] = value;

        return $"{_host}:{_key}:{value.ToString()}";
    }

    private SemaphoreSlim GetResetEvent()
    {
        var value = ResetEvents.GetValueOrDefault(_key);

        Debug.Assert(value != null, nameof(value) + " != null");
        return value;
    }

    private void ExtendLock(object? sender, ElapsedEventArgs e)
    {
        _database.LockExtend(_key, _value, Expiry);
    }

    #region Dispose

    private async Task<bool> ReleaseLockAsync()
    {
        try
        {
            return await _database.LockReleaseAsync(_key, _value);
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }

        return false;
    }

    private void ReleaseLock()
    {
        try
        {
            if (!HasTaken)
            {
                return;
            }

            var release = _database.LockRelease(_key, _value);
            if (!release)
            {
                Console.WriteLine($"LockRelease Failed. {_key} {_value}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message} {_key} {_value}");
        }
    }

    private void Cleanup()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();

        _renewalTimer?.Stop();
        _renewalTimer?.Dispose();
        _renewalTimer = null;
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            // ReSharper disable once MethodSupportsCancellation
            _dispose.Wait();

            try
            {
                Cleanup();
                ReleaseLock();
            }
            finally
            {
                //_dispose.Release();//todo: perhaps?
                _dispose.Dispose();
            }
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

            await ReleaseLockAsync();
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
