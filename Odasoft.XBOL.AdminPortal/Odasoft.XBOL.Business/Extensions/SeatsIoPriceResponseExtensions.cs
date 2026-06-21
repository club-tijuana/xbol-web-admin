namespace Odasoft.XBOL.Business
{
    public partial class SeatsIoPriceResponse
    {
        [Newtonsoft.Json.JsonProperty("fees", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<SeatFeeResponse> Fees { get; set; } = [];
    }

    public class SeatFeeResponse
    {
        [Newtonsoft.Json.JsonProperty("feeName")]
        public string FeeName { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonProperty("feeType")]
        public string FeeType { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonProperty("chargeCategory")]
        public string ChargeCategory { get; set; } = "Fee";

        [Newtonsoft.Json.JsonProperty("feeAmount")]
        public decimal FeeAmount { get; set; }
    }
}
