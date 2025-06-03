using CapTest.Depot.Service;
using CapTest.Shared;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StackExchange.Redis;

namespace CapTest.Depot.Host;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    #region Properties

    public IConfiguration Configuration { get; }

    #endregion

    #region Methods

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        _ = app.UseHttpsRedirection();

        _ = app.UseRouting();

        _ = app.UseAuthorization();

        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseEndpoints(
                (builder) =>
                {
                    _ = builder.MapOpenApi();
                    _ = builder.MapScalarApiReference();
                });
        }

        _ = app.UseEndpoints(
            endpoints =>
            {
                _ = endpoints.MapControllers();
            });
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddTransient<OrderCreatedEventHandler>();

        _ = services.AddControllers();
        _ = services.AddOpenApi();

        _ = services.AddDbContextPool<DepotDbContext>(
            builder => builder.UseNpgsql(Configuration.GetConnectionString(DepotConsts.DbContextConnName)));

        _ = services.AddCap(
            options =>
            {
                options.TopicNamePrefix = "test";
                options.UseStorageLock = true;

                _ = options.UseEntityFramework<DepotDbContext>(efOptions => efOptions.Schema = "cap");

                //_ = options.UseRabbitMQ(
                //    mqOptions =>
                //    {
                //        mqOptions.HostName = "localhost";
                //        //mqOptions.Port = ;
                //        //mqOptions.UserName = "";
                //        //mqOptions.Password = "";
                //    });

                options.UseRedis(
                    (redisOptions) =>
                    {
                        redisOptions.Configuration = ConfigurationOptions.Parse(
                            Configuration.GetConnectionString("Redis")!);
                    });

                _ = options.UseDashboard(dashboardOptions => dashboardOptions.PathMatch = "/cap");
            });
    }

    #endregion

}
