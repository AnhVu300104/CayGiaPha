using CayGiaPha.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CayGiaPha.Controllers
{
    [Authorize]
    public class TreeController : Controller
    {
        private readonly GenealogyDbContext _context;

        public TreeController(GenealogyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var families = await _context.Families
                .Include(f => f.People)
                    .ThenInclude(p => p.ParentChildren)
                        .ThenInclude(pc => pc.Child)
                .Include(f => f.People)
                    .ThenInclude(p => p.ChildParents)
                        .ThenInclude(cp => cp.Parent)
                .Include(f => f.People)
                    .ThenInclude(p => p.Marriages1)
                        .ThenInclude(m => m.Spouse2)
                .Include(f => f.People)
                    .ThenInclude(p => p.Marriages2)
                        .ThenInclude(m => m.Spouse1)
                .ToListAsync();

            var treeViewModels = new List<FamilyTreeViewModel>();

            foreach (var family in families)
            {
                var familyTree = new FamilyTreeViewModel
                {
                    FamilyName = family.Name
                };

                // Find married couples (people with spouses)
                var marriedPeople = family.People.Where(p => p.Marriages1.Any() || p.Marriages2.Any()).ToList();

                // Group by marriage to avoid duplicates
                var processedMarriages = new HashSet<int>();

                foreach (var person in marriedPeople)
                {
                    var marriages = person.Marriages1.Concat(person.Marriages2).ToList();

                    foreach (var marriage in marriages)
                    {
                        if (processedMarriages.Contains(marriage.MarriageID)) continue;

                        processedMarriages.Add(marriage.MarriageID);

                        var spouse = marriage.Spouse1ID == person.PersonalID ? marriage.Spouse2 : marriage.Spouse1;

                        var coupleViewModel = new MarriedCoupleViewModel();

                        if (person.Gender == "Nam")
                        {
                            coupleViewModel.HusbandName = person.Name;
                            coupleViewModel.HusbandAge = CalculateAge(person.Birthday);
                            coupleViewModel.HusbandGender = person.Gender;
                            coupleViewModel.WifeName = spouse.Name;
                            coupleViewModel.WifeAge = CalculateAge(spouse.Birthday);
                            coupleViewModel.WifeGender = spouse.Gender;
                        }
                        else
                        {
                            coupleViewModel.HusbandName = spouse.Name;
                            coupleViewModel.HusbandAge = CalculateAge(spouse.Birthday);
                            coupleViewModel.HusbandGender = spouse.Gender;
                            coupleViewModel.WifeName = person.Name;
                            coupleViewModel.WifeAge = CalculateAge(person.Birthday);
                            coupleViewModel.WifeGender = person.Gender;
                        }

                        // Get children of the couple from People table (children of either parent for completeness)
                        var coupleChildren = family.People.Where(p =>
                            p.ChildParents.Any(cp => cp.ParentID == person.PersonalID) ||
                            p.ChildParents.Any(cp => cp.ParentID == spouse.PersonalID)
                        ).Distinct().ToList();

                        // Sort children by age descending
                        coupleChildren = coupleChildren.OrderByDescending(c => c.Birthday ?? DateTime.MinValue).ToList();

                        foreach (var child in coupleChildren)
                        {
                            var childViewModel = new ChildViewModel
                            {
                                PersonalID = child.PersonalID,
                                Name = child.Name,
                                Age = CalculateAge(child.Birthday),
                                Gender = child.Gender
                            };

                            // Recursively get children of this child
                            GetChildrenRecursive(child, family.People, childViewModel.Children);

                            coupleViewModel.Children.Add(childViewModel);
                        }

                        familyTree.Couples.Add(coupleViewModel);
                    }
                }

                treeViewModels.Add(familyTree);
            }

            return View(treeViewModels);
        }



        private int? CalculateAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return null;
            var currentYear = DateTime.Now.Year;
            return currentYear - birthDate.Value.Year;
        }

        private void GetChildrenRecursive(People parent, IEnumerable<People> allPeople, List<ChildViewModel> childrenList)
        {
            var children = allPeople.Where(p => p.ChildParents.Any(cp => cp.ParentID == parent.PersonalID)).ToList();

            foreach (var child in children)
            {
                var childViewModel = new ChildViewModel
                {
                    PersonalID = child.PersonalID,
                    Name = child.Name,
                    Age = CalculateAge(child.Birthday),
                    Gender = child.Gender
                };

                // Check if child has a spouse
                var spouse = allPeople.FirstOrDefault(p => p.Marriages1.Any(m => m.Spouse2ID == child.PersonalID) || p.Marriages2.Any(m => m.Spouse1ID == child.PersonalID));
                if (spouse != null)
                {
                    childViewModel.SpouseName = spouse.Name;
                    childViewModel.SpouseAge = CalculateAge(spouse.Birthday);
                    childViewModel.SpouseGender = spouse.Gender;
                }

                // Recursively get children of this child
                GetChildrenRecursive(child, allPeople, childViewModel.Children);

                childrenList.Add(childViewModel);
            }
        }
    }
}
