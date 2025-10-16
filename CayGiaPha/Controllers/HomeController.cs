using System.Diagnostics;
using CayGiaPha.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CayGiaPha.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GenealogyDbContext _context;

        public HomeController(ILogger<HomeController> logger, GenealogyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalPeople = await _context.People.CountAsync();
            ViewBag.TotalFamilies = await _context.Families.CountAsync();
            ViewBag.TotalEvents = await _context.Events.CountAsync();

            // Get people data for the tree
            ViewBag.People = await _context.People
                .Include(p => p.Family)
                .Include(p => p.ChildParents)
                    .ThenInclude(cp => cp.Parent)
                .Include(p => p.ParentChildren)
                    .ThenInclude(pc => pc.Child)
                .Include(p => p.Marriages1)
                    .ThenInclude(m => m.Spouse2)
                .Include(p => p.Marriages2)
                    .ThenInclude(m => m.Spouse1)
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
