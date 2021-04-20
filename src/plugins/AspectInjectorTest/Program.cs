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

            var t = new AsyncBoundaryTest();
            await t.Test();
            Console.WriteLine();
            await t.AwaitTest();

            Console.WriteLine();
            var tt = new AsyncAroundTest();
            tt.SyncTest();

            await tt.Test();
            Console.WriteLine();
            await tt.AwaitTest();
        }
    }
}
