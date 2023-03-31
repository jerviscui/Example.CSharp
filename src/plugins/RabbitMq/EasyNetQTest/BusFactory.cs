using EasyNetQ;

namespace EasyNetQTest;

public static class BusFactory
{
    private static IBus? _bus;

    private static IBus? _conventionsBus;

    private static readonly string Conn = "host=10.98.59.35;port=30677;username=;password=";

    private static readonly string Confirms = "publisherConfirms=true";

    private static readonly string Prefetch = "prefetchCount=1";

    public static IBus GetBus()
    {
        return _bus ??= RabbitHutch.CreateBus(Conn,
            register =>
            {
                register.EnableSystemTextJson();

                register.EnableConsoleLogger();
                //register.EnableMicrosoftLogging();
            });
    }

    public static IBus GetConventionsBus()
    {
        return _conventionsBus ??= RabbitHutch.CreateBus(Conn,
            register =>
            {
                register.EnableSystemTextJson();

                register.EnableConsoleLogger();

                register.Register(typeof(IConventions), typeof(CustomConventions));
            });
    }

    public static IBus GetPubConfirmBus()
    {
        return _pubConfirmBus ??= RabbitHutch.CreateBus($"{Conn};{Confirms}",
            register =>
            {
                register.EnableSystemTextJson();

                register.EnableConsoleLogger();

                register.Register(typeof(IConventions), typeof(CustomConventions));
            });
    }

    public static IBus GetErrorRequeueBus()
    {
        return _requeueBus ??= RabbitHutch.CreateBus($"{Conn};{Prefetch}",
            register =>
            {
                register.EnableSystemTextJson();

                register.EnableConsoleLogger();

                register.Register(typeof(IConventions), typeof(CustomConventions));

                register.EnableAlwaysNackWithRequeueConsumerErrorStrategy();
            });
    }
}
