using Daylog.Api;
using Daylog.Api.Extensions;
using Daylog.Api.Middlewares;
using Daylog.Application;
using Daylog.Infrastructure;
using Daylog.Infrastructure.Database.Factories;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.MapEndpoints();

app.Run();
