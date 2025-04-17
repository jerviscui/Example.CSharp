using CapTest.Depot.Service;
using CapTest.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapTest.Depot.Host v1"));
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
        _ = services.AddTransient<OrderCreatedEventHandler>();

        _ = services.AddControllers();
        _ = services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CapTest.Depot.Host", Version = "v1" });
            });

        _ = services.AddDbContextPool<DepotDbContext>(
            builder => builder.UseNpgsql(Configuration.GetConnectionString(DepotConsts.DbContextConnName)));

        _ = services.AddCap(
            options =>
            {
                options.TopicNamePrefix = "test";

                _ = options.UseEntityFramework<DepotDbContext>(efOptions => efOptions.Schema = "cap");

                _ = options.UseRabbitMQ(
                    mqOptions =>
                    {
                        mqOptions.HostName = "localhost";
                        //mqOptions.Port = ;
                        //mqOptions.UserName = "";
                        //mqOptions.Password = "";
                    });

                _ = options.UseDashboard(dashboardOptions => dashboardOptions.PathMatch = "/cap");
            });
    }

    #endregion

}
