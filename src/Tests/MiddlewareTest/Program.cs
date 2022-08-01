using System.Diagnostics;
using Microsoft.AspNetCore.MiddlewareAnalysis;
using MiddlewareTest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Insert(0, ServiceDescriptor.Transient<IStartupFilter, AnalysisStartupFilter>());

builder.WebHost.ConfigureLogging(logging =>
{
    logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
});

// Add services to the container.
builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddRazorPages().AddControllersAsServices();

var app = builder.Build();

var adapter = ActivatorUtilities.CreateInstance<AnalysisDiagnosticAdapter>(app.Services);
var listener = app.Services.GetRequiredService<DiagnosticListener>();
_ = listener.SubscribeWithAdapter(adapter);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
