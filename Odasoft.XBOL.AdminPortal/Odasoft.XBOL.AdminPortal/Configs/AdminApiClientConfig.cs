using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.AdminPortal.Configs
{
    public class AdminApiClientConfig
    {
        [Required]
        public string BaseAddress { get; set; } = string.Empty;
    }
}
