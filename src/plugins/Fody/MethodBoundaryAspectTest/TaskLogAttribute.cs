using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MethodBoundaryAspect.Fody.Attributes;

namespace MethodBoundaryAspectTest
{
    public class TaskLogAttribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs arg)
        {
            Console.WriteLine($"OnEntry {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs arg)
        {
            Console.WriteLine($"OnExit {Thread.CurrentThread.ManagedThreadId}");
        }

        ///// <inheritdoc />
        //public override void OnException(MethodExecutionArgs arg)
        //{
        //    Console.WriteLine($"OnException {Thread.CurrentThread.ManagedThreadId}");
        //    arg.FlowBehavior = FlowBehavior.Continue;
        //}
    }
}
