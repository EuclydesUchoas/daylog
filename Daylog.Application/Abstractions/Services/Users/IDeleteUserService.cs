using Daylog.Application.Dtos.Users.Request;

namespace Daylog.Application.Abstractions.Services.Users;

public interface IDeleteUserService : IService<DeleteUserRequestDto, bool>;
