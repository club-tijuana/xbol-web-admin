namespace Odasoft.XBOL.AdminPortal.Services;

public static class SeasonPassPurchaseWindow
{
    public static bool CanBuy(
        DateTimeOffset? onSaleDate,
        DateTimeOffset? offSaleDate,
        bool hasPreviousBundle,
        DateTimeOffset? renewalEndDate,
        bool isBookable,
        DateTimeOffset now)
    {
        if (!isBookable)
        {
            return false;
        }

        if (!IsSaleOpen(onSaleDate, offSaleDate, now))
        {
            return false;
        }

        return !hasPreviousBundle
            || (renewalEndDate.HasValue && now >= renewalEndDate.Value);
    }

    public static bool IsRenewalOpen(
        DateTimeOffset? onSaleDate,
        DateTimeOffset? offSaleDate,
        DateTimeOffset? renewalStartDate,
        DateTimeOffset? renewalEndDate,
        bool isBookable,
        DateTimeOffset now)
    {
        if (!isBookable)
        {
            return false;
        }

        return IsSaleOpen(onSaleDate, offSaleDate, now)
            && renewalStartDate.HasValue
            && renewalEndDate.HasValue
            && now >= renewalStartDate.Value
            && now < renewalEndDate.Value;
    }

    public static bool CanRenew(
        DateTimeOffset? onSaleDate,
        DateTimeOffset? offSaleDate,
        DateTimeOffset? renewalStartDate,
        DateTimeOffset? renewalEndDate,
        bool isBookable,
        DateTimeOffset now)
    {
        if (!isBookable)
        {
            return false;
        }

        return IsSaleOpen(onSaleDate, offSaleDate, now)
            && renewalStartDate.HasValue
            && renewalEndDate.HasValue
            && now >= renewalStartDate.Value;
    }

    public static bool RequiresSeatSelectionForRenewal(DateTimeOffset? renewalEndDate, DateTimeOffset now)
    {
        return renewalEndDate.HasValue && now >= renewalEndDate.Value;
    }

    private static bool IsSaleOpen(
        DateTimeOffset? onSaleDate,
        DateTimeOffset? offSaleDate,
        DateTimeOffset now)
    {
        return onSaleDate.HasValue
            && offSaleDate.HasValue
            && now >= onSaleDate.Value
            && now < offSaleDate.Value;
    }
}
