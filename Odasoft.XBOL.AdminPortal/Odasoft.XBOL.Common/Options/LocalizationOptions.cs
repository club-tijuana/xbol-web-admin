using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public sealed class LocalizationOptions
{
    [Required]
    [MinLength(1)]
    [Description("List of supported culture codes")]
    public string[] SupportedCultures { get; set; } = ["es-MX"];

    [Required]
    [Description("Default culture code used by Admin Portal")]
    public string DefaultCulture { get; set; } = "es-MX";

    [Required]
    [DefaultValue("America/Tijuana")]
    [Description("IANA or Windows timezone ID used for admin-facing date and time display")]
    public string TimeZoneId { get; set; } = "America/Tijuana";
}
