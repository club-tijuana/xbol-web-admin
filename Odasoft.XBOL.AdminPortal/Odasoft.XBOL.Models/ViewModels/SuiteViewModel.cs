namespace Odasoft.XBOL.Models.ViewModels
{
    public record SuiteViewModel
    {
        public long SuiteId { get; set; }
        public string SuiteName { get; set; } = "";
        public int Armchairs { get; set; }
        public int Benches { get; set; }
        public int FoldingBenches { get; set; }
    }
}
