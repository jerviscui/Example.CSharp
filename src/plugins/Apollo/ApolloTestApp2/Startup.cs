using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace ApolloTestApp2;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApolloTestApp2", Version = "v1" });
        });

        var config = Configuration.GetSection("MyOption");
        var name = config.GetSection("Name");
        var ints = config.GetSection("Ints").Get<int[]>();
        services.Configure<MyOption>(config);

        var value = Configuration.GetSection("Logging:LogLevel:Default").Value;
        var value2 = Configuration.GetSection("OAuth:Authority").Value;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApolloTestApp2 v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        lifetime.ApplicationStarted.Register((o, token) =>
        {
            var options = ((IServiceProvider)o!).GetRequiredService<IOptions<MyOption>>();
            var logger = ((IServiceProvider)o!).GetRequiredService<ILogger<MyOption>>();

            logger.LogWarning($"MyOption values: {JsonSerializer.Serialize(options.Value)}");

            var monitor = ((IServiceProvider)o!).GetRequiredService<IOptionsMonitor<MyOption>>();
            monitor.OnChange((option, s) =>
            {
                logger.LogWarning($"MyOption {s} has changed: {JsonSerializer.Serialize(option)}");
            });
        }, app.ApplicationServices);
    }
}
