using MinimalApiTest.WebApi;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddMvcCore().AddApiExplorer();

        //todo: 这里调用不能被 Swagger 识别
        //services.AddControllers();

        //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        ////services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen(options =>
        //{
        //    //options.SwaggerDoc("Api",
        //    //    new OpenApiInfo { Title = "Ft.Parking.LocalService.Api", Version = "v1" });

        //    options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MinimalApiTest.WebApi.xml"),
        //        true);
        //});
        int a = 1;
    })
    .ConfigureWebHostDefaults(webBuilder =>
        {
            //webBuilder.Configure((_, app) =>
            //{
            //    // Configure the HTTP request pipeline.
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //    app.UseRouting();

            //    app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
            //});

            webBuilder.UseStartup<Startup>();
        }
    );

var app = builder.Build();
app.Run();
