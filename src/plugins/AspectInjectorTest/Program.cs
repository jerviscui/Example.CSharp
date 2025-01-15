using System.Threading.Tasks;

namespace AspectInjectorTest
{
    internal sealed class Program
    {
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        private static async Task Main(string[] args)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            //new PerformanceTests().GetMethod_ReflectionPerformance_Test();
            //new PerformanceTests().TestClass_GetDeclareType_Test();
            PerformanceTests.DelegateCacheTestClass_MethodWithInstance_Test();

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
            //Console.WriteLine($"result {i.ToString()} {Environment.CurrentManagedThreadId.ToString()}");
        }
    }
}
