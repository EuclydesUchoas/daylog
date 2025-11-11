using Daylog.Application.Abstractions.Configurations;
using Daylog.Shared.Enums;
using Microsoft.Extensions.Configuration;

namespace Daylog.Infrastructure.Configurations;

public sealed class AppConfiguration : IAppConfiguration
{
    // Configuration keys
    // Connection strings
    const string KeyConnectionStrings = "ConnectionStrings";
    const string KeyDatabaseConnectionString = "Database";
    // Aspire
    const string AspireDatabaseKeyPostgreSqlConnectionString = "daylog-postgres-db";
    const string AspireDatabaseKeySqlServerConnectionString = "daylog-sqlserver-db";
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
    const string EnvVarPrefix = "DAYLOG_";
    // Providers
    const string EnvVarDatabaseProvider = $"{EnvVarPrefix}DATABASE_PROVIDER";
    const string EnvVarDocumentationProvider = $"{EnvVarPrefix}DOCUMENTATION_PROVIDER";
    // Connection strings
    const string EnvVarDatabaseConnectionString = $"{EnvVarPrefix}DATABASE_CONNECTION_STRING";
    // JWT
    const string EnvVarJwtSecret = $"{EnvVarPrefix}JWT_SECRET";
    const string EnvVarJwtIssuer = $"{EnvVarPrefix}JWT_ISSUER";
    const string EnvVarJwtAudience = $"{EnvVarPrefix}JWT_AUDIENCE";
    const string EnvVarJwtTokenExpiration = $"{EnvVarPrefix}JWT_TOKEN_EXPIRATION_IN_MINUTES";

    private static readonly Dictionary<DatabaseProviderEnum, string> AspireDatabaseProviderConnectionStringKeys = new()
    {
        [DatabaseProviderEnum.PostgreSql] = AspireDatabaseKeyPostgreSqlConnectionString,
        [DatabaseProviderEnum.SqlServer] = AspireDatabaseKeySqlServerConnectionString,
    };

    // Implementation

    private readonly IConfiguration _configuration;

    public DatabaseProviderEnum DatabaseProvider { get; }

    public string DatabaseConnectionString { get; }

    public DocumentationProviderEnum DocumentationProvider { get; }

    public string JwtSecretKey { get; }

    public string JwtIssuer { get; }

    public string JwtAudience { get; }

    public int JwtTokenExpirationInMinutes { get; }

    public AppConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;

        DatabaseProvider = LoadDatabaseProvider();
        DatabaseConnectionString = LoadDatabaseConnectionString()!;
        DocumentationProvider = LoadDocumentationProvider();
        JwtSecretKey = LoadJwtSecretKey()!;
        JwtIssuer = LoadJwtIssuer()!;
        JwtAudience = LoadJwtAudience()!;
        JwtTokenExpirationInMinutes = LoadJwtTokenExpirationInMinutes();

        AssertConfigurationIsValid();
    }

    public void AssertConfigurationIsValid()
    {
        if (DatabaseProvider is DatabaseProviderEnum.None)
        {
            throw new ArgumentException("Database provider is not configured properly.", nameof(DatabaseProvider));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(DatabaseConnectionString, nameof(DatabaseConnectionString));
        if (DocumentationProvider is DocumentationProviderEnum.None)
        {
            throw new ArgumentException("Documentation provider is not configured properly.", nameof(DocumentationProvider));
        }
        /*ArgumentException.ThrowIfNullOrWhiteSpace(JwtSecretKey, nameof(JwtSecretKey));
        ArgumentException.ThrowIfNullOrWhiteSpace(JwtIssuer, nameof(JwtIssuer));
        ArgumentException.ThrowIfNullOrWhiteSpace(JwtAudience, nameof(JwtAudience));
        if (JwtTokenExpirationInMinutes <= 0)
        {
            throw new ArgumentException("JWT token expiration time is not configured properly.", nameof(JwtTokenExpirationInMinutes));
        }*/
    }

    // Helper methods to retrieve configuration values

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

    // Loaders for specific configuration values

    private DatabaseProviderEnum LoadDatabaseProvider()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarDatabaseProvider, out string? databaseProviderName)
            && !TryGetConfigurationValue(_configuration, KeyPathDatabaseProvider, out databaseProviderName))
        {
            return DatabaseProviderEnum.None;
        }

        if (Enum.TryParse(databaseProviderName, true, out DatabaseProviderEnum databaseProvider))
        {
            return databaseProvider;
        }

        return DatabaseProviderEnum.None;
    }

    private string? LoadDatabaseConnectionString()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarDatabaseConnectionString, out string? connectionString))
        {
            if (AspireDatabaseProviderConnectionStringKeys.TryGetValue(DatabaseProvider, out string? connectionStringKey)
                && !string.IsNullOrWhiteSpace(connectionString = _configuration.GetConnectionString(connectionStringKey)))
            {
                return connectionString;
            }
            connectionString = _configuration.GetConnectionString(KeyDatabaseConnectionString);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }
        }

        return connectionString;
    }

    private DocumentationProviderEnum LoadDocumentationProvider()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarDocumentationProvider, out string? documentationProviderName)
            && !TryGetConfigurationValue(_configuration, KeyPathDocumentationProvider, out documentationProviderName))
        {
            return DocumentationProviderEnum.None;
        }

        if (Enum.TryParse(documentationProviderName, true, out DocumentationProviderEnum documentationProvider))
        {
            return documentationProvider;
        }

        return DocumentationProviderEnum.None;
    }

    private string? LoadJwtSecretKey()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtSecret, out string? jwtSecret)
            && !TryGetConfigurationValue(_configuration, KeyPathJwtSecret, out jwtSecret))
        {
            return null;
        }

        return jwtSecret;
    }

    private string? LoadJwtIssuer()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtIssuer, out string? jwtIssuer)
            && !TryGetConfigurationValue(_configuration, KeyPathJwtIssuer, out jwtIssuer))
        {
            return null;
        }

        return jwtIssuer;
    }

    private string? LoadJwtAudience()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtAudience, out string? jwtAudience)
            && !TryGetConfigurationValue(_configuration, KeyPathJwtAudience, out jwtAudience))
        {
            return null;
        }

        return jwtAudience;
    }

    private int LoadJwtTokenExpirationInMinutes()
    {
        if (!TryGetEnvironmentVariableValue(EnvVarJwtTokenExpiration, out string? jwtTokenExpirationInMinutesString)
            && !TryGetConfigurationValue(_configuration, KeyPathJwtTokenExpirationInMinutes, out jwtTokenExpirationInMinutesString))
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
