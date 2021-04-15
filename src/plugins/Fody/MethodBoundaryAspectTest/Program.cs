using System;
using System.Threading.Tasks;

namespace MethodBoundaryAspectTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var t = new TaskTest();
            await t.Test();

            Console.WriteLine();
            await t.AwaitTest();
        }
    }
}
