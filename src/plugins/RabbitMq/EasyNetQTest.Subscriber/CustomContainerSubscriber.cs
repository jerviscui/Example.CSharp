using EasyNetQ;

namespace EasyNetQTest.Subscriber;

internal sealed class CustomContainerSubscriber
{
    private readonly IBus _bus;

    private IServiceProvider _serviceProvider;

    public CustomContainerSubscriber()
    {
        (_serviceProvider, _bus) = BusFactory.GetAutofacBus();
    }

    public Task<SubscriptionResult> SubscribeTest()
    {
        return _bus.PubSub.SubscribeAsync<CustomNameMessage>(HandleCustomNameMessage);
    }

    private static void HandleCustomNameMessage(CustomNameMessage message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Got CustomNameMessage: {0}", message.Text);
        Console.ResetColor();

        Thread.Sleep(1000);
    }
}
