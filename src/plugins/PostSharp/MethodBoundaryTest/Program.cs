using System;
using System.Threading.Tasks;

namespace MethodBoundaryTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            Console.WriteLine("Test");
            new SyncMethodBoundaryTest().Test();
            Console.WriteLine();

            Console.WriteLine("AsyncTest");
            new AsyncMethodBoundaryTest().AsyncTest();
            Console.WriteLine();

            Console.WriteLine("NoneAwaitTaskTest");
            await new AsyncMethodBoundaryTest().NoneAwaitTaskTest();
            Console.WriteLine();

            Console.WriteLine("AwaitTaskTest");
            await new AsyncMethodBoundaryTest().AwaitTaskTest();
            Console.WriteLine();

            Console.WriteLine("ContinuationTest");
            var a = await new TaskMethodBoundaryTest().ContinuationTest();
            Console.WriteLine(a);
        }
    }
}
