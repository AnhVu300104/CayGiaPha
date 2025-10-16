using CayGiaPha.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CayGiaPha.Controllers
{
    [Authorize]
    public class RelationshipsController : Controller
    {
        private readonly GenealogyDbContext _context;

        public RelationshipsController(GenealogyDbContext context)
        {
            _context = context;
        }

        // GET: Relationships/Index
        public IActionResult Index()
        {
            return View();
        }

        // Parent-Child Relationships
        [Authorize(Roles = "Admin")]
        public IActionResult CreateParentChild()
        {
            ViewData["ParentID"] = new SelectList(_context.People, "PersonalID", "Name");
            ViewData["ChildID"] = new SelectList(_context.People, "PersonalID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateParentChild([Bind("PC_ID,ParentID,ChildID,RelationshipType")] ParentChild parentChild)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parentChild);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "People");
            }
            ViewData["ParentID"] = new SelectList(_context.People, "PersonalID", "Name", parentChild.ParentID);
            ViewData["ChildID"] = new SelectList(_context.People, "PersonalID", "Name", parentChild.ChildID);
            return View(parentChild);
        }

        // Marriages
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMarriage()
        {
            ViewData["Spouse1ID"] = new SelectList(_context.People, "PersonalID", "Name");
            ViewData["Spouse2ID"] = new SelectList(_context.People, "PersonalID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMarriage([Bind("MarriageID,Spouse1ID,Spouse2ID,StartDate,Status")] Marriages marriage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marriage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "People");
            }
            ViewData["Spouse1ID"] = new SelectList(_context.People, "PersonalID", "Name", marriage.Spouse1ID);
            ViewData["Spouse2ID"] = new SelectList(_context.People, "PersonalID", "Name", marriage.Spouse2ID);
            return View(marriage);
        }
    }
}
