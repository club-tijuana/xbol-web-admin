namespace Odasoft.XBOL.Auth;

public static class PermissionPolicy
{
    public const string Prefix = "Permission:";
    public const string AnyPrefix = "AnyPermission:";
    private const char Separator = '|';

    public static string Build(string permission) => $"{Prefix}{permission}";

    public static string BuildAny(params string[] permissions)
    {
        if (permissions.Length == 0 || permissions.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("At least one permission is required.", nameof(permissions));

        return $"{AnyPrefix}{string.Join(Separator, permissions)}";
    }

    public static bool TryGetPermission(string policyName, out string permission)
    {
        if (policyName.StartsWith(Prefix, StringComparison.Ordinal))
        {
            permission = policyName[Prefix.Length..];
            return !string.IsNullOrWhiteSpace(permission);
        }

        permission = string.Empty;
        return false;
    }

    public static bool TryGetAnyPermissions(string policyName, out IReadOnlyList<string> permissions)
    {
        permissions = [];

        if (!policyName.StartsWith(AnyPrefix, StringComparison.Ordinal))
            return false;

        var policyValue = policyName[AnyPrefix.Length..];
        if (string.IsNullOrWhiteSpace(policyValue))
            return false;

        var parsed = policyValue
            .Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parsed.Length == 0)
            return false;

        permissions = parsed;
        return true;
    }
}
