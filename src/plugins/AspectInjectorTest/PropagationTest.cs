using System;
using System.Diagnostics.CodeAnalysis;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class PropagationAspect
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

    [Injection(typeof(PropagationAspect), Propagation = PropagateTo.Methods | PropagateTo.Types,
        PropagationFilter = "Method2")]
    [AttributeUsage(AttributeTargets.All)]
    public class PropagationAttribute : Attribute
    {
    }

    [Propagation]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class PropagationTest
    {
        public string S2 { get; set; } = string.Empty;

        public void Method2()
        {
        }

        private class InnerClass
        {
            public string S { get; set; } = string.Empty;

            public void Method()
            {
            }
        }
    }
}
