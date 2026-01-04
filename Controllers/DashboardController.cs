using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JordanTouristGuide.Data;
using JordanTouristGuide.Models;

namespace JordanTouristGuide.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // User Dashboard - Shows user's bookings
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var bookings = await _context.Bookings
                .Where(b => b.UserId == user.Id || b.Email == user.Email)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.User = user;
            return View(bookings);
        }

        // Admin Dashboard - Overview
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var stats = new AdminDashboardViewModel
            {
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalContacts = await _context.Contacts.CountAsync(),
                TotalProperties = await _context.Properties.CountAsync(),
                TotalExperiences = await _context.Experiences.CountAsync(),
                TotalUsers = await _userManager.Users.CountAsync(),
                RecentBookings = await _context.Bookings
                    .OrderByDescending(b => b.BookingDate)
                    .Take(5)
                    .ToListAsync()
            };
            return View(stats);
        }

        // Properties Management
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Properties()
        {
            var properties = await _context.Properties
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(properties);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddProperty()
        {
            return View(new Property());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                property.CreatedByUserId = user?.Id;
                property.CreatedAt = DateTime.Now;

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property added successfully!";
                return RedirectToAction(nameof(Properties));
            }
            return View(property);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound();
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                _context.Properties.Update(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property updated successfully!";
                return RedirectToAction(nameof(Properties));
            }
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Property deleted successfully!";
            }
            return RedirectToAction(nameof(Properties));
        }

        // Experiences Management
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Experiences()
        {
            var experiences = await _context.Experiences
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            return View(experiences);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddExperience()
        {
            return View(new ExperienceDb());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddExperience(ExperienceDb experience)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                experience.CreatedByUserId = user?.Id;
                experience.CreatedAt = DateTime.Now;

                _context.Experiences.Add(experience);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Experience added successfully!";
                return RedirectToAction(nameof(Experiences));
            }
            return View(experience);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null) return NotFound();
            return View(experience);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExperience(ExperienceDb experience)
        {
            if (ModelState.IsValid)
            {
                _context.Experiences.Update(experience);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Experience updated successfully!";
                return RedirectToAction(nameof(Experiences));
            }
            return View(experience);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience != null)
            {
                _context.Experiences.Remove(experience);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Experience deleted successfully!";
            }
            return RedirectToAction(nameof(Experiences));
        }

        // Manage Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    FullName = user.FullName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    CreatedAt = user.CreatedAt,
                    Roles = roles.ToList()
                });
            }

            return View(userList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["Success"] = $"{user.Email} is no longer an admin.";
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["Success"] = $"{user.Email} is now an admin.";
            }

            return RedirectToAction(nameof(Users));
        }

        // Bookings Management
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking status updated!";
            }
            return RedirectToAction(nameof(Bookings));
        }
    }

    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalContacts { get; set; }
        public int TotalProperties { get; set; }
        public int TotalExperiences { get; set; }
        public int TotalUsers { get; set; }
        public List<Booking> RecentBookings { get; set; } = new();
    }

    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
 