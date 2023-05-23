using System.Text.Json;
using LoggerMessageTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//new DefineTest().LogTest();
//new DefineTest().LogExtensionTest();

var services = new ServiceCollection();
services.AddLogging(builder =>
{
    builder.AddJsonConsole(options => options.JsonWriterOptions = new JsonWriterOptions { Indented = true });
    //builder.AddConsole();
});

services.AddTransient<LoggerMessageAttributeTest>();

var factory = new DefaultServiceProviderFactory();
var serviceProvider = factory.CreateServiceProvider(services);

var attributeTest = serviceProvider.GetRequiredService<LoggerMessageAttributeTest>();
//attributeTest.LogErrorTest();
//attributeTest.LogError_WithoutThis_Test();
attributeTest.LogInfo_WithParameter_Test();
attributeTest.DynamicLevel_Info_Test();
attributeTest.DynamicLevel_Error_Test();

Thread.Sleep(1000);
