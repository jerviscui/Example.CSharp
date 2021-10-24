using CapTest.Order.Service;
using CapTest.Shared;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CapTest.Order.Host
{
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
            services.AddTransient<OrderService>();
            services.AddTransient<OrderCreatedEventHandler>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CapTest.Order.Host", Version = "v1" });
            });

            services.AddDbContextPool<OrderDbContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString(OrderConsts.DbContextConnName)));

            services.AddTransient<ICapTransaction, PostgreSqlCapTransaction>();
            services.AddCap(options =>
            {
                options.TopicNamePrefix = "test";

                options.UseEntityFramework<OrderDbContext>(efOptions => efOptions.Schema = "cap");

                options.UseRabbitMQ("localhost");
                options.DefaultGroupName = OrderConsts.MessageGroupName;

                options.UseDashboard(dashboardOptions => dashboardOptions.PathMatch = "/cap");
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapTest.Order.Host v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
