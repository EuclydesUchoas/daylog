using Daylog.Shared.Enums;

namespace Daylog.Application.Abstractions.Configurations;

public interface IAppConfiguration
{
    /// <summary>
    /// Asserts that the application configuration is valid.
    /// </summary>
    void AssertConfigurationIsValid();

    /// <summary>
    /// Retrieves the connection string used to connect to the database.
    /// </summary>
    /// <returns>The database connection string if available; otherwise, <see langword="null"/>.</returns>
    string? GetDatabaseConnectionString();

    /// <summary>
    /// Retrieves the database provider type.
    /// </summary>
    /// <returns>The <see cref="DatabaseProviderEnum"/> value representing the database provider.</returns>
    DatabaseProviderEnum GetDatabaseProvider();

    /// <summary>
    /// Retrieves the documentation provider type.
    /// </summary>
    /// <returns>The <see cref="DocumentationProviderEnum"/> value representing the documentation provider.</returns>
    DocumentationProviderEnum GetDocumentationProvider();

    /// <summary>
    /// Retrieves the secret key used for JWT authentication.
    /// </summary>
    /// <returns>The JWT secret key if available; otherwise, <see langword="null"/>.</returns>
    string? GetJwtSecretKey();

    /// <summary>
    /// Retrieves the issuer of the JWT tokens.
    /// </summary>
    /// <returns>The JWT issuer if available; otherwise, <see langword="null"/>.</returns>
    string? GetJwtIssuer();

    /// <summary>
    /// Retrieves the audience for the JWT tokens.
    /// </summary>
    /// <returns>The JWT audience if available; otherwise, <see langword="null"/>.</returns>
    string? GetJwtAudience();

    /// <summary>
    /// Retrieves the expiration time for JWT tokens in minutes.
    /// </summary>
    /// <returns>The JWT expiration time in minutes.</returns>
    int GetJwtTokenExpirationInMinutes();
}
