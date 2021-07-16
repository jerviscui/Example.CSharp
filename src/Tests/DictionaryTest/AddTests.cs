using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DictionaryTest
{
    public class AddTests
    {
        public void OneThread_Test()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var d = new ConcurrentDictionary<int, int>();
            for (int i = 0; i < 1000000; i++)
            {
                d[i] = 123;
            }

            for (int i = 1000000; i < 2000000; i++)
            {
                d[i] = 123;
            }

            for (int i = 2000000; i < 3000000; i++)
            {
                d[i] = 123;
            }

            sw.Stop();
            Print.Microsecond(sw, "conDic:");

            sw.Restart();
            var d2 = new Dictionary<int, int>();
            for (int i = 0; i < 1000000; i++)
            {
                lock (d2)
                {
                    d2[i] = 123;
                }
            }

            for (int i = 1000000; i < 2000000; i++)
            {
                lock (d2)
                {
                    d2[i] = 123;
                }
            }

            for (int i = 2000000; i < 3000000; i++)
            {
                lock (d2)
                {
                    d2[i] = 123;
                }
            }

            sw.Stop();
            Print.Microsecond(sw, "dic   :");

            //单线程插入 6 倍左右差距
            //conDic:    983,642 us
            //dic   :    171,558 us
        }

        public void MultiThreads_Test()
        {
            Run(1, 100000, 10);
            Run(10, 100000, 10);
            Run(100, 100000, 10);
            Run(1000, 100000, 10);
        }

        private void Run(int threads, int count, int cycles)
        {
            //todo: fix
            Console.WriteLine("");
            Console.WriteLine($"Threads: {threads}, items: {count}, cycles:{cycles}");

            var semaphore = new SemaphoreSlim(0, threads);
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(() => Run(concurrentDictionary, count, cycles, semaphore));
                t.Start();
            }

            Thread.Sleep(1000);

            var w = Stopwatch.StartNew();
            semaphore.Release(threads);
            for (int i = 0; i < threads; i++)
            {
                semaphore.Wait();
            }

            Print.Microsecond(w, "conDic:");

            var dictionary = new Dictionary<int, string>();
            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(() => Run(dictionary, count, cycles, semaphore));
                t.Start();
            }

            Thread.Sleep(1000);

            w.Restart();
            semaphore.Release(threads);
            for (int i = 0; i < threads; i++)
            {
                semaphore.Wait();
            }

            Print.Microsecond(w, "dic   :");
        }

        private void Run(ConcurrentDictionary<int, string> dic, int elements, int cycles, SemaphoreSlim semaphore)
        {
            semaphore.Wait();
            try
            {
                for (int i = 0; i < cycles; i++)
                {
                    for (int j = 0; j < elements; j++)
                    {
                        var x = dic.GetOrAdd(i, x => x.ToString());
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void Run(Dictionary<int, string> dic, int elements, int cycles, SemaphoreSlim semaphore)
        {
            semaphore.Wait();
            try
            {
                for (int i = 0; i < cycles; i++)
                {
                    for (int j = 0; j < elements; j++)
                    {
                        if (!dic.TryGetValue(i, out string? value))
                        {
                            lock (dic)
                            {
                                if (!dic.TryGetValue(i, out value))
                                {
                                    dic[i] = value = i.ToString();
                                }
                            }
                        }

                        var x = value;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
