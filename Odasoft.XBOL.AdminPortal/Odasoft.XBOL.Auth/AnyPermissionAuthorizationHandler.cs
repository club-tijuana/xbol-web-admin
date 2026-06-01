using Microsoft.AspNetCore.Authorization;

namespace Odasoft.XBOL.Auth;

public sealed class AnyPermissionAuthorizationHandler : AuthorizationHandler<AnyPermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AnyPermissionRequirement requirement)
    {
        if (requirement.Permissions.Any(permission =>
            context.User.HasClaim(AdminClaimTypes.Permission, permission)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
