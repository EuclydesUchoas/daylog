using System.ComponentModel.DataAnnotations;

namespace Daylog.Application.Abstractions.Configurations;

public sealed class ConnectionStringsOptions
{
    [Required]
    public string Database { get; set; } = null!;
}
