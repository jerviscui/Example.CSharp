using System;
using System.Threading.Tasks;

namespace AspectInjectorTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //new PropTest().S = "";
            //throw ArgumentException

            var boundary = new BoundaryAsyncTest();
            await boundary.AwaitTask();
            Console.WriteLine();
            await boundary.ContinueTask();

            //Console.WriteLine();
            //var tt = new AsyncAroundTest();
            //tt.SyncTest();
            //await tt.Test();
            //Console.WriteLine();
            //await tt.AwaitTest();
        }
    }
}
