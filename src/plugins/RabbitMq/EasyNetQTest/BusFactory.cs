using EasyNetQ;

namespace EasyNetQTest;

public static class BusFactory
{
    private static IBus? _bus;

    private static IBus? _conventionsBus;

    private static readonly string Conn = "host=10.98.59.35;port=30677;username=;password=";

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
                //register.EnableMicrosoftLogging();

                register.Register(typeof(IConventions), typeof(CustomConventions));
            });
    }
}
