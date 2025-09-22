using Daylog.Application.Abstractions.Configurations;
using Daylog.Shared.Enums;
using Microsoft.Extensions.Configuration;

namespace Daylog.Infrastructure.Configurations;

public sealed class AppConfiguration(
    IConfiguration configuration
    ) : IAppConfiguration
{
    // Configuration keys
    // Connection strings
    const string KeyConnectionStrings = "ConnectionStrings";
    const string KeyDatabaseConnectionString = "Database";
    // Providers
    const string KeyProviders = "Providers";
    const string KeyDatabaseProvider = "Database";
    const string KeyPathDatabaseProvider = $"{KeyProviders}:{KeyDatabaseProvider}";
    const string KeyDocumentationProvider = "Documentation";
    const string KeyPathDocumentationProvider = $"{KeyProviders}:{KeyDocumentationProvider}";
    // JWT
    const string KeyJwt = "Jwt";
    const string KeyJwtSecret = "Secret";
    const string KeyPathJwtSecret = $"{KeyJwt}:{KeyJwtSecret}";
    const string KeyJwtIssuer = "Issuer";
    const string KeyPathJwtIssuer = $"{KeyJwt}:{KeyJwtIssuer}";
    const string KeyJwtAudience = "Audience";
    const string KeyPathJwtAudience = $"{KeyJwt}:{KeyJwtAudience}";
    const string KeyJwtTokenExpirationInMinutes = "TokenExpirationInMinutes";
    const string KeyPathJwtTokenExpirationInMinutes = $"{KeyJwt}:{KeyJwtTokenExpirationInMinutes}";

    // Environment variable names
    // Connection strings
    const string EnvVarDatabaseConnectionString = "DAYLOG_DATABASE_CONNNECTION_STRING";
    // Providers
    const string EnvVarDatabaseProvider = "DAYLOG_DATABASE_PROVIDER";
    const string EnvVarDocumentationProvider = "DAYLOG_DOCUMENTATION_PROVIDER";
    // JWT
    const string EnvVarJwtSecret = "DAYLOG_JWT_SECRET";
    const string EnvVarJwtIssuer = "DAYLOG_JWT_ISSUER";
    const string EnvVarJwtAudience = "DAYLOG_JWT_AUDIENCE";
    const string EnvVarJwtTokenExpiration = "DAYLOG_JWT_TOKEN_EXPIRATION_IN_MINUTES";

    static bool TryGetEnvironmentVariableValue(string key, out string? value)
    {
        value = Environment.GetEnvironmentVariable(key);
        return !string.IsNullOrWhiteSpace(value);
    }

    static bool TryGetConfigurationValue(IConfiguration configuration, string key, out string? value)
    {
        value = configuration.GetValue<string>(key);
        return !string.IsNullOrWhiteSpace(value);
    }

    public void AssertConfigurationIsValid()
    {
        // Future implementation: Validate required configurations and throw exceptions if invalid.
    }

    public string? GetDatabaseConnectionString()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarDatabaseConnectionString, out string? connectionString))
        {
            connectionString = configuration.GetConnectionString(KeyDatabaseConnectionString);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }
        }

        return connectionString;
    }

    public DatabaseProviderEnum GetDatabaseProvider()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarDatabaseProvider, out string? databaseProviderName)
            && !TryGetConfigurationValue(configuration, KeyPathDatabaseProvider, out databaseProviderName))
        {
            return DatabaseProviderEnum.None;
        }

        if (Enum.TryParse(databaseProviderName, true, out DatabaseProviderEnum databaseProvider))
        {
            return databaseProvider;
        }

        return DatabaseProviderEnum.None;
    }

    public DocumentationProviderEnum GetDocumentationProvider()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarDocumentationProvider, out string? documentationProviderName)
            && !TryGetConfigurationValue(configuration, KeyPathDocumentationProvider, out documentationProviderName))
        {
            return DocumentationProviderEnum.None;
        }

        if (Enum.TryParse(documentationProviderName, true, out DocumentationProviderEnum documentationProvider))
        {
            return documentationProvider;
        }

        return DocumentationProviderEnum.None;
    }

    public string? GetJwtSecretKey()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtSecret, out string? jwtSecret)
            && !TryGetConfigurationValue(configuration, KeyPathJwtSecret, out jwtSecret))
        {
            return null;
        }

        return jwtSecret;
    }

    public string? GetJwtIssuer()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtIssuer, out string? jwtIssuer)
            && !TryGetConfigurationValue(configuration, KeyPathJwtIssuer, out jwtIssuer))
        {
            return null;
        }

        return jwtIssuer;
    }

    public string? GetJwtAudience()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtAudience, out string? jwtAudience)
            && !TryGetConfigurationValue(configuration, KeyPathJwtAudience, out jwtAudience))
        {
            return null;
        }

        return jwtAudience;
    }

    public int GetJwtTokenExpirationInMinutes()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtTokenExpiration, out string? jwtTokenExpirationInMinutesString)
            && !TryGetConfigurationValue(configuration, KeyPathJwtTokenExpirationInMinutes, out jwtTokenExpirationInMinutesString))
        {
            return 0;
        }

        if (!int.TryParse(jwtTokenExpirationInMinutesString, out int jwtTokenExpirationInMinutes)
            || jwtTokenExpirationInMinutes <= 0)
        {
            return 0;
        }

        return jwtTokenExpirationInMinutes;
    }
}
