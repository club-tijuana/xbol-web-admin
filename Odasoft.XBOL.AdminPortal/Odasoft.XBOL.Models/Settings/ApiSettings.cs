namespace Odasoft.XBOL.Models.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
