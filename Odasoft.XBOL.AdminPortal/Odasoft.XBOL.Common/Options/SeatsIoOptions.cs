using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public class SeatsIoOptions
{
    [Required]
    [Description("Seats.io workspace secret key")]
    public string SecretKey { get; set; } = string.Empty;
}
