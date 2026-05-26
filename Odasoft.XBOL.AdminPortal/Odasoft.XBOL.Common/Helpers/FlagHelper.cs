namespace Odasoft.XBOL.Common.Helpers
{
    public static class FlagHelper
    {
        public static string GetFlagImageUrl(string regionCode)
        {
            if (string.IsNullOrWhiteSpace(regionCode))
            {
                return string.Empty;
            }

            return $"https://flagcdn.com/w40/{regionCode.ToLower()}.png";
        }
    }
}
