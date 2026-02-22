namespace Daylog.Application.Authentication.Models;

public sealed record AccessTokenInfo(
    string Token,
    DateTime ExpiresAt
    );
