using Daylog.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Daylog.Application.Abstractions.Configurations;

public sealed class ProvidersOptions
{
    [Required, DeniedValues(DatabaseProviderEnum.None)]
    public DatabaseProviderEnum Database { get; set; }

    [Required, DeniedValues(DocumentationProviderEnum.None)]
    public DocumentationProviderEnum Documentation { get; set; }
}
