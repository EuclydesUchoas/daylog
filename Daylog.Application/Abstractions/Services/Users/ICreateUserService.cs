using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Results;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Abstractions.Services.Users;

public interface ICreateUserService : IService<CreateUserRequestDto, Result<User>>;
