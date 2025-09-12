using Daylog.Shared.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public interface IAppConfiguration
{
    string? GetDatabaseConnectionString();

    DatabaseProviderEnum GetDatabaseProvider();

    DocumentationProviderEnum GetDocumentationProvider();
}
