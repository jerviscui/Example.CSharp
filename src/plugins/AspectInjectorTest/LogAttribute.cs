using System;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [Injection(typeof(LogAttribute))]
    public class LogAttribute : Attribute
    {
        [Advice(Kind.Before, Targets = Target.Instance | Target.Method | Target.Public)]
        public void Before()
        {

        }

        [Advice(Kind.After, Targets = Target.Instance | Target.Method | Target.Public)]
        public void After()
        {

        }
    }
}