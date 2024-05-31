using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal sealed class ReaderWriterLockSlimTest
    {
        public static void TryEnterReadLock_Test()
        {
            var rwLock = new ReaderWriterLockSlim();

            using var tokenSource = new CancellationTokenSource(30 * 1000);
            var token = tokenSource.Token;

            var count = 0;

            var tasks = new Task[2];
            for (int i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                tasks[i] = Task.Run(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        if (rwLock.TryEnterReadLock(1000))
                        {
                            try
                            {
                                Thread.Sleep(100);
                                Console.WriteLine($"r{i1}: {count}");
                            }
                            finally
                            {
                                rwLock.ExitReadLock();
                            }
                        }
                        else
                        {
                            Console.WriteLine($"r{i1}: read lock failed");
                        }
                    }
                }, token);
            }

            var write = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (rwLock.TryEnterWriteLock(1000))
                    {
                        try
                        {
                            Thread.Sleep(5000);
                            count += 1;
                        }
                        finally
                        {
                            rwLock.ExitWriteLock();
                        }
                    }
                    else
                    {
                        Console.WriteLine("writer: write lock failed");
                    }

                    Thread.Sleep(1000);
                }
            }, token);

#pragma warning disable CA1843 // Do not use 'WaitAll' with a single task
            Task.WaitAll(write);
