using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchangeRedisTest
{
    internal class LockTest
    {
        public static void LockTakeAsync_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            var key = "lock:key1";

            var t1 = Task.Run(async () =>
            {
                while (true)
                {
                    var taken = await database.LockTakeAsync(key, 1, TimeSpan.FromSeconds(60));
                    if (!taken)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    try
                    {
                        Console.WriteLine("task1 get lock");
                        Thread.Sleep(10_000);
                        return;
                    }
                    finally
                    {
                        if (taken)
                        {
                            await database.LockReleaseAsync(key, 1);
                        }
                    }
                }
            });

            var t2 = Task.Run(async () =>
            {
                while (true)
                {
                    var taken = await database.LockTakeAsync(key, 2, TimeSpan.FromSeconds(60));
                    if (!taken)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    try
                    {
                        Console.WriteLine("task2 get lock");
                        Thread.Sleep(10_000);
                        return;
                    }
                    finally
                    {
                        if (taken)
                        {
                            await database.LockReleaseAsync(key, 2);
                        }
                    }
                }
            });

            Task.WaitAll(t1, t2);
        }

        public static void LockExtend_Test()
        {
            var database = DatabaseProvider.GetDatabase();
            var subscriber = database.Multiplexer.GetSubscriber();

            var key = "lock:key2";

            var t1 = Task.Run(async () =>
            {
                var taken = await database.LockTakeAsync(key, 1, TimeSpan.FromSeconds(10));
                if (!taken)
                {
                    Console.WriteLine("task1 don't get lock");
                    return;
                }

                try
                {
                    var cts = new CancellationTokenSource();
                    var token = cts.Token;
                    token.Register(() => cts.Dispose());
                    var expr = DateTime.Now.AddMinutes(30);

                    //create extend task
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    Task.Run(() =>
                    {
                        while (expr > DateTime.Now && token.CanBeCanceled)
                        {
                            Thread.Sleep(3000);

                            if (token.IsCancellationRequested)
                            {
                                return;
                            }

                            database.LockExtend(key, 1, TimeSpan.FromSeconds(10));
                        }
                    }, token);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法

                    var subKey = $"__keyspace@{database.Database}__:{key}";
                    //subscribe
                    await subscriber.SubscribeAsync(subKey, (channel, value) =>
                    {
                        if (value == "del")
                        {
                            //stop extend task
                            cts.Cancel();
                            //unsubscribe
                            subscriber.Unsubscribe(subKey);
                        }
                    });

                    Console.WriteLine("task1 get lock");
                    Thread.Sleep(20_000);
                }
                finally
                {
                    if (taken)
                    {
                        await database.LockReleaseAsync(key, 1);
                    }
                }
            });

            Thread.Sleep(5000);

            var t2 = Task.Run(async () =>
            {
                while (true)
                {
                    var taken = await database.LockTakeAsync(key, 2, TimeSpan.FromSeconds(60));
                    if (!taken)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    try
                    {
                        Console.WriteLine("task2 get lock");
                        Thread.Sleep(20_000);
                        return;
                    }
                    finally
                    {
                        if (taken)
                        {
                            await database.LockReleaseAsync(key, 2);
                        }
                    }
                }
            });

            Task.WaitAll(t1, t2);
        }

        public static void LockTakeAsync_FailedWithBlock_Test()
        {
            var t1 = Task.Run(DoSth);
            var t2 = Task.Run(DoSth);
            var t3 = Task.Run(DoSth);
            var t4 = Task.Run(DoSth);

            Task.WaitAll(t1, t2, t3, t4);
        }

        private static int _value;

        private static ManualResetEventSlim? _resetEvent;

        private static async Task DoSth()
        {
            var database = DatabaseProvider.GetDatabase();
            var key = "lock:key3";
            var value = Interlocked.Increment(ref _value);

            if (Interlocked.CompareExchange(ref _resetEvent, new ManualResetEventSlim(false, 100), null) is null)
            {
                var subscriber = database.Multiplexer.GetSubscriber();
                await subscriber.SubscribeAsync($"__keyspace@{database.Database}__:{key}",
                    (channel, redisValue) =>
                    {
                        Console.WriteLine($"{channel.ToString()} {redisValue}");
                        if (redisValue == "del")
                        {
                            Console.WriteLine($"{channel.ToString()} has deleted");
                            _resetEvent.Set();
                        }
                    });
            }

            bool taken;
            do
            {
                taken = await database.LockTakeAsync(key, value, TimeSpan.FromSeconds(10));
                if (!taken)
                {
                    Console.WriteLine($"{value} don't get lock.");

                    //wait untill rlease lock
                    if (!_resetEvent.Wait(TimeSpan.FromSeconds(20)))
                    {
                        Console.WriteLine($"{value} timeout.");
                        break;
                    }
                }
            } while (!taken);

            if (taken)
            {
                _resetEvent.Reset();
                var timer = new Timer(state =>
                {
                    Console.WriteLine($"{value} renewal lock.");
                    database.LockExtend(key, value, TimeSpan.FromSeconds(10));
                }, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

                try
                {
                    Console.WriteLine($"{value} get lock.");
                    Thread.Sleep(10_000);
                }
                finally
                {
                    await database.LockReleaseAsync(key, value);
                    Console.WriteLine($"{value} release lock.");
                    //remove task
                    await timer.DisposeAsync();
                }
            }
        }
    }
}
