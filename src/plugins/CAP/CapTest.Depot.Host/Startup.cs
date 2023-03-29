using CapTest.Depot.Service;
using CapTest.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CapTest.Depot.Host
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
            services.AddTransient<OrderCreatedEventHandler>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CapTest.Depot.Host", Version = "v1" });
            });

            services.AddDbContextPool<DepotDbContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString(DepotConsts.DbContextConnName)));

            services.AddCap(options =>
            {
                options.TopicNamePrefix = "test";

                options.UseEntityFramework<DepotDbContext>(efOptions => efOptions.Schema = "cap");

                options.UseRabbitMQ(mqOptions =>
                {
                    mqOptions.HostName = "localhost";
                    //mqOptions.Port = ;
                    //mqOptions.UserName = "";
                    //mqOptions.Password = "";
                });

                options.UseDashboard(dashboardOptions => dashboardOptions.PathMatch = "/cap");
                //options.UseDiscovery(discoveryOptions => { });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapTest.Depot.Host v1"));
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
