using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncEnumerableTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await foreach (var i in GenerateSequence())
            {
                Console.WriteLine(i);
            }
        }

        static async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
    }
}
