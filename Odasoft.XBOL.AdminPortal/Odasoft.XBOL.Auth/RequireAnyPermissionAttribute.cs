using Microsoft.AspNetCore.Authorization;

namespace Odasoft.XBOL.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class RequireAnyPermissionAttribute : AuthorizeAttribute
{
    public RequireAnyPermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
        Policy = PermissionPolicy.BuildAny(permissions);
    }

    public IReadOnlyList<string> Permissions { get; }
}
