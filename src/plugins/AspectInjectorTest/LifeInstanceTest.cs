using AspectInjector.Broker;
using System;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance)]
    public class Log2Aspect
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

    [Injection(typeof(Log2Aspect))]
    public class Log2Attribute : Attribute
    {
    }

    [Aspect(Scope.PerInstance)]
    [Injection(typeof(Together2Attribute))]
    public class Together2Attribute : Attribute
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

    [Log2]
    public class LifeInstanceClassTest
    {
        [Log2]
        public void PublicMethod()
        {
        }

        private void PrivateMethod()
        {
        }

        public string Prop { get; set; } = string.Empty;
    }

    [Together2(Num = 1)]
    public class LifeInstanceTogetherTest
    {
        [Together2(Num = 2)]
        public void PublicMethod()
        {
        }

        private void PrivateMethod()
        {
        }
    }
}