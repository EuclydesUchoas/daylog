namespace Daylog.Application.Abstractions.Configurations;

public sealed class ConnectionStringsOptions
{
    public required string Database { get; init; } = null!;
}
