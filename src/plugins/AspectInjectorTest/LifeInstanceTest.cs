using System;
using System.Diagnostics.CodeAnalysis;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
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
    [AttributeUsage(AttributeTargets.All)]
    public class Log2Attribute : Attribute
    {
    }

    [Aspect(Scope.PerInstance)]
    [Injection(typeof(Together2Attribute))]
    [AttributeUsage(AttributeTargets.All)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
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
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
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
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
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
