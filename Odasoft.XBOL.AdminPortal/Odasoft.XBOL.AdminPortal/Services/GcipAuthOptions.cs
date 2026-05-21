using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class GcipAuthOptions
{
    [Required]
    [Description("Firebase Auth tenant ID for admin users")]
    public string TenantId { get; set; } = string.Empty;

    [Required]
    [Description("Firebase project ID")]
    public string ProjectId { get; set; } = string.Empty;

    [Description("Path to a Firebase service account JSON file")]
    public string? ServiceAccountJsonPath { get; set; }

    [Description("Firebase service account JSON content")]
    public string? ServiceAccountJson { get; set; }
}
