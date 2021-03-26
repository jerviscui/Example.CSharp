using System;
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

            await new LogTestService().DoAsync();

            Console.WriteLine("Hello World!");
        }
    }
}
