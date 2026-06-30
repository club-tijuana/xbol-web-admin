namespace Odasoft.XBOL.Auth;

public static class AdminPermissions
{
    public static class Auth
    {
        public const string InvitationsCreate = "auth.invitations.create";
    }

    public static class Categories
    {
        public const string Read = "categories.read";
    }

    public static class Clients
    {
        public const string Read = "clients.read";
        public const string Create = "clients.create";
        public const string Update = "clients.update";
        public const string Delete = "clients.delete";
    }

    public static class Docs
    {
        public const string Read = "docs.read";
    }

    public static class CreditAccounts
    {
        public const string Read = "credit_accounts.read";
        public const string Create = "credit_accounts.create";
        public const string Update = "credit_accounts.update";
        public const string Delete = "credit_accounts.delete";
    }

    public static class CreditTransactions
    {
        public const string Read = "credit_transactions.read";
        public const string Create = "credit_transactions.create";
        public const string Update = "credit_transactions.update";
        public const string Delete = "credit_transactions.delete";
    }

    public static class LegalRepresentatives
    {
        public const string Read = "legal_representatives.read";
        public const string Create = "legal_representatives.create";
        public const string Update = "legal_representatives.update";
        public const string Delete = "legal_representatives.delete";
    }

    public static class Media
    {
        public const string Read = "media.read";
        public const string Create = "media.create";
        public const string Update = "media.update";
        public const string Delete = "media.delete";
    }

    public static class Events
    {
        public const string Read = "events.read";
        public const string ReadCurrent = "events.read_current";
        public const string ReadFuture = "events.read_future";
        public const string ReadPast = "events.read_past";
        public const string Create = "events.create";
        public const string Update = "events.update";
        public const string Delete = "events.delete";
    }

    public static class Orders
    {
        public const string Read = "orders.read";
        public const string Create = "orders.create";
        public const string Update = "orders.update";
        public const string Cancel = "orders.cancel";
        public const string OverrideSaleWindow = "orders.override_sale_window";
    }

    public static class Prices
    {
        public const string Read = "prices.read";
        public const string Create = "prices.create";
        public const string Update = "prices.update";
        public const string Publish = "prices.publish";
    }

    public static class PhoneRegionCodes
    {
        public const string Read = "phone_region_codes.read";
    }

    public static class Reports
    {
        public const string Read = "reports.read";

        public static class Sales
        {
            public const string SelfRead = "reports.sales.self.read";
            public const string CashiersRead = "reports.sales.cashiers.read";
            public const string WebRead = "reports.sales.web.read";
            public const string AllRead = "reports.sales.all.read";
        }
    }

    public static class Roles
    {
        public const string Read = "roles.read";
        public const string Create = "roles.create";
        public const string Update = "roles.update";
        public const string Delete = "roles.delete";
        public const string AssignPermissions = "roles.assign_permissions";
    }

    public static class Seats
    {
        public const string Read = "seats.read";
        public const string Reserve = "seats.reserve";
        public const string Block = "seats.block";
        public const string Hold = "seats.hold";
        public const string Book = "seats.book";
    }

    public static class Support
    {
        public const string Read = "support.read";
    }

    public static class Seasons
    {
        public const string Read = "seasons.read";
        public const string Create = "seasons.create";
        public const string Update = "seasons.update";
        public const string Delete = "seasons.delete";
    }

    public static class SeasonPasses
    {
        public const string Read = "season_passes.read";
        public const string Update = "season_passes.update";
    }

    public static class Settings
    {
        public const string Read = "settings.read";
    }

    public static class Suites
    {
        public const string Read = "suites.read";
        public const string Create = "suites.create";
        public const string Update = "suites.update";
        public const string Delete = "suites.delete";
    }

    public static class SuiteAgreements
    {
        public const string Read = "suite_agreements.read";
        public const string Create = "suite_agreements.create";
        public const string Update = "suite_agreements.update";
        public const string Delete = "suite_agreements.delete";
    }

    public static class SuiteAmenities
    {
        public const string Read = "suite_amenities.read";
        public const string Update = "suite_amenities.update";
    }

    public static class SuiteImages
    {
        public const string Read = "suite_images.read";
        public const string Create = "suite_images.create";
        public const string Update = "suite_images.update";
        public const string Delete = "suite_images.delete";
    }

    public static class SuiteLevels
    {
        public const string Read = "suite_levels.read";
        public const string Create = "suite_levels.create";
        public const string Update = "suite_levels.update";
        public const string Delete = "suite_levels.delete";
    }

    public static class Tests
    {
        public const string EmailRead = "tests.email_read";
        public const string EmailSend = "tests.email_send";
    }

    public static class Tickets
    {
        public const string Read = "tickets.read";
        public const string Print = "tickets.print";
        public const string Reissue = "tickets.reissue";
    }

    public static class Users
    {
        public const string Read = "users.read";
        public const string Update = "users.update";
        public const string AssignRoles = "users.assign_roles";
    }

    public static class Venues
    {
        public const string Read = "venues.read";
        public const string Create = "venues.create";
        public const string Update = "venues.update";
        public const string Delete = "venues.delete";
    }

    public static class VenueAmenities
    {
        public const string Read = "venue_amenities.read";
        public const string Update = "venue_amenities.update";
    }

    public static class VenueImages
    {
        public const string Read = "venue_images.read";
        public const string Create = "venue_images.create";
        public const string Update = "venue_images.update";
        public const string Delete = "venue_images.delete";
    }

