namespace Daylog.Application.Authentication.Models;

public sealed record RefreshTokenInfo(
    string Token,
    DateTime ExpiresAt
    );
