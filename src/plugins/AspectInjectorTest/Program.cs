using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspectInjectorTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //new PerformanceTests().GetMethod_ReflectionPerformance_Test();
            //new PerformanceTests().TestClass_GetDeclareType_Test();
            new PerformanceTests().DelegateCacheTestClass_MethodWithInstance_Test();

            //new PropTest().S = "";
            //throw ArgumentException

            //var boundary = new BoundaryAsyncTest();
            //await boundary.AwaitTask();
            //Console.WriteLine();
            //await boundary.ContinueTask();

            //var around = new AroundAsyncTest();
            //around.AsyncMethod();

            //Console.WriteLine();
            //await around.TaskMehtod();

            //Console.WriteLine();
            //await around.AwaitTask();

            //Console.WriteLine();
            //var i = await around.ContinueTask();
            //Console.WriteLine($"result {i.ToString()} {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }
    }
}
