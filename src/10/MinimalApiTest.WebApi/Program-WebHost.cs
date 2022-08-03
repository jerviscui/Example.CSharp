//var builder = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//    })
//    .ConfigureWebHostDefaults(webBuilder =>
//        {
//            webBuilder.ConfigureServices((context, services) =>
//            {
//                services.AddControllers().AddControllersAsServices();

//                services.AddSwaggerGen(options =>
//                {
//                    options.IncludeXmlComments(
//                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MinimalApiTest.WebApi.xml"),
//                        true);
//                });
//            });

//            webBuilder.Configure((_, app) =>
//            {
//                // Configure the HTTP request pipeline.
//                app.UseSwagger();
//                app.UseSwaggerUI();

//                app.UseRouting();

//                app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
//            });

//            webBuilder.UseUrls("http://*:10000", "https://*:10001");

//            //代替 Startup
//            //webBuilder.UseStartup<Startup>();
//        }
//    );

//var app = builder.Build();

//app.Run();
