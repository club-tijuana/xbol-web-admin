namespace Odasoft.XBOL.Models.DTO;

public class CategoryFilterSeatsIoConfig
{
    public bool Enabled { get; set; }
    public string SortBy { get; set; } = "price";
    public bool MultiSelect { get; set; }
    public bool ZoomOnSelect { get; set; }
}
