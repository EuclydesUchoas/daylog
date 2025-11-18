namespace Daylog.Domain.Users;

public sealed record UserId(Guid Value)
    : GuidEntityId<UserId>(Value);
