using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public sealed class CloudStorageOptions
{
    [Required]
    [Description("Google Cloud Storage bucket name used by shared portal infrastructure.")]
    public string BucketName { get; set; } = string.Empty;

    [Description("Google Cloud service account JSON content.")]
    public string? ServiceAccountJson { get; set; }

    [Description("Path to a Google Cloud service account JSON file.")]
    public string? ServiceAccountJsonPath { get; set; }
}
