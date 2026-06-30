using Odasoft.XBOL.Auth;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class AdminRoleCatalogTests
{
    [Fact]
    public void AdminRoles_includes_support_agent_role()
    {
        Assert.Equal("support.agent", AdminRoles.SupportAgent);
        Assert.Contains(AdminRoles.All, role =>
            role.Code == AdminRoles.SupportAgent &&
            role.Name == "Support Agent" &&
            role.DisplayNameKey == "roles.support.agent");
    }

    [Fact]
    public void AdminRoles_includes_debug_admin_role()
    {
        Assert.Equal("debug.admin", AdminRoles.Debug.Admin);
        Assert.Contains(AdminRoles.All, role =>
            role.Code == AdminRoles.Debug.Admin &&
            role.Name == "Debug Admin" &&
            role.DisplayNameKey == "roles.debug.admin");
    }
}
