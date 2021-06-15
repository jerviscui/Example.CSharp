using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
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
    public class PropagationAttribute : Attribute
    {

    }
    
    [Propagation]
    public class PropagationTest
    {
        public string S2 { get; set; } = string.Empty;

        public void Method2()
        {

        }

        class InnerClass
        {
            public string S { get; set; } = string.Empty;

            public void Method()
            {

            }
        }
    }
}
