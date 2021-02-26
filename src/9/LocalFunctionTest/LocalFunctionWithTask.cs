using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocalFunctionTest
{
    public class LocalFunctionWithTask
    {
        public static async Task Test()
        {
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Test");
            //var t = NoLocalFunc(6);
            var t = LocalFunc(6);//LocalFunc() 在这里抛出异常
            Console.WriteLine("Got the task");

            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Test");
            var result = await t;//NoLocalFunc() 在这里抛出异常
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} The returned value is {result:N0}");
        }

        static async Task<int> NoLocalFunc(int delayInSeconds)
        {
            if (delayInSeconds < 0 || delayInSeconds > 5)
                throw new ArgumentOutOfRangeException(nameof(delayInSeconds), "Delay cannot exceed 5 seconds.");

            await Task.Delay(delayInSeconds * 1000);
            return delayInSeconds * new Random().Next(2, 10);
        }

        static Task<int> LocalFunc(int delayInSeconds)
        {
            if (delayInSeconds < 0 || delayInSeconds > 5)
                throw new ArgumentOutOfRangeException(nameof(delayInSeconds), "Delay cannot exceed 5 seconds.");

            return GetValueAsync();

            async Task<int> GetValueAsync()
            {
                await Task.Delay(delayInSeconds * 1000);
                return delayInSeconds * new Random().Next(2, 10);
            }
        }
    }
}
