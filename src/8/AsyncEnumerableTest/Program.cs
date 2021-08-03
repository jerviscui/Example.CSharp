using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncEnumerableTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await foreach (var i in GenerateSequence())
            {
                Console.WriteLine(i);
            }
        }

        private static async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
    }
}
