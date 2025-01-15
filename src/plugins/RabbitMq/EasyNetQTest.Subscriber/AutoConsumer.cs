using EasyNetQ.AutoSubscribe;

namespace EasyNetQTest.Subscriber;

internal sealed class AutoConsumer : IConsume<AutoConsumerMessage>, IConsumeAsync<CustomNameMessage>
{
    /// <inheritdoc />
    //[ForTopic("#")] //default
    [AutoSubscriberConsumer(SubscriptionId = "subId-1")]
    public void Consume(AutoConsumerMessage message, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("AutoConsumer Got AutoConsumerMessage: {0}", message.Text);
        Console.ResetColor();

        //Declared exchange ExEasyNetQTest: type=topic, durable=True, autoDelete=False, arguments=
        //Declared queue MQAutoConsumerMessage_subId-1: durable=True, exclusive=False, autoDelete=False, arguments=
        //Bound queue MQAutoConsumerMessage_subId-1 to exchange ExEasyNetQTest with routingKey=# and arguments=
    }

    /// <inheritdoc />
    [ForTopic(nameof(CustomNameMessage))]
    [AutoSubscriberConsumer(SubscriptionId = "")]
    public Task ConsumeAsync(CustomNameMessage message, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("AutoConsumer Got CustomNameMessage: {0}", message.Text);
        Console.ResetColor();

        return Task.CompletedTask;

        //Declared exchange ExEasyNetQTest: type=topic, durable=True, autoDelete=False, arguments=
        //Declared queue MQCustomNameMessage: durable=True, exclusive=False, autoDelete=False, arguments=
        //Bound queue MQCustomNameMessage to exchange ExEasyNetQTest with routingKey=CustomNameMessage and arguments=
    }
}
