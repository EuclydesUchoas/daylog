using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Results;

namespace Daylog.Application.Abstractions.Services;

public interface IService<TRequestDto, TResponseDto> 
    where TRequestDto : IRequestDto
    where TResponseDto : IResponseDto?
{
    Task<Result<TResponseDto>> HandleAsync(TRequestDto requestDto, CancellationToken cancellationToken = default);
}

public interface IService<TRequestDto> 
    where TRequestDto : IRequestDto
{
    Task<Result> HandleAsync(TRequestDto requestDto, CancellationToken cancellationToken = default);
}
