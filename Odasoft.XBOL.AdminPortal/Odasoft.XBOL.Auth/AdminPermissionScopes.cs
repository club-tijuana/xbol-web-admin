using System.Security.Claims;

namespace Odasoft.XBOL.Auth;

public static class AdminPermissionScopes
{
    public static EventReadScope GetEventReadScope(ClaimsPrincipal user)
    {
        if (HasPermission(user, AdminPermissions.Events.Read))
            return EventReadScope.All;

        var scope = EventReadScope.None;

        if (HasPermission(user, AdminPermissions.Events.ReadCurrent))
            scope |= EventReadScope.Current;

        if (HasPermission(user, AdminPermissions.Events.ReadFuture))
            scope |= EventReadScope.Future;

        if (HasPermission(user, AdminPermissions.Events.ReadPast))
            scope |= EventReadScope.Past;

        return scope;
    }

    public static SalesReportReadScope GetSalesReportReadScope(ClaimsPrincipal user)
    {
        if (HasPermission(user, AdminPermissions.Reports.Sales.AllRead))
            return SalesReportReadScope.All;

        var scope = SalesReportReadScope.None;

        if (HasPermission(user, AdminPermissions.Reports.Sales.SelfRead))
            scope |= SalesReportReadScope.Self;

        if (HasPermission(user, AdminPermissions.Reports.Sales.CashiersRead))
            scope |= SalesReportReadScope.Cashiers;

        if (HasPermission(user, AdminPermissions.Reports.Sales.WebRead))
            scope |= SalesReportReadScope.Web;

        return scope;
    }

    public static bool HasPermission(ClaimsPrincipal user, string permission)
    {
        return user.HasClaim(AdminClaimTypes.Permission, permission);
    }
}
