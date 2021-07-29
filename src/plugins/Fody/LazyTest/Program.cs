using Lazy;
using System;
using System.Threading;

namespace LazyTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var t = new Test();
            Console.WriteLine(t.StrService);

            Thread.Sleep(1000);

            Console.WriteLine(t.StrService);
        }
    }

    public class Test
    {
        [Lazy]
        public string StrService => $"{DateTime.Now:O}";
    }
}
