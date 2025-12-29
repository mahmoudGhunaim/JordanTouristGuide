using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Data;

namespace JordanTouristGuide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var experiences = await _context.Experiences
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .Take(6)
                .ToListAsync();
            return View(experiences);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
