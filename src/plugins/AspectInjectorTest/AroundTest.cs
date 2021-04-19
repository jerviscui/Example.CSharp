using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(ArroundAttribute))]
    public class ArroundAttribute : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object OnBefore([Argument(Source.Triggers)] Attribute[] attributes, 
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)]string name, 
            [Argument(Source.Instance)]object instance, [Argument(Source.Target)]Func<object[], object> method)
        {
            return new object();
        }
    }

    [Arround]
    public class AroundTest
    {
        public Task Test()
        {
            return Task.CompletedTask;
        }

        public async Task AwaitTest()
        {
            //todo cuizj: Test AsyncMethod Around
            await Task.Delay(10);
        }
    }
}
