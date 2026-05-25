using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public sealed class DataProtectionKeyRingOptions
{
    [Required]
    [DefaultValue("Odasoft.XBOL.AdminPortal")]
    [Description("ASP.NET Core DataProtection application name shared by all admin portal instances.")]
    public string ApplicationName { get; set; } = "Odasoft.XBOL.AdminPortal";

    [Required]
    [Description("Google Cloud Storage object name containing the admin portal DataProtection key ring XML.")]
    public string KeyRingObjectName { get; set; } = string.Empty;
}
