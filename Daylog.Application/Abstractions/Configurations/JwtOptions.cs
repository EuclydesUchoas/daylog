using System.ComponentModel.DataAnnotations;

namespace Daylog.Application.Abstractions.Configurations;

public sealed class JwtOptions
{
    [Required]
    public string Secret { get; set; } = null!;

    [Required]
    public string Issuer { get; set; } = null!;

    [Required]
    public string Audience { get; set; } = null!;

    [Required, Range(1, 1440)] // 1 minute to 24 hours
    public int TokenExpirationInMinutes { get; set; }
}
