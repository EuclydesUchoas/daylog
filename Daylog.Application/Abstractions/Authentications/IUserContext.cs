using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Abstractions.Authentications;

public interface IUserContext
{
    UserId? UserId { get; }
}
