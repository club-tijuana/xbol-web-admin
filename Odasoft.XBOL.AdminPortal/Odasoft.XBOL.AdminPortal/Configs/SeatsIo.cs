using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.AdminPortal.Configs;

public class SeatsIo
{
    [Required]
    public string PublicWorkspaceKey { get; set; } = string.Empty;
}
