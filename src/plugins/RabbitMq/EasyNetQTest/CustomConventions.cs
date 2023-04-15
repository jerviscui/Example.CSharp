using EasyNetQ;
using EasyNetQ.Internals;

namespace EasyNetQTest;

public class CustomConventions : Conventions
{
    /// <inheritdoc />
    public CustomConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
    {
        ExchangeNamingConvention = type =>
        {
            var ex = GetQueueAttribute(type)?.ExchangeName;
            return string.IsNullOrEmpty(ex) ? "ExEasyNetQTest" : ex;
        };

        //Topic RoutingKey
        TopicNamingConvention = type => type.Name;
    }

    private static QueueAttribute? GetQueueAttribute(Type messageType)
    {
        return messageType.GetAttribute<QueueAttribute>();
    }
}
