using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public class AdminApiClientOptions
{
    [Required]
    [Description("Base URL for the Admin API")]
    public string BaseAddress { get; set; } = string.Empty;
}
