using Daylog.Application.Abstractions.Dtos;
using System.Numerics;

namespace Daylog.Application.Shared.Dtos.Response;

public sealed record NumberResponseDto<TResponse>(
    TResponse Value
    ) : IResponseDto
    where TResponse : struct, INumber<TResponse>;
