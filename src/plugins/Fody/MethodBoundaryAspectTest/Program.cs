using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common;

namespace MethodBoundaryAspectTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var w = new Stopwatch();

            //w.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    var pp = new PropTest2();
            //    //var _ = pp.Prop;
            //    pp.Prop = "";
            //}
            //Print.Microsecond(w);

            //w.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    var pp = new PropTest();
            //    //var _ = pp.Prop;
            //    pp.Prop = "";
            //}
            //Print.Microsecond(w);

            //var t = new AsyncTest();
            //await t.ContinueTask();
            
            Console.WriteLine("\r\nused by specify method");
            await new AsyncTest2().ContinueTaskAndReturn();

            Console.WriteLine("\r\nused by class with public methods");
            await new AsyncTest3().ContinueTaskAndReturn();

            Console.WriteLine("\r\nused by class with all methods");
            await new AsyncTest4().ContinueTaskAndReturn();
        }
    }
}
