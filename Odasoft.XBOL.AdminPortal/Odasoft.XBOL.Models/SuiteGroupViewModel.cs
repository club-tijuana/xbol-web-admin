using Odasoft.XBOL.Models.ViewModels;

namespace Odasoft.XBOL.Models
{
    public class SuiteGroupViewModel
    {
        public long LevelId { get; set; }
        public string LevelName { get; set; } = "";
        public int TotalOfArmchairs { get; set; }
        public int TotalOfBenches { get; set; }
        public int TotalOfFoldingBenches { get; set; }
        public int TotalOfSuites { get; set; }
        public List<SuiteViewModel> Suites { get; set; } = [];
    }
}
