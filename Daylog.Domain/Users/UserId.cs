namespace Daylog.Domain.Users;

public readonly record struct UserId(Guid Value)
    : IEntityId<UserId>
{
    public static UserId New()
        => new(IEntityId.NewGuid());

    public static UserId Existing(Guid value)
        => new(value);
}
