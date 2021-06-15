using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
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
    public class InheritParentAttribute : Attribute
    {

    }
    
    public class InheritAttribute : InheritParentAttribute
    {

    }

    [Inherit]
    public class InheritedTest
    {
        public void Method()
        {

        }
    }
}
