using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace LocalFunctionTest
{
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    public class LocalFunctionWithTask
    {
        public static async Task Test()
        {
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId.ToString()} Test");
            var t = NoLocalFunc(6);
            //var t = LocalFunc(6); //LocalFunc() 在这里抛出异常
            Console.WriteLine("Got the task");

            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId.ToString()} Test");
            var result = await t; //NoLocalFunc() 在这里抛出异常
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId.ToString()} The returned value is {result:N0}");
        }

        private static async Task<int> NoLocalFunc(int delayInSeconds)
        {
            if (delayInSeconds < 0 || delayInSeconds > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(delayInSeconds), "Delay cannot exceed 5 seconds.");
            }

            await Task.Delay(delayInSeconds * 1000);
            return delayInSeconds * new Random().Next(2, 10);
        }

        private static Task<int> LocalFunc(int delayInSeconds)
        {
            if (delayInSeconds < 0 || delayInSeconds > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(delayInSeconds), "Delay cannot exceed 5 seconds.");
            }

            return GetValueAsync();

            async Task<int> GetValueAsync()
            {
                await Task.Delay(delayInSeconds * 1000);
                return delayInSeconds * new Random().Next(2, 10);
            }
        }
    }
}
