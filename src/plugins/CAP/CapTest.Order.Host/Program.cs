namespace CapTest.Order.Host;

public class Program
{

    #region Constants & Statics

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(args)
            .ConfigureLogging((logging) => logging.AddConsole())
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    #endregion

}
