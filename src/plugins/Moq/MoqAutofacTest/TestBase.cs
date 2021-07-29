using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MoqTest.Domain;
using MoqTest.Domain.Goods;

namespace MoqAutofacTest
{
    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; }

        private IContainer Container { get; set; } = null!;

        protected TestBase()
        {
            ServiceProvider = Init();
        }

        private IServiceProvider Init()
        {
            var builder = new ContainerBuilder();

            //register default
            builder.RegisterType<DefaultIdGenerator>().As<IIdGenerator>().SingleInstance();

            var services = new ServiceCollection();
            PreInit(services);

            builder.Populate(services);
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        protected virtual void PreInit(IServiceCollection services)
        {

        }

        public ILifetimeScope CreateScope(Action<ContainerBuilder>? configurationAction = null)
        {
            return configurationAction is null
                ? Container.BeginLifetimeScope()
                : Container.BeginLifetimeScope(configurationAction);
        }
    }
}