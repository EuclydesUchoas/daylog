namespace Daylog.Application.Authentication.Models;

public sealed record TokenInfo(
    string Token,
    DateTime ExpiresAt
    );
