namespace Daylog.Application.Abstractions.Configurations;

public sealed class JwtOptions
{
    public required string SecretKey { get; init; } = null!;

    public required string Issuer { get; init; } = null!;

    public required string Audience { get; init; } = null!;

    public required int TokenExpirationInMinutes { get; init; }

    public required int RefreshTokenExpirationInHours { get; init; }
}
