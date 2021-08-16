using System;
using System.Diagnostics.CodeAnalysis;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class InheritAspect
    {
        [Advice(Kind.Before)]
        public void Before()
        {
            Console.WriteLine("before");
        }

        [Advice(Kind.After)]
        public void After()
        {
            Console.WriteLine("after");
        }
    }

    [Injection(typeof(PropagationAspect), Inherited = true)]
    [AttributeUsage(AttributeTargets.All)]
    public class InheritParentAttribute : Attribute
    {
    }

    public class InheritAttribute : InheritParentAttribute
    {
    }

    [Inherit]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class InheritedTest
    {
        public void Method()
        {
        }
    }
}
