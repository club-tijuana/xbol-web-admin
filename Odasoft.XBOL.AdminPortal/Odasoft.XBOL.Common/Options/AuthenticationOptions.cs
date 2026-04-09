using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public class AuthenticationOptions
{
    [Description("List of users allowed to log in to the admin portal")]
    public List<AllowedUser> AllowedUsers { get; set; } = [];
}

public class AllowedUser
{
    [Required]
    [Description("User email address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Description("User password")]
    public string Password { get; set; } = string.Empty;
}
