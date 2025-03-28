using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace ConfigureAwaitTest
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var xmlTest = new XmlTest();

            await xmlTest.Test1();
            //编译后
            //await xmlTest.Test1().ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class XmlTest
    {
        public Task Test1()
        {
            return Task.Delay(10);
            //编译后
            //return Task.Delay(10);
        }

        public async Task Test2()
        {
            await Task.Delay(10);
            //编译后
            //await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
