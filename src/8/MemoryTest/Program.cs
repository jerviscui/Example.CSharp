using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MemoryTest
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            var list = new[]
            {
                new A { Name = "aaa", Age = 10 }, new A { Name = "bbb", Age = 11 },
                new A { Name = "ccc", Age = 12 }, new A { Name = "ddd", Age = 13 },
                new A { Name = "eee", Age = 14 }, new A { Name = "fff", Age = 15 }
            };

            await Print(list, 2, 2);
            await Print(new Memory<A>(list, 2, 2));
            Console.WriteLine();

            var isBisect = args.Length > 0 && args[0] == "1";

            var s = new S();
            //single thread
            var r1 = ComputeAvg(1, s, isBisect);
            Console.WriteLine(r1);
            //two threads
            var r2 = ComputeAvg(2, s, isBisect);
            Console.WriteLine(r2);
            //four threads
            var r4 = ComputeAvg(4, s, isBisect);
            Console.WriteLine(r4);

            Console.WriteLine();
            var ss = new Ss();
            var rr1 = ComputeAvg(1, ss, isBisect);
            Console.WriteLine(rr1);
            var rr2 = ComputeAvg(2, ss, isBisect);
            Console.WriteLine(rr2);
            var rr4 = ComputeAvg(4, ss, isBisect);
            Console.WriteLine(rr4);

            Console.WriteLine();
            var st = new St();
            var rrr1 = ComputeAvg(1, st, isBisect);
            Console.WriteLine(rrr1);
            var rrr2 = ComputeAvg(2, st, isBisect);
            Console.WriteLine(rrr2);
            var rrr4 = ComputeAvg(4, st, isBisect);
            Console.WriteLine(rrr4);
        }

#pragma warning disable 1998
        private static async Task Print(A[] arr, int start, int len)
#pragma warning restore 1998
        {
            for (int i = start; i < arr.Length && i < start + len; i++)
            {
                arr[i].Name += i;

                Console.WriteLine($"{arr[i].Name},{arr[i].Age}");
            }
        }

        private static async Task Print(Memory<A> mem)
        {
            for (int i = 0; i < mem.Length; i++)
            {
                //Span<A> a = mem.Span;//Span 不能用于异步

                mem.Span[i].Name += i;

                await Task.Delay(10);

                Console.WriteLine($"{mem.Span[i].Name},{mem.Span[i].Age}");
            }
        }

        private class A
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        #region StructLayout

        //todo: 管理类的内存分配大小，测试没有体现出预期的效果
        public static (double sum, double time) ComputeAvg(int thread, object obj, bool bisect = true)
        {
            var results = new (long sum, double time)[10];

            for (int i = 0; i < 10; i++)
            {
                results[i] = Compute(thread, obj, bisect);
            }

            return (results.Average(tuple => tuple.sum), results.Average(tuple => tuple.time));
        }

        public static (long sum, double time) Compute(int thread, object obj, bool bisect = true)
        {
            const long count = 5_000_000_000;

            var times = bisect ? count / thread : count;

            var values = new long[thread];
            var tasks = new Task[thread];

            var type1 = obj is S;
            var type2 = obj is Ss;
            var type3 = obj is St;

            for (int i = 0; i < thread; i++)
            {
                var ii = i;
                tasks[i] = new Task(() =>
                {
                    long v = 0;

                    if (type1)
                    {
                        var s1 = (S)obj;
                        for (long j = 0; j < times; j++)
                        {
                            v += s1.A;
                            //s1.A = s1.A + 1;
                        }
                    }
                    else if (type2)
                    {
                        var s2 = (Ss)obj;
                        for (long j = 0; j < times; j++)
                        {
                            v += s2.A;
                        }
                    }
                    else if (type3)
                    {
                        var s3 = (St)obj;
                        for (long j = 0; j < times; j++)
                        {
                            v += s3.A;
                        }
                    }

                    values[ii] = v;
                }, TaskCreationOptions.LongRunning);
            }

            var watch = new Stopwatch();
            watch.Start();

            foreach (var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks);
            watch.Stop();

            return (values.Sum(), watch.Elapsed.TotalMilliseconds);
        }

        private class S
        {
            public readonly long A = 1;
        }

        [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
        private class Ss
        {
            public readonly long A = 1;

            private readonly long _p1, _p2, _p3, _p4, _p5, _p6, _p7;

            private readonly long _p9, _p10, _p11, _p12, _p13, _p14, _p15 /*, p16*/;
        }

        [StructLayout(LayoutKind.Explicit, Size = 120)]
        private class St
        {
            [FieldOffset(56)]
            public readonly long A = 1;
        }

        #endregion
    }
}
