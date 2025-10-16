using CayGiaPha.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
            var people = await _context.People
                .Include(p => p.Family)
                .Include(p => p.ParentChildren)
                    .ThenInclude(pc => pc.Child)
                .Include(p => p.ChildParents)
                    .ThenInclude(cp => cp.Parent)
                .Include(p => p.Marriages1)
                    .ThenInclude(m => m.Spouse2)
                .Include(p => p.Marriages2)
                    .ThenInclude(m => m.Spouse1)
                .ToListAsync();

            return View(people);
        }
    }
}
