using Daylog.Application.Enums;
using Microsoft.Extensions.Configuration;

namespace Daylog.Application.Helpers.Configuration;

public interface IConfigurationHelper
{
    static IConfigurationHelper CreateDefaultInstance(IConfiguration configuration)
        => new ConfigurationHelper(configuration);

    string? GetDatabaseConnectionString();

    DatabaseProviderEnum GetDatabaseProvider();

    DocumentationProviderEnum GetDocumentationProvider();
}
