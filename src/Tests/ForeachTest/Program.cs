using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BenchmarkDotNet.Running;

namespace ForeachTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ToArray 快
            BenchmarkRunner.Run<ToArrayAndListTests>();

            //array 最快，IEnumerable IList 有装箱，
            BenchmarkRunner.Run<ForeachTest>();

            //Test();
        }

        public static void Test()
        {
            IEnumerableForeachTest();
            IListForeachTest();
            ListForeachTest();

            // array 最快，IEnumerable IList 有装箱，
            //i=0,时间:1
            //i=1,时间:1
            //i=2,时间:1
            //i=3,时间:1
            //i=4,时间:1
            //i=5,时间:1
            //i=6,时间:1
            //i=7,时间:1
            //i=8,时间:1
            //i=9,时间:1
            //avg:1
            //IEnumerable---------------
            //i=0,时间:20
            //i=1,时间:20
            //i=2,时间:20
            //i=3,时间:20
            //i=4,时间:20
            //i=5,时间:20
            //i=6,时间:20
            //i=7,时间:20
            //i=8,时间:20
            //i=9,时间:19
            //avg:19.9
            //IList---------------
            //i=0,时间:33
            //i=1,时间:33
            //i=2,时间:33
            //i=3,时间:32
            //i=4,时间:33
            //i=5,时间:32
            //i=6,时间:32
            //i=7,时间:31
            //i=8,时间:31
            //i=9,时间:31
            //avg:32.1
            //List---------------
            //i=0,时间:10
            //i=1,时间:9
            //i=2,时间:10
            //i=3,时间:9
            //i=4,时间:9
            //i=5,时间:10
            //i=6,时间:10
            //i=7,时间:10
            //i=8,时间:10
            //i=9,时间:10
            //avg:9.7

            //list dictionary array set queue
        }

        public static void IEnumerableForeachTest()
        {
            long t1 = 0;
            var array = GetArray();
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in array)
                {
                    var a = item + 1;
                }
                watch.Stop();
                t1 += watch.ElapsedMilliseconds;
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds.ToString()}");
            }
            Console.WriteLine($"avg:{t1 / 10.0}");

            Console.WriteLine("IEnumerable---------------");
            long t2 = 0;
            var iEnumerable = GetEnumerable();
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in iEnumerable)
                {
                    var a = item + 1;
                }
                watch.Stop();
                t2 += watch.ElapsedMilliseconds;
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds.ToString()}");
            }
            Console.WriteLine($"avg:{t2 / 10.0}");

            int[] GetArray()
            {
                return Enumerable.Range(1, 5_000_000).ToArray();
            }

            IEnumerable<int> GetEnumerable()
            {
                return Enumerable.Range(1, 5_000_000);
            }
        }

        public static void IListForeachTest()
        {
            Console.WriteLine("IList---------------");
            long t2 = 0;
            var iList = GetIList();
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in iList)
                {
                    var a = item + 1;
                }
                watch.Stop();
                t2 += watch.ElapsedMilliseconds;
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds.ToString()}");
            }
            Console.WriteLine($"avg:{t2 / 10.0}");

            IList<int> GetIList()
            {
                return Enumerable.Range(1, 5_000_000).ToList();
            }
        }

        public static void ListForeachTest()
        {
            Console.WriteLine("List---------------");
            long t2 = 0;
            var list = GetList();
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in list)
                {
                    var a = item + 1;
                }
                watch.Stop();
                t2 += watch.ElapsedMilliseconds;
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds.ToString()}");
            }
            Console.WriteLine($"avg:{t2 / 10.0}");

            List<int> GetList()
            {
                return Enumerable.Range(1, 5_000_000).ToList();
            }
        }
    }
}
