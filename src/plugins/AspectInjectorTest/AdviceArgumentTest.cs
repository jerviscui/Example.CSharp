using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    public class AdviceArgumentAspect
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void Before([Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type,
            [Argument(Source.Metadata)] MethodBase methodInfo,
            [Argument(Source.Name)] string targetName, [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {

        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void After([Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type,
            [Argument(Source.Metadata)] MethodBase methodInfo,
            [Argument(Source.Name)] string targetName, [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.ReturnValue)] object returnValue, [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {

        }

        [Advice(Kind.Around, Targets = Target.Method)]
        public object? Arround([Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type,
            [Argument(Source.Metadata)] MethodBase methodInfo, [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Name)] string targetName, [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {
            return null;
        }
    }

    [Injection(typeof(AdviceArgumentAspect))]
    public class AdviceArgumentAttribute : Attribute
    {

    }

    [AdviceArgument]
    public class AdviceArgumentTest
    {
        public int Mehtod(int i)
        {
            return i;
        }
    }
}
