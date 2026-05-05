namespace Odasoft.XBOL.Models.DTO;

public class SeatsIoErrorResponse
{
    public List<SeatsIoErrorDetail>? Errors { get; set; }
    public string? RequestId { get; set; }
}

public class SeatsIoErrorDetail
{
    public string? Code { get; set; }
    public string? Message { get; set; }
}
