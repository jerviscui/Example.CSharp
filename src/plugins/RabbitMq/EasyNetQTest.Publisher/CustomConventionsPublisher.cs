using EasyNetQ;

namespace EasyNetQTest.Publisher;

internal class CustomConventionsPublisher
{
    public static void PublishTest()
    {
        BusFactory.GetConventionsBus().PubSub
            .Publish(new CustomNameMessage { Text = DateTime.Now.ToShortTimeString() });
    }
}
