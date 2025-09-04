using Daylog.Application.Abstractions.Dtos;

namespace Daylog.Application.Abstractions.Services;

public interface IService<TRequestDto, TResponse> 
    where TRequestDto : class, IRequestDto
    //where TResponse : Result
{
    Task<TResponse> HandleAsync(TRequestDto requestDto, CancellationToken cancellationToken = default);
}

public interface IService<TRequestDto> 
    where TRequestDto : class, IRequestDto
{
    Task HandleAsync(TRequestDto requestDto, CancellationToken cancellationToken = default);
}
