using EasyNetQ;

namespace EasyNetQTest;

[Queue("EasyNetQTest.TextThrowMessage, EasyNetQTest", ExchangeName = "EasyNetQTest.TextThrowMessage, EasyNetQTest")]
public class TextThrowMessage
{
    public TextThrowMessage(string text)
    {
        Text = text;
    }

    public TextThrowMessage(string text, string prop)
    {
        Text = text;
        Prop = prop;
    }

    public string Text { get; set; }

    public string Prop { get; set; }
}
