using Microsoft.AspNetCore.Authorization;

namespace Odasoft.XBOL.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
        Policy = PermissionPolicy.Build(permission);
    }

    public string Permission { get; }
}
