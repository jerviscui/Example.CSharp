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

            //var boundary = new BoundaryAsyncTest();
            //await boundary.AwaitTask();
            //Console.WriteLine();
            //await boundary.ContinueTask();

            var around = new AroundAsyncTest();
            around.AsyncMethod();

            Console.WriteLine();
            await around.TaskMehtod();

            Console.WriteLine();
            await around.AwaitTask();

            Console.WriteLine();
            var i = await around.ContinueTask();
            Console.WriteLine($"result {i.ToString()}");
        }
    }
}
