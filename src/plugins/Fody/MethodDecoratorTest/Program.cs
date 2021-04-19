using System;
using System.Threading.Tasks;

namespace MethodDecoratorTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            await new Test("name", "text").Show().ShowAsync2();
        }
    }
}
