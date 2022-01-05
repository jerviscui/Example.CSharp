using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionTest;

public class Test
{
    public static void GetServices_Test()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITester, SimpleTester>();
        services.AddTransient<ITester, FirstTester>();
        services.AddTransient<ITester, SecondTester>();

        var serviceProvider = services.BuildServiceProvider();
        var testers = serviceProvider.GetServices<ITester>();
    }
}
