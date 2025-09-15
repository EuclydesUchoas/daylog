using Daylog.Application.Abstractions.Authentications;
using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Infrastructure.Authentications;
using Daylog.Infrastructure.Configurations;
using Daylog.Infrastructure.Database.Data;
using Daylog.Infrastructure.Database.Factories;
using Daylog.Infrastructure.Database.SaveChangesInterceptors;
using Daylog.Shared.Enums;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Daylog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfiguration = new AppConfiguration(configuration); // Default implementation of IAppConfiguration
        services.AddSingleton<IAppConfiguration>(appConfiguration);

        services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        services.AddAppDbContextAndMigrationRunner(appConfiguration);

        services
            .AddAppAuthentication(appConfiguration)
            .AddAppAuthorization();

        return services;
    }

    private static IServiceCollection AddAppDbContextAndMigrationRunner(this IServiceCollection services, [SuppressMessage("Performance", "CA1859")] IAppConfiguration appConfiguration)
    {
        var databaseProvider = appConfiguration.GetDatabaseProvider();
        string? connectionString = appConfiguration.GetDatabaseConnectionString();
        
        if (databaseProvider is DatabaseProviderEnum.None)
            throw new Exception("Database provider not set.");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Connection string not provided.");

        services.AddScoped<CreatableInterceptor>();
        services.AddScoped<UpdatableInterceptor>();
        services.AddScoped<SoftDeletableInterceptor>();

        services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
        {
            options = databaseProvider switch
            {
                DatabaseProviderEnum.SqlServer => options.UseSqlServer(connectionString),
                DatabaseProviderEnum.PostgreSql => options.UseNpgsql(connectionString),
                _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
            };

            options
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    sp.GetRequiredService<CreatableInterceptor>(),
                    sp.GetRequiredService<UpdatableInterceptor>(),
                    sp.GetRequiredService<SoftDeletableInterceptor>()
                );
        });

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                runner = databaseProvider switch
                {
                    DatabaseProviderEnum.SqlServer => runner.AddSqlServer(),
                    DatabaseProviderEnum.PostgreSql => runner.AddPostgres(),
                    _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported."),
                };

                runner
                    .WithGlobalConnectionString(connectionString)
                    .WithMigrationsIn(InfrastructureAssemblyReference.Assembly);
            })
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        return services;
    }

    private static IServiceCollection AddAppAuthentication(this IServiceCollection services, [SuppressMessage("Performance", "CA1859")] IAppConfiguration appConfiguration)
    {
        string? jwtSecret = appConfiguration.GetJwtSecretKey();
        string? jwtIssuer = appConfiguration.GetJwtIssuer();
        string? jwtAudience = appConfiguration.GetJwtAudience();
        int jwtTokenExpirationInMinutes = appConfiguration.GetJwtTokenExpirationInMinutes();

        if (string.IsNullOrWhiteSpace(jwtSecret))
            throw new Exception("JWT Secret Key not provided.");

        if (string.IsNullOrWhiteSpace(jwtIssuer))
            throw new Exception("JWT Issuer not provided.");

        if (string.IsNullOrWhiteSpace(jwtAudience))
            throw new Exception("JWT Audience not provided.");

        if (jwtTokenExpirationInMinutes <= 0)
            throw new Exception("JWT Token Expiration not provided or invalid.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }

    private static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
