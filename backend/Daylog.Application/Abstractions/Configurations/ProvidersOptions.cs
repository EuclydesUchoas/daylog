using Daylog.Shared.Core.Enums;
using Daylog.Shared.Data.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public sealed class ProvidersOptions
{
    public required DatabaseProviderEnum Database { get; init; }

    public required DocumentationProviderEnum Documentation { get; init; }
}
