using System;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsyncExConsoleTest
{
    internal sealed class AsyncContextTest
    {
        public static void Run_Task_Test()
        {
            var r = AsyncContext.Run(async () =>
            {
                await Task.Delay(1000);
                return 100;
            });

            Console.WriteLine(r);
        }

        public static void Run_Test()
        {
            var r = AsyncContext.Run(() =>
            {
                Thread.Sleep(1000);
                return 100;
            });

            Console.WriteLine(r);
        }
    }
}
