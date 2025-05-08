using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LoggerMessageTest;

internal sealed class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        //new DefineTest().LogTest();
        //new DefineTest().LogExtensionTest();

        var services = new ServiceCollection();
        _ = services.AddLogging(
            builder =>
            {
                _ = builder.AddJsonConsole(
                    options => options.JsonWriterOptions = new JsonWriterOptions { Indented = true });
                //builder.AddConsole();
            });

        _ = services.AddTransient<LoggerMessageAttributeTest>();

        var factory = new DefaultServiceProviderFactory();
        var serviceProvider = factory.CreateServiceProvider(services);

        var attributeTest = serviceProvider.GetRequiredService<LoggerMessageAttributeTest>();
        attributeTest.LogErrorTest();
        //attributeTest.LogError_WithoutThis_Test();
        //attributeTest.LogInfo_WithParameter_Test();
        //attributeTest.DynamicLevel_Info_Test();
        //attributeTest.DynamicLevel_Error_Test();

        Thread.Sleep(1000);
    }

    #endregion

    private Program()
    {
    }
}
