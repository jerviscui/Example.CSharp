using System;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal sealed class TaskDelayTest
    {
        public static void Test()
        {
            var task = Task.Run(async () =>
            {
                await Task.Delay(1000 * 10);
                return 100;
            });

            task.Wait();
            var r = task.Result;
        }

        public static void Exception_Test()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ExTask();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private static async Task ExTask()
        {
            await Task.Delay(1000 * 2);
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new Exception("test exception");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }

        public static void Exception_Catch_Test()
        {
            ExTask().ContinueWith(task =>
            {
                try
                {
                    task.Wait();
                }
                catch (Exception ex)
                {
                    if (ex is AggregateException e)
                    {
                        foreach (var inner in e.InnerExceptions)
                        {
                            Console.WriteLine(inner.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }
    }
}
