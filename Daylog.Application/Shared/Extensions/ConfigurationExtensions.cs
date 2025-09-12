using Daylog.Shared.Enums;
using Microsoft.Extensions.Configuration;

namespace Daylog.Application.Shared.Extensions;

public static class ConfigurationExtensions
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

    private static bool TryGetEnvironmentVariableValue(string key, out string? value)
    {
        value = Environment.GetEnvironmentVariable(key);
        return !string.IsNullOrWhiteSpace(value);
    }

    private static bool TryGetConfigurationValue(IConfiguration configuration, string key, out string? value)
    {
        value = configuration.GetValue<string>(key);
        return !string.IsNullOrWhiteSpace(value);
    }

    public static string? GetDatabaseConnectionString(this IConfiguration configuration)
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

    public static DatabaseProviderEnum GetDatabaseProvider(this IConfiguration configuration)
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

    public static DocumentationProviderEnum GetDocumentationProvider(this IConfiguration configuration)
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
