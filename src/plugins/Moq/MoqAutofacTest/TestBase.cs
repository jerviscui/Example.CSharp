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
        public IServiceProvider ServiceProvider { get; }

        protected TestBase()
        {
            ServiceProvider = Init();
        }

        private IServiceProvider Init()
        {
            var builder = new ContainerBuilder();

            //register default
            builder.RegisterType<DefaultIdGenerator>().As<IIdGenerator>().SingleInstance().PreserveExistingDefaults();

            var services = new ServiceCollection();
            PreInit(services);

            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        protected virtual void PreInit(IServiceCollection services)
        {
        }
    }
}