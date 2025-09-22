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

builder.Host.UseSerilog((context, options) => options.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

var databaseFactory = app.Services.GetRequiredService<IDatabaseFactory>();

databaseFactory.StartDatabase();
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
    .UseSerilogRequestLogging()
    .UseMiddleware<ExceptionMiddleware>()
    .UseAuthentication()
    .UseAuthorization();

app.MapEndpoints();

app.Run();
