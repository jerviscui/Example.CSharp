using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace PostSharpTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //1
            //new ServiceC().Do();
            //Console.WriteLine();
            //new ServiceD().Do();

            //2
            //var ser = new LogTestService();
            //await ser.DoAsync();
            //ser.Dispose();

            //3
            //new OnExceptionAspectTest().Test();

            //4
            //var t1 = new OnMethodBoundaryAspectTest();
            //t1.Test1();
            //t1.Test2();
            //new OnMethodBoundaryAspectTest().Test2();

            //5
            //new InstanceScopedTest().Test();
            //new InstanceScopedTest().Test();

            //new InstanceSingletonTest().Test();
            //new InstanceSingletonTest().Test();

            //6
            new CompileInitializeTest().Test();

            Console.WriteLine("Hello World!");
        }
    }
}
