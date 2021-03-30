using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace PostSharpTest
{
    [Serializable]
    public class InstanceScopedAttribute: OnMethodBoundaryAspect, IInstanceScopedAspect
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
        ///   Creates a new instance of the aspect based on the current instance, serving as a prototype.
        /// </summary>
        /// <param name="adviceArgs">Aspect arguments.</param>
        /// <returns>A new instance of the aspect, typically a clone of the current prototype instance.</returns>
        public object CreateInstance(AdviceArgs adviceArgs)
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        ///   Initializes the aspect instance. This method is invoked when all system elements of the aspect (like member imports)
        ///   have completed.
        /// </summary>
        public void RuntimeInitializeInstance()
        {
            DateTime = DateTime.Now;
        }
    }

    [InstanceScoped(AttributeTargetElements = MulticastTargets.Method)]
    public class InstanceScopedTest
    {
        public InstanceScopedTest Test()
        {
            Console.WriteLine("InstanceScopedTest has different time");

            return this;
        }
    }
}
