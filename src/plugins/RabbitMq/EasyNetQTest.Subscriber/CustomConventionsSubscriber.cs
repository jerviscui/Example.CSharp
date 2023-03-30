using EasyNetQ;

namespace EasyNetQTest.Subscriber;

internal class CustomConventionsSubscriber
{
    public static SubscriptionResult SubscribeTest()
    {
        return BusFactory.GetConventionsBus().PubSub.Subscribe<CustomNameMessage>(HandleCustomNameMessage);
    }

    private static void HandleCustomNameMessage(CustomNameMessage textMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("subId-1 Got message: {0}", textMessage.Text);
        Console.ResetColor();
    }
}
