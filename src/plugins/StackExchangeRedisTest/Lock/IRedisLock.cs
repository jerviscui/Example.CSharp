using System;
using System.Threading.Tasks;

namespace StackExchangeRedisTest.Lock;

public interface IRedisLock : IRedisLockOnce
{
    /// <summary>
    /// 获取锁
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    public bool Lock(TimeSpan timeout);

    /// <summary>
    /// 获取锁
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    public Task<bool> LockAsync(TimeSpan timeout);
}

public interface IRedisLockOnce : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 是否获取锁
    /// </summary>
    /// <value>
    ///   <c>true</c> if the lock has taken; otherwise, <c>false</c>.
    /// </value>
    public bool IsLocked { get; }

    /// <summary>
    /// 是否获取锁
    /// </summary>
    /// <value>
    ///   <c>true</c> if try get lock once; otherwise, <c>false</c>.
    /// </value>
    public bool LockedOnce { get; }
}
