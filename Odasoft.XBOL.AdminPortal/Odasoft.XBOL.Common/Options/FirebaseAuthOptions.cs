using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public class FirebaseAuthOptions
{
    [Required]
    [Description("Firebase Web API key for the admin tenant")]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    [Description("Firebase auth domain for the admin tenant")]
    public string AuthDomain { get; set; } = string.Empty;

    [Required]
    [Description("Firebase project ID for the admin tenant")]
    public string ProjectId { get; set; } = string.Empty;

    [Description("Firebase storage bucket")]
    public string? StorageBucket { get; set; }

    [Description("Firebase messaging sender ID")]
    public string? MessagingSenderId { get; set; }

    [Required]
    [Description("Firebase web app ID for the admin tenant")]
    public string AppId { get; set; } = string.Empty;

    [Description("Firebase measurement ID")]
    public string? MeasurementId { get; set; }

    [Required]
    [Description("Firebase Auth tenant ID for admin users")]
    public string TenantId { get; set; } = string.Empty;
}
