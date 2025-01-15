using Autofac;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNetQTest.Subscriber;

internal sealed class AutoConsumerSubscriber
{
    private readonly IBus _bus;

    private readonly IServiceProvider _serviceProvider;

    public AutoConsumerSubscriber()
    {
        (_serviceProvider, _bus) = BusFactory.GetAutofacBus(builder =>
        {
            builder.RegisterType(typeof(AutoConsumer)).AsSelf().InstancePerLifetimeScope();
        });
    }

    public async Task AutoSubscriberTest()
    {
        var autoSubscriber = new AutoSubscriber(_bus, "not use");
        autoSubscriber.AutoSubscriberMessageDispatcher =
            new DefaultAutoSubscriberMessageDispatcher(_serviceProvider.GetRequiredService<IServiceResolver>());
        await autoSubscriber.SubscribeAsync(new[] { typeof(AutoConsumer).Assembly });

        //等价于手动订阅
        //await _bus.PubSub.SubscribeAsync<CustomNameMessage>(async message =>
        //{
        //    using var scope = _serviceProvider.CreateScope();
        //    await scope.ServiceProvider.GetRequiredService<AutoConsumer>().ConsumeAsync(message);
        //});

        //重写 GenerateSubscriptionId
        //var auto = new AutoSubscriber(null, "")
        //{
        //    GenerateSubscriptionId = info => string.Empty
        //};
    }
}
