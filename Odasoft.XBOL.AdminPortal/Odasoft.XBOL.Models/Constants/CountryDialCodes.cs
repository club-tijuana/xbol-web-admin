using Odasoft.XBOL.Models.Enums;

namespace Odasoft.XBOL.Models.Constants
{
    public static class CountryDialCodes
    {
        public static readonly Dictionary<Country, string> Map = new()
        {
            { Country.Mexico, "+52" },
            { Country.USA, "+1" },
            { Country.Spain, "+34" },
            { Country.Argentina, "+54" },
            { Country.Colombia, "+57" }
        };
    }
}
