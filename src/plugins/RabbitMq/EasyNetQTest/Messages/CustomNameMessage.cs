using EasyNetQ;

namespace EasyNetQTest;

[Queue("MQCustomNameMessage", ExchangeName = "ExEasyNetQTest")]
public class CustomNameMessage
{
    public string Text { get; set; }
}
