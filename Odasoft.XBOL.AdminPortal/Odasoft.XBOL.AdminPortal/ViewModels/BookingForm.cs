using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels;

public class BookingForm
{
    public string? TicketType { get; set; } = "Boleto";
    public string? DeliveryType { get; set; } = "Impreso";
    public bool AcceptTermsAndConditions { get; set; } = false;
    public long? ClientId { get; set; }
    public long? PhoneRegionCodeId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? City { get; set; }
    public string? Neighborhood { get; set; }
    public ClientGender? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public decimal? CardAmount { get; set; }
    public decimal? CashAmount { get; set; }
    public decimal? DolarAmount { get; set; }
    public decimal? CreditAmount { get; set; }
    public decimal? OtherAmount { get; set; }
}
