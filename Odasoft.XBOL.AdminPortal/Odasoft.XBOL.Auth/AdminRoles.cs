namespace Odasoft.XBOL.Auth;

public static class AdminRoles
{
    public const string Printer = "printer";
    public const string TicketSeller = "ticket_seller";
    public const string CreditTicketSeller = "credit_ticket_seller";
    public const string Supervisor = "supervisor";
    public const string Administrator = "administrator";
    public const string SupportAgent = "support.agent";

    public static class Debug
    {
        public const string Admin = "debug.admin";
    }

    public static readonly IReadOnlyList<AdminRoleDefinition> All =
    [
        new(Printer, "Printer", "roles.printer"),
        new(TicketSeller, "Ticket Seller", "roles.ticket_seller"),
        new(CreditTicketSeller, "Credit Ticket Seller", "roles.credit_ticket_seller"),
        new(Supervisor, "Supervisor", "roles.supervisor"),
        new(Administrator, "Administrator", "roles.administrator"),
        new(SupportAgent, "Support Agent", "roles.support.agent"),
        new(Debug.Admin, "Debug Admin", "roles.debug.admin")
    ];
}

public sealed record AdminRoleDefinition(string Code, string Name, string DisplayNameKey);
