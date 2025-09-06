using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Shared.Enums;
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
    const string KeyDocumentationProvider = "Documentation";

    // Environment variable names
    // Connection strings
    const string EnvVarDatabaseConnectionString = "DAYLOG_DATABASE_CONNNECTION_STRING";
    // Providers
    const string EnvVarDatabaseProvider = "DAYLOG_DATABASE_PROVIDER";
    const string EnvVarDocumentationProvider = "DAYLOG_DOCUMENTATION_PROVIDER";
    
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
            && !TryGetConfigurationValue(configuration.GetSection(KeyProviders), KeyDatabaseProvider, out databaseProviderName))
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
            && !TryGetConfigurationValue(configuration.GetSection(KeyProviders), KeyDocumentationProvider, out documentationProviderName))
        {
            return DocumentationProviderEnum.None;
        }

        if (Enum.TryParse(documentationProviderName, true, out DocumentationProviderEnum documentationProvider))
        {
            return documentationProvider;
        }

        return DocumentationProviderEnum.None;
    }
}
