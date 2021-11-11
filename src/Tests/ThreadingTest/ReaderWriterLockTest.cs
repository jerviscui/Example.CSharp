using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal class ReaderWriterLockTest
    {
        public static void ReaderWriterLock_Test()
        {
            var rwLock = new ReaderWriterLock();

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
                        var locked = false;
                        try
                        {
                            rwLock.AcquireReaderLock(1000);
                            locked = true;
                            Thread.Sleep(100);
                            Console.WriteLine($"r{i1}: {count}");
                        }
                        catch (ApplicationException e)
                        {
                            Console.WriteLine($"r{i1}: {e.Message}");
                        }
                        finally
                        {
                            if (locked)
                            {
                                locked = false;
                                rwLock.ReleaseReaderLock();
                            }
                        }
                    }
                }, token);
            }

            var write = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var locked = false;
                    try
                    {
                        rwLock.AcquireWriterLock(1000);
                        locked = true;
                        Thread.Sleep(5000);
                        count += 1;
                    }
                    catch (ApplicationException e)
                    {
                        Console.WriteLine($"writer: {e.Message}");
                    }
                    finally
                    {
                        if (locked)
                        {
                            locked = false;
                            rwLock.ReleaseWriterLock();
                        }
                    }

                    Thread.Sleep(1000);
                }
            }, token);

            Task.WaitAll(write);
        }

        public static void UpgradeToWriterLock_Test()
        {
            var rwLock = new ReaderWriterLock();

            using var tokenSource = new CancellationTokenSource(30 * 1000);
            var token = tokenSource.Token;
            var count = 0;

            var rwTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    int locked = 0;
                    try
                    {
                        rwLock.AcquireReaderLock(1000);
                        locked = 1;
                        Console.WriteLine($"r1: {count}");

                        LockCookie lc = default;
                        try
                        {
                            lc = rwLock.UpgradeToWriterLock(1000);
                            locked = 2;
                            Console.WriteLine("r1: get write lock.");

                            Thread.Sleep(4000);
                            count++;
                        }
                        catch (ApplicationException e)
                        {
                            Console.WriteLine($"rw: {e.Message}");
                        }
                        finally
                        {
                            if (locked == 2)
                            {
                                locked = 1;
                                rwLock.DowngradeFromWriterLock(ref lc);
                            }
                        }
                    }
                    catch (ApplicationException e)
                    {
                        Console.WriteLine($"r1: {e.Message}");
                    }
                    finally
                    {
                        if (locked == 1)
                        {
                            locked = 0;
                            rwLock.ReleaseReaderLock();
                        }
                    }
                }
            }, token);

            var read = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var locked = false;
                    try
                    {
                        rwLock.AcquireReaderLock(1000);
                        locked = true;

                        Console.WriteLine($"r2: {count}");
                        Thread.Sleep(3000);
                    }
                    catch (ApplicationException e)
                    {
                        Console.WriteLine($"r2: {e.Message}");
                    }
                    finally
                    {
                        if (locked)
                        {
                            locked = false;
                            rwLock.ReleaseReaderLock();
                        }
                    }
                }
            }, token);

            Task.WaitAll(rwTask, read);
        }

        public static void ReleaseLock_Test()
        {
            var rwLock = new ReaderWriterLock();

            using var tokenSource = new CancellationTokenSource(30 * 1000);
            var token = tokenSource.Token;

            var count = 0;

            var read = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var locked = false;
                    try
                    {
                        Console.WriteLine("-------");

                        rwLock.AcquireReaderLock(1000);
                        locked = true;

                        var tmp = count;
                        Console.WriteLine($"read: {tmp}");

                        var seq = rwLock.WriterSeqNum;
                        var lc = rwLock.ReleaseLock();
                        Console.WriteLine("release");

                        Thread.Sleep(1000);
                        rwLock.RestoreLock(ref lc);
                        Console.WriteLine("restore");

                        if (rwLock.AnyWritersSince(seq))
                        {
                            Console.WriteLine("count is changed");
                            tmp = count;
                        }
                        Console.WriteLine($"read: {tmp}");
                    }
                    catch (ApplicationException e)
                    {
                        Console.WriteLine($"read: {e.Message}");
                    }
                    finally
                    {
                        if (locked)
                        {
                            locked = false;
                            rwLock.ReleaseReaderLock();
                        }
                    }
                }
            }, token);

            var write = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var locked = false;
                    try
                    {
                        rwLock.AcquireWriterLock(1000);
                        locked = true;

                        count += 1;
                    }
                    catch (ApplicationException e)
                    {
                        Console.WriteLine($"writer: {e.Message}");
                    }
                    finally
                    {
                        if (locked)
                        {
                            locked = false;
                            rwLock.ReleaseWriterLock();
                        }
                    }

                    Thread.Sleep(5000);
                }
            }, token);

            Task.WaitAll(read, write);
        }
    }
}
