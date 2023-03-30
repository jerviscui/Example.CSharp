using EasyNetQ;

namespace EasyNetQTest;

public class CustomConventions : Conventions
{
    /// <inheritdoc />
    public CustomConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
    {
        ExchangeNamingConvention = type => "ExEasyNetQTest";

        TopicNamingConvention = type => type.Name;
    }
}
