using Autofac;
using EasyNetQ;
using EasyNetQ.Logging;

namespace EasyNetQTest;

public static class BusFactory
{
    private static IBus? _bus;

    private static IBus? _conventionsBus;

    private static IBus? _pubConfirmBus;

    private static IBus? _requeueBus;

    private static readonly string Conn = "host=10.98.59.35;port=30677;username=;password=";

    public static readonly string Confirms = "publisherConfirms=true";

    public static readonly string Prefetch = "prefetchCount=1";

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

    public static (IServiceProvider serviceProvider, IBus bus) GetAutofacBus(
        Action<ContainerBuilder>? registerAction = null)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterEasyNetQ($"{Conn};{Prefetch}",
            register =>
            {
                register.EnableSystemTextJson();

                //https://github.com/EasyNetQ/EasyNetQ/issues/1502
                //TryRegister not work for generic type
                //register.EnableConsoleLogger();

                register.Register(typeof(IConventions), typeof(CustomConventions));

                register.EnableAlwaysNackWithRequeueConsumerErrorStrategy();
            });
        containerBuilder.RegisterType(typeof(ConsoleLogger)).As(typeof(ILogger)).SingleInstance();
        containerBuilder.RegisterGeneric(typeof(ConsoleLogger<>)).As(typeof(ILogger<>)).SingleInstance();

        registerAction?.Invoke(containerBuilder);

        var container = containerBuilder.Build();

        return ((IServiceProvider)container, container.Resolve<IBus>());
    }
}
