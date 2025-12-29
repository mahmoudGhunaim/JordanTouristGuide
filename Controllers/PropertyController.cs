using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Models;
using JordanTouristGuide.Data;

namespace JordanTouristGuide.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _context.Properties
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(properties);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> BookNow([FromForm] Booking booking)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    var user = await _userManager.GetUserAsync(User);
                    booking.UserId = user?.Id;
                }

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Your booking request has been submitted successfully! We will contact you soon.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Please fill in all required fields correctly.";
            return RedirectToAction("Detail", new { id = booking.PropertyId });
        }
    }
}
