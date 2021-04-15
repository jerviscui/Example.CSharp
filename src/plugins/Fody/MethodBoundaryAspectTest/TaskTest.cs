using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MethodBoundaryAspectTest
{
    [TaskLog]
    public class TaskTest
    {
        public Task Test()
        {
            Console.WriteLine($"front {Thread.CurrentThread.ManagedThreadId}");

            return Task.Delay(1000);
            //Console.WriteLine("end");
        }
        
        //todo cuizj: error!!!
        public async Task AwaitTest()
        {
            Console.WriteLine($"front {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(10);
            Console.WriteLine($"end {Thread.CurrentThread.ManagedThreadId}");
            //throw new ArgumentException();
        }
    }

    public class TaskTest2
    {
        public Task Test()
        {
            Console.WriteLine($"front {Thread.CurrentThread.ManagedThreadId}");

            return Task.Delay(1000);
            //Console.WriteLine("end");
        }
        
        public async Task AwaitTest()
        {
            Console.WriteLine($"front {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(10);
            Console.WriteLine($"end {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
