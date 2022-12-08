using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using StackExchange.Redis;
using Timer = System.Timers.Timer;

namespace StackExchangeRedisTest.Lock;

public sealed class RedisLock : IRedisLock
{
    private static readonly TimeSpan Expiry = TimeSpan.FromSeconds(30);

    private static readonly long Period = ((long)Expiry.TotalMilliseconds - 1) / 3;

    private static readonly Dictionary<string, SemaphoreSlim> ResetEvents = new();

    private static readonly object LockObj = new();

    //docker windows Linux
    private static readonly string Host = Environment.GetEnvironmentVariable("HOSTNAME") ??
        Environment.GetEnvironmentVariable("COMPUTERNAME") ??
        Environment.GetEnvironmentVariable("NAME") ?? Stopwatch.GetTimestamp().ToString();

    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly IDatabase _database;

    private readonly string _key;

    private readonly Timer _renewalTimer;

    private readonly string _value;

    private readonly SemaphoreSlim _dispose;

    private int _once;

    private static int _count;

    internal RedisLock(IDatabase database, string key)
    {
        _database = database;
        _key = key;
        _value = GetValue();

        _cancellationTokenSource = new CancellationTokenSource();
        _dispose = new SemaphoreSlim(1);

        _renewalTimer = new Timer { Interval = Period, AutoReset = true };
        _renewalTimer.Elapsed += ExtendLock;
    }

    private CancellationToken DisposeToken => _cancellationTokenSource.Token;

    private string GetValue()
    {
        var value = Interlocked.Increment(ref _count);

        return $"{Host}:{_key}:{value.ToString()}";
    }

    private static SemaphoreSlim Init(string key, IDatabase database)
    {
        if (!ResetEvents.TryGetValue(key, out var resetEvent))
        {
            lock (LockObj)
            {
                if (!ResetEvents.TryGetValue(key, out resetEvent))
                {
                    resetEvent = new SemaphoreSlim(0);
                    ResetEvents.Add(key, resetEvent);

                    //subscribe
                    var subscriber = database.Multiplexer.GetSubscriber();
                    subscriber.Subscribe($"__keyspace@{database.Database}__:{key}", (_, value) =>
                    {
                        if (value == "del")
                        {
                            resetEvent.Release();
                        }

                        //todo: 内存使用情况，应当清理无用的 resetEvent
                        //todo: ref class = null 数组的 item 也为 null？
                    });
                }
            }
        }

        return resetEvent;
    }

    /// <inheritdoc />
    public bool IsLocked { get; private set; }

    /// <inheritdoc />
    public bool LockedOnce => _once != 0;

    public bool Lock(TimeSpan timeout)
    {
        if (Interlocked.CompareExchange(ref _once, 1, 0) != 0)
        {
            throw new Exception($"Can only be locked once. key:{_key}");
        }

        var clocker = new Clocker(timeout);
        var resetEvent = Init(_key, _database);

        do
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId} {_key} {_value}");

            try
            {
                _dispose.Wait(DisposeToken);

                IsLocked = _database.LockTake(_key, _value, Expiry);
                if (IsLocked)
                {
                    _renewalTimer.Start();
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                //token canceled
                IsLocked = false;
                break;
            }
            finally
            {
                _dispose.Release();
            }

            clocker = clocker.NextTime();
            Console.WriteLine(timeout);

            try
            {
                if (!resetEvent.Wait(clocker.Timeout, DisposeToken))
                {
                    //timeout
                    IsLocked = false;
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                //token canceled
                IsLocked = false;
                break;
            }
        } while (!DisposeToken.IsCancellationRequested);

        return IsLocked;
    }

    /// <inheritdoc />
    public async Task<bool> LockAsync(TimeSpan timeout)
    {
        if (Interlocked.CompareExchange(ref _once, 1, 0) != 0)
        {
            throw new Exception($"Can only be locked once. key:{_key}");
        }

        var clocker = new Clocker(timeout);
        var resetEvent = Init(_key, _database);

        do
        {
            Console.WriteLine($"{Environment.CurrentManagedThreadId} {_key} {_value}");

            try
            {
                await _dispose.WaitAsync(DisposeToken);

                IsLocked = await _database.LockTakeAsync(_key, _value, Expiry);
                if (IsLocked)
                {
                    _renewalTimer.Start();
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                //token canceled
                IsLocked = false;
                break;
            }
            finally
            {
                _dispose.Release();
            }

            clocker = clocker.NextTime();
            Console.WriteLine(timeout);

            try
            {
                if (!await resetEvent.WaitAsync(clocker.Timeout, DisposeToken).ConfigureAwait(false))
                {
                    //timeout
                    IsLocked = false;
                    break;
                }
            }
            catch (OperationCanceledException) //contains: TaskCanceledException
            {
                //token canceled
                IsLocked = false;
                break;
            }
        } while (!DisposeToken.IsCancellationRequested);

        return IsLocked;
    }

    private struct Clocker
    {
        private bool _forever;

        public TimeSpan Timeout;

        private readonly long _createtime;

        public Clocker(TimeSpan timeout)
        {
            Timeout = timeout;
            _forever = (long)timeout.TotalMilliseconds == System.Threading.Timeout.Infinite;

            _createtime = Stopwatch.GetTimestamp();
        }

        public Clocker NextTime()
        {
            if (_forever)
            {
                return this;
            }

            var next = GetNext();

            var left = Timeout - TimeSpan.FromMilliseconds(GetMillionsec(next._createtime - _createtime));
            next.Timeout = left.TotalMilliseconds <= 0 ? TimeSpan.Zero : left;

            return next;
        }

        private Clocker GetNext() => new(TimeSpan.Zero) { _forever = _forever };

        private static long GetMillionsec(long duration)
        {
            return duration / (Stopwatch.Frequency / 1000);
        }
    }

    private void ExtendLock(object? sender, ElapsedEventArgs e)
    {
        try
        {
            _database.LockExtend(_key, _value, Expiry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LockExtend error:{ex.Message} key:{_key} value:{_value}");
        }
    }

    #region Dispose

    private async Task ReleaseLockAsync()
    {
        try
        {
            if (!IsLocked)
            {
                return;
            }

            var release = await _database.LockReleaseAsync(_key, _value);
            if (!release)
            {
                Console.WriteLine($"LockRelease Failed. {_key} {_value}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"LockRelease error:{e.Message} key:{_key} value:{_value}");
        }
    }

    private void ReleaseLock()
    {
        try
        {
            if (!IsLocked)
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
            Console.WriteLine($"LockRelease error:{e.Message} key:{_key} value:{_value}");
        }
    }

    private void Cleanup()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();

        _renewalTimer.Stop();
        _renewalTimer.Dispose();
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
                _dispose.Dispose();
            }
        }
    }

    private async Task DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            // ReSharper disable once MethodSupportsCancellation
            await _dispose.WaitAsync();

            try
            {
                Cleanup();
                await ReleaseLockAsync();
            }
            finally
            {
                _dispose.Dispose();
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    ~RedisLock()
    {
        Dispose(false);
    }

    #endregion
}
