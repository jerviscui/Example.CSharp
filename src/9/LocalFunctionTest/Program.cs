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

            SimpleTask().GetAwaiter().GetResult();
        }

        public static async Task SimpleTask()
        {
            Console.WriteLine("test");
        }

        public static Task SyncTask()
        {
            Console.WriteLine("test");
            return Task.CompletedTask;
        }
    }
}