#pragma warning restore CA1843 // Do not use 'WaitAll' with a single task
            rwLock.Dispose();
        }

        public static void EnterUpgradeableReadLock_WhenHasRead_Test()
        {
            var rwLock = new ReaderWriterLockSlim();

            var read1 = Task.Run(() =>
            {
                if (rwLock.TryEnterReadLock(1000))
                {
                    try
                    {
                        Console.WriteLine("r1: get read lock");
                        Thread.Sleep(5000);
                    }
                    finally
                    {
                        rwLock.ExitReadLock();
                        Console.WriteLine("r1: exit read lock");
                    }
                }
                else
                {
                    Console.WriteLine("r1: get read lock faliled");
                }
            });

            var read2 = Task.Run(() =>
            {
                Thread.Sleep(5000);
                var i = 10;
                while (i-- > 0)
                {
                    if (rwLock.TryEnterReadLock(1000))
                    {
                        try
                        {
                            Console.WriteLine("r2: get read lock");
                            Thread.Sleep(500);
                        }
                        finally
                        {
                            rwLock.ExitReadLock();
                            Console.WriteLine("r2: exit read lock");
                        }
                    }
                    else
                    {
                        Console.WriteLine("r2: get read lock faliled");
                    }
                }
            });

            Thread.Sleep(1000);
            var upgrade = Task.Run(() =>
            {
                while (true)
                {
                    if (rwLock.TryEnterUpgradeableReadLock(1000))
                    {
                        try
                        {
                            Console.WriteLine("get upgradeable lock");
                            Thread.Sleep(5000);

                            Console.WriteLine("will get write lock");
                            if (rwLock.TryEnterWriteLock(1000))
                            {
                                try
                                {
                                    Console.WriteLine("get write lock");
                                    Thread.Sleep(5000);
                                }
                                finally
                                {
                                    rwLock.ExitWriteLock();
                                    Console.WriteLine("exit write lock");
                                }
                            }
                            else
                            {
                                Console.WriteLine("get write lock faliled");
                            }

                            return;
                        }
                        finally
                        {
                            rwLock.ExitUpgradeableReadLock();
                            Console.WriteLine("upgradeable lock exited");
                        }
                    }

                    Console.WriteLine("upgradeable lock failed");
                }
            });

            Task.WaitAll(read1, read2, upgrade);
            rwLock.Dispose();

            //执行顺序：read1 -> upgrade -> read2.EnterRead -> read2.ExitRead -> upgrade.EnterWrite
            //r1: get read lock
            //get upgradeable lock
            //r2: get read lock
            //r1: exit read lock
            //r2: exit read lock
            //r2: get read lock
            //will get write lock
            //r2: exit read lock
            //get write lock
            //r2: get read lock faliled
            //exit write lock
            //upgradeable lock exited
            //r2: get read lock
            //r2: exit read lock
        }

        public static void EnterUpgradeableReadLock_OnlyOne_Test()
        {
            var rwLock = new ReaderWriterLockSlim();

            var upgrade1 = Task.Run(() =>
            {
                while (true)
                {
                    if (rwLock.TryEnterUpgradeableReadLock(1000))
                    {
                        try
                        {
                            Console.WriteLine("1: get upgradeable lock");
                            Thread.Sleep(5000);
                            return;
                        }
                        finally
                        {
                            rwLock.ExitUpgradeableReadLock();
                            Console.WriteLine("1: upgradeable lock exited");
                        }
                    }

                    Console.WriteLine("1: upgradeable lock failed");
                }
            });

            var upgrade2 = Task.Run(() =>
            {
                while (true)
                {
                    if (rwLock.TryEnterUpgradeableReadLock(1000))
                    {
                        try
                        {
                            Console.WriteLine("2: get upgradeable lock");
                            Thread.Sleep(5000);
                            return;
                        }
                        finally
                        {
                            rwLock.ExitUpgradeableReadLock();
                            Console.WriteLine("2: upgradeable lock exited");
                        }
                    }

                    Console.WriteLine("2: upgradeable lock failed");
                }
            });

            Task.WaitAll(upgrade1, upgrade2);
            rwLock.Dispose();

            //2: get upgradeable lock
            //1: upgradeable lock failed
            //2: upgradeable lock exited
            //1: get upgradeable lock
            //1: upgradeable lock exited
        }

        public static void EnterUpgradeableReadLock_WhenHasWrite_Test()
        {
            var rwLock = new ReaderWriterLockSlim();

            var write1 = new Task(() =>
            {
                while (true)
                {
                    if (rwLock.TryEnterWriteLock(100))
                    {
                        try
                        {
                            Console.WriteLine("w1: get write lock");
                            Thread.Sleep(5000);
                            return;
                        }
                        finally
                        {
                            rwLock.ExitWriteLock();
                            Console.WriteLine("w1: exit write lock");
                        }
                    }
                    Console.WriteLine("w1: get write lock faliled");
                }
            });

            var write2 = new Task(() =>
            {
                rwLock.EnterWriteLock();
                var locked = true;
                Console.WriteLine("w2: get write lock");

                try
                {
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (locked)
                    {
                        rwLock.ExitWriteLock();
                        Console.WriteLine("w2: exit write lock");
                    }
                }
            });

            var upgrade = new Task(() =>
            {
                rwLock.EnterUpgradeableReadLock();
                var locked = true;

                try
                {
                    Console.WriteLine("get upgradeable lock");
                    Thread.Sleep(5000);

                    Console.WriteLine("will get write lock");
                    if (rwLock.TryEnterWriteLock(1000))
                    {
                        try
                        {
                            Console.WriteLine("get write lock");
                            Thread.Sleep(5000);
                        }
                        finally
                        {
                            rwLock.ExitWriteLock();
                            Console.WriteLine("exit write lock");
                        }
                    }
                    else
                    {
                        Console.WriteLine("get write lock faliled");
                    }
                }
                finally
                {
                    if (locked)
                    {
                        rwLock.ExitUpgradeableReadLock();
                        Console.WriteLine("upgradeable lock exited");
                    }
                }
            });

            //write1.Start();
            //Thread.Sleep(100);
            //upgrade.Start();
            //Thread.Sleep(100);
            //write2.Start();
            //获得锁顺序：write1 -> write2 -> upgrade

            upgrade.Start();
            Thread.Sleep(100);
            write1.Start();
            write2.Start();
            //获得锁顺序：upgrade -> write2 -> write1

            Task.WaitAll(write1, write2, upgrade);
            rwLock.Dispose();
        }

        public static void ExitUpgradeableReadLock_Test()
        {
            var rwLock = new ReaderWriterLockSlim();

            var write1 = new Task(() =>
            {
                rwLock.EnterWriteLock();
                var locked = true;
                Console.WriteLine("w1: get write lock");

                try
                {
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (locked)
                    {
                        rwLock.ExitWriteLock();
                        Console.WriteLine("w1: exit write lock");
                    }
                }
            });

            var read1 = new Task(() =>
            {
                rwLock.EnterReadLock();
                var locked = true;
                Console.WriteLine("r1: get read lock");

                try
                {
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (locked)
                    {
                        rwLock.ExitReadLock();
                        Console.WriteLine("r1: exit read lock");
                    }
                }
            });

            var upgrade = new Task(() =>
            {
                rwLock.EnterUpgradeableReadLock();
                var locked = true;

                try
                {
                    Console.WriteLine("get upgradeable lock");
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (locked)
                    {
                        rwLock.ExitUpgradeableReadLock();
                        Console.WriteLine("upgradeable lock exited");
                    }
                }
            });

            var upgrade2 = new Task(() =>
            {
                rwLock.EnterUpgradeableReadLock();
                var locked = true;

                try
                {
                    Console.WriteLine("u2: get upgradeable lock");
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (locked)
                    {
                        rwLock.ExitUpgradeableReadLock();
                        Console.WriteLine("u2: upgradeable lock exited");
                    }
                }
            });

            upgrade.Start();
            Thread.Sleep(100);
            upgrade2.Start();
            write1.Start();
            read1.Start();

            Task.WaitAll(write1, read1, upgrade, upgrade2);
            rwLock.Dispose();
        }

        public static void Upgradeable_ToRead_Test()
        {
            var rwLock = new ReaderWriterLockSlim();

            var t = Task.Run(() =>
            {
                rwLock.EnterUpgradeableReadLock();

                try
                {
                    Console.WriteLine(
                        $"CurrentReadCount: {rwLock.CurrentReadCount}\tWaitingReadCount: {rwLock.WaitingReadCount}\tWaitingUpgradeCount: {rwLock.WaitingUpgradeCount}");

                    rwLock.EnterReadLock();
                    Console.WriteLine(
                        $"CurrentReadCount: {rwLock.CurrentReadCount}\tWaitingReadCount: {rwLock.WaitingReadCount}\tWaitingUpgradeCount: {rwLock.WaitingUpgradeCount}");

                    rwLock.ExitUpgradeableReadLock();
                    Console.WriteLine(
                        $"CurrentReadCount: {rwLock.CurrentReadCount}\tWaitingReadCount: {rwLock.WaitingReadCount}\tWaitingUpgradeCount: {rwLock.WaitingUpgradeCount}");
                }
                finally
                {
                    rwLock.ExitReadLock();
                    Console.WriteLine(
                        $"CurrentReadCount: {rwLock.CurrentReadCount}\tWaitingReadCount: {rwLock.WaitingReadCount}\tWaitingUpgradeCount: {rwLock.WaitingUpgradeCount}");
                }
            });

#pragma warning disable CA1843 // Do not use 'WaitAll' with a single task
            Task.WaitAll(t);
#pragma warning restore CA1843 // Do not use 'WaitAll' with a single task
            rwLock.Dispose();
        }
    }
}
