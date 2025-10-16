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
    public class EventsController : Controller
    {
        private readonly GenealogyDbContext _context;

        public EventsController(GenealogyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = _context.Events.Include(e => e.Organizer);
            return View(await events.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.Person)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OrganizerID"] = new SelectList(_context.People, "PersonalID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("EventID,Title,EventType,EventDate,Location,Description,ImageURL,OrganizerID")] Events eventItem)
        {
            if (ModelState.IsValid)
            {
                eventItem.CreatedAt = DateTime.Now;
                _context.Add(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizerID"] = new SelectList(_context.People, "PersonalID", "Name", eventItem.OrganizerID);
            return View(eventItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            ViewData["OrganizerID"] = new SelectList(_context.People, "PersonalID", "Name", eventItem.OrganizerID);
            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("EventID,Title,EventType,EventDate,Location,Description,ImageURL,OrganizerID,CreatedAt")] Events eventItem)
        {
            if (id != eventItem.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.EventID))
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
            ViewData["OrganizerID"] = new SelectList(_context.People, "PersonalID", "Name", eventItem.OrganizerID);
            return View(eventItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null || user.PersonalID == null)
            {
                return NotFound();
            }

            var registration = new EventRegistration
            {
                EventID = eventId,
                PersonalID = user.PersonalID.Value,
                RegisteredAt = DateTime.Now
            };

            _context.Add(registration);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }
}
