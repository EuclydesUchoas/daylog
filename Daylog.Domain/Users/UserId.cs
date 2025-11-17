namespace Daylog.Domain.Users;

public readonly record struct UserId(Guid Value)
    : IGuidEntityId<UserId>
{
    public static UserId New()
        => new(IGuidEntityId<UserId>.NewId());

    public static UserId Existing(Guid value)
        => new(value);
}
