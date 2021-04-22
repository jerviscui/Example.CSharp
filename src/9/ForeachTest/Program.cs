using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ForeachTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IEnumerableForeachTest();
            IListForeachTest();
            ListForeachTest();

            //list dictionary array set queue

            Console.ReadKey();
        }

        public static void IEnumerableForeachTest()
        {
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in GetArray())
                {
                    var a = item + 1;
                }
                watch.Stop();
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds}");
            }

            Console.WriteLine("IEnumerable---------------");
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in GetEnumerable())
                {
                    var a = item + 1;
                }
                watch.Stop();
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds}");
            }

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
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in GetIList())
                {
                    var a = item + 1;
                }
                watch.Stop();
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds}");
            }
            
            IList<int> GetIList()
            {
                return Enumerable.Range(1, 5_000_000).ToList();
            }
        }

        public static void ListForeachTest()
        {
            Console.WriteLine("List---------------");
            for (int i = 0; i < 10; i++)
            {
                var watch = Stopwatch.StartNew();
                foreach (var item in GetIList())
                {
                    var a = item + 1;
                }
                watch.Stop();
                Console.WriteLine($"i={i},时间:{watch.ElapsedMilliseconds}");
            }
            
            List<int> GetIList()
            {
                return Enumerable.Range(1, 5_000_000).ToList();
            }
        }
    }
}
