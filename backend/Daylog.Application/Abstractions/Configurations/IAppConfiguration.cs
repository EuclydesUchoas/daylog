using Daylog.Shared.Core.Enums;
using Daylog.Shared.Data.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public interface IAppConfiguration
{
    /// <summary>
    /// Gets the database provider type.
    /// </summary>
    DatabaseProviderEnum DatabaseProvider { get; }

    /// <summary>
    /// Gets the connection string used to connect to the database.
    /// </summary>
    string DatabaseConnectionString { get; }

    /// <summary>
    /// Gets the documentation provider type.
    /// </summary>
    DocumentationProviderEnum DocumentationProvider { get; }

    /// <summary>
    /// Gets the secret key used for JWT authentication.
    /// </summary>
    string JwtSecretKey { get; }

    /// <summary>
    /// Gets the issuer of the JWT tokens.
    /// </summary>
    string JwtIssuer { get; }

    /// <summary>
    /// Gets the audience for the JWT tokens.
    /// </summary>
    string JwtAudience { get; }

    /// <summary>
    /// Gets the expiration time for JWT tokens in minutes.
    /// </summary>
    int JwtTokenExpirationInMinutes { get; }

    /// <summary>
    /// Gets the expiration time for JWT refresh tokens in hours.
    /// </summary>
    int JwtRefreshTokenExpirationInHours { get; }
}
