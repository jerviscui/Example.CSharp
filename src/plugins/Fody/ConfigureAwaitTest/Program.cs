using System;
using System.Threading.Tasks;

namespace ConfigureAwaitTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var xmlTest = new XmlTest();
            
            await xmlTest.Test1();
            //编译后
            //await xmlTest.Test1().ConfigureAwait(continueOnCapturedContext: false);
        }
    }

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
