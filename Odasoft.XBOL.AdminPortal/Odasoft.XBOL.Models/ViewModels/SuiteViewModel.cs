namespace Odasoft.XBOL.Models.ViewModels
{
    public record SuitesViewModel
    {
        public long Id { get; set; }
        public string Level { get; set; } = "";
        public string Name { get; set; } = "";
        public int Seats { get; set; }
    }
}
