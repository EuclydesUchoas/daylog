using Daylog.Api;
using Daylog.Api.Extensions;
using Daylog.Api.Middlewares;
using Daylog.Application;
using Daylog.Infrastructure;
using Daylog.Infrastructure.Database.Factories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

builder.Configuration.AddEnvironmentVariables("DAYLOG_");
builder.Host.UseSerilog((context, options) => options.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

//app.MapDefaultEndpoints();

var databaseFactory = app.Services.GetRequiredService<IDatabaseFactory>();

databaseFactory.StartDatabase(DatabaseStarterStrategyEnum.DefaultCreator);
databaseFactory.RunMigrations(migrateUp: true);

app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDocumentation();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app
    .UseHttpsRedirection()
    .UseSerilogRequestLogging();

app
    .UseMiddleware<ExceptionMiddleware>();

app
    .UseAuthentication()
    .UseAuthorization();

app.MapEndpoints();

app.Run();
