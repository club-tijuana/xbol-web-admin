namespace Odasoft.XBOL.AdminPortal.ViewModels.SeasonPass;

public record SeasonPassViewModel(
    string OrderReference,
    string PhoneNumber,
    string PhoneNumberCountryCode,
    string Email,
    int QuantityTickets,
    decimal TotalAmount
)
{
    //public string Availability => $"{Available:D3}/{Total:D3}";
}
