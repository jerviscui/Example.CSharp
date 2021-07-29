using AspectInjector.Broker;
using System;
using System.Threading.Tasks;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance, Factory = typeof(AspectFactory))]
    public class MethodFactoryAspect
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnBefore([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name,
            [Argument(Source.Instance)] object instance)
        {

        }

        private IServiceProvider ServiceProvider { get; set; }

        public MethodFactoryAspect(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }

    [Injection(typeof(MethodFactoryAspect))]
    public class MethodFactoryAttribute : Attribute
    {

    }

    public static class AspectFactory
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static object GetInstance(Type type)
        {
            return new MethodFactoryAspect(ServiceProvider ?? throw new ArgumentException());
        }
    }

    [MethodFactory]
    public class FactoryTest
    {
        public Task Test()
        {
            return Task.CompletedTask;
        }
    }
}
