using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Data;

namespace JordanTouristGuide.Controllers
{
    public class ExperienceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExperienceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var experiences = await _context.Experiences
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            return View(experiences);
        }
    }
}
