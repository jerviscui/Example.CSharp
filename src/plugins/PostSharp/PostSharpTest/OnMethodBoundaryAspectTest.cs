using System;
using System.Diagnostics;
using System.Threading;
using PostSharp.Aspects;

namespace PostSharpTest
{
    [Serializable]
    public class OnMethodBoundaryAspectDemoAttribute : OnMethodBoundaryAspect
    {
        public bool Enabled
        {
            get;
            set;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (this.Enabled)
            {
                args.MethodExecutionTag = Stopwatch.StartNew();
            }
        }
        public override void OnExit(MethodExecutionArgs args)
        {
            if (Enabled)
            {
                if (args.MethodExecutionTag is Stopwatch sw)
                {
                    sw.Stop();
                    Console.WriteLine($"方法{args.Method.Name}执行时间为:{sw.ElapsedMilliseconds}");
                }
            }
            else
            {
                Console.WriteLine("disabled.");
            }
        }
    }

    [OnMethodBoundaryAspectDemo(Enabled = true)]
    public class OnMethodBoundaryAspectTest
    {
        //[OnMethodBoundaryAspectDemo(Enabled = true)]

        public void Test1()
        {
            Thread.Sleep(500);
            Console.WriteLine("Test1");
        }

        [OnMethodBoundaryAspectDemo(Enabled = false)]
        public void Test2()
        {
            Thread.Sleep(500);
            Console.WriteLine("Test2");
        }
    }
}
