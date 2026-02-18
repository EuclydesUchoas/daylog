using Daylog.Domain.Users;
using Daylog.Shared.Core.Temporal;

namespace Daylog.Domain.RefreshTokens;

public sealed class RefreshToken : Entity, ICreatable, IUpdatable, IDeletable
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;
    
    public string Token { get; private set; } = null!;
    
    public DateTime ExpiresAt { get; private set; }

    public bool IsRevoked { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public Guid? RevokedByUserId { get; private set; }

    public User? RevokedByUser { get; private set; }

    // Creatable

    public DateTime CreatedAt { get; private set; }

    public Guid? CreatedByUserId { get; private set; }

    public User? CreatedByUser { get; private set; }

    // Updatable

    public DateTime UpdatedAt { get; private set; }

    public Guid? UpdatedByUserId { get; private set; }

    public User? UpdatedByUser { get; private set; }

    public void Update(RefreshToken refreshToken)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(refreshToken.UserId, UserId, nameof(UserId));

        UserId = refreshToken.UserId;
        Token = refreshToken.Token;
        ExpiresAt = refreshToken.ExpiresAt;
        IsRevoked = refreshToken.IsRevoked;
        RevokedAt = refreshToken.RevokedAt;
        RevokedByUserId = refreshToken.RevokedByUserId;
        RevokedByUser = refreshToken.RevokedByUser;
    }

    public static RefreshToken New(Guid userId, string token, DateTime expiresAt)
    {
        return new RefreshToken
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
        };
    }

    public static RefreshToken Existing(Guid id, Guid userId, string token, DateTime expiresAt, bool isRevoked, DateTime revokedAt, Guid? revokedByUserId)
    {
        return new RefreshToken
        {
            Id = id,
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            IsRevoked = isRevoked,
            RevokedAt = revokedAt,
            RevokedByUserId = revokedByUserId,
        };
    }

    public void ChangeToken(string newToken, DateTime newExpiresAt)
    {
        Token = newToken;
        ExpiresAt = newExpiresAt;
    }

    public void Revoke(Guid revokedByUserId, IDateTimeProvider dateTimeProvider)
    {
        IsRevoked = true;
        RevokedByUserId = revokedByUserId;
        RevokedAt = dateTimeProvider.UtcNow;
        RevokedByUser = null;
    }

    public bool IsExpired(IDateTimeProvider dateTimeProvider)
    {
        return ExpiresAt < dateTimeProvider.UtcNow;
    }

    public bool IsValid(IDateTimeProvider dateTimeProvider)
    {
        return !IsRevoked && !IsExpired(dateTimeProvider);
    }
}
