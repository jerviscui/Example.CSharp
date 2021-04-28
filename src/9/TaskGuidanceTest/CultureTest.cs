using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    public class CultureTest
    {
        public static void Test()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
            Console.WriteLine(CultureInfo.DefaultThreadCurrentCulture.Name);
            Console.WriteLine(CultureInfo.CurrentCulture.Name);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
            Console.WriteLine(CultureInfo.CurrentCulture.Name);


            var t = TaskCultureTest();
            var awaiter = t.GetAwaiter();
            awaiter.GetResult();

            ThreadCultureTest();
        }

        public static async Task TaskCultureTest()
        {
            await Task.Factory.StartNew(() =>
            {
                //output: en
                Console.WriteLine(CultureInfo.CurrentCulture.Name);
            });
        }

        public static void ThreadCultureTest()
        {
            //现在 Thread 会延续调用线程的 Culture
            new Thread(() =>
            {
                //output: en
                Console.WriteLine(CultureInfo.CurrentCulture.Name);
            }).Start();
        }
    }
}
