using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var list = new A[]
            {
                new A(){Name = "aaa", Age = 10},
                new A(){Name = "bbb", Age = 11},
                new A(){Name = "ccc", Age = 12},
                new A(){Name = "ddd", Age = 13},
                new A(){Name = "eee", Age = 14},
                new A(){Name = "fff", Age = 15}
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
            var ss = new SS();
            var rr1 = ComputeAvg(1, ss, isBisect);
            Console.WriteLine(rr1);
            var rr2 = ComputeAvg(2, ss, isBisect);
            Console.WriteLine(rr2);
            var rr4 = ComputeAvg(4, ss, isBisect);
            Console.WriteLine(rr4);

            Console.WriteLine();
            var st = new ST();
            var rrr1 = ComputeAvg(1, st, isBisect);
            Console.WriteLine(rrr1);
            var rrr2 = ComputeAvg(2, st, isBisect);
            Console.WriteLine(rrr2);
            var rrr4 = ComputeAvg(4, st, isBisect);
            Console.WriteLine(rrr4);
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
            var type2 = obj is SS;
            var type3 = obj is ST;

            for (int i = 0; i < thread; i++)
            {
                var ii = i;
                tasks[i] = new Task(() =>
                {
                    long v = 0;

                    if (type1)
                    {
                        var s1 = (S) obj;
                        for (long j = 0; j < times; j++)
                        {
                            v += s1.A;
                            //s1.A = s1.A + 1;
                        }
                    }
                    else if (type2)
                    {
                        var s2 = (SS) obj;
                        for (long j = 0; j < times; j++)
                        {
                            v += s2.A;
                        }
                    }
                    else if (type3)
                    {
                        var s3 = (ST) obj;
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
        
        class S
        {
            public long A = 1;
        }

        class SS
        {
            private long p1, p2, p3, p4, p5, p6, p7;
            public long A = 1;
            private long p9, p10, p11, p12, p13, p14, p15/*, p16*/;
        }

        [StructLayout(LayoutKind.Explicit, Size = 120)]
        class ST
        {
            [FieldOffset(56)]
            public long A = 1;
        }

        #endregion

        class A
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        static async Task Print(A[] arr, int start, int len)
        {
            for (int i = start; i < arr.Length && i < start + len; i++)
            {
                arr[i].Name += i;

                Console.WriteLine($"{arr[i].Name},{arr[i].Age}");
            }
        }

        static async Task Print(Memory<A> mem)
        {
            for (int i = 0; i < mem.Length; i++)
            {
                //Span<A> a = mem.Span;//Span 不能用于异步

                mem.Span[i].Name += i;

                await Task.Delay(10);

                Console.WriteLine($"{mem.Span[i].Name},{mem.Span[i].Age}");
            }
        }

        void Test()
        {
            var s = "aaa";
            ref string a = ref s;

            //new Span<char>(ref a, 3);
        }
    }
}
