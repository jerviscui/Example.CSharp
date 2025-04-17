using CapTest.Order.Service;
using CapTest.Shared;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CapTest.Order.Host;

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
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapTest.Order.Host v1"));
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseRouting();

        _ = app.UseAuthorization();

        _ = app.UseEndpoints(
            endpoints =>
            {
                _ = endpoints.MapControllers();
            });
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddTransient<OrderService>();
        _ = services.AddTransient<OrderCreatedEventHandler>();

        _ = services.AddControllers();
        _ = services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CapTest.Order.Host", Version = "v1" });
            });

        _ = services.AddDbContextPool<OrderDbContext>(
            builder => builder.UseNpgsql(Configuration.GetConnectionString(OrderConsts.DbContextConnName)));

        _ = services.AddTransient<ICapTransaction, PostgreSqlCapTransaction>();
        _ = services.AddCap(
            options =>
            {
                options.TopicNamePrefix = "test";

                _ = options.UseEntityFramework<OrderDbContext>(efOptions => efOptions.Schema = "cap");

                _ = options.UseRabbitMQ(
                    mqOptions =>
                    {
                        mqOptions.HostName = "localhost";
                        //mqOptions.Port = ;
                        //mqOptions.UserName = "";
                        //mqOptions.Password = "";
                    });
                options.DefaultGroupName = OrderConsts.MessageGroupName;

                _ = options.UseDashboard(dashboardOptions => dashboardOptions.PathMatch = "/cap");
                //options.UseDiscovery(discoveryOptions =>
                //{
                //    discoveryOptions.DiscoveryServerHostName = "localhost";
                //    discoveryOptions.DiscoveryServerPort = 8500;
                //    discoveryOptions.CurrentNodeHostName = Configuration.GetValue<string>("ASPNETCORE_HOSTNAME");
                //    discoveryOptions.CurrentNodePort = Configuration.GetValue<int>("ASPNETCORE_PORT");
                //    discoveryOptions.NodeId = "node1";
                //    discoveryOptions.NodeName = "node1 name";
                //});
            });
    }

    #endregion

}
