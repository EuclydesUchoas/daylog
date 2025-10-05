using Daylog.Domain.Users;

namespace Daylog.Application.Abstractions.Authentication;

public interface IUserContext
{
    UserId? UserId { get; }
}
