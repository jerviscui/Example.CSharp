using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Common;

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

            //单线程插入 5 倍左右差距
            //conDic:    659,331 us
            //dic   :    128,337 us
        }

        public void MultiThreads_Test()
        {
            Run(1, 10000, 10);
            Run(10, 10000, 10);
            Run(100, 10000, 10);
            Run(1000, 10000, 10);

            //并发越高 ConcurrentDictionary 性能越好
            //Threads: 1, items: 10000, cycles:10
            //conDic:         19 us
            //dic   :          2 us
            //Threads: 10, items: 10000, cycles:10
            //conDic:         13 us
            //dic   :          2 us
            //Threads: 100, items: 10000, cycles:10
            //conDic:          6 us
            //dic   :          8 us
            //Threads: 1000, items: 10000, cycles:10
            //conDic:         30 us
            //dic   :  5,701,567 us
        }

        private void Run(int threads, int count, int cycles)
        {
            Console.WriteLine("");
            Console.WriteLine($"Threads: {threads}, items: {count}, cycles:{cycles}");

            var semaphore = new SemaphoreSlim(0, threads);
            //run ConcurrentDictionary
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(() => Run(concurrentDictionary, count, cycles, semaphore));
                t.Start();
            }

            semaphore.Release(threads);
            Thread.Sleep(threads > 100 ? 5000 : 1000);

            var w = Stopwatch.StartNew();
            for (int i = 0; i < threads; i++)
            {
                semaphore.Wait();
            }
            w.Stop();
            Print.Microsecond(w, "conDic:");

            //run Dictionary
            var dictionary = new Dictionary<int, string>();
            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(() => Run(dictionary, count, cycles, semaphore));
                t.Start();
            }

            semaphore.Release(threads);
            Thread.Sleep(threads > 100 ? 5000 : 1000);

            w.Restart();
            for (int i = 0; i < threads; i++)
            {
                semaphore.Wait();
            }
            w.Stop();
            Print.Microsecond(w, "dic   :");

            semaphore.Dispose();
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
                        dic.TryAdd(j, j.ToString());

                        //get or add
                        //dic.GetOrAdd(j, j1 => j1.ToString());
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
                        //ThrowInvalidOperationException_ConcurrentOperationsNotSupported
                        //resolve by lock
                        lock (dic)
                        {
                            dic.TryAdd(j, j.ToString());
                        }

                        //Stupid，不能并发问题
                        //try
                        //{
                        //    dic.TryAdd(j, j.ToString());
                        //}
                        //catch (InvalidOperationException)
                        //{
                        //    dic.TryGetValue(j, out string? value);
                        //    Console.WriteLine(value ?? "null");
                        //}

                        //get or add
                        //if (!dic.TryGetValue(j, out string? value))
                        //{
                        //    lock (dic)
                        //    {
                        //        if (!dic.TryGetValue(j, out value))
                        //        {
                        //            dic[i] = value = j.ToString();
                        //        }
                        //    }
                        //}
                        //var x = value;
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
