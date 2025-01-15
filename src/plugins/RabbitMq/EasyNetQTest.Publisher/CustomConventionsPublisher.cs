using EasyNetQ;

namespace EasyNetQTest.Publisher;

internal sealed class CustomConventionsPublisher
{
    public static void PublishTest()
    {
        BusFactory.GetConventionsBus().PubSub
            .Publish(new CustomNameMessage { Text = DateTime.Now.ToShortTimeString() });
    }
}
