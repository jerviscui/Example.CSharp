using EasyNetQ;

namespace EasyNetQTest;

public class CustomConventions : Conventions
{
    /// <inheritdoc />
    public CustomConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
    {
        ExchangeNamingConvention = type => "ExEasyNetQTest";

        //Topic RoutingKey
        TopicNamingConvention = type => type.Name;
    }
}
