using System;
using System.Threading.Tasks;

namespace AspectInjectorTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //new PropTest().S = "";
            //throw ArgumentException

            var t = new AsyncTest();

            await t.Test();
            await t.AwaitTest();

        }
    }
}
