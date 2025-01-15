using System;
using System.Threading.Tasks;

namespace MethodDecoratorTest
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await new Test("name", "text").Show().ShowAsync2();
        }
    }
}