    public static class VenueMaps
    {
        public const string Read = "venue_maps.read";
        public const string Create = "venue_maps.create";
        public const string Update = "venue_maps.update";
        public const string Delete = "venue_maps.delete";
    }

    public static class Worker
    {
        public const string Dashboard = "worker.dashboard";
    }

    public static readonly IReadOnlyList<AdminPermissionDefinition> All =
    [
        Definition(Auth.InvitationsCreate),
        Definition(Categories.Read),
        Definition(Clients.Read),
        Definition(Clients.Create),
        Definition(Clients.Update),
        Definition(Clients.Delete),
        Definition(Docs.Read),
        Definition(CreditAccounts.Read),
        Definition(CreditAccounts.Create),
        Definition(CreditAccounts.Update),
        Definition(CreditAccounts.Delete),
        Definition(CreditTransactions.Read),
        Definition(CreditTransactions.Create),
        Definition(CreditTransactions.Update),
        Definition(CreditTransactions.Delete),
        Definition(LegalRepresentatives.Read),
        Definition(LegalRepresentatives.Create),
        Definition(LegalRepresentatives.Update),
        Definition(LegalRepresentatives.Delete),
        Definition(Media.Read),
        Definition(Media.Create),
        Definition(Media.Update),
        Definition(Media.Delete),
        Definition(Events.Read),
        Definition(Events.ReadCurrent, "events", "read_current"),
        Definition(Events.ReadFuture, "events", "read_future"),
        Definition(Events.ReadPast, "events", "read_past"),
        Definition(Events.Create),
        Definition(Events.Update),
        Definition(Events.Delete),
        Definition(Orders.Read),
        Definition(Orders.Create),
        Definition(Orders.Update),
        Definition(Orders.Cancel, "orders", "cancel"),
        Definition(Prices.Read),
        Definition(Prices.Create),
        Definition(Prices.Update),
        Definition(Prices.Publish, "prices", "publish"),
        Definition(PhoneRegionCodes.Read),
        Definition(Reports.Read),
        Definition(Reports.Sales.SelfRead, "reports", "read_self_sales"),
        Definition(Reports.Sales.CashiersRead, "reports", "read_cashier_sales"),
        Definition(Reports.Sales.WebRead, "reports", "read_web_sales"),
        Definition(Reports.Sales.AllRead, "reports", "read_all_sales"),
        Definition(Roles.Read),
        Definition(Roles.Create),
        Definition(Roles.Update),
        Definition(Roles.Delete),
        Definition(Roles.AssignPermissions, "roles", "assign_permissions"),
        Definition(Seats.Read),
        Definition(Seats.Reserve, "seats", "reserve"),
        Definition(Seats.Block, "seats", "block"),
        Definition(Seats.Hold, "seats", "hold"),
        Definition(Seats.Book, "seats", "book"),
        Definition(Seasons.Read),
        Definition(Seasons.Create),
        Definition(Seasons.Update),
        Definition(Seasons.Delete),
        Definition(SeasonPasses.Read),
        Definition(SeasonPasses.Update),
        Definition(Settings.Read),
        Definition(Suites.Read),
        Definition(Suites.Create),
        Definition(Suites.Update),
        Definition(Suites.Delete),
        Definition(SuiteAgreements.Read),
        Definition(SuiteAgreements.Create),
        Definition(SuiteAgreements.Update),
        Definition(SuiteAgreements.Delete),
        Definition(SuiteAmenities.Read),
        Definition(SuiteAmenities.Update),
        Definition(SuiteImages.Read),
        Definition(SuiteImages.Create),
        Definition(SuiteImages.Update),
        Definition(SuiteImages.Delete),
        Definition(SuiteLevels.Read),
        Definition(SuiteLevels.Create),
        Definition(SuiteLevels.Update),
        Definition(SuiteLevels.Delete),
        Definition(Tests.EmailRead),
        Definition(Tests.EmailSend),
        Definition(Tickets.Read),
        Definition(Tickets.Print, "tickets", "print"),
        Definition(Tickets.Reissue, "tickets", "reissue"),
        Definition(Users.Read),
        Definition(Users.Update),
        Definition(Users.AssignRoles, "users", "assign_roles"),
        Definition(Venues.Read),
        Definition(Venues.Create),
        Definition(Venues.Update),
        Definition(Venues.Delete),
        Definition(VenueAmenities.Read),
        Definition(VenueAmenities.Update),
        Definition(VenueImages.Read),
        Definition(VenueImages.Create),
        Definition(VenueImages.Update),
        Definition(VenueImages.Delete),
        Definition(VenueMaps.Read),
        Definition(VenueMaps.Create),
        Definition(VenueMaps.Update),
        Definition(VenueMaps.Delete),
        Definition(Worker.Dashboard, "worker", "dashboard"),
        Definition(Support.Read)
    ];

    public static readonly IReadOnlyList<string> AllCodes =
        All.Select(x => x.Code).ToArray();

    private static AdminPermissionDefinition Definition(string code)
    {
        var parts = code.Split('.', 2);
        if (parts.Length < 2)
            throw new ArgumentException($"Permission code must contain a dot: '{code}'", nameof(code));
        return new AdminPermissionDefinition(code, parts[0], parts[1], $"permissions.{code}");
    }

    private static AdminPermissionDefinition Definition(string code, string group, string action)
    {
        return new AdminPermissionDefinition(code, group, action, $"permissions.{code}");
    }
}

public sealed record AdminPermissionDefinition(
    string Code,
    string Group,
    string Action,
    string DisplayNameKey);
