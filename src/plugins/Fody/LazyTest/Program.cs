using System;
using System.Threading;
using Lazy;

namespace LazyTest
{
    internal sealed class Program
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
