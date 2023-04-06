using EasyNetQ;

namespace EasyNetQTest;

[Queue("MQAutoConsumerMessage", ExchangeName = "ExEasyNetQTest")]
public class AutoConsumerMessage
{
    public string Text { get; set; }
}
