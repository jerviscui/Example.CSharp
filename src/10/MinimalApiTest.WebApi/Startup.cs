namespace MinimalApiTest.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("Api",
                //    new OpenApiInfo { Title = "Ft.Parking.LocalService.Api", Version = "v1" });

                options.IncludeXmlComments(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MinimalApiTest.WebApi.xml"),
                    true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
        }
    }
}
