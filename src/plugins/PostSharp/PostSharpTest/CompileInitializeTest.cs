using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;

namespace PostSharpTest
{
    [Serializable]
    public class CompileInitializeAttribute : OnMethodBoundaryAspect
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

        /// <summary>
        ///   Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
        ///   before any other build-time method.
        /// </summary>
        /// <param name="method">Method to which the current aspect is applied</param>
        /// <param name="aspectInfo">Reserved for future usage.</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            DateTime = DateTime.Now;
        }
    }

    [CompileInitialize(AttributeTargetElements = MulticastTargets.Method)]
    public class CompileInitializeTest
    {
        public CompileInitializeTest Test()
        {
            Console.WriteLine("DateTime is Compile time.");

            return this;
        }
    }
}
