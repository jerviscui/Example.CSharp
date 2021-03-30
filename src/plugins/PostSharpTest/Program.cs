using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace PostSharpTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //new ServiceC().Do();
            //Console.WriteLine();
            //new ServiceD().Do();

            //var ser = new LogTestService();
            //await ser.DoAsync();
            //ser.Dispose();

            //new OnExceptionAspectTest().Test();

            //var t1 = new OnMethodBoundaryAspectTest();
            //t1.Test1();
            //t1.Test2();
            //new OnMethodBoundaryAspectTest().Test2();

            new InstanceScopedTest().Test();
            new InstanceScopedTest().Test();

            new InstanceSingletonTest().Test();
            new InstanceSingletonTest().Test();

            Console.WriteLine("Hello World!");
        }
    }
}
