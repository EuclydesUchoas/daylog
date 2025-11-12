using Daylog.Shared.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public sealed class DatabaseOptions
{
    public required DatabaseProviderEnum Provider { get; init; }

    public required string ConnectionString { get; init; } = null!;
}
