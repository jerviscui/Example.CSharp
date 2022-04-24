using System;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal class TaskDelayTest
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
            ExTask();
        }

        private static async Task ExTask()
        {
            await Task.Delay(1000 * 2);
            throw new Exception("test exception");
        }

        public static void Exception_Cath_Test()
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
