using Autofac;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;

namespace EasyNetQTest.Subscriber;

internal class AutoConsumerSubscriber
{
    private readonly IBus _bus;

    private IServiceProvider _serviceProvider;

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
        await autoSubscriber.SubscribeAsync(new[] { typeof(AutoConsumer).Assembly });

        //var auto = new AutoSubscriber(null, "")
        //{
        //    GenerateSubscriptionId = info => string.Empty
        //};
    }
}
