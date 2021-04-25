using System;
using System.Threading;
using System.Threading.Tasks;

namespace LocalFunctionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //LocalFunctionWithEnumerable.Test();
            //await LocalFunctionWithTask.Test();

            TaskTest.SimpleTask().GetAwaiter().GetResult();
        }

        class TaskTest
        {
            public static async Task SimpleTask()
            {
                Console.WriteLine("SimpleTask");
            }

            public static async Task<int> GenericTask()
            {
                Console.WriteLine("GenericTask");
                return await Task.FromResult(3);
            }

            public static async ValueTask<int> ValueTask()
            {
                return await System.Threading.Tasks.ValueTask.FromResult(3);
            }
        }

        class SyncTaskTest
        {
            public static Task SyncTask()
            {
                Console.WriteLine("test");
                return Task.CompletedTask;
            }

            public static Task<int> SyncGenericTask()
            {
                return Task.FromResult(3);
            }

            public static ValueTask<int> SyncValueTask()
            {
                return ValueTask.FromResult(3);
            }
        }
    }
}
