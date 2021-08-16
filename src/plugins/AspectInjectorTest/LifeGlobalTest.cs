using System;
using System.Diagnostics.CodeAnalysis;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
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
    [AttributeUsage(AttributeTargets.All)]
    public class LogAttribute : Attribute
    {
    }

    [Aspect(Scope.Global)]
    [Injection(typeof(TogetherAttribute))]
    [AttributeUsage(AttributeTargets.All)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
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
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
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
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
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
