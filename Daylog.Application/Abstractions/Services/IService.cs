using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Results;

namespace Daylog.Application.Abstractions.Services;

public interface IService<TRequestDto, TResponseDto> 
    where TRequestDto : class, IRequestDto
    where TResponseDto : class?, IResponseDto?
{
    Task<Result<TResponseDto>> HandleAsync(TRequestDto requestDto, CancellationToken cancellationToken = default);
}

public interface IService<TRequestDto> 
    where TRequestDto : class, IRequestDto
{
    Task<Result> HandleAsync(TRequestDto requestDto, CancellationToken cancellationToken = default);
}
