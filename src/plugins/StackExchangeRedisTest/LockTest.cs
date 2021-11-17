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

            var key = "lock:key1";

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
                            Thread.Sleep(5000);

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

            //var timer = new Timer();
            //timer.Change(TimeSpan.MaxValue, )

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
    }
}
