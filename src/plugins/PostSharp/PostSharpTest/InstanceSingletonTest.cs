using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace PostSharpTest
{
    [Serializable]
    public class InstanceSingletonAttribute : OnMethodBoundaryAspect
    {
        public DateTime DateTime { get; set; }

        /// <summary>
        ///   Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine($"exec time: {DateTime:O}");
        }

        /// <summary>Initializes the current aspect.</summary>
        /// <param name="method">Method to which the current aspect is applied.</param>
        public override void RuntimeInitialize(MethodBase method)
        {
            DateTime = DateTime.Now;
        }
    }

    [InstanceSingleton(AttributeTargetElements = MulticastTargets.Method)]
    public class InstanceSingletonTest
    {
        public InstanceSingletonTest Test()
        {
            Console.WriteLine("InstanceSingletonTest has same time");

            return this;
        }
    }
}
