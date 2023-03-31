using EasyNetQ;

namespace EasyNetQTest.Subscriber;

internal static class SimpleConsumerErrorStrategySubscriber
{
    public static Task<SubscriptionResult> SubscribeTest()
    {
        return BusFactory.GetErrorRequeueBus().PubSub.SubscribeAsync<CustomNameMessage>(HandleCustomNameMessage);
    }

    private static void HandleCustomNameMessage(CustomNameMessage message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Got CustomNameMessage: {0}", message.Text);
        Console.ResetColor();

        Thread.Sleep(1000);

        throw new Exception("requeue test");
    }
}
