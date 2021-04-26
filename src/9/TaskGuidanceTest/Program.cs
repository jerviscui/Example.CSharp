using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = TaskTest.SimpleTask();

            var awaiter = t.GetAwaiter();
            awaiter.GetResult();//阻塞主线程
        }

        public class TaskTest
        {
            public static async void Void()
            {
                Console.WriteLine("");
            }

            public static async Task SimpleTask()
            {
                Console.WriteLine("SimpleTask");
            }

            public static async Task SimpleTask2()
            {
                AsyncTaskMethodBuilder<int>

                Console.WriteLine("SimpleTask2");
                await Task.Delay(1000 * 30);
                Console.WriteLine("Completed");
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