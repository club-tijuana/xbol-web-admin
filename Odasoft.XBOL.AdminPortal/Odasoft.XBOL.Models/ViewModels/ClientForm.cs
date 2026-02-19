using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Models.ViewModels
{
    public class ClientForm
    {
        [Required(ErrorMessage = "La empresa es requerida.")]
        public string? CompanyName { get; set; }
        public string? SocialReason { get; set; }
        public string? RFC { get; set; }
        public int? PersonTypeId { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public string? ExtNumber { get; set; }
        public string? IntNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? Neighbourhood { get; set; }
        public string? City { get; set; }
        public string? CountryPhoneISO { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public bool HasCredit { get; set; }

        public LegalRepForm LegalRep { get; set; } = new();
        public CreditForm Credit { get; set; } = new();
    }

    public class LegalRepForm
    {
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string? RFC { get; set; }
        public string? CURP { get; set; }
    }

    public class CreditForm
    {
        public decimal? AuthorizedAmount { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime? StartDate { get; set; }
        public int? PaymentCycleTypeId { get; set; }
        public bool? InterestApply { get; set; }
        public decimal? TotalPlusInterest { get; set; }
        public int? TermInDays { get; set; }
    }
}
