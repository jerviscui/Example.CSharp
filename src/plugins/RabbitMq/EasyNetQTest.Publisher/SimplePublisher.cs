using EasyNetQ;

namespace EasyNetQTest.Publisher;

internal class SimplePublisher
{
    private readonly IBus _bus = BusFactory.GetBus();

    public void PublishTest()
    {
        _bus.PubSub.Publish(new TextMessage { Text = DateTime.Now.ToShortTimeString() });
    }

    public void PublishThrowExceptionTest()
    {
        _bus.PubSub.Publish(new TextThrowMessage(DateTime.Now.ToShortTimeString()));
    }

    public async Task PublishAsyncTest()
    {
        await _bus.PubSub.PublishAsync(new TextMessage { Text = DateTime.Now.ToShortTimeString() });
    }

    public void CustomNameTest()
    {
        _bus.PubSub.Publish(new CustomNameMessage { Text = DateTime.Now.ToShortTimeString() });
    }

    public void PublishWithTopicTest()
    {
        _bus.PubSub.Publish(new CustomNameMessage { Text = DateTime.Now.ToShortTimeString() }, "X.A");

        //Published to exchange ExEasyNetQTest with routingKey=X.A and correlationId=623d2261-3f7a-4be1-ae3f-a43bb2b95c4b
    }
}
