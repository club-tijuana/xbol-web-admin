namespace Odasoft.XBOL.Models.Parameters
{
    public record BaseFilterParameters
    {
        public string SearchTerm { get; set; } = "";
        public string SortBy { get; set; } = "";
        public bool Descending { get; set; } = false;
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
