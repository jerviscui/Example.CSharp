using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureMetricsWithDefaults(metricsBuilder =>
{
    //metricsBuilder.Report.ToConsole(TimeSpan.FromSeconds(2));
    metricsBuilder.Report.ToInfluxDb(options =>
    {
        var config = builder.Configuration.GetSection(nameof(MetricsReportingInfluxDbOptions));
        var dbConfig = config.GetSection(nameof(MetricsReportingInfluxDbOptions.InfluxDb));
        var httpConfig = config.GetSection(nameof(MetricsReportingInfluxDbOptions.HttpPolicy));

        options.FlushInterval =
            TimeSpan.FromSeconds(config.GetValue<int>(nameof(MetricsReportingInfluxDbOptions.FlushInterval)));
        options.InfluxDb.BaseUri =
            new Uri(dbConfig.GetValue<string>(nameof(InfluxDbOptions.BaseUri)));
        options.InfluxDb.Database = dbConfig.GetValue<string>(nameof(InfluxDbOptions.Database));
        options.InfluxDb.Password = dbConfig.GetValue<string>(nameof(InfluxDbOptions.Password));
        options.InfluxDb.UserName = dbConfig.GetValue<string>(nameof(InfluxDbOptions.UserName));
        options.InfluxDb.Consistenency = dbConfig.GetValue<string>(nameof(InfluxDbOptions.Consistenency));
        options.InfluxDb.RetentionPolicy = dbConfig.GetValue<string>(nameof(InfluxDbOptions.RetentionPolicy));
        options.InfluxDb.CreateDataBaseIfNotExists =
            dbConfig.GetValue<bool>(nameof(InfluxDbOptions.CreateDataBaseIfNotExists));
        options.HttpPolicy.BackoffPeriod =
            TimeSpan.FromSeconds(httpConfig.GetValue<int>(nameof(HttpPolicy.BackoffPeriod)));
        options.HttpPolicy.FailuresBeforeBackoff = httpConfig.GetValue<int>(nameof(HttpPolicy.FailuresBeforeBackoff));
        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(httpConfig.GetValue<int>(nameof(HttpPolicy.Timeout)));

        options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
    });
});
builder.Host.UseMetrics();

// Add services to the container.
builder.Services.AddAppMetricsCollectors();
builder.Services.AddControllers().AddMetrics();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMetricsAllMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
