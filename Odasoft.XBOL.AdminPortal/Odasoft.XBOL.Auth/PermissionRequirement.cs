using Microsoft.AspNetCore.Authorization;

namespace Odasoft.XBOL.Auth;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
