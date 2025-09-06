namespace Daylog.Domain.Users;

public readonly record struct UserId(Guid Value) : IEntityId<UserId>
{
    public static UserId Empty
        => new(IEntityId.EmptyGuid());

    public static UserId CreateNew()
        => new(IEntityId.NewGuid());

    public static UserId CreateExisting(Guid value)
        => new(value);
}
