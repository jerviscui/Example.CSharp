using EasyNetQ;

namespace EasyNetQTest.Subscriber;

public class SimpleSubscriber
{
    private readonly IBus _bus;

    public SimpleSubscriber()
    {
        _bus = BusFactory.GetBus();
    }

    public SubscriptionResult SubscribeTest()
    {
        return _bus.PubSub.Subscribe<TextMessage>("subId-1", HandleTextMessage);
    }

    public async Task<SubscriptionResult> SubscribeAsyncTest()
    {
        return await _bus.PubSub.SubscribeAsync<TextMessage>("subId-2", HandleTextMessageAsync);
    }

    public SubscriptionResult CustomNameTest()
    {
        return _bus.PubSub.Subscribe<CustomNameMessage>("subId-1", HandleCustomNameMessage);
    }

    public SubscriptionResult SubscribeWithTopicTest()
    {
        return _bus.PubSub.Subscribe<CustomNameMessage>("subId-2", HandleCustomNameMessage,
            configuration => configuration.WithTopic("X.*").WithTopic("*.B"));

        //Bound queue MQCustomNameMessage_subId-2 to exchange ExEasyNetQTest with routingKey = X.* and arguments =
        //Bound queue MQCustomNameMessage_subId-2 to exchange ExEasyNetQTest with routingKey = *.B and arguments =
    }

    private static void HandleTextMessage(TextMessage textMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("subId-1 Got message: {0}", textMessage.Text);
        Console.ResetColor();
    }

    private static Task HandleTextMessageAsync(TextMessage textMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("subId-2 Got message: {0}", textMessage.Text);
        Console.ResetColor();

        return Task.CompletedTask;
    }

    private static void HandleCustomNameMessage(CustomNameMessage message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Got CustomNameMessage: {0}", message.Text);
        Console.ResetColor();
    }
}
