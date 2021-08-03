using System;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    public class LogAspect
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

    [Injection(typeof(LogAspect))]
    public class LogAttribute : Attribute
    {
    }

    [Aspect(Scope.Global)]
    [Injection(typeof(TogetherAttribute))]
    public class TogetherAttribute : Attribute
    {
        public int Num { get; set; }

        [Advice(Kind.Before)]
        public void Before([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name)
        {
            Console.WriteLine("before");
        }

        [Advice(Kind.After)]
        public void After()
        {
            Console.WriteLine("after");
        }
    }

    [Log]
    public class LifeGlobalTest
    {
        //only one LogAspect
        [Log]
        public void PublicMethod()
        {
        }

        private void PrivateMethod()
        {
        }

        public string Prop { get; set; } = string.Empty;
    }

    [Together(Num = 1)]
    public class LifeGlobalTogetherTest
    {
        [Together(Num = 10)]
        public void PublicMethod()
        {
        }

        private void PrivateMethod()
        {
        }
    }
}
