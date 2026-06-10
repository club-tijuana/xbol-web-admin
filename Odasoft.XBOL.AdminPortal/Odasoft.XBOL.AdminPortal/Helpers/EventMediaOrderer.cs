using Odasoft.XBOL.AdminPortal.ViewModels;

namespace Odasoft.XBOL.AdminPortal.Helpers;

public static class EventMediaOrderer
{
    public static IEnumerable<(ImageModel Image, int Order)> CompactSponsors(IEnumerable<ImageModel?> sponsorImages)
    {
        var order = 0;

        foreach (var sponsorImage in sponsorImages)
        {
            if (sponsorImage is null)
            {
                continue;
            }

            yield return (sponsorImage, order);
            order++;
        }
    }
}
