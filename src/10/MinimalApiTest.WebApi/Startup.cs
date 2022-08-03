namespace MinimalApiTest.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();

            services.AddSwaggerGen(options =>
            {
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
