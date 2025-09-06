using Daylog.Application.Shared.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public interface IAppConfiguration
{
    /*static IAppConfiguration CreateDefaultInstance(IConfiguration configuration)
        => new AppConfiguration(configuration);*/

    string? GetDatabaseConnectionString();

    DatabaseProviderEnum GetDatabaseProvider();

    DocumentationProviderEnum GetDocumentationProvider();
}
