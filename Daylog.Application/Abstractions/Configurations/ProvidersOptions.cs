using Daylog.Shared.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public sealed class ProvidersOptions
{
    public required DatabaseProviderEnum Database { get; init; }

    public required DocumentationProviderEnum Documentation { get; init; }
}
