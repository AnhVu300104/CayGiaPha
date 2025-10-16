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
    public class PeopleController : Controller
    {
        private readonly GenealogyDbContext _context;

        public PeopleController(GenealogyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var people = from p in _context.People.Include(p => p.Family)
                         select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.Name.Contains(searchString));
            }

            return View(await people.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var person = await _context.People
                .Include(p => p.Family)
                .Include(p => p.ParentChildren)
                .Include(p => p.ChildParents)
                .Include(p => p.Marriages1)
                .Include(p => p.Marriages2)
                .Include(p => p.OrganizedEvents)
                .FirstOrDefaultAsync(m => m.PersonalID == id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FamilyID"] = new SelectList(_context.Families, "FamilyID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("PersonalID,FamilyID,Name,Gender,Birthday,DeathDate,BirthPlace,PhotoUrl,Generation,Email,Phone")] People person)
        {
            if (ModelState.IsValid)
            {
                person.CreatedAt = DateTime.Now;
                person.UpdatedAt = DateTime.Now;
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FamilyID"] = new SelectList(_context.Families, "FamilyID", "Name", person.FamilyID);
            return View(person);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["FamilyID"] = new SelectList(_context.Families, "FamilyID", "Name", person.FamilyID);
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("PersonalID,FamilyID,Name,Gender,Birthday,DeathDate,BirthPlace,PhotoUrl,Generation,Email,Phone,CreatedAt")] People person)
        {
            if (id != person.PersonalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    person.UpdatedAt = DateTime.Now;
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonalID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FamilyID"] = new SelectList(_context.Families, "FamilyID", "Name", person.FamilyID);
            return View(person);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonalID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.PersonalID == id);
        }
    }
}
