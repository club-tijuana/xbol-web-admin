namespace Odasoft.XBOL.Models.Parameters
{
    public record SuitesFilterParameters : BaseFilterParameters
    {
        public List<string> Levels { get; set; } = [];
    }

    public static class SuitesFilterParametersExtensions
    {
        public static bool HasAnyFilter(this SuitesFilterParameters parameters) =>
            parameters.Levels?.Count > 0;

        public static SuitesFilterParameters Empty => new();
    }
}
