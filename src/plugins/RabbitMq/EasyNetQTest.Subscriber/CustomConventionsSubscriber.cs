using EasyNetQ;

namespace EasyNetQTest.Subscriber;

internal static class CustomConventionsSubscriber
{
    public static SubscriptionResult SubscribeTest()
    {
        return BusFactory.GetConventionsBus().PubSub.Subscribe<CustomNameMessage>(HandleCustomNameMessage);
    }

    private static void HandleCustomNameMessage(CustomNameMessage message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Got CustomNameMessage: {0}", message.Text);
        Console.ResetColor();
    }
}
