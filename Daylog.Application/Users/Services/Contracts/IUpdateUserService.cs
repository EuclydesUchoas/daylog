using Daylog.Application.Abstractions.Services;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;

namespace Daylog.Application.Users.Services.Contracts;

public interface IUpdateUserService : IService<UpdateUserRequestDto, UserResponseDto>;
