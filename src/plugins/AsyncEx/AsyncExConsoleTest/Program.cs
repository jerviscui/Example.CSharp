using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsyncExConsoleTest
{
    class Program
    {
        private delegate void Ac(object o);

        private event Ac ac = o =>
        {
            int a = (int)o;
        };

        public void Test()
        {
            Action<object> action = o =>
            {
                int a = (int)o;
            };

            action += o =>
            {
                int b = (int)o;
            };

            ac += o =>
            {
                int b = (int)o;
            };
        }

        private static async Task Main(string[] args)
        {
            await AsyncLock_Test();

            Console.WriteLine("Hello World!");
        }

        private static readonly AsyncLock _mutex = new();

        public static async Task AsyncLock_Test()
        {
            using (await _mutex.LockAsync())
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine("Foo1 start: " + 1);
                //await DoSomethingAsync(1);
                Console.WriteLine("Foo1 end: " + 2);
            }
        }
    }
}
