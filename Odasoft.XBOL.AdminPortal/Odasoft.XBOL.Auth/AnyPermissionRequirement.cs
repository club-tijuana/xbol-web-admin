using Microsoft.AspNetCore.Authorization;

namespace Odasoft.XBOL.Auth;

public sealed class AnyPermissionRequirement : IAuthorizationRequirement
{
    public IReadOnlyList<string> Permissions { get; }

    public AnyPermissionRequirement(IEnumerable<string> permissions)
    {
        Permissions = permissions
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (Permissions.Count == 0)
            throw new ArgumentException("At least one valid permission is required.", nameof(permissions));
    }
}
