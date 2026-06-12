using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Event
{
    public record EventInfoModel()
    {
        public VenueResponse? SelectedVenue { get; set; }
        public VenueMapResponse? SelectedVenueMap { get; set; }
        public long? OrganizerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public AgeRestriction? AgeRestriction { get; set; }
        public string? SecurityPolicies { get; set; }
        public string? AdditionalComments { get; set; }

        public IReadOnlyCollection<AdminEventCategoryResult> SelectedCategories { get; set; } = [];
    }
}
