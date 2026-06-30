using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public sealed class PaymentLinkOptions
{
    [Range(1, 720)]
    [DefaultValue(48)]
    [Description("Default payment-link expiration duration, in hours, used by Admin Portal when creating or regenerating payment links")]
    public int DefaultExpirationHours { get; set; } = 48;
}
