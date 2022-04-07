using System.Runtime.InteropServices;
using System.Text;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;
using LogLevel = Com.Ctrip.Framework.Apollo.Logging.LogLevel;

namespace ApolloTestApp2;

public class Program
{
    public static void Main(string[] args)
    {
        LogManager.UseConsoleLogging(LogLevel.Info);
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => logging.AddConsole())
            .ConfigureAppConfiguration((context, builder) =>
            {
                var idc = Environment.GetEnvironmentVariable("IDC");
                if (idc == string.Empty)
                {
                    idc = null;
                }

                if (idc is null)
                {
                    //read file
                    string? path = null;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        path = @"C:\opt\settings\server.properties";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        path = "/opt/settings/server.properties";
                    }
                    if (path is not null)
                    {
                        try
                        {
                            var strings = File.ReadAllLines(path, Encoding.UTF8);
                            var dictionary = strings.Select(o => o.Split('='))
                                .ToDictionary(o => o[0], o => o[1]);

                            dictionary.TryGetValue("idc", out idc);
                        }
                        catch (DirectoryNotFoundException)
                        {
                        }
                        catch (FileNotFoundException)
                        {
                        }
                    }
                }

                var contextConfiguration = context.Configuration;

                var options = builder.Build().GetSection("apollo").Get<ApolloOptions>();
                options.DataCenter = idc;

                var apollo = builder.AddApollo(options);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
