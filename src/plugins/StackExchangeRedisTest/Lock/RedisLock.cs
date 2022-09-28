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

    private static readonly Dictionary<string, int> Counts = new();

    private const int MaxTryLock = 1;

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

    private string _value;

    private readonly SemaphoreSlim _dispose;

    private int _once;

    internal RedisLock(IDatabase database, string key)
    {
        _database = database;
        _key = key;

        _cancellationTokenSource = new CancellationTokenSource();
        _dispose = new SemaphoreSlim(1);
        _value = string.Empty;

        _renewalTimer = new Timer { Interval = Period, AutoReset = true };
        _renewalTimer.Elapsed += ExtendLock;

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
                    var resetEvent = new SemaphoreSlim(MaxTryLock);
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
                        //todo: 一直没有收到消息时，释放逻辑 expired
                        //1. 先取再睡眠，cancel 唤醒，cancel 同时用于dispose
                        //2. 先睡眠（当前）, expire 续期，定时器用于没有回调，并且超时自动释放
                        //todo: 内存使用情况，应当清理无用的 resetEvent
                        //todo: ref class = null 数组的 item 也为 null？
                    });
                }
            }
        }
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

        do
        {
            var timestamp = Stopwatch.GetTimestamp();
            var resetEvent = GetResetEvent();

            try
            {
                if (!resetEvent.Wait(timeout, Token))
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

            Console.WriteLine($"{Environment.CurrentManagedThreadId} {_key} {_value}");

            _value = GetValue();

            Console.WriteLine($"{Environment.CurrentManagedThreadId} {_key} {_value}");

            try
            {
                _dispose.Wait(Token);

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

            if ((long)timeout.TotalMilliseconds != Timeout.Infinite)
            {
                timeout -= TimeSpan.FromMilliseconds(GetMillionsec(Stopwatch.GetTimestamp() - timestamp));

                if (timeout.TotalMilliseconds <= 0)
                {
                    IsLocked = false;
                    break;
                }
            }

            Console.WriteLine(timeout);
        } while (!Token.IsCancellationRequested);

        return IsLocked;
    }

    /// <inheritdoc />
    public async Task<bool> LockAsync(TimeSpan timeout)
    {
        if (Interlocked.CompareExchange(ref _once, 1, 0) != 0)
        {
            throw new Exception($"Can only be locked once. key:{_key}");
        }

        do
        {
            var timestamp = Stopwatch.GetTimestamp();
            var resetEvent = GetResetEvent();

            try
            {
                if (!await resetEvent.WaitAsync(timeout, Token).ConfigureAwait(false))
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

            Console.WriteLine($"{Environment.CurrentManagedThreadId} {_key} {_value}");

            _value = GetValue();

            Console.WriteLine($"{Environment.CurrentManagedThreadId} {_key} {_value}");

            try
            {
                await _dispose.WaitAsync(Token);

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

            if ((long)timeout.TotalMilliseconds != Timeout.Infinite)
            {
                timeout -= TimeSpan.FromMilliseconds(GetMillionsec(Stopwatch.GetTimestamp() - timestamp));

                if (timeout.TotalMilliseconds <= 0)
                {
                    IsLocked = false;
                    break;
                }
            }

            Console.WriteLine(timeout);
        } while (!Token.IsCancellationRequested);

        return IsLocked;
    }

    private string GetValue()
    {
        var value = Counts[_key];
        value += 1;
        Counts[_key] = value;

        return $"{Host}:{_key}:{value.ToString()}";
    }

    private SemaphoreSlim GetResetEvent()
    {
        var value = ResetEvents.GetValueOrDefault(_key);

        Debug.Assert(value != null, nameof(value) + " != null");
        return value;
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
