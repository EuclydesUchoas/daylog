using Daylog.Api;
using Daylog.Api.Extensions;
using Daylog.Application;
using Daylog.Infrastructure;
using Daylog.Infrastructure.Database.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

var databaseFactory = app.Services.GetRequiredService<IDatabaseFactory>();

databaseFactory.StartDatabase();
databaseFactory.RunMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDocumentation();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
