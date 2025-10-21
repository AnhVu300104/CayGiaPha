using System.Collections.Generic;

namespace CayGiaPha.Models
{
    public class FamilyTreeViewModel
    {
        public string FamilyName { get; set; }
        public List<MarriedCoupleViewModel> Couples { get; set; } = new List<MarriedCoupleViewModel>();
    }

    public class MarriedCoupleViewModel
    {
        public string HusbandName { get; set; }
        public int? HusbandAge { get; set; }
        public string WifeName { get; set; }
        public int? WifeAge { get; set; }
        public List<ChildViewModel> Children { get; set; } = new List<ChildViewModel>();
        public string HusbandGender { get; set; }
        public string WifeGender { get; set; }
    }

    public class ChildViewModel
    {
        public int PersonalID { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string SpouseName { get; set; }
        public int? SpouseAge { get; set; }
        public string SpouseGender { get; set; }
        public List<ChildViewModel> Children { get; set; } = new List<ChildViewModel>();
    }
}
