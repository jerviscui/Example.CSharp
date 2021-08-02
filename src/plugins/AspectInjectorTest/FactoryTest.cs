using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [Aspect(Scope.PerInstance, Factory = typeof(AspectFactory))]
    public class MethodFactoryAspect
    {
        public MethodFactoryAspect(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

#pragma warning disable IDE0052 // 删除未读的私有成员
        private IServiceProvider ServiceProvider { get; }
#pragma warning restore IDE0052 // 删除未读的私有成员

        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnBefore([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name,
            [Argument(Source.Instance)] object instance)
        {
        }
    }

    [Injection(typeof(MethodFactoryAspect), Propagation = PropagateTo.Methods)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MethodFactoryAttribute : Attribute
    {
    }

    public static class AspectFactory
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static object GetInstance(Type type)
        {
            return new MethodFactoryAspect(ServiceProvider ?? throw new ArgumentException(nameof(ServiceProvider)));
        }
    }

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [MethodFactory]
    public class FactoryTest
    {
        public Task Test()
        {
            return Task.CompletedTask;
        }
    }
}
