using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.AdminPortal.Configs;

public class SeatsIo
{
    [Required]
    public string WorkspaceKey { get; set; } = string.Empty;

    [Required]
    public string EventKey { get; set; } = string.Empty;

    [Required]
    public string EventName { get; set; } = string.Empty;
}
