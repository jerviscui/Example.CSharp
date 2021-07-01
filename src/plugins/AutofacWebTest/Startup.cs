using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacWebTest.Controllers;
using AutofacWebTest.Services;

namespace AutofacWebTest
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
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutofacWebTest", Version = "v1" });
            });
        }

        // ConfigureContainer is where you can register things directly with Autofac.
        // This runs after ConfigureServices so the things here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here.
            // Don't call builder.Populate(), that happens in AutofacServiceProviderFactory for you.
            //builder.RegisterModule(new MyApplicationModule());
        }

        public ILifetimeScope AutofacContainer { get; private set; }
        
        // This is called after ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutofacWebTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Ԥ�ȶ�Ĭ������ ServiceCollection ��Ӱ��
            //Ԥ�ȶ� Autofac ������Ӱ��
            //_ = app.ApplicationServices.GetService<WeatherForecastController>();
            //_ = app.ApplicationServices.GetService<IWeatherForecastService>();

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            //Ԥ�� Container ����
            //AutofacContainer.Resolve<IWeatherForecastService>();
            //Ĭ��
            //AutofacContainer.Resolve<WeatherForecastController>();
        }
    }
}
